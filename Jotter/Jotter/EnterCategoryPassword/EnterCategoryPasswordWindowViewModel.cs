using System.Windows;
using System.Windows.Controls;

namespace Jotter.EnterCategoryPassword
{
    public class EnterCategoryPasswordWindowViewModel
	{
		public string EnterPasswordText { get; set; }

        private string _categoryPassword { get; set; }
        private Window _window { get; set; }

        public EnterCategoryPasswordWindowViewModel(string categoryName)
		{
			EnterPasswordText = $"Enter password for category {categoryName}";
		}

        private Command _confirmPasswordCommand;
        public Command ConfirmPasswordCommand
        {
            get {
                return _confirmPasswordCommand ??
                    (_confirmPasswordCommand = new Command(obj => {
                        var password = (obj as PasswordBox).Password;
                        if (string.IsNullOrWhiteSpace(password)) {
                            MessageBox.Show("Password can't be empty!");
                            return;
                        }

                        _categoryPassword = password;
                        _window.Hide();
                    })
                );
            }
        }

        public void SetWindow(Window window)
        {
            _window = window;
        }

        public string GetCategoryPassword()
        {
            return _categoryPassword;
        }
    }
}
