using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    public class TechnicalSplitSendMailViewModel
    {
        public File File { get; set; }
        public ICollection<SupplierCityViewModel> LineItemSuppliers { get; set; }
        public IEnumerable<OrderLineItem> OrderLineItems { get; set; }        
        public IEnumerable<ReferanceNumber> ReferanceNumbers { get; set; }
        public string Querry { get; set; }

        public ICollection<SupplierCityViewModel> SelectedWarehouses { get; set; }
        public ICollection<MailLogView> RfqMailLogs { get; set; }


    }
}