using System.Collections.Generic;

namespace CrossZeroAPI {
    public class TTCPlayersEnumeration {
        public static Marks GetOppositeMark(Marks mark) {
            return mark == Marks.Cross ? Marks.Zero : Marks.Cross;
        }

        const int playersCount = 2;

        List<IPlayer> players = new List<IPlayer>(playersCount);
        int counter = 0;
        readonly Marks player1Mark;

        public Marks CurrentMark => counter == 0 ? player1Mark : GetOppositeMark(player1Mark);
        public IPlayer Current => players[counter];

        public TTCPlayersEnumeration(IPlayer player1, IPlayer player2, Marks player1Mark) {
            players.Add(player1);
            players.Add(player2);
            this.player1Mark = player1Mark;
        }

        public void MoveNext() {
            counter = (counter + 1) % playersCount;
        }
    }
}