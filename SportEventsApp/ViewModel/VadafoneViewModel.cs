using SportEventsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportEventsApp.ViewModel
{
    public class VodafoneViewModel
    {
        public VodafoneViewModel()
        {
            Events = new List<Event>();
        }
        public IEnumerable<Event> Events { get; set; }
        public VodafoneCash Vodafone { get; set; }
    }
}