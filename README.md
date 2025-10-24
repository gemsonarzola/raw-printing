# Raw Dot Matrix Printing

A .NET library for raw printing to dot matrix printers using ESC/POS commands. Supports both Windows and Linux/macOS systems.

## Features

- Cross-platform support (Windows, Linux, macOS)
- ESC/POS command implementation
- Fluent builder API for receipt creation
- Text formatting options (bold, condensed, double width/height)
- Text alignment (left, center, right)
- Column-based layout support
- Paper feed and form control
- Tear-off positioning

## Installation

1. Clone the repository
2. Build using .NET 9.0:
```sh
dotnet build
```

## Usage

### Basic Example

```csharp
using Helpers.DotMatrixPrinting;
using static Helpers.DotMatrixPrinting.ReceiptBuilder.Align;

var receipt = new ReceiptBuilder(42)
    .SetBold(true)
    .AppendLine("MY STORE", Center)
    .SetBold(false)
    .AppendLine("123 Main Street", Center)
    .AppendColumns(
        ("Item", 20, Left),
        ("Qty", 4, Right),
        ("Price", 7, Right),
        ("Total", 9, Right)
    )
    .Feed();

string data = EscPos.Initialize + receipt.ToString();

// Windows
var printer = new DotMatrixPrinter("EPSON LX-310");
// Linux
// var printer = new DotMatrixPrinter("/dev/usb/lp0");

printer.Print(data);
```

### Supported Printers

The library has been tested with:
- EPSON LX-310
- Other ESC/POS compatible printers should work

### Platform-Specific Notes

#### Windows
Uses the Windows Printing System API for raw printing.

#### Linux/macOS
- Direct device printing via `/dev/usb/lpX` 
- Fallback to CUPS (`lp` command) when using printer names

## License

[MIT License](LICENSE)
