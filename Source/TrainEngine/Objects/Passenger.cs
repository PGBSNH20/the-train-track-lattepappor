using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.Objects
{
    public class Passenger
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Passenger(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
