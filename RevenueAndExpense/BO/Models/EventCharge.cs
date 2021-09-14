using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.Models
{
    [Table("tblEventInfo")]
    public class EventInfo
    {
        [Key]
        public long EventInfoId { get; set; }
        [StringLength(150)]
        public string EventName { get; set; }
        public short Month{ get; set; }
        public short Year { get; set; }
        public decimal Amount { get; set; }
        [StringLength(50)]
        public string AmountBN { get; set; }

        [StringLength(150)]
        public string Remarks { get; set; }
        public long FundInfoId { get; set; }
        public long FundDetailId { get; set; }
        [StringLength(150)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long OrganizationId { get; set; }
        public ICollection<EventDetail> EventDetails { get; set; }
    }

    [Table("tblEventDetail")]
    public class EventDetail
    {
        [Key]
        public long EventDetailId { get; set; }
        public long FloorId { get; set; }
        public decimal Amount { get; set; }
        [StringLength(150)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }

        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        public long OrganizationId { get; set; }

        [ForeignKey("EventInfo")]
        public long EventInfoId { get; set; }
        public EventInfo EventInfo { get; set; }
    }
}