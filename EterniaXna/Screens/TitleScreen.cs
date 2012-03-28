using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;
using System;

namespace EterniaXna.Screens
{
    public class TitleScreen: MenuScreen
    {
        private readonly Player player;

        public TitleScreen(Player player)
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
            grid.Rows.Add(GridSize.Fixed(80));
            grid.Columns.Add(GridSize.Fill());
            Controls.Add(grid);

            grid.Cells[0, 0].Add(new Label { Text = "Eternia" });

            var startButton = CreateButton("Encounter", Vector2.Zero);
            startButton.Click += encounterButton_Click;
            grid.Cells[2, 0].Add(startButton);

            var storeButton = CreateButton("Store", Vector2.Zero);
            storeButton.Click += storeButton_Click;
            grid.Cells[3, 0].Add(storeButton);

            var equipmentButton = CreateButton("Equipment", Vector2.Zero);
            equipmentButton.Click += equipmentButton_Click;
            grid.Cells[4, 0].Add(equipmentButton);

            var exitButton = CreateButton("Exit", Vector2.Zero);
            exitButton.Click += quitButton_Click;
            grid.Cells[5, 0].Add(exitButton);
        }

        public override void HandleInput(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        private void encounterButton_Click()
        {
            ScreenManager.AddScreen(new SelectEncounterScreen(player));
            ScreenManager.RemoveScreen(this);
        }

        private void storeButton_Click()
        {
            ScreenManager.AddScreen(new StoreScreen(player));
        }

        private void equipmentButton_Click()
        {
            if (player.Heroes.Count > 0)
                ScreenManager.AddScreen(new EquipmentScreen(player, player.Heroes, player.Heroes[0]));
        }

        private void quitButton_Click()
        {
            ScreenManager.Game.Exit();
        }
    }
}
