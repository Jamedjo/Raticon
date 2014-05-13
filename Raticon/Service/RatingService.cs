using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raticon.Service
{
    public class RatingResult
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Rating { get; set; }
    }
    public abstract class IRatingService
    {
        public abstract RatingResult getRating(string imdbId);
    }
    public class RatingService : IRatingService
    {
        public override RatingResult getRating(string imdbId)
        {
            throw new NotImplementedException();
        }
    }
}
