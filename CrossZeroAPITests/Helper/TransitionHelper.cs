using CrossZeroAPI;


namespace CrossZeroAPITests {
    public static class TransitionHelper {
        public static EndResult MarksToEndResult(Marks value) {
            return value == Marks.Cross ? EndResult.CrossWin : EndResult.ZeroWin;
        }

        public static Marks IndexToChessOrderCellMark(int row, int column) {
            return row % 2 == 0 ? ReturnFirstCrossLine(column) : ReturnFirstZeroLine(column);
        }

        static Marks ReturnFirstCrossLine(int column) {
            return column % 2 == 0 ? Marks.Cross : Marks.Zero;
        }

        static Marks ReturnFirstZeroLine(int column) {
            return column % 2 == 0 ? Marks.Zero : Marks.Cross; 
        }
    }
}
