using CrossZeroAPI;

namespace CrossZeroAPITests {
    public class OnePointTestPlayer : IPlayer {
        readonly Coordinate point;

        public OnePointTestPlayer(Coordinate point) {
            this.point = point;
        }

        public Coordinate MakeTurn() {
            return point;
        }
    }
}
