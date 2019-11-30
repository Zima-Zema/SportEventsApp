using SportEventsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportEventsApp.ViewModel
{
    public class UsersViewModel
    {
        public UsersViewModel()
        {
            Events = new List<Event>();
            Groups = new List<Group>();
        }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Group> Groups { get; set; }

        public ApplicationUser User { get; set; }


    }
}