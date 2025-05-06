namespace Common.Extension
{
    public static class DoubleExtension
    {
        /// <summary>
        /// Convert seconds to mm:ss string format
        /// </summary>
        public static string ToStringMMSS(this double totalSeconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);
            return timeSpan.ToString(@"mm\:ss");
        }
    }
}