using Codename_Battlecruiser.Engine.Rendering;
using Codename_Battlecruiser.Game_Assets;
using System.Runtime.CompilerServices;
using System.Text;

namespace Codename_Battlecruiser.Engine.Base
{
    public class GameLoop
    {
        private Game game = new Game();

        private static string pathToDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\AAAGR.io";

        public void LaunchGame()
        {
            game.InitGame();

            while (Render.window.IsOpen)
            {
                Time.UpdateSystemTime();

                if (Time.totalTimeBeforeUpdate >= 1 / Render.wantedFrameRate)
                {
                    Time.ResetTimeBeforeUpdate();

                    DoGameStep();

                    Thread.Sleep(1);

                    Time.UpdateTime();
                }

                Render.TryClose();
            }
        }
        private void DoGameStep()
        {
            Render.window.DispatchEvents();

            Render.UpdateRender();
        }
        public static GameLoop InitGameLoop()
        {
            GameLoop gameLoop = new GameLoop();

            if (File.Exists(Directories.pathToConfig + @"Config.ini"))
                LoadConfigs();

            return gameLoop;
        }

        #region Config loading
        private static void LoadConfigs()
        {
            using (StreamReader sr = new StreamReader(Directories.pathToConfig + @"Config.ini"))
            {
                while (!sr.EndOfStream)
                {
                    var input = sr.ReadLine()?.Split("::");

                    if (input.Length >= 2)
                        ProcessConfigLine(input[0], input[1], input[2]);
                }
            }
        }
        private static void ProcessConfigLine(string className, string varName, string configValue)
        {
            Type type = className switch
            {
                "Render" => typeof(Render),
            };

            var field = type?.GetField(varName);

            var varIype = Convert.GetTypeCode(field?.GetValue(null));

            var parsedValue = Convert.ChangeType(configValue, varIype);

            field?.SetValue(null, parsedValue);
        }
        #endregion
    }
}
