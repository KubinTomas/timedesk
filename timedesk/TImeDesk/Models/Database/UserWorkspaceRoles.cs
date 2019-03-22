using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TImeDesk.Models.Database
{
    public class UserWorkspaceRoles
    {
        public int Id { get; set; }

        public bool CanManageRoles { get; set; }

        public bool CanManageProjects{ get; set; }

        public bool CanManageBudget { get; set; }

        public bool CanManageStatistic { get; set; }

        public bool CanManageSettings { get; set; }

        public bool CanManageUsers { get; set; }
    }
}