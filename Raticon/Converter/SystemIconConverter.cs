using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Raticon.Converter
{
    [ValueConversion(typeof(string), typeof(BitmapSource))]
    public class SystemIconConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, CultureInfo culture)
        {
            Icon icon = (Icon)typeof(SystemIcons).GetProperty(parameter.ToString(), BindingFlags.Public | BindingFlags.Static).GetValue(null, null);
            BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            return bs;
        }

        public object ConvertBack(object value, Type type, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
