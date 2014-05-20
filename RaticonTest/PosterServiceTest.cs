using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Abstractions.TestingHelpers;
using System.Collections.Generic;
using Raticon.Model;
using Raticon.Service;
using System.IO.Abstractions;

namespace RaticonTest
{
    [TestClass]
    public class PosterServiceTest
    {
        string url = @"http://posters.com/film27.jpg";
        string folderPath = @"C:\Film27";
        string imagePath = @"C:\Film27\folder.jpg";

        [TestMethod]
        public void PosterService_create_folderJpg()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                { folderPath, new MockDirectoryData() }
            });
            var httpService = new MockBinaryHttpService(fileSystem);
            new PosterService(fileSystem, httpService).Download(url, imagePath);
            Assert.IsTrue(fileSystem.File.Exists(imagePath));
        }

        [TestMethod]
        public void PosterService_shouldnt_create_folderJpg_if_it_exists()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                {folderPath, new MockDirectoryData() },
                {imagePath, new MockFileData("")}
            });
            var httpService = new MockBinaryHttpService(fileSystem);
            new PosterService(fileSystem, httpService).Download(url, imagePath);
            Assert.IsFalse(httpService.WasCalled);
        }

        [TestMethod]
        public void PosterService_shouldnt_request_folderJpg_from_empty_url()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
                {folderPath, new MockDirectoryData() }
            });
            var httpService = new MockBinaryHttpService(fileSystem);
            new PosterService(fileSystem, httpService).Download("", imagePath);
            Assert.IsFalse(httpService.WasCalled);
        }

        [TestMethod]
        public void PosterService_should_call_handler_on_error()
        {
            bool errorHandlerCalled = false;
            var httpService = new MockErrorBinaryHttpService();
            new PosterService(new MockFileSystem(), httpService).Download(url, imagePath, (u, p) => errorHandlerCalled = true);
            Assert.IsTrue(errorHandlerCalled);
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

    public class MockErrorBinaryHttpService : MockBinaryHttpService
    {
        public MockErrorBinaryHttpService() : base(null) { }

        public override void GetBinary(string url, string path)
        {
            throw new System.Net.WebException();
        }
    }

    //public class MockInvalidFilmRatingService : IRatingService
    //{
    //    public override RatingResult GetRating(string imdbId)
    //    {
    //        return new RatingResult { };
    //    }
    //}
}
