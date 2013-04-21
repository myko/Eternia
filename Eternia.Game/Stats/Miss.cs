using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public class Miss : RatingStat<Miss>
    {
        public override float Chance { get { return 0.075f + Rating / (2f * Rating + 1000f); } }
        public override string Name { get { return "Miss rating"; } }

        public Miss()
        {
            this.Rating = 0;
        }

        public Miss(int rating)
        {
            this.Rating = rating;
        }

        public override Miss Add(Miss s)
        {
            return new Miss(Rating + s.Rating);
        }

        public override Miss Subtract(Miss s)
        {
            return new Miss(Rating - s.Rating);
        }

        public override StatBase Negate()
        {
            return new Miss(-Rating);
        }

        public override StatBase Multiply(float f)
        {
            return new Miss((int)(Rating * f));
        }
    }
}
