using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Myko.Xna.Ui;

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
            for (int i = 0; i < 5 + battle.Actors.Sum(x => x.CurrentStatistics.ExtraRewards); i++)
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
                    var meleeSwings = actorEvents.Where(x => x.Type == EventTypes.Swing);
                    var missSwings = meleeSwings.Where(x => x.CombatOutcome == CombatOutcome.Miss);
                    var dodgeSwings = meleeSwings.Where(x => x.CombatOutcome == CombatOutcome.Dodge);
                    var hitSwings = meleeSwings.Where(x => x.CombatOutcome == CombatOutcome.Hit);
                    var critSwings = meleeSwings.Where(x => x.CombatOutcome == CombatOutcome.Crit);

                    y += 16;
                    SpriteBatch.DrawString(smallFont, actorEvents.Key.Name, new Vector2(200, y), Color.White, 0.1f);
                    SpriteBatch.DrawString(smallFont, (actorEvents.Sum(x => x.Damage) / duration.TotalSeconds).ToString("0") + " DPS", new Vector2(350, y), Color.White, 0.1f);
                    SpriteBatch.DrawString(smallFont, meleeSwings.Count().ToString() + " swings", new Vector2(450, y), Color.White, 0.1f);
                    if (meleeSwings.Count() > 0)
                    {
                        SpriteBatch.DrawString(smallFont, (100 * missSwings.Count() / meleeSwings.Count()).ToString() + "% m", new Vector2(550, y), Color.White, 0.1f);
                        SpriteBatch.DrawString(smallFont, (100 * dodgeSwings.Count() / meleeSwings.Count()).ToString() + "% d", new Vector2(600, y), Color.White, 0.1f);
                        SpriteBatch.DrawString(smallFont, (100 * hitSwings.Count() / meleeSwings.Count()).ToString() + "% h", new Vector2(650, y), Color.White, 0.1f);
                        if (hitSwings.Count() > 0)
                            SpriteBatch.DrawString(smallFont, (100 * critSwings.Count() / hitSwings.Count()).ToString() + "% c", new Vector2(700, y), Color.White, 0.1f);
                    }
                }
            }
        }

        public static void DrawStatistics(SpriteBatch spriteBatch, SpriteFont font, Statistics statistics, int x, int y, bool hideZero)
        {
            if (!hideZero || statistics.Health != 0)
                spriteBatch.DrawString(font, "Health: " + statistics.Health.ToString(), new Vector2(x, y += 20), Color.LightGreen, 0.1f);
            if (!hideZero || statistics.Mana != 0)
                spriteBatch.DrawString(font, "Mana: " + statistics.Mana.ToString(), new Vector2(x, y += 20), Color.LightGreen, 0.1f);
            if (!hideZero || statistics.ArmorRating != 0)
                spriteBatch.DrawString(font, "Armor rating: " + statistics.ArmorRating.ToString() + " (" + (statistics.ArmorReduction * 100).ToString("0") + "%)", new Vector2(x, y += 20), Color.LightGreen, 0.1f);
            if (!hideZero || statistics.AttackPower != 0)
                spriteBatch.DrawString(font, "Attack power: " + statistics.AttackPower.ToString(), new Vector2(x, y += 20), Color.LightGreen, 0.1f);
            if (!hideZero || statistics.SpellPower != 0)
                spriteBatch.DrawString(font, "Spell power: " + statistics.SpellPower.ToString(), new Vector2(x, y += 20), Color.LightGreen, 0.1f);
            if (!hideZero || statistics.HitRating != 0)
                spriteBatch.DrawString(font, "Hit rating: " + statistics.HitRating.ToString(), new Vector2(x, y += 20), Color.LightGreen, 0.1f);
            if (!hideZero || statistics.CritRating != 0)
                spriteBatch.DrawString(font, "Crit rating: " + statistics.CritRating.ToString() + " (" + (statistics.CritChance * 100).ToString("0") + "%)", new Vector2(x, y += 20), Color.LightGreen, 0.1f);
            if (!hideZero || statistics.PrecisionRating != 0)
                spriteBatch.DrawString(font, "Precision rating: " + statistics.PrecisionRating.ToString() + " (" + (statistics.Precision * 100).ToString("0") + "%)", new Vector2(x, y += 20), Color.LightGreen, 0.1f);
            if (!hideZero || statistics.DodgeRating != 0)
                spriteBatch.DrawString(font, "Dodge rating: " + statistics.DodgeRating.ToString(), new Vector2(x, y += 20), Color.LightGreen, 0.1f);
        }

        public static string GetStatisticsString(Statistics statistics, bool hideZero)
        {
            StringBuilder sb = new StringBuilder();

            if (!hideZero || statistics.Health != 0)
                sb.AppendLine("Health: " + statistics.Health.ToString());
            if (!hideZero || statistics.Mana != 0)
                sb.AppendLine("Mana: " + statistics.Mana.ToString());
            if (!hideZero || statistics.ArmorRating != 0)
                sb.AppendLine("Armor rating: " + statistics.ArmorRating.ToString() + " (" + (statistics.ArmorReduction * 100).ToString("0") + "%)");
            if (!hideZero || statistics.AttackPower != 0)
                sb.AppendLine("Attack power: " + statistics.AttackPower.ToString());
            if (!hideZero || statistics.SpellPower != 0)
                sb.AppendLine("Spell power: " + statistics.SpellPower.ToString());
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

        void okButton_Click(object sender, System.EventArgs e)
        {
            foreach (var item in rewardsListBox.CheckedItems)
            {
                player.Inventory.Add(item);
            }

            SaveActors(ScreenManager, player);

            ScreenManager.AddScreen(new SelectEncounterScreen(player));
            ScreenManager.RemoveScreen(this);
        }

        public static void SaveActors(ScreenManager screenManager, Player player)
        {
            StorageDevice storage = (StorageDevice)screenManager.Game.Services.GetService(typeof(StorageDevice));

            if (storage != null)
            {
                using (var container = storage.OpenContainer("Eternia"))
                {
                    player.Heroes.ForEach(x => x.Auras.Clear());

                    // Add the container path to our file name.
                    string filename = Path.Combine(container.Path, "Player.xml");

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
