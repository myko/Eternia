using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eternia.Game.Stats;
using System.Collections;

namespace Eternia.Game.Items
{
    public class ItemDefinition
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public ItemRarities Rarity { get; set; }
        public ItemQualities Quality { get; set; }
        public ItemSlots Slot { get; set; }
        public ItemArmorClasses ArmorClass { get; set; }
        public StatDefinitionList Statistics { get; set; }

        public ItemDefinition()
        {
            Statistics = new StatDefinitionList();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
