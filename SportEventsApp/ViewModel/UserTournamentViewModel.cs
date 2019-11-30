using SportEventsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportEventsApp.ViewModel
{
    public class UserTournamentViewModel
    {
        public string UserId { get; set; }
        public int EventId { get; set; }
        public int? GroupId { get; set; }
        public string CashNumber { get; set; }
        public bool Status { get; set; }
        public ApplicationUser User { get; set; }
        public Event Event { get; set; }
        public Group Group { get; set; }
        public List<Group> Groups { get; set; }
    }
}