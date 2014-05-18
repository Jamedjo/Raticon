using Raticon.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public GuiResultPickerService(Window parentWindow)
        {
            this.parentWindow = parentWindow;
        }

        public LookupChoice Pick(LookupContext lookup)
        {
            SearchResultPicker picker = new SearchResultPicker();
            picker.DataContext = new SearchResultPickerViewModel(lookup);
            picker.Owner = parentWindow;
            picker.ShowDialog();

            if(picker.DialogResult==true)
            {
                return ((SearchResultPickerViewModel)picker.DataContext).LookupChoice;
            }
            else
            {
                return new LookupChoice(LookupChoice.Action.GiveUp);
            }
        }
    }
}
