﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Myko.Xna.Ui;
using System;
using EterniaGame.Actors;

namespace EterniaXna.Screens
{
    public class VictoryScreen: MenuScreen
    {
        private readonly Player player;
        private readonly Battle battle;
        private readonly EncounterDefinition encounterDefinition;
        private IEnumerable<Turn> turns;
        private ListBox<Item> rewardsListBox;
        private SpriteFont smallFont;

        public VictoryScreen(Player player, EncounterDefinition encounterDefinition, Battle battle, IEnumerable<Turn> turns)
        {
            this.player = player;
            this.battle = battle;
            this.encounterDefinition = encounterDefinition;
            this.turns = turns;

            if (!player.CompletedEncounters.Contains(encounterDefinition.Name))
                player.CompletedEncounters.Add(encounterDefinition.Name);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            smallFont = ContentManager.Load<SpriteFont>(@"Fonts\KootenaySmall");

            var grid = new Grid(); 
            grid.Width = Width;
            grid.Height = Height;
            grid.Rows.Add(GridSize.Fixed(80));
            grid.Rows.Add(GridSize.Fill(2));
            grid.Rows.Add(GridSize.Fill());
            grid.Rows.Add(GridSize.Fixed(80));
            grid.Columns.Add(GridSize.Fill());
            Controls.Add(grid);

            grid.Cells[0, 0].Add(new Label { Text = "Victory!" });

            ItemGenerator generator = new ItemGenerator(new Randomizer());
            rewardsListBox = AddListBox<Item>(grid.Cells[1, 0], Vector2.Zero, 450, 250);
            rewardsListBox.ZIndex = 0.2f;
            rewardsListBox.EnableCheckBoxes = true;
            for (int i = 0; i < 5 + new Random().Between(0, battle.Actors.Sum(x => x.CurrentStatistics.ExtraRewards)); i++)
            {
                var item = generator.Generate(encounterDefinition.ItemLevel);
                rewardsListBox.Items.Add(item, new ItemTooltip(item) { Font = smallFont }, ItemTooltip.GetItemColor(item.Rarity));
            }

            var okButton = CreateButton("OK", Vector2.Zero);
            okButton.Click += okButton_Click;
            grid.Cells[3, 0].Add(okButton);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            var events = turns.SelectMany(x => x.Events);

            if (events.Any())
            {
                var duration = events.Last().TimeStamp - events.First().TimeStamp;

                float y = Height - 200;
                foreach (var actorEvents in events.GroupBy(x => x.Actor))
                {
                    var abilitySwings = actorEvents.Where(x => x.Type == EventTypes.Ability);
                    var missSwings = abilitySwings.Where(x => x.CombatOutcome == CombatOutcome.Miss);
                    var dodgeSwings = abilitySwings.Where(x => x.CombatOutcome == CombatOutcome.Dodge);
                    var hitSwings = abilitySwings.Where(x => x.CombatOutcome == CombatOutcome.Hit);
                    var critSwings = abilitySwings.Where(x => x.CombatOutcome == CombatOutcome.Crit);

                    y += 16;
                    SpriteBatch.DrawString(smallFont, actorEvents.Key.Name, new Vector2(200, y), Color.White, 0.1f);
                    SpriteBatch.DrawString(smallFont, (actorEvents.Sum(x => x.Damage) / duration.TotalSeconds).ToString("0") + " DPS", new Vector2(350, y), Color.White, 0.1f);
                    SpriteBatch.DrawString(smallFont, abilitySwings.Count().ToString(), new Vector2(450, y), Color.White, 0.1f);
                    if (abilitySwings.Count() > 0)
                    {
                        SpriteBatch.DrawString(smallFont, (100 * missSwings.Count() / abilitySwings.Count()).ToString() + "% m", new Vector2(550, y), Color.White, 0.1f);
                        SpriteBatch.DrawString(smallFont, (100 * dodgeSwings.Count() / abilitySwings.Count()).ToString() + "% d", new Vector2(600, y), Color.White, 0.1f);
                        SpriteBatch.DrawString(smallFont, (100 * hitSwings.Count() / abilitySwings.Count()).ToString() + "% h", new Vector2(650, y), Color.White, 0.1f);
                        if (hitSwings.Count() > 0)
                            SpriteBatch.DrawString(smallFont, (100 * critSwings.Count() / hitSwings.Count()).ToString() + "% c", new Vector2(700, y), Color.White, 0.1f);
                    }
                }
            }
        }

