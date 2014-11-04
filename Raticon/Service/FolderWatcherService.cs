using Raticon.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Raticon.Service
{
    //enum WatchAction
    //{
    //    Process<GuiFilm>
    //    Process<ConsoleFilm>
    //    Alert(action)

    //Create Gui/Console Film
    //GetImdb
    //onComplete GetRating
    //onComplete IconService.Process(film)

    //}

    public interface IFolderWatcherService
    {
        void Watch(string path);
        void Stop();
    }

    public class FolderWatcherService : IFolderWatcherService
    {
        FileSystemWatcher watcher;
        Action<string> onCreate;

        public FolderWatcherService(Action<string> onCreate)
        {
            this.onCreate = onCreate;
            watcher = new FileSystemWatcher()
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.DirectoryName
            };
            watcher.Created += new FileSystemEventHandler(OnCreatedEvent);
        }

        private void OnCreatedEvent(object source, FileSystemEventArgs e)
        {
            onCreate(e.FullPath);
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

    public class IconMakingFilmFolderWatcher
    {
        public IFolderWatcherService watcher;
        public IconMakingFilmFolderWatcher(Func<string, IFilmFromFolder> filmFactory, Func<Action<string>, IFolderWatcherService> watcherFactory, IFilmProcessor filmProcessor)
        {
            watcher = watcherFactory(path => filmProcessor.Process(filmFactory(path)));
        }

        public IconMakingFilmFolderWatcher(Func<string, IFilmFromFolder> filmFactory)
            : this(filmFactory, action => new FolderWatcherService(action), new IconService())
        {
        }
    }

    public class GuiFilmFolderWatcher : IconMakingFilmFolderWatcher
    {
        public GuiFilmFolderWatcher() : base(path => new GuiFilm(path)) { }
    }

    public class ConsoleFilmFolderWatcher : IconMakingFilmFolderWatcher
    {
        public ConsoleFilmFolderWatcher() : base(path => new ConsoleFilm(path)) { }
    }
}
