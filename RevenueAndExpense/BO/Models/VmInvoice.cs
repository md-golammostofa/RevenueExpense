using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.Models
{
    public class VmInvoiceInfo
    {        
        public long Id { get; set; }
        [StringLength(100)]
        public string InvoiceNo { get; set; }
        [Range(1,long.MaxValue),Required]
        public long FloorId { get; set; }
        [StringLength(100)]
        public string FloorNo { get; set; }
        [Range(1, long.MaxValue), Required]
        public long HoldingId { get; set; }
        [StringLength(100)]
        public string HoldingNo { get; set; }
        public long ShopId { get; set; }
        [StringLength(100)]
        public string ShopName { get; set; }
        public decimal? TotalFineAmount { get; set; }
        public decimal? TotalConnectionFee { get; set; }
        [Range(1,double.MaxValue)]
        public decimal TotalChargeAmount { get; set; }
        [Range(1, double.MaxValue)]
        public decimal TotalAmount { get; set; }
        [Required,StringLength(20)]
        public string TotalFineAmountBN { get; set; }
        [Required, StringLength(20)]
        public string TotalConnectionFeeBN { get; set; }
        [Required, StringLength(50)]
        public string TotalChargeAmountBN { get; set; }
        [Required, StringLength(50)]
        public string TotalAmountBN { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long OrganizationId { get; set; }
    }
    public class VmInvoiceDetail
    {
        public long Id { get; set; }
        [StringLength(100)]
        public string InvoiceId { get; set; }
        [StringLength(100)]
        public string ShopId { get; set; }
        [Required,Range(1, 12)]
        public short Month { get; set; }
        [Required,Range(2019, 2050)]
        public short Year { get; set; }
        [Range(0, double.MaxValue)]
        public decimal ChargeAmount { get; set; }
        public decimal? FineAmount { get; set; }
        public decimal? ConnectionFee { get; set; }
        [Range(0, double.MaxValue)]
        public decimal NetAmount { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [StringLength(50)]
        public string BillExpireDate { get; set; }
        public long InvoiceInfoId { get; set; }
        public long OrganizationId { get; set; }
        [Required, StringLength(50)]
        public string ChargeAmountBN { get; set; }
        [Required, StringLength(20)]
        public string FineAmountBN { get; set; }
        [Required, StringLength(20)]
        public string ConnectionFeeBN { get; set; }
        [Required, StringLength(50)]
        public string NetAmountBN { get; set; }
    }

    public class VmInvoiceDetailForModal
    {
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string ShopNo { get; set; }
        public string Address { get; set; }
        public decimal TotalAmount { get; set; }
        public string TotalAmountBN { get; set; }
        public decimal TotalChargeAmount { get; set; }
        public string TotalChargeAmountBN { get; set; }
        public decimal TotalConnectionFee { get; set; }
        public string TotalConnectionFeeBN { get; set; }
        public decimal TotalFineAmount { get; set; }
        public string TotalFineAmountBN { get; set; }
        public string MonthAndYear { get; set; }
        public decimal ChargeAmount { get; set; }
        public string ChargeAmountBN { get; set; }
        public decimal FineAmount { get; set; }
        public string FineAmountBN { get; set; }
        public decimal ConnectionFee { get; set; }
        public string ConnectionFeeBN { get; set; }
        public decimal NetAmount { get; set; }
        public string NetAmountBN { get; set; }
    }
}