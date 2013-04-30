using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eternia.Game.Stats;

namespace Eternia.Game.Items
{
    public class ItemModifier
    {
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public int Rank { get; set; }
        public Statistics Statistics { get; set; }
        public ItemSlots[] Slots { get; set; }
        public ItemArmorClasses[] ArmorClasses { get; set; }

        public ItemModifier(string prefix, string suffix, int rank, Statistics statistics, ItemSlots[] slots, ItemArmorClasses[] armorClasses)
        {
            Prefix = prefix;
            Suffix = suffix;
            Rank = rank;
            Statistics = statistics;
            Slots = slots;
            ArmorClasses = armorClasses;
        }

        public static ItemModifier[] AllModifiers = new[]
        {
            new ItemModifier("Energized", "of the Owl", 1,
            new Statistics() { new Mana(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Cloth }),

            new ItemModifier("Jagged", "of the Lion", 1,
            new Statistics() { new AttackPower(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Leather, ItemArmorClasses.Plate }),

            new ItemModifier("Arcane", "of the Lizard", 1,
            new Statistics() { new SpellPower(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather }),

            new ItemModifier("Vicious", "of the Hawk", 1,
            new Statistics() { new Hit(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Leather, ItemArmorClasses.Plate }),

            new ItemModifier("Deadly", "of the Serpent", 1,
            new Statistics() { new CriticalStrike(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather, ItemArmorClasses.Plate }),

            new ItemModifier("Sharp", "of the Thief", 1,
            new Statistics() { new CriticalStrike(1) }, 
            new[] { ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Leather }),

            new ItemModifier("Precise", "of the Spider", 1,
            new Statistics() { new Precision(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather }),

            new ItemModifier("Agile", "of the Cat", 1,
            new Statistics() { new Dodge(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Hardened", "of the Turtle", 1,
            new Statistics() { new Block(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Glimmering", "of Crystals", 2,
            new Statistics() { new Mana(1), new SpellPower(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Cloth }),

            new ItemModifier("Maiden's", "of the Maiden", 2,
            new Statistics() { new Mana(1), new CriticalStrike(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Cloth }),

            new ItemModifier("Furious", "of Fury", 2,
            new Statistics() { new AttackPower(1), new Hit(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Leather, ItemArmorClasses.Plate }),

            new ItemModifier("Berzerker", "of Berzerking", 2,
            new Statistics() { new AttackPower(1), new CriticalStrike(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Leather, ItemArmorClasses.Plate }),

            new ItemModifier("Astral", "of Stars", 2,
            new Statistics() { new SpellPower(1), new Hit(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather }),

            new ItemModifier("Charged", "of Electricity", 2,
            new Statistics() { new SpellPower(1), new CriticalStrike(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather }),

            new ItemModifier("Wicked", "of the Wicked", 2,
            new Statistics() { new Hit(1), new CriticalStrike(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Leather }),

            new ItemModifier("Fortified", "of Fortification", 2,
            new Statistics() { new AttackPower(1), new Dodge(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Brilliant", "of Brilliance", 2,
            new Statistics() { new SpellPower(1), new Precision(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather }),

            new ItemModifier("Swift", "of Swiftness", 2,
            new Statistics() { new Hit(1), new Dodge(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Perceptive", "of Perception", 2,
            new Statistics() { new Hit(1), new Precision(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather }),

            new ItemModifier("Cunning", "of Cunning", 2,
            new Statistics() { new CriticalStrike(1), new Precision(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Leather }),

            new ItemModifier("Accurate", "of Accuracy", 3,
            new Statistics() { new Hit(1), new CriticalStrike(1), new Precision(1) }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Leather }),

            //new ItemModifier("Guardian's", "of the Guardian", 5,
            //new Statistics() { new Health(1), new DamageReduction { ArmorRating = 1 }, new AttackPower(1), new Hit(1), new Dodge(1) }, 
            //new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            //new[] { ItemArmorClasses.Plate }),

            //new ItemModifier("Physician's", "of the Physician", 5,
            //new Statistics() { new Health(1), new Mana(1), new SpellPower(1), new CriticalStrike(1), new Precision(1) }, 
            //new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            //new[] { ItemArmorClasses.Cloth }),

            //new ItemModifier("Ravager's", "of the Ravager", 5,
            //new Statistics() { new Health(1), new AttackPower(1), new Hit(1), new CriticalStrike(1), new Precision(1) }, 
            //new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            //new[] { ItemArmorClasses.Leather }),

            //new ItemModifier("Mages'", "of the Magi", 5,
            //new Statistics() { new Health(1), new SpellPower(1), new Hit(1), new CriticalStrike(1), new Precision(1) }, 
            //new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            //new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather }),
        };
    }
}
