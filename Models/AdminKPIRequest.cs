using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class AdminKPIRequest
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; } = new DateOnly();
    }
}