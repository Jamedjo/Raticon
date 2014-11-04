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

    public class IconMakingFilmFolderWatcher<T> where T : IFilmFromFolder
    {
        public IFolderWatcher Watcher { get; private set; }

        public IconMakingFilmFolderWatcher(Func<Action<string>, IFolderWatcher> watcherFactory, IFilmProcessor filmProcessor)
        {
            Watcher = watcherFactory(path => filmProcessor.Process(FilmToProcess(path)));
        }

        public IconMakingFilmFolderWatcher(string watchPath)
            : this(action => new FolderWatcher(action), new IconService())
        {
            Watcher.Watch(watchPath);
        }

        private T FilmToProcess(string path)
        {
            BeforeProcess(path);
            return FilmFactory<T>.BuildFilm(path);
        }

        protected virtual void BeforeProcess(string path) { }
    }

    public class GuiFilmFolderWatcher : IconMakingFilmFolderWatcher<GuiFilm>
    {
        public GuiFilmFolderWatcher(string watchPath) : base(watchPath) { }
    }

    public class ConsoleFilmFolderWatcher : IconMakingFilmFolderWatcher<ConsoleFilm>
    {
        public ConsoleFilmFolderWatcher(string watchPath) : base(watchPath) { }

        protected override void BeforeProcess(string path)
        {
            Console.WriteLine("Detected change: " + path);
        }

        public void InfiniteWait()
        {
            while (true) ((FolderWatcher)Watcher).WaitForChange();
        }

    }
}
