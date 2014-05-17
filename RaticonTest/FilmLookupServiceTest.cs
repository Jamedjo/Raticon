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
            var results = new FilmLookupService(new MockHttpService(FullMyApiFilmsResponse)).Search("Italian Job");
            Assert.IsTrue(results.Count >= 2);
        }

        [TestMethod]
        public void Search_gets_a_correct_imdb_id()
        {
             var results = new FilmLookupService(new MockHttpService(FullMyApiFilmsResponse)).Search("Italian Job");
            var ids = results.Select(r => r.ImdbId).ToList();
            CollectionAssert.Contains(ids, "tt0064505");
        }

        [TestMethod]
        public void Search_filters_titles_before_using_them()
        {
            var results = new FilmLookupService(new MockHttpService(url => {
                if (url.Contains("Directors"))
                {
                    throw new Exception("Search was not filtered and tried to call url '"+url+"'");
                }
                else
                {
                    return MyApiFilmsResponse;
                }
            })).Search("Italian.Job.1969.Directors.Cut");
        }

        [TestMethod]
        public void Search_returns_an_empty_list_when_no_results_found()
        {
            //"This is a movie which doesn't exist and never will unless someone reads this an makes one to prove a point"
            var results = new FilmLookupService(new MockHttpService("{\"code\":110,\"message\":\"Movie not found\"}")).Search("32498238409");
            Assert.IsTrue(results.Count == 0);
        }

        [TestMethod]
        public void CachingFilmLookup_doesnt_use_http_if_nfo_is_present()
        {
            IFileSystem mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                {@"C:\Trailers\Italian Job\Italian.Job_imdb_.nfo", new MockFileData(@"http://www.imdb.com/title/tt0064505/")}
            });

            //AssertDontCallHttpService
            new CachingFilmLookupService(@"C:\Trailers\Italian Job", mockFileSystem, new AssertDontCallHttpService()).Lookup("Italian Job", null);

        }

        IFileSystem mockFilmFolder;
        string folderPath = @"C:\Trailers\Italian Job";
        string filmName = "Italian Job";
        string MyApiFilmsResponse = "[{'idIMDB':'tt0064505'}]";
        string FullMyApiFilmsResponse = "[{\"idIMDB\":\"tt0391247\",\"rating\":\"6.7\",\"title\":\"The Italian Job\",\"urlIMDB\":\"http://www.imdb.com/title/tt0391247\",\"year\":\"2003\"}, {\"idIMDB\":\"tt0317740\",\"rating\":\"7.0\",\"title\":\"The Italian Job\",\"urlIMDB\":\"http://www.imdb.com/title/tt0317740\",\"year\":\"2003\"}, {\"idIMDB\":\"tt0064505\",\"rating\":\"7.4\",\"title\":\"The Italian Job\",\"urlIMDB\":\"http://www.imdb.com/title/tt0064505\",\"year\":\"1969\"}]";
        string MyApiFilmsSupermanResponse = "[{'idIMDB':'tt0078346'}]";

        [TestInitialize]
        public void FilmLookupServiceTestInitialize()
        {
            mockFilmFolder = new MockFileSystem(new Dictionary<string, MockFileData> {
                {@"C:\Trailers\Italian Job\", new MockDirectoryData()}
            });
        }

        [TestMethod]
        public void CachingFilmLookup_caches_result_after_search()
        {
            new CachingFilmLookupService(folderPath, mockFilmFolder, new MockHttpService(MyApiFilmsResponse)).Lookup(filmName, (rs) => new LookupChoice(rs.First()));
            Assert.IsTrue(mockFilmFolder.File.Exists(@"C:\Trailers\Italian Job\Italian.Job_imdb_.nfo"));
        }

        [TestMethod]
        public void FilmLookup_returns_null_if_callback_tells_it_to_give_up()
        {
            string imdbId = new FilmLookupService(new MockHttpService(MyApiFilmsResponse)).Lookup(filmName, (rs) => new LookupChoice(LookupChoice.Action.GiveUp));
            Assert.IsNull(imdbId);
        }

        [TestMethod]
        public void FilmLookup_returns_id_chosen_by_callback()
        {
            LookupResult pick = null;
            string imdbId = new FilmLookupService(new MockHttpService(MyApiFilmsResponse)).Lookup(filmName, (rs) => {
                pick = rs.ElementAt(new Random().Next(0, rs.Count - 1));
                return new LookupChoice(pick);
            });
            Assert.AreEqual(imdbId, pick.ImdbId);
        }

        [TestMethod]
        public void FilmLookup_searches_again_for_new_title_given_by_callback()
        {
            int attempt = 0;
            string imdbId = new FilmLookupService(new MockHttpService(url => (url.Contains("Superman")) ? MyApiFilmsSupermanResponse : MyApiFilmsResponse)).Lookup("Superman", (rs) => {
                attempt++;
                return (attempt == 1) ? new LookupChoice(LookupChoice.Action.NewSearch, filmName) : new LookupChoice(rs.First());
            });
            Assert.AreEqual("tt0064505", imdbId);
        }

        //The callback should be able to ask for 10 more results
        //If there was a timout due to bad internet or API rate limiting, should be able to retry same search again.
        //Shouldn't necessarily re-clean title if a user has entered a new title to search for.
    }
}
