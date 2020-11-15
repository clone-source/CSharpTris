using System.Drawing;

namespace CSharpTris.Engine
{
    public class GraphicsPainter
    {
        private int _width;
        private int _height;

        public Bitmap Buffer;
        public Graphics g;

        public GraphicsPainter(int width, int height)
        {
            _width = width;
            _height = height;

            Buffer = new Bitmap(width, height);
            g = Graphics.FromImage(Buffer);
        }
    }
}
