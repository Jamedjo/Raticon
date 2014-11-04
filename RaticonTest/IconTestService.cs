using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Service;
using Raticon.Model;
using System.IO;

namespace RaticonTest
{
    [TestClass]
    public class IconServiceTest
    {
        IFilmFromFolder filmMock = FilmMock.FromBasePath(@"C:\Temp");

        [TestInitialize]
        public void IconServiceTest_Initialize()
        {
            System.IO.Directory.CreateDirectory(filmMock.Path);
        }

        [TestMethod]
        public void It_shouldnt_attempt_much_if_folderIco_already_exists()
        {
            File.WriteAllText(filmMock.PathTo("folder.ico"),"");
            new IconService().Process(filmMock);
            Assert.IsFalse(File.Exists(filmMock.PathTo("desktop.ini")));
            File.Delete(filmMock.PathTo("folder.ico"));
        }

        [TestMethod]
        public void It_shouldnt_attempt_much_if_folder_has_no_rating()
        {
            filmMock = new NotAFilmMock(@"C:\Temp");
            new IconService().Process(filmMock);
            Assert.IsFalse(File.Exists(filmMock.PathTo("desktop.ini")));
        }

        [TestMethod]
        public void It_should_make_an_icon()
        {
            new IconService().Process(filmMock);
            Assert.IsTrue(System.IO.File.Exists(filmMock.PathTo("folder.ico")));
        }

        [TestMethod]
        public void It_should_set_correct_attributes()
        {
            new IconService().Process(filmMock);
            Assert.AreEqual(FileAttributes.ReadOnly | FileAttributes.Directory, File.GetAttributes(filmMock.Path));
            Assert.AreEqual(FileAttributes.Hidden, File.GetAttributes(filmMock.PathTo("desktop.ini")));
        }

        [TestCleanup]
        public void IconServiceTestCleanup()
        {
            try
            {
                System.IO.File.SetAttributes(filmMock.Path, System.IO.FileAttributes.Normal);
                System.IO.Directory.Delete(filmMock.Path, true);
            }
            catch (Exception)
            { }
        }
    }

    [TestClass]
    public class ResourceServiceTest
    {
        [TestMethod]
        public void ResourceService_extracts_embedded_resource_to_filesytem()
        {
            string path = @"C:\Temp\logo.png";
            new EmbeddedResourceService(typeof(ResourceServiceTest)).ExtractTo("RaticonTest.Logo.png", path);
            Assert.IsTrue(System.IO.File.Exists(path));
            System.IO.File.Delete(path);
        }

        [TestMethod]
        public void ResourceService_extracts_wpf_resource_to_filesystem()
        {
            string path = @"C:\Temp\star.png";
            new ResourceService().ExtractTo(new Uri("pack://application:,,,/Raticon;component/star.png"), path);
            Assert.IsTrue(System.IO.File.Exists(path));
            System.IO.File.Delete(path);
        }
    }

    public class FilmMock : AbstractFilmFromFolder
    {
        public FilmMock(string path)
        {
            Path = path;
            Rating = "8.0";
            Poster = @"http://i.imgur.com/OXGEGDr.jpg";
        }

        public static FilmMock FromBasePath(string base_path)
        {
            return new FilmMock(base_path + @"\In.the.Heat.of.the.Night.1967");
        }
    }

    public class NotAFilmMock : AbstractFilmFromFolder
    {
        public NotAFilmMock(string base_path)
        {
            Path = base_path + @"\.git";
        }
    }
}
