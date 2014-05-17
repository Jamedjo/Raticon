using GalaSoft.MvvmLight;
using Raticon.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raticon.ViewModel
{
    public class SearchResultPickerViewModel : ViewModelBase
    {
        private string searchText = "Try a different title...";
        public string SearchText { get { return searchText; } set { searchText = value; RaisePropertyChanged("SearchText"); } }
        public List<LookupResult> LookupResultList { get; private set; }
        public LookupResult SelectedItem { get; set; }

        public SearchResultPickerViewModel(List<LookupResult> results)
        {
            LookupResultList = results;
            //RaisePropertyChanged("LookupResultList");

            SelectedItem = LookupResultList.First();
        }
    }
}
