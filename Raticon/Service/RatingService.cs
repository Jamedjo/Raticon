using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Raticon.Model;
using System.IO.Abstractions;
using Newtonsoft.Json;

namespace Raticon.Service
{
    public abstract class IRatingService
    {
        public abstract RatingResult GetRating(string imdbId);
    }

    public class RatingService : IRatingService
    {
        public override RatingResult GetRating(string imdbId)
        {
            return GetRating(imdbId, new HttpService());
        }

        public virtual RatingResult GetRating(string imdbId, IHttpService httpService)
        {
            return GetRatingFromApi(imdbId, httpService);
        }

        protected RatingResult GetRatingFromApi(string imdbId, IHttpService httpService)
        {
            string data = httpService.Get(@"http://www.omdbapi.com/?i="+imdbId);
            JObject o = JObject.Parse(data);

            return new RatingResult
            {
                Title = o["Title"].ToString(),
                Year = o["Year"].ToString(),
                Rating = o["imdbRating"].ToString(),
                Poster = o["Poster"].ToString()
            };
        }
    }

    public class DiskCachedRatingService : RatingService
    {
        public override RatingResult GetRating(string imdbId, IHttpService httpService)
        {
            return GetRatingWithCache(imdbId, httpService, new FileSystem());
        }

        public RatingResult GetRatingWithCache(string imdbId, IHttpService httpService, IFileSystem fileSystem)
        {
            RatingResult result = AttemptReadFromCache(imdbId, fileSystem);
            if (result == null)
            {
                result = base.GetRatingFromApi(imdbId, httpService);
                WriteResultToCache(imdbId, result, fileSystem);
            }
            return result;
        }

        private RatingResult AttemptReadFromCache(string imdbId, IFileSystem fileSystem)
        {
            string filePath = JsonCacheFilePath(imdbId, fileSystem);
            if(!fileSystem.File.Exists(filePath))
            {
                return null;
            }
            string json = fileSystem.File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<RatingResult>(json);
        }

        private void WriteResultToCache(string imdbId, RatingResult result, IFileSystem fileSystem)
        {
            string json = JsonConvert.SerializeObject(result, Formatting.Indented);
            fileSystem.File.WriteAllText(JsonCacheFilePath(imdbId,fileSystem), json);
        }

        private string JsonCacheFilePath(string imdbId, IFileSystem fileSystem)
        {
            return fileSystem.Path.Combine(fileSystem.Path.Combine(Raticon.Constants.CommonApplicationDataPath, "Cache"), imdbId + ".json");
        }
    }
}
