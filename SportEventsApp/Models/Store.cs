using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SportEventsApp.Models
{
    public class Store
    {
        public Store()
        {
            Photos = new List<StorePhotos>();
            Matches = new List<Match>();
            Events = new List<Event>();
        }
        [Key]
        public int Id { get; set; }
        [Display(Name ="Name")]
        public string StoreName { get; set; }
        public string Address { get; set; }
        [Display(Name ="Number Of Devices")]
        public int? NumberOfDevices { get; set; }
        public double? HoureFees { get; set; }
        public int WorkingHours { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public DayOfWeek From { get; set; }
        public DayOfWeek To { get; set; }
        public bool? Approved { get; set; }

        [ForeignKey("City")]
        [Required]
        [Display(Name = "City")]
        public string CityId { get; set; }
        public virtual City City { get; set; }

        [InverseProperty("Store")]
        public virtual List<StorePhotos> Photos { get; set; }

        [InverseProperty("Store")]
        public virtual List<Match> Matches { get; set; }

        [InverseProperty("Store")]
        public virtual List<Event> Events { get; set; }
        [ForeignKey("Owner")]
        [Display(Name = "Owner")]
        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }



    }
}