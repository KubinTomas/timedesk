using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models;
using TImeDesk.Models.Database;

namespace TImeDesk.ViewModel.Workspace.User
{
    public class UserDetailWorkspaceViewModel
    {
        public UserWorkspace UserWorkspace { get; set; }

        public IEnumerable<Billings> AvailableBillings { get; set; }

        public IEnumerable<Billings> AssignedBillings { get; set; }
    }
}