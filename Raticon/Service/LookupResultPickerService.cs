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
        LookupChoice Pick(List<LookupResult> results);
    }

    public class FirstResultPicker : IResultPicker
    {
        public LookupChoice Pick(List<LookupResult> results)
        {
            return new LookupChoice(results.First());
        }
    }

    public class GuiResultPickerService : IResultPicker
    {
        private Window parentWindow;
        public GuiResultPickerService(Window parentWindow)
        {
            this.parentWindow = parentWindow;
        }

        public LookupChoice Pick(List<LookupResult> results)
        {
            SearchResultPicker picker = new SearchResultPicker();
            picker.DataContext = new SearchResultPickerViewModel(results);
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
