using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models.Database;

namespace TImeDesk.Models.Others
{
    // Interface or TimeEntry
    public class CustomTimeEntry
    {
        public string ProjectName { get; set; }
        public string Color { get; set; }

        public string StartedDate { get; set; }


        public int TotalTimeSpend { get; set; }

        public string TotalTimeInFormat { get; set; }
        public string Percentage { get; set; } 
        
    }
}