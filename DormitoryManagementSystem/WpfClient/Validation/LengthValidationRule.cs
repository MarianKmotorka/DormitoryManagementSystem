using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;

namespace WpfClient.Validation
{
    public class LengthValidationRule : ValidationRule
    {
        public int Min { get; set; } = 1;

        public int Max { get; set; } = 100;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null) return ValidationResult.ValidResult;

            var length = value.ToString().Length;

            if (length < Min)
                return new ValidationResult(false, IoC.Get<ResourceDictionary>("language")["MinLength"].ToString() + Min);

            if (length > Max)
                return new ValidationResult(false, IoC.Get<ResourceDictionary>("language")["MaxLength"].ToString() + Max);

            return ValidationResult.ValidResult;
        }
    }
}
