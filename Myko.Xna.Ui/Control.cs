using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Myko.Xna.Ui
{
    public class Control
    {
        private ContentControl tooltip;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Texture2D blankTexture;
        private MouseState mouseState;

        public Color Foreground { get; set; }
        public Color Background { get; set; }
        public Texture2D BackgroundTexture { get; set; }
        public Texture2D MouseOverTexture { get; set; }
        public Control Parent { get; set; }
        public Vector2 Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public bool IsMouseOver { get; set; }
        public bool IsMouseDown { get; set; }
        public bool IsMouseUp { get; set; }
        public float ZIndex { get; set; }

        public SpriteBatch SpriteBatch 
        {
            get
            {
                if (spriteBatch != null)
                    return spriteBatch;

                if (Parent == null)
                    return null;

                return Parent.SpriteBatch;
            }
            set
            {
                spriteBatch = value;
            }
        }

        public SpriteFont Font
        {
            get
            {
                if (font != null)
                    return font;

                if (Parent == null)
                    return null;

                return Parent.Font;
            }
            set
            {
                font = value;
            }
        }

        protected Texture2D BlankTexture
        {
            get
            {
                if (blankTexture == null)
                {
                    if (Parent != null)
                        blankTexture = Parent.BlankTexture;
                    else
                    {
                        blankTexture = new Texture2D(SpriteBatch.GraphicsDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color);
                        blankTexture.SetData(new Color[] { Color.White });
                    }
                }

                return blankTexture;
            }
        }

        public object Tooltip
        {
            get
            {
                if (tooltip == null)
                    return null;

                return tooltip.Content;
            }
            set
            {
                if (tooltip == null)
                {
                    tooltip = new ContentControl();
                    tooltip.Parent = this;
                    tooltip.ZIndex = ZIndex + 0.01f;
                }

                tooltip.Content = value;
            }
        }

        public Control()
        {
            ZIndex = 0;
            Foreground = Color.White;
            Background = Color.Black;
        }

        public Binding<T> Bind<T>(Func<T> f)
        {
            return new Binding<T>(f);
        }

        public virtual void HandleInput(Vector2 position, GameTime gameTime)
        {
            var bounds = new Rectangle((int)position.X, (int)position.Y, (int)Width, (int)Height);

            mouseState = Mouse.GetState();

            IsMouseOver = bounds.Contains(mouseState.X, mouseState.Y);
            IsMouseDown = IsMouseOver && mouseState.LeftButton == ButtonState.Pressed;
            IsMouseUp = mouseState.LeftButton == ButtonState.Released;

            if (tooltip != null)
                tooltip.HandleInput(position, gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (tooltip != null)
            {
                tooltip.Update(gameTime);
            }
        }

        protected void DrawBackground(Vector2 position)
        {
            var bounds = new Rectangle((int)position.X, (int)position.Y, (int)Width, (int)Height);

            if (BackgroundTexture != null)
                SpriteBatch.Draw(BackgroundTexture, bounds, Background, ZIndex);
            else
                SpriteBatch.Draw(BlankTexture, bounds, Background, ZIndex);

            if (MouseOverTexture != null && IsMouseOver)
                SpriteBatch.Draw(MouseOverTexture, bounds, Color.White, ZIndex + 0.01f);
        }

        public virtual void Draw(Vector2 position, GameTime gameTime)
        {
            if (tooltip != null && IsMouseOver)
            {
                tooltip.Font = Font;
                tooltip.SpriteBatch = SpriteBatch;
                tooltip.Draw(new Vector2(mouseState.X, mouseState.Y), gameTime);
            }
        }
    }
}
