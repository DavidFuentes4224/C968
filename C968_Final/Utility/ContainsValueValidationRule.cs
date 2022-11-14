using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace C968_Final.Utility
{
    public class ContainsValueValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string stringValue)
                return ValidateCore(stringValue.Trim());
            else
                return new ValidationResult(false, $"{nameof(value)} is not a valid input type.");
        }

        public ValidationResult ValidateCore(string value)
        {
            return value.Length > 0 ? ValidationResult.ValidResult : new ValidationResult(false, "This field requires a value.");
        }
    }
}
