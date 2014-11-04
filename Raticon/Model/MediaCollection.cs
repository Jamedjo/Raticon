using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Abstractions;

namespace Raticon.Model
{
    public interface IMediaCollection<out T> where T : IFilm
    {
        IEnumerable<T> Items { get; }
    }

    public class MediaCollection<T> : IMediaCollection<T> where T : IFilmFromFolder
    {
        public IEnumerable<T> Items { get; protected set; }

        public MediaCollection(string folder, IFileSystem fileSystem=null)
        {
            if (fileSystem == null) fileSystem = new FileSystem();

            string[] subfolders = fileSystem.Directory.GetDirectories(folder);
            Items = subfolders.Select(f => FilmFactory<T>.BuildFilm(f, fileSystem)).ToList();
        }
    }
}
