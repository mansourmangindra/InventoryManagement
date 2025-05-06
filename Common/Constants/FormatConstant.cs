using System.Collections.ObjectModel;

namespace Common.Constants
{
    public static class FormatConstant
    {
        public const string DateFormat = "MM/dd/yyyy";
        public const string DateFullMonthFormat = "MMMM dd, yyyy";
        public const string DateTimeFormat = DateFormat + " hh:mm:ss";
        public const string DateStringFormat = "{0:" + DateFormat + "}";
        public const string DateTimeFileFormat = "MMddyyyyHHmmss";

        private static readonly ReadOnlyCollection<string> InvalidDomains = new(new[] { "sykes.com", "connect.sitel.com", "sitel.com" });

        public static ReadOnlyCollection<string> InvalidEmailAddresses
        {
            get { return InvalidDomains; }
        }
    }
}