using Raticon.Service;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace Raticon.Model
{
    public class ConsoleFilm : CachedFilm
    {
        public ConsoleFilm(string path, IFileSystem fileSystem = null) : base(path, fileSystem, new ConsoleResultPickerService())
        {
        }
    }
}
