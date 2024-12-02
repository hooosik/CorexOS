using System.Drawing;
using Cosmos.System.Graphics;

namespace CorexOS.GUI
{
    public class Icon
    {
        public string Name { get; }
        public int X { get; }
        public int Y { get; }

        public Icon(string name, int x, int y)
        {
            Name = name;
            X = x;
            Y = y;
        }

        public void Draw(Canvas canvas)
        {
            // Draw a simple rectangle for the icon
            canvas.DrawFilledRectangle(new Pen(Color.LightGray), X, Y, 50, 50);

            // Draw the icon's name
            canvas.DrawString(Name, Cosmos.System.Graphics.Fonts.PCScreenFont.Default, new Pen(Color.Black), X + 5, Y + 55);
        }
    }
}
