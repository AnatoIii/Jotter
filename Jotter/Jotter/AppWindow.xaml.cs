using BL;
using Jotter.MainWindow;
using Jotter.Model;
using Microsoft.Extensions.DependencyInjection;
using Ninject;
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

namespace Jotter
{
	/// <summary>
	/// Interaction logic for AppWindow.xaml
	/// </summary>
	public partial class AppWindow : Window
    {
        private readonly IKernel _kernel;

        public AppWindow()
		{
			InitializeComponent();
			this.Hide();

            _kernel = new StandardKernel(new MainModule());

            StartApp();

        }

        private void StartApp()
        {
            var storage = _kernel.GetRequiredService<IStorage>();

            if (!storage.IsUserLoggedIn()) {
                var loginFormResult = ShowLogInForm();
                if (loginFormResult == FormStatus.ClosedManually) {
                    this.Close();
                }
            }

            var mainWindow = _kernel.GetRequiredService<MainForm>();
            mainWindow.ShowDialog();
            this.Close();
        }

        private FormStatus ShowLogInForm()
        {
            var loginWindow = _kernel.GetRequiredService<LoginWindow>();
            loginWindow.ShowDialog();

            var windowStatus = loginWindow.GetCloseStatus();

            loginWindow.Close();
            return windowStatus;
        }
    }
}
