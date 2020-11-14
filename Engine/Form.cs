using System.Drawing;
using System.Windows.Forms;
using Forms = System.Windows.Forms;

namespace CSharpTris.Engine
{
    class Player
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; }
        public float Height { get; }

        public Player(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }

    public sealed class Form : Forms.Form
    {
        private new int Width;
        private new int Height;
        readonly Bitmap _buffer;
        readonly Graphics _bufferG;

        private readonly Player _player;

        public Form(int width, int height)
        {
            _player = new Player(0, 0, 20, 20);

            Width = width;
            Height = height;
            Text = @"Pong";
            Size = new Size(Width, Height);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            DoubleBuffered = true;

            _buffer = new Bitmap(Width, Height);
            _bufferG = Graphics.FromImage(_buffer);

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint,
                true);
        }

        public new void Update()
        {
            const float dt = 15 / 1000f;

            _player.X += 55 * dt;
            _player.Y += 111 * dt;

            if (_player.X > Width)
            {
                _player.X = -_player.Width;
            }

            if (_player.Y > Height)
            {
                _player.Y = -_player.Height;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            _bufferG.Clear(Color.White);
            _bufferG.FillRectangle(Brushes.Brown, _player.X, _player.Y, 20, 20);
            g.DrawImage(_buffer, 0, 0, Width, Height);
        }
    }
}
