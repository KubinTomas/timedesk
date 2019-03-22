using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models;
using TImeDesk.Models.Database;
using TImeDesk.Models.Others;

namespace TImeDesk.ViewModel
{
    public class WorkspaceDetailViewModel
    {
        public WorkSpace WorkSpace { get; set; }

        public UserWorkspaceRoles Role { get; set; }

        public IEnumerable<UserWorkspace> UserWorkspaces { get; set; }

        public IEnumerable<Project> Projects { get; set; }

        public List<ProjectTotalTime> ProjectsTotalTime { get; set; }

        public int UserId { get; set; }
    }
}