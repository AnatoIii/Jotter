using Jotter.Model;
using System.Windows;

namespace Jotter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginViewModel loginViewModel)
        {
            InitializeComponent();

            loginViewModel.SetWindow(this);
            DataContext = loginViewModel;
        }

        public FormStatus GetCloseStatus()
        {
            return ((LoginViewModel)DataContext).GetFormStatus();
        }
    }
}
