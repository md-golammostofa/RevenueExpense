using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmShop
    {
        public long ShopId { get; set; }
        [StringLength(100),Required]
        public string ShopName { get; set; }
        [StringLength(100), Required]
        public string ProprietorName { get; set; }
        [StringLength(100), Required]
        public string MobileNo { get; set; }
        [StringLength(100)]
        public string PhoneNo { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(150)]
        public string HomeAddress { get; set; }
        [StringLength(100)]
        public string RegistrationNo { get; set; }
        [StringLength(50), Required]
        public string StateStatus { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string HoldingNo { get; set; }
        [Required]
        public long? FloorId { get; set; }
        public string FloorName { get; set; }
        public long? OrganizationId { get; set; }
        public string OrganizationName { get; set; }

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
        [StringLength(200),Required]
        public string MeterNo { get; set; }
        [StringLength(100)]
        public string ShopSize { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }

        public string Month { get; set; }
        public string Year { get; set; }
    }

    public class VmShopHolding
    {
        [Required]
        public  long ShopId { get; set; }
        [Required]
        public long HoldingId { get; set; }
        public string HoldingNo { get; set; }
    }

    public class VmShopModel
    {
        public VmShop Shop { get; set; }
        public List<VmShopHolding> ShopHoldings { get; set; }
        public List<VmShopCharge> ShopCharges { get; set; }
    }

    public class VmShopCharge
    {
        public long ShopChargeId { get; set; }
        public long FundInfoId { get; set; }
        [Required]
        public long FundDetailId { get; set; }
        [StringLength(200)]
        public string ChargeName { get; set; }
        public decimal ChargeAmountEN { get; set; }
        public string ChargeAmountBN { get; set; }
        public long OrgnizationId { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [Required]
        public long ShopId { get; set; }
        public string ShopName { get; set; }

        [StringLength(200)]
        public string FundDetailName { get; set; }
        [StringLength(200)]
        public string FundDetailNameBN { get; set; }
        [StringLength(100)]
        public string FundName { get; set; }
        [StringLength(100)]
        public string FundNameBN { get; set; }
        public string ProprietorName { get; set; }
    }

    public class VmShopWiseCharge
    {
        public VmShop Shop { get; set; }
        public List<VmShopCharge> Charges { get; set; }
    }

}