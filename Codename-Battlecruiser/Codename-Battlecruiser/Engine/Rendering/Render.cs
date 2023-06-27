using Codename_Battlecruiser.Engine.Base;
using Codename_Battlecruiser.Engine.Ui;
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

        private static Button[,] Field = new Button[Cells.fieldSize, Cells.fieldSize];
        private static Button[,] VisibleField = new Button[Cells.fieldSize, Cells.fieldSize];

        #region Init
        public static void InitRender()
        {
            window = new RenderWindow(new VideoMode(width, height), "Codename: Battlecruiser");

            window.SetFramerateLimit(wantedFrameRate);

            InitFields();
        }
        private static void InitFields()
        {
            Field = CreateVisualField(Game.Instance.Player1.PlayerMaps.Map, 100, false);
            VisibleField = CreateVisualField(Game.Instance.Player2.PlayerMaps.Map, 600, true);

            BindActions();
        }
        private static void BindActions()
        {
            for(int i = 0; i < Cells.fieldSize; i++)
            {
                for(int j = 0; j < Cells.fieldSize; j++)
                {
                    VisibleField[i, j].OnButtonPressed += () => Game.Instance.Player1.ShootEnemyCell(new Vector2i(i, j));
                }
            }
        }
        private static Button[,] CreateVisualField(Cell[,] cells, float yOffset, bool isInteractable)
        {
            Button[,] buttons = new Button[Cells.fieldSize, Cells.fieldSize];

            float cordX = 100;
            float cordY = yOffset;

            for(int i = 0; i < Cells.fieldSize; i++)
            {
                for(int j = 0; j < Cells.fieldSize; j++)
                {
                    buttons[i, j] = new Button(isInteractable);

                    buttons[i, j].SetNewPosition(new Vector2f(cordX, cordY));

                    cordX += 24.5f;
                }

                cordX = 100;
                cordY += 24.5f;
            }

            buttons.FetchMap(cells);

            return buttons;
        }
        #endregion

        public static void UpdateRender()
        {
            window.Clear(Color.White);

            window.DispatchEvents();

            DrawGameObjects();

            VisibleField.TryPressButtons();

            window.Display();
        }
        private static void TryPressButtons(this Button[,] buttons)
        {
            foreach(var button in buttons)
                button.TryPressButton();
        }
        #region Drawing
        private static void DrawGameObjects()
        {
            Field.FetchMap(Game.Instance.Player1.PlayerMaps.Map);
            VisibleField.FetchMap(Game.Instance.Player1.PlayerMaps.VisibleCells);

            VisibleField[0, 0].ButtonShape.FillColor = Color.Red;

            DrawButtons(VisibleField);
            DrawButtons(Field);
        }
        private static Button[,] FetchMap(this Button[,] _buttons, Cell[,] fetched)
        {
            Button[,] buttons = _buttons;

            for (int i = 0; i < Cells.fieldSize; i++)
            {
                for (int j = 0; j < Cells.fieldSize; j++)
                {
                    buttons[i, j].ButtonShape.FillColor = GetCellColor(fetched[i, j]);
                }
            }

            return buttons;
        }
        private static Color GetCellColor(Cell cell)
        {
            if(!cell.IsOccupied)
                return Color.Cyan;

            if(cell.IsShip)
                return Color.Green;

            if(cell.IsDestroyed)
                return Color.Red;

            return cell.HpCount switch
            { 
                1 => Color.Magenta,
                2 => Color.Magenta,
                3 => Color.Yellow,
                4 => Color.Yellow,
                5 => Color.Green,
                6 => Color.White,
                _ => Color.Black,
            };
        }
        private static void DrawButtons(Button[,] buttons)
        {
            foreach(var button in buttons)
                window.Draw(button.ButtonShape);
        }
        #endregion

        #region Closing
        private static void WindowClosed(object sender, EventArgs e)
        {
            RenderWindow w = (RenderWindow)sender;
            w.Close();
        }
        public static void TryClose()
            => window.Closed += WindowClosed;
        #endregion
    }
}
