using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Eternia.Game
{
    public class GraphicsEffectDefinition
    {
        public Vector2 Position { get; set; }
        public float Scale { get; set; }

        public GraphicsEffectDefinition()
        {
            Scale = 1f;
        }
    }
}
