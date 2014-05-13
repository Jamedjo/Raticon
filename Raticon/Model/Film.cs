using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Abstractions;
using System.Text.RegularExpressions;
using Raticon.Service;

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
        public override string Rating
        {
            get
            {
                if (ratingResultCache == null)
                {
                    ratingResultCache = ratingService.getRating(ImdbId);
                }
                return ratingResultCache.Rating;
            }
        }


        private IFileSystem fileSystem;
        private IRatingService ratingService;

        public Film(string path, IFileSystem fileSystem=null, IRatingService ratingService=null)
        {
            if (fileSystem == null) fileSystem = new FileSystem();
            this.fileSystem = fileSystem;
            if (ratingService == null) ratingService = new RatingService();
            this.ratingService = ratingService;

            Path = path;
            
            FolderName = fileSystem.Path.GetFileName(path);
        }

        private string ImdbIdFromNfo()
        {
            string nfo_file = fileSystem.Directory.GetFiles(Path, "*imdb*.nfo").First();
            string imdb_line =  fileSystem.File.ReadAllLines(nfo_file).First();
            return Regex.Match(imdb_line,@"/(tt\d+)",RegexOptions.IgnoreCase).Groups[1].Value;
        }
    }
}
