using EterniaGame;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;
using System;
using System.Collections.Generic;

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

            var encounters = new List<EncounterDefinition>();
            encounters.Add(ContentManager.Load<EncounterDefinition>(@"Encounters\1_1_Gates"));
            encounters.Add(ContentManager.Load<EncounterDefinition>(@"Encounters\1_2_OuterCourtyard"));
            encounters.Add(ContentManager.Load<EncounterDefinition>(@"Encounters\1_3_InnerCourtyard"));
            encounters.Add(ContentManager.Load<EncounterDefinition>(@"Encounters\1_4_ThroneRoom"));

            encounterListBox = AddListBox<EncounterDefinition>(grid.Cells[1, 0], Vector2.Zero, 400, 250);
            foreach (var encounter in encounters)
            {
                if (encounter.Map == null)
                    encounter.Map = new Map(18, 16);
                encounter.Map.UpdateTileReferences();
                var e = encounter;
                encounterListBox.Items.Add(e, null, Bind(() =>
                {
                    if (player.CompletedEncounters.Contains(e.Name))
                        return Color.Gold;

                    if (e.PrerequisiteEncounters.Any(x => !player.CompletedEncounters.Contains(x)))
                        return Color.Salmon;

                    return Color.LightGreen;
                }));
            }

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

            var backButton = CreateButton("Back", Vector2.Zero);
            backButton.Click += backButton_Click;
            grid.Cells[4,0].Add(backButton);
        }

        public override void HandleInput(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        private void okButton_Click()
        {
            if (encounterListBox.SelectedItem == null)
                return;

            if (encounterListBox.SelectedItem.PrerequisiteEncounters.Any(x => !player.CompletedEncounters.Contains(x)))
                return;

            ScreenManager.AddScreen(new SelectPartyScreen(player, encounterListBox.SelectedItem));
            ScreenManager.RemoveScreen(this);
        }

        private void backButton_Click()
        {
            ScreenManager.AddScreen(new TitleScreen(player));
            ScreenManager.RemoveScreen(this);
        }
    }
}
