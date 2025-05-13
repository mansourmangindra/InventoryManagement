using ApiConfiguration.Service;
using Common.Constants;
using Common.DataTransferObjects._Core.ErrorLog;
using Common.DataTransferObjects.Role;
using Common.DataTransferObjects.User;
using DataAccess.DbContexts.InventoryManagement.Models;
using DataAccess.UnitOfWorks.InventoryManagement;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using User = DataAccess.DbContexts.InventoryManagement.Models.User;

namespace WebApi.Services.Registration
{
    public class AuthService : IAuthService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly PasswordHasher<User> _passwordHasher = new();
        public AuthService(IInventoryUnitOfWork inventoryUnitOfWork, IJwtTokenGenerator jwtTokenGenerator)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<string> Register(string transactionBy, SaveUser saveUserDto)
        {
            User user = new()
            {
                Name = saveUserDto.Name,
                PhoneNumber = "000",
                EmailAddress = saveUserDto.EmailAddress,
                Password = saveUserDto.Password,
                CreatedBy = transactionBy,
                UpdatedBy = transactionBy,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Active = true
            };

            await _inventoryUnitOfWork.UserRepository.AddAsync(user);

            await _inventoryUnitOfWork.SaveChangesAsync(transactionBy);

            UserRole userRole = new()
            {
                UserId = user.UserId,
                RoleId = IMConstant.CUSTOMER_ID,
                CreatedBy = transactionBy,
                UpdatedBy = transactionBy,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Active = true
            };

            await _inventoryUnitOfWork.UserRoleRepository.AddAsync(userRole);



            await _inventoryUnitOfWork.SaveChangesAsync(transactionBy);

            return user.UserId.ToString();

        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var user = await _inventoryUnitOfWork.UserRepository.FirstOrDefaultAsync(u => u.EmailAddress.ToLower() == loginRequest.EmailAddress.ToLower());

            if (user == null)
            {
                return new LoginResponse() { User = null, Token = "" };
            }
            else if (user.EmailAddress != loginRequest.EmailAddress || user.Password != loginRequest.Password)
            {
                return new LoginResponse() { User = null, Token = "" };
            }

            //if user was found, Generate JWT Token
            var passwordHasher = new PasswordHasher<User>();
            //var result = passwordHasher.VerifyHashedPassword(user, user.Password, loginRequest.Password);

            //if (result == PasswordVerificationResult.Failed)
            //    return new LoginResponse { User = null, Token = "" };

            var userRoles = await _inventoryUnitOfWork.UserRoleRepository.GetAllAsync();
            var rolesList = await _inventoryUnitOfWork.RoleRepository.GetAllAsync();

            var roles = (from ur in userRoles
                         join r in rolesList on ur.RoleId equals r.RoleId
                         where ur.UserId == user.UserId
                         select r.Name).ToList();

            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            UserDetail userDetail = new()
            {
                EmailAddress = user.EmailAddress,
                UserId = user.UserId,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponse loginResponseDto = new LoginResponse()
            {
                User = userDetail,
                Token = token
            };

            return loginResponseDto;
        }
    }
}
