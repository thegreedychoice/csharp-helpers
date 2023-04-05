using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using WordCounter.ModernApp.Data;
using WordCounter.ModernApp.IO;
using WordCounter.ModernApp.Processors;
using WordCounter.ModernApp.Services;

namespace WordCounter.ModernApp
{
    class Program
    {
        private static IConfiguration _Config;
        private static string _InputPath;
        private static string _ArchivePath;
        private static ServiceProvider _ServiceProvider;

        static void Main(string[] args)
        {
            _Config = new ConfigurationBuilder()
                 .AddJsonFile("appSettings.json")
                 .Build();

            // config, logging & HttpClientFactory
            var services = new ServiceCollection()
                .AddSingleton(_Config)
                .AddLogging(builder =>
                {
                    builder.AddConfiguration(_Config.GetSection("Logging"));
                    builder.AddConsole();
                })
                .AddHttpClient();

            services.AddSingleton<FolderWatcher>()
                .AddTransient<FileArchiver>()
                .AddTransient<ApiClient>()
                .AddTransient<BookFeedProcessor>()
                .AddTransient<BookLineProcessor>();

            // DbContext manages connections
            // SqlConnections are disposable but you don't need to dispose DbContext

            services.AddDbContext<BookFeedContext>(
                options => options.UseSqlServer(_Config.GetConnectionString("WordCount")),
                ServiceLifetime.Transient);

            _ServiceProvider = services.BuildServiceProvider();

            _InputPath = _Config["WordCounter:Paths:Input"];
            _ArchivePath = _Config["WordCounter:Paths:Archive"];
            EnsureDirectory(_InputPath, _ArchivePath);

            Console.WriteLine("'s' to Start; 'p' to Stop; 'gc' to Garbage Collect; 'x' to Exit");
            var command = "";
            while (command != "x")
            {
                command = Console.ReadLine();
                switch (command)
                {
                    case "s":
                        Start();
                        break;

                    case "p":
                        Stop();
                        break;

                    case "gc":
                        GC.Collect(generation: 2, GCCollectionMode.Forced, blocking: true, compacting: true);
                        break;
                }
            }
        }

        private static void EnsureDirectory(params string[] paths)
        {
            foreach (var path in paths)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }

        private static void Start()
        {
            var watcher = _ServiceProvider.GetRequiredService<FolderWatcher>();
            watcher.Start(_InputPath, "*.txt");
        }

        private static void Stop()
        {
            var watcher = _ServiceProvider.GetRequiredService<FolderWatcher>();
            watcher.Stop();
        }
    }
}
