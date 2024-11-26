using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class AddRecurring
    {
        public int TrainerId { get; set; }
        public string DayOfWeek { get; set; }
        public string StartTime { get; set; }
        public decimal Price { get; set; }
    }
}