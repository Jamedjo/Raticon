using Raticon.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Raticon.Service
{
    public interface IFolderWatcher
    {
        void Watch(string path);
        void Stop();
    }

    public class FolderWatcher : IFolderWatcher
    {
        protected FileSystemWatcher watcher;
        Action<string> onChange;

        public FolderWatcher(Action<string> onChange)
        {
            this.onChange = onChange;
            watcher = new FileSystemWatcher()
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.DirectoryName
            };
            watcher.Created += new FileSystemEventHandler(OnCreatedEvent);
            watcher.Renamed += new RenamedEventHandler(OnRenamedEvent);
        }

        private void OnCreatedEvent(object source, FileSystemEventArgs e)
        {
            onChange(e.FullPath);
        }

        private void OnRenamedEvent(object source, RenamedEventArgs e)
        {
            onChange(e.FullPath);
        }

        public void Watch(string path)
        {
            watcher.Path = path;
            watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
        }

        public void WaitForChange()
        {
            watcher.WaitForChanged(WatcherChangeTypes.All);
        }
    }
}
