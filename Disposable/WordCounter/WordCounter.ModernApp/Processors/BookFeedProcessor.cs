using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WordCounter.ModernApp.Data;
using WordCounter.ModernApp.Data.Model;
using WordCounter.ModernApp.IO;

namespace WordCounter.ModernApp.Processors
{
    public class BookFeedProcessor
    {
        private readonly BookFeedContext _context;
        private readonly FileArchiver _fileArchiver;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;
        private readonly string _archivePath;

        public BookFeedProcessor(BookFeedContext context, FileArchiver fileArchiver, IServiceScopeFactory scopeFactory, IConfiguration config, ILogger<BookFeedProcessor> logger)
        {
            _context = context;
            _fileArchiver = fileArchiver;
            _scopeFactory = scopeFactory;
            _logger = logger;

            _archivePath = config["WordCounter:Paths:Archive"];
        }

        public FileArchiver FileArchiver
        {
            get => default;
            set
            {
            }
        }

        public BookFeedContext BookFeedContext
        {
            get => default;
            set
            {
            }
        }

        public async Task ProcessFile(string path)
        {
            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation("Processing file: " + path);

            var archivePath = Path.Combine(_archivePath, $"{Guid.NewGuid()}.txt");
            _logger.LogInformation("Archiving to: " + archivePath);
            _fileArchiver.CopyFile(path, archivePath);

            _logger.LogInformation("Initialising db results");
            _context.BookFeeds.Add(new BookFeed
            {
                Path = path,
                LineCount = 0,
                WordCount = 0,
                ProcessingMilliseconds = 0
            });
            await _context.SaveChangesAsync();

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var apiTasks = new List<Task<int>>();

                var lines = File.ReadAllLines(path);
                for (var i = 0; i < lines.Length; i++)
                {
                    var lineNumber = i;
                    var line = lines[i];
                    apiTasks.Add(Task.Run(async () =>
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var processor = scope.ServiceProvider.GetRequiredService<BookLineProcessor>();
                            return await processor.GetWordCount(path, lineNumber, line, cancellationTokenSource);
                        }
                    }));
                }
                try
                {
                    Task.WaitAll(apiTasks.ToArray(), cancellationTokenSource.Token);
                    var wordCount = apiTasks.Sum(x => x.Result);

                    _logger.LogInformation("Saving results to db");
                    var bookFeed = _context.BookFeeds.First(x => x.Path == path);
                    bookFeed.LineCount = lines.Length;
                    bookFeed.WordCount = wordCount;
                    bookFeed.ProcessingMilliseconds = stopwatch.ElapsedMilliseconds;
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Complete, took: {0}ms", stopwatch.ElapsedMilliseconds);
                    File.Delete(path);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Feed errored: " + ex.Message);
                }
                finally
                {
                    // Tasks are disposable but they're managed by the runtime -
                    // typically you don't dispose them unless you manage the lifetime

                    apiTasks.ForEach(x =>
                    {
                        x.Dispose();
                        x = null;
                    });
                    apiTasks.Clear();
                }
            }
        }   
    }
}
