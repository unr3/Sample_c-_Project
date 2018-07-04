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
    public class LineItem
    {
        public LineItem()
        {
            MasterNumber = "0";
            CreatedDate=DateTime.Now;
             
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public ReferanceNumber ReferanceNumber { get; set; }

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

        public bool IsAlternative { get; set; }
        public int? MasterItemId { get; set; }

        public int UserId { get; set; }
        public DateTime? CreatedDate { get; set; }

        // public IEnumerable<LineItemSupplier> LineItemSuppliers { get; set; }


        //stock information will be on view

        //supplier changes will not effect after selection
        [DisplayName("Supplier Name")]
        public string SelectedSupplierName { get; set; }

        public string SelectedSupplierId { get; set; }
        [DisplayName("Supplier Price")]
        public decimal SelectedSupplierCalculatedPrice { get; set; }
        [DisplayName("Supplier Price")]
        public decimal SelectedSupplierPrice { get; set; }
        [DisplayName("Supplier Remark")]
        public string SelectedSupplierRemark { get; set; }
        [DisplayName("Supplier Currency")]
        public string SelectedSupplierCurrency { get; set; }

        public int SelectedSupplier { get; set; }

        public virtual ICollection<LineItemSupplier> LineItemSuppliers { get; set; }

        public bool IsRemoved { get; set; }
        public string RemovedUser { get; set; }

        public string MasterNumber { get; set; }

        public bool IsWareHouseSelected { get; set; }

        public string WarehouseInfo { get; set; }

        public string SelectedSupplierTurkishDefinition { get; set; }


    }

    
}