using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ArithmeticaRomana.Core.Unit.Tests")]
namespace ArithmeticaRomana.Core.Internal
{
    internal static class Internals
    {
        public static readonly RomanNumeralMap Apostrophus =
            new RomanNumeralMap("I", "V", "X", "L", "C", "D", "ↀ", "ↁ", "ↂ", "ↇ", "ↈ", "IↃↃↃↃ", "CCCCIↃↃↃↃ");
        public static readonly RomanNumeralMap Vinculum =
            new RomanNumeralMap("I", "V", "X", "L", "C", "D", "M", "V\u0305", "X\u0305", "L\u0305", "C\u0305", "D\u0305", "M\u0305", "V\u0305\u0305", "X\u0305\u0305", "L\u0305\u0305", "C\u0305\u0305", "D\u0305\u0305", "M\u0305\u0305");
        public static readonly RomanNumeralMap FramedNumerals =
            new RomanNumeralMap("I", "V", "X", "L", "C", "D", "ↀ", "ↁ", "ↂ", "ↇ", "|I\u0305|", "|V\u0305|", "|X\u0305|");
    }
}
