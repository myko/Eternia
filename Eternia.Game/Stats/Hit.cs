using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public class Hit: RatingStat<Hit>
    {
        public override float Chance { get { return 0.075f + Rating / (2f * Rating + 1000f); } }
        public override string Name { get { return "Hit rating"; } }

        public Hit()
        {
            this.Rating = 0;
        }

        public Hit(int rating)
        {
            this.Rating = rating;
        }

        public override Hit Add(Hit s)
        {
            return new Hit(Rating + s.Rating);
        }

        public override Hit Subtract(Hit s)
        {
            return new Hit(Rating - s.Rating);
        }

        public override StatBase Negate()
        {
            return new Hit(-Rating);
        }

        public override StatBase Multiply(float f)
        {
            return new Hit((int)(Rating * f));
        }
    }
}
