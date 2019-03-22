using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models;
using TImeDesk.Models.Database;

namespace TImeDesk.ViewModel.Workspace.Statistic
{
    public class WorkspaceStatisticViewModel
    {

        public ApplicationUser User { get; set; }

        public IEnumerable<ApplicationUser> WorkspaceUsers { get; set; }
        public IEnumerable<Models.Database.Project> Projects { get; set; }
    }   
}