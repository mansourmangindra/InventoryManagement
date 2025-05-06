using Common.DataTransferObjects._Core.ErrorLog;
using Newtonsoft.Json;
using System.Net;

namespace Common.Extension
{
    public static class ResponseMessageExtension
    {
        public static async Task<ErrorMessage> GetErrorMessage(this HttpResponseMessage httpResponseMessage)
        {
            string responseText = await httpResponseMessage.Content.ReadAsStringAsync();

            if (responseText.Contains("traceId") &&
                responseText.Contains("message") &&
                responseText.Contains("type"))
            {
                ErrorMessage errorMessage = JsonConvert.DeserializeObject<ErrorMessage>(responseText);
                return errorMessage;
            }
            else if (httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
            {
                return new ErrorMessage(responseText);
            }
            else
            {
                ErrorMessage errorMessage = new ErrorMessage(httpResponseMessage.ReasonPhrase);
                return errorMessage;
            }
        }
    }
}