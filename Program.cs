using System;
using System.Diagnostics;
using System.Windows.Forms;
using CSharpTris.Engine;

namespace CSharpTris
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Debug.WriteLine(GameConfig.CanvasWidth);
            Debug.WriteLine(GameConfig.CanvasHeight);
            Application.Run(new Game(GameConfig.CanvasWidth, GameConfig.CanvasHeight).Form);
        }
    }
}
