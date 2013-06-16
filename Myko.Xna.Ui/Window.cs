using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Myko.Xna.Ui
{
    public class Window: ControlContainer
    {
        private Vector2? dragOffset;

        public string Title { get; set; }

        public Window()
        {
        }

        public override void HandleInput(Vector2 position, GameTime gameTime)
        {
            base.HandleInput(position, gameTime);

            var mouseState = Mouse.GetState();
            var mousePosition = new Vector2(mouseState.X, mouseState.Y);

            if (IsMouseUp)
                dragOffset = null;

            if (IsMouseDown && !dragOffset.HasValue && (mousePosition.Y - position.Y) < 20)
                dragOffset = mousePosition - position;

            if (dragOffset.HasValue)
                Position = mousePosition - dragOffset.Value;

            HandleInputControls(position + new Vector2(0, 20), gameTime);
        }

        public override void Draw(Vector2 position, GameTime gameTime)
        {
            base.Draw(position, gameTime); 
            
            DrawBackground(position);

            SpriteBatch.Draw(BlankTexture, new Rectangle((int)position.X, (int)position.Y, (int)ActualWidth, 20), Color.Green, ZIndex + 0.01f);
            SpriteBatch.DrawString(Font, Title, position, Color.White, ZIndex + 0.02f);

            DrawControls(position + new Vector2(0, 20), gameTime);
        }
    }
}
