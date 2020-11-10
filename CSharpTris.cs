using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace CSharpTris
{
    sealed class CSharpTris : Form
    {
        private Thread _gameThread;
        private ManualResetEvent _closeGameEvent;
        private const int FrameSize = 15;
        private double _x;

        public CSharpTris()
        {
            Text = @"Pong";
            Size = new Size(800, 600);
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
            // invokerResult exists but the wait handle is not ready yet
            // it's been `FrameSize` milliseconds already so we to get us out of the slowness
            if (!invokerResult.AsyncWaitHandle.WaitOne(0))
            {
                // if both _resetEvent and invokerResult's WaitHandle are nowhere to be seen
                // let's get out of the loop
                if (WaitHandle.WaitAny(
                    new[]
                    {
                        _closeGameEvent,
                        invokerResult.AsyncWaitHandle
                    }) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void Loop()
        {
            // invoker is in charge of getting a handle on the Main Thread and invoke BuildFrame
            IAsyncResult invokerResult = null;
            bool running = true;

            while (running)
            {
                if (invokerResult != null && IsStuck(invokerResult))
                {
                    return;
                }

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
            _x += 1;
        }

        private void ExitGame()
        {
            Close();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            g.DrawRectangle(new Pen(Color.Brown), (float) _x, (float) _x, 20, 20);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _closeGameEvent = new ManualResetEvent(false);
            _gameThread = new Thread(Loop);
            _gameThread.Name = "Game Thread";
            _gameThread.Start();
        }

        protected override void OnClosed(EventArgs e)
        {
            _closeGameEvent.Set();
            _gameThread.Join();
            _closeGameEvent.Close();
            base.OnClosed(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // do nothing
        }
    }
}
