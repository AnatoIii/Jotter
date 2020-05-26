using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;

namespace Jotter.Valdators
{
    public abstract class ValidatorBase : IDataErrorInfo
    {
        public abstract ValidationResult Validate(string columnName);

        public string this[string columnName]
        {
            get
            {
                var result = Validate(columnName);
                return result.IsValid ? null : result.ErrorContent.ToString();
            }
        }

        public string Error
        {
            get
            {
                //var result = Validate("error");
                //return result.IsValid ? null : result.ErrorContent.ToString();
                return "No";
            }
        }
    }
}
