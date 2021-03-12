using System;
using System.Collections.Generic;
using System.Text;
using TrainEngine.Objects;

namespace TrainEngine
{
        public interface ITrainPlanner
        {

        public ITrainPlanner CreateTimeTable(List<TimeTable> timeTables);

        public TrainPlanner ToPlan();
        }
}
