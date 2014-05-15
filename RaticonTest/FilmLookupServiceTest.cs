using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Service;

namespace RaticonTest
{
    [TestClass]
    public class FilmLookupServiceTest
    {
        [TestMethod]
        public void It_should_return_multiple_results_for_ambiguous_titles()
        {
            var results = new FilmLookupService().Search("Italian Job");
            Assert.IsTrue(results.Length >= 2);
        }
    }
}
