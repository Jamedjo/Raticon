﻿using Raticon.Model;
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
            Watcher = watcherFactory(path => filmProcessor.Process(FilmToProcess(path)));
        }

        public FilmProcessingWatcher(string watchPath)
            : this(action => new FolderWatcher(action), new IconService())
        {
            Watcher.Watch(watchPath);
        }

        protected virtual T FilmToProcess(string path)
        {
            return FilmFactory<T>.BuildFilm(path);
        }
    }

    public class GuiFilmProcessingWatcher : FilmProcessingWatcher<GuiFilm>
    {
        IResultPicker resultPicker;
        public GuiFilmProcessingWatcher(string watchPath) : base(watchPath)
        {
            resultPicker = new GuiResultPickerService(Application.Current.MainWindow);
        }

        protected override GuiFilm FilmToProcess(string path)
        {
            MessageBox.Show("Detected change: " + path);
            return new GuiFilm(path, null, resultPicker);
        }
    }

    public class ConsoleFilmProcessingWatcher : FilmProcessingWatcher<ConsoleFilm>
    {
        public ConsoleFilmProcessingWatcher(string watchPath) : base(watchPath) { }

        protected override ConsoleFilm FilmToProcess(string path)
        {
            Console.WriteLine("Detected change: " + path);
            return base.FilmToProcess(path);
        }

        public void InfiniteWait()
        {
            while (true) ((FolderWatcher)Watcher).WaitForChange();
        }

    }
}
