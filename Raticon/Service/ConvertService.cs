using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raticon.Service
{
    public class ConvertService
    {
        /// <summary>
        /// Uses ImageMagick COM object to convert images
        /// </summary>
        /// <param name="workingDirectory"></param>
        /// <param name="arguments">Array of arguments to pass to ImageMagick</param>
        /// <param name="fileName">Original name of file to be processed</param>
        public void Convert(string workingDirectory, List<string> arguments, string fileName)
        {
            ImageMagickObject.MagickImage img = new ImageMagickObject.MagickImage();
            arguments.Add(fileName);
            object[] args = arguments.ToArray<object>();
            object result = img.Convert(args);
        }
    }
}
