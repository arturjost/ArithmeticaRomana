# ArithmeticaRomana.Core 🏛️

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