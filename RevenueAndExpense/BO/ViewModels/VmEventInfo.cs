using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmEventInfo
    {
        public long EventInfoId { get; set; }
        [StringLength(150)]
        public string EventName { get; set; }
        [StringLength(50),Required]
        public string EventMonth { get; set; }

        [Range(1, double.MaxValue)]
        public decimal Amount { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        [Range(1, long.MaxValue)]
        public long FundInfoId { get; set; }
        public long FundDetailId { get; set; }
        [StringLength(150)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long OrganizationId { get; set; }
        [Required]
        public List<VmEventList> EventDetails { get; set; }
    }
    public class VmEventList
    {
        public long EventInfoId { get; set; }
        public string EventName { get; set; }
        public string MonthAndYear { get; set; }
        public string EventFloors { get; set; }
        public string FundNameBN { get; set; }
        public string FundDetailNameBN { get; set; }
        public string Remarks { get; set; }
        public string EntryUser { get; set; }
        //[Range(1, long.MaxValue)]
        public long? FloorId { get; set; }
        [StringLength(50)]
        public string FloorName { get; set; }
        public decimal Amount { get; set; }
        public string AmountBN { get; set; }
    }
}