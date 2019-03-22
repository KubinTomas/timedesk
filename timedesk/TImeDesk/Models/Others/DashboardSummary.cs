using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TImeDesk.Models.Others
{
    public class DashboardSummary
    {
        public int ProjectCount { get; set; }
        public int TotalTime { get; set; }
        public int TotalEarned { get; set; }
        public string Symbol { get; set; }
        public string mostTracket { get; set; }
    }
}