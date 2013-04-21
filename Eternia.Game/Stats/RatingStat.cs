using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public abstract class RatingStat<StatT>: Stat<StatT> where StatT: StatBase, new()
    {
        public int Rating { get; set; }
        public abstract float Chance { get; }
        public override Color Color { get { return Rating < 0 ? Color.Salmon : Color.LightGreen; } }

        public RatingStat()
        {
            this.Rating = 0;
        }

        public RatingStat(int rating)
        {
            this.Rating = rating;
        }

        public override string ToValueString()
        {
            return Rating + " (" + Chance.ToString("P") + ")";
        }

        public override string ToItemTooltipString()
        {
            if (Rating >= 0)
                return "+" + Rating.ToString() + " " + Name;
            else
                return Rating.ToString() + " " + Name;
        }

        public override string ToItemUpgradeString()
        {
            if (Rating >= 0)
                return Name + ": " + "+" + Rating.ToString();
            else
                return Name + ": " + Rating.ToString();
        }
    }
}
