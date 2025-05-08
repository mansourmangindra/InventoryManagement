using Common.DataTransferObjects.User;

namespace Common.Constants
{
    public class LoginResponse
    {
        public UserDetail User { get; set; }
        public string Token { get; set; }
    }
}
