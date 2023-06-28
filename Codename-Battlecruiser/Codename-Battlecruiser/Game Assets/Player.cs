
using SFML.System;

namespace Codename_Battlecruiser.Game_Assets
{
    public class Player
    {
        public Cells PlayerMaps = new Cells();

        private Player opponent;

        public Vector2i? chosenCoordinates = null;

        public Player(){}

        public void BindEnemy(Player enemy)
            => opponent = enemy;

        public void ShootEnemyCell(Vector2i damage)
        {
            if (chosenCoordinates == null)
                return;

            Vector2i shootPos = chosenCoordinates.Value;

            if (IsUnrevealedShipPoint(shootPos))
                PlayerMaps.VisibleCells[shootPos.X, shootPos.Y].CreateShip();

            PlayerMaps.VisibleCells[shootPos.X, shootPos.Y].DestroyCell(damage.X);
            opponent.PlayerMaps.Map[shootPos.X, shootPos.Y].DestroyCell(damage.X);

            chosenCoordinates = null;
        }
        public void ChooseShootCoordinate(Vector2i shootPos)
        {
            if (opponent.PlayerMaps.Map[shootPos.X, shootPos.Y].IsDestroyed)
                return;

            chosenCoordinates = shootPos;
        }
        private bool IsUnrevealedShipPoint(Vector2i shootPos)
        {
            bool cellIsEnemyShip = opponent.PlayerMaps.Map[shootPos.X, shootPos.Y].IsShip;
            bool cellIsNotRevelaledShip = !PlayerMaps.VisibleCells[shootPos.X, shootPos.Y].IsShip;

            return cellIsEnemyShip && cellIsNotRevelaledShip;
        }
    }
}
