using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Appa_MVC.Models;

namespace Appa_MVC.Models
{
    public class LineItemEditModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; } 

        [Required]
        public int ReferanceNumberId { get; set; }

        [Required]
        public string Impa { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Unit { get; set; }

        [Required]
        public decimal Qtty { get; set; }

        [Required]
        public string Number { get; set; }

        public decimal? Price { get; set; }
        public string Remark { get; set; }

        [DisplayName("Alternative Unit")]
        public string AltUnit { get; set; }
        [DisplayName("Alternative Qtty")]
        public decimal? AltQtty { get; set; }
        [DisplayName("Alternative Price")]
        public decimal? AltPrice { get; set; } 
     

        //supplier changes will not effect after selection
        [DisplayName("Supplier Name")]
        public string SelectedSupplierName { get; set; }

     
        [DisplayName("Supplier Price")]
        public decimal SelectedSupplierCalculatedPrice { get; set; }
     
    }
}