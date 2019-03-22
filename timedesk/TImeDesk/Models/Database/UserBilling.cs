﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TImeDesk.Models.Database
{
    public class UserBilling
    {
        public int Id { get; set; }

        //Relationships
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public int ApplicationUserId { get; set; }

        [ForeignKey("WorkSpaceId")]
        public WorkSpace WorkSpace { get; set; }
        public int? WorkSpaceId { get; set; }

        [ForeignKey("ProjectId")]
        public Billings Billings { get; set; }
        public int BillingsId { get; set; }
    }
}