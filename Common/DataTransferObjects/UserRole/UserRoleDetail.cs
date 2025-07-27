using System.ComponentModel.DataAnnotations.Schema;

namespace Common.DataTransferObjects.UserRole
{
    public class UserRoleDetail
    {
        [NotMapped]
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        [NotMapped]
        public short RoleId { get; set; }
        public string RoleName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string UpdatedBY { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Active { get; set; }
        public string CreatedBy { get; set; }
        public string UserName { get; set; }

        [NotMapped]
        public UserRoleDetail urdList { get; set; }
        
    }
}
