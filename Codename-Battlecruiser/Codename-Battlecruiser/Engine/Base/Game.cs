using Codename_Battlecruiser.Engine.Rendering;
using Codename_Battlecruiser.Game_Assets;

namespace Codename_Battlecruiser.Engine.Base
{
    public class Game
    {
        public static Game Instance { get; private set; }

        public static string PathToProject = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        public static Cells Cells { get; private set; }

        #region Init
        public void InitGame()
        {
            Instance = this;
            
            Cells = new Cells();

            Cells.GenerateField();

            Render.InitRender();
        }
        #endregion
    }
}
