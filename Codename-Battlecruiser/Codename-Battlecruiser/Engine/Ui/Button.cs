using Codename_Battlecruiser.Engine.Base;
using Codename_Battlecruiser.Engine.Rendering;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Codename_Battlecruiser.Engine.Ui
{
    public class Button
    {
        public Action<Vector2i> OnButtonPressed;

        public Shape ButtonShape;

        public Text Label;

        private bool isInteractable = true;

        private float cooldown = 0f;

        private Font font;

        public Button(bool isInteractable, Vector2f buttonSize, bool doOutline = false)
        {
            ButtonShape = new RectangleShape(buttonSize);

            ButtonShape.Origin = new Vector2f(ButtonShape.Origin.X, buttonSize.Y / 2.75f);

            ButtonShape.OutlineThickness = 2;

            if (doOutline)
                ButtonShape.OutlineColor = Color.Black;

            //Text
            font = new Font(Directories.pathToFonts + "Oswald-Medium.ttf");

            Label = new Text("null", font);

            Label.CharacterSize = 22;
            Label.FillColor = Color.Black;

            this.isInteractable = isInteractable;
        }
        public void ChangeText(string message)
            => Label.DisplayedString = message;
        public void SetNewPosition(Vector2f newPosition)
        {
            ButtonShape.Position = newPosition;
            Label.Position = ButtonShape.Position;
        }
        public void ChangeFillColor(Color color)
            => ButtonShape.FillColor = color;
        public void TryPressButton(Vector2i position)
        {
            if (!isInteractable)
                return;

            if(cooldown <= 10f)
            {
                cooldown += Time.GetTime();
                return;
            }

            if (!IsMouseOver())
                return;

            if (Mouse.IsButtonPressed(Mouse.Button.Left))
                OnButtonPressed.Invoke(position);

            cooldown = 0;
        }
        private bool IsMouseOver()
        {
            if (!isInteractable)
                return false;

            float mouseX = Mouse.GetPosition(Render.window).X;
            float mouseY = Mouse.GetPosition(Render.window).Y;

            float buttonPosX = ButtonShape.GetGlobalBounds().Left;
            float buttonPosY = ButtonShape.GetGlobalBounds().Top;

            float buttonPosWidth = ButtonShape.GetGlobalBounds().Width + buttonPosX;
            float buttonPosHeight = ButtonShape.GetGlobalBounds().Height + buttonPosY;

            if (mouseX < buttonPosWidth && mouseX > buttonPosX && mouseY < buttonPosHeight && mouseY > buttonPosY)
                return true;

            return false;
        }
    }
}
