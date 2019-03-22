using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models.Database;

namespace TImeDesk.ViewModel
{
    public class ClientViewModel
    {
        public IEnumerable<Client> Clients { get; set; }

        public string Name { get; set; }
    }
}