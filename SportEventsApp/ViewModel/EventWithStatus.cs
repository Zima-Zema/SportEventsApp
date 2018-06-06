using SportEventsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportEventsApp.ViewModel
{
    public class EventWithStatus
    {
        public ReturnEventViewModel Event { get; set; }
        public bool Status { get; set; }
        public string CashNumber { get; set; }
    }
}