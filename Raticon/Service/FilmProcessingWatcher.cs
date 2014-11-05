using Raticon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raticon.Service
{
    public class FilmProcessingWatcher<T> where T : IFilmFromFolder
    {
        public IFolderWatcher Watcher { get; private set; }

        public FilmProcessingWatcher(Func<Action<string>, IFolderWatcher> watcherFactory, IFilmProcessor filmProcessor)
        {
            Watcher = watcherFactory(path => filmProcessor.Process(FilmToProcess(path)));
        }

        public FilmProcessingWatcher(string watchPath)
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

    public class GuiFilmProcessingWatcher : FilmProcessingWatcher<GuiFilm>
    {
        public GuiFilmProcessingWatcher(string watchPath) : base(watchPath) { }

        protected override void BeforeProcess(string path)
        {
            //Message
        }
    }

    public class ConsoleFilmProcessingWatcher : FilmProcessingWatcher<ConsoleFilm>
    {
        public ConsoleFilmProcessingWatcher(string watchPath) : base(watchPath) { }

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
