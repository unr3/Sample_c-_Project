using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    [Table("V_Garrets_Contract")]
    public class GarretsContract
    {
        public string Id { get; set; }
        public string description { get; set; }
        public string uom { get; set; }
        public string brand { get; set; }
        public decimal netprice { get; set; }
        public decimal grossprice { get; set; }
        public string country { get; set; }
        public string package { get; set; }

        public string maliyet { get; set; }
        public string remark { get; set; }


    }
}