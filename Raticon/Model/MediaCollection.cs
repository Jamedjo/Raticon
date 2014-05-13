using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Abstractions;

namespace Raticon.Model
{
    public class IMediaCollection
    {
        public IFilm[] Items { get; protected set; }
    }
    public class MediaCollection : IMediaCollection
    {
        private IFileSystem fileSystem;
        public MediaCollection(string folder, IFileSystem fileSystem=null)
        {
            if (fileSystem == null) fileSystem = new FileSystem();
            this.fileSystem = fileSystem;

            string[] subfolders = fileSystem.Directory.GetDirectories(folder);
            Items = subfolders.Select(f => new Film(f, fileSystem)).ToArray();
        }
    }
}
