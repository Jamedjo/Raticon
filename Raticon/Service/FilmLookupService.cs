using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raticon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raticon.Service
{
    public class FilmLookupService
    {
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
                ImdbId = o["idIMDB"].ToString(),
                Title = o["title"].ToString(),
                Year = o["year"].ToString(),
                Rating = o["rating"].ToString(),
                Poster = o["urlPoster"].ToString()
            };
        }
    }

    public class LookupResult : RatingResult
    {
        public string ImdbId { get; set; }
    }
}
