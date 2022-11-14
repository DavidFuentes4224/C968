using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace C968_Final.Utility
{
    public class IsNumericValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string inputValue)
                return ValidateCore(inputValue);
            else
                return new ValidationResult(false, $"{nameof(value)} is not a valid input type.");
        }

        private ValidationResult ValidateCore(string value)
        {
            if (int.TryParse(value, out var result))
                return ValidationResult.ValidResult;

            return new ValidationResult(false, "This field only accepts whole numbers.");
        }
    }
}
