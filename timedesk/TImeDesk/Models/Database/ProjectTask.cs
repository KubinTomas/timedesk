using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TImeDesk.Models.Database
{
    public class ProjectTask
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ColorString { get; set; }
        //Relationships

        public Project Project { get; set; }
        public int ProjectId { get; set; }

        public Status Status { get; set; }
        public int? StatusId { get; set; }

        [ForeignKey("WorkSpaceId")]
        public WorkSpace WorkSpace { get; set; }
        public int? WorkSpaceId { get; set; }
    }
}