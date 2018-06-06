using SportEventsApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportEventsApp.ViewModel
{
    public class UserReturn
    {
        public UserReturn()
        {
            Matches = new List<ReturnMatchViewModel>();
            Events = new List<EventWithStatus>();
            
        }
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string UserName { get; set; }
        [Required]
        public string CityID { get; set; }
        public string CityName { get; set; }
        public string Mobile { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        public List<ReturnMatchViewModel> Matches { get; set; }
        public List<EventWithStatus> Events { get; set; }
        
        public string Role { get; set; }
        public bool ValidUser { get; set; } = false;
    }
}