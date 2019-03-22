using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TImeDesk.Models.Database
{
    public class Billings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HourRate { get; set; }

       
        //RelationShips
        public WorkSpace WorkSpace { get; set; }
        public int WorkSpaceId { get; set; }

        public Status Status { get; set; }
        public int? StatusId { get; set; }

        public Currency Currency { get; set; }
        public int CurrencyId { get; set; }

    }
}