using System;
using System.Collections.Generic;

namespace TrainEngine.Objects
{
    public class Train
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TopSpeed { get; set; }
        public bool Operated { get; set; }

        public List<Passenger> Passengers = new ();

        public Train(int id, string name, int topSpeed, bool operated)
        {
            this.Id = id;
            this.Name = name;
            this.TopSpeed = topSpeed;
            this.Operated = operated;
        }
    }
}
