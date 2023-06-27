
using SFML.System;

namespace Codename_Battlecruiser.Game_Assets
{
    public class Player
    {
        public Cells PlayerMaps = new Cells();

        private Player opponent;

        public Player(){}

        public void BindEnemy(Player enemy)
            => opponent = enemy;

        public void ShootEnemyCell(Vector2i shootPos)
        {
            if (IsUnrevealedShipPoint(shootPos))
                PlayerMaps.VisibleCells[shootPos.X, shootPos.Y].CreateShip();

            PlayerMaps.VisibleCells[shootPos.X, shootPos.Y].DestroyCell();
            opponent.PlayerMaps.Map[shootPos.X, shootPos.Y].DestroyCell();
        }
        private bool IsUnrevealedShipPoint(Vector2i shootPos)
        {
            bool cellIsEnemyShip = opponent.PlayerMaps.Map[shootPos.X, shootPos.Y].IsShip;
            bool cellIsNotRevelaledShip = !PlayerMaps.VisibleCells[shootPos.X, shootPos.Y].IsShip;

            return cellIsEnemyShip && cellIsNotRevelaledShip;
        }
    }
}
