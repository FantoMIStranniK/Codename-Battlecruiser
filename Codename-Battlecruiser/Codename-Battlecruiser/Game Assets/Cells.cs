using SFML.System;

namespace Codename_Battlecruiser.Game_Assets
{
    public class Cells
    {
        public Cell[,] Map = new Cell[fieldSize, fieldSize];

        private Random rand = new Random();

        public static int fieldSize { get; private set; } = 10;
        public static int shipCount { get; private set; } = 20;

        public Cells() { }

        public void GenerateField()
        {
            GenerateBlankField();

            Vector2i coordinateForShip = new Vector2i(0, 0);

            for(int i = 0; i < shipCount; i++)
            {
                do
                {
                    coordinateForShip = new Vector2i(rand.Next(0, fieldSize), rand.Next(0, fieldSize));
                }
                while (Map[coordinateForShip.X, coordinateForShip.Y].IsOccupied);

                Map[coordinateForShip.X, coordinateForShip.Y].CreateShip();
            }
        }
        public void GenerateBlankField()
        {
            for(int i = 0; i < fieldSize; i++)
                for(int j = 0; j < fieldSize; j++)
                    Map[i, j] = new Cell();
        }
    }
}
