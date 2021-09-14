using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ReportModels
{
    public class ReportInvoiceInfo
    {
        public string InvoiceNo { get; set; }
        public string InvoiceNoBN { get; set; }
        public long ShopId { get; set; }
        public string ShopName { get; set; }
        public string Address { get; set; }
        public string ProprietorName { get; set; }
        public string InvoiceDate { get; set; }
        public decimal TotalFineAmount { get; set; }
        public string TotalFineAmountBN { get; set; }
        public decimal TotalConnectionFee { get; set; }
        public string TotalConnectionFeeBN { get; set; }
        public decimal TotalChargeAmount { get; set; }
        public string TotalChargeAmountBN { get; set; }
        public decimal TotalAmount { get; set; }
        public string TotalAmountBN { get; set; }
        public string TotalInWord { get; set; }

    }

    public class ReportInvoiceDetail
    {
        public string Serial { get; set; }
        public string MonthAndYear { get; set; }
        public decimal ChargeAmount { get; set; }
        public string ChargeAmountBN { get; set; }
        public decimal FineAmount { get; set; }
        public string FineAmountBN { get; set; }
        public decimal ConnectionFee { get; set; }
        public string ConnectionFeeBN { get; set; }
        public decimal NetAmount { get; set; }
        public string NetAmountBN { get; set; }
        public decimal CurrentReading { get; set; }
        public string CurrentReadingBN { get; set; }
        public decimal PreviousReading { get; set; }
        public string PreviousReadingBN { get; set; }
    }
}