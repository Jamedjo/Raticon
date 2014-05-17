using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Abstractions;
using System.Reflection;

namespace Raticon.Model
{
    public class IMediaCollection
    {
        public IList<IFilm> Items { get; protected set; }
    }
    public class MediaCollection<T> : IMediaCollection where T : IFilm
    {
        private IFileSystem fileSystem;
        public MediaCollection(string folder, IFileSystem fileSystem=null)
        {
            if (fileSystem == null) fileSystem = new FileSystem();
            this.fileSystem = fileSystem;

            string[] subfolders = fileSystem.Directory.GetDirectories(folder);
            Items = subfolders.Select(f => (T)CreateInstance(typeof(T), f, fileSystem)).ToList<IFilm>();
        }

        //Fix Activator.CreateInstance so it handles Constructor with optional params
        private static object CreateInstance(Type type, params object[] args)
        {
            BindingFlags flags = BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.OptionalParamBinding;
            return Activator.CreateInstance(type, flags, null, args, System.Globalization.CultureInfo.CurrentCulture);
        }
    }
}
