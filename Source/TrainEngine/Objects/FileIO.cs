using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace TrainEngine.Objects
{
    class FileIO
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

        //public static List<TimeTable> DeserializeTimeTables(string filePath, char separator)
        //{
        //    string[] timeTables;
        //    List<TimeTable> timeTableList = new List<TimeTable>();
        //    timeTables = File.ReadAllLines(filePath);

        //    foreach (string lines in timeTables.Skip(1))
        //    {
        //        string[] parts = lines.Split(separator);
        //        int id = int.Parse(parts[0]);
        //        int stationId = int.Parse(parts[1]);
        //        DateTime? departure = DateTime.TryParse(parts[2], out DateTime result) ? result : (DateTime?)null;
        //        DateTime? arrival = DateTime.TryParse(parts[3], out DateTime result1) ? result1 : (DateTime?)null;
        //        timeTableList.Add(new TimeTable(id, stationId, departure, arrival));
        //    }

        //    return timeTableList;
        //}

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
    }
}
