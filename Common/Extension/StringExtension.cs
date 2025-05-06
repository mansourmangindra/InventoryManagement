namespace Common.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// Returns null if value is empty or already null
        /// </summary>
        public static string ToNullString(this string value)
        {
            return String.IsNullOrEmpty(value) ? null : value;
        }

        /// <summary>
        /// Returns "NA" if value is null or empty
        /// </summary>
        public static string ReplaceEmptyString(this string value)
        {
            return String.IsNullOrEmpty(value) ? "NA" : value;
        }

        /// <summary>
        /// Returns replacement if value is null or empty
        /// </summary>
        public static string ReplaceEmptyString(this string value, string replacement)
        {
            return String.IsNullOrEmpty(value) ? replacement : value;
        }

        /// <summary>
        /// Shortens string to maxLength
        /// </summary>
        public static string ShortenString(this string str, int maxLength = 40)
        {
            if (str.Length > maxLength)
            {
                return String.Concat(str.AsSpan(0, maxLength), "...");
            }
            return str;
        }
    }
}