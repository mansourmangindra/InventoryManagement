using Common.DataTransferObjects.RoleModule;
using Common.DataTransferObjects.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataTransferObjects.Role
{
    public class RoleDetail
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Active { get; set; }

        public IEnumerable<RoleModuleDetail> RoleModuleDetails { get; set; }
        public IEnumerable<UserDetail> ListOfUsers { get; set; }
    }
}
