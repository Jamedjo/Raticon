using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Service;
using System.Linq;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Collections.Generic;

namespace RaticonTest
{
    [TestClass]
    public class FilmLookupServiceTest
    {
        [TestMethod]
        public void Search_returns_multiple_results_for_ambiguous_titles()
        {
            var results = new FilmLookupService().Search("Italian Job");
            Assert.IsTrue(results.Count >= 2);
        }

        [TestMethod]
        public void Search_gets_a_correct_imdb_id()
        {
            var results = new FilmLookupService().Search("Italian Job");
            var ids = results.Select(r => r.ImdbId).ToList();
            CollectionAssert.Contains(ids, "tt0064505");
        }

        [TestMethod]
        public void Search_filters_titles_before_using_them()
        {
            var results = new FilmLookupService().Search("Italian.Job.1969.Directors.Cut");
            Assert.IsTrue(results.Count >= 1);
        }

        [TestMethod]
        public void Search_returns_an_empty_list_when_no_results_found()
        {
            //"This is a movie which doesn't exist and never will unless someone reads this an makes one to prove a point"
            var results = new FilmLookupService().Search("32498238409");
            Assert.IsTrue(results.Count == 0);
        }

        [TestMethod]
        public void FilmLookup_doesnt_use_http_if_nfo_is_present()
        {
            IFileSystem mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                {@"C:\Trailers\Italian Job\Italian.Job_imdb_.nfo", new MockFileData(@"http://www.imdb.com/title/tt0064505/")}
            });

            //AssertDontCallHttpService
            new FilmLookupService().Lookup("Italian Job", @"C:\Trailers\Italian Job", null, mockFileSystem, new AssertDontCallHttpService());

        }

        [TestMethod]
        public void FilmLookup_caches_result_after_search()
        {
            IFileSystem mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                {@"C:\Trailers\Italian Job\", new MockDirectoryData()}
            });
            new FilmLookupService().Lookup("Italian Job", @"C:\Trailers\Italian Job", (rs) => new LookupChoice(rs.First()), mockFileSystem, new MockHttpService("[{'idIMDB':'tt0064505'}]"));
            Assert.IsTrue(mockFileSystem.File.Exists(@"C:\Trailers\Italian Job\Italian.Job_imdb_.nfo"));
        }

        [TestMethod]
        public void FilmLookup_returns_null_if_callback_tells_it_to_give_up()
        {
            IFileSystem mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                {@"C:\Trailers\Italian Job\", new MockDirectoryData()}
            });
            string imdbId = new FilmLookupService().Lookup("Italian Job", @"C:\Trailers\Italian Job", (rs) => new LookupChoice(LookupChoice.Action.GiveUp), mockFileSystem, new MockHttpService("[{'idIMDB':'tt0064505'}]"));
            Assert.IsNull(imdbId);
        }

        //The callback should be able to pick a result index
        //The callback should be able to ask for 10 more results
        //The callback should be able to ask it to search for a different title
    }
}
