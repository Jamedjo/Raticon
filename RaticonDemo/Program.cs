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
            string[] films = new[] { "The Shawshank Redemption", "The Godfather", "The Godfather: Part II", "The Dark Knight", "Pulp Fiction", "The Good, the Bad and the Ugly", "Schindler's List", "12 Angry Men", "The Lord of the Rings: The Return of the King", "Fight Club", "The Lord of the Rings: The Fellowship of the Ring", "Star Wars: Episode V - The Empire Strikes Back", "Inception", "Forrest Gump", "One Flew Over the Cuckoo's Nest", "Goodfellas", "The Lord of the Rings: The Two Towers", "Star Wars: Episode IV - A New Hope", "The Matrix", "Seven Samurai" };
            for (int i = 0; i < films.Length; i++)
            {
                Directory.CreateDirectory(Path.Combine(base_path, FilterPath(films[i])));
            }
            MediaCollection<CachedFilm> collection = new MediaCollection<CachedFilm>(base_path);
            new IconService().ProcessCollection(collection);
            new ShellService().Execute("explorer " + base_path);
        }

        private static string FilterPath(string path)
        {
            var filter = Path.GetInvalidFileNameChars();
            return new string(path.Where(x => !filter.Contains(x)).ToArray());
        }
    }
}
