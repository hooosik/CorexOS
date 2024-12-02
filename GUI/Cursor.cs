using Cosmos.System.Graphics;
using CorexOS.Utils;

namespace CorexOS.GUI
{
    public class Cursor
    {
        private Canvas canvas;
        private CursorLoader cursorLoader;

        public Cursor(Canvas canvas, string cursorPath)
        {
            this.canvas = canvas;
            cursorLoader = new CursorLoader(cursorPath);
        }

        public void Draw()
        {
            // Get the current mouse position
            int mouseX = (int)Cosmos.System.MouseManager.X;
            int mouseY = (int)Cosmos.System.MouseManager.Y;

            // Adjust for the hotspot
            int drawX = mouseX - cursorLoader.HotspotX;
            int drawY = mouseY - cursorLoader.HotspotY;

            // Draw the cursor pixel by pixel
            int width = cursorLoader.Width;
            int height = cursorLoader.Height;
            byte[] pixelData = cursorLoader.PixelData;

            int pixelIndex = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte blue = pixelData[pixelIndex++];
                    byte green = pixelData[pixelIndex++];
                    byte red = pixelData[pixelIndex++];

                    if (red == 0 && green == 0 && blue == 0) // Skip black (transparent)
                        continue;

                    Pen pen = new Pen(ColorUtils.FromRGB(red, green, blue));
                    canvas.DrawPoint(pen, drawX + x, drawY + y);
                }
            }
        }
    }
}
