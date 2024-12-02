using Cosmos.System.Graphics;
using System.Collections.Generic;
using System.Drawing;

namespace CorexOS.GUI
{
    public class DesktopManager
    {
        private Canvas canvas;
        private Cursor cursor;
        private List<Icon> icons;

        public DesktopManager(Canvas canvas)
        {
            this.canvas = canvas;
            this.cursor = new Cursor(canvas, @"0:\Assets\Posy Black default.bmp");
            this.icons = new List<Icon>();
        }

        public void Initialize()
        {
            // Add some default icons
            icons.Add(new Icon("My Computer", 50, 50));
            icons.Add(new Icon("Recycle Bin", 150, 50));
            icons.Add(new Icon("Documents", 50, 150));

            DrawBackground();
            DrawIcons();
            canvas.Display();
        }

        public void Update()
        {
            // Clear the screen
            DrawBackground();

            // Draw icons
            DrawIcons();

            // Draw the cursor
            cursor.Draw();

            // Display the canvas
            canvas.Display();
        }

        private void DrawBackground()
        {
            canvas.Clear(Color.DarkBlue); // Background color
        }

        private void DrawIcons()
        {
            foreach (var icon in icons)
            {
                icon.Draw(canvas);
            }
        }
    }
}
