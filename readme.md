# ArithmeticaRomana 🏛️
Ever needed to bring a touch of ancient Rome to your .NET projects? `ArithmeticaRomana.Core` is a robust library designed for processing and manipulating Roman numerals.
`ArithmeticaRomana.WPF` is a calculator app to demonstrate the usage of the library.

It's not just your basic `I` to `MMMCMXCIX`. This library supports values all the way up to `2,147,483,647` by using the *vinculum* notation (the overline that multiplies a numeral's value by 1,000).

> For more information on Roman numerals, check out the Wikipedia article:
> https://en.wikipedia.org/wiki/Roman_numerals

# Overview 

* **`src/ArithmeticaRomana.Core/`**: Contains the core logic for Roman numeral parsing, formatting, and representation.

* **`src/ArithmeticaRomana.WPF/`**: Contains the WPF application files, including views, view models, and the main application entry point.

* **`tests/ArithmeticaRomana.Core.Unit.Tests/`**: Includes unit tests for the core library's functionality, ensuring accuracy and reliability.

* **`tests/ArithmeticaRomana.WPF.Unit.Tests/`**: Contains unit tests for the WPF application's view models and other components.

* **`ArithmeticaRomana.sln`**: The Visual Studio solution file that organizes the entire project.

* **`.github/workflows/build-test-publish-wpf.yml`**: A GitHub Actions workflow file that automates the build, testing, and publishing process for the WPF application.

---