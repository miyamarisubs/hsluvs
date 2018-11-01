namespace HsluvS {
    using static System.Math;

    // 2-tuple of double
    using T2 = System.ValueTuple<double, double>;

    // 3-tuple of double
    using T3 = System.ValueTuple<double, double, double>;

    // 4-tuple of double
    using T4 = System.ValueTuple<double, double, double, double>;

    // 3-tuple of T3
    using T33 = System.ValueTuple<
        System.ValueTuple<double, double, double>, System.ValueTuple<double, double, double>,
        System.ValueTuple<double, double, double>>;

    // 6-tuple of T2
    using T62 =
        System.ValueTuple<System.ValueTuple<double, double>, System.ValueTuple<double, double>,
            System.ValueTuple<double, double>, System.ValueTuple<double, double>, System.ValueTuple<double, double>,
            System.ValueTuple<double, double>>;

    public static class Hsluv {
        #region Math
        private static readonly T33 M = ((3.240969941904521, -1.537383177570093, -0.498610760293),
                                         (-0.96924363628087, 1.87596750150772, 0.041555057407175),
                                         (0.055630079696993, -0.20397695888897, 1.056971514242878));

        private static readonly T33 MInv = ((0.41239079926595, 0.35758433938387, 0.18048078840183),
                                            (0.21263900587151, 0.71516867876775, 0.072192315360733),
                                            (0.019330818715591, 0.11919477979462, 0.95053215224966));

        private const double RefX = 0.95045592705167;
        private const double RefY = 1.0;
        private const double RefZ = 1.089057750759878;

        private const double RefU = 0.19783000664283;
        private const double RefV = 0.46831999493879;

        private const double K = 903.2962962;
        private const double E = 0.0088564516;

        private static T62 GetBounds(double l) {
            T62 result = ((0, 0), (0, 0), (0, 0), (0, 0), (0, 0), (0, 0));

            var sub1 = Pow(l + 16, 3) / 1560896;
            var sub2 = sub1 > E ? sub1 : l / K;

            // ReSharper disable JoinDeclarationAndInitializer
            double m1;
            double m2;
            double m3;
            double top1;
            double top2;
            double bottom;

            // ReSharper restore JoinDeclarationAndInitializer

            #region Outer 1
            m1 = M.Item1.Item1;
            m2 = M.Item1.Item2;
            m3 = M.Item1.Item3;

            #region Inner 1
            top1   = (284517 * m1 - 94839 * m3)                    * sub2;
            top2   = (838422 * m3 + 769860 * m2 + 731718 * m1) * l * sub2;
            bottom = (632260 * m3 - 126452 * m2)                   * sub2;

            result.Item1 = (top1 / bottom, top2 / bottom);
            #endregion

            #region Inner 2
            top1   = (284517 * m1 - 94839 * m3) * sub2;
            top2   = (838422 * m3 + 769860 * m2 + 731718 * m1) * l * sub2 - 769860 * l;
            bottom = (632260 * m3 - 126452 * m2)                   * sub2 + 126452;

            result.Item2 = (top1 / bottom, top2 / bottom);
            #endregion
            #endregion

            #region Outer 2
            m1 = M.Item2.Item1;
            m2 = M.Item2.Item2;
            m3 = M.Item2.Item3;

            #region Inner 3
            top1   = (284517 * m1 - 94839 * m3)                    * sub2;
            top2   = (838422 * m3 + 769860 * m2 + 731718 * m1) * l * sub2;
            bottom = (632260 * m3 - 126452 * m2)                   * sub2;

            result.Item3 = (top1 / bottom, top2 / bottom);
            #endregion

            #region Inner 4
            top1   = (284517 * m1 - 94839 * m3) * sub2;
            top2   = (838422 * m3 + 769860 * m2 + 731718 * m1) * l * sub2 - 769860 * l;
            bottom = (632260 * m3 - 126452 * m2)                   * sub2 + 126452;

            result.Item4 = (top1 / bottom, top2 / bottom);
            #endregion
            #endregion

            #region Outer 3
            m1 = M.Item3.Item1;
            m2 = M.Item3.Item2;
            m3 = M.Item3.Item3;

            #region Inner 5
            top1   = (284517 * m1 - 94839 * m3)                    * sub2;
            top2   = (838422 * m3 + 769860 * m2 + 731718 * m1) * l * sub2;
            bottom = (632260 * m3 - 126452 * m2)                   * sub2;

            result.Item5 = (top1 / bottom, top2 / bottom);
            #endregion

            #region Inner 6
            top1   = (284517 * m1 - 94839 * m3) * sub2;
            top2   = (838422 * m3 + 769860 * m2 + 731718 * m1) * l * sub2 - 769860 * l;
            bottom = (632260 * m3 - 126452 * m2)                   * sub2 + 126452;

            result.Item6 = (top1 / bottom, top2 / bottom);
            #endregion
            #endregion

            return result;
        }

        private static double IntersectLineLine(T2 a, T2 b) => (a.Item2 - b.Item2) / (b.Item1 - a.Item1);
        private static double DistanceFromPole(T2  point) => Sqrt(Pow(point.Item1, 2) + Pow(point.Item2, 2));

        private static (bool, double) LengthOfRayUntilIntersect(double theta, T2 line) {
            var length = line.Item2 / (Sin(theta) - line.Item1 * Cos(theta));

            return (length >= 0, length);
        }

        private static double MaxSafeChromaForL(double l) {
            var bounds = GetBounds(l);
            var min    = double.MaxValue;

            // ReSharper disable JoinDeclarationAndInitializer
            double m1;
            double b1;
            double x;
            double length;

            // ReSharper restore JoinDeclarationAndInitializer

            #region 1
            m1     = bounds.Item1.Item1;
            b1     = bounds.Item1.Item2;
            x      = IntersectLineLine((m1, b1), (-1 / m1, 0));
            length = DistanceFromPole((x, b1 + x * m1));
            min    = Min(min, length);
            #endregion

            #region 2
            m1     = bounds.Item2.Item1;
            b1     = bounds.Item2.Item2;
            x      = IntersectLineLine((m1, b1), (-1 / m1, 0));
            length = DistanceFromPole((x, b1 + x * m1));
            min    = Min(min, length);
            #endregion

            #region 3
            m1     = bounds.Item3.Item1;
            b1     = bounds.Item3.Item2;
            x      = IntersectLineLine((m1, b1), (-1 / m1, 0));
            length = DistanceFromPole((x, b1 + x * m1));
            min    = Min(min, length);
            #endregion

            #region 4
            m1     = bounds.Item4.Item1;
            b1     = bounds.Item4.Item2;
            x      = IntersectLineLine((m1, b1), (-1 / m1, 0));
            length = DistanceFromPole((x, b1 + x * m1));
            min    = Min(min, length);
            #endregion

            #region 5
            m1     = bounds.Item5.Item1;
            b1     = bounds.Item5.Item2;
            x      = IntersectLineLine((m1, b1), (-1 / m1, 0));
            length = DistanceFromPole((x, b1 + x * m1));
            min    = Min(min, length);
            #endregion

            #region 6
            m1     = bounds.Item6.Item1;
            b1     = bounds.Item6.Item2;
            x      = IntersectLineLine((m1, b1), (-1 / m1, 0));
            length = DistanceFromPole((x, b1 + x * m1));
            min    = Min(min, length);
            #endregion

            return min;
        }

        private static double MaxChromaForLh(double l, double h) {
            var hrad = h / 360 * PI * 2;

            var bounds = GetBounds(l);
            var min    = double.MaxValue;

            // ReSharper disable JoinDeclarationAndInitializer
            T2     bound;
            bool   intersects;
            double length;

            // ReSharper restore JoinDeclarationAndInitializer

            #region 1
            bound = bounds.Item1;

            (intersects, length) = LengthOfRayUntilIntersect(hrad, bound);

            if (intersects) min = Min(min, length);
            #endregion

            #region 2
            bound = bounds.Item2;

            (intersects, length) = LengthOfRayUntilIntersect(hrad, bound);

            if (intersects) min = Min(min, length);
            #endregion

            #region 3
            bound = bounds.Item3;

            (intersects, length) = LengthOfRayUntilIntersect(hrad, bound);

            if (intersects) min = Min(min, length);
            #endregion

            #region 4
            bound = bounds.Item4;

            (intersects, length) = LengthOfRayUntilIntersect(hrad, bound);

            if (intersects) min = Min(min, length);
            #endregion

            #region 5
            bound = bounds.Item5;

            (intersects, length) = LengthOfRayUntilIntersect(hrad, bound);

            if (intersects) min = Min(min, length);
            #endregion

            #region 6
            bound = bounds.Item6;

            (intersects, length) = LengthOfRayUntilIntersect(hrad, bound);

            if (intersects) min = Min(min, length);
            #endregion

            return min;
        }

        private static double DotProduct(T3 a, T3 b) => a.Item1 * b.Item1 + a.Item2 * b.Item2 + a.Item3 * b.Item3;

        private static double Fix(double value, int places) {
            var n = Pow(10, places);

            return Round(value * n) / n;
        }

        private static double FromLinear(double c) => c <= 0.0031308 ? 12.92 * c : 1.055 * Pow(c, 1 / 2.4) - 0.055;

        private static double ToLinear(double c) => c > 0.04045 ? Pow((c + 0.055) / (1 + 0.055), 2.4) : c / 12.92;

        // ReSharper disable once InconsistentNaming
        private static double YToL(double y) => y <= E ? y / RefY * K : 116 * Pow(y / RefY, 1.0 / 3.0) - 16;

        // ReSharper disable once InconsistentNaming
        private static double LToY(double l) => l <= 8 ? RefY * l / K : RefY * Pow((l + 16) / 116, 3);

        private static double Clamp(double x, double a, double b) =>
            x < a ? a :
            x > b ? b : x;

        public static (byte, byte, byte) ToBytes(T3 t3) {
            var (x1, x2, x3) = t3;

            x1 = Clamp(Fix(x1, 3), 0, 1);
            x2 = Clamp(Fix(x2, 3), 0, 1);
            x3 = Clamp(Fix(x3, 3), 0, 1);

            return ((byte)Round(x1 * 255), (byte)Round(x2 * 255), (byte)Round(x3 * 255));
        }
        #endregion

        #region XYZ ↔ RGB
        public static T3 XyzToRgb(T3 xyz) =>
            (FromLinear(DotProduct(M.Item1, xyz)), FromLinear(DotProduct(M.Item2, xyz)),
             FromLinear(DotProduct(M.Item3, xyz)));

        public static T3 RgbToXyz(T3 rgb) {
            var (r, g, b) = rgb;

            var rgbl = (ToLinear(r), ToLinear(g), ToLinear(b));

            return (DotProduct(MInv.Item1, rgbl), DotProduct(MInv.Item2, rgbl), DotProduct(MInv.Item3, rgbl));
        }
        #endregion

        #region XYZ ↔ LUV
        public static T3 XyzToLuv(T3 xyz) {
            var (x, y, z) = xyz;

            var varU = 4 * x / (x + 15 * y + 3 * z);
            var varV = 9 * y / (x + 15 * y + 3 * z);

            var l = YToL(y);

            if (Abs(l) < 0.00000001) return (0, 0, 0);

            var u = 13 * l * (varU - RefU);
            var v = 13 * l * (varV - RefV);

            return (l, u, v);
        }

        public static T3 LuvToXyz(T3 luv) {
            var (l, u, v) = luv;

            if (Abs(l) < 0.00000001) return (0, 0, 0);

            var varU = u / (13 * l) + RefU;
            var varV = v / (13 * l) + RefV;

            var y = LToY(l);
            var x = 0 - 9 * y                               * varU / ((varU - 4) * varV - varU * varV);
            var z = (9 * y - 15 * varV * y - varV * x) / (3 * varV);

            return (x, y, z);
        }
        #endregion

        #region LUV ↔ LCH
        public static T3 LuvToLch(T3 luv) {
            var (l, u, v) = luv;

            var c    = Pow(Pow(u, 2) + Pow(v, 2), 0.5);
            var hrad = Atan2(v, u);

            var h = hrad * 180.0 / PI;

            if (h < 0) h = 360 + h;

            return (l, c, h);
        }

        public static T3 LchToLuv(T3 lch) {
            var (l, c, h) = lch;

            var hrad = h / 360.0 * 2 * PI;
            var u    = Cos(hrad)     * c;
            var v    = Sin(hrad)     * c;

            return (l, u, v);
        }
        #endregion

        #region HSL ↔ LCH
        public static T3 HslToLch(T3 hsl) {
            var (h, s, l) = hsl;

            if (l > 99.9999999) return (100, 0, h);
            if (l < 0.00000001) return (0, 0, h);

            var max = MaxChromaForLh(l, h);
            var c   = max / 100 * s;

            return (l, c, h);
        }

        public static T3 LchToHsl(T3 lch) {
            var (l, c, h) = lch;

            if (l > 99.9999999) return (h, 0, 100);
            if (l < 0.00000001) return (h, 0, 0);

            var max = MaxChromaForLh(l, h);
            var s   = c / max * 100;

            return (h, s, l);
        }
        #endregion

        #region HPL ↔ LCH
        public static T3 HplToLch(T3 hpl) {
            var (h, p, l) = hpl;

            if (l > 99.9999999) return (100, 0, h);
            if (l < 0.00000001) return (0, 0, h);

            var max = MaxSafeChromaForL(l);
            var c   = max / 100 * p;

            return (l, c, h);
        }

        public static T3 LchToHpl(T3 lch) {
            var (l, c, h) = lch;

            if (l > 99.9999999) return (h, 0, 100);
            if (l < 0.00000001) return (h, 0, 0);

            var max = MaxSafeChromaForL(l);
            var s   = c / max * 100;

            return (h, s, l);
        }
        #endregion

        #region LCH ↔ RGB
        public static T3 LchToRgb(T3 lch) => XyzToRgb(LuvToXyz(LchToLuv(lch)));

        public static T3 RgbToLch(T3 rgb) => LuvToLch(XyzToLuv(RgbToXyz(rgb)));
        #endregion

        #region HSL ↔ RGB
        public static T3 HslToRgb(T3 hsl) => LchToRgb(HslToLch(hsl));

        public static T3 RgbToHsl(T3 rgb) => LchToHsl(RgbToLch(rgb));
        #endregion

        #region HPL ↔ RGB
        public static T3 HplToRgb(T3 hpl) => LchToRgb(HplToLch(hpl));

        public static T3 RgbToHpl(T3 rgb) => LchToHpl(RgbToLch(rgb));
        #endregion

        #region CMYK ↔ RGB
        public static T3 CmykToRgb(T4 cmyk) {
            var (c, m, y, k) = cmyk;
            var k1 = 1 - k;

            return ((1 - c) * k1, (1 - m) * k1, (1 - y) * k1);
        }

        public static T4 RgbToCmyk(T3 rgb) {
            var (r, g, b) = rgb;
            var k  = 1 - Max(r, Max(g, b));
            var k1 = 1 - k;

            return ((1 - r - k) * k1, (1 - g - k) * k1, (1 - b - k) * k1, k);
        }
        #endregion

        #region RGB ↔ HEX
        public static string RgbToHex(T3 rgb) {
            var (r, g, b) = ToBytes(rgb);

            return $"#{r:x2}{g:x2}{b:x2}";
        }

        // TODO: Parse w/o #, parse #rgb colors.
        public static T3 HexToRgb(string hex) =>
            (int.Parse(hex.Substring(1, 2), System.Globalization.NumberStyles.HexNumber) / 255.0,
             int.Parse(hex.Substring(3, 2), System.Globalization.NumberStyles.HexNumber) / 255.0,
             int.Parse(hex.Substring(5, 2), System.Globalization.NumberStyles.HexNumber) / 255.0);
        #endregion

        #region HSL ↔ HEX
        public static string HslToHex(T3 hsl) => RgbToHex(HslToRgb(hsl));

        public static T3 HexToHsl(string hex) => RgbToHsl(HexToRgb(hex));
        #endregion

        #region HPL ↔ HEX
        public static string HplToHex(T3 hpl) => RgbToHex(HplToRgb(hpl));

        public static T3 HexToHpl(string hex) => RgbToHpl(HexToRgb(hex));
        #endregion

        #region CMYK ↔ HEX
        public static string CmykToHex(T4 cmyk) => RgbToHex(CmykToRgb(cmyk));

        public static T4 HexToCmyk(string hex) => RgbToCmyk(HexToRgb(hex));
        #endregion
    }
}
