using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SportEventsApp.Models
{
    public class Match
    {
        public Match()
        {
            Users = new List<ApplicationUser>();
        }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int? Type { get; set; }
        public int? NofSlots { get; set; }
        public double Price { get; set; }

        public virtual List<ApplicationUser> Users { get; set; }

        [ForeignKey("EntryFee")]
        [Required]
        [Display(Name = "Entry Fees")]
        public int? EntryFeesId { get; set; }
        public virtual EntryFees EntryFee { get; set; }

        [ForeignKey("City")]
        [Required]
        [Display(Name = "City")]
        public string CityId { get; set; }
        public virtual City City { get; set; }

        [ForeignKey("Store")]
        [Display(Name = "Store")]
        public int? StoreId { get; set; }
        public virtual Store Store { get; set; }
    }
}