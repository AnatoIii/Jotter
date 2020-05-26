using BL;
using Model.DTO;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Jotter
{
    public class LoginViewModel
    {
        public LoginModel Login { get; set; }
        public string ErrorMessage { get; set; }

        private readonly IStorage _storage;
        private Window _window;

        private Command loginCommand;
        public Command LoginCommand
        {
            get
            {
                return loginCommand ??
                    (loginCommand = new Command(obj =>
                    {
                        var password = (obj as PasswordBox).Password;
                        if (password.Length < 8)
                        {
                            MessageBox.Show("Password minimum length is 8 symbols!");
                            return;
                        }

                        var loginResponse = _storage.LogIn(new UserLoginModel { Email = Login.Email, Password = password });
                        if (loginResponse.IsSuccessful) {
                            _window.Close();
                        } else {
                            ErrorMessage = loginResponse.ErrorMessage;
                        }
                    })
                );
            }
        }

        public LoginViewModel(IStorage storage)
        {
            Login = new LoginModel() { Email = "123" };

            _storage = storage;
        }

        public void SetWindow(Window window)
        {
            _window = window;
        }
    }
}
