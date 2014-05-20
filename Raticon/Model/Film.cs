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
    public interface IFilm
    {
        string ImdbId { get; }
        string Rating { get; }
        string Title { get; }
        string Year { get; }
        string Poster { get; }
    }
    public interface IFilmFromFolder : IFilm
    {
        string Path { get; }
        string FolderName { get; }
        string PathTo(string fileName);
    }
    public abstract class AbstractFilm : IFilm
    {
        public string ImdbId { get; protected set; }
        public string Rating { get; protected set; }
        public string Title { get; protected set; }
        public string Year { get; protected set; }
        public string Poster { get; protected set; }
    }
    public abstract class AbstractFilmFromFolder : AbstractFilm, IFilmFromFolder
    {
        public string Path { get; protected set; }
        public string FolderName { get; protected set; }


        public string PathTo(string fileName)
        {
            return System.IO.Path.Combine(Path, fileName);
        }
    }
    public class Film : IFilm
    {
        protected string imdbIdCache;
        public string ImdbId
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
            throw new NotImplementedException();
            //imdbIdCache = idLookupService.Lookup(FolderName, (l) => resultPicker.Pick(l));
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

        public string Rating
        {
            get { return getResult("", r => r.Rating); }
        }
        public string Title
        {
            get { return getResult("", r => r.Title); }
        }
        public string Year
        {
            get { return getResult("", r => r.Year); }
        }
        public string Poster
        {
            get { return getResult("", r => r.Poster); }
        }

        protected IFileSystem fileSystem;
        protected IRatingService ratingService;
        protected IResultPicker resultPicker;
        protected FilmLookupService idLookupService;

        public Film(IFileSystem fileSystem = null, IRatingService ratingService = null, IResultPicker resultPicker = null)
        {
            this.fileSystem = fileSystem ?? new FileSystem();
            this.ratingService = ratingService ?? new RatingService();
            this.resultPicker = resultPicker ?? new FirstResultPicker();

            this.idLookupService = new FilmLookupService();

        }
    }

    public class FilmFromFolder : Film, IFilmFromFolder
    {
        public virtual string Path { get; protected set; }
        public virtual string FolderName { get; protected set; }

        public FilmFromFolder(string path, IFileSystem fileSystem, IRatingService ratingService = null, IResultPicker resultPicker = null) : base(fileSystem, ratingService, resultPicker)
        {
            this.Path = path;
            FolderName = fileSystem.Path.GetFileName(path);
        }

        public string PathTo(string fileName)
        {
            return System.IO.Path.Combine(Path, fileName);
        }

        protected override void setImdbFromService()
        {
            imdbIdCache = idLookupService.Lookup(FolderName, (l) => resultPicker.Pick(l));
        }
    }

    public class CachedFilm : FilmFromFolder
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
                    imdbIdCache = idLookupService.Lookup(FolderName, (lookup) => resultPicker.Pick(lookup));
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
