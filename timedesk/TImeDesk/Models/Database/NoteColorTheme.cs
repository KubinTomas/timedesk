using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TImeDesk.Models.Database
{
    public class NoteColorTheme
    {
        public int Id { get; set; }

        public string Header { get; set; }
        public string Body { get; set; }
        public string ThemeBgColor { get; set; }

    }
}