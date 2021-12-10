using System;
using System.Text.RegularExpressions;
using System.Linq;

using Lab01.Analysis;

namespace Lab01
{
    class Util
    {
        public static string ReplaceHexSymbols(string text)
        {
            return Regex.Replace(text, "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]", "", RegexOptions.Compiled);
        }
    }
}
