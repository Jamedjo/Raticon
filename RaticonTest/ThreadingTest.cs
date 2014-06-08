using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Model;
using Raticon.Service;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace RaticonTest
{
    [TestClass]
    public class ThreadingTest
    {
        //[TestMethod]
        //public async Task Multiple_films_should_fetch_their_ratings_at_once()
        //{
        //    List<IFilm> items = new List<IFilm>();
        //    for (int i = 0; i < items.Count; i++)
        //    {
        //        //items.Add(new Film(collection_path, i));
        //    }

        //}


        //Total time to get 5 backgroundRequests should be no more than twice the average time.
        //Film rating should fetch in background
        //Icons should build in background maybe
    }
}
