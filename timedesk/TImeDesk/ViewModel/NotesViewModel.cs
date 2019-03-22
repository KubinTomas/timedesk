using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models;
using TImeDesk.Models.Database;

namespace TImeDesk.ViewModel
{
    public class NotesViewModel
    {
        public ToDoNotes Note { get; set; }

        public IEnumerable<ToDoNotes> PrivateNotes { get; set; }
        public IEnumerable<ToDoNotes> PublicNotes { get; set; }


        public IEnumerable<NoteColorTheme> NoteColorThemes { get; set; }
    }
}