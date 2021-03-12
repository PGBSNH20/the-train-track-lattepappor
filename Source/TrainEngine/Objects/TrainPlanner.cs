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

        public ITrainPlanner CreateTimeTable(List<TimeTable> timeTables)
        {
            Table = timeTables.Where(x => x.TrainId == Train.Id).ToList();

            return this;
        }

        public TrainPlanner ToPlan()
        {
            return this;
        }
    }
}
