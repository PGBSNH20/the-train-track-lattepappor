using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TrainEngine.Objects
{
    public class Mr_Carlos
    {
        public List<Train> trains = FileIO.DeserializeTrains(@"Data\trains.txt", ',');
        public List<Station> stations = FileIO.DeserializeStations(@"Data\stations.txt", '|');
        public List<Passenger> passengers = FileIO.DeserializePassenger(@"Data\passengers.txt", ';', ':');
        public List<TimeTable> timeTables = FileIO.DeserializeTimeTables(@"Data\timetable.txt", ',');

        private Timer ObserverTimer;
        public static DateTime GlobalTime = new DateTime().AddHours(10).AddMinutes(40);

        public void BeginObserving()
        {
            Switch.Switches = Switch.GetSwitches();
            TrainTrack.TrackGrid = TrainTrack.READONLYGRID;
            ObserverTimer = new Timer(1000);
            ObserverTimer.Elapsed += UpdateConsole;
            ObserverTimer.AutoReset = true;
            ObserverTimer.Enabled = true;
            ObserverTimer.Start();

            trains.RemoveAll(x => !x.Operated);
            trains.ForEach(x => x.Start());
        }

        private void UpdateConsole(Object src, ElapsedEventArgs e)
        {
            GlobalTime = GlobalTime.AddMinutes(1);
            Console.Clear();
            TrainTrack.RefreshTrack();

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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[Mr. Carlos][{GlobalTime.ToString("HH:mm")}]");
            Console.ResetColor();
        }
    }
}
