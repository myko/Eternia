using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Myko.Xna.Ui
{
    public abstract class GameScreen: ControlContainer
    {
        public ScreenManager ScreenManager { get; set; }

        public ContentManager ContentManager
        {
            get
            {
                if (ScreenManager == null)
                    return null;

                return ScreenManager.ContentManager;
            }
        }

        public virtual void LoadContent() { }
        public virtual void HandleInput(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }

        public void HandleInputScreen(GameTime gameTime)
        {
            HandleInput(gameTime);
            HandleInputControls(Position, gameTime);
        }

        public void UpdateScreen(GameTime gameTime)
        {
            Update(gameTime);
            UpdateControls(gameTime);
        }

        public void DrawScreen(GameTime gameTime)
        {
            Draw(gameTime);
            DrawControls(Position, gameTime);
        }

        public override void HandleInput(Vector2 position, GameTime gameTime)
        {
            HandleInput(gameTime);

            base.HandleInput(position, gameTime);
        }

        public override void Draw(Vector2 position, GameTime gameTime)
        {
            Draw(gameTime);

            base.Draw(position, gameTime);
        }
    }
}
