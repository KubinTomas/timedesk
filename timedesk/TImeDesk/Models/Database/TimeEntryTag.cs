using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TImeDesk.Models.Database
{
    public class TimeEntryTag
    {
        public int Id { get; set; }

        public Tag Tag { get; set; }
        public int TagId { get; set; }

        public TimeEntry TimeEntry { get; set; }
        public int TimeEntryId { get; set; }
    }
}