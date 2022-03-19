using CrossZeroAPI;

namespace CrossZeroAPITests {
    public class TestGameProcessor : TTCGameProcessor {
        public EndResult Result { get; private set; }

        public TestGameProcessor(int fieldSize, IPlayer player1, IPlayer player2, Marks player1Mark) : base(fieldSize, player1, player2, player1Mark) {
        }

        public override void RenderGameField(ReadOnlyTable gameField) {

        }

        public override void RenderLastFieldAndResult(ReadOnlyTable gameField, EndResult result) {
            Result = result;
        }

        public bool TryGetField(out ReadOnlyTable result) {
            return TryGetGameField(out result);
        }
    }
}