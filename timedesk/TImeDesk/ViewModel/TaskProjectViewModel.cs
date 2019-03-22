using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models.Database;

namespace TImeDesk.ViewModel
{
    public class TaskProjectViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string ColorString { get; set; }

        //Relationships

        public int? WorkSpaceId { get; set; }

        public int? StatusId { get; set; }

        public int? ProjectId { get; set; }

        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Project> AllProjects { get; set; }
        public IEnumerable<ProjectTask> Tasks { get; set; }
    }
}