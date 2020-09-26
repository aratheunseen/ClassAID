using System;
using System.Collections.Generic;
using System.Text;

namespace ClassAid.Models.Schedule
{
    public class EventModel
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
