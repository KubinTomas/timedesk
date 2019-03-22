using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TImeDesk.Models.Others
{
    public class UserProjectSpentTime
    {
        public ApplicationUser User { get; set; }

        public int TimeWorkedOnProject { get; set; }
    }
}