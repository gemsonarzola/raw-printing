namespace Helpers.DotMatrixPrinting
{
    public static class EscPos
    {
        public const string ESC = "\x1B";
        public const string GS = "\x1D";

        // --- Printer control ---
        public static readonly string Initialize = ESC + "@";

        // --- Fonts and styles ---
        public static readonly string BoldOn = ESC + "E" + "\x01";
        public static readonly string BoldOff = ESC + "E" + "\x00";
        public static readonly string CondensedOn = ESC + "\x0F";
        public static readonly string CondensedOff = ESC + "P";
        public static readonly string DoubleWidthOn = ESC + "W" + "\x01";
        public static readonly string DoubleWidthOff = ESC + "W" + "\x00";
        public static readonly string DoubleHeightOn = ESC + "w" + "\x01";
        public static readonly string DoubleHeightOff = ESC + "w" + "\x00";

        // --- Alignment ---
        public static readonly string AlignLeft = ESC + "a" + "\x00";
        public static readonly string AlignCenter = ESC + "a" + "\x01";
        public static readonly string AlignRight = ESC + "a" + "\x02";

        // --- Paper feeding ---
        public static string FeedLines(int n) => ESC + "d" + (char)n;
        public static readonly string FormFeed = "\x0C";

        // --- Paper length & margins ---
        public static string SetPageLengthInLines(int lines) => ESC + "C" + (char)lines;
        public static string SetPageLengthInInches(int inches) => ESC + "C" + "\x00" + (char)inches;
        public static string SetTopMargin(int lines) => ESC + "O" + (char)lines;

        // --- Tear-off controls ---
        public static readonly string TearOffOn = ESC + "\x19" + "\x01";   // ESC EM 1
        public static readonly string TearOffOff = ESC + "\x19" + "\x00";  // ESC EM 0
        public static readonly string MoveToTearOff = GS + "V" + "\x01";   // GS V 1
        public static readonly string ReturnFromTearOff = GS + "V" + "\x00"; // GS V 0

        // --- Cut (for printers that support it) ---
        public static readonly string CutFull = GS + "V" + "\x00";
        public static readonly string CutPartial = GS + "V" + "\x01";

                // Feed backward (reverse feed)
        public static string ReverseFeedLines(int n) => ESC + "e" + (char)n;


    }
}
