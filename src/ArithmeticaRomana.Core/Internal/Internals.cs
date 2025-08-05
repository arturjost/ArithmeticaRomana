using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ArithmeticaRomana.Core.Unit.Tests")]
namespace ArithmeticaRomana.Core.Internal
{
    internal static class Internals
    {
        public static readonly RomanNumeralMap Apostrophus =
            new RomanNumeralMap(["I", "V", "X", "L", "C", "D", "ↀ", "ↁ", "ↂ", "ↇ", "ↈ", "IↃↃↃↃ", "CCCCIↃↃↃↃ"]);
        public static readonly RomanNumeralMap Vinculum =
            new RomanNumeralMap(["I", "V", "X", "L", "C", "D", "M"], "\u0305");
        public static readonly RomanNumeralMap FramedNumerals =
            new RomanNumeralMap(["I", "V", "X", "L", "C", "D", "ↀ", "ↁ", "ↂ", "ↇ", "|I|", "|V|", "|X|"]);
    }
}
