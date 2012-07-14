using System;
using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;
using EterniaGame.Actors;

namespace EterniaXna
{
    public class TargetingStrategyButton: IControlContent
    {
        public TargetingStrategy TargetingStrategy { get; set; }
        public Actor Actor { get; set; }

        private ContentControl container;
        private Texture2D texture;

        public TargetingStrategyButton(ContentControl container, Actor actor, TargetingStrategy strategy, Texture2D texture)
        {
            this.container = container;
            this.Actor = actor;
            this.TargetingStrategy = strategy;
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
            if (TargetingStrategy.Value == Actor.TargettingStrategy)
                container.SpriteBatch.Draw(texture, bounds, Color.Yellow, container.ZIndex + 0.001f);
            else
                container.SpriteBatch.Draw(texture, bounds, Color.White, container.ZIndex + 0.001f);
        }
    }
}
