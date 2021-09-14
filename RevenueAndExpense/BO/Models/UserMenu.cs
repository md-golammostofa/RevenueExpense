using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.Models
{
    [Table("tblUserMenus")]
    public class UserMenu
    {
        [Key]
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public long MainmenuId { get; set; }
        public long SubmenuId { get; set; }
        public long UserId { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
}