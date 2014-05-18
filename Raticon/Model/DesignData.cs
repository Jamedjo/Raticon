using Raticon.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raticon.Model
{
    public class DummyCollection : IMediaCollection
    {
        public DummyCollection(int count = 10, string collection_path = @"C:\Some\Long\Path\To\Trailers")
        {
            Items = new List<IFilm>();
            for (int i = 0; i < count; i++)
            {
                Items.Add(new DummyFilm(collection_path,i));
            }
        }
    }

    public class DummyFilm : IFilm
    {
        private string[] titles = new[]{"A Film","B Movie","C Sequel","D Drama","E Episode","F Fantasy"};
        public DummyFilm(string collection_path,int seed)
        {
            Random r = new Random(seed);
            Title = titles[r.Next(0,titles.Length)];
            Path = collection_path + @"\" + Title;
            ImdbId = "tt"+r.Next(0,9999999).ToString("0000000");
            Year = r.Next(1937,2015).ToString();
            Rating = (5.0 + r.NextDouble()*5.0).ToString("0.0");
            FolderName = System.IO.Path.GetFileName(Path);
        }
    }

    public class DummyResults : List<LookupResult>
    {
        public DummyResults() : base()
        {
            Add(new LookupResult { ImdbId = "tt0391247", Title = "The Italian Job", Year = "2003", Rating = "6.7", Poster = "" });
            Add(new LookupResult { ImdbId = "tt0317740", Title = "The Italian Job", Year = "2003", Rating = "7.0", Poster = "" });
            Add(new LookupResult { ImdbId = "tt0064505", Title = "The Italian Job", Year = "1969", Rating = "7.4", Poster = "" });
            Add(new LookupResult { ImdbId = "tt0450450", Title = "The Italian", Year = "2005", Rating = "7.6", Poster = "http://ia.media-imdb.com/images/M/MV5BMTU2Nzc0NDA0N15BMl5BanBnXkFtZTcwODQwMTM0MQ@@._V1_SY317_CR0,0,214,317_AL_.jpg" });
            Add(new LookupResult { ImdbId = "tt1801042", Title = "Clarkson: The Italian Job", Year = "2010", Rating = "7.3", Poster = "http://ia.media-imdb.com/images/M/MV5BMjIzMzY0MDY1N15BMl5BanBnXkFtZTcwNDIzMDk1OA@@._V1_SX214_CR0,0,214,317_AL_.jpg" });
            Add(new LookupResult { ImdbId = "tt1465513", Title = "The Italian Key", Year = "2011", Rating = "5.4", Poster = "" });
        }
    }

    public class DummyLookupContext : LookupContext
    {
        public DummyLookupContext() : base(new DummyResults(), "Italian Job") { }
    }
}
