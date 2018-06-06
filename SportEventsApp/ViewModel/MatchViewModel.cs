using SportEventsApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportEventsApp.ViewModel
{
    public class MatchesIndex
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan Time { get; set; }
        [Display(Name ="City Name")]
        public string CityName { get; set; }
        [Display(Name ="Creator Name")]
        public string CreatorName { get; set; }
        [Display(Name ="Creator Number")]
        public string CreatorUserName { get; set; }
    }
    public class MatchViewModel
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Type { get; set; }
        public int? NofSlots { get; set; }
        public double Prize { get; set; }
        public int? EntryFeesId { get; set; }
        [Required]
        public string CityId { get; set; }
        public string CityName { get; set; }
        [Required]
        public int? StoreId { get; set; }
        public string StoreName { get; set; }

        public int? NumberOfUsres { get; set; }
        [Required]
        public string CreatorId { get; set; }
        public string CreatorUserName { get; set; }
        public string CreatorName { get; set; }

    }
    public class NewMatchViewModel
    {
        public NewMatchViewModel()
        {
            Cities = new List<CitiesDropDown>();
           
            Fees = new List<EntryFees>();
        }
        public int Id { get; set; } = 0;
        [Required]
        public DateTime? Date { get; set; }
        [Required]
        public TimeSpan? Time { get; set; }

        [Display(Name = "Game Type")][Required]
        public string Type { get; set; } //dropDown

        [Display(Name = "Game Mode")][Required]
        public int? NofSlots { get; set; } //Game Mode //dropDown

        public double Prize { get; set; }

        [Display(Name = "Entry Fees")][Required]
        public int? EntryFeesId { get; set; }

        [Display(Name = "City")][Required]
        public string CityId { get; set; }

        [Display(Name = "Hosting Store")][Required]
        public int? StoreId { get; set; }
        [Display(Name ="Creator")]
        public string CreatorId { get; set; }

        public List<CitiesDropDown> Cities { get; set; }
        
        //public List<CreatorDropDown> Creators { get; set; }
        public List<EntryFees> Fees { get; set; }

    }
    
    public class CitiesDropDown
    {
        public string Id { get; set; }
        public string Name { get; set; }

    }

    public class CreatorDropDown
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }


}