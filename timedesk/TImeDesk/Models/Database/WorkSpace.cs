using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models.Database;

namespace TImeDesk.Models.Database
{
    public class WorkSpace
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public DateTime Created { get; set; }

        //Relationships
        public ToDoList ToDoList { get; set; }
        public int ToDoListId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public int ApplicationUserId { get; set; }

        public Currency Currency { get; set; }
        public int? CurrencyId { get; set; }

    }
}