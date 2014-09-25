using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Service;
using System.Linq;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Collections.Generic;
using Nito.AsyncEx;

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
        public void Search_doesnt_throw_exception_when_internet_down()
        {
            var results = new FilmLookupService(new MockHttpService(s => { throw new System.Net.WebException("Internet down"); })).Search("32498238409");
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
        string MatrixResultsWithSpoofResponse = "[{\"genres\":[\"Comedy\",\"Short\"],\"idIMDB\":\"tt0274085\",\"plot\":\"A spoof created for the 2000 MTV Movie Awards, combining The Matrix and \\\"Sex in the City\\\".\",\"rating\":\"7.2\",\"runtime\":[\"6 min\"],\"simplePlot\":\"A spoof created for the 2000 MTV Movie Awards, combining The Matrix and \\\"Sex in the City\\\".\",\"title\":\"Sex and the Matrix\",\"year\":\"2000\"},{\"countries\":[\"USA\",\"Australia\"],\"directors\":[{\"name\":\"Andy Wachowski\",\"nameId\":\"nm0905152\"},{\"name\":\"Lana Wachowski\",\"nameId\":\"nm0905154\"}],\"filmingLocations\":[\"AON Tower, Kent Street, Sydney, New South Wales, Australia\"],\"genres\":[\"Action\",\"Sci-Fi\"],\"idIMDB\":\"tt0133093\",\"languages\":[\"English\"],\"metascore\":\"8.7\",\"plot\":\"Thomas A. Anderson is a man living two lives. By day he is an average computer programmer and by night a hacker known as Neo. Neo has always questioned his reality, but the truth is far beyond his imagination. Neo finds himself targeted by the police when he is contacted by Morpheus, a legendary computer hacker branded a terrorist by the government. Morpheus awakens Neo to the real world, a ravaged wasteland where most of humanity have been captured by a race of machines that live off of the humans' body heat and electrochemical energy and who imprison their minds within an artificial reality known as the Matrix. As a rebel against the machines, Neo must return to the Matrix and confront the agents: super-powerful computer programs devoted to snuffing out Neo and the entire human rebellion. Written by redcommander27\",\"rated\":\"R\",\"rating\":\"8.7\",\"releaseDate\":\"19990331\",\"runtime\":[\"136 min\"],\"simplePlot\":\"A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.\",\"title\":\"The Matrix\",\"urlIMDB\":\"http://www.imdb.com/title/tt0133093\",\"urlPoster\":\"http://ia.media-imdb.com/images/M/MV5BMTkxNDYxOTA4M15BMl5BanBnXkFtZTgwNTk0NzQxMTE@._V1_SX214_AL_.jpg\",\"writers\":[{\"name\":\"Andy Wachowski\",\"nameId\":\"nm0905152\"},{\"name\":\"Lana Wachowski\",\"nameId\":\"nm0905154\"}],\"year\":\"1999\"}]";
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
            new CachingFilmLookupService(folderPath, mockFilmFolder, new MockHttpService(MyApiFilmsResponse)).Lookup(filmName, (l)=>new FirstResultPicker().Pick(l));
            Assert.IsTrue(mockFilmFolder.File.Exists(@"C:\Trailers\Italian Job\Italian.Job_imdb_.nfo"));
        }

        [TestMethod]
        public void FilmLookup_returns_null_if_callback_tells_it_to_give_up()
        {
            string imdbId = new FilmLookupService(new MockHttpService(MyApiFilmsResponse)).Lookup(filmName, (l) => new LookupChoice(LookupChoice.Action.GiveUp));
            Assert.IsNull(imdbId);
        }

        [TestMethod]
        public void FilmLookup_returns_id_chosen_by_callback()
        {
            LookupResult pick = null;
            string imdbId = new FilmLookupService(new MockHttpService(MyApiFilmsResponse)).Lookup(filmName, (lookup) => {
                pick = lookup.Results.ElementAt(new Random().Next(0, lookup.Results.Count - 1));
                return new LookupChoice(pick);
            });
            Assert.AreEqual(imdbId, pick.ImdbId);
        }

        [TestMethod]
        public void FilmLookup_searches_again_for_new_title_given_by_callback()
        {
            int attempt = 0;
            string imdbId = new FilmLookupService(new MockHttpService(url => (url.Contains("Superman")) ? MyApiFilmsSupermanResponse : MyApiFilmsResponse)).Lookup("Superman", (lookup) => {
                attempt++;
                return (attempt == 1) ? new LookupChoice(LookupChoice.Action.NewSearch, filmName) : new LookupChoice(lookup.Results.First());
            });
            Assert.AreEqual("tt0064505", imdbId);
        }

        [TestMethod]
        public void FilmLookup_provides_title_to_callback()
        {
            string query = "";
            string imdbId = new FilmLookupService(new MockHttpService(MyApiFilmsResponse)).Lookup(filmName, (lookup) => { query = lookup.Query; return new LookupChoice(LookupChoice.Action.GiveUp); });
            Assert.AreEqual(filmName, query);
        }

        [TestMethod]
        public void FilmLookup_async_fetches_ids_in_background()
        {
            AsyncContext.Run(async () =>
            {
                var idLookupService = new FilmLookupService(new MockHttpService(""));
                string result = await idLookupService.LookupAsync("Superman", (l) => new LookupChoice("tt0078346"));
                Assert.AreEqual("tt0078346", result);
            });
        }

        [TestMethod]
        public void Search_sorts_results_by_score()
        {
            var results = new FilmLookupService(new MockHttpService(MatrixResultsWithSpoofResponse)).Search("The Matrix 1999");
            var ids = results.Select(r => r.ImdbId).ToList();
            Assert.AreEqual(ids.First(), "tt0133093");
            Assert.AreEqual(ids.Last(), "tt0274085");
        }

        //The callback should be able to ask for 10 more results
        //If there was a timout due to bad internet or API rate limiting, should be able to retry same search again.
        //Shouldn't necessarily re-clean title if a user has entered a new title to search for.
    }
}
