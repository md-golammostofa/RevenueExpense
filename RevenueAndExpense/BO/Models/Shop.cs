using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.Models
{
    [Table("tblShops")]
    public class Shop
    {
        [Key]
        public long ShopId { get; set; }
        [StringLength(100)]
        public string ShopName { get; set; }
        [StringLength(100)]
        public string ProprietorName { get; set; }
        [StringLength(100)]
        public string MobileNo { get; set; }
        [StringLength(100)]
        public string PhoneNo { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(150)]
        public string HomeAddress { get; set; }
        [StringLength(100)]
        public string RegistrationNo{ get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        // Navigation Propeties //
        // Parent table navigation.
        [ForeignKey("Floor")]
        public long? FloorId { get; set; }
        public Floor Floor { get; set; }

        [ForeignKey("Organization")]
        public long? OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public ICollection<ShopCharge> ShopCharges { get; set; }
        public ICollection<MonthWiseShopCharge> monthWiseShopCharges { get; set; }

        // New Fields 20-Jan-2020 //
        [StringLength(100)]
        public string LandOwner { get; set; }
        [StringLength(50)]
        public string LandOwnerMobile { get; set; }
        [StringLength(200)]
        public string LandOwnerAddress { get; set; }

        //
        [StringLength(100)]
        public string LeaseholderName { get; set; }
        [StringLength(200)]
        public string LeaseholderAddress { get; set; }
        [StringLength(50)]
        public string LeaseholderPhone { get; set; }

        public string ImagePath { get; set; }
        [StringLength(50)]
        public string NIDNo { get; set; }
        public string FilePath { get; set; }
        [StringLength(200)]
        public string MeterNo { get; set; }
        [StringLength(100)]
        public string ShopSize { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }

    }
}