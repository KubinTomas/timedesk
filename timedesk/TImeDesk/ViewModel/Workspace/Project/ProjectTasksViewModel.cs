using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models;
using TImeDesk.Models.Database;
using TImeDesk.Models.Others;

namespace TImeDesk.ViewModel
{
    public class ProjectTasksViewModel
    {
        public Project Project { get; set; }

        public IEnumerable<ProjectTask> Tasks { get; set; }

        public List<TaskTimeSpent> TasksTimeSpent { get; set; }

        public int WorkspaceId { get; set; }

    }
}