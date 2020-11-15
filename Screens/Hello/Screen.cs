using System.Drawing;
using CSharpTris.Engine;


namespace CSharpTris.Screens.Hello
{
    public class Screen : GameScreen
    {
        private HelloPainter _painter =
            new HelloPainter(GameConfig.CanvasWidth, GameConfig.CanvasHeight);

        public override void Paint()
        {
            _painter.Clear();
            _painter.DrawGraphic();
        }

        public override Bitmap Canvas()
        {
            return _painter.Buffer;
        }
    }
}
