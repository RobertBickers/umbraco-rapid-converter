using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Codetreehouse.RapidUmbracoConverter.Tools.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// takes a substring between two anchor strings (or the end of the string if that anchor is null)
        /// </summary>
        /// <param name="this">a string</param>
        /// <param name="from">an optional string to search after</param>
        /// <param name="until">an optional string to search before</param>
        /// <param name="comparison">an optional comparison for the search</param>
        /// <returns>a substring based on the search</returns>
        public static string Substring(this string @this, string from = null, string until = null, StringComparison comparison = StringComparison.InvariantCulture)
        {
            var fromLength = (from ?? string.Empty).Length;
            var startIndex = !string.IsNullOrEmpty(from)
                ? @this.IndexOf(from, comparison) + fromLength
                : 0;

            if (startIndex < fromLength) { throw new ArgumentException("from: Failed to find an instance of the first anchor"); }

            var endIndex = !string.IsNullOrEmpty(until)
            ? @this.IndexOf(until, startIndex, comparison)
            : @this.Length;

            if (endIndex < 0) { throw new ArgumentException("until: Failed to find an instance of the last anchor"); }

            var subString = @this.Substring(startIndex, endIndex - startIndex);
            return subString;
        }


        public static string FromCamelToWord(this string @this)
        {
            @this.FirstCharacterToUpper();

            return Regex.Replace(@this, "(\\B[A-Z])", " $1");
        }


        public static string FirstCharacterToUpper(this string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("Name has not been set on the UmbracoConversionObject");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }


        public static string FirstCharacterToLower(this string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("Name has not been set on the UmbracoConversionObject");
            return input.First().ToString().ToLower() + input.Substring(1);
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
