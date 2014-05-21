using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Raticon.Service
{
    public class ImdbIdCacheService
    {
        public string ReadFromFolder(string nfoFolder, IFileSystem fileSystem)
        {
            try
            {
                string nfoFile = fileSystem.Directory.GetFiles(nfoFolder, "*imdb*.nfo").First();
                return ReadFromNfoFile(nfoFile, fileSystem);
            }
            catch (Exception e)
            {
                if (e is InvalidOperationException || e is System.IO.DirectoryNotFoundException)
                    return null;
                else
                    throw;
            }
        }

        public string ReadFromNfoFile(string nfoFile, IFileSystem fileSystem)
        {
            string imdb_line = fileSystem.File.ReadAllLines(nfoFile).First();
            return Regex.Match(imdb_line, @"/(tt\d+)", RegexOptions.IgnoreCase).Groups[1].Value;
        }

        public void CacheInFolder(string imdbId, string nfoFolder, IFileSystem fileSystem)
        {
            string filePrefix = System.IO.Path.GetFileName(nfoFolder).Replace(' ', '.');
            string nfoFile = fileSystem.Path.Combine(nfoFolder, filePrefix + "_imdb_.nfo");
            WriteToNfoFile(nfoFile, imdbId, fileSystem);
        }

        public void WriteToNfoFile(string nfoFile, string imdbId, IFileSystem fileSystem)
        {
            try
            {
                fileSystem.File.WriteAllText(nfoFile, @"http://www.imdb.com/title/" + imdbId + @"/");
            }
            catch(System.IO.DirectoryNotFoundException)
            {

            }
        }
    }
}
