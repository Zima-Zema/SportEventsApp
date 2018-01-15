using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportEventsApp.ViewModel
{
    public class EventsViewModel
    {
        public EventsViewModel()
        {
            VodafoneCashNumbers = new List<string>();
            EtisalatCashNumbers = new List<string>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Prize_1 { get; set; }
        public int Prize_2 { get; set; }
        public int Prize_3 { get; set; }
        [Required]
        public string Host_1 { get; set; }
        public string Host_2 { get; set; }
        public string Host_3 { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int Entry_Fees { get; set; }
        public int No_Of_Players { get; set; }
        [Required]
        public string Type { get; set; }
        public int Match_Duration { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Location_URL { get; set; }

        public List<string> VodafoneCashNumbers { get; set; }
        public List<string> EtisalatCashNumbers { get; set; }

    }
}