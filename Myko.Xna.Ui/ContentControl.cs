using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Myko.Xna.Ui
{
    public interface IControlContent
    {
        void HandleInput(Vector2 position, GameTime gameTime);
        void Update(GameTime gameTime);
        void Draw(Vector2 position, GameTime gameTime);
    }

    public class ContentControl: Control
    {
        private class DefaultControlContent: IControlContent
        {
            private object value;
            private ContentControl container;

            public DefaultControlContent(ContentControl container, object value)
            {
                this.value = value;
                this.container = container;
            }

            public void HandleInput(Vector2 position, GameTime gameTime)
            {
            }

            public void Update(GameTime gameTime)
            {
            }

            public void Draw(Vector2 position, GameTime gameTime)
            {
                var text = value.ToString();
                var textSize = container.Font.MeasureString(text);
                var textPosition = position + new Vector2(container.ActualWidth, container.ActualHeight) / 2 - textSize / 2;
                container.SpriteBatch.DrawString(container.Font, text, new Vector2((int)textPosition.X, (int)textPosition.Y), container.Foreground, ZIndex);
            }

            public float ZIndex
            {
                get { return container.ZIndex + 0.01f; }
            }
        }

        private class Texture2DControlContent : IControlContent
        {
            private Texture2D texture;
            private ContentControl container;

            public Texture2DControlContent(ContentControl container, Texture2D texture)
            {
                this.container = container;
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
                container.SpriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)container.ActualWidth, (int)container.ActualHeight), Color.White, ZIndex);
            }

            public float ZIndex
            {
                get { return container.ZIndex + 0.01f; }
            }
        }

        private class ControlControlContent : IControlContent
        {
            private readonly Control control;
            private readonly ContentControl container;
            
            public ControlControlContent(ContentControl container, Control control)
            {
                this.container = container;
                this.control = control;

                control.Parent = container;
            }

            public void HandleInput(Vector2 position, GameTime gameTime)
            {
                control.ZIndex = container.ZIndex + 0.01f;
                control.HandleInput(position + control.Position, gameTime);
            }

            public void Update(GameTime gameTime)
            {
                control.ZIndex = container.ZIndex + 0.01f;
                control.Update(gameTime);
            }

            public void Draw(Vector2 position, GameTime gameTime)
            {
                control.ZIndex = container.ZIndex + 0.01f;
                control.Draw(position + control.Position, gameTime);
            }
        }

        private IControlContent content;
        public object Content
        {
            get
            {
                return content;
            }
            set
            {

                if (value == null)
                    content = null;
                else if (value is IControlContent)
                    content = (IControlContent)value;
                else if (value is Control)
                    content = new ControlControlContent(this, (Control)value);
                else if (value is Texture2D)
                    content = new Texture2DControlContent(this, (Texture2D)value);
                else
                    content = new DefaultControlContent(this, value);
            }
        }

        public override void HandleInput(Vector2 position, GameTime gameTime)
        {
            if (content != null)
                content.HandleInput(position, gameTime);

            base.HandleInput(position, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (content != null)
                content.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(Vector2 position, GameTime gameTime)
        {
            if (content != null)
                content.Draw(position, gameTime);

            base.Draw(position, gameTime);
        }
    }
}
