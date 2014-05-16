using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Service;
using System.Linq;

namespace RaticonTest
{
    [TestClass]
    public class FilmLookupServiceTest
    {
        [TestMethod]
        public void It_should_return_multiple_results_for_ambiguous_titles()
        {
            var results = new FilmLookupService().Search("Italian Job");
            Assert.IsTrue(results.Count >= 2);
        }

        [TestMethod]
        public void It_should_get_a_correct_imdb_id()
        {
            var results = new FilmLookupService().Search("Italian Job");
            var ids = results.Select(r => r.ImdbId).ToList();
            CollectionAssert.Contains(ids, "tt0064505");
        }

        [TestMethod]
        public void It_should_filter_titles_before_using_them()
        {
            var results = new FilmLookupService().Search("Italian.Job.1969.Directors.Cut");
            Assert.IsTrue(results.Count >= 1);
        }

        [TestMethod]
        public void It_should_and_empty_list_when_no_results_found()
        {
            //"This is a movie which doesn't exist and never will unless someone reads this an makes one to prove a point"
            var results = new FilmLookupService().Search("32498238409");
            Assert.IsTrue(results.Count == 0);
        }
    }
}
