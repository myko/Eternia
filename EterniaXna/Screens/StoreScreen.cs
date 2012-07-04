﻿using System.Collections.Generic;
using System.Linq;
using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;
using EterniaGame.Actors;
using EterniaGame.Abilities;
using System.Text;

namespace EterniaXna.Screens
{
    public class StoreScreen: MenuScreen
    {
        private readonly Player player;
        private SpriteFont smallFont;

        private List<Actor> availableHeroes;
        private List<Ability> availableAbilities;
        private List<Item> availableItems;

        public StoreScreen(Player player)
        {
            this.player = player;

            availableHeroes = GenerateAvailableHeroes().OrderBy(x => x.Name).ToList();
            availableAbilities = GenerateAvailableAbilities().OrderBy(x => x.Name).ToList();
            availableItems = GenerateAvailableItems().OrderBy(x => x.ArmorClass).OrderBy(x => x.Rarity).ToList();
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
            grid.Rows.Add(GridSize.Fixed(120));
            grid.Columns.Add(GridSize.Fill());
            Controls.Add(grid);

            grid.Cells[0, 0].Add(new Label { Text = "Store" });
            grid.Cells[1, 0].Add(new Label { Text = Bind(() => "Gold: " + player.Gold) });

            var tabControl = new TabControl();
            tabControl.Width = Width - 40;
            tabControl.Height = Height - 80 - 40 - 120 - 40;
            tabControl.AddPage("Heroes", BuildHeroesTabPage());
            tabControl.AddPage("Skills", BuildAbilitiesTabPage());
            tabControl.AddPage("Items", BuildItemsTabPage());
            tabControl.AddPage("Tactics", BuildTacticsTabPage());
            grid.Cells[2, 0].Add(tabControl);

            //equipmentListBox = AddListBox<Item>(grid.Cells[3, 1], new Vector2(0, -160), 300, 120);
            //equipmentListBox.ZIndex = 0.2f;
            //equipmentListBox.Font = smallFont;

            //abilitiesListBox = AddListBox<Ability>(grid.Cells[3, 1], new Vector2(0, -40), 300, 120);
            //abilitiesListBox.ZIndex = 0.2f;
            //abilitiesListBox.Font = smallFont;

            //targetingStrategiesListBox = AddListBox<TargetingStrategy>(grid.Cells[2, 2], Vector2.Zero, 300, 250);
            //targetingStrategiesListBox.ZIndex = 0.2f;
            //UpdateTargetingStrategyList();

            //var buyTargetingStrategyButton = CreateButton("Buy", Vector2.Zero);
            //buyTargetingStrategyButton.Click += buyTargetingStrategyButton_Click;
            //grid.Cells[3, 2].Add(buyTargetingStrategyButton);

            var okButton = CreateButton("Close", Vector2.Zero);
            okButton.Click += okButton_Click;
            grid.Cells[3, 0].Add(okButton);
        }

        private Control BuildHeroesTabPage()
        {
            var grid = new Grid();
            grid.Width = Width - 40;
            grid.Height = Height - 80 - 40 - 120 - 40 - 20;
            grid.Rows.Add(GridSize.Fixed(60));
            grid.Rows.Add(GridSize.Fill());
            grid.Rows.Add(GridSize.Fixed(100));
            grid.Columns.Add(GridSize.Fill());
            grid.Columns.Add(GridSize.Fill());
            grid.Columns.Add(GridSize.Fill());
            grid.Columns.Add(GridSize.Fill());

            grid.Cells[0, 0].Add(new Label { Text = "Current Heroes" });
            grid.Cells[0, 3].Add(new Label { Text = "Available Heroes" });

            var currentHeroesListBox = AddBoundListBox<Actor>(grid.Cells[1, 0], Vector2.Zero, 300, 400);
            currentHeroesListBox.ZIndex = 0.2f;
            currentHeroesListBox.Source = player.Heroes;

            var availableHeroesListBox = AddBoundListBox<Actor>(grid.Cells[1, 3], Vector2.Zero, 300, 400);
            availableHeroesListBox.ZIndex = 0.2f;
            availableHeroesListBox.Source = availableHeroes;
            availableHeroesListBox.ColorBinder = x => Bind(() => GetHeroColor(x));

            grid.Cells[1, 1].Add(new Label { Text = Bind(() => GetHeroStatistics(currentHeroesListBox)) });
            grid.Cells[1, 2].Add(new Label { Text = Bind(() => GetHeroStatistics(availableHeroesListBox )) });

            var sellHeroButton = CreateButton("Sell Hero", Vector2.Zero);
            sellHeroButton.Click += () => SellHero(currentHeroesListBox.SelectedItem);
            grid.Cells[2, 0].Add(sellHeroButton);

            var buyHeroButton = CreateButton("Buy Hero", Vector2.Zero);
            buyHeroButton.Click += () => BuyHero(availableHeroesListBox.SelectedItem);
            grid.Cells[2, 3].Add(buyHeroButton);

            return grid;
        }

