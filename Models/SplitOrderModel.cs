using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    public class SplitOrderModel
    {
        public IEnumerable<ReferanceNumber> ReferanceNumbers { get; set; }
        public IEnumerable<OrderLineItem> OrderLineItems { get; set; }
        public IEnumerable<RfqTotal> RfqTotals { get; set; }
        public File File { get; set; }
        public string Querry { get; set; }
    }
}