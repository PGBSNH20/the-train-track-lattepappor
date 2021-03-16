using System;
using TrainEngine.Objects;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace TrainEngine.Tests
{
    public class TrackOrmTests
    {
        [Fact]
        public void When_OnlyAStationIsProvided_Expect_TheResultOnlyToContainAStationWithId1()
        {
            // Arrange
            string track = "[1]";
            TrackOrm trackOrm = new TrackOrm();

            // Act
            var result = trackOrm.ParseTrackDescription(track);

            // Assert
            //Assert.IsType<Station>(result.TackPart[0]);
            //Station s = (Station)result.TackPart[0];
            //Assert.Equal(1, s.Id);
        }

        [Fact]
        public void When_ProvidingTwoStationsWithOneTrackBetween_Expect_TheTrackToConcistOf3Parts()
        {
            // Arrange
            string track = "[1]-[2]";
            TrackOrm trackOrm = new TrackOrm();
            
            // Act
            var result = trackOrm.ParseTrackDescription(track);

            // Assert
            Assert.Equal(3, result.NumberOfTrackParts);
        }

        [Fact]
        public void ReadAllTrains()
        {
            //Arrange
            List<Train> trains = new List<Train>();

            //Act
            trains = FileIO.DeserializeTrains(@"Data\trains.txt", ',');

            //Assert
            Assert.Equal(4, trains.Count);
        }

        [Fact]
        public void TrainsInOperation()
        {
            List<Train> trains = new();
            trains = FileIO.DeserializeTrains(@"Data\trains.txt", ',');

            Assert.Equal(2, trains.Where(x => x.Operated == true).ToList().Count);
        }

        //[Fact]
        //public void FailedToSavePlanner()
        //{
        //    var newPlan = new TrainPlanner();
        //}
    }
}
