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
    }

    public class WaitingFolderWatcher : FolderWatcher
    {
        public WaitingFolderWatcher(Action<string> onCreate) : base(onCreate) { }

        public void WaitForChange()
        {
            watcher.WaitForChanged(WatcherChangeTypes.All);
        }
    }

    public class IconMakingFilmFolderWatcher
    {
        public IFolderWatcher Watcher { get; private set; }

        public IconMakingFilmFolderWatcher(Func<string, IFilmFromFolder> filmFactory, Func<Action<string>, IFolderWatcher> watcherFactory, IFilmProcessor filmProcessor)
        {
            Watcher = watcherFactory(path => filmProcessor.Process(filmFactory(path)));
        }

        public IconMakingFilmFolderWatcher(Func<string, IFilmFromFolder> filmFactory, Func<Action<string>, IFolderWatcher> watcherFactory)
            : this(filmFactory, watcherFactory, new IconService())
        {
        }

        public IconMakingFilmFolderWatcher(Func<string, IFilmFromFolder> filmFactory)
            : this(filmFactory, action => new FolderWatcher(action))
        {
        }
    }

    public class GuiFilmFolderWatcher : IconMakingFilmFolderWatcher
    {
        public GuiFilmFolderWatcher() : base(path => new GuiFilm(path)) { }
    }

    public class ConsoleFilmFolderWatcher : IconMakingFilmFolderWatcher
    {

        public ConsoleFilmFolderWatcher(string watchPath) : base(path => FilmFactory(path) , action => new WaitingFolderWatcher(action))
        {
            Watcher.Watch(watchPath);
        }

        private static IFilmFromFolder FilmFactory(string path)
        {
            Console.WriteLine("Detected change: " + path);
            return new ConsoleFilm(path);
        }

        public void InfiniteWait()
        {
            while (true) ((WaitingFolderWatcher)Watcher).WaitForChange();
        }

    }
}
