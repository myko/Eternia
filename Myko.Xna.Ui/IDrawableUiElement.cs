using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Myko.Xna.Ui
{
    public interface IDrawableUiElement
    {
        int ZIndex { get; }

        void Draw(GameTime gameTime);
    }
}
