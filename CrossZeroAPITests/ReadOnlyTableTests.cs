using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrossZeroAPI;

namespace CrossZeroAPITests {
    [TestClass]
    public class ReadOnlyTableTests {
        static ReadOnlyTable CreateTable(int size) {
            Cell[,] grid = new Cell[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    grid[i, j] = Cell.Empty;
            return new ReadOnlyTable(grid);
        }

        static void CheckSize(int size) {
            Assert.AreEqual(size, CreateTable(size).Size);
        }

        [TestMethod]
        public void CreateNotEmptyTable() {
            ReadOnlyTable table = new ReadOnlyTable(new Cell[,] { { Cell.Empty, Cell.Cross }, { Cell.Empty, Cell.Zero } });
            Assert.AreEqual(Cell.Empty, table[0, 0]);
            Assert.AreEqual(Cell.Cross, table[0, 1]);
            Assert.AreEqual(Cell.Zero, table[new Coordinate(1, 1)]);
        }

        [TestMethod]
        public void Size() {
            const int minSize = 2;
            const int maxSize = 7;
            for (int i = minSize; i < maxSize; i++)
                CheckSize(i);
        }
    }
}