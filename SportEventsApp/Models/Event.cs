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
            Users = new List<User>();
            Groups = new List<Group>();
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
        [Required]
        [Display(Name = "First Host")]
        public string Host_1 { get; set; }
        [Display(Name = "Second Host")]
        public string Host_2 { get; set; }
        [Display(Name = "Third Host")]
        public string Host_3 { get; set; }

        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        [Display(Name = "Entry Fees")]
        public int Entry_Fees { get; set; }
        public int No_Of_Players { get; set; }
        [Required]
        public string Type { get; set; }
        [Display(Name = "Match Duration")]
        public int Match_Duration { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Location URL")]
        public string Location_URL { get; set; }

        [InverseProperty("Event")]
        public virtual List<User> Users { get; set; }


        [InverseProperty("Event")]
        public virtual List<Group> Groups { get; set; }


        [InverseProperty("Event")]
        public virtual List<VodafoneCash> VodafoneCashNumbers { get; set; }

        [InverseProperty("Event")]
        public virtual List<EtisalatCash> EtisalatCashNumbers { get; set; }
    }
}