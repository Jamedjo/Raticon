using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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

        public LookupChoice LookupChoice { get; private set; }

        private bool? dialogResult;
        public bool? DialogResult { get { return dialogResult; } set { dialogResult = value; RaisePropertyChanged("DialogResult"); } }

        public RelayCommand SearchCommand { get; private set; }
        public void RetrySearch()
        {
            LookupChoice = new LookupChoice(LookupChoice.Action.NewSearch, SearchText);
            DialogResult = true;
        }

        public RelayCommand PickSelectedCommand { get; private set; }
        public void PickSelected()
        {
            LookupChoice = new LookupChoice(SelectedItem.ImdbId);
            DialogResult = true;
        }

        public SearchResultPickerViewModel(List<LookupResult> results)
        {
            LookupResultList = results;
            //RaisePropertyChanged("LookupResultList");

            SearchCommand = new RelayCommand(RetrySearch);
            PickSelectedCommand = new RelayCommand(PickSelected);

            SelectedItem = LookupResultList.FirstOrDefault();
        }
    }
}
