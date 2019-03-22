using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models.Database;

namespace TImeDesk.ViewModel.Workspace.Permissions
{
    public class WorkspaceUserPermissions
    {
        public IEnumerable<UserWorkspace> UserWorkspaces { get; set; }

        public UserWorkspaceRoles Role { get; set; }
    }
}