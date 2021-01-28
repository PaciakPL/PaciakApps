using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace App.ZBP.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex _trimLinesRegex = new Regex(@"^\s+$[\r\n]*", RegexOptions.Multiline);
        public static string JustNumbers(this string data)
        {
            return Regex.Replace(data ?? string.Empty, @"[^0-9]*", string.Empty, RegexOptions.Multiline);
        }

        public static string TrimNewLines(this string data)
        {
            return _trimLinesRegex.Replace(data ?? string.Empty, string.Empty);
        }

        public static string GetFirstLine(this string data)
        {
            var stringReader = new StringReader(data);
            return stringReader.ReadLine();
        } 
    }
}