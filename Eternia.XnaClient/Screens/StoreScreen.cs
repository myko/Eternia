﻿using System.Collections.Generic;
using System.Linq;
using Eternia.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;
using Eternia.Game.Actors;
using Eternia.Game.Abilities;
using System.Text;
using Eternia.Game.Stats;
using Eternia.Game.Items;
using Microsoft.Xna.Framework.Audio;

namespace EterniaXna.Screens
{
    public class StoreScreen: MenuScreen
    {
        private readonly Player player;
        private SpriteFont smallFont;

        private List<ActorDefinition> availableHeroes;
        private List<Ability> availableAbilities;
        private List<Item> availableItems;

        public StoreScreen(Player player)
        {
            this.player = player;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            smallFont = ContentManager.Load<SpriteFont>(@"Fonts\KootenaySmall");
            availableHeroes = GenerateAvailableHeroes().ToList();
            availableAbilities = GenerateAvailableAbilities().OrderBy(x => x.Name).ToList();
            availableItems = GenerateAvailableItems().OrderBy(x => x.ArmorClass).OrderBy(x => x.Rarity).ToList();

            var grid = new Grid();
            grid.Width = Width;
            grid.Height = Height;
            grid.Rows.Add(Size.Fixed(80));
            grid.Rows.Add(Size.Fixed(40));
            grid.Rows.Add(Size.Fill());
            grid.Rows.Add(Size.Fixed(120));
            grid.Columns.Add(Size.Fill());
            Controls.Add(grid);

            grid.Cells[0, 0].Add(new Label { Text = "Store" });
            grid.Cells[1, 0].Add(new Label { Text = Bind(() => "Gold: " + player.Gold) });

            var tabControl = new TabControl();
            tabControl.Width = ActualWidth - 40;
            tabControl.Height = ActualHeight - 80 - 40 - 120 - 40;
            tabControl.AddPage("Heroes", BuildHeroesTabPage());
            //tabControl.AddPage("Skills", BuildAbilitiesTabPage());
            tabControl.AddPage("Items", BuildItemsTabPage());
            //tabControl.AddPage("Tactics", BuildTacticsTabPage());
            grid.Cells[2, 0].Add(tabControl);

            var okButton = CreateButton("Close", Vector2.Zero);
            okButton.Click += okButton_Click;
            grid.Cells[3, 0].Add(okButton);
        }

        private Control BuildHeroesTabPage()
        {
            var grid = new Grid();
            grid.Width = ActualWidth - 40;
            grid.Height = ActualHeight - 80 - 40 - 120 - 40 - 20;
            grid.Rows.Add(Size.Fixed(60));
            grid.Rows.Add(Size.Fill());
            grid.Rows.Add(Size.Fixed(100));
            grid.Columns.Add(Size.Fill());
            grid.Columns.Add(Size.Fill());
            grid.Columns.Add(Size.Fill());
            grid.Columns.Add(Size.Fill());

            grid.Cells[0, 0].Add(new Label { Text = "Current Heroes" });
            grid.Cells[0, 3].Add(new Label { Text = "Available Heroes" });

            var currentHeroesListBox = AddBoundListBox<Actor>(grid.Cells[1, 0], Vector2.Zero, 300, 400);
            currentHeroesListBox.ZIndex = 0.2f;
            currentHeroesListBox.Source = player.Heroes;

            var availableHeroesListBox = AddBoundListBox<ActorDefinition>(grid.Cells[1, 3], Vector2.Zero, 300, 400);
            availableHeroesListBox.ZIndex = 0.2f;
            availableHeroesListBox.Source = availableHeroes;
            availableHeroesListBox.ColorBinder = x => Bind(() => GetHeroColor(x));

            grid.Cells[1, 1].Add(new Label { Text = Bind(() => GetHeroStatistics(currentHeroesListBox)) });
            grid.Cells[1, 2].Add(new Label { Text = Bind(() => GetHeroStatistics(availableHeroesListBox )) });

            var sellHeroButton = CreateButton("Sell Hero", Vector2.Zero);
            sellHeroButton.Sound = ContentManager.Load<SoundEffect>(@"Sounds\140380__d-w__coins-38");
            sellHeroButton.Click += () => SellHero(currentHeroesListBox.SelectedItem);
            grid.Cells[2, 0].Add(sellHeroButton);

            var buyHeroButton = CreateButton("Buy Hero", Vector2.Zero);
            buyHeroButton.Sound = ContentManager.Load<SoundEffect>(@"Sounds\140380__d-w__coins-38");
            buyHeroButton.Click += () => BuyHero(availableHeroesListBox.SelectedItem);
            grid.Cells[2, 3].Add(buyHeroButton);

            return grid;
        }

        private Control BuildAbilitiesTabPage()
        {
            var grid = new Grid();
            grid.Width = ActualWidth - 40;
            grid.Height = ActualHeight - 80 - 40 - 120 - 40 - 20;
            grid.Rows.Add(Size.Fill());
            grid.Rows.Add(Size.Fixed(100));
            grid.Columns.Add(Size.Fill());
            grid.Columns.Add(Size.Fill());
            grid.Columns.Add(Size.Fill());

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
            sellAbilityButton.Sound = ContentManager.Load<SoundEffect>(@"Sounds\140380__d-w__coins-38");
            sellAbilityButton.Click += () => SellAbility(currentHeroesListBox.SelectedItem, currentHeroAbilitiesListBox.SelectedItem);
            grid.Cells[1, 1].Add(sellAbilityButton);

            var buyAbilityButton = CreateButton("Buy Skill", Vector2.Zero);
            buyAbilityButton.Sound = ContentManager.Load<SoundEffect>(@"Sounds\140380__d-w__coins-38");
            buyAbilityButton.Click += () => BuyAbility(currentHeroesListBox.SelectedItem, availableAbilitiesListBox.SelectedItem);
            grid.Cells[1, 2].Add(buyAbilityButton);

            return grid;
        }

