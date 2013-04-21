using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public class Dodge: RatingStat<Dodge>
    {
        public override float Chance { get { return 0.05f + Rating / (3f * Rating + 1000f); } }
        public override string Name { get { return "Dodge rating"; } }
        
        public Dodge()
        {
            this.Rating = 0;
        }

        public Dodge(int rating)
        {
            this.Rating = rating;
        }

        public override Dodge Add(Dodge s)
        {
            return new Dodge(Rating + s.Rating);
        }

        public override Dodge Subtract(Dodge s)
        {
            return new Dodge(Rating - s.Rating);
        }

        public override StatBase Negate()
        {
            return new Dodge(-Rating);
        }

        public override StatBase Multiply(float f)
        {
            return new Dodge((int)(Rating * f));
        }
    }
}
