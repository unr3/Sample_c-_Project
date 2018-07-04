using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    public class File
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public byte Id { get; set; }
        public string FileNumber { get; set; }
        public DateTime? CreatedDate
        {
            get; set;
        }
        public byte Stage { get; set; }
        public bool Isactive { get; set; }
        public string VesselId { get; set; }

        public int? Currency { get; set; }
        // 0 tl 
        // 1 dolar
        // 2 usd

        public DateTime? Eta { get; set; }
        public string Company { get; set; }
        public string DeliveryPlace { get; set; }
        public int DeliveryCost { get; set; }
        public int Discount { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public string Note3 { get; set; }
        public string PaymentTerm { get; set; }
        public string Department { get; set; }
        public string VadePesin { get; set; }
        public bool IsClosed { get; set; }
        public string Createduser { get; set; }





    }
}