using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Model;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Raticon.Service;


namespace RaticonTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class FilmTest
    {
        public FilmTest()
        {

        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        Film film;
        string test_path = @"C:\Some\Path\To\In.the.Heat.of.the.Night.1967";

        [TestInitialize()]
        public void FilmTestInitialize()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"C:\Some\Path\To\In.the.Heat.of.the.Night.1967\InTheHeatofTheNight_imdb_.nfo", new MockFileData("http://www.imdb.com/title/tt0061811/") }
            });
            film = new Film(test_path, fileSystem, new MockRatingService());
        }


        [TestMethod]
        public void It_should_have_a_folder_name()
        {
            Assert.IsTrue(film.FolderName.Length > 10);
        }

        [TestMethod]
        public void It_should_have_a_path()
        {
            Assert.AreEqual(film.Path, test_path);
        }

        [TestMethod]
        public void Its_folder_name_should_be_the_last_part_of_its_path()
        {
            Assert.IsTrue(film.Path.IndexOf(film.FolderName) > 10);
        }

        [TestMethod]
        public void It_should_get_imdb_id_from_nfo()
        {
            Assert.AreEqual("tt0061811", film.ImdbId);
        }

        [TestMethod]
        public void It_should_get_rating_from_rating_service()
        {
            Assert.AreEqual(film.Rating, "7.0");
        }

        [TestMethod]
        public void It_should_create_folderJpg_when_required()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                { film.Path, new MockDirectoryData() }
            });
            var httpService = new MockBinaryHttpService(fileSystem);
            film.RequireFolderJpg(fileSystem,httpService);
            Assert.IsTrue(fileSystem.File.Exists(film.PathTo("folder.jpg")));
        }

        [TestMethod]
        public void It_shouldnt_create_folderJpg_if_it_exists()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                {film.Path, new MockDirectoryData() },
                {film.PathTo("folder.jpg"), new MockFileData("")}
            });
            var httpService = new MockBinaryHttpService(fileSystem);
            film.RequireFolderJpg(fileSystem, httpService);
            Assert.IsFalse(httpService.WasCalled);
        }
    }

    public class MockRatingService : IRatingService
    {
        public override RatingResult getRating(string imdbId)
        {
            return new RatingResult { Rating = "7.0" };
        }
    }

    public class MockBinaryHttpService : IHttpService
    {
        IFileSystem fileSystem;
        public bool WasCalled = false;
        public MockBinaryHttpService(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }
        public override void GetBinary(string url, string path)
        {
            WasCalled = true;
            fileSystem.File.Create(path);
        }

        public override string Get(string url)
        {
            throw new NotImplementedException();
        }
    }
}
