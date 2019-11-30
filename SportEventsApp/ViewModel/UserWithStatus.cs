using SportEventsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportEventsApp.ViewModel
{
    public class UserWithStatus
    {
        public ApplicationUser User { get; set; }
        public bool Status { get; set; }
        public string CashNumber { get; set; }
    }
}