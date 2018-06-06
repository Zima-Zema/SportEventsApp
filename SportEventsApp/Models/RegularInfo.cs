using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SportEventsApp.Models
{
    public class RegularInfo
    {
        public RegularInfo()
        {
            Points = new List<InfoPoint>();
        }
        public int Id { get; set; }
        public string Title { get; set; }

        [InverseProperty("Info")]
        public virtual List<InfoPoint> Points { get; set; }
        [ForeignKey("NestedInfo")]
        public int? NestedId { get; set; }
        public virtual NestedInfo NestedInfo { get; set; }

    }
}