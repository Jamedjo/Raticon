﻿using Raticon.Control;
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
using System.Windows.Shapes;

namespace Raticon
{
    /// <summary>
    /// Interaction logic for SearchResultPicker.xaml
    /// </summary>
    public partial class SearchResultPicker : Window
    {
        ListViewSorter listViewSorter;
        public SearchResultPicker()
        {
            InitializeComponent();
            listViewSorter = new ListViewSorter(SearchResults, new DefaultHeaderSortDirections(new[] { "Rating", "Year" }));
        }

        void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as Raticon.Service.LookupResult;
            if (item != null)
            {
                PickButton.Command.Execute(null);
            }
        }

        void SortClickHandler(object sender, RoutedEventArgs e)
        {
            listViewSorter.OnGridViewColumnHeaderClick(e);
        }
    }
}
