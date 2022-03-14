using System.Collections.Generic;
using CrossZeroAPI;

namespace CrossZeroAPITests {
    public class TestPlayer : IPlayer {
        readonly IList<Coordinate> coordinates;
        int counter = 0;

        public TestPlayer(IList<Coordinate> coordinateList) {
            coordinates = coordinateList;
        }

        public Coordinate MakeTurn() {
            return coordinates[counter++];
        }
    }
}
