using System;
using System.Collections.Generic;
using TrainEngine.Objects;
using System.Timers;
using System.Linq;

namespace TrainConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Mr_Carlos carlos = new Mr_Carlos();
            carlos.BeginObserving();

            Console.ReadLine();
        }
    }
}
