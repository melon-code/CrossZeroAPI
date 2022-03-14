using CrossZeroAPI;

namespace CrossZeroAPITests {
    public class TestAI : IAI {
        readonly int row;
        int counter = 0;

        public ReadOnlyTable GameField { get; set; }
        public Marks Mark { get; set; }

        public TestAI(int rowNumber) {
            row = rowNumber;
        }

        public Coordinate MakeTurn() {
            return new Coordinate(row, counter++);
        }
    }
}