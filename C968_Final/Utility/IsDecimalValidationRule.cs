using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace C968_Final.Utility
{
    public class IsDecimalValidationRule : ValidationRule
    {
        public IsDecimalValidationRule()
        {
            m_regex = new Regex(c_demicalRegex);
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string inputValue)
                return ValidateCore(inputValue);
            else
                return new ValidationResult(false, $"{nameof(value)} is not a valid input type.");
        }

        private ValidationResult ValidateCore(string value)
        {
            if (int.TryParse(value, out var result) || m_regex.IsMatch(value))
                return ValidationResult.ValidResult;

            return new ValidationResult(false, "This field only accepts floats.");
        }

        const string c_demicalRegex = @"^[0-9]+\.[0-9][0-9]$";

        Regex m_regex;
    }
}
