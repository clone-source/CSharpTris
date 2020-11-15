using System;
using System.Drawing;

namespace CSharpTris.Engine
{
    public abstract class GameScreen
    {
        void Update(int dt)
        {
        }

        public abstract void Paint();

        public abstract Bitmap Canvas();

        protected void Unload()
        {
        }
    }
}
