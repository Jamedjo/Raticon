using GalaSoft.MvvmLight;
using Raticon.Model;
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
            }
        }
        //string ProgressMessage [get ====> execute some callback?

        public IconProgressViewModel()
        {
        }
    }
}
