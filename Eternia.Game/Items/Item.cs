using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eternia.Game.Stats;

namespace Eternia.Game.Items
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

        public Item(ItemDefinition itemDefinition)
            : this()
        {
            Name = itemDefinition.Name;
            Level = itemDefinition.Level;
            Rarity = itemDefinition.Rarity;
            Quality = itemDefinition.Quality;
            Slot = itemDefinition.Slot;
            ArmorClass = itemDefinition.ArmorClass;

            foreach (var statDefinition in itemDefinition.Statistics)
            {
                var stat = (StatBase)Activator.CreateInstance(statDefinition.StatType);
                stat.SetItemValue(itemDefinition.Level, itemDefinition.ArmorClass, ItemSlotHelper.ItemSlotModifier[(int)itemDefinition.Slot]);
                Statistics.Add(stat);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
