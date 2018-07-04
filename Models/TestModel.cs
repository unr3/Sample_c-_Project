using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DataTables;

namespace Appa_MVC.Models
{
    public class TestModel
    {
        public const string database = "adamar17y";

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        
        public byte FileId { get; set; }
        public string Name { get; set; }
        public string CreatedDate
        {
            get; set;
        }
        public DateTime? DueDate { get; set; }
        public byte Stage { get; set; }
        public bool Isactive { get; set; }
        public bool IsDeleted { get; set; }
    }
}