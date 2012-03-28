using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EterniaGame
{
    public enum ItemAffixes
    {
        None,

        Vigorous,
        Energized,
        Sturdy,
        Jagged,
        Arcane,
        Vicious,
        Deadly,
        Precise,
        Agile,

        Owl,
        Bear,
        Lion,
        Lizard,
        Hawk,
        Serpent,
        Spider,
        Cat,
        Rabbit,

        Vitalized,
        Unyielding,
        Mighty,
        Wizards,
        Archers,
        Veteran,
        Glimmering,
        Maidens,
        Etched,
        Wardens,
        Furious,
        Berzerker,
        Astral,
        Charged,
        Wicked,

        Carnage,
        Fortification,
        Balance,
        Brilliance,
        Cunning,
        Perception,
        Swiftness,
        Perfection,

        Physician,
        Guardian,
        Ravager,
        Magi,

        Lucky
    }

    public class ItemAffix
    {
        public string Name { get; set; }
        public Statistics Statistics { get; set; }

        public ItemAffix()
        {
            Name = string.Empty;
            Statistics = new Statistics();
        }

        public ItemAffix(string name, Statistics statistics)
        {
            this.Name = name;
            this.Statistics = statistics;
        }
    }

    public static class ItemAffixConstants
    {
        public static readonly ItemAffixes[,][] Prefixes = new ItemAffixes[,][] {
        // UNCOMMON (1-way prefix)
        {   // CLOTH 
            new ItemAffixes[] { ItemAffixes.Vigorous, ItemAffixes.Energized, ItemAffixes.Arcane, ItemAffixes.Vicious, ItemAffixes.Deadly, ItemAffixes.Precise, ItemAffixes.Lucky }, 
            // LEATHER
            new ItemAffixes[] { ItemAffixes.Vigorous, ItemAffixes.Jagged, ItemAffixes.Vicious, ItemAffixes.Deadly, ItemAffixes.Precise, ItemAffixes.Lucky }, 
            // PLATE
            new ItemAffixes[] { ItemAffixes.Vigorous, ItemAffixes.Sturdy, ItemAffixes.Vicious, ItemAffixes.Agile, ItemAffixes.Lucky } 
        },
        // RARE (1-way prefix + 1-way suffix)
        {   // CLOTH 
            new ItemAffixes[] { ItemAffixes.Vigorous, ItemAffixes.Energized, ItemAffixes.Arcane, ItemAffixes.Vicious, ItemAffixes.Deadly, ItemAffixes.Precise, ItemAffixes.Lucky }, 
            // LEATHER
            new ItemAffixes[] { ItemAffixes.Vigorous, ItemAffixes.Jagged, ItemAffixes.Vicious, ItemAffixes.Deadly, ItemAffixes.Precise, ItemAffixes.Lucky }, 
            // PLATE
            new ItemAffixes[] { ItemAffixes.Vigorous, ItemAffixes.Sturdy, ItemAffixes.Vicious, ItemAffixes.Agile, ItemAffixes.Lucky } 
        },
        // HEROIC (2-way prefix + 1-way suffix)
        {   // CLOTH 
            new ItemAffixes[] { ItemAffixes.Vitalized, ItemAffixes.Wizards, ItemAffixes.Archers, ItemAffixes.Veteran, ItemAffixes.Glimmering, ItemAffixes.Maidens, ItemAffixes.Astral, ItemAffixes.Charged, ItemAffixes.Wicked }, 
            // LEATHER
            new ItemAffixes[] { ItemAffixes.Mighty, ItemAffixes.Archers, ItemAffixes.Veteran, ItemAffixes.Furious, ItemAffixes.Berzerker, ItemAffixes.Wicked }, 
            // PLATE
            new ItemAffixes[] { ItemAffixes.Unyielding, ItemAffixes.Mighty, ItemAffixes.Archers, ItemAffixes.Etched, ItemAffixes.Wardens, ItemAffixes.Furious } 
        },
        // EPIC (2-way prefix + 2-way suffix)
        {   // CLOTH 
            new ItemAffixes[] { ItemAffixes.Vitalized, ItemAffixes.Wizards, ItemAffixes.Archers, ItemAffixes.Veteran, ItemAffixes.Glimmering, ItemAffixes.Maidens, ItemAffixes.Astral, ItemAffixes.Charged, ItemAffixes.Wicked }, 
            // LEATHER
            new ItemAffixes[] { ItemAffixes.Mighty, ItemAffixes.Archers, ItemAffixes.Veteran, ItemAffixes.Furious, ItemAffixes.Berzerker, ItemAffixes.Wicked }, 
            // PLATE
            new ItemAffixes[] { ItemAffixes.Unyielding, ItemAffixes.Mighty, ItemAffixes.Archers, ItemAffixes.Etched, ItemAffixes.Wardens, ItemAffixes.Furious } 
        },
        // LEGENDARY (5-way suffix)
        {   // CLOTH 
            new ItemAffixes[] { ItemAffixes.None }, 
            // LEATHER
            new ItemAffixes[] { ItemAffixes.None }, 
            // PLATE
            new ItemAffixes[] { ItemAffixes.None } 
        },
        };

        public static readonly ItemAffixes[,][] Suffixes = new ItemAffixes[,][] {
        // UNCOMMON (1-way prefix)
        {   // CLOTH 
            new ItemAffixes[] { ItemAffixes.None }, 
            // LEATHER
            new ItemAffixes[] { ItemAffixes.None }, 
            // PLATE
            new ItemAffixes[] { ItemAffixes.None } 
        },
        // RARE (1-way prefix + 1-way suffix)
        {   // CLOTH 
            new ItemAffixes[] { ItemAffixes.Owl, ItemAffixes.Lizard, ItemAffixes.Hawk, ItemAffixes.Serpent, ItemAffixes.Spider, ItemAffixes.Rabbit }, 
            // LEATHER
            new ItemAffixes[] { ItemAffixes.Lion, ItemAffixes.Hawk, ItemAffixes.Serpent, ItemAffixes.Spider, ItemAffixes.Rabbit }, 
            // PLATE
            new ItemAffixes[] { ItemAffixes.Bear, ItemAffixes.Lion, ItemAffixes.Hawk, ItemAffixes.Cat, ItemAffixes.Rabbit } 
        },
        // HEROIC (2-way prefix + 1-way suffix)
        {   // CLOTH 
            new ItemAffixes[] { ItemAffixes.Owl, ItemAffixes.Lizard, ItemAffixes.Hawk, ItemAffixes.Serpent, ItemAffixes.Spider, ItemAffixes.Rabbit }, 
            // LEATHER
            new ItemAffixes[] { ItemAffixes.Lion, ItemAffixes.Hawk, ItemAffixes.Serpent, ItemAffixes.Spider, ItemAffixes.Rabbit }, 
            // PLATE
            new ItemAffixes[] { ItemAffixes.Bear, ItemAffixes.Lion, ItemAffixes.Hawk, ItemAffixes.Cat, ItemAffixes.Rabbit } 
        },
        // EPIC (2-way prefix + 2-way suffix)
        {   // CLOTH 
            new ItemAffixes[] { ItemAffixes.Carnage, ItemAffixes.Balance, ItemAffixes.Brilliance, ItemAffixes.Cunning, ItemAffixes.Perception, ItemAffixes.Perfection }, 
            // LEATHER
            new ItemAffixes[] { ItemAffixes.Carnage, ItemAffixes.Cunning, ItemAffixes.Perception, ItemAffixes.Perfection }, 
            // PLATE
            new ItemAffixes[] { ItemAffixes.Fortification, ItemAffixes.Swiftness } 
        },
        // LEGENDARY (5-way suffix)
        {   // CLOTH 
            new ItemAffixes[] { ItemAffixes.Physician, ItemAffixes.Magi }, 
            // LEATHER
            new ItemAffixes[] { ItemAffixes.Ravager }, 
            // PLATE
            new ItemAffixes[] { ItemAffixes.Guardian } 
        },
        };

        // 1-way prefixes
        private static readonly Statistics Vigorous = new Statistics() { Health = 1 };
        private static readonly Statistics Energized = new Statistics() { Mana = 1 };
        private static readonly Statistics Sturdy = new Statistics() { ArmorRating = 1 };
        private static readonly Statistics Jagged = new Statistics() { AttackPower = 1 };
        private static readonly Statistics Arcane = new Statistics() { SpellPower = 1 };
        private static readonly Statistics Vicious = new Statistics() { HitRating = 1 };
        private static readonly Statistics Deadly = new Statistics() { CritRating = 1 };
        private static readonly Statistics Precise = new Statistics() { PrecisionRating = 1 };
        private static readonly Statistics Agile = new Statistics() { DodgeRating = 1 };
        private static readonly Statistics Lucky = new Statistics() { ExtraRewards = 1 };
        
        // 1-way suffixes
        private static readonly Statistics Owl = new Statistics() { Mana = 1 };
        private static readonly Statistics Bear = new Statistics() { ArmorRating = 1 };
        private static readonly Statistics Lion = new Statistics() { AttackPower = 1 };
        private static readonly Statistics Lizard = new Statistics() { SpellPower = 1 };
        private static readonly Statistics Hawk = new Statistics() { HitRating = 1 };
        private static readonly Statistics Serpent = new Statistics() { CritRating = 1 };
        private static readonly Statistics Spider = new Statistics() { PrecisionRating = 1 };
        private static readonly Statistics Cat = new Statistics() { DodgeRating = 1 };
        private static readonly Statistics Rabbit = new Statistics() { ExtraRewards = 1 };

        // 2-way prefixes
        private static readonly Statistics Vitalized = new Statistics() { Health = 1, Mana = 1 };
        private static readonly Statistics Unyielding = new Statistics() { Health = 1, ArmorRating = 1 };
        private static readonly Statistics Mighty = new Statistics() { Health = 1, AttackPower = 1 };
        private static readonly Statistics Wizards = new Statistics() { Health = 1, SpellPower = 1 };
        private static readonly Statistics Archers = new Statistics() { Health = 1, HitRating = 1 };
        private static readonly Statistics Veteran = new Statistics() { Health = 1, CritRating = 1 };
        private static readonly Statistics Glimmering = new Statistics() { Mana = 1, SpellPower = 1 };
        private static readonly Statistics Maidens = new Statistics() { Mana = 1, CritRating = 1 };
        private static readonly Statistics Etched = new Statistics() { ArmorRating = 1, AttackPower = 1 };
        private static readonly Statistics Wardens = new Statistics() { ArmorRating = 1, HitRating = 1 };
        private static readonly Statistics Furious = new Statistics() { AttackPower = 1, HitRating = 1 };
        private static readonly Statistics Berzerker = new Statistics() { AttackPower = 1, CritRating = 1 };
        private static readonly Statistics Astral = new Statistics() { SpellPower = 1, HitRating = 1 };
        private static readonly Statistics Charged = new Statistics() { SpellPower = 1, CritRating = 1 };
        private static readonly Statistics Wicked = new Statistics() { HitRating = 1, CritRating = 1 };

        // 2-way suffixes
        private static readonly Statistics Carnage = new Statistics() { AttackPower = 1, HitRating = 1 };
        private static readonly Statistics Fortification = new Statistics() { AttackPower = 1, DodgeRating = 1 };
        private static readonly Statistics Balance = new Statistics() { SpellPower = 1, CritRating = 1 };
        private static readonly Statistics Brilliance = new Statistics() { SpellPower = 1, PrecisionRating = 1 };
        private static readonly Statistics Cunning = new Statistics() { HitRating = 1, CritRating = 1 };
        private static readonly Statistics Perception = new Statistics() { HitRating = 1, PrecisionRating = 1 };
        private static readonly Statistics Swiftness = new Statistics() { HitRating = 1, DodgeRating = 1 };
        private static readonly Statistics Perfection = new Statistics() { CritRating = 1, PrecisionRating = 1 };

        // 5-way suffixes
        private static readonly Statistics Physician = new Statistics() { Health = 1, Mana = 1, SpellPower = 1, CritRating = 1, PrecisionRating = 1 };
        private static readonly Statistics Guardian = new Statistics() { Health = 1, ArmorRating = 1, AttackPower = 1, HitRating = 1, DodgeRating = 1 };
        private static readonly Statistics Ravager = new Statistics() { Health = 1, AttackPower = 1, HitRating = 1, CritRating = 1, PrecisionRating = 1 };
        private static readonly Statistics Magi = new Statistics() { Health = 1, SpellPower = 1, HitRating = 1, CritRating = 1, PrecisionRating = 1 };

        private static Dictionary<ItemAffixes, ItemAffix> affixes;
        public static Dictionary<ItemAffixes, ItemAffix> Affixes
        {
            get
            {
                if (affixes == null)
                {
                    affixes = new Dictionary<ItemAffixes, ItemAffix>();
                    affixes.Add(ItemAffixes.None, new ItemAffix());

                    affixes.Add(ItemAffixes.Vigorous, new ItemAffix("Vigorous ", Vigorous));
                    affixes.Add(ItemAffixes.Energized, new ItemAffix("Energized ", Energized));
                    affixes.Add(ItemAffixes.Sturdy, new ItemAffix("Sturdy ", Sturdy));
                    affixes.Add(ItemAffixes.Jagged, new ItemAffix("Jagged ", Jagged));
                    affixes.Add(ItemAffixes.Arcane, new ItemAffix("Arcane ", Arcane));
                    affixes.Add(ItemAffixes.Vicious, new ItemAffix("Vicious ", Vicious));
                    affixes.Add(ItemAffixes.Deadly, new ItemAffix("Deadly ", Deadly));
                    affixes.Add(ItemAffixes.Precise, new ItemAffix("Precise ", Precise));
                    affixes.Add(ItemAffixes.Agile, new ItemAffix("Agile ", Agile));
                    affixes.Add(ItemAffixes.Lucky, new ItemAffix("Lucky ", Lucky));

                    affixes.Add(ItemAffixes.Owl, new ItemAffix(" of the Owl", Owl));
                    affixes.Add(ItemAffixes.Bear, new ItemAffix(" of the Bear", Bear));
                    affixes.Add(ItemAffixes.Lion, new ItemAffix(" of the Lion", Lion));
                    affixes.Add(ItemAffixes.Lizard, new ItemAffix(" of the Lizard", Lizard));
                    affixes.Add(ItemAffixes.Hawk, new ItemAffix(" of the Hawk", Hawk));
                    affixes.Add(ItemAffixes.Serpent, new ItemAffix(" of the Serpent", Serpent));
                    affixes.Add(ItemAffixes.Spider, new ItemAffix(" of the Spider", Spider));
                    affixes.Add(ItemAffixes.Cat, new ItemAffix(" of the Cat", Cat));
                    affixes.Add(ItemAffixes.Rabbit, new ItemAffix(" of the Rabbit", Rabbit));

                    affixes.Add(ItemAffixes.Vitalized, new ItemAffix("Vitalized ", Vitalized));
                    affixes.Add(ItemAffixes.Unyielding, new ItemAffix("Unyielding ", Unyielding));
                    affixes.Add(ItemAffixes.Mighty, new ItemAffix("Mighty ", Mighty));
                    affixes.Add(ItemAffixes.Wizards, new ItemAffix("Wizard's ", Wizards));
                    affixes.Add(ItemAffixes.Archers, new ItemAffix("Archer's ", Archers));
                    affixes.Add(ItemAffixes.Veteran, new ItemAffix("Veteran ", Veteran));
                    affixes.Add(ItemAffixes.Glimmering, new ItemAffix("Glimmering ", Glimmering));
                    affixes.Add(ItemAffixes.Maidens, new ItemAffix("Maiden's ", Maidens));
                    affixes.Add(ItemAffixes.Etched, new ItemAffix("Etched ", Etched));
                    affixes.Add(ItemAffixes.Wardens, new ItemAffix("Warden's ", Wardens));
                    affixes.Add(ItemAffixes.Furious, new ItemAffix("Furious ", Furious));
                    affixes.Add(ItemAffixes.Berzerker, new ItemAffix("Berzerker ", Berzerker));
                    affixes.Add(ItemAffixes.Astral, new ItemAffix("Astral ", Astral));
                    affixes.Add(ItemAffixes.Charged, new ItemAffix("Charged ", Charged));
                    affixes.Add(ItemAffixes.Wicked, new ItemAffix("Wicked ", Wicked));

                    affixes.Add(ItemAffixes.Carnage, new ItemAffix(" of Carnage", Carnage));
                    affixes.Add(ItemAffixes.Fortification, new ItemAffix(" of Fortification", Fortification));
                    affixes.Add(ItemAffixes.Balance, new ItemAffix(" of Balance", Balance));
                    affixes.Add(ItemAffixes.Brilliance, new ItemAffix(" of Brilliance", Brilliance));
                    affixes.Add(ItemAffixes.Cunning, new ItemAffix(" of Cunning", Cunning));
                    affixes.Add(ItemAffixes.Perception, new ItemAffix(" of Perception", Perception));
                    affixes.Add(ItemAffixes.Swiftness, new ItemAffix(" of Swiftness", Swiftness));
                    affixes.Add(ItemAffixes.Perfection, new ItemAffix(" of Perfection", Perfection));

                    affixes.Add(ItemAffixes.Physician, new ItemAffix(" of the Physician", Physician));
                    affixes.Add(ItemAffixes.Guardian, new ItemAffix(" of the Guardian", Guardian));
                    affixes.Add(ItemAffixes.Ravager, new ItemAffix(" of the Ravager", Ravager));
                    affixes.Add(ItemAffixes.Magi, new ItemAffix(" of the Magi", Magi));
                }

                return affixes;
            }
        }
    }
}
