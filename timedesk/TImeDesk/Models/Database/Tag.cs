using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TImeDesk.Models.Database
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public WorkSpace WorkSpace { get; set; }
        public int WorkSpaceId { get; set; }
    }
}