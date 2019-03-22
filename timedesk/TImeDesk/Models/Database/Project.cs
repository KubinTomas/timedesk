using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TImeDesk.Models.Database
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ColorString { get; set; }

        public DateTime Created { get; set; }

        public DateTime? DeadLine { get; set; }

        public float? Budget { get; set; }

        public int? HourLimit { get; set; }


        //Relationships
        [ForeignKey("WorkSpaceId")]
        public WorkSpace WorkSpace { get; set; }
        public int? WorkSpaceId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public int? ApplicationUserId { get; set; }

        public Status Status { get; set; }
        public int? StatusId { get; set; }

        public Client Client { get; set; }
        public int? ClientId { get; set; }

        public Currency Currency { get; set; }
        public int? CurrencyId { get; set; }
    }
}