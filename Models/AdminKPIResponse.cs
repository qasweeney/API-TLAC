using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class AdminKPIResponse
    {
        public decimal TotalRevenue { get; set; }
        public int TotalSessions { get; set; }
        public int ActiveMembers { get; set; }
        public int ActiveTrainers { get; set; }
    }
}