        private Control BuildAbilitiesTabPage()
        {
            var grid = new Grid();
            grid.Width = Width - 40;
            grid.Height = Height - 80 - 40 - 120 - 40 - 20;
            grid.Rows.Add(GridSize.Fill());
            grid.Rows.Add(GridSize.Fixed(100));
            grid.Columns.Add(GridSize.Fill());
            grid.Columns.Add(GridSize.Fill());
            grid.Columns.Add(GridSize.Fill());

            var currentHeroesListBox = AddBoundListBox<Actor>(grid.Cells[0, 0], Vector2.Zero, 300, 400);
            currentHeroesListBox.ZIndex = 0.2f;
            currentHeroesListBox.Source = player.Heroes;

            var currentHeroAbilitiesListBox = AddBoundListBox<Ability>(grid.Cells[0, 1], Vector2.Zero, 300, 400);
            currentHeroAbilitiesListBox.Font = smallFont;
            currentHeroAbilitiesListBox.ZIndex = 0.2f;
            currentHeroAbilitiesListBox.Source = Bind<IEnumerable<Ability>>(() => currentHeroesListBox.SelectedItem != null ? currentHeroesListBox.SelectedItem.Abilities : null);
            currentHeroAbilitiesListBox.ToolTipBinder = x => new AbilityTooltip(currentHeroesListBox.SelectedItem, x) { Font = smallFont };
            currentHeroAbilitiesListBox.ColorBinder = x => Bind(() => GetAbilityColor(currentHeroesListBox.SelectedItem, x));

            var availableAbilitiesListBox = AddBoundListBox<Ability>(grid.Cells[0, 2], Vector2.Zero, 300, 400);
            availableAbilitiesListBox.Font = smallFont;
            availableAbilitiesListBox.ZIndex = 0.2f;
            availableAbilitiesListBox.Source = availableAbilities;
            availableAbilitiesListBox.ToolTipBinder = x => new AbilityTooltip(Bind(() => currentHeroesListBox.SelectedItem), x) { Font = smallFont };
            availableAbilitiesListBox.ColorBinder = x => Bind(() => GetAbilityColor(currentHeroesListBox.SelectedItem, x));

            var sellAbilityButton = CreateButton("Sell Skill", Vector2.Zero);
            sellAbilityButton.Click += () => SellAbility(currentHeroesListBox.SelectedItem, currentHeroAbilitiesListBox.SelectedItem);
            grid.Cells[1, 1].Add(sellAbilityButton);

            var buyAbilityButton = CreateButton("Buy Skill", Vector2.Zero);
            buyAbilityButton.Click += () => BuyAbility(currentHeroesListBox.SelectedItem, availableAbilitiesListBox.SelectedItem);
            grid.Cells[1, 2].Add(buyAbilityButton);

            return grid;
        }

        private Control BuildItemsTabPage()
        {
            var grid = new Grid();
            grid.Width = Width - 40;
            grid.Height = Height - 80 - 40 - 120 - 40 - 20;
            grid.Rows.Add(GridSize.Fill());
            grid.Rows.Add(GridSize.Fixed(100));
            grid.Columns.Add(GridSize.Fill());
            grid.Columns.Add(GridSize.Fill());
            grid.Columns.Add(GridSize.Fill());

            var currentHeroesListBox = AddBoundListBox<Actor>(grid.Cells[0, 0], Vector2.Zero, 300, 400);
            currentHeroesListBox.ZIndex = 0.2f;
            currentHeroesListBox.Source = player.Heroes;

            var currentItemsListBox = AddBoundListBox<Item>(grid.Cells[0, 1], Vector2.Zero, 300, 400);
            currentItemsListBox.ZIndex = 0.2f;
            currentItemsListBox.Source = player.Inventory;
            currentItemsListBox.ToolTipBinder = x => new ItemTooltip(x) { Font = smallFont, ShowUpgrade = false };
            currentItemsListBox.ColorBinder = x => Bind(() => ItemTooltip.GetItemColor(x.Rarity));

            var availableItemsListBox = AddBoundListBox<Item>(grid.Cells[0, 2], Vector2.Zero, 300, 400);
            availableItemsListBox.ZIndex = 0.2f;
            availableItemsListBox.Source = availableItems;
            availableItemsListBox.ToolTipBinder = x => new ItemTooltip(x) { Font = smallFont, ShowUpgrade = Bind(() => currentHeroesListBox.SelectedItem != null), Upgrade = Bind(() => currentHeroesListBox.SelectedItem.GetItemUpgrade(x)) };
            availableItemsListBox.ColorBinder = x => Bind(() => ItemTooltip.GetItemColor(x.Rarity));

            var sellItemButton = CreateButton("Sell Item", Vector2.Zero);
            sellItemButton.Click += () => SellItem(currentItemsListBox.SelectedItem);
            grid.Cells[1, 1].Add(sellItemButton);

            var buyItemButton = CreateButton("Buy Item", Vector2.Zero);
            buyItemButton.Click += () => BuyItem(availableItemsListBox.SelectedItem);
            grid.Cells[1, 2].Add(buyItemButton);

            return grid;
        }

