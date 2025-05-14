namespace Common.DataTransferObjects.UserRole
{
    public class UserRoleDetail
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Active { get; set; }
        public UserRoleDetail urdList { get; set; }
    }
}
