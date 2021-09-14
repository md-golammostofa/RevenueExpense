using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.Models
{
    [Table("tblShopHolding")]
    public class ShopHolding
    {
        [Key,Column(Order =0)]
        public long ShopId { get; set; }
        public Shop Shop { get; set; }

        [Key, Column(Order = 1)]
        public long HoldingId { get; set; }
        public Holding Holding { get; set; }
       
        [StringLength(50)]
        public string EntryUser { get; set; }
        public DateTime? EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? OrganizationId { get; set; }
        public long? FloorId { get; set; }
    }
}