        private Control BuildItemsTabPage()
        {
            var grid = new Grid();
            grid.Width = ActualWidth - 40;
            grid.Height = ActualHeight - 80 - 40 - 120 - 40 - 20;
            grid.Rows.Add(Size.Fill());
            grid.Rows.Add(Size.Fixed(100));
            grid.Columns.Add(Size.Fill());
            grid.Columns.Add(Size.Fill());
            grid.Columns.Add(Size.Fill());

            var currentHeroesListBox = AddBoundListBox<Actor>(grid.Cells[0, 0], Vector2.Zero, 300, 400);
            currentHeroesListBox.ZIndex = 0.2f;
            currentHeroesListBox.Source = player.Heroes;

            var currentItemsListBox = AddBoundListBox<Item>(grid.Cells[0, 1], Vector2.Zero, 300, 400);
            currentItemsListBox.ZIndex = 0.2f;
            currentItemsListBox.Source = player.Inventory;
            currentItemsListBox.ToolTipBinder = x => new ItemTooltip(x) { Font = smallFont, ShowUpgrade = false };
            currentItemsListBox.ColorBinder = x => Bind(() => ItemTooltip.GetItemColor(x.Rarity));
            currentItemsListBox.Font = smallFont;

            var availableItemsListBox = AddBoundListBox<Item>(grid.Cells[0, 2], Vector2.Zero, 300, 400);
            availableItemsListBox.ZIndex = 0.2f;
            availableItemsListBox.Source = availableItems;
            availableItemsListBox.ToolTipBinder = x => new ItemTooltip(x) { Font = smallFont, ShowUpgrade = Bind(() => currentHeroesListBox.SelectedItem != null), Upgrade = Bind(() => currentHeroesListBox.SelectedItem.GetItemUpgrade(x)) };
            availableItemsListBox.ColorBinder = x => Bind(() => ItemTooltip.GetItemColor(x.Rarity));
            availableItemsListBox.Font = smallFont;

            var sellItemButton = CreateButton("Sell Item", Vector2.Zero);
            sellItemButton.Sound = ContentManager.Load<SoundEffect>(@"Sounds\140380__d-w__coins-38");
            sellItemButton.Click += () => SellItem(currentItemsListBox.SelectedItem);
            grid.Cells[1, 1].Add(sellItemButton);

            var buyItemButton = CreateButton("Buy Item", Vector2.Zero);
            buyItemButton.Sound = ContentManager.Load<SoundEffect>(@"Sounds\140380__d-w__coins-38");
            buyItemButton.Click += () => BuyItem(availableItemsListBox.SelectedItem);
            grid.Cells[1, 2].Add(buyItemButton);

            return grid;
        }

        private Control BuildTacticsTabPage()
        {
            var grid = new Grid();
            grid.Width = ActualWidth - 40;
            grid.Height = ActualHeight - 80 - 40 - 120 - 40 - 20;
            grid.Rows.Add(Size.Fill());
            grid.Rows.Add(Size.Fixed(100));
            grid.Columns.Add(Size.Fill());
            grid.Columns.Add(Size.Fill());
            grid.Columns.Add(Size.Fill());

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

        private void BuyHero(ActorDefinition hero)
        {
            if (hero != null && player.Gold >= hero.Cost)
            {
                player.Gold -= hero.Cost;
                player.Heroes.Add(new Actor(hero));
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
                //availableHeroes.Add(hero);
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

        private IEnumerable<ActorDefinition> GenerateAvailableHeroes()
        {
            yield return ContentManager.Load<ActorDefinition>(@"Actors\0_Warrior");
            yield return ContentManager.Load<ActorDefinition>(@"Actors\0_Cleric");
            yield return ContentManager.Load<ActorDefinition>(@"Actors\1_Rogue");
            yield return ContentManager.Load<ActorDefinition>(@"Actors\1_Ranger");
            yield return ContentManager.Load<ActorDefinition>(@"Actors\1_Wizard");
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
            var randomizer = new Randomizer();
            var generator = new ItemGenerator(new Randomizer());
            for (int i = 0; i < 15; i++)
            {
                yield return new Item(generator.Generate(5));
            }
        }

        private Color GetHeroColor(ActorDefinition hero)
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
                return GetStatisticsString(actor.CurrentStatistics, actor.ResourceType);
            else
                return "No hero selected.";
        }

        private string GetHeroStatistics(ListBoxBase<ActorDefinition> listBox)
        {
            var actor = listBox.SelectedItem;
            if (actor != null)
                return GetStatisticsString(actor.BaseStatistics, actor.ResourceType);
            else
                return "No hero selected.";
        }

        private string GetStatisticsString(Statistics statistics, ActorResourceTypes resourceType)
        {
            return string.Join("\n", statistics
                .OrderBy(x => x.Name)
                .Select(x => x.ToItemTooltipString())
                .ToArray());
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
