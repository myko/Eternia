using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public class Precision: Stat<Precision>
    {
        public int Rating { get; set; }
        public float Chance { get { return 0.50f + Rating / (2f * Rating + 1200f); } }
        public override string Name { get { return "Precision rating"; } }
        public override Color Color { get { return Color.White; } }

        public Precision()
        {
            this.Rating = 0;
        }

        public Precision(int rating)
        {
            this.Rating = rating;
        }

        public override Precision Add(Precision s)
        {
            return new Precision(Rating + s.Rating);
        }

        public override Precision Subtract(Precision s)
        {
            return new Precision(Rating - s.Rating);
        }

        public override StatBase Negate()
        {
            return new Precision(-Rating);
        }

        public override StatBase Multiply(float f)
        {
            return new Precision((int)(Rating * f));
        }
    }
}
