using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Service;

namespace RaticonTest
{
    [TestClass]
    public class MatchScoreServiceTest
    {
        MatchScoreService subject = new MatchScoreService("The Matrix 1999");

        [TestMethod]
        public void MatchScore_uses_low_runtime_to_demote_tv()
        {
            int minScore = subject.Score(new MatchScoreService.Fields { Runtime = "50 min" });
            int maxScore = subject.Score(new MatchScoreService.Fields { Runtime = "180 min" });
            Assert.AreEqual(100, maxScore - minScore);
        }

        [TestMethod]
        public void MatchScore_uses_empty_runtime_to_demote_games()
        {
            int zeroScore = subject.Score(new MatchScoreService.Fields { });
            int halfScore = subject.Score(new MatchScoreService.Fields { Runtime = "115 min" });
            Assert.AreEqual(50, halfScore - zeroScore);
        }

        [TestMethod]
        public void MatchScore_uses_plot_length_as_popularity_heuristic()
        {
            int minScore = subject.Score(new MatchScoreService.Fields { PlotLength = 300 });
            int maxScore = subject.Score(new MatchScoreService.Fields { PlotLength = 1000 });
            Assert.AreEqual(100, maxScore - minScore);
        }

        [TestMethod]
        public void MatchScore_looks_for_year_in_unsanitized_title()
        {
            int minScore = subject.Score(new MatchScoreService.Fields { Year = "2003" });
            int maxScore = subject.Score(new MatchScoreService.Fields { Year = "March 31, 1999" });
            Assert.AreEqual(100, maxScore - minScore);
        }

        [TestMethod]
        public void MatchScore_ignores_year_when_not_included_in_unsanitized_title()
        {
            var matchService = new MatchScoreService("The Matrix");
            int emptyYearScore = matchService.Score(new MatchScoreService.Fields { Year = "" });
            int givenYearScore = matchService.Score(new MatchScoreService.Fields { Year = "2003" });
            Assert.AreEqual(0, emptyYearScore);
            Assert.AreEqual(0, givenYearScore);
        }
    }
}
