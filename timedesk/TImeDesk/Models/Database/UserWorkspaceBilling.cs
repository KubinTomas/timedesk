using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TImeDesk.Models.Database
{
    public class UserWorkspaceBilling
    {
        public int Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public int ApplicationUserId { get; set; }

        public WorkSpace WorkSpace { get; set; }
        public int? WorkSpaceId { get; set; }

        [ForeignKey("BillingsId")]
        public Billings Billings { get; set; }
        public int? BillingsId { get; set; }

        public Status Status { get; set; }
        public int? StatusId { get; set; }   
    }
}