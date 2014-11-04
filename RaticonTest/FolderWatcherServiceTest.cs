using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raticon.Service;
using Raticon.Model;

namespace RaticonTest
{
    [TestClass]
    public class FolderWatcherTest
    {
        string watchPath, filmPath;
        IFolderWatcher watcher;
        [TestInitialize()]
        public void FolderWatcher_TestInitialize()
        {
            watchPath = @"C:\Temp\RaticonWatch";
            System.IO.Directory.CreateDirectory(watchPath);
            filmPath = System.IO.Path.Combine(watchPath, "Italian Job");
        }

        [TestCleanup()]
        public void FolderWatcher_TestCleanup()
        {
            watcher.Stop();
            System.IO.Directory.Delete(watchPath, true);
        }

        void WatchAction(Action action)
        {
            watcher.Watch(watchPath);
            action();
            for (int i = 0; i < 5; i++) System.Threading.Thread.Sleep(1);
        }

        void AssertWatchTriggeredBy(Action action)
        {
            bool called = false;
            watcher = new FolderWatcher(path => called = true);
            WatchAction(action);
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void FolderWatcher_reacts_to_directory_created()
        {
            AssertWatchTriggeredBy(() => System.IO.Directory.CreateDirectory(filmPath));
        }

        [TestMethod]
        public void FolderWatcher_reacts_to_folder_rename()
        {
            string preExistingFilmPath = System.IO.Path.Combine(watchPath, "New Folder");
            System.IO.Directory.CreateDirectory(preExistingFilmPath);

            AssertWatchTriggeredBy(() => System.IO.Directory.Move(preExistingFilmPath, filmPath));
        }
    }
    
    [TestClass]
    public class IconMakingFilmFolderWatcherTest
    {
        [TestMethod]
        public void IconMakingFilmFolderWatcher_calls_icon_service_with_film()
        {
            IFilmFromFolder lastProcessed = null;
            var filmProcessor = new MockFilmProcessor(f => lastProcessed = f);
            Func<Action<string>, IFolderWatcher> watcherFactory = action => new MockFolderWatcher(action);
            MockFolderWatcher watcher = (MockFolderWatcher)new IconMakingFilmFolderWatcher(path => new FilmMock(path), watcherFactory, filmProcessor).Watcher;
            watcher.TriggerChange(@"Z:\mock\path");
            Assert.AreEqual(@"Z:\mock\path", lastProcessed.Path);
        }

        class MockFolderWatcher : IFolderWatcher
        {
            Action<string> onChange;
            internal MockFolderWatcher(Action<string> onChange)
            {
                this.onChange = onChange;
            }

            void IFolderWatcher.Watch(string path)
            {
                throw new NotImplementedException();
            }

            void IFolderWatcher.Stop()
            {
                throw new NotImplementedException();
            }

            internal void TriggerChange(string s)
            {
                onChange(s);
            }
        }

        class MockFilmProcessor : AbstractFilmProcessor
        {
            Action<IFilmFromFolder> onProcess;
            public MockFilmProcessor(Action<IFilmFromFolder> onProcess)
            {
                this.onProcess = onProcess;
            }

            public override void Process(IFilmFromFolder film)
            {
                onProcess(film);
            }
        }

    }
}
