using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TImeDesk.Models.Database
{
    public class TimeEntry
    {
        public int Id { get; set; }

        public int SpendedTime { get; set; }

        public DateTime StartedDate { get; set; }

        public DateTime EndedDate { get; set; }

        public string Description { get; set; }

        public bool IsFinished { get; set; }

        //Relationships

        public ApplicationUser ApplicationUser { get; set; }
        public int ApplicationUserId { get; set; }

        public WorkSpace WorkSpace { get; set; }
        public int WorkspaceId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        public int? ProjectId { get; set; }

        [ForeignKey("ProjectTaskId")]
        public ProjectTask ProjectTask { get; set; }
        public int? ProjectTaskId { get; set; }

        public Billings Billings { get; set; }
        public int? BillingsId { get; set; } 
    }
}