using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.Models
{
    [Table("tblInvoiceInfo")]
    public class InvoiceInfo
    {
        [Key]
        public long Id { get; set; }
        [StringLength(100)]
        public string InvoiceNo { get; set; }
        public long? FloorId { get; set; }

        [StringLength(100)]
        public string FloorNo { get; set; }
        public long? HoldingId { get; set; }
        [StringLength(100)]
        public string HoldingNo { get; set; }
        public long ShopId { get; set; }
        public decimal? TotalFineAmount { get; set; }
        public decimal? TotalConnectionFee { get; set; }
        public decimal TotalChargeAmount { get; set; }
        public decimal TotalAmount { get; set; }
        [StringLength(20)]
        public string TotalFineAmountBN { get; set; }
        [StringLength(20)]
        public string TotalConnectionFeeBN { get; set; }
        [StringLength(50)]
        public string TotalChargeAmountBN { get; set; }
        [StringLength(50)]
        public string TotalAmountBN { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string  UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        [ForeignKey("Organization")]
        public long OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }

    [Table("tblInvoiceDetails")]
    public class InvoiceDetail
    {
        [Key]
        public long Id { get; set; }
        [StringLength(100)]
        public string InvoiceNo { get; set; }
        [StringLength(100)]
        public string ShopId { get; set; }
        [Range(1,12)]
        public short Month { get; set; }
        [Range(2019, 2050)]
        public short Year { get; set; }
        public decimal ChargeAmount { get; set; }
        public decimal? FineAmount { get; set; }
        public decimal? ConnectionFee { get; set; }
        public decimal NetAmount { get; set; }
        [StringLength(50)]
        public string ChargeAmountBN { get; set; }
        [StringLength(20)]
        public string FineAmountBN { get; set; }
        [StringLength(20)]
        public string ConnectionFeeBN { get; set; }
        [StringLength(50)]
        public string NetAmountBN { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [StringLength(50)]
        public string BillExpireDate { get; set; }
        public long OrganizationId { get; set; }

        [ForeignKey("InvoiceInfo")]
        public long InvoiceInfoId { get; set; }
        public InvoiceInfo InvoiceInfo { get; set; }
    }
}