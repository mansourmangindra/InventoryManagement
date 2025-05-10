namespace WebApp.Services.Interfaces
{
    public interface ITokenProvider
    {
        void SetTokeN(string token);
        string? GetToken();
        void ClearToken();
    }
}
