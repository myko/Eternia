using System;
using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;

namespace EterniaXna
{
    public class ItemTooltip: Control
    {
        private Item item;

        public bool ShowZeroValues { get; set; }
        public Binding<bool> ShowUpgrade { get; set; }
        public Binding<Statistics> Upgrade { get; set; }

        public ItemTooltip(Item item)
        {
            this.item = item;
            this.Upgrade = new Statistics();

            Width = 160;
            Height = 80;
        }

        public override void Draw(Vector2 position, GameTime gameTime)
        {
            int x = (int)position.X + 10;
            int y = (int)position.Y + 10;

            SpriteBatch.DrawString(Font, item.Name, new Vector2(x, y), GetItemColor(item.Rarity), ZIndex + 0.002f);
            SpriteBatch.DrawString(Font, item.Quality.ToString() + " " + item.ArmorClass.ToString() + " " + item.Slot.ToString(), new Vector2(x, y + Font.LineSpacing), Color.Gray, ZIndex + 0.002f);
            SpriteBatch.DrawString(Font, "Level " + item.Level + " " + item.Rarity.ToString(), new Vector2(x, y + 2 * Font.LineSpacing), Color.Gray, ZIndex + 0.002f);

            y += Font.LineSpacing * 3 + 10;
            y = DrawStatistics(item.Statistics, x, y, !ShowZeroValues);
            if (ShowUpgrade)
                y = DrawUpgradeStatistics(Upgrade, x, y + 10);

            Width = Math.Max(Width, Font.MeasureString(item.Name).X + 20);
            Height = y - position.Y + 10;

            var bounds = new Rectangle((int)position.X, (int)position.Y, (int)Width, (int)Height);
            var innerBounds = bounds;
            innerBounds.Inflate(-1, -1);

            SpriteBatch.Draw(BlankTexture, bounds, Color.White, ZIndex);
            SpriteBatch.Draw(BlankTexture, innerBounds, new Color(20, 20, 20), ZIndex + 0.001f);
        }

        public int DrawStatistics(Statistics statistics, int x, int y, bool hideZero)
        {
            y = DrawStatistic("Health", (int)statistics.Health, x, y, hideZero);
            y = DrawStatistic("Mana", (int)statistics.Mana, x, y, hideZero);
            y = DrawStatistic("Energy", (int)statistics.Energy, x, y, hideZero);
            y = DrawStatistic("Armor rating", statistics.ArmorRating, x, y, hideZero);
            y = DrawStatistic("Attack power", (int)statistics.AttackPower, x, y, hideZero);
            y = DrawStatistic("Spell power", (int)statistics.SpellPower, x, y, hideZero);
            y = DrawStatistic("Hit rating", statistics.HitRating, x, y, hideZero);
            y = DrawStatistic("Crit rating", statistics.CritRating, x, y, hideZero);
            y = DrawStatistic("Precision rating", statistics.PrecisionRating, x, y, hideZero);
            y = DrawStatistic("Dodge rating", statistics.DodgeRating, x, y, hideZero);
            y = DrawStatistic("Chance of extra rewards", statistics.ExtraRewards, x, y, hideZero);

            return y;
        }

        public int DrawUpgradeStatistics(Statistics statistics, int x, int y)
        {
            y = DrawUpgradeStatistic("Health", (int)statistics.Health, x, y);
            y = DrawUpgradeStatistic("Mana", (int)statistics.Mana, x, y);
            y = DrawUpgradeStatistic("Energy", (int)statistics.Energy, x, y);
            y = DrawUpgradeStatistic("Armor rating", statistics.ArmorRating, x, y);
            y = DrawUpgradeStatistic("Attack power", (int)statistics.AttackPower, x, y);
            y = DrawUpgradeStatistic("Spell power", (int)statistics.SpellPower, x, y);
            y = DrawUpgradeStatistic("Hit rating", statistics.HitRating, x, y);
            y = DrawUpgradeStatistic("Crit rating", statistics.CritRating, x, y);
            y = DrawUpgradeStatistic("Precision rating", statistics.PrecisionRating, x, y);
            y = DrawUpgradeStatistic("Dodge rating", statistics.DodgeRating, x, y);
            y = DrawUpgradeStatistic("Chance of extra rewards", statistics.ExtraRewards, x, y);

            return y;
        }

        private int DrawStatistic(string valueName, int value, int x, int y, bool hideZero)
        {
            if (hideZero && value == 0)
                return y;

            if (value > 0)
                SpriteBatch.DrawString(Font, "+" + value.ToString() + " " + valueName, new Vector2(x, y), Color.LightYellow, ZIndex + 0.003f);
            else
                SpriteBatch.DrawString(Font, value.ToString() + " " + valueName, new Vector2(x, y), Color.LightYellow, ZIndex + 0.003f);

            return y + Font.LineSpacing;
        }

        private int DrawUpgradeStatistic(string valueName, int value, int x, int y)
        {
            if (value == 0)
                return y;

            if (value > 0)
                SpriteBatch.DrawString(Font, valueName + ": +" + value.ToString(), new Vector2(x, y), Color.LightGreen, ZIndex + 0.003f);
            else
                SpriteBatch.DrawString(Font, valueName + ": " + value.ToString(), new Vector2(x, y), Color.Salmon, ZIndex + 0.003f);

            return y + Font.LineSpacing;
        }

        public static Color GetItemColor(ItemRarities rarity)
        {
            switch (rarity)
            {
                case ItemRarities.Common:
                    return Color.Gray;
                case ItemRarities.Uncommon:
                    return Color.White;
                case ItemRarities.Rare:
                    return Color.MediumAquamarine;
                case ItemRarities.Heroic:
                    return Color.CornflowerBlue;
                case ItemRarities.Epic:
                    return Color.MediumPurple;
                case ItemRarities.Legendary:
                    return Color.Orange;
            }

            return Color.White;
        }
    }
}
