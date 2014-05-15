using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raticon.Model;
using Raticon.Resources;

namespace Raticon.Service
{
    public class IconService
    {
        public string Process(IFilm film)
        {
            new ResourceService().ExtractTo("Raticon.star.png", film.Path + @"\star.png");
            new ResourceService().ExtractTo("Raticon.Resources.desktop.ini", film.Path + @"\desktop.ini");
            System.IO.File.SetAttributes(film.Path + @"\desktop.ini", System.IO.FileAttributes.Hidden);
            new HttpService().GetBinary(film.Poster,film.Path + @"\folder.jpg");
            string script = new IconScript(film.Rating).TransformText();
            string output = new ShellService().Execute(script, film.Path);
            System.IO.File.Delete(film.Path + @"\star.png");
            System.IO.File.SetAttributes(film.Path, System.IO.FileAttributes.ReadOnly);
            return output;
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
