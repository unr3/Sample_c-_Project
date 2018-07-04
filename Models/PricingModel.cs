using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    public class PricingModel
    {
        public IEnumerable<ReferanceNumber> ReferanceNumbers { get; set; }
        public IEnumerable<LineItem> LineItems { get; set; }
        public IEnumerable<SupplierMaster> SupplierOffers { get; set; }
        public IEnumerable<RfqTotal> RfqTotals { get; set; }
        public File File { get; set; }
    }
}