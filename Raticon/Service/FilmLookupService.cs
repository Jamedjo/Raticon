using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raticon.Model;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Raticon.Service
{
    public class FilmLookupService
    {
        private IHttpService httpService;
        public FilmLookupService(IHttpService httpService = null)
        {
            this.httpService = httpService ?? new HttpService();
        }

        /// <summary>
        /// Returns the imdbId of a movie.
        /// </summary>
        /// <param name="unsanitized_title">Title of the film to search for, such as the folder name or the filename of a trailer</param>
        /// <param name="choiceCallback"></param>
        /// <returns></returns>
        public virtual string Lookup(string unsanitized_title, Func<LookupContext, LookupChoice> choiceCallback)
        {
            return ResultFromSearch(unsanitized_title, choiceCallback);
        }

        public async Task<string> LookupAsync(string unsanitized_title, Func<LookupContext, LookupChoice> choiceCallback)
        {
            return await Task.Factory.StartNew<string>(()=>Lookup(unsanitized_title, choiceCallback));
        }

        protected virtual string ResultFromSearch(string unsanitized_title, Func<LookupContext, LookupChoice> choiceCallback)
        {
            List<LookupResult> results = Search(unsanitized_title);
            LookupChoice choice = choiceCallback(new LookupContext(results, unsanitized_title));
            return choice.Run(newTitleToSearch => ResultFromSearch(newTitleToSearch, choiceCallback));
        }


        public List<LookupResult> Search(string title)
        {
            string clean_title = new TitleCleaner().Clean(title);
            try
            {
                string data = httpService.Get(@"http://www.myapifilms.com/title?limit=10&title=" + clean_title);
                JArray objects = JArray.Parse(data);
                return objects.Select(o => parseOne(o, title)).OrderBy(r => -r.SearchScore).ToList<LookupResult>();
            }
            catch (JsonReaderException)
            {
                return new List<LookupResult>();
            }
            catch (System.Net.WebException)
            {
                return new List<LookupResult>();
            }
        }

        private LookupResult parseOne(JToken o, string unsanitized_title)
        {
            int score = 0;
            score = new MatchScoreService(unsanitized_title).Score(new MatchScoreService.Fields
            {
                Year = (o["year"] ?? "").ToString(),
                Runtime = ((o["runtime"] ?? new JArray()).First ?? "").ToString(),
                PlotLength = (o["plot"] ?? o["simplePlot"] ?? "").ToString().Length
            });
            return new LookupResult
            {
                ImdbId = (o["idIMDB"] ?? "").ToString(),
                Title = (o["title"] ?? "").ToString(),
                Year = (o["year"] ?? "").ToString(),
                Rating = (o["rating"] ?? "").ToString(),
                Poster = (o["urlPoster"] ?? "").ToString(),
                SearchScore = score
            };
        }
    }

    public class CachingFilmLookupService : FilmLookupService
    {
        private string nfoFolder;
        private IFileSystem fileSystem;

        /// <summary>
        /// ImdbId lookup service which first checks its nfoFolder folder for a cached id.
        /// </summary>
        /// <param name="nfoFolder">Path to the folder where the _imdb_.nfo file will be found or cached</param>
        public CachingFilmLookupService(string nfoFolder, IFileSystem fileSystem = null, IHttpService httpService = null) : base(httpService)
        {
            this.fileSystem = fileSystem ?? new FileSystem();
            this.nfoFolder = nfoFolder;
        }

        public override string Lookup(string unsanitized_title, Func<LookupContext, LookupChoice> choiceCallback)
        {
            string cachedId = new ImdbIdCacheService().ReadFromFolder(nfoFolder, fileSystem);
            if (cachedId != null)
            {
                return cachedId;
            }
            else
            {
                return base.Lookup(unsanitized_title, choiceCallback);
            }
        }

        protected override string ResultFromSearch(string unsanitized_title, Func<LookupContext, LookupChoice> choiceCallback)
        {
            string imdbId = base.ResultFromSearch(unsanitized_title, choiceCallback);

            if (!String.IsNullOrWhiteSpace(imdbId))
            {
                new ImdbIdCacheService().CacheInFolder(imdbId, nfoFolder, fileSystem);
            }
            return imdbId;
        }
    }

    public class LookupResult : RatingResult
    {
        public string ImdbId { get; set; }
        public int SearchScore { get; set; }
    }

    public class LookupContext
    {
        public List<LookupResult> Results { get; private set; }
        public string Query { get; private set; }
        public LookupContext(List<LookupResult> results, string query)
        {
            Results = results;
            Query = query;
        }
    }
}
