using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raticon.Model;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

namespace Raticon.Service
{
    public class FilmLookupService
    {
        /// <summary>
        /// Returns the imdbId of a movie. First checks its folder for a cached id.
        /// </summary>
        /// <param name="unsanitized_title">Title of the film to search for, such as the folder name or the filename of a trailer</param>
        /// <param name="nfoFolder">Path to the folder where the _imdb_.nfo file will be found or cached</param>
        /// <param name="choiceCallback"></param>
        /// <returns></returns>
        public string Lookup(string unsanitized_title, string nfoFolder, Func<List<LookupResult>, LookupChoice> choiceCallback, IFileSystem fileSystem = null, IHttpService httpService = null)
        {
            fileSystem = fileSystem ?? new FileSystem();
            string cachedId = new ImdbIdCacheService().ReadFromFolder(nfoFolder, fileSystem);
            if (cachedId != null)
            {
                return cachedId;
            }
            else
            {
                return ResultFromSearch(unsanitized_title, nfoFolder, choiceCallback, fileSystem, httpService);
            }
        }

        private string ResultFromSearch(string unsanitized_title, string nfoFolder, Func<List<LookupResult>, LookupChoice> choiceCallback, IFileSystem fileSystem, IHttpService httpService)
        {

            List<LookupResult> results = Search(unsanitized_title, httpService);

            //lookupchoice should be an imdbId string, or some other constent string
            LookupChoice choice = choiceCallback(results);

            string imdbId = choice.Run();
            if (!String.IsNullOrWhiteSpace(imdbId))
            {
                new ImdbIdCacheService().CacheInFolder(imdbId, nfoFolder, fileSystem);
            }
            return imdbId;
        }


        public List<LookupResult> Search(string title, IHttpService httpService = null)
        {
            string clean_title = new TitleCleaner().Clean(title);
            string data = (httpService ?? new HttpService()).Get(@"http://www.myapifilms.com/title?limit=10&title=" + clean_title);
            try
            {
                JArray objects = JArray.Parse(data);
                return objects.Select(o => parseOne(o)).ToList<LookupResult>();
            }
            catch(JsonReaderException)
            {
                return new List<LookupResult>();
            }
        }

        private LookupResult parseOne(JToken o)
        {
            return new LookupResult
            {
                ImdbId = (o["idIMDB"] ?? "").ToString(),
                Title = (o["title"] ?? "").ToString(),
                Year = (o["year"] ?? "").ToString(),
                Rating = (o["rating"] ?? "").ToString(),
                Poster = (o["urlPoster"] ?? "").ToString()
            };
        }
    }

    public class LookupResult : RatingResult
    {
        public string ImdbId { get; set; }
    }

    public class LookupChoice
    {
        public enum Action { Retry, MoreResults, GiveUp }

        private string imdbId;
        private LookupChoice.Action action;

        public LookupChoice(LookupResult choice)
        {
            this.imdbId = choice.ImdbId;
        }
        public LookupChoice(string imdbId)
        {
            this.imdbId = imdbId;
        }

        public LookupChoice(LookupChoice.Action action)
        {
            this.action = action;
        }

        public string Run()
        {
            if (!String.IsNullOrWhiteSpace(imdbId))
            {
                return imdbId;
            }
            else
            {
                switch (action)
                {
                    case LookupChoice.Action.MoreResults:
                        throw new NotImplementedException();
                    //return "";
                    case LookupChoice.Action.Retry:
                        throw new NotImplementedException();
                    //return "";
                    case LookupChoice.Action.GiveUp:
                    default:
                        return null;
                }
            }
        }
    }
}
