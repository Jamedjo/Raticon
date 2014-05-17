using Raticon.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Raticon.Service
{
    public class LookupResultPickerService
    {
        public LookupChoice LookupChoice { get; private set; }
        public LookupResultPickerService(List<LookupResult> results, Window parentWindow = null)
        {
            var popupWindow = new SearchResultPicker();
            popupWindow.DataContext = new SearchResultPickerViewModel(results);
            popupWindow.Owner = parentWindow;
            popupWindow.ShowDialog();

            if(popupWindow.DialogResult==true)
            {
                LookupChoice = new LookupChoice("what");
            }
            else
            {
                LookupChoice = new LookupChoice(LookupChoice.Action.GiveUp);
            }
        }
    }
}
