namespace Common.DataTransferObjects.UserRole
{
    public class UserRoleDetail
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool LimitedByLob { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool RoleLimitedByLob { get; set; }
        public IEnumerable<string> LobValues { get; set; }
        public bool RoleLimitedByProject { get; set; }
        public IEnumerable<string> AccountValues { get; set; }
        public bool LegalApprover { get; set; }
        public bool Active { get; set; }

        public long LobManagerId { get; set; }
        public long LobApproverId { get; set; }
        public UserRoleDetail urdList { get; set; }
    }
}
