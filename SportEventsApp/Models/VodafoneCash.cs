using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SportEventsApp.Models
{
    public class VodafoneCash
    {
        public int Id { get; set; }
        [RegularExpression(@"^(010)([0-9]{8})$")]
        public string Number { get; set; }
        public int Count { get; set; } = 0;

        [ForeignKey("Event")]
        public int Event_ID { get; set; }
        public virtual Event Event { get; set; }

    }
}