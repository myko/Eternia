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
        }

        public override void LoadContent()
        {
            base.LoadContent();

            smallFont = ContentManager.Load<SpriteFont>(@"Fonts\KootenaySmall");

            equipmentListBox = AddListBox<Item>(Controls, new Vector2(Width / 2 - 350, 250), 300, 200);
            equipmentListBox.ZIndex = 0.2f;
            equipmentListBox.Font = smallFont;
            UpdateEquipmentList();

            inventoryListBox = AddListBox<Item>(Controls, new Vector2(Width / 2 + 50, 250), 300, 200);
            inventoryListBox.ZIndex = 0.2f;
            inventoryListBox.Font = smallFont;
            UpdateInventoryList();
            Controls.Add(inventoryListBox);

            var nextActorButton = CreateButton(">", new Vector2(100, 20));
            nextActorButton.Click += nextActorButton_Click;
            Controls.Add(nextActorButton);

            var unequipButton = CreateButton("Unequip", new Vector2(Width / 2 - 275, 500));
            unequipButton.Click += unequipButton_Click;
            Controls.Add(unequipButton);

            var equipButton = CreateButton("Equip", new Vector2(Width / 2 + 125, 500));
            equipButton.Click += equipButton_Click;
            Controls.Add(equipButton);

            var deleteButton = CreateButton("Delete", new Vector2(Width / 2 + 125, 550));
            deleteButton.Click += deleteButton_Click;
            Controls.Add(deleteButton);

            var okButton = CreateButton("Close", new Vector2(Width / 2 - 75, Height - 200));
            okButton.Click += okButton_Click;
            Controls.Add(okButton);
        }

        public override void HandleInput(GameTime gameTime)
        {
            base.HandleInput(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.DrawString(Font, currentActor.Name, new Vector2(100, 100), Color.White, 0.1f);

            VictoryScreen.DrawStatistics(SpriteBatch, smallFont, currentActor.CurrentStatistics, 100, 250, false);
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
                inventoryListBox.Items.Add(item, new ItemTooltip(item) { ShowZeroValues = false, ShowUpgrade = true, Upgrade = currentActor.GetItemUpgrade(item) });
            });
        }

        private void UpdateEquipmentList()
        {
            equipmentListBox.Items.Clear();
            currentActor.Equipment.ForEach(item =>
            {
                equipmentListBox.Items.Add(item, new ItemTooltip(item) { ShowZeroValues = false });
            });
        }
    }
}
