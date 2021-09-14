using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.Models
{
    [Table("tblMonthWiseShopCharges")]
    public class MonthWiseShopCharge
    {
        [Key]
        public long ChargeId { get; set; }
        [StringLength(200)]
        public string ChargeName { get; set; }
        public decimal PreviousReading { get; set; }
        public decimal CurrentReading { get; set; }
        public decimal ConsumUnit { get; set; }
        public decimal UnitRate { get; set; }
        public decimal Amount { get; set; }
        public long  FundInfoId{ get; set; }
        public long FundDetailId { get; set; }
        public long OrganizationId { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }

        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [StringLength(30)]
        public string Remark { get; set; }
        [StringLength(30)]
        public string AmountBN { get; set; }

        [ForeignKey("Shop")]
        public long ShopId { get; set; }
        public Shop Shop { get; set; }
        [StringLength(30)]
        public string StateStatus { get; set; }
        [StringLength(30)]
        public string BillNo { get; set; }
        [StringLength(30)]
        public string BillExpireDate { get; set; }
        [StringLength(50)]
        public string InvoiceNo { get; set; }
        public bool? IsItEventCharge { get; set; }
        public long? EventId { get; set; }
    }
}