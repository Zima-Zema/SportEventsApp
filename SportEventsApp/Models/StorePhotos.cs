using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SportEventsApp.Models
{
    public class StorePhotos
    {
        public int Id { get; set; }
        public string Url { get; set; }

        [ForeignKey("Store")]
        [Required]
        [Display(Name = "Store")]
        public int? StoreId { get; set; }
        [JsonIgnore]
        public virtual Store Store { get; set; }
    }
}