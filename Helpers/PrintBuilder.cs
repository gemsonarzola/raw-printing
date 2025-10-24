using System;
using System.Text;

namespace Helpers.DotMatrixPrinting
{
    public class ReceiptBuilder
    {
        private readonly StringBuilder _builder = new();
        private readonly int _lineWidth;

        public ReceiptBuilder(int lineWidth = 80)
        {
            _lineWidth = lineWidth;
        }

        // --- Style controls ---
        public ReceiptBuilder SetBold(bool enabled)
        {
            _builder.Append(enabled ? EscPos.BoldOn : EscPos.BoldOff);
            return this;
        }

        public ReceiptBuilder SetCondensed(bool enabled)
        {
            _builder.Append(enabled ? EscPos.CondensedOn : EscPos.CondensedOff);
            return this;
        }

        public ReceiptBuilder SetDoubleWidth(bool enabled)
        {
            _builder.Append(enabled ? EscPos.DoubleWidthOn : EscPos.DoubleWidthOff);
            return this;
        }

        public ReceiptBuilder SetDoubleHeight(bool enabled)
        {
            _builder.Append(enabled ? EscPos.DoubleHeightOn : EscPos.DoubleHeightOff);
            return this;
        }

        // --- Text output ---
        public ReceiptBuilder AppendLine(string text = "", Align align = Align.Left)
        {
            _builder.Append(GetAlignCode(align));
            _builder.AppendLine(text);
            return this;
        }

        public ReceiptBuilder AppendColumns(params (string text, int width, Align align)[] columns)
        {
            var line = new StringBuilder();
            foreach (var (text, width, align) in columns)
                line.Append(FormatColumn(text, width, align));
            _builder.AppendLine(line.ToString());
            return this;
        }

        // --- Feed / Cut / Tear-Off ---
        public ReceiptBuilder FeedLines(int lines = 5)
        {
            _builder.Append(EscPos.FeedLines(lines));
            return this;
        }


        public ReceiptBuilder MoveToTearOff()
        {
            _builder.Append(EscPos.MoveToTearOff);
            return this;
        }

        public ReceiptBuilder ReturnFromTearOff()
        {
            _builder.Append(EscPos.ReturnFromTearOff);
            return this;
        }

        // --- Paper configuration ---
        public ReceiptBuilder SetFormLengthInLines(int lines)
        {
            _builder.Append(EscPos.SetPageLengthInLines(lines));
            return this;
        }

        public ReceiptBuilder SetFormLengthInInches(int inches)
        {
            _builder.Append(EscPos.SetPageLengthInInches(inches));
            return this;
        }

        public ReceiptBuilder SetTopMargin(int lines)
        {
            _builder.Append(EscPos.SetTopMargin(lines));
            return this;
        }

        // --- Helpers ---
        private static string GetAlignCode(Align align) => align switch
        {
            Align.Center => EscPos.AlignCenter,
            Align.Right => EscPos.AlignRight,
            _ => EscPos.AlignLeft
        };

        private static string FormatColumn(string text, int width, Align align)
        {
            if (text.Length > width) text = text[..width];
            return align switch
            {
                Align.Left => text.PadRight(width),
                Align.Right => text.PadLeft(width),
                Align.Center => text.PadLeft((width + text.Length) / 2).PadRight(width),
                _ => text
            };
        }

        public ReceiptBuilder SetTearOffPosition(int lines)
        {
            // Feed forward a specific number of lines to reach tear-off zone
            // Each "line" = 1/6 inch by default at 6 LPI
            if (lines > 0)
                _builder.Append(EscPos.FeedLines(lines));
            else if (lines < 0)
                _builder.Append(EscPos.ReverseFeedLines(Math.Abs(lines)));

            return this;
        }
        
        public ReceiptBuilder Feed(bool fullCut = true)
        {
            _builder.Append(EscPos.FeedLines(6));
            _builder.Append(EscPos.FormFeed);
            return this;
        }


        public override string ToString() => _builder.ToString();

        public enum Align
        {
            Left,
            Center,
            Right
        }
    }
}
