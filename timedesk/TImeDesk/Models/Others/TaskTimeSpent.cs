using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models.Database;

namespace TImeDesk.Models.Others
{
    public class TaskTimeSpent
    {
        public ProjectTask Task { get; set; }

        public int TotalTimeSpent { get; set; }
    }
}
