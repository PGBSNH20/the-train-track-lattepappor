using System;
using System.Collections.Generic;
using System.Text;
using TrainEngine.Objects;

namespace TrainEngine
{
        public interface ITrainPlanner
        {
            public ITrainPlanner CreateTimeTable(string departure, string arrival, int trainId, int stationId);

            public TrainPlanner ToPlan();
        }
}
