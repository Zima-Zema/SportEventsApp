using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SportEventsApp.Models;
namespace SportEventsApp.ViewModel
{
    public class EtisalateViewModel
    {
        public EtisalateViewModel()
        {
            Events = new List<Event>();
        }
        public IEnumerable<Event> Events { get; set; }
        public EtisalatCash Etisalat { get; set; }
    }
}