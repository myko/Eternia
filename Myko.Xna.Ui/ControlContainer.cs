using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Myko.Xna.Ui
{
    public class ControlContainer: Control
    {
        public ControlCollection Controls { get; private set; }

        public ControlContainer()
        {
            Controls = new ControlCollection(this);
        }

        public void HandleInputControls(Vector2 position, GameTime gameTime)
        {
            Controls.ForEach(control => control.HandleInput(position + control.Position, gameTime));
        }

        public void UpdateControls(GameTime gameTime)
        {
            Controls.ForEach(control => control.Update(gameTime));
        }

        public void DrawControls(Vector2 position, GameTime gameTime)
        {
            Controls.ForEach(control => control.Draw(position + control.Position, gameTime));
        }
    }
}
