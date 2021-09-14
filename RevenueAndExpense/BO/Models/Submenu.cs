using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.Models
{
    [Table("tblSubmenus")]
    public class Submenu
    {
        public long SubmenuId { get; set; }
        [StringLength(100)]
        public string SubmenuName { get; set; }
        [StringLength(100)]
        public string IconClass { get; set; }
        public bool IsViewable { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        [ForeignKey("Mainmenu")]
        public long MainmenuId { get; set; }
        public Mainmenu Mainmenu { get; set; }

        [StringLength(150)]
        public string ControllerName { get; set; }
        [StringLength(150)]
        public string ActionName { get; set; }


        //public int MyProperty { get; set; }
    }
}