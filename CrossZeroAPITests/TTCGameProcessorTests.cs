using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrossZeroAPI;

namespace CrossZeroAPITests {
    [TestClass]
    public class TTCGameProcessorTests {
        static List<Coordinate> singlePoint = new List<Coordinate>() { new Coordinate(0, 0) };

        static void CheckPlayerValidationBase(IPlayer player1, IPlayer player2, bool setNullPlayer1, bool setNullPlayer2) {
            TTCGameProcessor processor = new TestGameProcessor(3, player1, player2, Marks.Cross);
            if (setNullPlayer1)
                processor.Player1 = null;
            if (setNullPlayer2)
                processor.Player2 = null;
            Assert.ThrowsException<ArgumentNullException>(() => processor.Play());
        }

        static void CheckPlayerValidation(IPlayer player1, IPlayer player2) {
            CheckPlayerValidationBase(player1, player2, false, false);
        }

        static void CheckSetPlayerValidation(bool setNullPlayer1, bool setNullPlayer2) {
            TestPlayer player = new TestPlayer(singlePoint);
            CheckPlayerValidationBase(player, player, setNullPlayer1, setNullPlayer2);
        }

        static void CheckSetGameFieldToAI(IPlayer player1, IPlayer player2) {
            IList<IPlayer> players = new List<IPlayer>() { player1, player2 };
            TTCGameProcessor processor = new TestGameProcessor(3, players[0], players[1], Marks.Cross);
            processor.Play();
            foreach (var item in players)
                if (item is TestAI ai)
                    Assert.IsNotNull(ai.GameField);
        }

        static void CheckSetMarkToAI(Marks player1Mark) {
            TestAI ai1 = new TestAI(0);
            TestAI ai2 = new TestAI(1);
            TTCGameProcessor processor = new TestGameProcessor(3, ai1, ai2, player1Mark);
            processor.Play();
            Assert.AreEqual(player1Mark, ai1.Mark);
            Assert.AreEqual(TTCPlayersEnumeration.GetOppositeMark(player1Mark), ai2.Mark);
        }

        static void CheckWinnerResult(Marks winnerMark) {
            TestGameProcessor processor = new TestGameProcessor(3, new TestAI(0), new TestAI(1), winnerMark);
            processor.Play();
            Assert.AreEqual(TransitionHelper.MarksToEndResult(winnerMark), processor.Result);
        }

        [TestMethod]
        public void PlayerValidation() {
            TestPlayer player = new TestPlayer(singlePoint);
            CheckPlayerValidation(null, player);
            CheckPlayerValidation(player, null);
            CheckPlayerValidation(null, null);
        }

        [TestMethod]
        public void SetPlayerValidation() {
            CheckSetPlayerValidation(true, false);
            CheckSetPlayerValidation(false, true);
            CheckSetPlayerValidation(true, true);
        }

        [TestMethod]
        public void SetGameFieldToAI() {
            List<Coordinate> points = new List<Coordinate>() { new Coordinate(0, 0), new Coordinate(0, 1), new Coordinate(0, 2) };
            CheckSetGameFieldToAI(new TestPlayer(points), new TestAI(1));
            CheckSetGameFieldToAI(new TestAI(1), new TestPlayer(points));
            CheckSetGameFieldToAI(new TestAI(0), new TestAI(1));
        }

        [TestMethod]
        public void SetMarkToAI() {
            CheckSetMarkToAI(Marks.Cross);
            CheckSetMarkToAI(Marks.Zero);
        }

        [TestMethod]
        public void WinnerResult() {
            CheckWinnerResult(Marks.Cross);
            CheckWinnerResult(Marks.Zero);
        }

        [TestMethod]
        public void TryGetGameField() {
            TestGameProcessor processor = new TestGameProcessor(3, new TestAI(0), new TestAI(1), Marks.Cross);
            Assert.IsFalse(processor.TryGetField(out ReadOnlyTable table));
            processor.Play();
            Assert.IsTrue(processor.TryGetField(out table));
            Assert.IsNotNull(table);
        }
    }
}