using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Myko.Xna.Ui
{
    public class TextBox: Control
    {
        Keys[] keysPressedLastFrame;

        public string Text { get; set; }

        public TextBox()
        {
            Text = string.Empty;
        }

        public override void HandleInput(Vector2 position, GameTime gameTime)
        {
            //if (!Microsoft.Xna.Framework.GamerServices.Guide.IsVisible)
            //    Microsoft.Xna.Framework.GamerServices.Guide.BeginShowKeyboardInput(PlayerIndex.One, "Test", "Test Keyboard", "Hej", asyncResult => Text = Microsoft.Xna.Framework.GamerServices.Guide.EndShowKeyboardInput(asyncResult), null);

            //var state = Keyboard.GetState();

            //var keysPressedThisFrame = state.GetPressedKeys();

            //if (keysPressedLastFrame != null)
            //{
            //    foreach (var key in keysPressedLastFrame)
            //    {
            //        if (!keysPressedThisFrame.Contains(key))
            //            Text += key.ToString();
            //    }
            //}

            //keysPressedLastFrame = keysPressedThisFrame;

            base.HandleInput(position, gameTime);
        }

        public override void Draw(Vector2 position, GameTime gameTime)
        {
            SpriteBatch.DrawString(Font, Text, position, Color.White, ZIndex);
        }
    }
}
