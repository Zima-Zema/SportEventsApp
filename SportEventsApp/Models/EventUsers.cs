using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SportEventsApp.Models
{
    public class EventUsers
    {
        public string UserId { get; set; }
        public int EventId { get; set; }
        
        public bool Status { get; set; } = false;
        public string CashNumber { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Event Event { get; set; }

        [ForeignKey("Group")]
        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}