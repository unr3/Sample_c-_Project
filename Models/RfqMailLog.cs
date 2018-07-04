using Appa_MVC.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Appa_MVC.Models
{
    public class RfqMailLog
    {
        public RfqMailLog()
        {
            Date = DateTime.Now; 
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public int RfqId { get; set; }
        public string ReceiverId { get; set; }
        public DateTime? Date { get; set; }
        public string User { get; set; }
    }
}