using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportEventsApp.Models
{
    public class City
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string ArName { get; set; }
        public string EnName { get; set; }

    }
}