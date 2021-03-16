using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainEngine.Objects
{
    public class TrainPlanner : ITrainPlanner
    {
        public List<TimeTable> Table = new();
        public Dictionary<DateTime, (Switch, Switch.Direction)> ChangeSwitchAt = new();
        public Train Train { get; set; }
        public DateTime CrossingOpenAt { get; set; }
        public DateTime CrossingCloseAt { get; set; }
        public TrainPlanner(Train train)
        {
            Train = train;
        }

        public ITrainPlanner CreateTimeTable(List<TimeTable> timeTables)
        {
            Table = timeTables.Where(x => x.TrainId == Train.Id).ToList();
            return this;
        }

        public ITrainPlanner CrossingPlan(string close, string open)
        {
            CrossingCloseAt = DateTime.Parse(close);
            CrossingOpenAt = DateTime.Parse(open);
            return this;
        }

        public ITrainPlanner SwitchPlan(Switch trackSwitch, string time,  Switch.Direction direction)
        {
            ChangeSwitchAt.Add(DateTime.Parse(time), (trackSwitch, direction));
            return this;
        }

        public TrainPlanner ToPlan()
        {
            return this;
        }
    }
}
