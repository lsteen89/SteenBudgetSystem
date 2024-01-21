using SteenBudgetSystemLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SteenBudgetSystemLib.Helpers
{
    public static class StringUtilities
    {
        public static string RemoveCommas(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            // Remove commas from the start and end of the string
            input = Regex.Replace(input, @"^,*|,*$", "");

            // Replace two or more consecutive commas with a single comma
            input = Regex.Replace(input, @",{2,}", ",");

            // Keep only the first comma and remove the rest
            int firstCommaIndex = input.IndexOf(',');
            if (firstCommaIndex != -1)
            {
                input = input.Substring(0, firstCommaIndex + 1) + input.Substring(firstCommaIndex + 1).Replace(",", "");
            }

            return input;
        }
        public static (bool IsValid, string ErrorMessage) ValidatePartnerName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return (true, string.Empty);
            }

            string pattern = @"^\s*\p{L}+(?:\s+\p{L}+)*\s*$";
            if (!Regex.IsMatch(name, pattern))
            {
                return (false, "Name can only contain alphabetical characters and spaces.");
            }

            return (true, string.Empty);
        }
        public static bool IsValidInput(string input)
        {
            return string.IsNullOrWhiteSpace(input) || Regex.IsMatch(input, @"^[\d\s,]*$");
        }
    }
}
