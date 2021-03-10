using System;
using System.Collections.Generic;

namespace TrainEngine.Objects
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEndStation { get; set; }
        public Train? CurrentTrain { get; set; }

        public List<Passenger> WaitingPassangers = new List<Passenger>();
    }
}
