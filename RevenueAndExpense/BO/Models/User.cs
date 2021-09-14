using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.Models
{
    [Table("tblUsers")]
    public class User
    {
        public long UserId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }
        [StringLength(150)]
        public string Address { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(50)]
        public string MobileNo { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsActive { get; set; }
        public bool IsRoleActive { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        [ForeignKey("Organization")]
        public long OrganizationId { get; set; }
        public Organization Organization { get; set; }

        [ForeignKey("Role")]
        public long RoleId { get; set; }
        public Role Role  { get; set; }
    }
}