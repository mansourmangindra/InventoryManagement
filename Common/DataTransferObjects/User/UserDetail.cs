using Common.DataTransferObjects.UserRole;
using Microsoft.AspNetCore.Identity;

namespace Common.DataTransferObjects.User
{
    public class UserDetail
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Active { get; set; }

        public IEnumerable<UserRoleDetail> UserRoleDetails { get; set; }
    }
}