using SportEventsApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportEventsApp.ViewModel
{
    public class EventsDetailsViewModel
    {
        public Event Event { get; set; }
        public List<UserTournamentViewModel> Tours { get; set; }
    }
    public class EventsViewModel
    {
        public EventsViewModel()
        {
            VodafoneCashNumbers = new List<string>();
            EtisalatCashNumbers = new List<string>();
            Stores = new List<StoreDropdown>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "First Prize")]
        public int? Prize_1 { get; set; }
        [Required]
        [Display(Name = "Second Prize")]
        public int? Prize_2 { get; set; }
        [Required]
        [Display(Name = "Third Prize")]
        public int? Prize_3 { get; set; }
        [Required]
        public DateTime? Start { get; set; }
        [Required]
        public DateTime? End { get; set; }
        [Required]
        public TimeSpan? From { get; set; }
        [Required]
        public TimeSpan? To { get; set; }

        [Display(Name = "Entry Fees")]
        [Required]
        public int? EntryFees { get; set; }
        [Display(Name = "No Of Players")]
        [Required]
        public int? NoOfPlayers { get; set; }
        [Required]
        public string Type { get; set; }

        [Required][Display(Name = "Match Duration")]
        public int? Match_Duration { get; set; }

        [Display(Name = "Location")]
        public string Location_URL { get; set; }
        public bool? Published { get; set; } = true;
        public List<string> VodafoneCashNumbers { get; set; }
        public List<string> EtisalatCashNumbers { get; set; }
        [Required][Display(Name ="First Store")]
        public int? Store1 { get; set; }
        [Display(Name = "Second Store")]
        public int? Store2 { get; set; }
        [Display(Name = "Third Store")]
        public int? Store3 { get; set; }
        public IEnumerable<StoreDropdown> Stores { get; set; }



    }
}