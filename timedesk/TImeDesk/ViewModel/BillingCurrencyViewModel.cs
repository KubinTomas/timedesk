using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TImeDesk.Models.Database;

namespace TImeDesk.ViewModel
{
    public class BillingCurrencyViewModel
    {
        public IEnumerable<Currency> Currencies { get; set; }
        public IEnumerable<Billings> Billingses { get; set; }

        public int BillingId { get; set; }

        public string Name { get; set; }
        public string HourRate { get; set; }
        public int CurrencyId { get; set; }
    }
}