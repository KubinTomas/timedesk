using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TImeDesk.Models.Database
{
    public class UserSettings
    {
        public int Id { get; set; }

        public bool AutomaticTimer { get; set; }

        public int currentWorkspace { get; set; }

        //Relationships
        public Currency Currency { get; set; }
        public int CurrencyId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public int ApplicationUserId { get; set; }
    }
}