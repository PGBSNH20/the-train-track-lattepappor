using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainEngine.Utils
{
    public enum TrackIdentity : int
    {
        Void = 32, //Space char int value = 32
        StartStation = '*',
        Track = '-',
        Crossing = '=',
        SwitchRight = '>',
        SwitchLeft = '<',
        Train = 'T',
        Station1 = '1',
        Station2 = '2',
        Station3 = '3',
        Station4 = '4'
    }
}
