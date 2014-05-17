using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Abstractions;
using System.Text.RegularExpressions;
using Raticon.Service;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;

namespace Raticon.Model
{
    public abstract class IFilm
    {
        public virtual string FolderName { get; protected set; }
        public virtual string Path { get; protected set; }
        public virtual string ImdbId { get; protected set; }
        public virtual string Rating { get; protected set; }
        public virtual string Title { get; protected set; }
        public virtual string Year { get; protected set; }
        public virtual string Poster { get; protected set; }

        public void RequireFolderJpg(IFileSystem fileSystem = null, IHttpService httpService = null)
        {
            fileSystem = fileSystem ?? new FileSystem();
            if (!fileSystem.File.Exists(PathTo("folder.jpg")) && Poster != null && Poster.Length > 0)
            {
                try
                {
                    (httpService ?? new HttpService()).GetBinary(Poster, PathTo("folder.jpg"));
                }
                catch
                {
#if DEBUG
                    throw;
#else
                    MessageBox.Show("Couldn't download folder.jpg for '"+Title+"' from url '"+Poster+"' to '"+PathTo("folder.jpg")+"'","Error downloading folder.jpg",MessageBoxButton.OK,MessageBoxImage.Error);
#endif
                }
            }
        }

        public string PathTo(string fileName)
        {
            return System.IO.Path.Combine(Path, fileName);
        }
    }
    public class Film : IFilm
    {
        protected string imdbIdCache;
        public override string ImdbId
        {
            get
            {
                if (imdbIdCache == null)
                {
                    setImdbFromService();
                }
                return imdbIdCache;
            }
        }

        protected virtual void setImdbFromService()
        {
            imdbIdCache = idLookupService.Lookup(FolderName, (rs) => resultPicker.Pick(rs));
        }

        protected RatingResult ratingResultCache;
        protected T getResult<T>(T default_value, Func<RatingResult, T> getProperty)
        {
            if (ratingResultCache == null)
            {
                if (ImdbId == null)
                {
                    return default_value;
                }
                ratingResultCache = ratingService.GetRating(ImdbId);
            }
            return getProperty(ratingResultCache);
        }

        public override string Rating
        {
            get { return getResult("", r => r.Rating); }
        }
        public override string Title
        {
            get { return getResult("", r => r.Title); }
        }
        public override string Year
        {
            get { return getResult("", r => r.Year); }
        }
        public override string Poster
        {
            get { return getResult("", r => r.Poster); }
        }

        protected IFileSystem fileSystem;
        protected IRatingService ratingService;
        protected IResultPicker resultPicker;
        protected FilmLookupService idLookupService;

        public Film(string path, IFileSystem fileSystem = null, IRatingService ratingService = null, IResultPicker resultPicker = null)
        {
            this.fileSystem = fileSystem ?? new FileSystem();
            this.ratingService = ratingService ?? new RatingService();
            this.resultPicker = resultPicker ?? new FirstResultPicker();

            this.idLookupService = new FilmLookupService();

            Path = path;

            FolderName = fileSystem.Path.GetFileName(path);
        }

    }

    public class CachedFilm : Film
    {
        public CachedFilm(string path, IFileSystem fileSystem, IRatingService ratingService, IResultPicker resultPicker = null) : base(path, fileSystem, ratingService, resultPicker)
        {
            this.idLookupService = new CachingFilmLookupService(Path, fileSystem);
        }

        public CachedFilm(string path, IFileSystem fileSystem = null, IResultPicker resultPicker = null) : this(path, fileSystem, new DiskCachedRatingService(), resultPicker)
        {
        }
    }

    public class GuiFilm : CachedFilm, INotifyPropertyChanged
    {
        public GuiFilm(string path, IFileSystem fileSystem = null) : base(path, fileSystem, new GuiResultPickerService(Application.Current.MainWindow))
        {
        }

        private bool lookupInvoked = false;
        protected override void setImdbFromService()
        {
            if(!lookupInvoked)
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => {
                    imdbIdCache = idLookupService.Lookup(FolderName, (rs) => resultPicker.Pick(rs));
                    OnPropertyChanged("Title");
                    OnPropertyChanged("Rating");
                    OnPropertyChanged("Year");
                    OnPropertyChanged("Poster");
                }));
                lookupInvoked = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
