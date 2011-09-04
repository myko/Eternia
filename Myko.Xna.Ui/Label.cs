using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myko.Xna.Ui
{
    public class Label: Control
    {
        public Binding<string> Text { get; set; }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var textSize = Font.MeasureString(Text);
            Width = textSize.X;
            Height = textSize.Y;

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(position, gameTime);

            SpriteBatch.DrawString(Font, Text, position, Foreground, ZIndex + 0.01f);
        }
    }
}
