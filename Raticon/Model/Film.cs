using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Abstractions;
using System.Text.RegularExpressions;

namespace Raticon.Model
{
    public class Film
    {
        public string FolderName { get; set; }
        public string Path { get; set; }

        private IFileSystem fileSystem;

        public Film(string path, IFileSystem fileSystem=null)
        {
            if (fileSystem == null) fileSystem = new FileSystem();
            this.fileSystem = fileSystem;

            Path = path;
            
            FolderName = fileSystem.Path.GetFileName(path);
        }

        public string ImdbIdFromNfo()
        {
            string nfo_file = fileSystem.Directory.GetFiles(Path, "*imdb*.nfo").First();
            string imdb_line =  fileSystem.File.ReadAllLines(nfo_file).First();
            return Regex.Match(imdb_line,@"/(tt\d+)",RegexOptions.IgnoreCase).Groups[1].Value;
        }
    }
}
