using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Raticon.Control
{
    /// <summary>
    /// Interaction logic for FilmListView.xaml
    /// </summary>
    public partial class FilmListView : UserControl
    {
        public FilmListView()
        {
            InitializeComponent();
        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        void SortClickHandler(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;

            if (headerClicked == null || headerClicked.Role == GridViewColumnHeaderRole.Padding)
            {
                return;
            }

            ListSortDirection direction = getDirection(headerClicked);

            Sort((string)headerClicked.Tag, direction);

            headerClicked.Column.HeaderTemplate = getArrow(direction);

            // Remove arrow from previously sorted header
            if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
            {
                _lastHeaderClicked.Column.HeaderTemplate = null;
            }

            _lastHeaderClicked = headerClicked;
            _lastDirection = direction;
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(FilmList.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        private DataTemplate getArrow(ListSortDirection direction)
        {
            return (DataTemplate)((direction == ListSortDirection.Ascending) ? Resources["HeaderTemplateArrowUp"] : Resources["HeaderTemplateArrowDown"]);
        }

        private ListSortDirection getDirection(GridViewColumnHeader headerClicked)
        {
            if (headerClicked == _lastHeaderClicked)
            {
                //Reverse direction
                return (_lastDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
            }
            else
            {
                return defaultDirection(headerClicked);
            }
        }

        private ListSortDirection defaultDirection(GridViewColumnHeader header)
        {
            switch(header.Content.ToString())
            {
                case "Rating":
                case "Year":
                    return ListSortDirection.Descending;
                case "Title":
                default:
                    return ListSortDirection.Ascending;
            }
        }

    }
}
