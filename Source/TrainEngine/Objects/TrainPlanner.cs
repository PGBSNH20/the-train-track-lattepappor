using System;
using System.Collections.Generic;
//using TrainEngine.Objects;

namespace TrainEngine.Objects
{
    public class TrainPlanner : ITrainPlanner
    {
        public List<TimeTable> Table = new ();
        public Train Train { get; set; }

        public TrainPlanner(Train train)
        {
            Train = train;
        }

        public ITrainPlanner CreateTimeTable(string departure, string arrival, int trainId, int stationId)
        {
            TimeTable table = new (){ DepartureTime = departure != null ? DateTime.Parse(departure) : null, ArrivalTime = arrival != null ? DateTime.Parse(arrival) : null, TrainId = trainId, StationId = stationId };
            Table.Add(table);
            return this;
        }

        public TrainPlanner ToPlan()
        {
            return this;
        }
    }
}
