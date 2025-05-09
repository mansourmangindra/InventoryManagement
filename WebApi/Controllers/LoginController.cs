using Common.DataTransferObjects._Core.ErrorLog;
using Common.DataTransferObjects.User;
using DataAccess.UnitOfWorks.InventoryManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Registration;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IInventoryUnitOfWork _inventoryUnityOfWork;
        private readonly IAuthService _authService;
        public LoginController(IInventoryUnitOfWork inventoryUnitOfWork, IAuthService authService)
        {
            _inventoryUnityOfWork = inventoryUnitOfWork;
            _authService = authService;
        }


        [HttpPost]
        [Route("TransactionBy/{transactionBy}")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var loginResponse = await _authService.Login(model);

            if (loginResponse.User == null)
            {
                return BadRequest(new ErrorMessage("Username or password is incorrect"));
            }


            //_response.Result = loginResponse;
            return Ok();

        }
    }
}
