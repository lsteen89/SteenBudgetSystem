using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteenBudgetSystemLib.Helpers
{
    public class SteenBudgetSystemHelper
    {
        public static decimal ConvertToDecimal(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return 0m; // Return 0 for null or whitespace input
            }

            input = input.Trim(); // Trim any leading or trailing whitespace

            if (decimal.TryParse(input, out decimal result))
            {
                return result; // Return the parsed decimal value
            }

            return 0m; // Return 0 if parsing fails
        }
    }
}
