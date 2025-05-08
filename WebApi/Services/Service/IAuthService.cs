using Common.Constants;
using Common.DataTransferObjects.User;

namespace WebApi.Services.Registration
{
    public interface IAuthService
    {
        Task<string> Register(string transactionBy, SaveUser saveUserDto);
        Task<LoginResponse> Login(LoginRequest loginRequest);
    }
}
