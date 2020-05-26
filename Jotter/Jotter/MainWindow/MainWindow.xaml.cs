using Model;
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

namespace Jotter.MainWindow
{
	/// <summary>
	/// Логика взаимодействия для MainForm.xaml
	/// </summary>
	public partial class MainForm : Window
	{
		public MainForm()
		{
			InitializeComponent();
			categoriesList.ItemsSource = new List<string> { "asd", "asd", "asd" };
			lVNotesData.ItemsSource = new List<Note> { new Note("Some data", "description", null)  };

			DataContext = new MainViewModel();
		}
	}
}
