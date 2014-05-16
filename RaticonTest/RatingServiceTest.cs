using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Service;
using Raticon.Model;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.IO;

namespace RaticonTest
{
    [TestClass]
    public class RatingServiceTest
    {
        private RatingService ratingService = new RatingService();

        [TestMethod]
        public void It_should_get_a_rating_result()
        {
            RatingResult expectedResult = new RatingResult
            {
                Rating = "8.0",
                Title = "In the Heat of the Night",
                Year = "1967",
                Poster = @"http://ia.media-imdb.com/images/M/MV5BMTk3NjkxMDc1MV5BMl5BanBnXkFtZTcwMDIwMjI0NA@@._V1_SX300.jpg"
            };
            
            Assert.AreEqual(ratingService.GetRating("tt0061811", new MockHttpService()), expectedResult);
        }
    }

    class MockHttpService : IHttpService
    {
        public override string Get(string url)
        {
            return @"{""Title"":""In the Heat of the Night"",""Year"":""1967"",""Rated"":""Approved"",""Released"":""14 Oct 1967"",""Runtime"":""109 min"",""Genre"":""Crime, Drama, Mystery"",""Director"":""Norman Jewison"",""Writer"":""Stirling Silliphant (screenplay), John Ball (based on a novel by)"",""Actors"":""Sidney Poitier, Rod Steiger, Warren Oates, Lee Grant"",""Plot"":""An African American police detective is asked to investigate a murder in a racially hostile southern town."",""Language"":""English"",""Country"":""USA"",""Awards"":""Won 5 Oscars. Another 17 wins & 12 nominations."",""Poster"":""http://ia.media-imdb.com/images/M/MV5BMTk3NjkxMDc1MV5BMl5BanBnXkFtZTcwMDIwMjI0NA@@._V1_SX300.jpg"",""Metascore"":""N/A"",""imdbRating"":""8.0"",""imdbVotes"":""39,893"",""imdbID"":""tt0061811"",""Type"":""movie"",""Response"":""True""}";
        }

        public override void GetBinary(string url, string fileName)
        {
            throw new NotImplementedException();
        }
    }

    class AssertDontCallHttpService : IHttpService
    {
        public override string Get(string url)
        {
            throw new AssertFailedException("Http service Get was called with url '"+url+"'");
        }

        public override void GetBinary(string url, string fileName)
        {
            throw new AssertFailedException("Http service GetBinary was called with url '" + url + "' and fileName '"+fileName+"'");
        }
    }

    [TestClass]
    public class DiskCachedRatingServiceTest
    {
        private DiskCachedRatingService ratingService = new DiskCachedRatingService();

        [TestMethod]
        public void It_should_store_result_after_http_lookup()
       {
            IFileSystem mockFileSystem = new MockFileSystem();
            ratingService.GetRatingWithCache("tt0061811", new MockHttpService(), mockFileSystem);
            string expectedPath = Path.Combine(Raticon.Constants.CommonApplicationDataPath, "Cache", "tt0061811.json");
            Assert.IsTrue(mockFileSystem.File.Exists(expectedPath));
        }

        [TestMethod]
        public void It_shouldnt_use_http_service_if_disk_rating_is_present()
        {
            IFileSystem mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                {Path.Combine(Raticon.Constants.CommonApplicationDataPath, "Cache", "tt0061811.json"), new MockFileData("{'Rating':'7.3'}")}
            });

            //AssertDontCallHttpService
            ratingService.GetRatingWithCache("tt0061811", new AssertDontCallHttpService(), mockFileSystem);
        }

        [TestMethod]
        public void It_should_return_valid_rating_result_from_cache()
        {
            string rating = "7.3";
            IFileSystem mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                {Path.Combine(Raticon.Constants.CommonApplicationDataPath, "Cache", "tt0061811.json"), new MockFileData("{'Rating':'"+rating+"'}")}
            });

            RatingResult result = ratingService.GetRatingWithCache("tt0061811", new MockHttpService(), mockFileSystem);
            Assert.AreEqual<string>(result.Rating, rating);
        }
    }
}

