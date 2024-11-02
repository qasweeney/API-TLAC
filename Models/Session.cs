using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Session
    {
        public int SessionID { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public SessionType SessionType { get; set; }
        public SessionStatus SessionStatus { get; set; }
        public decimal Price { get; set; }
        public int TrainerID { get; set; }
        public required Trainer Trainer { get; set; }
        public int? MemberID { get; set; }
        public Member? Member { get; set; }
        public int? RatingID { get; set; }
        public Rating? Rating { get; set; }
    }

    public enum SessionType
    {
        Recurring,
        OneTime
    }

    public enum SessionStatus
    {
        Available,
        Booked,
        Canceled
    }
}