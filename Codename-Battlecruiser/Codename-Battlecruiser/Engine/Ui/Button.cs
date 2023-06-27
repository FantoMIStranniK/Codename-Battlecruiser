using Codename_Battlecruiser.Engine.Rendering;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Codename_Battlecruiser.Engine.Ui
{
    public class Button
    {
        public Action OnButtonPressed = new Action(() => { });

        public Shape ButtonShape;

        private Vector2f buttonSize = new Vector2f(22.5f, 22.5f);

        private bool isInteractable = true;

        public Button(bool isInteractable)
        {
            ButtonShape = new RectangleShape(buttonSize);
            this.isInteractable = isInteractable;
        }
        public void SetNewPosition(Vector2f newPosition)
            => ButtonShape.Position = newPosition;
        public void ChangeFillColor(Color color)
            => ButtonShape.FillColor = color;
        public void TryPressButton()
        {
            if (!isInteractable)
                return;

            if (!IsMouseOver())
                return;

            if (Mouse.IsButtonPressed(Mouse.Button.Left))
                OnButtonPressed.Invoke();
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
