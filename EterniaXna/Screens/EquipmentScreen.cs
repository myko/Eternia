using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;
using System.Collections.Generic;

namespace EterniaXna.Screens
{
    public class EquipmentScreen: MenuScreen
    {
        private readonly Player player;
        private readonly List<Actor> actors;
        private Actor currentActor;

        private SpriteFont smallFont;
        private ListBox<Item> equipmentListBox;
        private ListBox<Item> inventoryListBox;

        public EquipmentScreen(Player player, IEnumerable<Actor> actors, Actor actor)
        {
            this.player = player;
            this.actors = new List<Actor>(actors);
            this.currentActor = actor;

            player.Inventory.Sort((i1, i2) => i1.ArmorClass.CompareTo(i2.ArmorClass));
        }

        public override void LoadContent()
        {
            base.LoadContent();

            smallFont = ContentManager.Load<SpriteFont>(@"Fonts\KootenaySmall");

            var grid = new Grid();
            grid.Width = Width;
            grid.Height = Height;
            grid.Rows.Add(GridSize.Fixed(80));
            grid.Rows.Add(GridSize.Fill());
            grid.Rows.Add(GridSize.Fixed(60));
            grid.Rows.Add(GridSize.Fixed(60));
            grid.Rows.Add(GridSize.Fixed(120));
            grid.Columns.Add(GridSize.Fill());
            grid.Columns.Add(GridSize.Fill());
            grid.Columns.Add(GridSize.Fill());
            Controls.Add(grid);

            grid.Cells[0, 1].Add(new Label { Text = Bind(() => currentActor.Name) });
            grid.Cells[1, 0].Add(new Label { Font = smallFont, Foreground = Color.LightGreen, Text = Bind(() => VictoryScreen.GetStatisticsString(currentActor.CurrentStatistics, false)) });

            equipmentListBox = AddListBox<Item>(grid.Cells[1, 1], Vector2.Zero, 300, 250);
            equipmentListBox.ZIndex = 0.2f;
            equipmentListBox.Font = smallFont;
            UpdateEquipmentList();

            inventoryListBox = AddListBox<Item>(grid.Cells[1, 2], Vector2.Zero, 300, 250);
            inventoryListBox.ZIndex = 0.2f;
            inventoryListBox.Font = smallFont;
            UpdateInventoryList();

            var prevActorButton = CreateButton("<", Vector2.Zero);
            prevActorButton.Click += prevActorButton_Click;
            grid.Cells[0, 0].Add(prevActorButton);

            var nextActorButton = CreateButton(">", Vector2.Zero);
            nextActorButton.Click += nextActorButton_Click;
            grid.Cells[0, 2].Add(nextActorButton);

            var unequipButton = CreateButton("Unequip", Vector2.Zero);
            unequipButton.Click += unequipButton_Click;
            grid.Cells[2, 1].Add(unequipButton);

            var equipButton = CreateButton("Equip", Vector2.Zero);
            equipButton.Click += equipButton_Click;
            grid.Cells[2, 2].Add(equipButton);

            var deleteButton = CreateButton("Delete", Vector2.Zero);
            deleteButton.Click += deleteButton_Click;
            grid.Cells[3, 2].Add(deleteButton);

            var okButton = CreateButton("Close", Vector2.Zero);
            okButton.Click += okButton_Click;
            grid.Cells[4, 2].Add(okButton);
        }

        private void prevActorButton_Click(object sender, System.EventArgs e)
        {
            var index = actors.IndexOf(currentActor);
            index = (index - 1 + actors.Count) % actors.Count;
            currentActor = actors[index];

            UpdateEquipmentList();
            UpdateInventoryList();
        }

        private void nextActorButton_Click(object sender, System.EventArgs e)
        {
            var index = actors.IndexOf(currentActor);
            index = (index + 1) % actors.Count;
            currentActor = actors[index];

            UpdateEquipmentList();
            UpdateInventoryList();
        }

        private void equipButton_Click(object sender, System.EventArgs e)
        {
            if (inventoryListBox.SelectedItem != null)
                currentActor.Equip(player, inventoryListBox.SelectedItem);

            UpdateEquipmentList();
            UpdateInventoryList();
        }

        private void unequipButton_Click(object sender, System.EventArgs e)
        {
            if (equipmentListBox.SelectedItem != null)
                currentActor.Unequip(player, equipmentListBox.SelectedItem);

            UpdateEquipmentList();
            UpdateInventoryList();
        }

        private void deleteButton_Click(object sender, System.EventArgs e)
        {
            if (inventoryListBox.SelectedItem != null)
                player.Inventory.Remove(inventoryListBox.SelectedItem);

            UpdateEquipmentList();
            UpdateInventoryList();
        }

        private void okButton_Click(object sender, System.EventArgs e)
        {
            ScreenManager.RemoveScreen(this);
        }

        private void UpdateInventoryList()
        {
            inventoryListBox.Items.Clear();
            player.Inventory.ForEach(item =>
            {
                inventoryListBox.Items.Add(
                    item, 
                    new ItemTooltip(item) { ShowZeroValues = false, ShowUpgrade = true, Upgrade = currentActor.GetItemUpgrade(item) }, 
                    ItemTooltip.GetItemColor(item.Rarity));
            });
        }

        private void UpdateEquipmentList()
        {
            equipmentListBox.Items.Clear();
            currentActor.Equipment.ForEach(item =>
            {
                equipmentListBox.Items.Add(
                    item, 
                    new ItemTooltip(item) { ShowZeroValues = false }, 
                    ItemTooltip.GetItemColor(item.Rarity));
            });
        }
    }
}
