# ArithmeticaRomana.Core 🏛️
Ever needed to bring a touch of ancient Rome to your .NET projects? `ArithmeticaRomana.Core` is a robust library designed for processing and manipulating Roman numerals.

It's not just your basic `I` to `X`. This library supports values all the way up to `2,147,483,647` by using the *vinculum* notation (the overline that multiplies a numeral's value by 1,000).

> For more information on Roman numerals, check out the Wikipedia article:
> https://en.wikipedia.org/wiki/Roman_numerals

---

## Features

* **Modular Design**: Provides `IRomanNumeralParser` and `IRomanNumeralFormatter` interfaces to clearly define and separate parsing and formatting responsibilities.

* **Default Implementations**: Includes `VinculumRomanNumeralParser` and `VinculumRomanNumeralFormatter` for immediate and practical usage.

* **`RomanNumeral` Class**: Provides a primary class for directly creating and interacting with Roman numeral values.

---

## Examples

### IRomanNumeralParser
```cs
IRomanNumeralParser parser = new VinculumRomanNumeralParser();
IRomanParserResult parseResult = parser.Parse("MCMXCIV");

if (parseResult.IsSuccess)
{
    Console.WriteLine($"Parsed 'MCMXCIV' to: {parseResult.Value}"); // Output: 1994
}
```
### IRomanNumeralFormatter
```cs
IRomanNumeralFormatter formatter = new VinculumRomanNumeralFormatter();
string roman = formatter.Format(2025);
Console.WriteLine($"Formatted 2025 to: {roman}"); // Output: MMXXV
```

### RomanNumeral
```cs
var romanNumeral = new RomanNumeral(2025);
Console.WriteLine($"Formatted 2025 to: {romanNumber}"); // Output: MMXXV
```