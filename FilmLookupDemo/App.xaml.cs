using Raticon.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace FilmLookupDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void Application_Startup(object sender, StartupEventArgs e)
        {
            MessageBoxResult demoType = MessageBox.Show("Would you like to demo with real results?", "Pick Http demo / Local Demo", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            string imdbId = (demoType == MessageBoxResult.Yes) ? RemoteLookup() : LocalLookup();
            MessageBox.Show("imdbId selected was:\n\"" + imdbId + "\"");
            this.Shutdown();
        }

        private LookupChoice LookupCalllback(LookupContext lookup)
        {
            return new GuiResultPickerService(null).Pick(lookup);
        }

        private string LocalLookup()
        {
            return LookupCalllback(new Raticon.Model.DummyLookupContext()).Run(retryText => { MessageBox.Show("Tried to retry with title '" + retryText + "'"); return retryText; });
        }

        private string RemoteLookup()
        {
            return new FilmLookupService().Lookup("Italian.Job", LookupCalllback);
        }
    }
}
