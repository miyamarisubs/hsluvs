# HSLuv for .NET Standard

>   [HSLuv](http://hsluv.org) implementation for .NET Standard.

````bash
dotnet add package HsluvS
````

## Hsluv vs. HsluvS

|                    | Hsluv  | HsluvS |
| :----------------- | :----: | :----: |
| .NET Standard      | no     | yes    |
| HSLuv method names | Hsluv* | Hsl*   |
| HPLuv method names | Hpluv* | Hpl*   |

## Example

````c#
using static System.Console;

using static HsluvS.Hsluv;

class Program {
    static void Main(string[] args) {
        WriteLine(HslToHex((0, 100, 50))); // → #ea0064
    }
}
````

## API

Tuples:

-   `Hsl = (double h, double s, double l)`
    -   `h: [0, 360)`
    -   `s: [0, 100]`
    -   `l: [0, 100]`
-   `Hpl = (double h, double p, double l)`
    -   `h: [0, 360)`
    -   `p: [0, 100]`
    -   `l: [0, 100]`
-   `Rgb = (double r, double g, double b)`
    -   `r: [0, 1]`
    -   `g: [0, 1]`
    -   `b: [0, 1]`

Methods:

-   `HslToRgb :: Hsl → Rgb`
-   `RgbToHsl :: Rgb → Hsl`
-   `HplToRgb :: Hpl → Rgb`
-   `RgbToHpl :: Rgb → Hpl`
-   `HexToRgb :: string → Rgb`
-   `RgbToHex :: Rgb → string`
-   `HexToHsl :: string → Hsl`
-   `HslToHex :: Hsl → string`
-   `HexToHpl :: string → Hpl`
-   `HplToHex :: Hpl → string`

## License

MIT

HsluvS includes code from [original C# HSLuv implementation](https://github.com/hsluv/hsluv-csharp/blob/master/LICENSE.txt).
