using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportEventsApp.Models
{
    public class EventUsers
    {
        public string UserId { get; set; }
        public int EventId { get; set; }
        public bool Status { get; set; } = false;

        public virtual ApplicationUser User { get; set; }
        public virtual Event Event { get; set; }
    }
}