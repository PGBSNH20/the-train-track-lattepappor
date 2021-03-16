using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TrainEngine.Objects
{
    public class TrainTrack
    {
        public static char[][] TrackGrid;
        //This is the real grid without trains, we'll use this to refresh the train positions on TrackGrid.
        public static char[][] READONLYGRID => FileIO.DeserializeTrainTrack(@"Data\traintrack.txt");
         
        public TrainTrack()
        { 
            TrackGrid = READONLYGRID;
        }

        public static void RefreshTrack()
        {
            //Refresh the grid so we can update the train positions elsewhere
            TrackGrid = READONLYGRID;
        }
    }
    public class Switch
    {
        public enum Direction
        {
            Left, 
            Right, 
            Forward
        }
        public Direction _Direction;
        public Position Position;
        public static List<Switch> Switches;

        public static List<Switch> GetSwitches()
        {
            List<Switch> result = new List<Switch>();

            //loop through the read-only grid and find out what positions < and > are located at, then make a new switch with that pos and add to result
            for(int y = 0; y < TrainTrack.READONLYGRID.Length; y++)
            {
                for(int x = 0; x < TrainTrack.READONLYGRID[y].Length; x++)
                {
                    if(TrainTrack.READONLYGRID[y][x] == '<' || TrainTrack.READONLYGRID[y][x] == '>')
                    {
                        Switch @switch = new Switch()
                        {
                            _Direction = Direction.Left,
                            Position = new Position(y, x)
                        };
                        result.Add(@switch);
                    }
                }
            }
            return result;
        }
    }


}
