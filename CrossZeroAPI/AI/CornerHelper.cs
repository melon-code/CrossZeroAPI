using System;

namespace CrossZeroAPI {
    public static class CornerHelper {
        const int cornerCount = 4;

        public static Coordinate GetLeftTopCorner() {
            return new Coordinate(0, 0);
        }

        public static Coordinate GetRigthTopCorner(int size) {
            return new Coordinate(0, size - 1);
        }

        public static Coordinate GetRightDownCorner(int size) {
            return new Coordinate(size - 1, size - 1);
        }

        public static Coordinate GetLeftDownCorner(int size) {
            return new Coordinate(size - 1, 0);
        }

        public static Coordinate GetRandomCorner(int size) {
            Random rand = new Random();
            int corner = rand.Next(cornerCount);
            switch (corner) {
                case 0:
                    return GetLeftTopCorner();
                case 1:
                    return GetRigthTopCorner(size);
                case 2:
                    return GetRightDownCorner(size);
                default:
                    return GetLeftDownCorner(size);
            }
        }
    }
}
