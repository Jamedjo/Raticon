using GalaSoft.MvvmLight;
using Raticon.Model;
using GalaSoft.MvvmLight.Command;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows;
using Raticon.Service;
using System.Collections.Generic;

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
                Collection = new DummyCollection().Items;
            }

            AddFolderCommand = new RelayCommand(AddFolder);
            MakeIconsCommand = new RelayCommand(MakeIcons);
        }

        public RelayCommand AddFolderCommand { get; private set; }
        public void AddFolder()
        {
            var dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            if (dialog.ShowDialog(Application.Current.MainWindow) == CommonFileDialogResult.Ok)
            {
                Collection = new MediaCollection<GuiFilm>(dialog.FileName).Items;
            }
        }

        public RelayCommand MakeIconsCommand { get; private set; }
        public void MakeIcons()
        {
            new GuiIconService(Application.Current.MainWindow).ProcessCollection(Collection);
        }


        private IEnumerable<IFilmFromFolder> collection;
        public IEnumerable<IFilmFromFolder> Collection
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