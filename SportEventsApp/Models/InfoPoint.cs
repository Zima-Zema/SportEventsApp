using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SportEventsApp.Models
{
    public class InfoPoint
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Value { get; set; }
        [ForeignKey("Info")]
        public int? InfoId { get; set; }
        public virtual RegularInfo Info { get; set; }
    }
}