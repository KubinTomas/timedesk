using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models.Database;

namespace TImeDesk.ViewModel.Workspace.Settings
{
    public class WorkspaceSettingsViewModel
    {
        public WorkSpace WorkSpace { get; set; }

        public int TotalUsersInWorkspace { get; set; }

        public Currency Currency { get; set; }

        public IEnumerable<Currency> Currencies { get; set; }
    }
}