using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.Models
{
    [Table("tblFundInfo")]
    public class FundInfo
    {
        [Key]
        public long FundInfoId { get; set; }
        [StringLength(100)]
        public string FundName { get; set; }
        [StringLength(100)]
        public string FundNameBN { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        // Navigation Property
        [ForeignKey("Organization")]
        public long OrganizationId { get; set; }
        public Organization Organization { get; set; }

        // Child Table Navigation
        public ICollection<FundDetail> FundDetails { get; set; }
        public ICollection<CashToBankAndBankToCash> CashToBankAndBankToCashes { get; set; }
    }
}