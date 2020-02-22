using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;

namespace WpfClient.Validation
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return value == null || value.ToString().Length > 0
                ? ValidationResult.ValidResult
                : new ValidationResult(false, IoC.Get<ResourceDictionary>("language")["Required"]);
        }
    }
}
