namespace Example {
    using System;

    using static HsluvS.Hsluv;

    class Program {
        static void Main(string[] args) {
            Console.WriteLine(HslToHex((0, 100, 50)));
        }
    }
}
