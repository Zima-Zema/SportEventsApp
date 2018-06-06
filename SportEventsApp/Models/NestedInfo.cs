using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SportEventsApp.Models
{
    public class NestedInfo
    {
        public NestedInfo()
        {
            InfoList = new List<RegularInfo>();
        }
        public int Id { get; set; }
        public string Header { get; set; }
        [InverseProperty("NestedInfo")]
        public virtual List<RegularInfo> InfoList { get; set; }
    }
}