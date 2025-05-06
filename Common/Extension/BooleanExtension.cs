namespace Common.Extension
{
    public static class BooleanExtension
    {
        /// <summary>
        /// Converts value into Yes or No string
        /// </summary>
        public static string ToYesNoString(this bool value)
        {
            return value ? "Yes" : "No";
        }
    }
}