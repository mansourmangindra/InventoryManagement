using Common.Constants;
using WebApp.Services.Interfaces;

namespace WebApp.Services
{
    public class TokenProvider : ITokenProvider
    {

        private readonly IHttpContextAccessor _contextAccessor;

        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }


        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(HttpClientConstant.TokenCookie);
        }

        public string GetToken()
        {
            string? token = null;
            bool? hasToken = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(HttpClientConstant.TokenCookie, out token);
            return hasToken is true ? token : null;
        }

        public void SetTokeN(string token)
        {
            _contextAccessor.HttpContext?.Response.Cookies.Append(HttpClientConstant.TokenCookie, token);
        }
    }
}
