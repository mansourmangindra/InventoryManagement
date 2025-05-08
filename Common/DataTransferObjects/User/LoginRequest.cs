using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataTransferObjects.User
{
    public class LoginRequest
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}
