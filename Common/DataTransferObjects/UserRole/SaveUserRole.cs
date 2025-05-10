using Common.DataTransferObjects._Base;

namespace Common.DataTransferObjects.UserRole
{
    public class SaveUserRole : SaveDataTransferObject
    {
        public int UserId { get; set; }
        public short RoleId { get; set; }
        public string UserPrincipalName { get; set; }
        public IEnumerable<string> FacilityIds { get; set; }
        public IEnumerable<string> ProjectIds { get; set; }
        public bool Active { get; set; }
        public List<int> SelectedPaymentType { get; set; }
        public List<int> SelectedPaymentSubType { get; set; }
        public List<int> SelectedPaymentTypeEdit  { get; set; }
        public List<int> SelectedPaymentSubTypeEdit { get; set; }
        public int RedundancyLevel { get; set; }
        public int RetirementLevel { get; set; }
        public int SettlementLevel { get; set; }
        public int RedundancyLevelEdit { get; set; }
        public int RetirementLevelEdit { get; set; }
        public int SettlementLevelEdit { get; set; }
    }
}
