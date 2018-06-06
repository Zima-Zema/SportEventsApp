using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportEventsApp.ViewModel
{
    public class ReturnEventStores
    {
        public string storeOwnerId { get; set; }
        public string storeName { get; set; }
        public string cityName { get; set; }
    }
    public class ReturnEventViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Prize1 { get; set; }
        public int Prize2 { get; set; }
        public int Prize3 { get; set; }
        public bool Full { get; set; } = false;
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public TimeSpan? From { get; set; }
        public TimeSpan? To { get; set; }
        public bool? Published { get; set; }
        public int EntryFees { get; set; }
        public int NoOfPlayers { get; set; }
        public string Type { get; set; }
        public int MatchDuration { get; set; }
        public virtual List<ReturnEventStores> Stores { get; set; }
    }
}