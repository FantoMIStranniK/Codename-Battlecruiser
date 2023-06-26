using Codename_Battlecruiser.Engine.Base;
using Codename_Battlecruiser.Game_Assets;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Codename_Battlecruiser.Engine.Rendering
{
    public static class Render
    {
        public static RenderWindow window;
        public static uint wantedFrameRate = 144;

        public static uint width = 1000;
        public static uint height = 1000;

        private static Shape[,] Field = new Shape[Cells.fieldSize, Cells.fieldSize];

        public static void InitRender()
        {
            window = new RenderWindow(new VideoMode(width, height), "Codename: Battlecruiser");

            window.SetFramerateLimit(wantedFrameRate);

            CreateVisualField(Game.Cells.Map);
        }
        private static void CreateVisualField(Cell[,] cells)
        {
            float cordX = 0;
            float cordY = 0;

            for(int i = 0; i < Cells.fieldSize; i++)
            {
                for(int j = 0; j < Cells.fieldSize; j++)
                {
                    Field[i, j] = CreateVisualCell(cells[i, j]);

                    Field[i, j].Position = new Vector2f(cordX, cordY);

                    cordX += 20f;
                }

                cordX = 0f;
                cordY += 20f;
            }
        }
        private static Shape CreateVisualCell(Cell cell)
        {
            Color color = cell.CellState switch
            {
                CellState.Free => Color.Cyan,
                CellState.Ship => Color.Green,
                CellState.Destroyed => Color.Red,
            };

            var visualCell = new RectangleShape(new Vector2f(20f, 20f));

            visualCell.FillColor = color;

            return visualCell;
        }
        public static void RenderWindow()
        {
            window.Clear(Color.White);

            window.DispatchEvents();

            DrawGameObjects();

            window.Display();
        }
        public static void TryClose()
        {
            window.Closed += WindowClosed;
        }
        private static void DrawGameObjects()
        {
            foreach (var item in Field)
                window.Draw(item);
        }
        private static void WindowClosed(object sender, EventArgs e)
        {
            RenderWindow w = (RenderWindow)sender;
            w.Close();
        }
    }
}
