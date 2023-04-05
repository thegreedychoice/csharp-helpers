using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WordCounter.App.Data;
using WordCounter.App.IO;
using WordCounter.App.Services;

namespace WordCounter.App
{
    class Program
    {
        private static IConfiguration _Config;
        private static string _InputPath;
        private static string _ArchivePath;
        private static readonly FolderWatcher _Watcher = new FolderWatcher();

        static void Main(string[] args)
        {
            _Config = new ConfigurationBuilder()
                 .AddJsonFile("appSettings.json")
                 .Build();

            _InputPath = _Config["WordCounter:Paths:Input"];
            _ArchivePath = _Config["WordCounter:Paths:Archive"];
            EnsureDirectory(_InputPath);
            EnsureDirectory(_ArchivePath);

            Console.WriteLine("'s' to Start; 'gc' to Garbage Collect; 'x' to Exit");
            var command = "";
            while (command != "x")
            {
                command = Console.ReadLine();
                switch (command)
                {
                    case "s":
                        Start();
                        break;

                    case "gc":
                        GC.Collect(generation: 2, GCCollectionMode.Forced, blocking: true, compacting: true);
                        break;
                }
            }
        }

        private static void EnsureDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static void Start()
        {
            _Watcher.Start(_InputPath, "*.txt", ProcessFile);
            Console.WriteLine($"Listening on {_InputPath}");
        }

        private static async Task ProcessFile(string path)
        {
            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Processing file: " + path);
            var archivePath = Path.Combine(_ArchivePath, $"{Guid.NewGuid()}.txt");
            Console.WriteLine("Archiving to: " + archivePath);
            new FileArchiver().CopyFile(path, archivePath);

            Console.WriteLine("Initialising db results");
            var sqlClient = new SqlClient(_Config);
            await sqlClient.CreateFeedResults(path, 0, 0, 0);

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var apiTasks = new List<Task<int>>();
                var apiClient = new ApiClient(_Config);
                var lines = File.ReadAllLines(path);
                for (var i = 0; i < lines.Length; i++)
                {
                    // create tasks using apiClient 
                    var lineNumber = i;
                    var line = lines[i];
                    apiTasks.Add(Task.Run(async () => await GetWordCount(apiClient, path, lineNumber, line, cancellationTokenSource)));
                }
                try
                {
                    Task.WaitAll(apiTasks.ToArray(), cancellationTokenSource.Token);
                    var wordCount = apiTasks.Sum(x => x.Result);

                    Console.WriteLine("Saving results to db");
                    await sqlClient.UpdateFeedResults(path, lines.Length, wordCount, stopwatch.ElapsedMilliseconds);
                    Console.WriteLine("Complete, took: {0}ms", stopwatch.ElapsedMilliseconds);

                    File.Delete(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Feed errored: " + ex.Message);
                }
                finally
                {
                    apiClient.Dispose();
                    apiClient = null;

                    apiTasks.ForEach(x =>
                    {
                        x.Dispose();
                        x = null;
                    });
                    apiTasks.Clear();
                }
            }
        }

        private static async Task<int> GetWordCount(ApiClient apiClient, string path, int lineNumber, string line, CancellationTokenSource cancellationTokenSource)
        {
            var wordCount = 0;
            cancellationTokenSource.Token.ThrowIfCancellationRequested();
            try
            {
                var excerpt = line.Length > 100 ? line.Substring(0, 100) : line;
                Console.WriteLine("Processing line: {0}, '{1}...'", lineNumber, excerpt);
                
                wordCount = await apiClient.GetWordCount(line);

                var sqlClient = new SqlClient(_Config);
                await sqlClient.SaveLineResults(path, lineNumber, wordCount, excerpt);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex}");
                cancellationTokenSource.Cancel();
            }
            return wordCount;
        }
    }        
}
