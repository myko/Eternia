using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EterniaGame;
using EterniaGame.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;
using Eternia.Game.Stats;
using Newtonsoft.Json;

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
            Random random = new Random();
            for (int i = 0; i < 5 + random.Between(0, battle.Actors.Sum(x => x.CurrentStatistics.For<ExtraRewards>().Value)); i++)
            {
                var item = encounterDefinition.Loot.Any() ? random.From(encounterDefinition.Loot) : generator.Generate(encounterDefinition.ItemLevel);
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
                    var missSwings = abilitySwings.Where(x => x.CombatOutcome.IsMiss);
                    var dodgeSwings = abilitySwings.Where(x => x.CombatOutcome.IsDodge);
                    var hitSwings = abilitySwings.Where(x => x.CombatOutcome.IsHit);
                    var critSwings = abilitySwings.Where(x => x.CombatOutcome.IsCrit);

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

        public static string GetStatisticsString(Statistics statistics, ActorResourceTypes resourceType, bool hideZero)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var stat in statistics.OrderBy(x => x.Name))
            {
                sb.AppendLine(stat.Name + ": " + stat.ToValueString());
            }

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
            var containerPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Eternia");
            //var containerPath = @"C:\Users\Christer\Documents\SavedGames\Eternia\AllPlayers";

            if (!Directory.Exists(containerPath))
                Directory.CreateDirectory(containerPath);

            player.Heroes.ForEach(x => x.Auras.Clear());

            // Add the container path to our file name.
            string filename = Path.Combine(containerPath, "Player.xml");

            // Open the file, creating it if necessary
            FileStream stream = File.Open(filename, FileMode.Create);
            var writer = new StreamWriter(stream);

            // Convert the object to XML data and put it in the stream
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(player, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            writer.Write(json);

            // Close the file
            writer.Close();
        }
    }
}
