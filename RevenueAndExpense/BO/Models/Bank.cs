using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.Models
{
    [Table("tblBanks")]
    public class Bank
    {
        [Key]
        public long BankId { get; set; }
        [StringLength(150)]
        public string BankName { get; set; }
        [StringLength(200)]
        public string BranchName { get; set; }
        [StringLength(100)]
        public string AccountNo { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [ForeignKey("Organization")]
        public long OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public ICollection<CashToBankAndBankToCash> CashToBankAndBankToCashes { get; set; }
    }
}