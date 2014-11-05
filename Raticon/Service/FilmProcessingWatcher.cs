using Raticon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Raticon.Service
{
    public class FilmProcessingWatcher<T> where T : IFilmFromFolder
    {
        public IFolderWatcher Watcher { get; private set; }

        public FilmProcessingWatcher(Func<Action<string>, IFolderWatcher> watcherFactory, IFilmProcessor filmProcessor)
        {
            Watcher = watcherFactory(path => OnChangeAction(path, filmProcessor));
        }

        public FilmProcessingWatcher(string watchPath)
            : this(action => new FolderWatcher(action), new IconService())
        {
            Watcher.Watch(watchPath);
        }

        protected virtual void OnChangeAction(string path, IFilmProcessor filmProcessor)
        {
            filmProcessor.Process(FilmFactory<T>.BuildFilm(path));
        }
    }

    public class GuiFilmProcessingWatcher : FilmProcessingWatcher<GuiFilm>
    {
        GuiFilmFactory filmFactory;
        public GuiFilmProcessingWatcher(string watchPath) : base(watchPath)
        {
            filmFactory = new GuiFilmFactory();
        }

        protected override void OnChangeAction(string path, IFilmProcessor filmProcessor)
        {
            MessageBox.Show("Detected change: " + path);
            filmFactory.BuildFilm(path, filmProcessor);
        }
    }

    public class ConsoleFilmProcessingWatcher : FilmProcessingWatcher<ConsoleFilm>
    {
        public ConsoleFilmProcessingWatcher(string watchPath) : base(watchPath) { }

        protected override void OnChangeAction(string path, IFilmProcessor filmProcessor)
        {
            Console.WriteLine("Detected change: " + path);
            base.OnChangeAction(path, filmProcessor);
        }

        public void InfiniteWait()
        {
            while (true) ((FolderWatcher)Watcher).WaitForChange();
        }

    }
}
