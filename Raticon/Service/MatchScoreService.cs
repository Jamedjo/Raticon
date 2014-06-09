using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Raticon.Service
{
    public class MatchScoreService
    {
        private string input_year;
        public MatchScoreService(string unsanitized_title)
        {
            try
            {
                input_year = Regex.Match(unsanitized_title, @"\d{4}").Value;
            }
            catch (Exception) { }
        }

        public int Score(MatchScoreService.Fields fields)
        {
            return RuntimeScore(fields.Runtime) + PlotScore(fields.PlotLength) + YearScore(fields.Year);
        }

        private int YearScore(string result_year)
        {
            if(String.IsNullOrWhiteSpace(input_year) || String.IsNullOrWhiteSpace(result_year) )
            {
                return 0;
            }
            return result_year.Contains(input_year) ? 100 : 0;
        }

        private int RuntimeScore(string runtime)
        {
            int mins = 0;
            try
            {
                mins = Int32.Parse(Regex.Match(runtime, @"\d+").Value);
            }
            catch (Exception) { }
            return LinearFade(mins, 50, 180);
        }

        private int PlotScore(int plotLength)
        {
            return LinearFade(plotLength, 300, 1000);
        }

        private int LinearFade(int input, int start, int end, int startout = 0, int endout = 100)
        {
            double m = (startout - endout) / (double)(start - end);
            double c = startout - (start * m);
            int max = Math.Max(startout, endout);
            int min = Math.Min(startout, endout);
            return (int)Math.Max(min, Math.Min(max, m * input + c));
        }

        public class Fields
        {
            public string Runtime;
            public string Year;
            public int PlotLength;
        }
    }
}
