using System.Drawing;
using CSharpTris.Engine;
using CSharpTris.Screens.Game.Colors;

namespace CSharpTris.Screens.Hello
{
    public class HelloPainter : GraphicsPainter
    {
        public HelloPainter(int width, int height) : base(width, height)
        {
        }

        public void Clear()
        {
            g.Clear(UiColors.Background);
        }

        public void DrawGraphic()
        {
            g.FillRectangle(new SolidBrush(ShapeColors.Cyan), 100, 99, 30, 40);
        }
    }
}
