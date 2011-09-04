using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;
using System;

namespace EterniaXna.Screens
{
    public class SelectEncounterScreen: MenuScreen
    {
        private readonly Player player;

        private ListBox<EncounterDefinition> encounterListBox;

        public SelectEncounterScreen(Player player)
        {
            this.player = player;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            var grid = new Grid();
            grid.Width = Width;
            grid.Height = Height;
            grid.Rows.Add(GridSize.Fixed(80));
            grid.Rows.Add(GridSize.Fill());
            grid.Rows.Add(GridSize.Fixed(80));
            grid.Rows.Add(GridSize.Fixed(80));
            grid.Rows.Add(GridSize.Fixed(80));
            grid.Columns.Add(GridSize.Fill());
            Controls.Add(grid);

            grid.Cells[0, 0].Add(new Label { Text = "Select Encounter" });

            encounterListBox = AddListBox<EncounterDefinition>(grid.Cells[1,0], Vector2.Zero, 400, 250);
            encounterListBox.Items.Add(ContentManager.Load<EncounterDefinition>(@"Encounters\PointDread"));
            encounterListBox.Items.Add(ContentManager.Load<EncounterDefinition>(@"Encounters\SeaOfRakash"));
            encounterListBox.Items.Add(ContentManager.Load<EncounterDefinition>(@"Encounters\VineJungle"));
            encounterListBox.Items.Add(ContentManager.Load<EncounterDefinition>(@"Encounters\SnakeMountain"));
            encounterListBox.Items.Add(ContentManager.Load<EncounterDefinition>(@"Encounters\Benchmark"));

            grid.Cells[2, 0].Add(new Label { Text = Bind(() => 
            {
                if (encounterListBox.SelectedItem != null)
                {
                    var encounter = encounterListBox.SelectedItem;
                    return "Maximum heroes: " + encounter.HeroLimit.ToString() + "\n" +
                        "Item level: " + encounter.ItemLevel.ToString();
                }
                return ""; 
            }) });

            var startButton = CreateButton("Start", Vector2.Zero);
            startButton.Click += okButton_Click;
            grid.Cells[3,0].Add(startButton);

            var exitButton = CreateButton("Exit", Vector2.Zero);
            exitButton.Click += quitButton_Click;
            grid.Cells[4,0].Add(exitButton);
        }

        public override void HandleInput(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        private void okButton_Click(object sender, System.EventArgs e)
        {
            if (encounterListBox.SelectedItem == null)
                return;

            ScreenManager.AddScreen(new SelectPartyScreen(player, encounterListBox.SelectedItem));
            ScreenManager.RemoveScreen(this);
        }

        private void quitButton_Click(object sender, System.EventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
