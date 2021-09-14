using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmUser
    {
        public long UserId { get; set; }
        [StringLength(100),Required]
        public string Name { get; set; }
        [StringLength(50), Required]
        public string UserName { get; set; }
        [StringLength(150)]
        public string Address { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(50)]
        public string MobileNo { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        public bool IsActive { get; set; }
        public bool IsRoleActive { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [Required,Range(1,long.MaxValue)]
        public long OrganizationId { get; set; }
        [Required, Range(1, long.MaxValue)]
        public long RoleId { get; set; }
        public string OrganizationName { get; set; }
        public string RoleName { get; set; }
    }
}