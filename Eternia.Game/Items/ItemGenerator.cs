using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eternia.Game.Stats;

namespace Eternia.Game.Items
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

            var rarity = ItemRarities.Rare;
            var rarityRoll = randomizer.Next(100);

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
            
            var item = new Item();
            item.Level = itemLevel;
            item.Rarity = rarity;
            item.Quality = ItemQualities.Superior;
            item.Slot = slot;
            item.ArmorClass = armorClass;

            var slotName = ItemSlotHelper.ItemSlotNames[(int)slot, (int)armorClass];
            var slotModifier = ItemSlotHelper.ItemSlotModifier[(int)slot];

            var itemLevelMultiplier = (float)(Math.Pow(2, (double)itemLevel / 10.0)) * 0.5f;
            var armorClassHealthMultiplier = (float)(armorClass == ItemArmorClasses.Plate ? 4 : 1);
            var armorClassArmorMultipler = (float)Math.Pow(2, (int)armorClass);

            var baseStatistics = new Statistics();
            baseStatistics.Add(new Health(itemLevelMultiplier * armorClassHealthMultiplier * slotModifier * 25));
            baseStatistics.Add(new DamageReduction { ArmorRating = (int)(itemLevelMultiplier * armorClassArmorMultipler * slotModifier * 25)});

            if (rarity > ItemRarities.Common)
            {
                var prefix = string.Empty;
                var suffix = string.Empty;
                var statistics = new Statistics();

                for (int i = (int)rarity + 1; i > 0;)
                {
                    var modifier = randomizer.From(ItemModifier.AllModifiers.Where(m => m.Rank <= i && m.Slots.Contains(slot) && m.ArmorClasses.Contains(armorClass)).ToArray());

                    if (string.IsNullOrEmpty(prefix))
                        prefix = modifier.Prefix;
                    else if (string.IsNullOrEmpty(suffix))
                        suffix = modifier.Suffix;

                    statistics = statistics + modifier.Statistics * itemLevelMultiplier * slotModifier * 6.25f;

                    i -= modifier.Rank;
                }

                item.Statistics = baseStatistics + statistics;
                item.Name = string.Format("{0} {1} {2}", prefix, slotName, suffix);
            }
            else
            {
                var quality = randomizer.Next<ItemQualities>();
                item.Name = quality.ToString() + " " + slotName;
                item.Statistics = new Statistics();
                item.Quality = quality;
            }

            return item;
        }
    }
}
