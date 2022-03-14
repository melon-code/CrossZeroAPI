using System;

namespace CrossZeroAPI {
    public class TTCGame {
        const string playerNullException = "Player{0} is null!";
        const string endlessInputLoopException = "Endless input loop!";
        const int firstPlayerNumber = 1;
        const int secondPlayerNumber = 2;
        const int classicFieldSize = 3;
        const int standartInputErrorsLimit = 50;

        readonly int errorsLimit;
        TTCGameField field;
        TTCPlayersEnumeration players;

        Marks CurrentMark => players.CurrentMark;
        IPlayer CurrentPlayer => players.Current;
        public bool IsEnd => field.IsEnd;
        public ReadOnlyTable GameField => field.Grid;

        public TTCGame(int gameFieldSize, IPlayer player1, IPlayer player2, Marks player1Mark, int inputErrorsLimit) {
            errorsLimit = inputErrorsLimit;
            field = new TTCGameField(gameFieldSize);
            ValidatePlayer(player1, firstPlayerNumber, player1Mark);
            ValidatePlayer(player2, secondPlayerNumber, TTCPlayersEnumeration.GetOppositeMark(player1Mark));
            players = new TTCPlayersEnumeration(player1, player2, player1Mark);
        }

        public TTCGame(int gameFieldSize, IPlayer player1, IPlayer player2, Marks player1Mark) : this(gameFieldSize, player1, player2, player1Mark, standartInputErrorsLimit) {
        }

        public TTCGame(int gameFieldSize, IPlayer player1, IPlayer player2) : this(gameFieldSize, player1, player2, Marks.Cross) {
        }

        public TTCGame(IPlayer player1, IPlayer player2) : this(classicFieldSize, player1, player2, Marks.Cross) {
        }

        void ValidatePlayer(IPlayer player, int playerNumber, Marks playerMark) {
            if (player == null)
                throw new ArgumentNullException(string.Format(playerNullException, playerNumber));
            if (player is IAI ai) {
                ai.GameField = GameField;
                ai.Mark = playerMark;
            }
        }

        public void MakePlayerTurn() {
            int errorsCount = 0;
            while (!field.MarkCell(CurrentPlayer.MakeTurn(), CurrentMark)) {
                errorsCount++;
                if (errorsCount == errorsLimit)
                    throw new ArgumentException(endlessInputLoopException);
            }
            players.MoveNext();
        }

        public bool TryGetEndResult(out EndResult result) {
            return field.TryGetEndResult(out result);
        }
    }
}