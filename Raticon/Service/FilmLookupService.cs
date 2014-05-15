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
        public RatingResult[] Search(string title, IHttpService httpService = null)
        {
            string data = (httpService ?? new HttpService()).Get(@"http://www.myapifilms.com/title?limit=10&title=" + title);
            JArray objects = JArray.Parse(data);
            return objects.Select(o => parseOne(o)).ToArray<RatingResult>();
        }

        private RatingResult parseOne(JToken o)
        {
            return new RatingResult
            {
                Title = o["title"].ToString(),
                Year = o["year"].ToString(),
                Rating = o["rating"].ToString(),
                Poster = o["urlPoster"].ToString()
            };
        }

    }
}
