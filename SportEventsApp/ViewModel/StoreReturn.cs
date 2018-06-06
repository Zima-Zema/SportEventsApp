using SportEventsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportEventsApp.ViewModel
{
    public class StoreReturn
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public int? NumberOfDevices { get; set; }
        public double? HoureFees { get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
        public DayOfWeek From { get; set; }
        public DayOfWeek To { get; set; }
        public bool? Approved { get; set; }
        public bool ValidUser { get; set; }
        public UserReturn Owner { get; set; }
        public City City { get; set; }
        public List<StorePhotos> Photos { get; set; }
        public virtual List<ReturnMatchViewModel> Matches { get; set; }
        public virtual List<ReturnEventViewModel> Events { get; set; }
    }
}