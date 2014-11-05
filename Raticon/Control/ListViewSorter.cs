using System.ComponentModel;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections;

namespace Raticon.Control
{
    public class ListViewSorter
    {
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        ListView listView;
        DefaultHeaderSortDirections defaultHeaderSortDirections;
        DataTemplate upArrow;
        DataTemplate downArrow;
        public ListViewSorter(ListView listView, DefaultHeaderSortDirections defaultHeaderSortDirections)
            : this(listView, defaultHeaderSortDirections, (DataTemplate)Application.Current.Resources["HeaderTemplateArrowUp"], (DataTemplate)Application.Current.Resources["HeaderTemplateArrowDown"])
        { }
        public ListViewSorter(ListView listView, DefaultHeaderSortDirections defaultHeaderSortDirections, DataTemplate upArrow, DataTemplate downArrow)
        {
            this.listView = listView;
            this.defaultHeaderSortDirections = defaultHeaderSortDirections;
            this.upArrow = upArrow;
            this.downArrow = downArrow;
        }

        public void OnGridViewColumnHeaderClick(RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;

            if (headerClicked == null || headerClicked.Content == null || headerClicked.Role == GridViewColumnHeaderRole.Padding)
            {
                return;
            }

            Sort(headerClicked);
        }

        private void Sort(GridViewColumnHeader headerClicked)
        {
            ListSortDirection direction = getDirection(headerClicked);
            SortData((string)headerClicked.Tag, direction);
            SetArrow(headerClicked, direction);

            _lastHeaderClicked = headerClicked;
            _lastDirection = direction;
        }

        private void SortData(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(listView.ItemsSource);

            dataView.SortDescriptions.Clear();
            dataView.SortDescriptions.Add(new SortDescription(sortBy, direction));
            dataView.Refresh();
        }

        private void SetArrow(GridViewColumnHeader headerClicked, ListSortDirection direction)
        {
            headerClicked.Column.HeaderTemplate = getArrow(direction);

            // Remove arrow from previously sorted header
            if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
            {
                _lastHeaderClicked.Column.HeaderTemplate = null;
            }
        }

        private DataTemplate getArrow(ListSortDirection direction)
        {
            return (direction == ListSortDirection.Ascending) ? upArrow : downArrow;
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
                return defaultHeaderSortDirections.DefaultDirection(headerClicked);
            }
        }
    }

    public class DefaultHeaderSortDirections
    {
        private string[] descendingHeaders;
        public DefaultHeaderSortDirections(string[] descendingHeaders)
        {
            this.descendingHeaders = descendingHeaders;
        }

        public ListSortDirection DefaultDirection(GridViewColumnHeader header)
        {
            var headerText = header.Content.ToString();
            if (descendingHeaders.Contains(headerText))
            {
                return ListSortDirection.Descending;
            }
            return ListSortDirection.Ascending;
        }
    }
}
