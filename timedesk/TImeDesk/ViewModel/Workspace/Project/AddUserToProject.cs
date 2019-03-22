using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models;
using TImeDesk.Models.Database;

namespace TImeDesk.ViewModel
{
    public class AddUserToProject
    {
        public IEnumerable<UserWorkspace> Users { get; set; }
        public Project Project { get; set; }
 
    }
}