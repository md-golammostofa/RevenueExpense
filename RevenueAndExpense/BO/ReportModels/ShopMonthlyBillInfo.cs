using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ReportModels
{
    public class ShopMonthlyBillInfo
    {
        public string ShopName { get; set; }
        public string ProprietorName { get; set; }
        public long ShopId { get; set; }
        public long FloorId { get; set; }
        public string FloorNo { get; set; }
        public long OrganizationId { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public string FundDetailNameBN { get; set; }
        public decimal CurrentReading { get; set; }
        public decimal PreviousReading { get; set; }
        public string CurrentReadingBN { get; set; }
        public string PreviousReadingBN { get; set; }
        public decimal ConsumUnit { get; set; }
        public string DataEndBN { get; set; }
        public string DateStartBN { get; set; }
        public string ConsumUnitBN { get; set; }
        public string BillDate { get; set; }
        public string MonthNameBN { get; set; }
        public decimal Amount { get; set; }
        public string AmountBN { get; set; }
        public string Address { get; set; }
        public string BillNo { get; set; }
        public string AmountInWord { get; set; }
        public string BillEntryDateBN { get; set; }
        public Nullable<DateTime> BillEntryDateEN { get; set; }
    }
}