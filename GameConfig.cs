using GameScreenConfig = CSharpTris.Screens.Game.Config;

namespace CSharpTris
{
    public static class GameConfig
    {
        public static int CanvasWidth =
            GameScreenConfig.SideBarWidth + GameScreenConfig.PlayFieldWidth;

        public static int CanvasHeight =
            GameScreenConfig.PuzzleHeight * GameScreenConfig.SquareWidth;
    }
}
