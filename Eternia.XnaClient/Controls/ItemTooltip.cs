using System;
using System.Linq;
using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;
using Eternia.Game.Stats;

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
            foreach (var stat in statistics.OrderBy(s => s.Name))
            {
                var text = stat.ToItemUpgradeString();

                SpriteBatch.DrawString(Font, text, new Vector2(x, y), Color.LightYellow, ZIndex + 0.003f);

                y += (int)Font.MeasureString(text).Y;
            }
            
            return y;
        }

        public int DrawUpgradeStatistics(Statistics statistics, int x, int y)
        {
            foreach (var stat in statistics.OrderBy(s => s.Name))
            {
                var text = stat.ToItemUpgradeString();

                SpriteBatch.DrawString(Font, text, new Vector2(x, y), stat.Color, ZIndex + 0.003f);

                y += (int)Font.MeasureString(text).Y;
            }

            return y;
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
