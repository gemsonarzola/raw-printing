

using Helpers.DotMatrixPrinting;
using static Helpers.DotMatrixPrinting.ReceiptBuilder.Align;

  // var receipt = new ReceiptBuilder(42)
  //           .SetBold(true).SetDoubleWidth(true).AppendLine("MY STORE", Center)
  //           .SetDoubleWidth(false).AppendLine("123 Main Street", Center)
  //           .SetBold(false).AppendLine("------------------------------------------")
  //           // .SetCondensed(true)
  //           .AppendColumns(("Item", 20, Left), ("Qty", 4, Right), ("Price", 7, Right), ("Total", 9, Right))
  //           .AppendLine("------------------------------------------")
  //           .AppendColumns(("Milk 1L", 20, Left), ("2", 4, Right), ("55.00", 7, Right), ("110.00", 9, Right))
  //           .AppendColumns(("Bread", 20, Left), ("1", 4, Right), ("40.00", 7, Right), ("40.00", 9, Right))
  //           .AppendColumns(("Sugar 1kg", 20, Left), ("1", 4, Right), ("60.00", 7, Right), ("60.00", 9, Right))
  //           // .SetCondensed(false)
  //           .AppendLine("------------------------------------------")
  //           .SetBold(true).AppendLine("TOTAL: 210.00", Right)
  //           .SetBold(false)
  //           .AppendLine()
  //           .AppendLine("Thank you for shopping!", Center)
  //           .Cut(); // 👈 feeds paper and simulates cut

    var model = new PrintResponse
    {
      AccountNumber = "1234567890",
      AccountNumberWithDash = "12-3456-7890",
      ConsumerName = "John Doe",
      ConsumerAddress = "123 Main St, Cityville",
      TotalBillsAmount = "1500.00",
      MeterNumber = "MTR123456",
      ConsumerType = "R",
      ServerDate = "2024-06-01",
      ServerTime = "12:00:00"
    };

var receipt = new ReceiptBuilder(72)
          .SetFormLengthInLines(22)
          .AppendLine()
          .AppendLine()
          .AppendColumns(
              (model.AccountNumber[..2], 3, Center),
              (model.AccountNumberWithDash, 16, Center),
              (model.MeterNumber, 16, Center),
              (model.ConsumerType, 4, Center),
              (model.ServerDate, 16, Center),
              (model.ServerTime, 11, Center)
          )
          .AppendLine() // 👈 feeds paper and simulates cut
          .AppendLine(model.ConsumerName)
          .AppendLine(model.ConsumerAddress)
          .AppendColumns(
              (model.TotalBillsAmount, 64, Right)
          )
          .Feed();

        string data = EscPos.Initialize + receipt.ToString();

        var printer = new DotMatrixPrinter(OperatingSystem.IsWindows() ? "EPSON LX-310" : "/dev/usb/lp0");
        printer.Print(data);

        Console.WriteLine("Receipt printed successfully!");