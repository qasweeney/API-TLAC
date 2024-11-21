using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class RegisterSessionRequest
    {
        public int SessionID { get; set; }
        public int MemberID { get; set; }
        public DateTime? Date { get; set; }
    }
}