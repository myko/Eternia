using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Myko.Xna.Ui;

namespace EterniaXna.Screens
{
    public class SelectPartyScreen: MenuScreen
    {
        private readonly Player player;
        private readonly EncounterDefinition encounterDefinition;
        
        private ListBox<Actor> memberListBox;
        private ListBox<Actor> recruitListBox;
        private SpriteFont smallFont;

        public SelectPartyScreen(Player player, EncounterDefinition encounterDefinition)
        {
            this.player = player;
            this.encounterDefinition = encounterDefinition;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            smallFont = ContentManager.Load<SpriteFont>(@"Fonts\kootenaySmall");

            var grid = new Grid();
            grid.Width = Width;
            grid.Height = Height;
            grid.Rows.Add(GridSize.Fill());
            grid.Rows.Add(GridSize.Fixed(30));
            grid.Rows.Add(GridSize.Fill(4));
            grid.Rows.Add(GridSize.Fixed(40));
            grid.Rows.Add(GridSize.Fixed(80));
            grid.Rows.Add(GridSize.Fixed(80));
            grid.Columns.Add(GridSize.Fill());
            grid.Columns.Add(GridSize.Fixed(120));
            grid.Columns.Add(GridSize.Fill());
            Controls.Add(grid);

            grid.Cells[1, 0].Add(new Label { Text = "Select Party Members" });
            grid.Cells[1, 2].Add(new Label { Text = "Recruit Party Members" });

            memberListBox = AddListBox<Actor>(grid.Cells[2,0], Vector2.Zero, 300, 250);
            memberListBox.EnableCheckBoxes = true;
            
            recruitListBox = AddListBox<Actor>(grid.Cells[2,2], Vector2.Zero, 300, 250);

            grid.Cells[3, 0].Add(new Label { Font = smallFont, Text = Bind(() => {
                if (memberListBox.CheckedItems.Any())
                {
                    if (memberListBox.CheckedItems.SelectMany(x => x.Equipment).Any())
                    {
                        var averageItemLevel = Math.Round(memberListBox.CheckedItems.SelectMany(x => x.Equipment).Average(x => x.Level));
                        return "Average item level: " + averageItemLevel.ToString();
                    }
                }
                return "";
            })
            });

            var deleteButton = CreateButton("Delete", Vector2.Zero);
            deleteButton.Click += deleteButton_Click;
            grid.Cells[4,0].Add(deleteButton);

            var recruitButton = CreateButton("Recruit", Vector2.Zero);
            recruitButton.Click += recruitButton_Click;
            grid.Cells[4,2].Add(recruitButton);

            var startButton = CreateButton("Start", Vector2.Zero);
            startButton.Click += okButton_Click;
            grid.Cells[5,1].Add(startButton);

            memberListBox.Items.AddRange(player.Heroes);
            GenerateRecruits();
        }

        private void deleteButton_Click(object sender, System.EventArgs e)
        {
            if (memberListBox.SelectedItem != null)
            {
                var actor = memberListBox.SelectedItem;
                memberListBox.Items.Remove(actor);
                player.Heroes.Remove(actor);
                GenerateRecruits();
            }
        }

        private void recruitButton_Click(object sender, System.EventArgs e)
        {
            if (recruitListBox.SelectedItem != null)
            {
                memberListBox.Items.Add(recruitListBox.SelectedItem);
                recruitListBox.Items.Remove(recruitListBox.SelectedItem);
                player.Heroes.Add(recruitListBox.SelectedItem);
            }
        }

        private void GenerateRecruits()
        {
            recruitListBox.Items.Clear();

            if (!memberListBox.Items.Any(x => x.Name == "He-Man"))
                recruitListBox.Items.Add(ContentManager.Load<Actor>(@"Actors\He-Man"));
            if (!memberListBox.Items.Any(x => x.Name == "Man-at-Arms"))
                recruitListBox.Items.Add(ContentManager.Load<Actor>(@"Actors\Man-at-Arms"));
            if (!memberListBox.Items.Any(x => x.Name == "Teela"))
                recruitListBox.Items.Add(ContentManager.Load<Actor>(@"Actors\Teela"));
            if (!memberListBox.Items.Any(x => x.Name == "Stratos"))
                recruitListBox.Items.Add(ContentManager.Load<Actor>(@"Actors\Stratos"));

            foreach (var recruit in recruitListBox.Items)
            {
                recruit.CurrentHealth = recruit.MaximumHealth;
                recruit.CurrentMana = recruit.MaximumMana;
            }
        }

        public override void HandleInput(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        void okButton_Click(object sender, System.EventArgs e)
        {
            var count = memberListBox.CheckedItems.Count();
            for (int i = 0; i < count; i++)
            {
                var actor = memberListBox.CheckedItems.ElementAt(i);
                var a = Math.PI * 2.0 / (double)count;
                var x = (float)Math.Cos(i * a) * 3f;
                var y = (float)Math.Sin(i * a) * 3f;

                // new Vector2(-10f, (memberListBox.CheckedItems.Count() / 2f * 5f - 2.5f) - i * 5f);
                actor.Position = new Vector2(-10f, 0f) + new Vector2(x, y);
                actor.Direction = Vector2.Normalize(new Vector2(1, -1));
                actor.Destination = null;
                actor.IsAlive = true;
                actor.CurrentHealth = actor.MaximumHealth;
                actor.CurrentMana = actor.MaximumMana;
            }

            var battle = new Battle(encounterDefinition);

            battle.Actors.AddRange(memberListBox.CheckedItems.Take(encounterDefinition.HeroLimit));

            VictoryScreen.SaveActors(ScreenManager, player);

            ScreenManager.AddScreen(new EncounterScreen(player, encounterDefinition, battle));
            ScreenManager.RemoveScreen(this);
        }
    }
}
