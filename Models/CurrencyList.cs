using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    public class CurrencyList
    {
        public enum Currencies
        {
            [Display(Name = "Turkish Liras")]
            TRY,
            [Display(Name = "US Dolar")]
            USD,
            [Display(Name = "Euro")]
            EURO,
          
        }
    }
}