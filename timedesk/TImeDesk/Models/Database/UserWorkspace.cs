using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TImeDesk.Models.Database
{
    public class UserWorkspace
    {
        public int Id { get; set; }

        public DateTime Invited { get; set; }
        public DateTime Joined { get; set; }

        // Relationships

        public ApplicationUser ApplicationUser { get; set; }
        public int? ApplicationUserId { get; set; }

        public WorkSpace WorkSpace { get; set; }
        public int? WorkspaceId { get; set; }

        public Status Status { get; set; }
        public int? StatusId { get; set; }

        public UserWorkspaceRoles UserWorkspaceRoles { get; set; }
        public int? UserWorkspaceRolesId { get; set; }


    }
}