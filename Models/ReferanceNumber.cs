using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    public class ReferanceNumber
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public File File { get; set; }
        public byte FileId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate
        {
            get; set;
        }
        public DateTime? DueDate { get; set; }
        public byte Stage { get; set; }
        public bool Isactive { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsOrdered { get; set; }

        public bool IsSplitted { get; set; }

        [DisplayName("PO Number")]
        public string PONumber { get; set; }

        public bool RemovedFromSaleReport { get; set; }

        public string RemovedFromSaleReportUser { get; set; }
        public string RemovedFromSaleReportReason { get; set; }


    }
}