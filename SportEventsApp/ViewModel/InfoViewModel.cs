using SportEventsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportEventsApp.ViewModel
{
    public class ComplexInfoViewModel
    {
        public ComplexInfoViewModel()
        {
            Titles = new List<string>();
            RegularId = new List<int>();
            RegularInfo = new List<RegularInfoViewModel>();
        }
        public int Id { get; set; }
        public string Header { get; set; }
        public List<RegularInfoViewModel> RegularInfo { get; set; }
        public List<string> Titles { get; set; }
        public List<int> RegularId { get; set; }

    }
    public enum InfoType
    {
        Regular=1,
        Nested
    }
    public class InfoViewModel
    {
        public int Id { get; set; }
        public InfoType Type { get; set; }
        public string Title { get; set; }
       
    }
    public class InfoApiViewModel
    {
        public InfoApiViewModel()
        {
            InfoList = new List<RegularInfoViewModel>();
            Points = new List<string>();
        }
        public int Id { get; set; }
        public InfoType Type { get; set; }
        public string Title { get; set; }
        public List<RegularInfoViewModel> InfoList { get; set; }
        public List<string> Points { get; set; }
    }
}