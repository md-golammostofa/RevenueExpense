using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.Models
{
    [Table("tblMainmenus")]
    public class Mainmenu
    {
        [Key]
        public long MenuId { get; set; }
        [StringLength(100)]
        public string MenuName { get; set; }
        [StringLength(100)]
        public string  IconClass { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<Submenu> Submenus { get; set; }
    }
}