
public enum CellState
{
    Free,
    Ship,
    Destroyed,
}
namespace Codename_Battlecruiser.Game_Assets
{
    public class Cell
    {
        public CellState CellState { get; private set; } = CellState.Free;

        public bool IsOccupied = false;

        public bool IsShip = false;

        public bool IsDestroyed = false;

        public int HpCount = 6;

        public Cell() { }

        public void CreateShip()
        {
            IsOccupied = true;
            IsShip = true;  

            CellState = CellState.Ship;
        }
        public void DestroyCell(int damage)
        {
            if(IsShip)
            {
                HpCount -= damage;

                if (HpCount <= 0)
                {
                    IsShip = false;
                    IsDestroyed = true;
                    CellState = CellState.Destroyed;
                }
            }
            else
            {
                IsDestroyed = true;
                CellState = CellState.Destroyed;
            }
        }
    }
}
