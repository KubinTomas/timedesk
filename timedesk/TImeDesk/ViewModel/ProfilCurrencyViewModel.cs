using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models.Database;

namespace TImeDesk.ViewModel
{
    public class ProfilCurrencyViewModel
    {
        public UserSettings UserSettings { get; set; }
        public int UserId { get; set; }

        public int CurrencyId { get; set; }
        public IEnumerable<Currency> Currencies { get; set; }

    }
}