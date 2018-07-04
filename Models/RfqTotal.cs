using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    public class RfqTotal
    {
        public int RfqId { get; set; }
        public decimal Total { get; set; }
        public decimal CostTotal { get; set; }
        public decimal AntrepoTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }

    }
}