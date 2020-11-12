using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace CSharpTris
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

    internal sealed class CSharpTris : Form
    {
        private Thread _loopThread;
        private ManualResetEvent _closeGameEvent;
        private const int FrameSize = 15;
        readonly Bitmap _buffer ;
        readonly Graphics _bufferG;

        private readonly Player _player;

        public CSharpTris()
        {
            _player = new Player(0, 0, 20, 20);
            Text = @"Pong";
            Size = new Size(800, 600);
            _buffer = new Bitmap(Width, Height);
            _bufferG = Graphics.FromImage(_buffer);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            DoubleBuffered = true;

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint,
                true);
        }

        private bool IsStuck(IAsyncResult invokerResult)
        {
            if (invokerResult.AsyncWaitHandle.WaitOne(0)) return false;

            // invokerResult exists but the wait handle is not ready yet
            // it's been `FrameSize` milliseconds already so we to get us out of the slowness

            // if both _resetEvent and invokerResult's WaitHandle are nowhere to be seen
            // let's get out of the loop
            return WaitHandle.WaitAny(
                new[]
                {
                    _closeGameEvent,
                    invokerResult.AsyncWaitHandle
                }) == 0;
        }

        private void Loop()
        {
            // invoker is in charge of getting a handle on the Main Thread and invoke BuildFrame
            IAsyncResult invokerResult = null;
            var running = true;

            while (running)
            {
                if (invokerResult != null && IsStuck(invokerResult))
                {
                    return;
                }

                // Now tell main thread to build the frame
                invokerResult = BeginInvoke(new MethodInvoker(BuildFrame));

                // wait for the remainder of the frame and then let the loop loop
                // if _closeGameEvent was set, stop the loop
                running = !_closeGameEvent.WaitOne(FrameSize);
            }
        }

        private void BuildFrame()
        {
            Compute();
            Invalidate();
        }

        private void Compute()
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

        // private void ExitGame()
        // {
        //     Close();
        // }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            _bufferG.Clear(Color.White);
            _bufferG.FillRectangle(Brushes.Brown, _player.X, _player.Y, 20, 20);
            g.DrawImage(_buffer, 0, 0, Width, Height);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _closeGameEvent = new ManualResetEvent(false);
            _loopThread = new Thread(Loop) {Name = "Game Thread"};
            _loopThread.Start();
        }

        protected override void OnClosed(EventArgs e)
        {
            _closeGameEvent.Set();
            _loopThread.Join();
            _closeGameEvent.Close();
            base.OnClosed(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // do nothing
        }
    }
}
