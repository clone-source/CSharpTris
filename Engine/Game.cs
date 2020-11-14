
namespace CSharpTris.Engine
{
    public class Game
    {
        public Form Form { get; }
        public int Width { get; }
        public int Height { get; }

        public Game(int width, int height)
        {
            Width = width;
            Height = height;
            
            Form = new Form(width, height);
        }
    }
}
