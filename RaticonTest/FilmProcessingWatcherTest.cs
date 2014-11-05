using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Model;
using Raticon.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaticonTest
{
    [TestClass]
    public class FilmProcessingWatcherTest
    {
        [TestMethod]
        public void IconMakingFilmFolderWatcher_calls_icon_service_with_film()
        {
            IFilmFromFolder lastProcessed = null;
            var filmProcessor = new MockFilmProcessor(f => lastProcessed = f);
            Func<Action<string>, IFolderWatcher> watcherFactory = action => new MockFolderWatcher(action);
            MockFolderWatcher watcher = (MockFolderWatcher)new FilmProcessingWatcher<FilmMock>(watcherFactory, filmProcessor).Watcher;
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
