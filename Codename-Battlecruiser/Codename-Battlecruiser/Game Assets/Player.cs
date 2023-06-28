
using Codename_Battlecruiser.Engine.Base;
using SFML.System;
using System;

namespace Codename_Battlecruiser.Game_Assets
{
    public class Player
    {
        public Cells PlayerMaps = new Cells();

        public Dictionary<ShootType, int> Arsenal = new Dictionary<ShootType, int>
        {
            {ShootType.Yamato, 4},
            {ShootType.ShootATA, int.MaxValue},
            {ShootType.Hellstorm, 7},
        };

        public bool IsStreak = false;
        public bool IsAi = false;   

        public Vector2i? chosenCoordinates = null;

        private Vector2i? memoryCords = null;

        private Player opponent;

        private Random rand = new Random();

        #region Init
        public Player(bool isAi)
            => IsAi = isAi;
        public void BindEnemy(Player enemy)
            => opponent = enemy;
        #endregion

        #region Shooting
        public void ShootEnemyCell(Vector2i damage)
        {
            if (chosenCoordinates == null)
                return;

            if (!CanUseWeapon(damage.X))
                return;

            Vector2i shootPos = chosenCoordinates.Value;

            if (IsUnrevealedShipPoint(shootPos))
                PlayerMaps.VisibleCells[shootPos.X, shootPos.Y].CreateShip();

            if (opponent.PlayerMaps.Map[shootPos.X, shootPos.Y].IsShip)
                IsStreak = true;
            else
                IsStreak = false;

            PlayerMaps.VisibleCells[shootPos.X, shootPos.Y].DestroyCell(damage.X);
            opponent.PlayerMaps.Map[shootPos.X, shootPos.Y].DestroyCell(damage.X);

            chosenCoordinates = null;

            Game.Instance.OnPlayerFinished();
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
        private bool CanUseWeapon(int weaponPower)
        {
            ShootType shootType = (ShootType)weaponPower;

            if (Arsenal[shootType] <= 0)
                return false;

            Arsenal[shootType]--;

            return true;
        }
        #endregion

        #region Input

        public void DoStep()
        {
            if (IsAi)
                AIInput();
        }
        private void AIInput()
        {
            if (!IsStreak)
                ShootRandomPoint();
            else
                GetNearPoints();
        }
        private void ShootRandomPoint()
        {
            chosenCoordinates = null;

            int x = 0;
            int y = 0;

            do
            {
                x = rand.Next(0, Cells.fieldSize);
                y = rand.Next(0, Cells.fieldSize);
            } while (opponent.PlayerMaps.Map[x, y].IsDestroyed);

            CheckIfPlayerShip(x, y);

            chosenCoordinates = new Vector2i (x, y);

            ShootEnemyCell(new Vector2i(ChooseWeapon(), 0));
        }
        private void CheckIfPlayerShip(int x, int y)
        {
            if (opponent.PlayerMaps.Map[x, y].IsShip)
                MemberPosition(x, y);
            else
                IsStreak = false;
        }
        private void MemberPosition(int x, int y)
        {
            IsStreak = true;

            memoryCords = new Vector2i(x, y);
        }
        private void GetNearPoints()
        {
            (int, int)[] points = GetAvailablePoints();

            List<(int, int)> validPoints = ValidatePoints(points);

            if (validPoints.Count is 0)
                ShootRandomPoint();
            else
                ShootNearPoints(validPoints);
        }
        private void ShootNearPoints(List<(int, int)> validPoints)
        {
            int randomNearX = validPoints[rand.Next(0, validPoints.Count)].Item1;
            int randomNearY = validPoints[rand.Next(0, validPoints.Count)].Item2;

            CheckIfPlayerShip(randomNearX, randomNearY);
    
            chosenCoordinates = new Vector2i(randomNearX,randomNearY);

            ShootEnemyCell(new Vector2i(ChooseWeapon(), 0));
        }
        private int ChooseWeapon()
        {
            int randomIndex;

            do
            {
                randomIndex = rand.Next(0, Arsenal.Count);
            } while (!CanUseWeapon(randomIndex));

            return randomIndex;
        }
        private List<(int, int)> ValidatePoints((int, int)[] points)
        {
            List<(int, int)> validPoints = new List<(int, int)>();

            foreach (var point in points)
            {
                if (IsValidPoint(point) && !IsBombedPosition(point))
                    validPoints.Add(point);
            }

            return validPoints;
        }
        private bool IsBombedPosition((int, int) point)
        {
            if (!IsValidPoint(point))
                return true;

            return opponent.PlayerMaps.Map[point.Item1, point.Item2].IsDestroyed;
        }
        private bool IsValidPoint((int, int) point)
        {
            bool xIsValid = point.Item1 >= 0 && point.Item1 < Cells.fieldSize;
            bool yIsValid = point.Item2 >= 0 && point.Item2 < Cells.fieldSize;

            return xIsValid && yIsValid;
        }
        private (int, int)[] GetAvailablePoints()
        {
            int startX = memoryCords.Value.X;
            int startY = memoryCords.Value.Y;

            (int, int)[] points = new (int, int)[]
            {
                (startX, startY + 1),
                (startX, startY - 1),
                (startX + 1, startY),
                (startX - 1, startY),
            };

            return points;
        }
        #endregion
    }
}
