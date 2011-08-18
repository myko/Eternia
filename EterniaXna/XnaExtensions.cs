using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EterniaXna
{
    public static class XnaExtensions
    {
        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Rectangle destinationRectangle, Color color, float layerDepth)
        {
            spriteBatch.Draw(texture, destinationRectangle, null, color, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float layerDepth)
        {
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float angle, float layerDepth)
        {
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, angle, Vector2.Zero, SpriteEffects.None, layerDepth);
        }

        public static void DrawString(this SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Vector2 position, Color color, float layerDepth)
        {
            spriteBatch.DrawString(spriteFont, text, position, color, 0, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
