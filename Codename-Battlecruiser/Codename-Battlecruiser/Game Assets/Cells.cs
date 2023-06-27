using SFML.System;

namespace Codename_Battlecruiser.Game_Assets
{
    public class Cells
    {
        public Cell[,] Map = new Cell[fieldSize, fieldSize];

        public Cell[,] VisibleCells = new Cell[fieldSize, fieldSize];

        private Random rand = new Random();

        public static int fieldSize { get; private set; } = 10;
        public static int shipCount { get; private set; } = 20;

        public Cells() { }

        public void GenerateField()
        {
            Map = GenerateBlankField(Map);
            VisibleCells = GenerateBlankField(VisibleCells);

            PlaceShips();
        }
        private void PlaceShips()
        {
            Vector2i coordinateForShip = new Vector2i(0, 0);

            for (int i = 0; i < shipCount; i++)
            {
                do
                {
                    coordinateForShip = new Vector2i(rand.Next(0, fieldSize), rand.Next(0, fieldSize));
                }
                while (Map[coordinateForShip.X, coordinateForShip.Y].IsOccupied);

                Map[coordinateForShip.X, coordinateForShip.Y].CreateShip();
            }
        }
        private Cell[,] GenerateBlankField(Cell[,] map)
        {
            for(int i = 0; i < fieldSize; i++)
                for(int j = 0; j < fieldSize; j++)
                    map[i, j] = new Cell();

            return map;
        }
    }
}
