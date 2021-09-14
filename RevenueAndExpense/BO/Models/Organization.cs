using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.Models
{
    [Table("tblOrganizations")]
    public class Organization
    {
        public Int64 OrganizationId { get; set; }
        [StringLength(100)]
        public string OrganizationName { get; set; }
        [StringLength(200)]
        public string OrganizationNameBN { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        [StringLength(200)]
        public string AddressBN { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(50)]
        public string MobileNo { get; set; }
        [StringLength(50)]
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

        // child table navigation
        public ICollection<Floor> Floors { get; set; }
        public ICollection<Holding> Holdings { get; set; }
        public ICollection<Shop> Shops { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<FundInfo> FundInfos { get; set; }
        public ICollection<Bank> Banks { get; set; }
    }
}