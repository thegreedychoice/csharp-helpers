using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using WordCounter.ModernApp.Processors;

namespace WordCounter.ModernApp.IO
{
    public class FolderWatcher : IDisposable
    {
        private readonly BookFeedProcessor _processor;
        private readonly ILogger _logger;
        private FileSystemWatcher _watcher;
        private bool disposedValue;

        public FolderWatcher(BookFeedProcessor processor, ILogger<FolderWatcher> logger)
        {
            _processor = processor;
            _logger = logger;
        }

        public BookFeedProcessor BookFeedProcessor
        {
            get => default;
            set
            {
            }
        }

        public void Start(string path, string filter)
        {
            var fullPath = Path.GetFullPath(path);
            _watcher = new FileSystemWatcher(fullPath, filter);
            _watcher.Created += async (x, y) =>
            {
                //HACK - let the file write finish:
                Thread.Sleep(1000);
                Console.WriteLine("New file created: " + y.Name);
                await _processor.ProcessFile(y.FullPath);
            };
            _watcher.EnableRaisingEvents = true;
            _logger.LogInformation($"Listening on {path}");
        }

        public void Stop()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && _watcher != null)
                {
                    _watcher.Dispose();
                    _watcher = null;
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}