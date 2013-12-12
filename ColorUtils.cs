using System;
using System.Drawing;

namespace ImageEdit
{
    public static class ColorUtils
    {
        public static byte Lerp(byte start, byte end, float percentToEnd)
        {
            return (byte)(start + Math.Round((end - start) * percentToEnd));
        }

        public static Color Lerp(Color start, Color end, float percentToEnd)
        {
            byte alpha = Lerp(start.A, end.A, percentToEnd);
            byte red = Lerp(start.R, end.R, percentToEnd);
            byte green = Lerp(start.G, end.G, percentToEnd);
            byte blue = Lerp(start.B, end.B, percentToEnd);

            return Color.FromArgb(alpha, Color.FromArgb(red, green, blue));
        }
    }
}