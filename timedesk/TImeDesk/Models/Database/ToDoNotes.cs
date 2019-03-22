using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TImeDesk.Models.Database;

namespace TImeDesk.Models
{
    public class ToDoNotes
    {
        [Key]
        public int Id { get; set; }

        public string Content { get; set; }

        public string Header { get; set; }

        public ToDoList ToDoList { get; set; }

        public int ToDoListId { get; set; }

        //Relationships
        public NoteColorTheme NoteColorTheme { get; set; }

        public int NoteColorThemeId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public int? ApplicationUserId { get; set; }

        public Status Status { get; set; }
        public int? StatusId { get; set; }


    }
}