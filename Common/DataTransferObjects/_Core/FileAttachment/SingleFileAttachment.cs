using Microsoft.AspNetCore.Http;

namespace Common.DataTransferObjects._Core.FileAttachment
{
    public class SingleFileAttachment
    {
        public IFormFile File { get; set; }
    }
}