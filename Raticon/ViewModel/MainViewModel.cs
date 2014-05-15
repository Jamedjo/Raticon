using System.Linq;
using GalaSoft.MvvmLight;
using Raticon.Model;
using GalaSoft.MvvmLight.Command;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows;
using Raticon.Service;

namespace Raticon.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                Collection = new DummyCollection();
            }

            AddFolderCommand = new RelayCommand(AddFolder);
            MakeIconsCommand = new RelayCommand(MakeIcons);
            ClearThumbCacheCommand = new RelayCommand(ClearThumbCache);
        }

        public RelayCommand AddFolderCommand { get; private set; }
        public void AddFolder()
        {
            var dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            if (dialog.ShowDialog(Application.Current.MainWindow) == CommonFileDialogResult.Ok)
            {
                Collection = new MediaCollection(dialog.FileName);
            }
        }

        public RelayCommand MakeIconsCommand { get; private set; }
        public void MakeIcons()
        {
            var validFilms = Collection.Items.Where(f => !string.IsNullOrWhiteSpace(f.Rating));
            foreach (IFilm film in validFilms)
            {
                new IconService().Process(film);
            }
            MessageBox.Show("Complete!\n\n" + validFilms.Count() + " folders have been processed and icons added.", "Complete!");
        }

        public RelayCommand ClearThumbCacheCommand { get; private set; }
        public void ClearThumbCache()
        {
            new ShellService().Execute("cleanmgr","");
        }

        private IMediaCollection collection;
        public IMediaCollection Collection
        {
            get { return collection; }
            set
            {
                collection = value;
                RaisePropertyChanged("Collection");
            }
        }
    }
}