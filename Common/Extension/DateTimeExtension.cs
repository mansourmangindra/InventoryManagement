using Common.Constants;

namespace Common.Extension
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// Converts value into a DateFormat string specified in FormatConstant
        /// </summary>
        public static string ToDateStringFormat(this DateTime value)
        {
            return value.ToString(FormatConstant.DateFormat);
        }

        public static string ToDateTimeStringFormat(this DateTime value)
        {
            return value.ToString(FormatConstant.DateTimeFormat);
        }

        public static string ToFullMonthStringFormat(this DateTime value)
        {
            return value.ToString(FormatConstant.DateFullMonthFormat);
        }

        public static DateTime RoundDown(this DateTime dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            return new DateTime(dt.Ticks - delta, dt.Kind);
        }
    }
}