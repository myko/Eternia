﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EterniaGame.Abilities;

namespace EterniaGame.Actors
{
    public class ActorGenerator
    {
        private Randomizer randomizer;

        public ActorGenerator(Randomizer randomizer)
        {
            this.randomizer = randomizer;
        }

        public Actor Generate()
        {
            var actor = new Actor
            {
                Id = Guid.NewGuid().ToString(),
                Faction = Factions.Friend,
                TextureName = randomizer.From(new[] { "heman", "manatarms" }),
                Name = GenerateName(),
                Cost = randomizer.Between(5, 100),
                PlayerControlled = true,
                BaseStatistics = new Statistics
                {
                    Health = 250,
                    MissRating = 100,
                }
            };

            actor.BaseModifiers.HealthModifier = randomizer.Between(1f, 20f);
            actor.BaseModifiers.AttackPowerModifier = randomizer.Between(1f, 20f);
            actor.BaseModifiers.SpellPowerModifier = randomizer.Between(1f, 20f);
            var sum = actor.BaseModifiers.HealthModifier + actor.BaseModifiers.AttackPowerModifier + actor.BaseModifiers.SpellPowerModifier;
            actor.BaseModifiers.HealthModifier = 0.5f + actor.BaseModifiers.HealthModifier / sum;
            actor.BaseModifiers.AttackPowerModifier = 0.5f + actor.BaseModifiers.AttackPowerModifier / sum;
            actor.BaseModifiers.SpellPowerModifier = 0.5f + actor.BaseModifiers.SpellPowerModifier / sum;

            actor.ResourceType = randomizer.Next<ActorResourceTypes>();

            switch (actor.ResourceType)
            {
                case ActorResourceTypes.Mana:
                    actor.BaseStatistics.Mana = randomizer.Between(475, 525);
                    break;
                case ActorResourceTypes.Energy:
                    actor.BaseStatistics.Energy = 100;
                    break;
            }

            actor.CurrentHealth = actor.CurrentStatistics.Health;
            actor.CurrentMana = actor.CurrentStatistics.Mana;
            actor.CurrentEnergy = actor.CurrentStatistics.Energy;
            
            //var itemGenerator = new ItemGenerator(randomizer);

            //actor.Equipment.Add(itemGenerator.Generate(randomizer.Between(5, 8), ItemSlots.Chest, ItemRarities.Rare));
            //actor.Equipment.Add(itemGenerator.Generate(randomizer.Between(5, 8), ItemSlots.Legs, ItemRarities.Uncommon));
            //actor.Equipment.Add(itemGenerator.Generate(randomizer.Between(5, 8), ItemSlots.Boots, ItemRarities.Uncommon));
            //actor.Equipment.Add(itemGenerator.Generate(randomizer.Between(5, 8), ItemSlots.Weapon, ItemRarities.Rare));
            //actor.Equipment.Add(itemGenerator.Generate(randomizer.Between(5, 8), ItemSlots.Offhand, ItemRarities.Uncommon));
            
            //var skillGenerator = new SkillGenerator(randomizer);

            //actor.Abilities.Add(skillGenerator.Generate(actor.ResourceType));
            //actor.Abilities.Add(skillGenerator.Generate(actor.ResourceType));
            //actor.Abilities.Add(skillGenerator.Generate(actor.ResourceType));
            //actor.Abilities.Add(skillGenerator.Generate(actor.ResourceType));
            //actor.Abilities.Add(skillGenerator.Generate(actor.ResourceType));

            return actor;
        }

        private string GenerateName()
        {
            var syllables = new[] { 
                "an", "am", "al", "af", "ag", "ah", "ahj", 
                "bi", "ba", "be", "bu", "by", "bah", "beh", "buh", "bruh", "bir", "ber",
                "ca", "cy", "ci", "ce", "cy", "cra", "cri", "cre", "chi", "cha", "che",
                "da", "du", "di", "de", "dy", "din", "dun", "den",
                "he", "man", "te", "tee", "ram", "ra", "of", "off", "fa", "li", "lo", "la",
                "ske", "ska", "sa", "se", "su", "si", "so",
                "ya", "yu", "yi", "ye", "ym", "yn", "ys", "yt"
            };

            var length = randomizer.Between(1, 4);
            string name = randomizer.From(syllables);

            for (int i = 0; i < length; i++)
            {
                name = name + randomizer.From(new[] { "", "", "", "", "", "", " ", "", "", " ", " ", "-", "-", "'" }) + randomizer.From(syllables);
            }

            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);
        }
    }
}
