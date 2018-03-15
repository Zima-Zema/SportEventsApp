using SportEventsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportEventsApp.ViewModel
{
    public class GroupsViewModel
    {
        public GroupsViewModel()
        {
            Events = new List<Event>();
        }
        public IEnumerable<Event> Events { get; set; }
        public Group Group { get; set; }
    }
}