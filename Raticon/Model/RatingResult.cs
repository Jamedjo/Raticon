using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raticon.Model
{
    public class RatingResult
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Rating { get; set; }

        public override bool Equals(object obj)
        {
            RatingResult rr = obj as RatingResult;
            return Title == rr.Title && Year == rr.Year && Rating == rr.Rating;
        }

        public override string ToString()
        {
            return Title + " - " + Year + " (" + Rating + ")";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
