using System;
using System.Threading;
using System.Windows.Forms;
using Hello = CSharpTris.Screens.Hello;

namespace CSharpTris.Engine
{
    public class Game
    {
        public Form Form { get; }
        public int Width { get; }
        public int Height { get; }

        private Thread _loopThread;
        private ManualResetEvent _closeGameEvent;
        private const int FrameSize = 15;

        public Game(int width, int height)
        {
            Width = width;
            Height = height;
            Form = new Form(width, height, new Hello.Screen());

            Form.Load += OnLoad;
            Form.Closed += OnClosed;
        }

        void OnLoad(object sender, EventArgs e)
        {
            _closeGameEvent = new ManualResetEvent(false);
            _loopThread = new Thread(Loop) {Name = "Game Thread"};
            _loopThread.Start();
        }

        void OnClosed(object sender, EventArgs e)
        {
            _closeGameEvent.Set();
            _loopThread.Join();
            _closeGameEvent.Close();
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
                invokerResult = Form.BeginInvoke(new MethodInvoker(BuildFrame));

                // wait for the remainder of the frame and then let the loop loop
                // if _closeGameEvent was set, stop the loop
                running = !_closeGameEvent.WaitOne(FrameSize);
            }
        }

        private void BuildFrame()
        {
            Form.Update();
            Form.Invalidate();
        }
    }
}
