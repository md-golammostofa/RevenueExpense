using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.Models
{
    [Table("tblShopCharges")]
    public class ShopCharge
    {
        [Key]
        public long ShopChargeId { get; set; }
        public long FundInfoId { get; set; }
        public long FundDetailId { get; set; }
        [StringLength(200)]
        public string ChargeName { get; set; }
        public decimal ChargeAmountEN { get; set; }
        public string ChargeAmountBN { get; set; }
        public long OrganizationId { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        [ForeignKey("Shop")]
        public long ShopId { get; set; }
        public Shop Shop { get; set; }
    }
}