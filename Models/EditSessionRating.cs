using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class EditSessionRating
    {
        public decimal Rating { get; set; }
        public int SessionID { get; set; }
    }
}