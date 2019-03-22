using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models.Database;

namespace TImeDesk.ViewModel
{
    public class WorkspaceViewModel
    {
        public IEnumerable<UserWorkspace> WorkSpaces { get; set; }

        public IEnumerable<UserWorkspace> WorkspaceInvitations { get; set; }

        public WorkSpace WorkSpace { get; set; }

        public int currentWorkspaceId { get; set; }
    }
}