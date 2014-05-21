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
                Collection = new MediaCollection<GuiFilm>(dialog.FileName);
            }
        }

        public RelayCommand MakeIconsCommand { get; private set; }
        public void MakeIcons()
        {
            new IconService().ProcessCollection(Collection.Items);
        }

        public RelayCommand ClearThumbCacheCommand { get; private set; }
        public void ClearThumbCache()
        {
            new ShellService().Execute("cleanmgr","");
        }

        private IMediaCollection<IFilmFromFolder> collection;
        public IMediaCollection<IFilmFromFolder> Collection
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