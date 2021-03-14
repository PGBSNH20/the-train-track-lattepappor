﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainEngine.Objects
{
    public class TrainPlanner : ITrainPlanner
    {
        public List<TimeTable> Table = new();
        public Dictionary<DateTime, (Switch, bool)> ChangeSwitchAt = new();
        public Train Train { get; set; }
        public LevelCrossing LevelCrossing { get; set; }
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

        public ITrainPlanner CrossingPlan(LevelCrossing levelCrossing, string close, string open)
        {
            LevelCrossing = levelCrossing;
            CrossingCloseAt = DateTime.Parse(close);
            CrossingOpenAt = DateTime.Parse(open);
            return this;
        }

        public ITrainPlanner SwitchPlan(Switch trackSwitch, string time,  bool directionLeft)
        {
            ChangeSwitchAt.Add(DateTime.Parse(time), (trackSwitch, directionLeft));
            return this;
        }

        public TrainPlanner ToPlan()
        {
            return this;
        }
    }
}
