using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Myko.Xna.Ui
{
    public class Button: ContentControl
    {
        private bool mousePressed = false;

        public event Action Click;

        public Button()
        {
            Width = 80;
            Height = 20;
            Foreground = Color.White;
            Background = Color.DarkGray;
        }

        protected void OnClick()
        {
            if (Click != null)
                Click();
        }

        public override void HandleInput(Vector2 position, GameTime gameTime)
        {
            base.HandleInput(position, gameTime);

            if (mousePressed && IsMouseUp)
                OnClick();

            mousePressed = IsMouseDown;
        }

        public override void Draw(Vector2 position, GameTime gameTime)
        {
            DrawBackground(position);

            base.Draw(position, gameTime);
        }
    }
}
