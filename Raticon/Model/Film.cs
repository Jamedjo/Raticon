using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Raticon.Model
{
    public class Film
    {
        public string FolderName { get; set; }
        public string Path { get; set; }

        public Film(string path)
        {
            Path = path;
            FolderName = System.IO.Path.GetFileName(path);
        }
    }
}
