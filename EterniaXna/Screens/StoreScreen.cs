using System.Collections.Generic;
using System.Linq;
using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;

namespace EterniaXna.Screens
{
    public class StoreScreen: MenuScreen
    {
        private readonly Player player;

        private SpriteFont smallFont;
        private ListBox<Actor> heroesListBox;
        private ListBox<TargetingStrategy> targetingStrategiesListBox;

        public StoreScreen(Player player)
        {
            this.player = player;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            smallFont = ContentManager.Load<SpriteFont>(@"Fonts\KootenaySmall");

            var grid = new Grid();
            grid.Width = Width;
            grid.Height = Height;
            grid.Rows.Add(GridSize.Fixed(80));
            grid.Rows.Add(GridSize.Fixed(40));
            grid.Rows.Add(GridSize.Fill());
            grid.Rows.Add(GridSize.Fixed(60));
            grid.Rows.Add(GridSize.Fixed(120));
            grid.Columns.Add(GridSize.Fill());
            grid.Columns.Add(GridSize.Fill());
            grid.Columns.Add(GridSize.Fill());
            Controls.Add(grid);

            grid.Cells[0, 1].Add(new Label { Text = "Store" });
            grid.Cells[1, 1].Add(new Label { Text = Bind(() => "Gold: " + player.Gold) });

            heroesListBox = AddListBox<Actor>(grid.Cells[2, 0], Vector2.Zero, 300, 250);
            heroesListBox.ZIndex = 0.2f;
            //heroesListBox.Font = smallFont;
            UpdateHeroList();

            targetingStrategiesListBox = AddListBox<TargetingStrategy>(grid.Cells[2, 2], Vector2.Zero, 300, 250);
            targetingStrategiesListBox.ZIndex = 0.2f;
            //targetingStrategiesListBox.Font = smallFont;
            UpdateTargetingStrategyList();

            var buyHeroButton = CreateButton("Buy", Vector2.Zero);
            buyHeroButton.Click += buyHeroButton_Click;
            grid.Cells[3, 0].Add(buyHeroButton);

            var buyTargetingStrategyButton = CreateButton("Buy", Vector2.Zero);
            buyTargetingStrategyButton.Click += buyTargetingStrategyButton_Click;
            grid.Cells[3, 2].Add(buyTargetingStrategyButton);

            var okButton = CreateButton("Back", Vector2.Zero);
            okButton.Click += okButton_Click;
            grid.Cells[4, 1].Add(okButton);
        }

        private void buyHeroButton_Click(object sender, System.EventArgs e)
        {
            if (heroesListBox.SelectedItem != null)
                if (player.Gold >= heroesListBox.SelectedItem.Cost)
                {
                    player.Gold -= heroesListBox.SelectedItem.Cost;
                    player.Heroes.Add(heroesListBox.SelectedItem);
                }
        }

        private void buyTargetingStrategyButton_Click(object sender, System.EventArgs e)
        {
            if (targetingStrategiesListBox.SelectedItem != null)
                if (player.Gold >= targetingStrategiesListBox.SelectedItem.Cost)
                {
                    player.Gold -= targetingStrategiesListBox.SelectedItem.Cost;
                    player.UnlockedTargetingStrategies.Add(targetingStrategiesListBox.SelectedItem.Value);
                }
        }

        private void okButton_Click(object sender, System.EventArgs e)
        {
            ScreenManager.RemoveScreen(this);
        }

        private void UpdateHeroList()
        {
            heroesListBox.Items.Clear();

            var heroes = new List<Actor>();
            heroes.Add(ContentManager.Load<Actor>(@"Actors\He-Man"));
            heroes.Add(ContentManager.Load<Actor>(@"Actors\Man-at-Arms"));
            heroes.Add(ContentManager.Load<Actor>(@"Actors\Teela"));
            heroes.Add(ContentManager.Load<Actor>(@"Actors\Stratos"));

            foreach (var hero in heroes)
            {
                var h = hero;
                heroesListBox.Items.Add(hero, null, Bind(() => GetHeroColor(h)));
            }
        }

        private Color GetHeroColor(Actor hero)
        {
            if (player.Heroes.Any(x => x.Name == hero.Name))
                return Color.LightGreen;

            if (hero.Cost > player.Gold)
                return Color.Salmon;

            return Color.LightGray;
        }

        private void UpdateTargetingStrategyList()
        {
            targetingStrategiesListBox.Items.Clear();
            var strategies = TargetingStrategy.All();

            foreach (var strategy in strategies)
            {
                var s = strategy;
                targetingStrategiesListBox.Items.Add(s, null, Bind(() => GetTargetingStrategyColor(s)));
            }
        }

        private Color GetTargetingStrategyColor(TargetingStrategy strategy)
        {
            if (player.UnlockedTargetingStrategies.Contains(strategy.Value))
                return Color.LightGreen;

            if (strategy.Cost > player.Gold)
                return Color.Salmon;

            return Color.LightGray;
        }
    }
}
