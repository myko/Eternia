using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            if (rarityRoll < 40)
                rarity = ItemRarities.Rare;
            if (rarityRoll < 20)
                rarity = ItemRarities.Heroic;
            if (rarityRoll < 10)
                rarity = ItemRarities.Epic;
            if (rarityRoll < 5)
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
            baseStatistics.Health = (int)(Math.Pow(2, (double)itemLevel / 10.0) * 15 * ((float)quality + 3) / 5f * (armorClass == ItemArmorClasses.Plate ? 2 : 1) * slotModifier);
            baseStatistics.ArmorRating = (int)(Math.Pow(2, (double)itemLevel / 10.0) * 5 * armorMultiplier * slotModifier);

            if (rarity > ItemRarities.Common)
            {
                var prefix = ItemAffixConstants.Affixes[randomizer.From(ItemAffixConstants.Prefixes[(int)rarity - 1, (int)armorClass])];
                var suffix = ItemAffixConstants.Affixes[randomizer.From(ItemAffixConstants.Suffixes[(int)rarity - 1, (int)armorClass])];

                prefix.Statistics.ArmorRating = (int)(prefix.Statistics.ArmorRating * armorMultiplier * 0.25f);
                suffix.Statistics.ArmorRating = (int)(suffix.Statistics.ArmorRating * armorMultiplier * 0.25f);

                item.Name = string.Format("{0}{1}{2}", prefix.Name, slotName, suffix.Name);
                item.Statistics = baseStatistics + (prefix.Statistics + suffix.Statistics) * (int)(Math.Pow(2, (double)itemLevel / 10.0) * 5 * slotModifier);
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
