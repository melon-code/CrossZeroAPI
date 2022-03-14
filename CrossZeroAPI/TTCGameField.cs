using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossZeroAPI {
    public class TTCGameField {
        readonly Cell[,] field;
        readonly int turnsToEnd;
        Coordinate lastPoint = null;
        EndResult? result = null;
        int turnsCount = 0;

        public int Size { get; }
        public ReadOnlyTable Grid => new ReadOnlyTable(field);
        public bool IsEnd {
            get {
                if (lastPoint != null && (result != null || CheckFullField() || CheckRows() || CheckColumns() || CheckDiagonal())) 
                    return true;
                return false;
            }
        }

        public TTCGameField(int fieldSize) {
            Size = fieldSize;
            turnsToEnd = Size * Size;
            field = new Cell[Size, Size];
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    field[i, j] = Cell.Empty;
        }

        bool CheckCoordinate(Coordinate point) {
            if (point != null && point.Row < Size && point.Column < Size)
                return true;
            return false;
        }

        bool MarkCell(Coordinate point, Cell value) {
            if (CheckCoordinate(point) && IsCellEmpty(point)) {
                field[point.Row, point.Column] = value;
                lastPoint = point;
                turnsCount++;
                return true;
            }
            return false;
        }

        public bool MarkZero(Coordinate point) {
            return MarkCell(point, Cell.Zero);
        }

        public bool MarkCross(Coordinate point) {
            return MarkCell(point, Cell.Cross);
        }

        public bool MarkCell(Coordinate point, Marks mark) {
            return MarkCell(point, mark == Marks.Cross ? Cell.Cross : Cell.Zero);
        }

        public bool IsCellEmpty(Coordinate point) {
            return field[point.Row, point.Column] == Cell.Empty ? true : false;
        }

        bool CheckLine(Func<Cell> GetInitialPoint, Func<int, Cell> GetNextPoint) {
            Cell currentRowMark = GetInitialPoint();
            if (currentRowMark == Cell.Empty)
                return false;
            for (int i = 1; i < Size; i++)
                if (GetNextPoint(i) != currentRowMark)
                    return false;
            switch (field[lastPoint.Row, lastPoint.Column]) {
                case Cell.Cross:
                    result = EndResult.CrossWin;
                    break;
                case Cell.Zero:
                    result = EndResult.ZeroWin;
                    break;
                default:
                    result = EndResult.Draw;
                    break;
            }
            return true;
        }

        public bool CheckRows() {
            return CheckLine(() => field[lastPoint.Row, 0], (ind) => field[lastPoint.Row, ind]);
        }

        public bool CheckColumns() {
            return CheckLine(() => field[0, lastPoint.Column], (ind) => field[ind, lastPoint.Column]);
        }

        public bool CheckDiagonal() {
            if (lastPoint.Row == lastPoint.Column && CheckLine(() => field[0, 0], (ind) => field[ind, ind])) //main diagonal
                return true;
            if (lastPoint.Row + lastPoint.Column == Size - 1) { //side diagonal
                int maxIndex = Size - 1;
                return CheckLine(() => field[0, maxIndex], (ind) => field[ind, maxIndex - ind]);
            }
            return false;
        }

        public bool CheckFullField() {
            if (turnsCount == turnsToEnd) {
                result = EndResult.Draw;
                return true;
            }
            return false;
        }

        public bool TryGetEndResult(out EndResult gameResult) {
            if (IsEnd && result != null) {
                gameResult = result.Value;
                return true;
            }
            gameResult = EndResult.CrossWin;
            return false;
        }
    }
}
