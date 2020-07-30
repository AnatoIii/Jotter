using System.Windows;

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
			viewModel.SetWindow(this);
			DataContext = viewModel;
		}

		public string GetCategoryPassword()
		{
			return ((EnterCategoryPasswordWindowViewModel)DataContext).GetCategoryPassword();
		}
	}
}
