using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raticon.Model;
using Raticon.Resources;
using System.Text.RegularExpressions;

namespace Raticon.Service
{
    public class IconService
    {
        public void Process(IFilm film)
        {
            if (System.IO.File.Exists(film.PathTo("folder.ico")) || String.IsNullOrWhiteSpace(film.Rating) ) { return; }
            RaiseErrorIfImageMagickInvalid(film.Path);
            BuildFolderIco(film);
            SetupFolderIcon(film.Path);
        }

        /// <summary>
        /// Checks ImageMagick version 6.8.8-3 or later is installed
        /// </summary>
        /// <param name="workingDir">A directory where convert.exe is on the PATH. Needed because default of System32 has a different convert.exe</param>
        public void RaiseErrorIfImageMagickInvalid(string workingDir)
        {
            string output = new ShellService().Execute("convert -version", workingDir);
            if(output.Length<10) {  throw new ImageMagickNotInstalledException("ImageMagick needs to be installed."); }
            string version = Regex.Match(output, @"\d+\.\d+\.\d+-?\d*").Value;
            string[] vs = version.Split(new []{'.','-'});
            int major = Int32.Parse(vs[0]);
            int minor = Int32.Parse(vs[1]);

            if (major >= 7 || (major == 6 && minor >= 9)) { return; }
            if (major == 6 && minor == 8)
            {
                int tiny = Int32.Parse(vs[2]);
                int revision = Int32.Parse(vs[3]);
                if (tiny >= 9 || tiny == 8 && revision >=3) { return; }
            }
            throw new ImageMagickVersionException("ImageMagick needs to be version 6.8.8-3 or later.");
        }
        public class ImageMagickVersionException : Exception { public ImageMagickVersionException(string m) : base(m) { } }
        public class ImageMagickNotInstalledException : Exception { public ImageMagickNotInstalledException(string m) : base(m) { } }

        private void SetupFolderIcon(string path)
        {
            new ResourceService().ExtractTo("Raticon.Resources.desktop.ini", path + @"\desktop.ini");
            System.IO.File.SetAttributes(path + @"\desktop.ini", System.IO.FileAttributes.Hidden);

            //Folder needs to be read only for icon to show
            System.IO.File.SetAttributes(path, System.IO.FileAttributes.ReadOnly);
        }

        private void BuildFolderIco(IFilm film)
        {
            //Required files
            new ResourceService().ExtractTo("Raticon.star.png", film.PathTo("star.png"));
            film.RequireFolderJpg();

            //Run Resources/IconScript.tt
            string script = new IconScript(film.Rating).TransformText();
            string output = new ShellService().Execute(script, film.Path);

            //Cleanup
            System.IO.File.Delete(film.PathTo("star.png"));
        }

    }

    public class ResourceService
    {
        private System.IO.Stream GetAsStream(string resource)
        {
            System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            return thisExe.GetManifestResourceStream(resource);
        }

        //private string GetAsText(string resource)
        //{
        //    var stream = GetAsStream(resource);
        //    string text;
        //    using (System.IO.StreamReader streamReader = new System.IO.StreamReader(stream, Encoding.UTF8))
        //    {
        //        text = streamReader.ReadToEnd();
        //    }
        //    return text;
        //}

        public void ExtractTo(string resource, string path)
        {
            System.IO.Stream star = GetAsStream(resource);
            using (var file = System.IO.File.Create(path))
            {
                star.CopyTo(file);
            }
        }
    }
}
