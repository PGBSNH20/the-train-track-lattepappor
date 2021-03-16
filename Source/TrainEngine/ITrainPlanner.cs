using System;
using System.Collections.Generic;
using System.Text;
using TrainEngine.Objects;

namespace TrainEngine
{
        public interface ITrainPlanner
        {
        public ITrainPlanner CreateTimeTable(List<TimeTable> timeTables);
        public ITrainPlanner CrossingPlan(string close, string open);
        public ITrainPlanner SwitchPlan(Switch trackSwitch, string time, Switch.Direction direction);
        public TrainPlanner ToPlan();
        }
}