        private Control BuildTacticsTabPage()
        {
            var grid = new Grid();
            grid.Width = Width - 40;
            grid.Height = Height - 80 - 40 - 120 - 40 - 20;
            grid.Rows.Add(GridSize.Fill());
            grid.Rows.Add(GridSize.Fixed(100));
            grid.Columns.Add(GridSize.Fill());
            grid.Columns.Add(GridSize.Fill());
            grid.Columns.Add(GridSize.Fill());

            var availableItemsListBox = AddBoundListBox<TargetingStrategy>(grid.Cells[0, 2], Vector2.Zero, 300, 400);
            availableItemsListBox.ZIndex = 0.2f;
            availableItemsListBox.Source = TargetingStrategy.All();
            //availableItemsListBox.ToolTipBinder = x => new ItemTooltip(x) { Font = smallFont, ShowUpgrade = Bind(() => currentHeroesListBox.SelectedItem != null), Upgrade = Bind(() => currentHeroesListBox.SelectedItem.GetItemUpgrade(x)) };
            availableItemsListBox.ColorBinder = x => Bind(() => GetTargetingStrategyColor(x));

            var buyItemButton = CreateButton("Buy Tactic", Vector2.Zero);
            buyItemButton.Click += () => BuyTargettingStrategy(availableItemsListBox.SelectedItem);
            grid.Cells[1, 2].Add(buyItemButton);

            return grid;
        }

        private void BuyHero(Actor hero)
        {
            if (hero != null && player.Gold >= hero.Cost)
            {
                player.Gold -= hero.Cost;
                player.Heroes.Add(hero);
                availableHeroes.Remove(hero);
            }
        }

        private void SellHero(Actor hero)
        {
            if (hero != null)
            {
                player.Gold += hero.Cost / 4;

                foreach (var item in hero.Equipment.ToArray())
                    hero.Unequip(player, item);

                player.Heroes.Remove(hero);
                availableHeroes.Add(hero);
            }
        }

        private void BuyAbility(Actor actor, Ability ability)
        {
            if (actor != null && ability != null)
            {
                //player.Gold -= actor.Cost;
                actor.Abilities.Add(ability);
                availableAbilities.Remove(ability);
            }
        }

        private void SellAbility(Actor actor, Ability ability)
        {
            if (actor != null && ability != null && actor.Abilities.Contains(ability))
            {
                //player.Gold += actor.Cost / 4;
                actor.Abilities.Remove(ability);
                availableAbilities.Add(ability);
            }
        }

        private void BuyItem(Item item)
        {
            if (item != null)
            {
                //player.Gold -= actor.Cost;
                player.Inventory.Add(item);
                availableItems.Remove(item);
            }
        }

        private void SellItem(Item item)
        {
            if (item != null)
            {
                //player.Gold += actor.Cost / 4;
                player.Inventory.Remove(item);
                availableItems.Add(item);
            }
        }

        private void BuyTargettingStrategy(TargetingStrategy item)
        {
            if (item != null)
            {
                //player.Gold -= item.Cost;
                player.UnlockedTargetingStrategies.Add(item.Value);
            }
        }

        private void buyTargetingStrategyButton_Click()
        {
            //if (targetingStrategiesListBox.SelectedItem != null)
            //    if (player.Gold >= targetingStrategiesListBox.SelectedItem.Cost)
            //    {
            //        player.Gold -= targetingStrategiesListBox.SelectedItem.Cost;
            //        player.UnlockedTargetingStrategies.Add(targetingStrategiesListBox.SelectedItem.Value);
            //    }
        }

