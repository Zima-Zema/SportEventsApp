using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportEventsApp.ViewModel
{
    public class RegularInfoViewModel
    {
        public RegularInfoViewModel()
        {
            Points = new List<string>();
        }
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public int? ComplexId { get; set; }
        public List<string> Points { get; set; }

    }
}