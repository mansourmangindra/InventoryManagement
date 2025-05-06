using Common.Constants;
using Common.DataTransferObjects._Core.ErrorLog;
using Common.DataTransferObjects.User;
using DataAccess.DbContexts.InventoryManagement.Models;
using DataAccess.UnitOfWorks.InventoryManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Services.Registration;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IInventoryUnitOfWork _inventoryUnityOfWork;
        private readonly IRegistration _registration;
        public UserController(IInventoryUnitOfWork inventoryUnitOfWork, IRegistration registration)
        {
            _inventoryUnityOfWork = inventoryUnitOfWork;
            _registration = registration;
        }

        [HttpPost]
        [Route("TransactionBy/{transactionBy}")]
        [SwaggerOperation(Summary = "Create User")]
        public async Task<ActionResult<int>> CreateUser([FromRoute] string transactionBy, [FromBody] SaveUser saveUserDto)
        {
            bool existing = await _inventoryUnityOfWork.UserRepository.IsExistAsync(
                predicate: u => u.EmailAddress != null && u.EmailAddress == saveUserDto.EmailAddress);

            if (existing)
            {
                return BadRequest(new ErrorMessage(ErrorMessageConstant.AlreadyExist($"{saveUserDto.EmailAddress}")));
            }


            string userId = await _registration.Register(transactionBy, saveUserDto);
            return Ok(userId);


        }
    }
}
