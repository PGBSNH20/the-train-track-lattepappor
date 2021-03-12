using System;
using System.Collections.Generic;
using System.Text;
using TrainEngine.Objects;

namespace TrainEngine
{
        public interface ITrainPlanner
        {
        public ITrainPlanner CreateTimeTable(List<TimeTable> timeTables);
        public ITrainPlanner CrossingPlan(LevelCrossing levelCrossing, string close, string open);
        public TrainPlanner ToPlan();
        }
}
