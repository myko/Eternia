using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public class CriticalStrike: Stat<CriticalStrike>
    {
        public int Rating { get; set; }
        public float Chance { get { return 0.075f + Rating / (2f * Rating + 1000f); } }
        public override string Name { get { return "Critical strike rating"; } }
        public override Color Color { get { return Color.White; } }

        public CriticalStrike()
        {
            this.Rating = 0;
        }

        public CriticalStrike(int rating)
        {
            this.Rating = rating;
        }

        public override CriticalStrike Add(CriticalStrike s)
        {
            return new CriticalStrike(Rating + s.Rating);
        }

        public override CriticalStrike Subtract(CriticalStrike s)
        {
            return new CriticalStrike(Rating - s.Rating);
        }

        public override StatBase Negate()
        {
            return new CriticalStrike(-Rating);
        }

        public override StatBase Multiply(float f)
        {
            return new CriticalStrike((int)(Rating * f));
        }
    }
}
