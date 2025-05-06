namespace Common.DataTransferObjects._Core.AppSettings
{
    public class IdentityServerClientDefinition
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string GrantType { get; set; }
        public string Authority { get; set; }
    }
}
