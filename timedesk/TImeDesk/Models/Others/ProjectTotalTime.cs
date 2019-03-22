using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models.Database;

namespace TImeDesk.Models.Others
{
    public class ProjectTotalTime
    {
        public Project Project { get; set; }

        public int ProjectTime { get; set; }
    }
}