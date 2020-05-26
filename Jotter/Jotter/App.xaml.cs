using BL;
using Jotter.MainWindow;
using Jotter.Model;
using Microsoft.Extensions.DependencyInjection;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Jotter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IKernel _kernel;

        public App()
        {
            _kernel = new StandardKernel(new MainModule());

            StartApp();
        }

        private void StartApp()
        {
            var storage = _kernel.GetRequiredService<IStorage>();

            if (!storage.IsUserLoggedIn()) {
                if (ShowLogInForm(storage) == FormStatus.ClosedManually) {
                    return;
                }
            }

            var mainWindow = _kernel.GetRequiredService<MainForm>();
            mainWindow.ShowDialog();
        }

        private FormStatus ShowLogInForm(IStorage storage)
        {
            var loginWindow = _kernel.GetRequiredService<LoginWindow>();
            loginWindow.ShowDialog();
            if (!storage.IsUserLoggedIn()) {
                return FormStatus.ClosedManually;
            }

            return FormStatus.ClosedDueToActions;
        }
    }
}
