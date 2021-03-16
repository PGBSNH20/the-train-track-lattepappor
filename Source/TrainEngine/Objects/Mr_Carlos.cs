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
        public static DateTime GlobalTime = DateTime.Parse("10:15");

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
                    .SwitchPlan(Switch.Switches[0], "10:23", Switch.Direction.Left)
                    .CrossingPlan("10:20", "10:24")
                    .SwitchPlan(Switch.Switches[1], "10:41", Switch.Direction.Forward)
                    .ToPlan();

            TrainPlanner planner2 = new TrainPlanner(trains[1])
                    .CreateTimeTable(timeTables)
                    .SwitchPlan(Switch.Switches[1], "10:38", Switch.Direction.Right)
                    .CrossingPlan("11:28", "11:34")
                    .SwitchPlan(Switch.Switches[0], "11:20", Switch.Direction.Forward)
                    .ToPlan();

            plans.Add(planner);
            plans.Add(planner2);
            trains[0].SetPlan(planner);
            trains[1].SetPlan(planner2);

            trains.ForEach(x => x.Start());

            AssignStartPosition(trains[0], planner);
            AssignStartPosition(trains[1], planner2);
        }

        private void UpdateConsole(Object src, ElapsedEventArgs e)
        {
            CheckCrossingPlan();
            CheckSwitchPlan();
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
            if (GlobalTime >= DateTime.Parse("11:42"))
            {
                ObserverTimer.Close();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nAll trains have arrived at their final destination. Terminating program :)");
            }
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

        private void CheckCrossingPlan()
        {
            if(plans.Any(x => x.CrossingCloseAt == GlobalTime))
            {
                ControllerLog.Content = $"Crossing is closing!";
                TimeLine.Add($"[TIMELINE][{GlobalTime.ToString("HH:mm")}]: Crossing closed!");
            }
            if(plans.Any(x => x.CrossingOpenAt== GlobalTime))
            {
                ControllerLog.Content = $"Crossing is opening!";
                TimeLine.Add($"[TIMELINE][{GlobalTime.ToString("HH:mm")}]: Crossing opened!");
            }
        }

        private void CheckSwitchPlan()
        {
            if (plans.Any(x => x.ChangeSwitchAt.ContainsKey(GlobalTime)))
            {
                TrainPlanner planner = plans.Where(x => x.ChangeSwitchAt.ContainsKey(GlobalTime)).First();
                planner.ChangeSwitchAt[GlobalTime].Item1._Direction = planner.ChangeSwitchAt[GlobalTime].Item2;
                ControllerLog.Content = $"Switch at position {planner.ChangeSwitchAt[GlobalTime].Item1.Position.ToString()} changed direction.";
                TimeLine.Add($"[TIMELINE][{GlobalTime.ToString("HH:mm")}]: Switch at position {planner.ChangeSwitchAt[GlobalTime].Item1.Position.ToString()} changed direction.");
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
