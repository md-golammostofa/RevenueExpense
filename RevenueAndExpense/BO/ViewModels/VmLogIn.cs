using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.ViewModels
{
    public class VmLogIn
    {
        [StringLength(50),Required]
        public string UserName { get; set; }
        [StringLength(20), Required]
        public string Password { get; set; }
    }
}