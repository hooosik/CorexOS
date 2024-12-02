using System.Drawing;
using Cosmos.System.Graphics;

namespace CorexOS.GUI
{
    public class GUIManager
    {
        private Canvas canvas;
        private Cursor cursor;

        public GUIManager(Canvas screenCanvas)
        {
            canvas = screenCanvas;
            cursor = new Cursor(canvas, @"0:\Assets\Posy Black default.bmp"); // Replace .CUR with .BMP
        }

        public void Initialize()
        {
            canvas.Clear(Color.DarkBlue);
            DrawTitleBar("CorexOS GUI");
            canvas.Display();
        }

        public void Update()
        {
            canvas.Clear(Color.DarkBlue);
            DrawTitleBar("CorexOS GUI");
            cursor.Draw(); // Draw the cursor
            canvas.Display();
        }

        private void DrawTitleBar(string title)
        {
            canvas.DrawFilledRectangle(new Pen(Color.LightGray), 0, 0, canvas.Mode.Columns, 30);
            canvas.DrawString(title, Cosmos.System.Graphics.Fonts.PCScreenFont.Default, new Pen(Color.Black), 10, 10);
        }
    }
}
