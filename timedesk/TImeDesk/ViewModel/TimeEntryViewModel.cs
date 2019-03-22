using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models.Database;

namespace TImeDesk.ViewModel
{
    public class TimeEntryViewModel
    {

        public int Id { get; set; }

        public int? SpendedTime { get; set; }

        public DateTime StartedDate { get; set; }

        public DateTime? EndedDate { get; set; }

        public string Description { get; set; }

        public bool IsFinished { get; set; }
      
        public int ApplicationUserId { get; set; }

        public int WorkspaceId { get; set; }

     
        public int? ProjectId { get; set; }

        public int? ProjectTaskId { get; set; }

        public int? BillingsId { get; set; }

        public Project Project { get; set; }
        public ProjectTask ProjectTask { get; set; }
 
        public IEnumerable<TimeEntry> TimeEntries { get; set; }
       // public IEnumerable<ProjectTask> ProjetTasks { get; set; }
        public IEnumerable<ProjectTask> AllProjectTasks { get; set; }

        public IEnumerable<Project> Projects { get; set; }

        public IEnumerable<Billings> AssignedBillingses { get; set; }
        public IEnumerable<Billings> Billingses { get; set; }
        public IEnumerable<Billings> AllBillingses { get; set; }

        public IEnumerable<Currency> Currency { get; set; }

        public IEnumerable<ProjectTask> ProjectTasks { get; set; }

        public IEnumerable<Tag> AllTags { get; set; }

        public IEnumerable<TimeEntryTag> SelectedTags { get; set; }

        public IEnumerable<Project> AllProjects { get; set; }

        public IEnumerable<TimeEntryTag> AllSelectedTags { get; set; }
        //For timer status
        public bool AutomaticTimer { get; set; }
        
        //ToDo TAGS

    }
}