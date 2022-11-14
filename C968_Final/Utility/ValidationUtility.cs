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

        const string c_demicalPattern = @"^[0-9]+(\.[0-9]{1,2})?$";
        static Regex s_decimalRegex;

    }
}
