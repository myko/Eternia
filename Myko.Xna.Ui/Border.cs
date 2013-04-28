using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Myko.Xna.Ui
{
    public class Border: ContentControl
    {
        public int BorderSize { get; set; }
        public Color BorderColor { get; set; }
        public Texture2D BorderTexture { get; set; }
        public Vector2 Padding { get; set; }

        public Border()
        {
            Background = Color.Black;
            Foreground = Color.White;
            BorderSize = 2;
            BorderColor = Color.White;
        }

        public Border(object content)
            : this()
        {
            Content = content;
        }

        public override void HandleInput(Vector2 position, GameTime gameTime)
        {
            base.HandleInput(position + new Vector2(BorderSize, BorderSize), gameTime);
        }

        public override void Draw(Vector2 position, GameTime gameTime)
        {
            if (BorderTexture != null)
            {
                int bx1 = (int)position.X;
                int bx2 = (int)position.X + BorderSize;
                int bx3 = (int)(position.X + Width) - BorderSize;

                int by1 = (int)position.Y;
                int by2 = (int)position.Y + BorderSize;
                int by3 = (int)(position.Y + Height) - BorderSize;

                int bw = (int)Width - BorderSize * 2;
                int bh = (int)Height - BorderSize * 2;

                // Background
                var bounds = new Rectangle(bx1 + BorderSize / 2, by1 + BorderSize / 2, (int)Width - BorderSize, (int)Height - BorderSize);
                if (BackgroundTexture != null)
                    SpriteBatch.Draw(BackgroundTexture, bounds, Background, ZIndex);
                else
                    SpriteBatch.Draw(BlankTexture, bounds, Background, ZIndex);

                // Corners
                SpriteBatch.Draw(BorderTexture, new Rectangle(bx1, by1, BorderSize, BorderSize), new Rectangle(0, 0, 256, 256), BorderColor, ZIndex + 0.02f);
                SpriteBatch.Draw(BorderTexture, new Rectangle(bx3, by1, BorderSize, BorderSize), new Rectangle(256, 0, 256, 256), BorderColor, ZIndex + 0.02f);
                SpriteBatch.Draw(BorderTexture, new Rectangle(bx1, by3, BorderSize, BorderSize), new Rectangle(0, 256, 256, 256), BorderColor, ZIndex + 0.02f);
                SpriteBatch.Draw(BorderTexture, new Rectangle(bx3, by3, BorderSize, BorderSize), new Rectangle(256, 256, 256, 256), BorderColor, ZIndex + 0.02f);

                // Left and right
                SpriteBatch.Draw(BorderTexture, new Rectangle(bx1, by2, BorderSize, bh), new Rectangle(512, 0, 256, 256), BorderColor, ZIndex + 0.02f);
                SpriteBatch.Draw(BorderTexture, new Rectangle(bx3, by2, BorderSize, bh), new Rectangle(512, 0, 256, 256), BorderColor, ZIndex + 0.02f);

                // Top and bottom
                SpriteBatch.Draw(BorderTexture, new Rectangle(bx2, by1, bw, BorderSize), new Rectangle(512, 256, 256, 256), BorderColor, ZIndex + 0.02f);
                SpriteBatch.Draw(BorderTexture, new Rectangle(bx2, by3, bw, BorderSize), new Rectangle(512, 256, 256, 256), BorderColor, ZIndex + 0.02f);
            }
            else
            {
                var bounds = new Rectangle((int)position.X, (int)position.Y, (int)Width, (int)Height);
                SpriteBatch.Draw(BlankTexture, bounds, BorderColor, ZIndex);

                var innerBounds = bounds;
                innerBounds.Inflate(-BorderSize, -BorderSize);
                SpriteBatch.Draw(BlankTexture, innerBounds, Background, ZIndex + 0.001f);
            }

            base.Draw(position + new Vector2(BorderSize, BorderSize) + Padding, gameTime);
        }
    }
}
