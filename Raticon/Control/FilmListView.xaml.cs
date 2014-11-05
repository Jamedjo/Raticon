using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Input;

namespace Raticon.Control
{
    /// <summary>
    /// Interaction logic for FilmListView.xaml
    /// </summary>
    public partial class FilmListView : UserControl
    {
        ListViewSorter listViewSorter;
        public FilmListView()
        {
            InitializeComponent();
            listViewSorter = new ListViewSorter(FilmList, new DefaultHeaderSortDirections(new[] { "Rating", "Year" }));
        }

        void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as Raticon.Model.IFilmFromFolder;
            if (item != null)
            {
                new Raticon.Service.ShellService().Execute("explorer " + item.Path);
            }
        }

        void SortClickHandler(object sender, RoutedEventArgs e)
        {
            listViewSorter.OnGridViewColumnHeaderClick(e);
        }

    }
}
