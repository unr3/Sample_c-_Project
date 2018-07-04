using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    public class OrderedLineItemEditViewModel
    {
        public OrderLineItem OrderLineItem { get; set; }
        public LineItem LineItem { get; set; }
        public ICollection<ViewSupplierPriceLineItem> LineItemSuppliers { get; set; }
        public File File { get; set; }
        public String Querry { get; set; }
    }
}