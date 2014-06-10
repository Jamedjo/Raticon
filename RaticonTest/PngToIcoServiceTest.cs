using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Service;

namespace RaticonTest
{
    [TestClass]
    public class PngToIcoServiceTest
    {
        [TestMethod]
        public void PngToIco_creates_an_icon_file()
        {
            string icoPath = @"C:\Temp\star.ico";
            if (System.IO.File.Exists(icoPath))
            {
                System.IO.File.Delete(icoPath);
            }

            string pngPath = @"C:\Temp\star.png";
            new ResourceService().ExtractTo("Raticon.star.png", pngPath);

            new PngToIcoService().Convert(pngPath, icoPath);
            Assert.IsTrue(System.IO.File.Exists(icoPath));
        }
    }
}
