﻿using System;
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
            Users = new List<ApplicationUser>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [InverseProperty("Group")]
        public virtual List<ApplicationUser> Users { get; set; }

        [ForeignKey("Event")][Required][Display(Name ="Event")]
        public int? Event_ID { get; set; }
        public virtual Event Event { get; set; }

    }
}