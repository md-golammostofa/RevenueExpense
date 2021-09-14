using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.Models
{
    [Table("tblCashToBankAndBankToCashes")]
    public class CashToBankAndBankToCash
    {
        [Key]
        public long Id { get; set; }
        public double? BankToCash { get; set; }
        public double? CashToBank { get; set; }
        [StringLength(100)]
        public string Remark { get; set; }
        [StringLength(50)]
        public string BankDate { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        [ForeignKey("Bank")]
        public long? BankId { get; set; }
        public Bank Bank { get; set; }

        [ForeignKey("FundInfo")]
        public long? FundInfoId { get; set; }
        public FundInfo FundInfo { get; set; }
        public long OrganizationId { get; set; }
        [StringLength(50)]
        public string ChequeNo { get; set; }
        public bool IsOpeningBalance { get; set; }
    }
}