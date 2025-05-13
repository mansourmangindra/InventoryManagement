using Common.DataTransferObjects._Base;
using System.ComponentModel.DataAnnotations;

namespace Common.DataTransferObjects.User
{
    public class SaveUser : SaveDataTransferObject
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Active { get; set; }
    }
}