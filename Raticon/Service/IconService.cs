using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raticon.Model;
using System.Windows;
using Raticon.ViewModel;
using System.ComponentModel;
using Raticon.Control;
using System.Drawing;
using Raticon.Utils;
using System.Threading.Tasks;

namespace Raticon.Service
{
    public interface IFilmProcessor
    {
        void Process(IFilmFromFolder film);
        void ProcessCollection(IEnumerable<IFilmFromFolder> films);
    }

    public abstract class AbstractFilmProcessor : IFilmProcessor
    {
        public abstract void Process(IFilmFromFolder film);
        public virtual void ProcessCollection(IEnumerable<IFilmFromFolder> films)
        {
            ProcessValidFilms(films, f => { });
        }

        public void ProcessValidFilms(IEnumerable<IFilmFromFolder> films, Action<IEnumerable<IFilmFromFolder>> onComplete)
        {
            var validFilms = ValidFilms(films);

            foreach (IFilmFromFolder film in validFilms)
            {
                Process(film);
            }

            onComplete(validFilms);
        }

        protected IEnumerable<IFilmFromFolder> ValidFilms(IEnumerable<IFilmFromFolder> films)
        {
            return films.Where(f => !string.IsNullOrWhiteSpace(f.Rating));
        }
    }

    public class IconService : AbstractFilmProcessor
    {
        public override void ProcessCollection(IEnumerable<IFilmFromFolder> films)
        {
            ProcessValidFilms(films,processedFilms=>
                MessageBox.Show("Complete!\n\n" + processedFilms.Count() + " folders have been processed and icons added.", "Complete!", MessageBoxButton.OK, MessageBoxImage.Information));
        }

        public override void Process(IFilmFromFolder film)
        {
            if (System.IO.File.Exists(film.PathTo("folder.ico")) || String.IsNullOrWhiteSpace(film.Rating)) { return; }
            BuildFolderIco(film);
            SetupFolderIcon(film.Path);
        }

        private void SetupFolderIcon(string path)
        {
            new EmbeddedResourceService().ExtractTo("Raticon.Resources.desktop.ini", path + @"\desktop.ini");
            System.IO.File.SetAttributes(path + @"\desktop.ini", System.IO.FileAttributes.Hidden);

            //Folder needs to be read only for icon to show
            System.IO.File.SetAttributes(path, System.IO.FileAttributes.ReadOnly);
        }

        private void BuildFolderIco(IFilmFromFolder film)
        {
            new PosterService().Download(film.Poster, film.PathTo("folder.jpg"), (url, path) =>
                MessageBox.Show("Couldn't download folder.jpg for '" + film.Title + "' from url '" + film.Poster + "' to '" + film.PathTo("folder.jpg") + "'", "Error downloading folder.jpg", MessageBoxButton.OK, MessageBoxImage.Error));

            if(!System.IO.File.Exists(film.PathTo("folder.jpg")))
            {
                return;
            }

            Task<Bitmap> task = StaTask.Start<Bitmap>(() => new IconLayout(new IconLayoutViewModel(film.PathTo("folder.jpg"), film.Rating)).RenderToBitmap());
            task.Wait();
            Bitmap icon = task.Result;
            new PngToIcoService().Convert(icon, film.PathTo("folder.ico"));
        }
    }

    public class GuiIconService : IconService
    {
        private Window parentWindow;

        private IconProgressViewModel viewModel;
        private IEnumerable<IFilmFromFolder> films;

        public GuiIconService(Window parentWindow)
        {
            this.parentWindow = parentWindow;
        }

        private void BeginBackgroundProcess()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            backgroundWorker.DoWork += (s, e) => BackgroundProcess(s, e, backgroundWorker, films);
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerAsync();
        }

        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            viewModel.ProgressPercentage = e.ProgressPercentage;
        }

        private void BackgroundProcess(object sender, DoWorkEventArgs e, BackgroundWorker worker, IEnumerable<IFilmFromFolder> films)
        {
            for (int i = 0; i < films.Count(); i++)
            {
                IFilmFromFolder film = films.ElementAt(i);
                Process(film);
                ((GuiFilm)film).IconUpdated();
                worker.ReportProgress((int)((i + 1) * 100 / (double)films.Count()));
            }
        }

        public override void ProcessCollection(IEnumerable<IFilmFromFolder> films)
        {
            this.viewModel = new IconProgressViewModel();
            this.films = ValidFilms(films);

            IconProgressBox progressBox = new IconProgressBox();
            progressBox.DataContext = viewModel;
            BeginBackgroundProcess();
            progressBox.Owner = parentWindow;
            progressBox.ShowDialog();
        }

    }
}
