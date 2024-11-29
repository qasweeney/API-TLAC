using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Trainer
    {
        public int TrainerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public string Password { get; set; }
        public decimal? SessionPrice { get; set; }
        public string Phone { get; set; }
        public int? IsActive { get; set; }
        public string Bio { get; set; }
        public string ProfilePic { get; set; }
    }
}