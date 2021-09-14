using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmOrganization
    {
        public Int64 OrganizationId { get; set; }
        [StringLength(100),Required]
        public string OrganizationName { get; set; }
        [StringLength(100), Required]
        public string Address { get; set; }
        [StringLength(100),DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [StringLength(50), Required]
        public string MobileNo { get; set; }
        [StringLength(50), Required]
        public string PhoneNo { get; set; }
        [StringLength(50)]
        public string Fax { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        [StringLength(150)]
        public string RegistrationBN { get; set; }

        [StringLength(150)]
        public string LicenceBN { get; set; }

        [StringLength(150)]
        public string Registration { get; set; }

        [StringLength(150)]
        public string Licence { get; set; }
        [StringLength(200)]
        public string OrganizationNameBN { get; set; }
        [StringLength(200)]
        public string AddressBN { get; set; }
    }
}