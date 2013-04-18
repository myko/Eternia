using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eternia.Game.Stats;

namespace EterniaGame
{
    public class ItemGenerator
    {
        private Randomizer randomizer;

        public ItemGenerator(Randomizer randomizer)
        {
            this.randomizer = randomizer;
        }

        public Item Generate(int itemLevel)
        {
            var slot = randomizer.Next<ItemSlots>();

            var rarity = ItemRarities.Common;
            var rarityRoll = randomizer.Next(100);
            if (rarityRoll < 60)
                rarity = ItemRarities.Uncommon;
            if (rarityRoll < 35)
                rarity = ItemRarities.Rare;
            if (rarityRoll < 15)
                rarity = ItemRarities.Heroic;
            if (rarityRoll < 8)
                rarity = ItemRarities.Epic;
            if (rarityRoll < 2)
                rarity = ItemRarities.Legendary;

            return Generate(itemLevel, slot, rarity);
        }

        public Item Generate(int itemLevel, ItemSlots slot, ItemRarities rarity)
        {
            var armorClass = randomizer.Next<ItemArmorClasses>();
            var quality = randomizer.Next<ItemQualities>();

            var item = new Item();
            item.Level = itemLevel;
            item.Rarity = rarity;
            item.Quality = quality;
            item.Slot = slot;
            item.ArmorClass = armorClass;

            var slotName = ItemSlotHelper.ItemSlotNames[(int)slot, (int)armorClass];
            var slotModifier = ItemSlotHelper.ItemSlotModifier[(int)slot];

            var armorMultiplier = 3 * ((float)quality + 3) / 5f * ((int)armorClass + 1);

            var baseStatistics = new Statistics();
            baseStatistics.Add(new Health((int)(Math.Pow(2, (double)itemLevel / 10.0) * 15 * ((float)quality + 3) / 5f * (armorClass == ItemArmorClasses.Plate ? 2 : 1) * slotModifier)));
            baseStatistics.Add(new DamageReduction { ArmorRating = (int)(Math.Pow(2, (double)itemLevel / 10.0) * 5 * armorMultiplier * slotModifier) });
            item.Statistics = baseStatistics;

            if (rarity > ItemRarities.Common)
            {
                string prefix = string.Empty;
                string suffix = string.Empty;
                for (int i = (int)rarity; i > 0;)
                {
                    var modifier = randomizer.From(ItemModifier.AllModifiers.Where(m => m.Rank <= i && (m.Slots == null || m.Slots.Contains(slot)) && (m.ArmorClasses == null || m.ArmorClasses.Contains(armorClass))).ToArray());
                    //modifier.Statistics.Add(new DamageReduction.ArmorRating = (int)(modifier.Statistics.DamageReduction.ArmorRating * armorMultiplier * 0.25f);

                    if (string.IsNullOrEmpty(prefix))
                        prefix = modifier.Prefix;
                    else if (string.IsNullOrEmpty(suffix))
                        suffix = modifier.Suffix;

                    item.Statistics = item.Statistics + modifier.Statistics * (int)(Math.Pow(2, (double)itemLevel / 10.0) * 5 * slotModifier);

                    i -= modifier.Rank;
                }

                item.Name = string.Format("{0} {1} {2}", prefix, slotName, suffix);
            }
            else
            {
                item.Name = quality.ToString() + " " + slotName;
                item.Statistics = baseStatistics;
            }

            return item;
        }
    }
}
