using System;
using System.Windows.Forms;
using CSharpTris.Engine;

namespace CSharpTris
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.Run(new Game(800, 600).Form);
        }
    }
}
