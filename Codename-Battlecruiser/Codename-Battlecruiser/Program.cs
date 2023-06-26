using Codename_Battlecruiser.Engine.Base;

namespace Codename_Battlecruiser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameLoop gameLoop = GameLoop.InitGameLoop();

            gameLoop.LaunchGame();
        }
    }
}