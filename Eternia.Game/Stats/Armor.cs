using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eternia.Game.Stats
{
    public  class Armor: RatingStat<Armor>
    {
        public override float Chance { get { return 0.05f + Rating / (3f * Rating + 1000f); } }
        public override string Name { get { return "Armor rating"; } }
        
        public Armor()
        {
            this.Rating = 0;
        }

        public Armor(int rating)
        {
            this.Rating = rating;
        }

        public override Armor Add(Armor s)
        {
            return new Armor(Rating + s.Rating);
        }

        public override Armor Subtract(Armor s)
        {
            return new Armor(Rating - s.Rating);
        }

        public override StatBase Negate()
        {
            return new Armor(-Rating);
        }

        public override StatBase Multiply(float f)
        {
            return new Armor((int)(Rating * f));
        }

        public override void SetItemValue(int level, Items.ItemArmorClasses armorClass, float itemSlotModifier)
        {
            var armorClassArmorMultipler = (float)Math.Pow(2, (int)armorClass);
            Rating = (int)(Items.ItemGenerator.GetItemLevelMultiplier(level) * armorClassArmorMultipler * itemSlotModifier * 25f);
        }
    }
}
