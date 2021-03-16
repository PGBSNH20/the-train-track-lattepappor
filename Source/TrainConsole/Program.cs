using System;
using System.Collections.Generic;
using TrainEngine.Objects;
using System.Timers;
using System.Linq;
using System.IO;

namespace TrainConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.WindowWidth * 2, Console.WindowHeight * 2);
            File.WriteAllText(@"C:\Users\Sebastian\source\repos\the-train-track-lattepappor\Source\TrainEngine\Data\controllerlog.txt", "");
            Mr_Carlos carlos = new Mr_Carlos();
            carlos.BeginObserving();

            Console.ReadLine();
        }
    }
}
