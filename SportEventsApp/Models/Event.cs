using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SportEventsApp.Models
{
    public class Event
    {
        public Event()
        {
            EventUsers = new List<EventUsers>();
            Groups = new List<Group>();
            Stores = new List<Store>();
            VodafoneCashNumbers = new List<VodafoneCash>();
            EtisalatCashNumbers = new List<EtisalatCash>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name = "First Prize")]
        public int Prize_1 { get; set; }
        [Display(Name = "Second Prize")]
        public int Prize_2 { get; set; }
        [Display(Name = "Third Prize")]
        public int Prize_3 { get; set; }
        
        public bool Full { get; set; } = false;
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public TimeSpan? From { get; set; }
        public TimeSpan? To { get; set; }
        public bool? Published { get; set; }

        [Display(Name = "Entry Fees")]
        public int Entry_Fees { get; set; }
        [Display(Name = "Number Of Player")]
        public int No_Of_Players { get; set; }
        [Required]
        public string Type { get; set; }
        [Display(Name = "Match Duration")]
        public int MatchDuration { get; set; }
        [Display(Name = "Location URL")]
        public string Location_URL { get; set; }
        [JsonIgnore]
        public virtual List<EventUsers> EventUsers { get; set; }

        public virtual List<Store> Stores { get; set; }

        [InverseProperty("Event")]
        [JsonIgnore]
        public virtual List<Group> Groups { get; set; }
        
        [InverseProperty("Event")]
        [JsonIgnore]
        public virtual List<VodafoneCash> VodafoneCashNumbers { get; set; }

        [InverseProperty("Event")][JsonIgnore]
        public virtual List<EtisalatCash> EtisalatCashNumbers { get; set; }
    }
}