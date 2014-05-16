using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raticon
{
    public static class Constants
    {
        public static string CommonApplicationDataPath
        {
            get
            {
                string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Raticon");
                if(!System.IO.File.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                return path;
            }
        }
    }
}