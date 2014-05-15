using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raticon.Model;

namespace Raticon.Service
{
    public class IconService
    {
        public void process(IFilm film)
        {
            new ResourceService().Extract("Raticon.star.png", film.Path+@"\star.png");
        }

    }

    public class ResourceService
    {
        public void Extract(string resouce, string path)
        {
            System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream star = thisExe.GetManifestResourceStream(resouce);
            using (var file = System.IO.File.Create(path))
            {
                star.CopyTo(file);
            }
        }
    }
}
