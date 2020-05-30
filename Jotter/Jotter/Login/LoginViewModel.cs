using BL;
using Jotter.Login;
using Jotter.Model;
using Model.DTO;
using System.Windows;
using System.Windows.Controls;

namespace Jotter
{
    public class LoginViewModel
    {
        public LoginModel Login { get; set; }
        public RegisterModel Register { get; set; }
        public string ErrorMessage { get; set; }

        private readonly IStorage _storage;
        private Window _window;
        private FormStatus _formStatus;

        private Command _loginCommand;
        public Command LoginCommand
        {
            get
            {
                return _loginCommand ??
                    (_loginCommand = new Command(obj =>
                    {
                        var password = (obj as PasswordBox).Password;
                        if (password.Length < 8)
                        {
                            MessageBox.Show("Password minimum length is 8 symbols!");
                            return;
                        }

                        var loginResponse = _storage.LogIn(new UserLoginModel { Email = Login.Email, Password = password });
                        if (loginResponse.IsSuccessful) {
                            _formStatus = FormStatus.ClosedDueToActions;
                            _window.Hide();
                        } else {
                            MessageBox.Show(loginResponse.ErrorMessage);
                        }
                    })
                );
            }
        }

        private Command _registerCommand;
        public Command RegisterCommand
        {
            get {
                return _registerCommand ??
                    (_registerCommand = new Command(obj => {
                        var passwordBoxes = (obj as MultiParametes);
                        var pass1 = (passwordBoxes.Parameter1 as PasswordBox).Password;
                        var pass2 = (passwordBoxes.Parameter1 as PasswordBox).Password;

                        if(pass1 != pass2) {
                            MessageBox.Show("Passwords should be the same!");
                            return;
                        }

                        if (pass1.Length < 8) {
                            MessageBox.Show("Password minimum length is 8 symbols!");
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(Register.Name)) {
                            MessageBox.Show("Name field is nessesary!");
                            return;
                        }

                        var registerResponse = _storage.Register(new UserRegisterModel { Email = Register.Email, Password = pass1, Name = Register.Name });

                        if (registerResponse.IsSuccessful) {
                            _formStatus = FormStatus.ClosedDueToActions;
                            _window.Hide();
                        } else {
                            MessageBox.Show(registerResponse.ErrorMessage);
                        }
                    })
                );
            }
        }

        public LoginViewModel(IStorage storage)
        {
            Login = new LoginModel();
            Register = new RegisterModel();

            _formStatus = FormStatus.ClosedManually;
            _storage = storage;
        }

        public void SetWindow(Window window)
        {
            _window = window;
        }

        public FormStatus GetFormStatus()
        {
            return _formStatus;
        }
    }
}
