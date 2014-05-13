using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Model;

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
            film = new Film(test_path);
        }
        
        
        [TestMethod]
        public void It_should_have_a_folder_name()
        {
            Assert.IsTrue(film.FolderName.Length > 10);
        }

        [TestMethod]
        public void It_should_have_a_path()
        {
            Assert.IsTrue(film.Path == test_path);
        }

        [TestMethod]
        public void Its_folder_name_should_be_the_last_part_of_its_path()
        {
            Assert.IsTrue(film.Path.IndexOf(film.FolderName) > 10);
        }
    }
}
