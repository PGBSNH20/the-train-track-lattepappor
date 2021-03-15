using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using TrainEngine.Utils;
using System.Linq;
using Timer = System.Timers.Timer;

namespace TrainEngine.Objects
{
    public class Train
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TopSpeed { get; set; }
        public bool Operated { get; set; }
        public bool IsAtEndStation { get; set; }
        private Position _myPos;
        public Position GetPosition => _myPos;
        public DirectionX DirectionX;
        public bool IsStopped { get; set; }

        //Speed = Calculate how many seconds should take between every 1 step on the track
        //our imaginary distance is 250km between each station, divided by 15 steps between every station = each step is ~16km
        //divide the km/step (16) with our topspeed (ie 120km/h) * 10 to get our "simulated" time, 120km/h topspeed would give us 1333ms per step and ~20s from one station to another
        private float Speed => 250f / 15f / TopSpeed * 10f; 
        private Timer InternalClock;

        private Thread TrainThread;
        public TrainPlanner Planner { get; set; }
        public List<Passenger> Passengers = new ();

        public Train(int id, string name, int topSpeed, bool operated)
        {
            this.Id = id;
            this.Name = name;
            this.TopSpeed = topSpeed;
            this.Operated = operated;
            this.DirectionX = DirectionX.East;
        }

        public void SetPlan(TrainPlanner plan)
        {
            this.Planner = plan;
        }

        public void SetStartPosition(Position pos)
        {
            this._myPos = pos;
        }

        //Start this trains movement on a new thread
        public void Start()
        {
            TrainThread = new Thread(this.BeginMove);
            TrainThread.Start();
        }

        private void BeginMove()
        {
            InternalClock = new Timer((int)(Speed * 300));
            InternalClock.Elapsed += MoveTrainEvent;
            InternalClock.AutoReset = true;
            InternalClock.Enabled = true;
            InternalClock.Start();
        }

        private void MoveTrainEvent(Object src, ElapsedEventArgs e)
        {
            int directionX = this.DirectionX == DirectionX.East ? 1 : -1;
            int currentPosValue = TrainTrack.READONLYGRID[GetPosition.Y][GetPosition.X];

            if ((_myPos.Y < TrainTrack.READONLYGRID.Length - 1 && _myPos.Y > 0) || _myPos.X == TrainTrack.READONLYGRID[_myPos.Y].Length - 1)
            {
                if (currentPosValue == (int)TrackIdentity.DiagonalRight)
                {
                    MoveXAxis(directionX);
                    MoveYAxis(-1);
                    return;
                }
                if (currentPosValue == (int)TrackIdentity.DiagonalLeft)
                {
                    MoveXAxis(directionX);
                    MoveYAxis(1);
                    return;
                }
            }
            else
            {
                if (currentPosValue == (int)TrackIdentity.DiagonalRight || currentPosValue == (int)TrackIdentity.DiagonalLeft)
                {
                    MoveXAxis(directionX);
                    return;
                }
            }
            //check if train is standing on a switch
            if(currentPosValue == (int)TrackIdentity.SwitchLeft || currentPosValue == (int)TrackIdentity.SwitchRight)
            {
                int directionY = 0;
                //Find the switch we're currently positioned at and move up/down depending on its state
                Switch currentSwitch = Switch.Switches.First(x => x.Position.X == _myPos.X && x.Position.Y == _myPos.Y);
                if(currentSwitch._Direction == Switch.Direction.Forward)
                {
                    directionY = 0;
                }
                else if(currentSwitch._Direction == Switch.Direction.Left)
                {
                    directionY = -1;
                }
                else if(currentSwitch._Direction == Switch.Direction.Right)
                {
                    directionY = 1;
                }
                //on switches we move diagonally 1 step (+1X, +1Y)
                MoveYAxis(directionY);
                MoveXAxis(directionX);
                return;
            }
            //check if we're standing on a normal track or a crossing and move 1 step on x axis
            if(currentPosValue == (int)TrackIdentity.Track || currentPosValue == (int)TrackIdentity.Crossing)
            {
                MoveXAxis(directionX);
                return;
            }
            //check if we're standing on a station
            if (currentPosValue == (int)TrackIdentity.Station1 || currentPosValue == (int)TrackIdentity.Station2 ||
                currentPosValue == (int)TrackIdentity.Station3 || currentPosValue == (int)TrackIdentity.Station4)
            {
                //Find out my start station
                int myStartStation = Planner.Table.First(x => x.ArrivalTime == null && x.TrainId == this.Id).StationId;
                //Convert my pos int value to its char equivalent
                char getChar = (char)currentPosValue;
                //Parse my char station (ie '3') to an integer
                int currentStation = int.Parse(getChar.ToString());
                Station station = Mr_Carlos.stations.First(x => x.Id == currentStation);
                //Get the departure time from my current station
                DateTime? departure = Planner.Table.First(x => x.StationId == currentStation && x.TrainId == this.Id).DepartureTime;
                if (Mr_Carlos.GlobalTime.Hour >= departure?.Hour && Mr_Carlos.GlobalTime.Minute >= departure?.Minute)
                {
                    ControllerLog.Content = $"{this.Name} departed from {station.Name}!";
                    Mr_Carlos.TimeLine.Add($"[TIMELINE][{Mr_Carlos.GlobalTime.ToString("HH:mm")}]: {this.Name} departed from {station.Name}!");
                    MoveXAxis(directionX);
                }
                else
                {
                    ControllerLog.Content = $"{this.Name} is stopped at {station.Name} until {departure?.ToString("HH:mm")}";
                    string str = $"{this.Name} stopped at {station.Name}!";
                    if (!Mr_Carlos.TimeLine.Any(x => x.Contains(str)))
                    {
                        Mr_Carlos.TimeLine.Add($"[TIMELINE][{Mr_Carlos.GlobalTime.ToString("HH:mm")}]: {str}");
                    }
                }
                return;
            }
        }

        private void MoveXAxis(int step)
        {
            this._myPos.X += step;
        }

        private void MoveYAxis(int step)
        {
            this._myPos.Y += step;
        }
    }

    public struct Position
    {
        public int Y;
        public int X;

        public Position(int y, int x)
        {
            this.Y = y;
            this.X = x;
        }
        public override string ToString()
        {
            return $"{Y}, {X}";
        }

        public bool AreEqual(Position obj)
        {
            return this.X == obj.X && this.Y == obj.Y;
        }
    }

    public enum DirectionX
    {
        East,
        West
    }
}
