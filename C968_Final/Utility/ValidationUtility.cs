using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace C968_Final.Utility
{
    public static class ValidationUtility
    {
        static ValidationUtility()
        {
            s_decimalRegex = new Regex(c_demicalPattern);
        }

        public static bool ValueIsDecimalFormat(string v) => s_decimalRegex.IsMatch(v);
        public static bool IsInRange(string x, string a, string b)
        {
            if (int.TryParse(x, out var xValue)
                && int.TryParse(a, out var aValue)
                && int.TryParse(b, out var bValue))
                return IsInRange(xValue, aValue, bValue);

            return false;
        }
        public static bool IsInRange(int x, int a, int b) => x >= a && x <= b;

        const string c_demicalPattern = @"^[0-9]+(\.[0-9]{1,2})?$";
        static Regex s_decimalRegex;

    }
}
