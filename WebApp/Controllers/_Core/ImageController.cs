using Common.Constants;
using Common.DataTransferObjects._Core.ErrorLog;
using Common.DataTransferObjects._Core.Image;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    public class ImageController : Controller
    {
        //private readonly IMicrosoftGraphService _microsoftGraphService;
        //public ImageController(IMicrosoftGraphService microsoftGraphService)
        //{
        //    _microsoftGraphService = microsoftGraphService;
        //}

        [HttpGet]
        [Route("GetUserPhotoByUpn")]
        public async Task<IActionResult> GetUserPhotoByUpn([FromQuery] string upn, string imageSize)
        {
            return File("~/img/avatar/defaultprofilepic.jpg", "image/jpg");
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(new ErrorMessage(ErrorMessageConstant.InvalidInputData));
            //}

            //string[] validImageSizes = MsGraphConstant.ValidImageSizes.Split(",");
            //if (!string.IsNullOrEmpty(imageSize) && validImageSizes.Contains(imageSize))
            //{
            //    ImageDetail imageDetail = await _microsoftGraphService.GetUserPhotoByUpn(upn, imageSize);
            //    if (imageDetail != null)
            //    {
            //        return File(imageDetail.FileContent, imageDetail.ContentType);
            //    }
            //    return File("~/img/avatar/defaultprofilepic.jpg", "image/jpg");
            //}
            //return BadRequest("Invalid Image Size");
        }
    }
}