using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossZeroAPI {
    public abstract class TTCGameProcessor {
        const string winnerException = "Winner is not found exception";
        const int minGameFieldSize = 2;

        ReadOnlyTable grid = null;

        public IPlayer Player1 { get; set; }
        public IPlayer Player2 { get; set; }
        public int GameFieldSize { get; set; }
        public Marks Player1Mark { get; set; }

        public TTCGameProcessor(int gameFieldSize, IPlayer player1, IPlayer player2, Marks player1Mark) {
            GameFieldSize = gameFieldSize;
            Player1 = player1;
            Player2 = player2;
            Player1Mark = player1Mark;
        }

        public abstract void RenderGameField(ReadOnlyTable gameField);
        public abstract void RenderLastFieldAndResult(ReadOnlyTable gameField, EndResult result);

        public void Play() {
            if (GameFieldSize < minGameFieldSize)
                GameFieldSize = minGameFieldSize;
            TTCGame tictactoe = new TTCGame(GameFieldSize, Player1, Player2, Player1Mark);
            grid = tictactoe.GameField;
            do {
                RenderGameField(grid);
                tictactoe.MakePlayerTurn();
            } while (!tictactoe.IsEnd);
            if (tictactoe.TryGetEndResult(out EndResult result)) 
                RenderLastFieldAndResult(grid, result);
            else
                throw new ArgumentException(winnerException);
        }

        protected bool TryGetGameField(out ReadOnlyTable result) {
            if (grid != null) {
                result = grid;
                return true;
            }
            result = null;
            return false;
        }
    }
}