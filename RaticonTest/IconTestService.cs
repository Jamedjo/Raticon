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
        IFilmFromFolder filmMock = new FilmMock(@"C:\Temp");

        public void IconServiceTestInitialize()
        {
            System.IO.Directory.CreateDirectory(filmMock.Path);
            new IconService().Process(filmMock);
        }

        [TestMethod]
        public void It_shouldnt_attempt_much_if_folderIco_already_exists()
        {
            System.IO.Directory.CreateDirectory(filmMock.Path);
            File.WriteAllText(filmMock.PathTo("folder.ico"),"");
            new IconService().Process(filmMock);
            Assert.IsFalse(File.Exists(filmMock.PathTo("desktop.ini")));
            File.Delete(filmMock.PathTo("folder.ico"));
        }

        [TestMethod]
        public void It_shouldnt_attempt_much_if_folder_has_no_rating()
        {
            filmMock = new NotAFilmMock(@"C:\Temp");
            IconServiceTestInitialize();
            Assert.IsFalse(File.Exists(filmMock.PathTo("desktop.ini")));
        }

        [TestMethod]
        public void It_should_make_an_icon()
        {
            IconServiceTestInitialize();
            Assert.IsTrue(System.IO.File.Exists(filmMock.PathTo("folder.ico")));
        }

        [TestMethod]
        public void It_should_set_correct_attributes()
        {
            IconServiceTestInitialize();
            Assert.AreEqual(FileAttributes.ReadOnly | FileAttributes.Directory, File.GetAttributes(filmMock.Path));
            Assert.AreEqual(FileAttributes.Hidden, File.GetAttributes(filmMock.PathTo("desktop.ini")));
        }

        [TestMethod]
        [ExpectedException(typeof(Raticon.Service.IconService.ImageMagickNotInstalledException))]
        public void It_should_throw_an_exception_if_imagemagick_not_installed()
        {
            string path = @"C:\Temp\RaticonNoImageMagickInstalledTest";
            Directory.CreateDirectory(path);
            File.WriteAllLines(path + @"\convert.bat", new[] { "" });
            new IconService().RaiseErrorIfImageMagickInvalid(path);
        }

        [TestMethod]
        [ExpectedException(typeof(Raticon.Service.IconService.ImageMagickVersionException))]
        public void It_should_throw_an_exception_if_imagemagick_version_isnt_suitable()
        {
            string path = @"C:\Temp\RaticonWrongImageMagickVersionTest";
            Directory.CreateDirectory(path);
            File.WriteAllLines(path + @"\convert.bat", new[] { "echo Version: ImageMagick 6.8.8-2 Q14 x64 2014-02-08" });
            new IconService().RaiseErrorIfImageMagickInvalid(path);
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


    public class FilmMock : AbstractFilmFromFolder
    {
        public FilmMock(string base_path)
        {
            Path = base_path+@"\In.the.Heat.of.the.Night.1967";
            Rating = "8.0";
            Poster = @"http://i.imgur.com/OXGEGDr.jpg";
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
