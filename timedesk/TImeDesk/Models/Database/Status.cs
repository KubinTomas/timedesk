using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TImeDesk.Models.Database
{
    public class Status
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static readonly int Active = 1;
        public static readonly int Archived = 2;
        public static readonly int Deleted = 3;
        public static readonly int Accepted = 4;
        public static readonly int Pending = 5;
        public static readonly int Private = 6;
        public static readonly int Public = 7;
        public static readonly int LeftWorkspace = 8;

    }
}