using System;
using System.Linq;

namespace DreamBuilders
{
    public static class StringEtensions
    {
        /// <summary>
        /// Reverse a String
        /// </summary>
        /// <param name="str">The string to Reverse</param>
        /// <returns>The reversed String</returns>
        public static string Reverse(this string str)
        {
            char[] array = str.ToCharArray();
            Array.Reverse(array);

            return new string(array);
        }

        public static int WordCount(this string str) =>
            str.Split(new char[]
            { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;

        public static bool IsValidString(this string str, char[] prohibitedChars, short maxChar = 0) =>
            (maxChar <= 0 || str.Length <= maxChar) && !string.IsNullOrEmpty(str) &&
            str.All(inputChar => prohibitedChars.All(prohibitedChar => inputChar != prohibitedChar));

        public static int ToInt(this string str)
        {
            if (!int.TryParse(str, out int result))
                result = 0;

            return result;
        }

        public static float ToFloat(this string str)
        {
            if (!float.TryParse(str, out float result))
                result = 0;

            return result;
        }

        public static T ToEnum<T>(this string str, bool ignorecase = true)
        {
            if (str == null)
                throw new ArgumentNullException("value");

            str = str.Trim();

            if (str.Length == 0)
                throw new ArgumentException("Must specify valid information for parsing in the string.", "value");

            Type t = typeof(T);

            return !t.IsEnum
                ? throw new ArgumentException("Type provided must be an Enum.", "T")
                : (T)Enum.Parse(t, str, ignorecase);
        }

        /// <summary>Checks if a string is Null or white space</summary>
        public static bool IsNullOrWhiteSpace(this string val) => string.IsNullOrWhiteSpace(val);

        /// <summary>Checks if a string is Null or empty</summary>
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        /// <summary>Checks if a string contains null, empty or white space.</summary>
        public static bool IsBlank(this string val) => val.IsNullOrWhiteSpace() || val.IsNullOrEmpty();

        /// <summary>Checks if a string is null and returns an empty string if it is.</summary>
        public static string OrEmpty(this string val) => val ?? string.Empty;

        /// <summary>
        /// Shortens a string to the specified maximum length. If the string's length
        /// is less than the maxLength, the original string is returned.
        /// </summary>
        public static string Shorten(this string val, int maxLength)
        {
            if (val.IsBlank()) return val;

            return val.Length <= maxLength ? val : val.Substring(0, maxLength);
        }

        /// <summary>Slices a string from the start index to the end index.</summary>
        /// <result>The sliced string.</result>
        public static string Slice(this string val, int startIndex, int endIndex)
        {
            if (val.IsBlank()) { throw new ArgumentNullException(nameof(val), "Value cannot be null or empty."); }

            if (startIndex < 0 || startIndex > val.Length - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            // If the end index is negative, it will be counted from the end of the string.
            endIndex = endIndex < 0 ? val.Length + endIndex : endIndex;

            if (endIndex < 0 || endIndex < startIndex || endIndex > val.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(endIndex));
            }

            return val.Substring(startIndex, endIndex - startIndex);
        }
    }
}