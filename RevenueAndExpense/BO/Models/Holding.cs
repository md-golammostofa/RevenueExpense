using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.Models
{
    [Table("tblHoldings")]
    public class Holding
    {
        public long HoldingId { get; set; }
        [StringLength(100)]
        public string HoldingName { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public DateTime? EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        // Navigation Propeties //
        // Parent table navigation.
        [ForeignKey("Floor")]
        public long? FloorId { get; set; }
        public Floor Floor { get; set; }

        [ForeignKey("Organization")]
        public long? OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}