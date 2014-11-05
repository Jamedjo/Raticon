using Raticon.Service;
using Raticon.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raticon.Model
{
    public class DummyCollection : IMediaCollection<DummyFilm>
    {
        public IEnumerable<DummyFilm> Items { get; protected set; }

        public DummyCollection(int count = 10)
        {
            Random r = new Random();
            var items = new List<DummyFilm>();
            for (int i = 0; i < count; i++)
            {
                items.Add(new DummyFilm(r));
            }
            Items = items;
        }
    }

    public class DummyFilm : AbstractFilmFromFolder
    {
        private string[] titles = new[]{"A Film","B Movie","C Sequel","D Drama","E Episode","F Fantasy"};

        private bool iconMissing;
        public string Icon
        {
            get
            {
                return (iconMissing) ? null : "pack://application:,,,/Raticon;component/Resources/Folder.ico";
            }
        }

        public DummyFilm(Random r)
        {
            Title = titles[r.Next(0,titles.Length)];
            Path = @"C:\Some\Long\Path\To\Trailers\" + Title;
            ImdbId = "tt"+r.Next(0,9999999).ToString("0000000");
            Year = r.Next(1937,2015).ToString();
            Rating = (5.0 + r.NextDouble()*5.0).ToString("0.0");
            iconMissing = r.NextDouble() < 0.2;
            FolderName = Title;
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

    public class DummyErrorLookupContext : LookupContext
    {
        public DummyErrorLookupContext()
            : base(new List<LookupResult>(), "Italian.Job",
            new System.Net.WebException("The operation has timed out", System.Net.WebExceptionStatus.Timeout),
            @"http://www.myapifilms.com/title?limit=10&title=Italian%20Job", "{\"code\":508,\"message\":\"The server does not accept more requests for lack of resources. Please try again later\"}") { }
    }

    public class DummyIconLayoutViewModel : IconLayoutViewModel
    {
        public DummyIconLayoutViewModel() : base(DummyFolderJpg(), "8.1")
        {
        }

        private static string DummyFolderJpg()
        {
            var folderJpgPath = @"C:\Temp\folder.jpg";
            if (!System.IO.File.Exists(folderJpgPath))
            {
                new PosterService().Download(@"http://ia.media-imdb.com/images/M/MV5BODU4MjU4NjIwNl5BMl5BanBnXkFtZTgwMDU2MjEyMDE@._V1_SX300.jpg", folderJpgPath);
            }
            return folderJpgPath;
        }
    }
}
