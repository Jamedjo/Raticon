using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Model;
using System.IO.Abstractions.TestingHelpers;

namespace RaticonTest
{
    [TestClass]
    public class FilmFactoryTest
    {
        [TestMethod]
        public void It_should_build_a_mock_film_from_a_path()
        {
            Assert.AreEqual(typeof(FilmMock), FilmFactory<FilmMock>.BuildFilm(@"C:\Media\A").GetType());
        }

        [TestMethod]
        public void FilmFactory_builds_a_film_from_path_and_filesystem()
        {
            var fileSystem = new MockFileSystem();
            IFilm film = FilmFactory<FilmFromFolder>.BuildFilm(@"C:\Media\A", fileSystem);
            Assert.AreEqual(typeof(FilmFromFolder), film.GetType());
        }
    }
}
