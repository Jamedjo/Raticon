using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Control;
using Raticon.ViewModel;
using Raticon.Model;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.IO;

namespace RaticonTest
{
    [TestClass]
    public class IconLayoutTest
    {
        Bitmap bitmap;

        [TestInitialize()]
        public void RenderBitmap()
        {
            bitmap = new IconLayout(new DummyIconLayoutViewModel()).RenderToBitmap();
        }

        [TestMethod]
        public void IconLayout_creates_a_32bppArgb_bitmap()
        {
            Assert.AreEqual(PixelFormat.Format32bppArgb, bitmap.PixelFormat);
        }

        [TestMethod]
        public void IconLayout_size_is_256x256()
        {
            Assert.AreEqual(new Size(256,256), bitmap.Size);
        }

        //[TestMethod]
        //public void IconLayout_looks_right()
        //{
        //    string path = @"C:\Temp\folder.png";
        //    if (System.IO.File.Exists(path))
        //    {
        //        System.IO.File.Delete(path);
        //    }
        //    bitmap.Save(path, ImageFormat.Png);
        //    new Raticon.Service.ShellService().Execute(@"explorer.exe C:\Temp");
        //    new Raticon.Service.PngToIcoService().Convert(new Bitmap(path), @"C:\Temp\folder.ico");
        //    throw new Exception("Doesn't look right");
        //}
    }
}
