namespace Common.DataTransferObjects._Core.AppSettings
{
    public class SecurityGroup
    {
        public string[] ApplicationSupport { get; set; }

        public string[] AllowedGroups {
            get
            {
                return ApplicationSupport;
            }
        }
    }
}
