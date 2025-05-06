namespace Common.DataTransferObjects._Core.Token
{
    public class IdentityServerTokenDetail
    {
        public string ClientId { get; set; }
        public string AccessToken { get; set; }
        public DateTime RefreshDate { get; set; }
    }
}