using Raticon.Service;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Windows;

namespace Raticon.Model
{
    /// <summary>
    /// Allows GuiFilms to be built from a non STA thread.
    /// </summary>
    public class GuiFilmFactory
    {
        private IResultPicker resultPicker;

        /// <summary>
        /// Allows GuiFilms to be built from a non STA thread.
        /// Usage: Create the factory on the UI thread and then call BuildFilm from anywhere.
        /// </summary>
        public GuiFilmFactory()
        {
            resultPicker = new GuiResultPickerService(Application.Current.MainWindow);
        }

        /// <summary>
        /// Creates a GuiFilm from any thread.
        /// </summary>
        public GuiFilm BuildFilm(string path, IFilmProcessor autoProcessor = null, IFileSystem fileSystem = null)
        {
            return new GuiFilm(path, fileSystem, resultPicker, autoProcessor);
        }
    }
}
