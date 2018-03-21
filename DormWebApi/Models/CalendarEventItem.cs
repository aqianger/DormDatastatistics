using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DormWebApi.Models
{
    public class CalendarEventItem
    {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        public decimal rate { get; set; }
        public string className {
            get {
                if (rate < 1.0m)
                {
                    return "blank";
                }
                else if (rate <= 40.0m)
                {                  
                    return "progress-bar-info";
                }
                else if (rate < 65)
                {
                    return "progress-bar-success";
                }
                else if (rate < 85)
                {
                    return "progress-bar-warning";
                }
                return "progress-bar-danger";
            } }
        public bool allDay { get { return true; } }
    }
}