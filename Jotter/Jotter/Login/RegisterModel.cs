using Jotter.Valdators;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Jotter.Login
{
	public class RegisterModel : ValidatorBase, INotifyPropertyChanged
    {
        private Regex _emailRegex = new Regex("^[a-z0-9][.a-z0-9]*[a-z0-9]@[a-z0-9][-a-z0-9]*[a-z0-9][.][a-z0-9]{1,5}", RegexOptions.IgnoreCase);
        private string _email;

        public string Email
        {
            get { return _email; }
            set {
                _email = value;
                OnPropertyChanged();
            }
        }
        public string Name { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public override ValidationResult Validate(string columnName)
        {
            switch (columnName) {
                case "Email":
                    var matches = Email != null && _emailRegex.IsMatch(Email);
                    return new ValidationResult(matches, !matches ? "Enter valid email" : null);
            }
            return new ValidationResult(true, null);
        }
    }
}
