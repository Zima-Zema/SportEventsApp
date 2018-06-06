using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportEventsApp.ViewModel
{
    public class MatchUsersViewModel
    {
        public string Id { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string StoreName { get; set; }

    }
    public class ReturnMatchViewModel
    {
        public ReturnMatchViewModel()
        {
            Users = new List<MatchUsersViewModel>();
        }
        public int Id { get; set; }
        public string CityName { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string StoreOwnerId { get; set; }
        public string CreatorStoreName { get; set; }
        public string CreatorRole { get; set; }
        public int NoOfSlots { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public DateTime Date { get; set; }
        public int EntryFeeValue { get; set; }
        public double Prize { get; set; }
        public string StoreAddress { get; set; }
        public TimeSpan Time { get; set; }
        public string Type { get; set; }
        public List<MatchUsersViewModel> Users { get; set; }

    }
}