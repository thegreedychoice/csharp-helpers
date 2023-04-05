using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WordCounter.App.IO
{
    public class FolderWatcher
    {
       private FileSystemWatcher _watcher;

        public void Start(string path, string filter, Func<string, Task> onFileCreated)
        {
            var fullPath = Path.GetFullPath(path);
            _watcher = new FileSystemWatcher(fullPath, filter);
            _watcher.Created += async (x, y) =>
            {
                //HACK - let the file write finish:
                Thread.Sleep(1000);
                Console.WriteLine("New file created: " + y.Name);
                await onFileCreated(y.FullPath);
            };
            _watcher.EnableRaisingEvents = true;
        }
    }
}
