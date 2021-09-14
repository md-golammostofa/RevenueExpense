using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmHolding
    {
        public long HoldingId { get; set; }
        [StringLength(100),Required]
        public string HoldingName { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public DateTime? EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        [Required]
        public long? FloorId { get; set; }
        [StringLength(100),Required]
        public string FloorNo { get; set; }
        public long? OrganizationId { get; set; }
        [StringLength(100)]
        public string OrganizationName { get; set; }
    }
}