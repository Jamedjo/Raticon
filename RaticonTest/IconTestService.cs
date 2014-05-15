using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Service;
using Raticon.Model;

namespace RaticonTest
{
    [TestClass]
    public class IconServiceTest
    {

        [TestMethod]
        public void It_should_make_icon()
        {
            IFilm filmMock = new FilmMock(@"C:\Temp");
            System.IO.Directory.Delete(filmMock.Path, true);
            System.IO.Directory.CreateDirectory(filmMock.Path);
            string output = new IconService().Process(filmMock);
            StringAssert.Equals(output, "");
        }

        //It should use make star.png avaliable (assert call resourceservice to extract to path)
        //It_should_fetch_folder_jpg_if_missing
        //It should throw and exception if imagemagick version isn't suitable
        //It should call shell service with correct command
        //It should extract a copy of the desktop.ini file
        //It should cleanup intermediate files if not done by the script
        //It should set directory and dektop.ini attributes if not done by the script
    }

    [TestClass]
    public class ResourceServiceTest
    {
        [TestMethod]
        public void It_should_extract_embedded_resource_to_filesytem()
        {
            string path = @"C:\Temp\star.png";
            new ResourceService().ExtractTo("Raticon.star.png", path);
            Assert.IsTrue(System.IO.File.Exists(path));
            System.IO.File.Delete(path);
        }
    }

    [TestClass]
    public class IconScriptTest
    {
        [TestMethod]
        public void It_should_print_rating_into_script()
        {
            string output = new Raticon.Resources.IconScript("7.3").TransformText();
            StringAssert.Contains(output, "7.3 rating.png");
        }
    }

    public class FilmMock : IFilm
    {
        public FilmMock(string base_path)
        {
            Path = base_path+@"\In.the.Heat.of.the.Night.1967";
            Rating = "8.0";
            Poster = @"http://i.imgur.com/OXGEGDr.jpg";
        }
    }
}
