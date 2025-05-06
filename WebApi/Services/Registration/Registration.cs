using Common.DataTransferObjects.User;
using DataAccess.DbContexts.InventoryManagement.Models;
using DataAccess.UnitOfWorks.InventoryManagement;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApi.Services.Registration
{
    public class Registration : IRegistration
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public Registration(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }
        public async Task<string> Register(string transactionBy, SaveUser saveUserDto)
        {
            User user = new()
            {
                Name = saveUserDto.Name,
                PhoneNumber = saveUserDto.PhoneNumber,
                EmailAddress = saveUserDto.EmailAddress,
                Password = saveUserDto.Password,
                CreatedBy = saveUserDto.CreatedBy,
                UpdatedBy = saveUserDto.UpdatedBy,
                CreatedDate = saveUserDto.CreatedDate,
                UpdatedDate = saveUserDto.UpdatedDate,
                Active = true
            };

            await _inventoryUnitOfWork.UserRepository.AddAsync(user);
            await _inventoryUnitOfWork.SaveChangesAsync(transactionBy);

            return user.UserId.ToString();

        }
    }
}
