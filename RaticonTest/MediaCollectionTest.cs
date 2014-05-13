using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Model;
using System.IO.Abstractions.TestingHelpers;

namespace RaticonTest
{
    [TestClass]
    public class MediaCollectionTest
    {
        [TestMethod]
        public void It_should_build_a_collection_from_subfolders()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"C:\Media\A", new MockDirectoryData() },
                { @"C:\Media\B", new MockDirectoryData() },
                { @"C:\Media\C", new MockDirectoryData() }
            });
            MediaCollection m = new MediaCollection(@"C:\Media",fileSystem);

            CollectionAssert.AreEqual(new[] { "A", "B", "C" }, m.Items.Select(f => f.FolderName).ToArray());
        }
    }
}
