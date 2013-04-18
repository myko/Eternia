using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public class Dodge: Stat<Dodge>
    {
        public int Rating { get; set; }
        public float Chance { get { return 0.075f + Rating / (2f * Rating + 1000f); } }
        public override string Name { get { return "Dodge rating"; } }
        public override Color Color { get { return Color.White; } }

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
