using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;

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

        public static Vector3 Project(this Vector2 v, float y)
        {
            return new Vector3(v.X, y, v.Y);
        }

        public static T SafeLoad<T>(this ContentManager contentManager, string fileName)
        {
            if (!System.IO.File.Exists(System.IO.Path.Combine(contentManager.RootDirectory, fileName + ".xnb")))
                return default(T);

            try
            {
                return contentManager.Load<T>(fileName);
            }
            catch (ContentLoadException)
            {
                return default(T);
            }
        }

        public static T SafeLoad<T>(this ContentManager contentManager, string fileName, string defaultFileName)
        {
            if (!System.IO.File.Exists(System.IO.Path.Combine(contentManager.RootDirectory, fileName + ".xnb")))
                return contentManager.Load<T>(defaultFileName);

            try
            {
                return contentManager.Load<T>(fileName);
            }
            catch (ContentLoadException)
            {
                return contentManager.Load<T>(defaultFileName);
            }
        }

        public static T SafeLoad<T>(this ContentManager contentManager, string fileName, T defaultContent)
        {
            if (!System.IO.File.Exists(System.IO.Path.Combine(contentManager.RootDirectory, fileName + ".xnb")))
                return defaultContent;

            try
            {
                return contentManager.Load<T>(fileName);
            }
            catch (ContentLoadException)
            {
                return defaultContent;
            }
        }
    }
}
