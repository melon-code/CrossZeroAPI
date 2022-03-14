using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrossZeroAPI;

namespace CrossZeroAPITests {
    [TestClass]
    public class TTCPlayersEnumerationTests {
        class TestPlayer1 : OnePointTestPlayer {
            public TestPlayer1() : base(new Coordinate(0,0)) {
            }
        }

        class TestPlayer2 : OnePointTestPlayer {
            public TestPlayer2() : base(new Coordinate(1, 1)) {
            }
        }

        static void CheckEnum(TTCPlayersEnumeration players, Type targetType, Marks currentMark) {
            Assert.IsInstanceOfType(players.Current, targetType);
            Assert.AreEqual(players.CurrentMark, currentMark);
        }

        static void CheckAndMove(TTCPlayersEnumeration players, Type targetType, Marks currentMark) {
            CheckEnum(players, targetType, currentMark);
            players.MoveNext();
        }

        [TestMethod]
        public void MoveNext() {
            TestPlayer1 player1 = new TestPlayer1();
            TestPlayer2 player2 = new TestPlayer2();
            TTCPlayersEnumeration players = new TTCPlayersEnumeration(player1, player2, Marks.Cross);
            CheckAndMove(players, player1.GetType(), Marks.Cross);
            CheckAndMove(players, player2.GetType(), Marks.Zero);
            CheckEnum(players, player1.GetType(), Marks.Cross);
        }
    }
}
