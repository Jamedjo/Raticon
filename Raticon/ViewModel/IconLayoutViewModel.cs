using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Raticon.ViewModel
{
    public class IconLayoutViewModel : ViewModelBase
    {
        public ImageSource FolderJpg { get; set; }
        public string Rating { get; set; }

        public IconLayoutViewModel(string folderJpgPath, string Rating)
        {
            this.Rating = Rating;
            this.FolderJpg = (ImageSource)new ImageSourceConverter().ConvertFromString(folderJpgPath);
        }
    }
}
