namespace Tests {
    using Xunit;

    using static HsluvS.Hsluv;

    public class Hsluv {
        private static readonly TupleComparer Comparer = new TupleComparer();

        [Fact]
        public void HslHex() {
            Assert.Equal("#ea0064",      HslToHex((0.1, 100, 50)));
            Assert.Equal((0.1, 100, 50), HexToHsl("#ea0064"), Comparer);
        }

        [Fact]
        public void HplHex() {
            Assert.Equal("#a36572",        HplToHex((359.5, 100, 50)));
            Assert.Equal((359.5, 100, 50), HexToHpl("#a36572"), Comparer);
        }
    }
}
