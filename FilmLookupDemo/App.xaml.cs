using Raticon.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FilmLookupDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private GuiResultPickerService resultPicker;
        public async void Application_Startup(object sender, StartupEventArgs e)
        {
            resultPicker = new GuiResultPickerService(null);

            MessageBoxResult demoType = MessageBox.Show("Would you like to demo with real results?", "Pick Http demo / Local Demo", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            string imdbId = (demoType == MessageBoxResult.Yes) ? await RemoteLookup() : await LocalLookup();
            MessageBox.Show("imdbId selected was:\n\"" + imdbId + "\"");
            this.Shutdown();
        }

        private LookupChoice LookupCalllback(LookupContext lookup)
        {
            return resultPicker.Pick(lookup);
        }

        private Task<string> LocalLookup()
        {
            return Task.Factory.StartNew(() => LookupCalllback(new Raticon.Model.DummyLookupContext()).Run(retryText => { MessageBox.Show("Tried to retry with title '" + retryText + "'"); return retryText; }));
        }

        private Task<string> RemoteLookup()
        {
            return new FilmLookupService().LookupAsync("Italian.Job", LookupCalllback);
        }
    }
}
