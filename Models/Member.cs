using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Member
    {
        public int MemberID { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;  // Default to current timestamp
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    }
}