using Codename_Battlecruiser.Engine.Rendering;
using Codename_Battlecruiser.Game_Assets;

namespace Codename_Battlecruiser.Engine.Base
{
    public class Game
    {
        public static Game Instance { get; private set; }

        public static string PathToProject = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;


        public Player Player1 = new Player();
        public Player Player2 = new Player();

        #region Init
        public void InitGame()
        {
            Player1.BindEnemy(Player2);
            Player2.BindEnemy(Player1);

            Instance = this;

            Player1.PlayerMaps.GenerateField();
            Player2.PlayerMaps.GenerateField();

            Render.InitRender();
        }
        #endregion
    }
}
