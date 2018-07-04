using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.Models
{
    public class EmailLog
    {
        public EmailLog()
        { 
            CreatedDate = DateTime.Now; 
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }
        public string ToAdress { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public int FileId { get; set; }

        public DateTime? CreatedDate { get; set; }



    }
}