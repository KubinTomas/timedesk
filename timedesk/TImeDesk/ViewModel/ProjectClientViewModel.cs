using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models.Database;

namespace TImeDesk.ViewModel
{
    public class ProjectClientViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string ColorString { get; set; }

        //Relationships
        
        public int? WorkSpaceId { get; set; }

        public int? StatusId { get; set; }

        public int? ColorId { get; set; }

        public int? ClientId { get; set; }

        //Old way
        public IEnumerable<Project> Projects { get; set; }


        public IEnumerable<UserProject> UserProjects { get; set; }

        public IEnumerable<Client> Clients { get; set; }
    }
}