using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TrainEngine.Utils;

namespace TrainEngine.Objects
{
    public class Mr_Carlos
    {
        public static List<Train> trains = FileIO.DeserializeTrains(@"Data\trains.txt", ',');
        public static List<Station> stations = FileIO.DeserializeStations(@"Data\stations.txt", '|');
        public static List<Passenger> passengers = FileIO.DeserializePassenger(@"Data\passengers.txt", ';', ':');
        public static List<TimeTable> timeTables = FileIO.DeserializeTimeTables(@"Data\timetable.txt", ',');
        public static List<string> TimeLine = new List<string>();

        public List<TrainPlanner> plans = new List<TrainPlanner>();

        private Timer ObserverTimer;
        public static DateTime GlobalTime = new DateTime().AddHours(10).AddMinutes(15);

        public void BeginObserving()
        {
            Switch.Switches = Switch.GetSwitches();
            TrainTrack.TrackGrid = TrainTrack.READONLYGRID;
            ObserverTimer = new Timer(500);
            ObserverTimer.Elapsed += UpdateConsole;
            ObserverTimer.AutoReset = true;
            ObserverTimer.Enabled = true;
            ObserverTimer.Start();

            trains.RemoveAll(x => !x.Operated);

            TrainPlanner planner = new TrainPlanner(trains[0])
                    .CreateTimeTable(timeTables)
                    .SwitchPlan(Switch.Switches[0], "10:45", Switch.Direction.Left)
                    .CrossingPlan("10:41", "10:44")
                    .SwitchPlan(Switch.Switches[1], "11:02", Switch.Direction.Forward)
                    .ToPlan();

            TrainPlanner planner2 = new TrainPlanner(trains[1])
                    .CreateTimeTable(timeTables)
                    .SwitchPlan(Switch.Switches[0], "10:45", Switch.Direction.Left)
                    .CrossingPlan("11:11", "11:14")
                    .SwitchPlan(Switch.Switches[1], "11:02", Switch.Direction.Right)
                    .ToPlan();

            plans.Add(planner);
            plans.Add(planner2);
            trains[0].SetPlan(planner);
            trains[1].SetPlan(planner2);

            Switch.Switches[1]._Direction = Switch.Direction.Forward;

            trains[0].Start();
            trains[1].Start();

            AssignStartPosition(trains[0], planner);
            AssignStartPosition(trains[1], planner2);
        }

        private void UpdateConsole(Object src, ElapsedEventArgs e)
        {
            Console.Clear();
            TrainTrack.RefreshTrack();

            //Place each train on the map
            foreach(Train t in trains)
            {
                TrainTrack.TrackGrid[t.GetPosition.Y][t.GetPosition.X] = 'T';
            }

            string[] rows = new string[TrainTrack.TrackGrid.Length]; //Make a string array based on the trackgrids Y-axis length
            for (int i = 0; i < rows.Length; i++)
            {
                rows[i] = new string(TrainTrack.TrackGrid[i]); //Make a string at current index with the char array at row index
            }
            string track = string.Join('\n', rows); //Make a full string of the whole track

            Console.WriteLine(track);
            ControllerLog.Log(@"C:\Users\Sebastian\source\repos\the-train-track-lattepappor\Source\TrainEngine\Data\controllerlog.txt", GlobalTime);
            GlobalTime = GlobalTime.AddMinutes(1);
        }

        private void AssignStartPosition(Train train, TrainPlanner planner)
        {
            int StartStation = planner.Table.Where(x => x.ArrivalTime == null).First().StationId;
            bool finished = false;

            for (int y = 0; y < TrainTrack.READONLYGRID.Length; y++)
            {
                for (int x = 0; x < TrainTrack.READONLYGRID[y].Length; x++)
                {
                    if (TrainTrack.READONLYGRID[y][x] == char.Parse(StartStation.ToString()))
                    {
                        train.SetStartPosition(new Position(y, x));
                        finished = true;
                        break;
                    }
                }
                if (finished) break;
            }
        }

        public static Dictionary<Station, Position> GetStationPositions()
        {
            Dictionary<Station, Position> result = new Dictionary<Station, Position>();

            for(int y = 0; y < TrainTrack.READONLYGRID.Length; y++)
            {
                for(int x = 0; x < TrainTrack.READONLYGRID[y].Length; x++)
                {
                    if(char.IsNumber(TrainTrack.READONLYGRID[y][x]))
                    {
                        char value = TrainTrack.READONLYGRID[y][x];
                        Station station = stations.First(x => x.Id == int.Parse(value.ToString()));
                        result.Add(station, new Position(y, x));
                    }
                }
            }
            return result;
        }
    }
}
