using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Raticon.Model;

namespace Raticon.Service
{
    public abstract class IRatingService
    {
        public abstract RatingResult getRating(string imdbId);
    }

    public class RatingService : IRatingService
    {
        public override RatingResult getRating(string imdbId)
        {
            return getRating(imdbId, new HttpService());
        }

        public RatingResult getRating(string imdbId, IHttpService httpService)
        {
            string data = httpService.get(@"http://www.omdbapi.com/?i="+imdbId);
            JObject o = JObject.Parse(data);

            return new RatingResult
            {
                Title = o["Title"].ToString(),
                Year = o["Year"].ToString(),
                Rating = o["imdbRating"].ToString()
            };
        }
    }
}
