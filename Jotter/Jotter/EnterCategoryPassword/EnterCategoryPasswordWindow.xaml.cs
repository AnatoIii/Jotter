using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Jotter.EnterCategoryPassword
{
	/// <summary>
	/// Interaction logic for EnterCategoryPasswordWindow.xaml
	/// </summary>
	public partial class EnterCategoryPasswordWindow : Window
	{
		public EnterCategoryPasswordWindow(EnterCategoryPasswordWindowViewModel viewModel)
		{
			InitializeComponent();

			DataContext = viewModel;
		}

		public string GetCategoryPassword()
		{
			return ((EnterCategoryPasswordWindowViewModel)DataContext).GetCategoryPassword();
		}
	}
}
