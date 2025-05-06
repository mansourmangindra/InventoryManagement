using Common.DataTransferObjects.User;

namespace WebApi.Services.Registration
{
    public interface IRegistration
    {
        Task<string> Register(string transactionBy, SaveUser saveUserDto);
    }
}
