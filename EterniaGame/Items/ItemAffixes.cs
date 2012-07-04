using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EterniaGame
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
            new ItemModifier("Vigorous", "of Vigor", 1,
            new Statistics() { Health = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather, ItemArmorClasses.Plate }),

            new ItemModifier("Energized", "of the Owl", 1,
            new Statistics() { Mana = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Cloth }),

            new ItemModifier("Sturdy", "of the Bear", 1,
            new Statistics() { DamageReduction = { ArmorRating = 1 } }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Fireproof", "of the Firefly", 1,
            new Statistics() { DamageReduction = { FireResistanceRating = 1 } }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Insulated", "of the Arctic Bear", 1,
            new Statistics() { DamageReduction = { FrostResistanceRating = 1 } }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Attuned", "of the Golem", 1,
            new Statistics() { DamageReduction = { ArcaneResistanceRating = 1 } }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Immunized", "of the Toad", 1,
            new Statistics() { DamageReduction = { NatureResistanceRating = 1 } }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Doubting", "of the Imp", 1,
            new Statistics() { DamageReduction = { HolyResistanceRating = 1 } }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Blessed", "of the Cherub", 1,
            new Statistics() { DamageReduction = { UnholyResistanceRating = 1 } }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Jagged", "of the Lion", 1,
            new Statistics() { AttackPower = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Leather, ItemArmorClasses.Plate }),

            new ItemModifier("Arcane", "of the Lizard", 1,
            new Statistics() { SpellPower = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather }),

            new ItemModifier("Vicious", "of the Hawk", 1,
            new Statistics() { HitRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Leather, ItemArmorClasses.Plate }),

            new ItemModifier("Deadly", "of the Serpent", 1,
            new Statistics() { CritRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather, ItemArmorClasses.Plate }),

            new ItemModifier("Sharp", "of the Thief", 1,
            new Statistics() { CritRating = 1 }, 
            new[] { ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Leather }),

            new ItemModifier("Precise", "of the Spider", 1,
            new Statistics() { PrecisionRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather }),

            new ItemModifier("Agile", "of the Cat", 1,
            new Statistics() { DodgeRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Lucky", "of the Rabbit", 1,
            new Statistics() { ExtraRewards = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather, ItemArmorClasses.Plate }),

            new ItemModifier("Vitalized", "of Vitality", 2,
            new Statistics() { Health = 1, Mana = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Cloth }),

            new ItemModifier("Unyielding", "of Defense", 2,
            new Statistics() { Health = 1, DamageReduction = { ArmorRating = 1 } }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Mighty", "of Might", 2,
            new Statistics() { Health = 1, AttackPower = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Leather, ItemArmorClasses.Plate }),

            new ItemModifier("Wizard's", "of the Wizard", 2,
            new Statistics() { Health = 1, SpellPower = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather }),

            new ItemModifier("Archer's", "of the Archer", 2,
            new Statistics() { Health = 1, HitRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Leather }),

            new ItemModifier("Veteran's", "of the Veteran", 2,
            new Statistics() { Health = 1, CritRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Leather }),

            new ItemModifier("Glimmering", "of Crystals", 2,
            new Statistics() { Mana = 1, SpellPower = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Cloth }),

            new ItemModifier("Maiden's", "of the Maiden", 2,
            new Statistics() { Mana = 1, CritRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Cloth }),

            new ItemModifier("Etched", "of the Rune", 2,
            new Statistics() { AttackPower = 1, DamageReduction = { ArmorRating = 1 } }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Warder's", "of the Warder", 2,
            new Statistics() { HitRating = 1, DamageReduction = { ArmorRating = 1 } }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Furious", "of Fury", 2,
            new Statistics() { AttackPower = 1, HitRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Leather, ItemArmorClasses.Plate }),

            new ItemModifier("Berzerker", "of Berzerking", 2,
            new Statistics() { AttackPower = 1, CritRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Leather, ItemArmorClasses.Plate }),

            new ItemModifier("Astral", "of Stars", 2,
            new Statistics() { SpellPower = 1, HitRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather }),

            new ItemModifier("Charged", "of Electricity", 2,
            new Statistics() { SpellPower = 1, CritRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon }, 
            new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather }),

            new ItemModifier("Wicked", "of the Wicked", 2,
            new Statistics() { HitRating = 1, CritRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Leather }),

            new ItemModifier("Fortified", "of Fortification", 2,
            new Statistics() { AttackPower = 1, DodgeRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Brilliant", "of Brilliance", 2,
            new Statistics() { SpellPower = 1, PrecisionRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather }),

            new ItemModifier("Swift", "of Swiftness", 2,
            new Statistics() { HitRating = 1, DodgeRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Perceptive", "of Perception", 2,
            new Statistics() { HitRating = 1, PrecisionRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather }),

            new ItemModifier("Cunning", "of Cunning", 2,
            new Statistics() { CritRating = 1, PrecisionRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Leather }),

            new ItemModifier("Indestructible", "", 3,
            new Statistics() { Health = 1, DamageReduction = { ArmorRating = 1 }, DodgeRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Prismatic", "", 4,
            new Statistics() { DamageReduction = { FireResistanceRating = 1, FrostResistanceRating = 1, ArcaneResistanceRating = 1, NatureResistanceRating = 1 } }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Accurate", "of Accuracy", 3,
            new Statistics() { HitRating = 1, CritRating = 1, PrecisionRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Leather }),

            new ItemModifier("Guardian's", "of the Guardian", 5,
            new Statistics() { Health = 1, DamageReduction = { ArmorRating = 1 }, AttackPower = 1, HitRating = 1, DodgeRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Plate }),

            new ItemModifier("Physician's", "of the Physician", 5,
            new Statistics() { Health = 1, Mana = 1, SpellPower = 1, CritRating = 1, PrecisionRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Cloth }),

            new ItemModifier("Ravager's", "of the Ravager", 5,
            new Statistics() { Health = 1, AttackPower = 1, HitRating = 1, CritRating = 1, PrecisionRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Leather }),

            new ItemModifier("Mages'", "of the Magi", 5,
            new Statistics() { Health = 1, SpellPower = 1, HitRating = 1, CritRating = 1, PrecisionRating = 1 }, 
            new[] { ItemSlots.Boots, ItemSlots.Chest, ItemSlots.Hands, ItemSlots.Head, ItemSlots.Legs, ItemSlots.Weapon, ItemSlots.Offhand }, 
            new[] { ItemArmorClasses.Cloth, ItemArmorClasses.Leather }),
        };
    }
}
