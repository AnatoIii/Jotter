using Jotter.AddCategory;
using Jotter.Model;
using System.Windows;
using System.Windows.Controls;

namespace Jotter
{
    public class AddCategoryViewModel
    {
        public AddCategoryModel AddCategory { get; set; }

        private AddCategoryModel _addCategoryResponseData { get; set; }
        private FormStatus _formStatus { get; set; }

        public string ErrorMessage { get; set; }

        private Window _window;

        private Command _addCategoryCommand;
        public Command AddCategoryCommand
        {
            get
            {
                return _addCategoryCommand ??
                    (_addCategoryCommand = new Command(obj =>
                    {
                        if(AddCategory.Name.Length < 3) {
                            MessageBox.Show("Minimum category name length is 3 symbols!");
                            return;
                        }

                        var password = (obj as PasswordBox).Password;
                        if (!string.IsNullOrEmpty(password) && password.Length < 8)
                        {
                            MessageBox.Show("Password minimum length is 8 symbols!");
                            return;
                        }

                        _addCategoryResponseData = new AddCategoryModel {
                            Name = AddCategory.Name,
                            Password = password
                        };
                        _formStatus = FormStatus.ClosedDueToActions;

                        _window.Close();
                    })
                );
            }
        }

        private Command _cancelCommand;
        public Command CancelCommand
        {
            get {
                return _cancelCommand ??
                    (_cancelCommand = new Command(obj => {
                        _window.Close();
                    })
                );
            }
        }

        public AddCategoryViewModel()
        {
            AddCategory = new AddCategoryModel();
            _formStatus = FormStatus.ClosedManually;
        }

        public void SetWindow(Window window)
        {
            _window = window;
        }

        public (FormStatus, AddCategoryModel) GetCloseStatus()
        {
            return (_formStatus, _addCategoryResponseData);
        }
    }
}
