using System;
using System.Globalization;
using System.Windows.Data;

namespace TodoList.Converters
{
    class BoolToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return (bool)value ? 0 : 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is int index)
                return index == 0;
            return true;
        }

    }
}
