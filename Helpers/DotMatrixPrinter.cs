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

        public DotMatrixPrinter(string printerNameOrDevice = "EPSON LX-310")
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
        private void PrintWindows(string content)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(content);
            IntPtr unmanagedBytes = Marshal.AllocCoTaskMem(bytes.Length);
            Marshal.Copy(bytes, 0, unmanagedBytes, bytes.Length);

            try
            {
                if (OpenPrinter(_printerNameOrDevice.Normalize(), out IntPtr hPrinter, IntPtr.Zero))
                {
                    if (StartDocPrinter(hPrinter, 1, IntPtr.Zero))
                    {
                        StartPagePrinter(hPrinter);
                        WritePrinter(hPrinter, unmanagedBytes, bytes.Length, out _);
                        EndPagePrinter(hPrinter);
                        EndDocPrinter(hPrinter);
                    }
                    ClosePrinter(hPrinter);
                }
                else
                {
                    throw new Exception($"Unable to open printer '{_printerNameOrDevice}'");
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(unmanagedBytes);
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

        #region Win32 Imports
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true)]
        private static extern bool OpenPrinter(string src, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true)]
        private static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true)]
        private static extern bool StartDocPrinter(IntPtr hPrinter, int level, IntPtr di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true)]
        private static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true)]
        private static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true)]
        private static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true)]
        private static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);
        #endregion
    }
}
