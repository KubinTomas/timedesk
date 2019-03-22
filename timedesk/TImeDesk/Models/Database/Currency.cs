
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TImeDesk.Models.Database
{
    public class Currency
    {
        public int Id { get; set; } 

        [Required]
        public string Name { get; set; }

        [Required]
        public string Symbol { get; set; }
    }
}