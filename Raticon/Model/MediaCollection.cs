using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Abstractions;
using System.Reflection;

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

    public static class FilmFactory<T>
    {
        public static T BuildFilm(params object[] args)
        {
            return (T)CreateInstance(typeof(T), args);
        }

        //Fix Activator.CreateInstance so it handles Constructor with optional params
        private static object CreateInstance(Type type, params object[] args)
        {
            BindingFlags flags = BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.OptionalParamBinding;
            return Activator.CreateInstance(type, flags, null, args, System.Globalization.CultureInfo.CurrentCulture);
        }
    }
}
