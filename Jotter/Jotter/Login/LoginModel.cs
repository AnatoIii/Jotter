using Jotter.Valdators;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Jotter
{
    public class LoginModel: ValidatorBase, INotifyPropertyChanged
    {
        private Regex _emailRegex = new Regex("^[a-z0-9][.a-z0-9]*[a-z0-9]@[a-z0-9][-a-z0-9]*[a-z0-9][.][a-z0-9]{1,5}", RegexOptions.IgnoreCase);
        private string email;
        public string Password { get; set; }

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public override ValidationResult Validate(string columnName)
        {
            switch(columnName)
            {
                case "Email":
                    var matches = email != null && _emailRegex.IsMatch(email);
                    return new ValidationResult(matches, !matches ? "Enter valid email" : null);
            }
            return new ValidationResult(true, null);
        }
    }
}
