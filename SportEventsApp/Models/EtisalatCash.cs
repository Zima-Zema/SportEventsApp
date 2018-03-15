using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SportEventsApp.Models
{
    public class EtisalatCash
    {
        public int Id { get; set; }
        [RegularExpression(@"^(011)([0-9]{8})$")]
        public string Number { get; set; }
        [Range(0,12)]
        [Display(Name ="Limit")]
        public int Count { get; set; } = 0;

        [ForeignKey("Event")]
        [Display(Name ="Event")]
        public int? Event_ID { get; set; }
        public virtual Event Event { get; set; }
    }
}