using Codename_Battlecruiser.Engine.Rendering;
using Codename_Battlecruiser.Game_Assets;

namespace Codename_Battlecruiser.Engine.Base
{
    public class Game
    {
        public static Game Instance { get; private set; }

        public static string PathToProject = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        public Player Player1 = new Player(false);
        public Player Player2 = new Player(true);
        public Player CurrentPlayer;

        private PlayerTurn turn = PlayerTurn.Player1;

        #region Init
        public void InitGame()
        {
            Player1.BindEnemy(Player2);
            Player2.BindEnemy(Player1);

            CurrentPlayer = Player1;

            Instance = this;

            Player1.PlayerMaps.GenerateField();
            Player2.PlayerMaps.GenerateField();

            Render.InitRender();
        }
        public void DoGameUpdate()
        {
            PickPlayer();

            CurrentPlayer.DoStep();
        }
        private void PickPlayer()
        {
            switch (turn)
            {
                case PlayerTurn.Player1:
                    CurrentPlayer = Player1;
                    break;
                case PlayerTurn.Player2:
                    CurrentPlayer = Player2;
                    break;
            }
        }
        private void ChangeTurn()
        {
            if (turn == PlayerTurn.Player1 && !Player1.IsStreak)
                turn = PlayerTurn.Player2;
            else if (turn == PlayerTurn.Player2 && !Player2.IsStreak)
                turn = PlayerTurn.Player1;
        }
        public void OnPlayerFinished()
        {
            ChangeTurn();

            Thread.Sleep(500);
        }
        #endregion
    }
}
