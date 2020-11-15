using CSharpTris.Properties;

namespace CSharpTris.Screens.Game
{
    static public class Config
    {
        public static readonly int SquareWidth = Settings.Default.SquareWidth;
        public static int SquareBorderWidth = 3;
        public static readonly int PuzzleWidth = Settings.Default.PuzzleWidth;
        public static readonly int PuzzleHeight = Settings.Default.PuzzleHeight;
        public static int SideBarWidth = SquareWidth * 6;
        public static int PlayFieldWidth = PuzzleWidth * SquareWidth;  
    }
}
