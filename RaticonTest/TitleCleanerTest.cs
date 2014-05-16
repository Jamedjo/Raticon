using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Service;

namespace RaticonTest
{
    [TestClass]
    public class TitleCleanerTest
    {
        [TestMethod]
        public void It_should_clean_up_trailer_names()
        {
            TitleAssertContains("Dawn_Of_The_Planet_Of_The_Apes_2014_Trailer_B_5.1-1080p-HDTN.mp4", "Dawn Of The Planet Of The Apes");

            TitleAssertRemoves("Dawn_Of_The_Planet_Of_The_Apes_2014_Trailer_B_5.1-1080p-HDTN.mp4", "1080p");
            TitleAssertRemoves("Captain_America_Winter_Soldier_TLR-H-UK-5.1-720p-HDTN.mp4", new[] { "720p", ".mp4" });
            TitleAssertRemoves("her-tlr1_h480p.mov", new[] { "420p", ".mov" });
        }

        [TestMethod]
        public void It_should_clean_up_anything_following_the_year_or_resolution_and_trailing_whitespace()
        {
            string cleanTitle = new TitleCleaner().Clean("Dawn_Of_The_Planet_Of_The_Apes_2014_Trailer_B_5.1-1080p-HDTN.mp4");
            Assert.AreEqual<string>(cleanTitle, "Dawn Of The Planet Of The Apes");
        }

        [TestMethod]
        public void It_should_remove_year_from_trailer_names()
        {
            TitleAssertRemoves("Godzilla (2014) Official Trailer #2 HD.mp4", new[] { "(2014)", "2014", "()" });
        }

        [TestMethod]
        public void It_should_remove_dashes_underscores_and_dots()
        {
            TitleAssertContains("The_Amazing.Spiderman-2-tlr1_h1080p", "The Amazing Spiderman 2");
        }

        [TestMethod]
        public void It_should_not_filter_out_the_entire_title()
        {
            TitleAssertContains("2012", "2012");
        }

        private void TitleAssertContains(string dirtyTitle, string cleanSubstring)
        {
            string cleanTitle = new TitleCleaner().Clean(dirtyTitle);
            StringAssert.Contains(cleanTitle, cleanSubstring);
        }

        private void TitleAssertRemoves(string dirtyTitle, string[] removedStrings)
        {
            string cleanTitle = new TitleCleaner().Clean(dirtyTitle);
            foreach (string badString in removedStrings)
            {
                AssertDoesNotContain(cleanTitle, badString);
            }
        }

        private void TitleAssertRemoves(string dirtyTitle, string removedString)
        {
            string cleanTitle = new TitleCleaner().Clean(dirtyTitle);
            AssertDoesNotContain(cleanTitle, removedString);
        }

        private void AssertDoesNotContain(string value, string substring)
        {
            Assert.IsFalse(value.Contains(substring));
        }
    }
}
