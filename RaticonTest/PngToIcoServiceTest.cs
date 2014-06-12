using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Service;
using System.Drawing;

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

            Bitmap bitmap = new Bitmap(new ResourceService().GetAsStream("Raticon;component/star.png"));

            new PngToIcoService().Convert(bitmap, icoPath);
            Assert.IsTrue(System.IO.File.Exists(icoPath));
        }
    }
}

