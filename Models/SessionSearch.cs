using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class SessionSearch
    {
        public int? TrainerID { get; set; }
        public string Date { get; set; }
        public string? Time { get; set; }
        public bool AvailableOnly { get; set; }
    }
}