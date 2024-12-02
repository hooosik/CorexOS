using System.Drawing;
using Cosmos.System.Graphics;

namespace CorexOS.Utils
{
    public static class ColorUtils
    {
        public static Color FromRGB(byte red, byte green, byte blue)
        {
            // Create an ARGB integer (Cosmos Color requires int)
            int colorValue = (red << 16) | (green << 8) | blue;
            return Color.FromArgb(colorValue);
        }
    }
}
