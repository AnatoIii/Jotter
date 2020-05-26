using Jotter.MainWindow.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Jotter.MainWindow
{
	public class MainWindowDataVisibilityConverter : IValueConverter
	{
		public MainWindowDataVisibilityConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is MainWindowStates state)) {
				throw new ArgumentException("Got not a MainWindowStates enum instance");
			}
			
			return (state, parameter as string) switch
			{
				(MainWindowStates.NotesShowing, "notesList")	=> Visibility.Visible,
				(MainWindowStates.NoteEditing, "noteEditing")	=> Visibility.Visible,
				(MainWindowStates.NoteViewing, "noteViewing")			=> Visibility.Visible,
				_												=> Visibility.Hidden
			};
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