        private void okButton_Click()
        {
            VictoryScreen.SaveActors(ScreenManager, player);
            ScreenManager.RemoveScreen(this);
        }

        private IEnumerable<Actor> GenerateAvailableHeroes()
        {
            var generator = new ActorGenerator(new Randomizer());
            for (int i = 0; i < 15; i++)
            {
                yield return generator.Generate();
            }
        }

        private IEnumerable<Ability> GenerateAvailableAbilities()
        {
            var generator = new AbilityGenerator(new Randomizer());
            for (int i = 0; i < 15; i++)
            {
                yield return generator.Generate();
            }
        }

        private IEnumerable<Item> GenerateAvailableItems()
        {
            var generator = new ItemGenerator(new Randomizer());
            for (int i = 0; i < 15; i++)
            {
                yield return generator.Generate(5);
            }
        }

        private Color GetHeroColor(Actor hero)
        {
            if (hero.Cost > player.Gold)
                return Color.Salmon;

            return Color.LightGray;
        }
        
        private Color GetAbilityColor(Actor actor, Ability ability)
        {
            if (actor != null)
            {
                if (ability.ManaCost != 0 && actor.ResourceType != ActorResourceTypes.Mana)
                    return Color.Salmon;

                if (ability.EnergyCost != 0 && actor.ResourceType != ActorResourceTypes.Energy)
                    return Color.Salmon;
            }

            return Color.LightGray;
        }

        private void UpdateTargetingStrategyList()
        {
            //targetingStrategiesListBox.Items.Clear();
            //var strategies = TargetingStrategy.All();

            //foreach (var strategy in strategies)
            //{
            //    var s = strategy;
            //    targetingStrategiesListBox.Items.Add(s, null, Bind(() => GetTargetingStrategyColor(s)));
            //}
        }

        private Color GetTargetingStrategyColor(TargetingStrategy strategy)
        {
            if (player.UnlockedTargetingStrategies.Contains(strategy.Value))
                return Color.LightGreen;

            if (strategy.Cost > player.Gold)
                return Color.Salmon;

            return Color.LightGray;
        }

        private string GetHeroStatistics(ListBoxBase<Actor> listBox)
        {
            var actor = listBox.SelectedItem;
            if (actor != null)
                return GetStatisticsString(actor.CurrentStatistics, actor.BaseModifiers, actor.ResourceType);
            else
                return "No hero selected.";
        }

        private string GetStatisticsString(Statistics statistics, Modifiers modifiers, ActorResourceTypes resourceType)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Health: " + statistics.Health.ToString("0"));
            
            if (resourceType == ActorResourceTypes.Mana)
                sb.AppendLine("Mana: " + statistics.Mana.ToString("0"));
            if (resourceType == ActorResourceTypes.Energy)
                sb.AppendLine("Energy: " + statistics.Energy.ToString("0"));

            sb.AppendLine();

            sb.AppendLine("Health modifier: " + modifiers.HealthModifier.ToString("0.00"));
            sb.AppendLine("Attack power modifier: " + modifiers.AttackPowerModifier.ToString("0.00"));
            sb.AppendLine("Spell power modifier: " + modifiers.SpellPowerModifier.ToString("0.00"));

            return sb.ToString();
        }

        private void UpdateEquipmentList(Actor actor)
        {
            //var index = equipmentListBox.SelectedIndex;

            //equipmentListBox.Items.Clear();

            //if (actor != null)
            //{
            //    actor.Equipment.ForEach(item =>
            //    {
            //        equipmentListBox.Items.Add(
            //            item,
            //            new ItemTooltip(item) { ShowZeroValues = false },
            //            ItemTooltip.GetItemColor(item.Rarity));
            //    });
            //}
            //equipmentListBox.SelectedIndex = index;
        }

        private void UpdateAbilityList(Actor actor)
        {
            //var index = abilitiesListBox.SelectedIndex;

            //abilitiesListBox.Items.Clear();

            //if (actor != null)
            //{
            //    actor.Abilities.ForEach(ability =>
            //    {
            //        abilitiesListBox.Items.Add(
            //            ability,
            //            new AbilityTooltip(actor, ability),
            //            Color.Yellow);
            //    });
            //}
            //abilitiesListBox.SelectedIndex = index;
        }
    }
}
