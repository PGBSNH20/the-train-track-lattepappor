using System;
using TrainEngine.Objects;

namespace TrainConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Train track!");
            // Step 1:
            // Parse the traintrack (Data/traintrack.txt) using ORM (see suggested code)
            // Parse the trains (Data/trains.txt)

            // Step 2:
            // Make the trains run in treads

            Train train1 = new() { Id = 1, Name = "Lapplandståget", Operated = true, TopSpeed = 50 };

            TrainPlanner plan1 = new TrainPlanner(train1)
            .CreateTimeTable(departure: "10:20", arrival: null, trainId: train1.Id, stationId: 1)
            .CreateTimeTable(departure: "10:45", arrival: "10:43", trainId: train1.Id, stationId: 2)
            .CreateTimeTable(departure: null, arrival: "10:59", trainId: train1.Id, stationId: 3).ToPlan();

            Console.WriteLine(plan1.Train.Name);
        }
    }
}
