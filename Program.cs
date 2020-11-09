﻿using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace CSharpTris
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.Run(new GameWindow());
        }

        class GameWindow : Form
        {
            private Thread _gameThread;
            private ManualResetEvent resetEvent;

            public GameWindow()
            {
                Text = "Pong";
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

            private void Loop()
            {
                IAsyncResult tick = null;
                while (!resetEvent.WaitOne(15))
                {
                    if (tick != null)
                    {
                        if (!tick.AsyncWaitHandle.WaitOne(0))
                        {
                            // we are running too slow, maybe we can do something about it
                            if (WaitHandle.WaitAny(
                                new WaitHandle[]
                                {
                                    resetEvent,
                                    tick.AsyncWaitHandle
                                }) == 0)
                            {
                                return;
                            }
                        }
                    }

                    tick = BeginInvoke(new MethodInvoker(OnGameTimerTick));
                }
            }

            private void OnGameTimerTick()
            {
                // perform game physics here
                // don't draw anything

                Invalidate();
            }

            private void ExitGame()
            {
                Close();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                var g = e.Graphics;
                g.Clear(Color.White);

                // do all painting here
                // don't do your own double-buffering here, it is working already
                // don't dispose g
            }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);
                resetEvent = new ManualResetEvent(false);
                _gameThread = new Thread(Loop);
                _gameThread.Name = "Game Thread";
                _gameThread.Start();
            }

            protected override void OnClosed(EventArgs e)
            {
                resetEvent.Set();
                _gameThread.Join();
                resetEvent.Close();
                base.OnClosed(e);
            }

            protected override void OnPaintBackground(PaintEventArgs e)
            {
                // do nothing
            }
        }
    }
}
