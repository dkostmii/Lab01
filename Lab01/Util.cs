using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace Lab01
{
    class Util
    {
        public static string ReplaceHexSymbols(string text)
        {
            return Regex.Replace(text, "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]", "", RegexOptions.Compiled);
        }

        public static bool CheckContains(string country, string[] tags)
        {
            if (tags.Length > 0)
            {
                return tags.Contains(country);
            }

            throw new Exception("Expected tags to be non-empty array");
        }
    }
}
