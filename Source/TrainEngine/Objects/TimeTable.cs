using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.Objects
{
    public class TimeTable
    {
        public int TrainId { get; set; }
        public int StationId { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
    }
}
