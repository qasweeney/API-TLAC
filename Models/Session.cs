using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Session
    {
        public int SessionID { get; set; }
        public string? DayOfWeek { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public string SessionType { get; set; }
        public string SessionStatus { get; set; }
        public decimal Price { get; set; }
        public int TrainerID { get; set; }
        public Trainer? Trainer { get; set; }
        public int? MemberID { get; set; }
        public Member? Member { get; set; }
        public decimal? Rating { get; set; }
    }
}