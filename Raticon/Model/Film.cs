using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Abstractions;
using System.Text.RegularExpressions;
using Raticon.Service;

namespace Raticon.Model
{
    public class Film
    {
        public string FolderName { get; set; }
        public string Path { get; set; }

        private string imdbIdCache;
        public string ImdbId
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
        public string Rating
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
