using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.Models
{
    public class GARRETS
    {
        [Key]
        public string Id { get; set; }
        public string description { get; set; }
        public string uom { get; set; }
        public string brand { get; set; }
        public decimal netprice { get; set; }
        public decimal grossprice { get; set; }
        public string country { get; set; }
        public string package { get; set; }
    }
}