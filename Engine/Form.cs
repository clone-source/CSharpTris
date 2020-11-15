using System.Drawing;
using System.Windows.Forms;
using Forms = System.Windows.Forms;

namespace CSharpTris.Engine
{
    public sealed class Form : Forms.Form
    {
        private new int Width;
        private new int Height;

        private GameScreen _screen;
        
        public Form(int width, int height, GameScreen screen)
        {

            Width = width;
            Height = height;
            _screen = screen;
            Text = @"Pong";
            Size = new Size(Width, Height);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            DoubleBuffered = true;

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint,
                true);
        }

        public new void Update()
        {
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _screen.Paint();
            e.Graphics.DrawImage(_screen.Canvas(), 0, 0);
        }
    }
}
