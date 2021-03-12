using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainEngine;
using TrainEngine.Objects;

namespace TrainEngine
{
    public interface IFileIO 
    {
        public IFileIO DeserializePassenger(string filePath, char separator, char separator1);

        public IFileIO DeserializeTimeTables(string filePath, char separator);

        public IFileIO DeserializeStations(string filePath, char separator);

        public IFileIO DeserializeTrains(string filePath, char separator);

        //public IFileIO Save(TrainPlanner trainPlanner);
        public IFileIO Save(TrainPlanner trainPlanner);
    }
}