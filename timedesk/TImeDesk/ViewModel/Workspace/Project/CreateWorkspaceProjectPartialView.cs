using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models.Database;

namespace TImeDesk.ViewModel.Workspace.Project
{
    public class CreateWorkspaceProjectPartialView
    {
        public int WorkspaceId { get; set; }

        public Models.Database.Project Project { get; set; }

        public string CurrencyName { get; set; }

        public Client Client { get; set; }

        public int? ClientId { get; set; }

        public IEnumerable<Client> Clients { get; set; }

    }
}