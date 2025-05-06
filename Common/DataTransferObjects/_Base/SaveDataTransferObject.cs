using Newtonsoft.Json;
using System.Text;

namespace Common.DataTransferObjects._Base
{
    public class SaveDataTransferObject
    {
        public StringContent GetStringContent()
        {
            return new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
        }
    }
}
