using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.Models
{
    [Table("tblFloors")]
    public class Floor
    {
        public long FloorId { get; set; }
        [StringLength(100)]
        public string FloorNo { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        // Navigation Property
        [ForeignKey("Organization")]
        public long? OrganizationId { get; set; }
        public Organization Organization { get; set; }

        // Child Table Navigation //
        public ICollection<Holding> Holdings { get; set; }
    }
}