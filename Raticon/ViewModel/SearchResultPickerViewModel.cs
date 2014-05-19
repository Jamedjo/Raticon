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
        private string searchText;
        public string SearchText { get { return searchText; } set { searchText = value; RaisePropertyChanged("SearchText"); } }
        private string queryTitle;
        public string QueryTitle { get { return queryTitle; } set { queryTitle = value; RaisePropertyChanged("QueryTitle"); } }


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

        public SearchResultPickerViewModel(LookupContext lookup)
        {
            LookupResultList = lookup.Results;
            //RaisePropertyChanged("LookupResultList");

            QueryTitle = lookup.Query;

            SearchCommand = new RelayCommand(RetrySearch);
            PickSelectedCommand = new RelayCommand(PickSelected);

            SelectedItem = LookupResultList.FirstOrDefault();
        }
    }
}
