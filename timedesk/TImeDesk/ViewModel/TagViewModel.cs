using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models.Database;

namespace TImeDesk.ViewModel
{
    public class TagViewModel
    {
        public Tag Tag { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
    }
}