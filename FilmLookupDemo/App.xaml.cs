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
            string imdbId = new FilmLookupService().Lookup("Italian.Job", LookupCalllback);
            //string imdbId = LookupCalllback(new Raticon.Model.DummyLookupContext()).Run(retryText => { MessageBox.Show("Tried to retry with title '" + retryText + "'"); return retryText; });
            MessageBox.Show("imdbId selected was:\n\"" + imdbId + "\"");
            this.Shutdown();
        }

        static LookupChoice LookupCalllback(LookupContext lookup)
        {
            return new GuiResultPickerService(null).Pick(lookup);
        }
    }
}
