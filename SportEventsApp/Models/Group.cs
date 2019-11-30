using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SportEventsApp.Models
{
    public class Group
    {
        public Group()
        {
            EventUsers = new List<EventUsers>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
       

        [ForeignKey("Event")][Required][Display(Name ="Event")]
        public int? Event_ID { get; set; }
        //[JsonIgnore]
        public virtual Event Event { get; set; }

        [InverseProperty("Group")]
        //[JsonIgnore]
        public virtual List<EventUsers> EventUsers { get; set; }

    }
}