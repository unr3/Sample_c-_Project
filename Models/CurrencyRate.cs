using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    public class CurrencyRate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int CurrencyId { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
        public string User { get; set; }


    }
}