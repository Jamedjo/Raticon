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
}
