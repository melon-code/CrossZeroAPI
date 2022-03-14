using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossZeroAPI {
    public class AI : IAI {
        List<Coordinate> turns = new List<Coordinate>();
        ReadOnlyTable board;
        Cell mark;
        Cell opponentMark;
        bool isFirst;
        Coordinate currentTurn;

        int FieldSize => board.Size;
        int MaxIndex => FieldSize - 1;
        int TurnCount => turns.Count;
        int FirstRivalTurn => isFirst ? 0 : 1;
        bool TimeToWin => TurnCount >= FieldSize - 1;
        bool TimeToCheck => TurnCount >= FieldSize - 1 - FirstRivalTurn;
        public ReadOnlyTable GameField { set => board = value; }
        public Marks Mark {
            set {
                mark = value == Marks.Cross ? Cell.Cross : Cell.Zero;
                opponentMark = mark == Cell.Cross ? Cell.Zero : Cell.Cross;
            }
        }

        public Coordinate MakeTurn() {
            Coordinate turn = NextTurn();
            turns.Add(turn);
            return turn;
        }

        Coordinate NextTurn() {
            if (TurnCount == 0) {
                isFirst = IsFirst(out Coordinate rivalTurn);
                return isFirst ? CornerTurn() : OppositeCornerTurn(rivalTurn);
            }
            if (TimeToWin && TryToWin(out Coordinate point))
                return point;
            if (TimeToCheck && CheckEndOnNextTurn(out point))
                return point;
            if (TryExtend(out point))
                return point;
            return FindNewLine();
        }

        bool IsFirst(out Coordinate firstRivalTurn) {
            for (int i = 0; i < FieldSize; i++)
                for (int j = 0; j < FieldSize; j++)
                    if (board[i, j] != Cell.Empty) {
                        firstRivalTurn = new Coordinate(i, j);
                        return false;
                    }
            firstRivalTurn = null;
            return true;
        }

        Coordinate CornerTurn() {
            return CornerHelper.GetRandomCorner(FieldSize);
        }

        Coordinate OppositeCornerTurn(Coordinate point) {
            int x = point.Row > FieldSize / 2 ? 0 : MaxIndex;
            int y = point.Column > FieldSize / 2 ? 0 : MaxIndex;
            return new Coordinate(x, y);
        }

        bool CheckWinLine(Cell oppositeMark, Func<int, Coordinate> GetPoint) {
            Coordinate winPoint = null;
            for (int j = 0; j < FieldSize; j++) {
                Coordinate point = GetPoint(j);
                Cell item = board[point.Row, point.Column];
                if (item == oppositeMark)
                    return false;
                if (item == Cell.Empty) {
                    if (winPoint != null)
                        return false;
                    winPoint = point;
                }
            }
            currentTurn = winPoint;
            return true;
        }

        bool FindWinningLineBase(Cell oppositeMark, Func<int, int, Coordinate> GetNextPoint) {
            for (int i = 0; i < FieldSize; i++)
                if (CheckWinLine(oppositeMark, (ind) => GetNextPoint(i, ind)))
                    return true;
            return false;
        }

        bool FindWinningLineHorizontal(Cell oppositeMark) {
            return FindWinningLineBase(oppositeMark, (i, j) => new Coordinate(i, j));
        }

        bool FindWinningLineVertical(Cell oppositeMark) {
            return FindWinningLineBase(oppositeMark, (i, j) => new Coordinate(j, i));
        }

        bool FindWinningDiagonal(Cell oppositeMark) {
            if (CheckWinLine(oppositeMark, (ind) => new Coordinate(ind, ind)))
                return true;
            if (CheckWinLine(oppositeMark, (ind) => new Coordinate(ind, MaxIndex - ind)))
                return true;
            return false;
        }

        bool CheckPotentialEnd(Cell lose, out Coordinate endTurn) {
            if (FindWinningLineHorizontal(lose) || FindWinningLineVertical(lose) || FindWinningDiagonal(lose)) {
                endTurn = currentTurn;
                return true;
            }
            endTurn = null;
            return false;
        }

        bool TryToWin(out Coordinate winTurn) {
            return CheckPotentialEnd(opponentMark, out winTurn);
        }

        bool CheckEndOnNextTurn(out Coordinate saveTurn) {
            return CheckPotentialEnd(mark, out saveTurn);
        }

        bool TryGetEmptyLine(Func<int, Coordinate> GetNextPoint, out PotentialLine line) {
            int count = 0;
            Coordinate potentialPoint = null;
            for (int i = 0; i < FieldSize; i++) {
                var point = GetNextPoint(i);
                var item = board[point];
                if (item == opponentMark) {
                    line = null;
                    return false;
                }
                if (item == mark)
                    count++;
                if (item == Cell.Empty && potentialPoint == null)
                    potentialPoint = point;
            }
            line = new PotentialLine(count, potentialPoint);
            return true;
        }

        bool TryGetEmptyMainDiagonal(out PotentialLine line) {
            return TryGetEmptyLine((ind) => new Coordinate(ind, ind), out line);
        }

        bool TryGetEmptySideDiagonal(out PotentialLine line) {
            return TryGetEmptyLine((ind) => new Coordinate(ind, MaxIndex - ind), out line);
        }

        bool TryExtend(Coordinate point, out Coordinate extendTurn) {
            List<PotentialLine> lines = new List<PotentialLine>();
            int row = point.Row;
            int column = point.Column;
            if (TryGetEmptyLine((ind) => new Coordinate(row, ind), out PotentialLine line))
                lines.Add(line);
            if (TryGetEmptyLine((ind) => new Coordinate(ind, column), out line))
                lines.Add(line);
            if (row == column && TryGetEmptyMainDiagonal(out line))
                lines.Add(line);
            if (row + column == MaxIndex && TryGetEmptySideDiagonal(out line))
                lines.Add(line);
            if (lines.Count != 0) {
                extendTurn = PotentialLine.GetBestCoordinate(lines);
                return true;
            }
            extendTurn = null;
            return false;
        }

        bool TryExtend(out Coordinate extendTurn) {
            return TryExtend(turns.Last(), out extendTurn);
        }

        Coordinate FindNewLine() {
            PotentialLine line;
            List<PotentialLine> lines = new List<PotentialLine>();
            for (int i = 0; i < FieldSize; i++) {
                if (TryGetEmptyLine((ind) => new Coordinate(i, ind), out line))
                    lines.Add(line);
                if (TryGetEmptyLine((ind) => new Coordinate(ind, i), out line))
                    lines.Add(line);
            }
            if (TryGetEmptyMainDiagonal(out line))
                lines.Add(line);
            if (TryGetEmptySideDiagonal(out line))
                lines.Add(line);
            if (lines.Count != 0)
                return PotentialLine.GetBestCoordinate(lines);
            List<Coordinate> points = new List<Coordinate>();
            for (int i = 0; i < FieldSize; i++)
                for (int j = 0; j < FieldSize; j++)
                    if (board[i, j] == Cell.Empty)
                        points.Add(new Coordinate(i, j));
            if (points.Count == 1)
                return points[0];
            Random rand = new Random();
            return points[rand.Next(points.Count)];
        }

        class PotentialLine {
            public static Coordinate GetBestCoordinate(IList<PotentialLine> potentialLines) {
                int max = 0;
                Coordinate point = null;
                foreach (var item in potentialLines) {
                    var potential = item.Potential;
                    if (potential > max) {
                        max = potential;
                        point = item.Point;
                    }
                }
                return point ?? potentialLines.First().Point;
            }

            public int Potential { get; }
            public Coordinate Point { get; }

            public PotentialLine(int potential, Coordinate point) {
                Potential = potential;
                Point = point;
            }
        }
    }
}
