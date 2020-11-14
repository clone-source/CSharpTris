using System.Drawing;
using System.Windows.Forms;
using Forms = System.Windows.Forms;

namespace CSharpTris.Engine
{
    public sealed class Form : Forms.Form
    {
        private new int Width;
        private new int Height;
        
        public Form(int width, int height)
        {
            Width = width;
            Height = height;
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

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangle(Brushes.Brown, 20, 20, 20, 20);
        }
    }
}
