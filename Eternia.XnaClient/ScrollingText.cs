using System;
using EterniaGame.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.XnaClient
{
    public class ScrollingText: SceneNode
    {
        private readonly SpriteBatch spriteBatch;

        public Actor Source { get; set; }
        public Actor Target { get; set; }
        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public float Alpha { get; set; }
        public SpriteFont Font { get; set; }
        public Color Color { get; set; }
        public float Speed { get; set; }
        public float Life { get; set; }

        public ScrollingText(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;

            Life = 2;
        }

        public override bool IsExpired()
        {
            return Life <= 0f;
        }

        public override void Update(GameTime gameTime, bool isPaused)
        {
            Life -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position = Position + new Vector2(0, (float)-gameTime.ElapsedGameTime.TotalSeconds * Speed);
            Alpha = Math.Min(1, Life);

            base.Update(gameTime, isPaused);
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            //if (selectedActors.Contains(st.Source) || selectedActors.Contains(st.Target))
            //spriteBatch.DrawString(Font, Text, Position, new Color(Color, Alpha));
            spriteBatch.DrawString(Font, Text, Position, new Color(Color.R, Color.G, Color.B, Alpha));

            base.Draw(view, projection);
        }
    }
}
