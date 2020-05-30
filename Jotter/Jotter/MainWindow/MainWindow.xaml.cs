using BL;
using System.Windows;

namespace Jotter.MainWindow
{
	/// <summary>
	/// Логика взаимодействия для MainForm.xaml
	/// </summary>
	public partial class MainForm : Window
	{
		public MainForm(IStorage storage)
		{
			DataContext = new MainViewModel(storage);

			InitializeComponent();
		}
	}
}
