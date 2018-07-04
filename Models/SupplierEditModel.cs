using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    public class SupplierEditModel
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int supplierlineid { get; set; }

        [Required]
        public string Impa { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Unit { get; set; }

        [Required]
        public decimal Qtty { get; set; }

        [Required]
        public string Number { get; set; }

        public string SupplierPrice { get; set; }
        public string Comment { get; set; }

       
    }
}