using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class ScheduleEntry
    {
        public string DayOfWeek { get; set; }
        public int TrainerID { get; set; }
        public int SessionID { get; set; }
        public TimeSpan StartTime { get; set; }
    }
}