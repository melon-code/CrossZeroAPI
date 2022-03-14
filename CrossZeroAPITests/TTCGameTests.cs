using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrossZeroAPI;

namespace CrossZeroAPITests {
    [TestClass]
    public class TTCGameTests {
        static IPlayer CreateOneTurnPlayer(int x, int y) {
            return new TestPlayer(new List<Coordinate> { new Coordinate(x, y) });
        }

        static void MakeTurnAndCheck(TTCGame game, Cell target, int row, int column) {
            game.MakePlayerTurn();
            Assert.AreEqual(target, game.GameField[row, column]);
        }

        [TestMethod]
        public void MakeTurn() {
            TTCGame game = new TTCGame(CreateOneTurnPlayer(0, 0), CreateOneTurnPlayer(0, 1));
            MakeTurnAndCheck(game, Cell.Cross, 0, 0);
            MakeTurnAndCheck(game, Cell.Zero, 0, 1);
        }

        [TestMethod]
        public void MakeTurnOnFilledCell() {
            List<Coordinate> points = new List<Coordinate>() { new Coordinate(0, 0), new Coordinate(0, 1) };
            TTCGame game = new TTCGame(CreateOneTurnPlayer(0, 0), new TestPlayer(points));
            game.MakePlayerTurn();
            MakeTurnAndCheck(game, Cell.Zero, 0, 1);
        }

        [TestMethod]
        public void TryGetWinner() {
            const int turnCount = 5;
            List<Coordinate> points1 = new List<Coordinate>() { new Coordinate(0, 0), new Coordinate(1, 0), new Coordinate(2, 0) };
            List<Coordinate> points2 = new List<Coordinate>() { new Coordinate(0, 1), new Coordinate(1, 1), new Coordinate(2, 1) };
            TTCGame game = new TTCGame(new TestPlayer(points1), new TestPlayer(points2));
            for (int i = 0; i < turnCount; i++)
                game.MakePlayerTurn();
            Assert.IsTrue(game.TryGetEndResult(out EndResult result));
            Assert.AreEqual(EndResult.CrossWin, result);
        }

        [TestMethod]
        public void EndlessInputLoopException() {
            Coordinate point = new Coordinate(0, 0);
            TTCGame game = new TTCGame(new OnePointTestPlayer(point), new OnePointTestPlayer(point));
            game.MakePlayerTurn();
            Assert.ThrowsException<ArgumentException>(() => game.MakePlayerTurn());
        }
    }
}