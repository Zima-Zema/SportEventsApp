using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SportEventsApp.Models
{
    public class EntryFees
    {
        public EntryFees()
        {
            Matches = new List<Match>();
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Value { get; set; }

        [InverseProperty("EntryFee")]
        public virtual List<Match> Matches { get; set; }

    }
}