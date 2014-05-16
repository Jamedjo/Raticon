using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Abstractions;
using System.Text.RegularExpressions;
using Raticon.Service;
using System.Windows;

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
        private string imdbIdCache;
        public override string ImdbId
        {
            get
            {
                if (imdbIdCache == null)
                {
                    imdbIdCache = ImdbIdFromNfo();
                }
                return imdbIdCache;
            }
        }

        private RatingResult ratingResultCache;
        private T getResult<T>(T default_value, Func<RatingResult,T> getProperty)
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

        private IFileSystem fileSystem;
        private IRatingService ratingService;

        public Film(string path, IFileSystem fileSystem=null, IRatingService ratingService=null)
        {
            if (fileSystem == null) fileSystem = new FileSystem();
            this.fileSystem = fileSystem;
            if (ratingService == null) ratingService = new DiskCachedRatingService();
            this.ratingService = ratingService;

            Path = path;
            
            FolderName = fileSystem.Path.GetFileName(path);
        }

        private string ImdbIdFromNfo()
        {
            try
            {
                string nfo_file = fileSystem.Directory.GetFiles(Path, "*imdb*.nfo").First();
                string imdb_line = fileSystem.File.ReadAllLines(nfo_file).First();
                return Regex.Match(imdb_line, @"/(tt\d+)", RegexOptions.IgnoreCase).Groups[1].Value;
            }
            catch(InvalidOperationException)
            {
                return null;
            }
        }
    }
}
