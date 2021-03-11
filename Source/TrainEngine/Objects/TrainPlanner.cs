using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        //public ITrainPlanner CreateTimeTable(string departure, string arrival, int trainId, int stationId)
        //{
        //    TimeTable table = new (){ DepartureTime = departure != null ? DateTime.Parse(departure) : null, ArrivalTime = arrival != null ? DateTime.Parse(arrival) : null, TrainId = trainId, StationId = stationId };
        //    Table.Add(table);
        //    return this;
        //}

        public ITrainPlanner LoadTimeTable(string path)
        {
            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines)
            {
                string[] columns = line.Split(',');
                int trainId = int.Parse(columns[0]);
                int stationId = int.Parse(columns[1]);
                string departure = columns[2] != "null" ? columns[2] : null;
                string arrival = columns[3] != "null" ? columns[3] : null; ;

                TimeTable table = new() { DepartureTime = departure != null ? DateTime.Parse(departure) : null, ArrivalTime = arrival != null ? DateTime.Parse(arrival) : null, TrainId = trainId, StationId = stationId };
                Table.Add(table);

                Console.WriteLine($"{trainId}, {stationId}, {departure ?? "null"}, {arrival ?? "null"}");
            }
            return this;
        }

        public TrainPlanner ToPlan()
        {
            return this;
        }
    }
}
