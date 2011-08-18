using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;

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
            grid.Rows.Add(GridSize.Fill);
            grid.Rows.Add(GridSize.Fixed(80));
            grid.Rows.Add(GridSize.Fixed(80));
            grid.Columns.Add(GridSize.Fill);
            Controls.Add(grid);

            encounterListBox = AddListBox<EncounterDefinition>(grid.Cells[0,0], new Vector2(Width / 2 - 200, 300), 400, 200);
            encounterListBox.Items.Add(ContentManager.Load<EncounterDefinition>(@"Encounters\PointDread"));
            encounterListBox.Items.Add(ContentManager.Load<EncounterDefinition>(@"Encounters\SeaOfRakash"));
            encounterListBox.Items.Add(ContentManager.Load<EncounterDefinition>(@"Encounters\VineJungle"));
            encounterListBox.Items.Add(ContentManager.Load<EncounterDefinition>(@"Encounters\SnakeMountain"));
            encounterListBox.Items.Add(ContentManager.Load<EncounterDefinition>(@"Encounters\Benchmark"));

            var startButton = CreateButton("Start", new Vector2(Width / 2 - 75, Height - 300));
            startButton.Click += okButton_Click;
            grid.Cells[1,0].Add(startButton);

            var exitButton = CreateButton("Exit", new Vector2(Width / 2 - 75, Height - 160));
            exitButton.Click += quitButton_Click;
            grid.Cells[2,0].Add(exitButton);
        }

        public override void HandleInput(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.DrawString(Font, "Select Encounter", new Vector2(600, 200), Color.White, 0.1f);

            if (encounterListBox.SelectedItem != null)
            {
                var encounter = encounterListBox.SelectedItem;

                SpriteBatch.DrawString(Font, "Maximum heroes: " + encounter.HeroLimit.ToString(), new Vector2(600, 510), Color.White, 0.1f);
                SpriteBatch.DrawString(Font, "Item level: " + encounter.ItemLevel.ToString(), new Vector2(600, 535), Color.White, 0.1f);
            }
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
