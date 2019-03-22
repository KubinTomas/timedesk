using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models.Database;

namespace TImeDesk.ViewModel
{
    public class TimeEntriesViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<ProjectTask> ProjectTask { get; set; }
        public IEnumerable<Billings> Billingses { get; set; }
        public IEnumerable<Currency> Currency { get; set; }
    }
}