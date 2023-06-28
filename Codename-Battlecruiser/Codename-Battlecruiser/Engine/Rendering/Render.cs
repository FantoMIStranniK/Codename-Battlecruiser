using Codename_Battlecruiser.Engine.Base;
using Codename_Battlecruiser.Engine.Ui;
using Codename_Battlecruiser.Game_Assets;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

public enum ShootType
{
    ShootATA = 2,
    Yamato = 3,
    Hellstorm = 1
}
namespace Codename_Battlecruiser.Engine.Rendering
{
    public static class Render
    {
        #region Parameters
        public static RenderWindow window;
        public static uint wantedFrameRate = 144;

        public static uint width = 1000;
        public static uint height = 1000;
        #endregion

        private static Button[,] Field = new Button[Cells.fieldSize, Cells.fieldSize];
        private static Button[,] VisibleField = new Button[Cells.fieldSize, Cells.fieldSize];

        private static Dictionary<ShootType, Button> ControlButtons = new Dictionary<ShootType, Button>();

        private static Button previousButton;

        #region Init
        public static void InitRender()
        {
            window = new RenderWindow(new VideoMode(width, height), "Codename: Battlecruiser");

            window.SetFramerateLimit(wantedFrameRate);

            InitFields();

            InitControlButtons();
        }
        private static void InitFields()
        {
            Field = CreateVisualField(Game.Instance.Player1.PlayerMaps.Map, 100, false);
            VisibleField = CreateVisualField(Game.Instance.Player2.PlayerMaps.Map, 600, true);

            BindActions();
        }
        private static void InitControlButtons()
        {
            Vector2f startPosition = new Vector2f(750, 250);

            List<Color> colors = new List<Color>
            {
                Color.Green, 
                Color.Red, 
                Color.Yellow,
            };
            List<string> labels = new List<string>
            {
                "ATA",
                "Yamato cannon",
                "''Hellstorm'' rockets",
            };


            int buttonIndex = 0;

            foreach (ShootType type in Enum.GetValues(typeof(ShootType)))
            {
                CreateButton(startPosition, type, colors[buttonIndex], labels[buttonIndex]);

                buttonIndex++;

                startPosition.Y += 200;
            }
        }
        private static void CreateButton(Vector2f position, ShootType shootType, Color color, string message)
        {
            Button button = new Button(true, new Vector2f(200, 75), true);

            button.SetNewPosition(position);

            button.ChangeFillColor(color);

            button.OnButtonPressed += Game.Instance.Player1.ShootEnemyCell;

            button.ChangeText(message);

            ControlButtons.Add(shootType, button);
        }
        private static void BindActions()
        {
            for(int i = 0; i < Cells.fieldSize; i++)
            {
                for(int j = 0; j < Cells.fieldSize; j++)
                {
                    VisibleField[i, j].OnButtonPressed += Game.Instance.Player1.ChooseShootCoordinate;
                }
            }
        }
        private static Button[,] CreateVisualField(Cell[,] cells, float yOffset, bool isInteractable)
        {
            Button[,] buttons = new Button[Cells.fieldSize, Cells.fieldSize];

            float cordX = 110;
            float cordY = yOffset;

            for(int i = 0; i < Cells.fieldSize; i++)
            {
                for(int j = 0; j < Cells.fieldSize; j++)
                {
                    buttons[i, j] = new Button(isInteractable, new Vector2f(22.5f, 22.5f));

                    buttons[i, j].SetNewPosition(new Vector2f(cordX, cordY));

                    cordX += 26.5f;
                }

                cordX = 110;
                cordY += 26.5f;
            }

            buttons.FetchMap(cells, false);

            return buttons;
        }
        #endregion

        public static void UpdateRender()
        {
            window.Clear(Color.White);

            window.DispatchEvents();

            DrawGameObjects();

            VisibleField.TryPressButtons();

            ControlButtons.TryPressControls();

            window.Display();
        }
        private static void TryPressControls(this Dictionary<ShootType, Button> buttons)
        {
            foreach (var type in buttons.Keys)
                buttons[type].TryPressButton(new Vector2i((int)type, 0));
        }
        private static void TryPressButtons(this Button[,] buttons)
        {
            for (int i = 0; i < Cells.fieldSize; i++)
            {
                for (int j = 0; j < Cells.fieldSize; j++)
                {
                    VisibleField[i, j].TryPressButton(new Vector2i(i, j));
                }
            }
        }

        #region Drawing
        private static void DrawGameObjects()
        {
            Field.FetchMap(Game.Instance.Player1.PlayerMaps.Map, false);
            VisibleField.FetchMap(Game.Instance.Player1.PlayerMaps.VisibleCells, true);

            DrawField(VisibleField);
            DrawField(Field);

            DrawControlButtons(ControlButtons);
        }
        private static Button[,] FetchMap(this Button[,] _buttons, Cell[,] fetched, bool isVisibleMap)
        {
            Button[,] buttons = _buttons;

            for (int i = 0; i < Cells.fieldSize; i++)
            {
                for (int j = 0; j < Cells.fieldSize; j++)
                {
                    buttons[i, j].ButtonShape.FillColor = GetCellColor(fetched[i, j]);

                    if (isVisibleMap && new Vector2i(i, j) == Game.Instance.Player1.chosenCoordinates)
                    {
                        if(previousButton != null)
                            previousButton.ButtonShape.OutlineColor = Color.White;

                        previousButton = buttons[i, j];

                        previousButton.ButtonShape.OutlineColor = Color.Black;
                    }
                    else if (isVisibleMap && Game.Instance.Player1.chosenCoordinates == null)
                    {
                        buttons[i, j].ButtonShape.OutlineColor = Color.White;
                    }
                }
            }

            return buttons;
        }
        private static Color GetCellColor(Cell cell)
        {
            if(cell.IsDestroyed)
                return Color.Red;

            if (!cell.IsOccupied)
                return Color.Cyan;

            return cell.HpCount switch
            { 
                1 => Color.Magenta,
                2 => Color.Magenta,
                3 => Color.Yellow,
                4 => Color.Yellow,
                5 => Color.Green,
                6 => Color.Green,
                _ => Color.Black,
            };
        }
        private static void DrawField(Button[,] field)
        {
            foreach(var cell in field)
                window.Draw(cell.ButtonShape);
        }
        private static void DrawControlButtons(Dictionary<ShootType, Button> buttons)
        {
            foreach (var button in buttons.Values)
            {
                window.Draw(button.ButtonShape);
                window.Draw(button.Label);
            }
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
