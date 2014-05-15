using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Service;

namespace RaticonTest
{
    [TestClass]
    public class IconServiceTest
    {
        [TestMethod]
        public void It_should_fetch_folder_jpg_if_missing()
        {

        }

        //It should use make star.png avaliable (assert call resourceservice to extract to path)
        //It_should_fetch_folder_jpg_if_missing
        //It should throw and exception if imagemagick version isn't suitable
        //It should call shell service with correct command 
    }

    [TestClass]
    public class ResourceServiceTest
    {
        [TestMethod]
        public void It_should_extract_embedded_resource_to_filesytem()
        {
            string path = @"C:\Temp\star.png";
            new ResourceService().Extract("Raticon.star.png", path);
            Assert.IsTrue(System.IO.File.Exists(path));
        }
    }
}
