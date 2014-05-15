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
        public void Process(IFilm film)
        {
            BuildFolderIco(film);
            SetupFolderIcon(film.Path);
        }

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
