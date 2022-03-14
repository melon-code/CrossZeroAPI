using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrossZeroAPI;

namespace CrossZeroAPITests {
    [TestClass]
    public class TTCGameFieldTests {
        static TTCGameField CreateGameField() {
            const int size = 3;
            return new TTCGameField(size);
        }

        static void CheckMark(TTCGameField field, Coordinate point) {
            Assert.IsFalse(field.MarkCross(point));
        }

        static void CheckEndOfGame(TTCGameField field, Func<bool> checkLine, Marks winnerMark) {
            Assert.IsTrue(checkLine());
            Assert.IsTrue(field.IsEnd);
            Assert.IsTrue(field.TryGetEndResult(out EndResult result));
            Assert.AreEqual(TransitionHelper.MarksToEndResult(winnerMark), result);
        }

        static void CheckFullFieldEnd(int size) {
            TTCGameField field = new TTCGameField(size);
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    field.MarkCell(new Coordinate(i, j), TransitionHelper.IndexToChessOrderCellMark(i, j));
            Assert.IsTrue(field.IsEnd);
            field.TryGetEndResult(out EndResult result);
            Assert.AreEqual(EndResult.Draw, result);
        }

        [TestMethod]
        public void MarkNotEmptyCell() {
            Coordinate point = new Coordinate(0, 0);
            TTCGameField field = CreateGameField();
            field.MarkCross(point);
            Assert.IsFalse(field.MarkCross(point));
        }

        [TestMethod]
        public void MarkOutOfBoundsPoint() {
            TTCGameField field = CreateGameField();
            CheckMark(field, new Coordinate(2, 3));
            CheckMark(field, new Coordinate(3, 2));
            CheckMark(field, new Coordinate(3, 3));
        }

        [TestMethod]
        public void FalseTryGetWinner() {
            TTCGameField field = CreateGameField();
            Assert.IsFalse(field.TryGetEndResult(out EndResult result));
            field.MarkCross(new Coordinate(0, 0));
            Assert.IsFalse(field.TryGetEndResult(out result));
        }

        [TestMethod]
        public void EmptyFieldIsEnd() {
            TTCGameField field = CreateGameField();
            Assert.IsFalse(field.IsEnd);
        }

        [TestMethod]
        public void CheckFieldRow() {
            TTCGameField field = CreateGameField();
            field.MarkCross(new Coordinate(0, 0));
            field.MarkZero(new Coordinate(1, 0));
            field.MarkCross(new Coordinate(0, 1));
            field.MarkZero(new Coordinate(1, 1));
            field.MarkCross(new Coordinate(0, 2));
            CheckEndOfGame(field, () => field.CheckRows(), Marks.Cross);
        }

        [TestMethod]
        public void CheckFieldColumn() {
            TTCGameField field = CreateGameField();
            field.MarkZero(new Coordinate(0, 0));
            field.MarkCross(new Coordinate(0, 1));
            field.MarkZero(new Coordinate(1, 0));
            field.MarkCross(new Coordinate(1, 1));
            field.MarkZero(new Coordinate(2, 0));
            CheckEndOfGame(field, () => field.CheckColumns(), Marks.Zero);
        }

        [TestMethod]
        public void CheckFieldMainDiagonal() {
            TTCGameField field = CreateGameField();
            field.MarkCross(new Coordinate(2, 2));
            field.MarkZero(new Coordinate(0, 2));
            field.MarkCross(new Coordinate(1, 1));
            field.MarkZero(new Coordinate(2, 0));
            field.MarkCross(new Coordinate(0, 0));
            CheckEndOfGame(field, () => field.CheckDiagonal(), Marks.Cross);
        }

        [TestMethod]
        public void CheckFieldSideDiagonal() {
            TTCGameField field = CreateGameField();
            field.MarkZero(new Coordinate(0, 2));
            field.MarkCross(new Coordinate(0, 0));
            field.MarkZero(new Coordinate(1, 1));
            field.MarkCross(new Coordinate(2, 2));
            field.MarkZero(new Coordinate(2, 0));
            CheckEndOfGame(field, () => field.CheckDiagonal(), Marks.Zero);
        }

        [TestMethod]
        public void LastPointOnBothDiagonals() {
            TTCGameField field = CreateGameField();
            field.MarkCross(new Coordinate(2, 2));
            field.MarkZero(new Coordinate(0, 0));
            field.MarkCross(new Coordinate(0, 2));
            field.MarkZero(new Coordinate(1, 2));
            field.MarkCross(new Coordinate(2, 0));
            field.MarkZero(new Coordinate(2, 1));
            field.MarkCross(new Coordinate(1, 1));
            CheckEndOfGame(field, () => field.CheckDiagonal(), Marks.Cross);
        }

        [TestMethod]
        public void FullFieldEnd() {
            const int size1 = 3;
            const int size2 = 4;
            CheckFullFieldEnd(size1);
            CheckFullFieldEnd(size2);
        }
    }
}
