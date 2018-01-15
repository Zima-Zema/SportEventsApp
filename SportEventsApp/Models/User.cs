using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SportEventsApp.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [RegularExpression(@"^(011|010|012|015)([0-9]{8})$")]
        public string Mobile { get; set; }
        public bool Status { get; set; } = false;

        [ForeignKey("Group")]
        public int? Group_ID { get; set; }
        public virtual Group Group { get; set; }

        [ForeignKey("Event")]
        public int? Event_ID { get; set; }
        public virtual Event Event { get; set; }
        public string CashNumber { get; set; }


    }
}