using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raticon.Model;
using Raticon.Service;

namespace RaticonDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string base_path = @"C:\Temp\RaticonDemo";
            Directory.CreateDirectory(base_path);
            List<DemoFilm> items = new List<DemoFilm>();
            string[] films = new[] { "The Shawshank Redemption", "The Godfather", "The Godfather: Part II", "The Dark Knight", "Pulp Fiction", "The Good, the Bad and the Ugly", "Schindler's List", "12 Angry Men", "The Lord of the Rings: The Return of the King", "Fight Club", "The Lord of the Rings: The Fellowship of the Ring", "Star Wars: Episode V - The Empire Strikes Back", "Inception", "Forrest Gump", "One Flew Over the Cuckoo's Nest", "Goodfellas", "The Lord of the Rings: The Two Towers", "Star Wars: Episode IV - A New Hope", "The Matrix", "Seven Samurai" };
            string[] ids = new[] { "tt0111161", "tt0068646", "tt0071562", "tt0468569", "tt0110912", "tt0060196", "tt0108052", "tt0050083", "tt0167260", "tt0137523", "tt0120737", "tt0080684", "tt1375666", "tt0109830", "tt0073486", "tt0099685", "tt0167261", "tt0076759", "tt0133093", "tt0047478" };
            for (int i = 0; i < films.Length; i++)
            {
                string film_path = Path.Combine(base_path, FilterPath(films[i]));
                Directory.CreateDirectory(film_path);
                items.Add(new DemoFilm(film_path, ids[i]));
            }
            new IconService().ProcessCollection(items);
            new ShellService().Execute("explorer " + base_path);
        }

        private static string FilterPath(string path)
        {
            var filter = Path.GetInvalidFileNameChars();
            return new string(path.Where(x => !filter.Contains(x)).ToArray());
        }

        class DemoFilm : CachedFilm
        {
            public DemoFilm(string path, string imdbId) : base(path)
            {
                this.imdbIdCache = imdbId;
            }

        }
    }
}
