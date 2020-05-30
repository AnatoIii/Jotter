using System;
using System.Windows.Data;

namespace Jotter
{
    public class MultiParametes
    {
        public object Parameter1 { get; set; }
        public object Parameter2 { get; set; }
    }

	public class MultiParameterConverter : IMultiValueConverter
	{
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new MultiParametes { Parameter1 = values[0], Parameter2 = values[1] };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
