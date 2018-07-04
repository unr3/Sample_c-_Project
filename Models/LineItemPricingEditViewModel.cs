using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Appa_MVC.Models;

namespace Appa_MVC.Models
{
    public class LineItemPricingEditViewModel
    {
        public LineItem LineItem { get; set; }
        public ICollection<ViewSupplierPriceLineItem> LineItemSuppliers { get; set; }
        public File File { get; set; }
    }
}