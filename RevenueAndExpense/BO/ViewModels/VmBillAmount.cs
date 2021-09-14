using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmBillAmount
    {
        public long ShopId { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public string ShopName { get; set; }
        public string BillMonth { get; set; }
        public string BillExpireDate { get; set; }
        public decimal CurrentBill { get; set; }
        public decimal Fine { get; set; }
        public decimal ConnectionFee { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalAmountFC { get; set; }
        public string CurrentBillBN { get; set; }
        public string FineBN { get; set; }
        public string ConnectionFeeBN { get; set; }
        public string TotalAmountBN { get; set; }
        public string TotalAmountFCBN { get; set; }
    }
}