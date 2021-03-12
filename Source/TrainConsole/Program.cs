using System;
using System.Collections.Generic;
using TrainEngine.Objects;

namespace TrainConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Train> trains = FileIO.DeserializeTrains(@"Data\trains.txt", ',');
            List<Station> stations = FileIO.DeserializeStations(@"Data\stations.txt", '|');
            List<Passenger> passengers = FileIO.DeserializePassenger(@"Data\passengers.txt", ';', ':');
            List<TimeTable> timeTables = FileIO.DeserializeTimeTables(@"Data\timetable.txt", ',');

            Train train1 = trains[2];

            TrainPlanner trainPlanner1 = new TrainPlanner(train1).CreateTimeTable(timeTables).ToPlan();

            // Step 1:
            // Parse the traintrack (Data/traintrack.txt) using ORM (see suggested code)
            // Parse the trains (Data/trains.txt)

            // Step 2:
            //Make the trains run in treads

            //TrainPlanner plan1 = new TrainPlanner(train1).LoadTimeTable(@"Data\timetable.txt").ToPlan();

            //TrainPlanner plan1 = new TrainPlanner(train1)
            //.CreateTimeTable(departure: "10:20", arrival: null, trainId: train1.Id, stationId: 1)
            //.CreateTimeTable(departure: "10:45", arrival: "10:43", trainId: train1.Id, stationId: 2)
            //.CreateTimeTable(departure: null, arrival: "10:59", trainId: train1.Id, stationId: 3).ToPlan();
        }
    }
}
