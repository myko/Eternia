using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EterniaGame
{
    public class Item
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public ItemRarities Rarity { get; set; }
        public ItemQualities Quality { get; set; }
        public ItemSlots Slot { get; set; }
        public ItemArmorClasses ArmorClass { get; set; }
        public Statistics Statistics { get; set; }

        public Item()
        {
            Statistics = new Statistics();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
