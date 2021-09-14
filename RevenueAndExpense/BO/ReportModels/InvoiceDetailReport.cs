using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ReportModels
{
    public class InvoiceDetailReport
    {
        public DateTime EntryDate { get; set; }
        public string EntryDateBN { get; set; }
        public long FloorId { get; set; }
        public string FloorNo { get; set; }
        public long ShopId { get; set; }
        public string ShopName { get; set; }
        public string BillMonth { get; set; }
        public string InvoiceNo { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public decimal CurrentBill { get; set; }
        public decimal Generator { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal WaterBill { get; set; }
        public decimal Penalty { get; set; }
        public decimal ToiletClean { get; set; }
        public decimal FanCharge { get; set; }
        public decimal Others { get; set; }
        public decimal FireService { get; set; }
        public decimal ChemicalBill { get; set; }
        public decimal CoronaPackage { get; set; }
        public decimal StuffBonus { get; set; }
        public decimal HalfStuffBonus { get; set; }
        public decimal WashRoomRent { get; set; }
        public decimal ReConFee { get; set; }
        public decimal RoadLine { get; set; }
        public decimal NetAmount { get; set; }

        public string CurrentBillBN { get; set; }
        public string GeneratorBN { get; set; }
        public string ServiceChargeBN { get; set; }
        public string WaterBillBN { get; set; }
        public string PenaltyBN { get; set; }
        public string ToiletCleanBN { get; set; }
        public string FanChargeBN { get; set; }
        public string OthersBN { get; set; }
        public string FireServiceBN { get; set; }
        public string ChemicalBillBN { get; set; }
        public string CoronaPackageBN { get; set; }
        public string StuffBonusBN { get; set; }
        public string HalfStuffBonusBN { get; set; }
        public string WashRoomRentBN { get; set; }
        public string ReConFeeBN { get; set; }
        public string RoadLineBN { get; set; }
        public string NetAmountBN { get; set; }
    }
}