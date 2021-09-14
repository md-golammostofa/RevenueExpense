using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BO.DataObject
{
    public class Dropdown
    {
        public string value { get; set; }
        public string text { get; set; }
    }

    public class AutoCompleteObj
    {
        public string value { get; set; }
        public string text { get; set; }
        public string amount{ get; set; }
    }
}