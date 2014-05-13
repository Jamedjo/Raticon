using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Service;
using Raticon.Model;

namespace RaticonTest
{
    [TestClass]
    public class RatingServiceTest
    {
        private RatingService ratingService = new RatingService();

        [TestMethod]
        public void ItShouldGetARatingResult()
        {
            RatingResult expectedResult = new RatingResult
            {
                Rating = "8.0",
                Title = "In the Heat of the Night",
                Year = "1967"
            };
            
            Assert.AreEqual(ratingService.getRating("tt0061811", new MockHttpService()), expectedResult);
        }
    }

    class MockHttpService : IHttpService
    {
        public override string get(string url)
        {
            return @"{""Title"":""In the Heat of the Night"",""Year"":""1967"",""Rated"":""Approved"",""Released"":""14 Oct 1967"",""Runtime"":""109 min"",""Genre"":""Crime, Drama, Mystery"",""Director"":""Norman Jewison"",""Writer"":""Stirling Silliphant (screenplay), John Ball (based on a novel by)"",""Actors"":""Sidney Poitier, Rod Steiger, Warren Oates, Lee Grant"",""Plot"":""An African American police detective is asked to investigate a murder in a racially hostile southern town."",""Language"":""English"",""Country"":""USA"",""Awards"":""Won 5 Oscars. Another 17 wins & 12 nominations."",""Poster"":""http://ia.media-imdb.com/images/M/MV5BMTk3NjkxMDc1MV5BMl5BanBnXkFtZTcwMDIwMjI0NA@@._V1_SX300.jpg"",""Metascore"":""N/A"",""imdbRating"":""8.0"",""imdbVotes"":""39,893"",""imdbID"":""tt0061811"",""Type"":""movie"",""Response"":""True""}";
        }
    }
}

