using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.Objects
{
    public class TrainTrack
    {
        public int Id;
        public bool IsOccuppied;
    }

    public class LevelCrossing : TrainTrack
    {
        public bool IsOpen = true;
    }

    public class Switch : TrainTrack
    {
        public bool DirectionLeft = true;
    }
}
