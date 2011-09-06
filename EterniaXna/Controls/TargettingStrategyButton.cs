using System;
using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;

namespace EterniaXna
{
    public class TargettingStrategyButton: IControlContent
    {
        public TargettingStrategies TargettingStrategy { get; set; }
        public Actor Actor { get; set; }

        private ContentControl container;
        private Texture2D texture;

        public TargettingStrategyButton(ContentControl container, Actor actor, TargettingStrategies strategy, Texture2D texture)
        {
            this.container = container;
            this.Actor = actor;
            this.TargettingStrategy = strategy;
            this.texture = texture;
        }

        public void HandleInput(Vector2 position, GameTime gameTime)
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(Vector2 position, GameTime gameTime)
        {
            var bounds = new Rectangle((int)position.X, (int)position.Y, (int)container.Width, (int)container.Height);
            if (TargettingStrategy == Actor.TargettingStrategy)
                container.SpriteBatch.Draw(texture, bounds, Color.Yellow, container.ZIndex + 0.001f);
            else
                container.SpriteBatch.Draw(texture, bounds, Color.White, container.ZIndex + 0.001f);
        }
    }
}
