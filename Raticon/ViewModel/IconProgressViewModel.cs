using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Raticon.Model;
using Raticon.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raticon.ViewModel
{
    public class IconProgressViewModel : ViewModelBase
    {
        private int progressPercentage = 0;
        public int ProgressPercentage
        {
            get { return progressPercentage; }
            set
            {
                progressPercentage = value;
                RaisePropertyChanged("ProgressPercentage");
                RaisePropertyChanged("ProgressMessage");
                RaisePropertyChanged("IsCompleted");
                RaisePropertyChanged("WindowTitle");
            }
        }

        public string CompletionMessage { get { return "Finished processing icons.\n\nYou may need to clear the Windows Thumbnail Cache and restart Windows Explorer for all icons to update."; } }

        public string ProgressMessage
        {
            get { return "Processing: " + ProgressPercentage + "% of folders have been processed and icons added."; }
        }

        public bool IsCompleted { get { return ProgressPercentage == 100; } }

        public string WindowTitle { get { return IsCompleted ? "Finished Building Icons" : "Building Icons..."; } }

        public RelayCommand ClearThumbCacheCommand { get; private set; }
        public void ClearThumbCache()
        {
            new ShellService().Execute("cleanmgr", "");
        }

        public IconProgressViewModel()
        {
            ClearThumbCacheCommand = new RelayCommand(ClearThumbCache);
        }
    }
}
