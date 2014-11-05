using GalaSoft.MvvmLight.Messaging;
using Raticon.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Raticon.Model
{
    public class GuiFilm : CachedFilm, INotifyPropertyChanged
    {
        public GuiFilm(string path, IFileSystem fileSystem = null) : this(path, fileSystem, new GuiResultPickerService(Application.Current.MainWindow)) { }

        public GuiFilm(string path, IFileSystem fileSystem, IResultPicker resultPicker, IFilmProcessor autoProcessor = null) : base(path, fileSystem, resultPicker)
        {
            if (autoProcessor != null)
            {
                FetchData(() => autoProcessor.Process(this));
            }
            else
            {
                FetchData();
            }
        }

        public bool IsLoading { get { return (idLookupInvoked && !idLookupComplete) || (ratingLookupInvoked && !ratingLookupComplete); } }

        private bool ratingLookupInvoked = false;
        private bool ratingLookupComplete = false;
        protected override void updateRatingResultCache(Action onComplete)
        {
            if (!ratingLookupInvoked)
            {
                ratingLookupInvoked = true;
                Messenger.Default.Send(this, "FilmLoadingChanged");

                Task.Factory.StartNew<RatingResult>(() => getRatingSafe()).ContinueWith((t) =>
                {
                    ratingResultCache = t.Result;
                    OnRatingChanged();
                    ratingLookupComplete = true;
                    onComplete();
                });
            }
        }

        public string Icon
        {
            get
            {
                string iconPath = PathTo("folder.ico");
                return (fileSystem.File.Exists(iconPath)) ? iconPath : null;
            }
        }
        public void IconUpdated() { OnPropertyChanged("Icon"); }

        private bool idLookupInvoked = false;
        private bool idLookupComplete = false;
        protected override void setImdbFromService(Action onComplete)
        {
            if (!idLookupInvoked)
            {
                idLookupInvoked = true;
                Messenger.Default.Send(this, "FilmLoadingChanged");
                var task = idLookupService.LookupAsync(FolderName, (lookup) => resultPicker.Pick(lookup));
                task.ContinueWith((t) =>
                {
                    imdbIdCache = t.Result;
                    OnRatingChanged();
                    idLookupComplete = true;
                    onComplete();
                });
            }
        }

        private void OnRatingChanged()
        {
            Messenger.Default.Send(this, "FilmLoadingChanged");
            OnPropertyChanged("IsLoading");
            OnPropertyChanged("Title");
            OnPropertyChanged("Rating");
            OnPropertyChanged("Year");
            OnPropertyChanged("Poster");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
