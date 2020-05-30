using Jotter.Model;
using System.Windows;

namespace Jotter.AddCategory
{
	/// <summary>
	/// Interaction logic for AddCategoryWindow.xaml
	/// </summary>
	public partial class AddCategoryWindow : Window
	{
		public AddCategoryWindow(AddCategoryViewModel viewModel)
		{
			viewModel.SetWindow(this);
			DataContext = viewModel;

			InitializeComponent();
		}

		public (FormStatus FormStatus, AddCategoryModel Category) GetCloseData()
		{
			return ((AddCategoryViewModel)DataContext).GetCloseStatus();
		}
	}
}
