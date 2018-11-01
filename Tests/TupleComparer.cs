namespace Tests {
    using static System.Math;

    using System.Collections.Generic;

    // 3-tuple of double
    using T3 = System.ValueTuple<double, double, double>;

    public class TupleComparer : IEqualityComparer<T3> {
        private static double Fix(double value, int places) {
            var n = Pow(10, places);

            return Round(value * n) / n;
        }

        private static double Clamp(double x, double a, double b) =>
            x < a ? a :
            x > b ? b : x;

        private static (byte, byte, byte) ToBytes(T3 t3) {
            var (x1, x2, x3) = t3;

            x1 = Clamp(Fix(x1, 1), 0, 1);
            x2 = Clamp(Fix(x2, 1), 0, 1);
            x3 = Clamp(Fix(x3, 1), 0, 1);

            return ((byte)Round(x1 * 255), (byte)Round(x2 * 255), (byte)Round(x3 * 255));
        }

        public bool Equals(T3 a, T3 b) {
            var a1 = ToBytes(a);
            var b1 = ToBytes(b);

            return a1.Item1 == b1.Item1 && a1.Item2 == b1.Item2 && a1.Item3 == b1.Item3;
        }

        public int GetHashCode(T3 a) => ToBytes(a).GetHashCode();
    }
}