        public static void DrawStatistics(SpriteBatch spriteBatch, SpriteFont font, Statistics statistics, int x, int y, bool hideZero)
        {
            if (!hideZero || statistics.Health != 0)
                spriteBatch.DrawString(font, "Health: " + statistics.Health.ToString("0"), new Vector2(x, y += 20), Color.LightGreen, 0.1f);
            if (!hideZero || statistics.Mana != 0)
                spriteBatch.DrawString(font, "Mana: " + statistics.Mana.ToString("0"), new Vector2(x, y += 20), Color.LightGreen, 0.1f);
            if (!hideZero || statistics.Energy != 0)
                spriteBatch.DrawString(font, "Energy: " + statistics.Energy.ToString("0"), new Vector2(x, y += 20), Color.LightGreen, 0.1f);
            if (!hideZero || statistics.ArmorRating != 0)
                spriteBatch.DrawString(font, "Armor rating: " + statistics.ArmorRating.ToString() + " (" + (statistics.ArmorReduction * 100).ToString("0") + "%)", new Vector2(x, y += 20), Color.LightGreen, 0.1f);
            if (!hideZero || statistics.AttackPower != 0)
                spriteBatch.DrawString(font, "Attack power: " + statistics.AttackPower.ToString("0"), new Vector2(x, y += 20), Color.LightGreen, 0.1f);
            if (!hideZero || statistics.SpellPower != 0)
                spriteBatch.DrawString(font, "Spell power: " + statistics.SpellPower.ToString("0"), new Vector2(x, y += 20), Color.LightGreen, 0.1f);
            if (!hideZero || statistics.HitRating != 0)
                spriteBatch.DrawString(font, "Hit rating: " + statistics.HitRating.ToString(), new Vector2(x, y += 20), Color.LightGreen, 0.1f);
            if (!hideZero || statistics.CritRating != 0)
                spriteBatch.DrawString(font, "Crit rating: " + statistics.CritRating.ToString() + " (" + (statistics.CritChance * 100).ToString("0") + "%)", new Vector2(x, y += 20), Color.LightGreen, 0.1f);
            if (!hideZero || statistics.PrecisionRating != 0)
                spriteBatch.DrawString(font, "Precision rating: " + statistics.PrecisionRating.ToString() + " (" + (statistics.Precision * 100).ToString("0") + "%)", new Vector2(x, y += 20), Color.LightGreen, 0.1f);
            if (!hideZero || statistics.DodgeRating != 0)
                spriteBatch.DrawString(font, "Dodge rating: " + statistics.DodgeRating.ToString(), new Vector2(x, y += 20), Color.LightGreen, 0.1f);
        }

        public static string GetStatisticsString(Statistics statistics, ActorResourceTypes resourceType, bool hideZero)
        {
            StringBuilder sb = new StringBuilder();

            if (!hideZero || statistics.Health != 0)
                sb.AppendLine("Health: " + statistics.Health.ToString("0"));
            if (resourceType == ActorResourceTypes.Mana)
                sb.AppendLine("Mana: " + statistics.Mana.ToString("0"));
            if (resourceType == ActorResourceTypes.Energy)
                sb.AppendLine("Energy: " + statistics.Energy.ToString("0"));
            if (!hideZero || statistics.ArmorRating != 0)
                sb.AppendLine("Armor rating: " + statistics.ArmorRating.ToString() + " (" + (statistics.ArmorReduction * 100).ToString("0") + "%)");
            if (!hideZero || statistics.AttackPower != 0)
                sb.AppendLine("Attack power: " + statistics.AttackPower.ToString("0"));
            if (!hideZero || statistics.SpellPower != 0)
                sb.AppendLine("Spell power: " + statistics.SpellPower.ToString("0"));
            if (!hideZero || statistics.HitRating != 0)
                sb.AppendLine("Hit rating: " + statistics.HitRating.ToString());
            if (!hideZero || statistics.CritRating != 0)
                sb.AppendLine("Crit rating: " + statistics.CritRating.ToString() + " (" + (statistics.CritChance * 100).ToString("0") + "%)");
            if (!hideZero || statistics.PrecisionRating != 0)
                sb.AppendLine("Precision rating: " + statistics.PrecisionRating.ToString() + " (" + (statistics.Precision * 100).ToString("0") + "%)");
            if (!hideZero || statistics.DodgeRating != 0)
                sb.AppendLine("Dodge rating: " + statistics.DodgeRating.ToString());

            return sb.ToString();
        }

        void okButton_Click()
        {
            foreach (var item in rewardsListBox.CheckedItems)
            {
                player.Inventory.Add(item);
            }
            player.Gold += (int)(encounterDefinition.ItemLevel * new Random().Between(9f, 11f));

            SaveActors(ScreenManager, player);

            ScreenManager.AddScreen(new TitleScreen(player));
            ScreenManager.RemoveScreen(this);
        }

        public static void SaveActors(ScreenManager screenManager, Player player)
        {
            //StorageDevice storage = (StorageDevice)screenManager.Game.Services.GetService(typeof(StorageDevice));

            //if (storage != null)
            {
                //using (var container = storage.OpenContainer("Eternia"))
                {
                    //var containerPath = container.Path;
                    var containerPath = @"C:\Users\Christer\Documents\SavedGames\Eternia\AllPlayers";

                    player.Heroes.ForEach(x => x.Auras.Clear());

                    // Add the container path to our file name.
                    string filename = Path.Combine(containerPath, "Player.xml");

                    // Open the file, creating it if necessary
                    FileStream stream = File.Open(filename, FileMode.Create);

                    // Convert the object to XML data and put it in the stream
                    XmlSerializer serializer = new XmlSerializer(typeof(Player));
                    serializer.Serialize(stream, player);

                    // Close the file
                    stream.Close();
                }
            }
        }
    }
}
