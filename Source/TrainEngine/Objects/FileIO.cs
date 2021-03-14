using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace TrainEngine.Objects
{
    public class FileIO
    {
        public static List<Train> DeserializeTrains(string filePath, char separator)
        {
            string[] trains;
            List<Train> trainList = new List<Train>();
            trains = File.ReadAllLines(filePath);

            foreach (string lines in trains.Skip(1))
            {
                string[] parts = lines.Split(separator);
                int id = int.Parse(parts[0]);
                string name = parts[1];
                int topSpeed = int.Parse(parts[2]);
                bool operated = bool.Parse(parts[3]);
                trainList.Add(new Train(id, name, topSpeed, operated));
            }
            return trainList;
        }

        public static List<Station> DeserializeStations(string filePath, char separator)
        {
            string[] stations;
            List<Station> stationList = new List<Station>();
            stations = File.ReadAllLines(filePath);

            foreach (string lines in stations.Skip(1))
            {
                string[] parts = lines.Split(separator);
                int id = int.Parse(parts[0]);
                string stationName = parts[1];
                bool isEndstation = bool.Parse(parts[2]);
                stationList.Add(new Station(id, stationName, isEndstation));
            }

            return stationList;
        }

        public static List<TimeTable> DeserializeTimeTables(string filePath, char separator)
        {
            string[] timeTables;
            List<TimeTable> timeTableList = new List<TimeTable>();
            timeTables = File.ReadAllLines(filePath);

            foreach (string lines in timeTables.Skip(1))
            {
                string[] parts = lines.Split(separator);
                int id = int.Parse(parts[0]);
                int stationId = int.Parse(parts[1]);
                DateTime? departure = DateTime.TryParse(parts[2], out DateTime result) ? result : (DateTime?)null;
                DateTime? arrival = DateTime.TryParse(parts[3], out DateTime result1) ? result1 : (DateTime?)null;
                timeTableList.Add(new TimeTable(id, stationId, departure, arrival));
            }

            return timeTableList;
        }

        public static List<Passenger> DeserializePassenger(string filePath, char separator, char separator1)
        {
            string[] passengers;
            List<Passenger> passengerList = new();
            passengers = File.ReadAllLines(filePath);

            foreach (string lines in passengers)
            {
                string[] parts = lines.Split(separator, separator1);
                int id = int.Parse(parts[0]);
                string name = parts[1];
                passengerList.Add(new Passenger(id, name));
            }
            return passengerList;
        }

        public static char[][] DeserializeTrainTrack(string filePath)
        {
            string[] rows = File.ReadAllLines(filePath);

            //split everything up into a 2d char array that we can modify later to have a visual track on screen
            char[][] grid = new char[rows.Length][];
            for (int y = 0; y < grid.Length; y++)
            {
                grid[y] = rows[y].ToCharArray(); //Write all chars from the current row to a chararray on current grid row
            }
            return grid;
        }

        public static void SavePlan(TrainPlanner plan, string planName)
        {
            string path = @"C:\Windows\Temp\" + planName;
            string timeTablePath = path + @"\timetables.txt";
            string trainPath = path + @"\trains.txt";
            string crossingPath = path + @"\crossing.txt";
            string switchPath = path + @"\switch.txt";

            try
            {
                if (Directory.Exists(path))
                {
                    Console.WriteLine("A plan with than name already exists.");
                    return;
                }
                DirectoryInfo folder = Directory.CreateDirectory(path);
                Console.WriteLine("Plan was created successfully at {0}.", Directory.GetCreationTime(path));
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                return;
            }

            if (!File.Exists(path))
            {
                using (StreamWriter swTable = File.CreateText(timeTablePath))
                {
                    swTable.WriteLine("trainId, stationId, departure, arrival");
                    foreach (var timeTable in plan.Table)
                    {
                        swTable.WriteLine($"{timeTable.TrainId}," +
                            $"{timeTable.StationId}," +
                            $"{(timeTable.DepartureTime != null ? $"{timeTable.DepartureTime.Value.Hour}:{timeTable.DepartureTime.Value.Minute}" : "null")}," +
                            $"{(timeTable.ArrivalTime != null ? $"{timeTable.ArrivalTime.Value.Hour}:{timeTable.ArrivalTime.Value.Minute}" : "null")}");
                    }
                }
                using (StreamWriter swTrain = File.CreateText(trainPath))
                {
                    swTrain.WriteLine("Id,Name,MaxSpeed,Operated");
                    swTrain.WriteLine($"{plan.Train.Id},{plan.Train.Name},{plan.Train.TopSpeed},{plan.Train.Operated}");
                }
                using (StreamWriter swCrossing = File.CreateText(crossingPath))
                {
                    swCrossing.WriteLine("Id,IsOpen,CloseAt,OpenAt");
                    swCrossing.WriteLine($"{plan.LevelCrossing.Id}, {plan.LevelCrossing.IsOpen}," +
                        $"{plan.CrossingCloseAt.Hour}:{plan.CrossingCloseAt.Minute}," +
                        $"{plan.CrossingOpenAt.Hour}:{plan.CrossingOpenAt.Minute}");
                }
                using (StreamWriter swSwitch = File.CreateText(switchPath))
                {
                    swSwitch.WriteLine("ChangeAt(Time),Position[X,Y],DirectionLeft");
                    foreach(KeyValuePair<DateTime,(Switch, bool)> kvp in plan.ChangeSwitchAt)
                    {
                        swSwitch.WriteLine($"{kvp.Key.Hour}:{kvp.Key.Minute},[{kvp.Value.Item1.Position.X},{kvp.Value.Item1.Position.Y}], {kvp.Value.Item2}");
                    }
                }
            }
        }

        public static TrainPlanner LoadPlan(string path)
        {
            string timeTablePath = path + @"\timetables.txt";
            string trainPath = path + @"\trains.txt";
            string crossingPath = path + @"\crossing.txt";

            List<TimeTable> timeTableList = DeserializeTimeTables(timeTablePath, ',');
            List<Train> train1 = DeserializeTrains(trainPath, ',');

            // Deserialize crossing
            string[] crossings;

            crossings = File.ReadAllLines(crossingPath);
            string open = "";
            string close = "";
            int crossingId = 0;
            bool something;

            foreach (string lines in crossings.Skip(1))
            {
                string[] parts = lines.Split(',');
                crossingId = int.Parse(parts[0]);
                something = bool.Parse(parts[1]);
                open = parts[2];
                close = parts[3];
            }

            LevelCrossing firstCrossing = new LevelCrossing() { Id = crossingId, IsOpen = true };
            TrainPlanner trainPlan = new TrainPlanner(train1[0])
                .CreateTimeTable(timeTableList)
                .CrossingPlan(firstCrossing, open, close)
                .ToPlan();

            return trainPlan;
        }
    }
}
