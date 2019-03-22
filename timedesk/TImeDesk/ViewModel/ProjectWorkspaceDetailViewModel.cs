using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models;
using TImeDesk.Models.Database;
using TImeDesk.Models.Others;

namespace TImeDesk.ViewModel
{
    public class ProjectWorkspaceDetailViewModel
    {
        public Project Project { get; set; }

        public int TotalAssignedUsers { get; set; }

        public int TotalWorkedTime { get; set; }

        public string WorkspaceCurrencyName { get; set; }

        public string Money { get; set; }


        public int DeadlineProgressBarValue { get; set; }
        public int BudgetProgressBarValue { get; set; }
        public int HourLimitProgressBarValue { get; set; }

        public List<ProjectTotalTime> ProjectsTotalTime { get; set; }

        public IEnumerable<UserProject> UserProjects { get; set; }

        public IEnumerable<UserWorkspace> WorkspaceUsers { get; set; }

        public IEnumerable<UserWorkspace> UsersToAssign { get; set; }

        public List<UserProjectSpentTime> UserProjectTimeList { get; set; }

    }
}