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
            //string imdbId = LookupCalllback(new Raticon.Model.DummyResults()).Run(retryText => { MessageBox.Show("Tried to retry with title '" + retryText + "'"); return retryText; });
            MessageBox.Show("imdbId selected was:\n\"" + imdbId + "\"");
            this.Shutdown();
        }

        static LookupChoice LookupCalllback(List<LookupResult> results)
        {
            return new LookupResultPickerService(results).LookupChoice;
        }
    }
}
