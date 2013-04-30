using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public class Block: RatingStat<Block>
    {
        public override float Chance { get { return 0.10f + Rating / (1.5f * Rating + 1000f); } }
        public override string Name { get { return "Block rating"; } }
        
        public Block()
        {
        }

        public Block(int rating)
            : base(rating)
        {
        }

        public override Block Add(Block s)
        {
            return new Block(Rating + s.Rating);
        }

        public override Block Subtract(Block s)
        {
            return new Block(Rating - s.Rating);
        }

        public override StatBase Negate()
        {
            return new Block(-Rating);
        }

        public override StatBase Multiply(float f)
        {
            return new Block((int)(Rating * f));
        }
    }
}
