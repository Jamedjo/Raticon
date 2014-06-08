using Raticon.ViewModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Raticon.Service
{
    public interface IResultPicker
    {
        LookupChoice Pick(LookupContext lookup);
    }

    public class FirstResultPicker : IResultPicker
    {
        public LookupChoice Pick(LookupContext lookup)
        {
            LookupResult pick = lookup.Results.FirstOrDefault();
            return (pick != null) ? new LookupChoice(pick) : new LookupChoice(LookupChoice.Action.GiveUp);
        }
    }

    public class GuiResultPickerService : IResultPicker
    {
        private Window parentWindow;
        private TaskScheduler uiTaskScheduler;
        private Thread uiThread;

        private static ConcurrentQueue<Task<LookupChoice>> queue = new ConcurrentQueue<Task<LookupChoice>>();

        private static bool isPickerVisible = false;

        public GuiResultPickerService(Window parentWindow)
        {
            this.parentWindow = parentWindow;

            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                this.uiTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
                this.uiThread = Thread.CurrentThread;
            }
            else
            {
                throw new ThreadStateException("GuiResultPickerService must be instantiated from STA thread");
            }
        }

        public LookupChoice Pick(LookupContext lookup)
        {
            if(uiThread == Thread.CurrentThread)
            {
                throw new ThreadStateException("Pick should be called from another thread");
            }
            var guiTask = EnqueuePick(lookup);
            guiTask.Wait();
            return guiTask.Result;
        }

        private LookupChoice ShowPicker(LookupContext lookup)
        {
            SearchResultPicker picker = new SearchResultPicker();
            picker.DataContext = new SearchResultPickerViewModel(lookup);
            picker.Owner = parentWindow;
            picker.ShowDialog();

            isPickerVisible = false;

            if(picker.DialogResult==true)
            {
                return ((SearchResultPickerViewModel)picker.DataContext).LookupChoice;
            }
            else
            {
                return new LookupChoice(LookupChoice.Action.GiveUp);
            }
        }

        private async Task<LookupChoice> EnqueuePick(LookupContext lookup)
        {
            Task<LookupChoice> task = new Task<LookupChoice>(()=> ShowPicker(lookup),CancellationToken.None, TaskCreationOptions.AttachedToParent);
            queue.Enqueue(task);

            //If task is the only item in queue we need to run it immediately
            TryDequeueNextPick();

            var choice = await task;

            //After dialog closes, continue by attempting to open next picker
            TryDequeueNextPick();

            return choice;
        }

        private void TryDequeueNextPick()
        {
            Console.WriteLine("isPickerVisible=" + isPickerVisible);
            if(isPickerVisible)
            {
                return;
            }

            Task<LookupChoice> lookupTask;
            if (queue.TryDequeue(out lookupTask))
            {
                isPickerVisible = true;
                lookupTask.Start(uiTaskScheduler);
            }
        }

    }
}
