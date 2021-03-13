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
            this._myPos = new Position(3, 0);
            this.Id = id;
            this.Name = name;
            this.TopSpeed = topSpeed;
            this.Operated = operated;
            this.DirectionX = DirectionX.East;
        }

        //Start this trains movement on a new thread
        public void Start()
        {
            TrainThread = new Thread(this.BeginMove);
            TrainThread.Start();
        }

        //these will be called from the mainthread (aka Mr. Carlos) to control the trains movement state
        //Halts the train by disabling the internal timer
        public void HaltTrain() => this.InternalClock.Enabled = false;
        //resumes the train by enabling the internal timer
        public void ResumeTrain() => this.InternalClock.Enabled = true;

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
            //if(IsAtEndStation)
            //{
            //    //end station reached, internalclock no longer needed and reclaim memory from this thread
            //    InternalClock.Stop();
            //    InternalClock = null;
            //    GC.Collect();
            //    return;
            //}

            int directionX = this.DirectionX == DirectionX.East ? 1 : -1;
            int next = TrainTrack.TrackGrid[GetPosition.Y][GetPosition.X + directionX]; //Check 1 step infront of us
            int currentPosValue = TrainTrack.READONLYGRID[GetPosition.Y][GetPosition.X];

            //check if train is standing on a switch
            if(currentPosValue == (int)TrackIdentity.SwitchLeft || currentPosValue == (int)TrackIdentity.SwitchRight)
            {
                int? directionY;
                //Find the switch we're currently positioned at and move up/down depending on its state
                Switch currentSwitch = Switch.Switches.Find(x => x.Position.X == _myPos.X && x.Position.Y == _myPos.Y);
                directionY = -1; //depending on state we want to move up or down on Y axis (+1 || -1)
                //on switches we move diagonally 1 step (+1X, +1Y)
                MoveYAxis(directionY ?? -1);
                MoveXAxis(directionX);
                return;
            }
            //check if we're standing on starting station, a normal track or a crossing and move 1 step on x axis
            if(currentPosValue == (int)TrackIdentity.StartStation || currentPosValue == (int)TrackIdentity.Track || currentPosValue == (int)TrackIdentity.Crossing)
            {
                MoveXAxis(directionX);
                return;
            }
            //check if we're standing on a station
            if(currentPosValue == (int)TrackIdentity.Station1 || currentPosValue == (int)TrackIdentity.Station2 || 
                currentPosValue == (int)TrackIdentity.Station3 || currentPosValue == (int)TrackIdentity.Station4)
            {
                MoveXAxis(directionX);
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
    }

    public enum DirectionX
    {
        East,
        West
    }
}
