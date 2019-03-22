using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TImeDesk.Models.Database
{
    public class Client
    {
        public int Id { get; set; }

        public string Name { get; set; }
        

        //Relationships
        public WorkSpace WorkSpace { get; set; }
        public int WorkSpaceId { get; set; }

        public Status Status { get; set; }
        public int? StatusId { get; set; }
        

        public ApplicationUser ApplicationUser { get; set; }
        public int? ApplicationUserId { get; set; }
    }
}