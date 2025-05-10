using Common.DataTransferObjects._Core.ReferenceData;

namespace Common.DataTransferObjects.UserRole
{
    public class UserRoleWithModule
    {
        public ReferenceDataDetail RoleDetail { get; set; }
        public bool Selected { get; set; }
        public IEnumerable<ReferenceDataDetail> ModuleDetails { get; set; }
    }
}
