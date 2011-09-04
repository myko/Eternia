using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EterniaGame
{
    public class ItemGenerator
    {
        private Randomizer random;

        public ItemGenerator(Randomizer random)
        {
            this.random = random;
        }

        public Item Generate(int itemLevel)
        {
            var armorClass = random.Next<ItemArmorClasses>();
            var quality = random.Next<ItemQualities>();
            var slot = random.Next<ItemSlots>();

            var rarity = ItemRarities.Common;
            var rarityRoll = random.Next(100);
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

            var item = new Item();
            item.Level = itemLevel;
            item.Rarity = rarity;
            item.Quality = quality;
            item.Slot = slot;
            item.ArmorClass = armorClass;

            var slotName = ItemSlotHelper.ItemSlotNames[(int)slot, (int)armorClass];
            var slotModifier = ItemSlotHelper.ItemSlotModifier[(int)slot];

            Statistics baseStatistics = new Statistics();
            baseStatistics.Health = (int)(Math.Pow(2, (double)itemLevel / 10.0) * 15 * ((float)quality + 3) / 5f * (armorClass == ItemArmorClasses.Plate ? 2 : 1) * slotModifier);
            baseStatistics.ArmorRating = (int)(Math.Pow(2, (double)itemLevel / 10.0) * 15 * ((float)quality + 3) / 5f * ((int)armorClass + 1) * slotModifier);

            if (rarity > ItemRarities.Common)
            {
                var prefix = ItemAffixConstants.Affixes[random.From(ItemAffixConstants.Prefixes[(int)rarity - 1, (int)armorClass])];
                var suffix = ItemAffixConstants.Affixes[random.From(ItemAffixConstants.Suffixes[(int)rarity - 1, (int)armorClass])];

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

    public static class RandomExtensions
    {
        public static T Next<T>(this Random rnd)
        {
            var values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(rnd.Next(values.Length));
        }

        public static T From<T>(this Random random, T[] array)
        {
            return array[random.Next(array.Length)];
        }

        public static int Between(this Random random, int min, int max)
        {
            return min + random.Next(max - min);
        }

        public static float Between(this Random random, float min, float max)
        {
            return min + (float)random.NextDouble() * (max - min);
        }
    }

    public class Randomizer
    {
        private Random random = new Random();

        public virtual T Next<T>()
        {
            return random.Next<T>();
        }

        public virtual int Next()
        {
            return random.Next();
        }

        public virtual int Next(int maxValue)
        {
            return random.Next(maxValue);
        }

        public virtual T From<T>(T[] array)
        {
            return random.From<T>(array);
        }
    }
}
