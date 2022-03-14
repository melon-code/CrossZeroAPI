namespace CrossZeroAPI {
    public class ReadOnlyTable {
        const int firstDimensionArrayIndex = 0;

        readonly Cell[,] initialTable;

        public int Size => initialTable.GetUpperBound(firstDimensionArrayIndex) + 1;
        public Cell this[int i, int j] { get => initialTable[i, j]; }
        public Cell this[Coordinate point] { get => initialTable[point.Row, point.Column]; }

        public ReadOnlyTable(Cell[,] table) {
            initialTable = table;
        }
    }
}
