using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class LoginReq
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; } // "member", "trainer", or "admin"
    }
}