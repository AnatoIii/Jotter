using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;

namespace Jotter
{
    public class LoginFormConverter : IMultiValueConverter
    {
        private Regex _emailRegex = new Regex("^[a-z0-9][.a-z0-9]*[a-z0-9]@[a-z0-9][-a-z0-9]*[a-z0-9][.][a-z0-9]{1,5}", RegexOptions.IgnoreCase);
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return _emailRegex.IsMatch(values[0].ToString());
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
