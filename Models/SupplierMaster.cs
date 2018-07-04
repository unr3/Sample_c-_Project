using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    [Table("V_SupplierMaster")]
    public class SupplierMaster
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public string SupplierId { get; set; }
        public int FileId { get; set; }
        public bool IsClosed { get; set; }
        [DisplayName("Is Complated")]
        public bool IsComplated { get; set; }

        [DisplayName("Approved By")]
        public string NameOfPriceGiven { get; set; }
        [DisplayName("Complated Date")]
        public DateTime? ComplatedDate { get; set; }

        public string OfferId { get; set; }

    }
}