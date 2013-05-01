using System;
using Eternia.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;
using Eternia.Game.Abilities;
using Eternia.Game.Actors;

namespace EterniaXna
{
    public class OrderButton: IControlContent
    {
        public Actor Actor { get; set; }
        public Order Order { get; set; }

        private ContentControl container;
        private Texture2D texture;
        private Texture2D blankTexture;
        private Texture2D targetTexture;
        private SpriteFont font;

        public OrderButton(ContentControl container, Actor actor, Order order, Texture2D texture, Texture2D blankTexture, Texture2D targetTexture, SpriteFont font)
        {
            this.container = container;
            this.Actor = actor;
            this.Order = order;
            this.texture = texture;
            this.blankTexture = blankTexture;
            this.targetTexture = targetTexture;
            this.font = font;
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
            var targetBounds = new Rectangle((int)position.X + 30, (int)position.Y, (int)container.Width, (int)container.Height);
            //if (Order == Actor.CurrentOrder)
            //    container.SpriteBatch.Draw(texture, bounds, Color.Yellow, container.ZIndex + 0.001f);
            //else
                container.SpriteBatch.Draw(texture, bounds, Color.White, container.ZIndex + 0.001f);
                container.SpriteBatch.Draw(targetTexture, targetBounds, Color.White, container.ZIndex + 0.002f);

            // Draw out of range

            if (!Order.Ability.Cooldown.IsReady)
            {
                var cooldown = ((int)Order.Ability.Cooldown.Current).ToString();
                var textSize = font.MeasureString(cooldown);
                var textPosition = new Vector2((int)(container.Width / 2 - textSize.X / 2), (int)(container.Height / 2 - textSize.Y / 2));

                container.SpriteBatch.DrawString(font, cooldown, position + textPosition + new Vector2(-1, -1), Color.Black, container.ZIndex + 0.003f);
                container.SpriteBatch.DrawString(font, cooldown, position + textPosition + new Vector2(1, -1), Color.Black, container.ZIndex + 0.003f);
                container.SpriteBatch.DrawString(font, cooldown, position + textPosition + new Vector2(-1, 1), Color.Black, container.ZIndex + 0.003f);
                container.SpriteBatch.DrawString(font, cooldown, position + textPosition + new Vector2(1, 1), Color.Black, container.ZIndex + 0.003f);
                container.SpriteBatch.DrawString(font, cooldown, position + textPosition, Color.Yellow, container.ZIndex + 0.004f);
            }
        }
    }
}
