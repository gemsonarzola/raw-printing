using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Helpers.DotMatrixPrinting
{
    public class DotMatrixPrinter
    {
        private readonly string _printerNameOrDevice;

        public DotMatrixPrinter(string printerNameOrDevice = "EPSON LX-310 ESC/P (Copy 1)")
        {
            _printerNameOrDevice = printerNameOrDevice;
        }

        public void Print(string content)
        {
            if (OperatingSystem.IsWindows())
            {
                PrintWindows(content);
            }
            else if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
            {
                PrintLinux(content);
            }
            else
            {
                throw new PlatformNotSupportedException("Unsupported OS for dot-matrix printing.");
            }
        }

        // --- WINDOWS RAW PRINT ---
            private void PrintWindows(string data)
        {
            try
            {
                if (!RawPrinterHelper.SendStringToPrinter(_printerNameOrDevice, data))
                {
                    throw new Exception("Failed to send data to printer.");
                }

                Console.WriteLine($"✅ Printed to Windows printer: {_printerNameOrDevice}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Windows printing error: {ex.Message}");
            }
        }

        // --- LINUX RAW PRINT ---
        private void PrintLinux(string content)
        {
            string devicePath = _printerNameOrDevice;

            // If user passed printer name instead of device path, fallback to CUPS
            if (!devicePath.StartsWith("/dev/"))
            {
                string tempFile = Path.Combine(Path.GetTempPath(), "printjob.txt");
                File.WriteAllText(tempFile, content);
                Process.Start("lp", $"-d \"{_printerNameOrDevice}\" \"{tempFile}\"")?.WaitForExit();
                return;
            }

            if (!File.Exists(devicePath))
                throw new FileNotFoundException($"Printer device not found at {devicePath}");

            using var fs = new FileStream(devicePath, FileMode.Open, FileAccess.Write);
            using var writer = new StreamWriter(fs);
            writer.Write(content);
            writer.Flush();
        }

        public static class RawPrinterHelper
        {
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public class DOCINFOA
            {
                [MarshalAs(UnmanagedType.LPStr)] public string pDocName;
                [MarshalAs(UnmanagedType.LPStr)] public string pOutputFile;
                [MarshalAs(UnmanagedType.LPStr)] public string pDataType;
            }

            [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true)]
            public static extern bool OpenPrinter(string pPrinterName, out IntPtr phPrinter, IntPtr pDefault);

            [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true)]
            public static extern bool ClosePrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true)]
            public static extern bool StartDocPrinter(IntPtr hPrinter, int level, [In] DOCINFOA di);

            [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true)]
            public static extern bool EndDocPrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true)]
            public static extern bool StartPagePrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true)]
            public static extern bool EndPagePrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true)]
            public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

            public static bool SendStringToPrinter(string printerName, string data)
            {
                IntPtr hPrinter;
                DOCINFOA di = new()
                {
                    pDocName = "DotMatrix Print Job",
                    pDataType = "RAW"
                };

                if (!OpenPrinter(printerName, out hPrinter, IntPtr.Zero))
                    return false;

                try
                {
                    StartDocPrinter(hPrinter, 1, di);
                    StartPagePrinter(hPrinter);

                    IntPtr pBytes = Marshal.StringToCoTaskMemAnsi(data);
                    WritePrinter(hPrinter, pBytes, data.Length, out _);
                    Marshal.FreeCoTaskMem(pBytes);

                    EndPagePrinter(hPrinter);
                    EndDocPrinter(hPrinter);
                }
                finally
                {
                    ClosePrinter(hPrinter);
                }

                return true;
            }
        }
    }
}
