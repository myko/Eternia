using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Myko.Xna.Ui
{
    public class TabPage
    {
        public string Title { get; set; }
        public Control Control { get; set; }
    }

    public class TabControl: Control
    {
        public List<TabPage> Pages { get; set; }
        public TabPage ActivePage { get; set; }

        public TabControl()
        {
            Pages = new List<TabPage>();
        }

        public void AddPage(string title, Control control)
        {
            control.Parent = this;
            var page = new TabPage { Title = title, Control = control };
            Pages.Add(page);
            if (ActivePage == null)
                ActivePage = page;
        }

        public override void HandleInput(Vector2 position, GameTime gameTime)
        {
            if (IsMouseOver && IsMouseDown)
            {
                var mouseState = Mouse.GetState();
                var point = new Vector2(mouseState.X - (Position + position).X, mouseState.Y - (Position + position).Y);
                if (point.Y < 20)
                {
                    var pageIndex = (int)(point.X / 100f);
                    if (pageIndex < Pages.Count)
                        ActivePage = Pages[pageIndex];
                }
            }

            if (ActivePage != null)
                ActivePage.Control.HandleInput(Position + position + new Vector2(0, 30), gameTime);

            base.HandleInput(position, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Update all pages or just the active page?

            if (ActivePage != null)
                ActivePage.Control.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(Vector2 position, GameTime gameTime)
        {
            for (int i = 0; i < Pages.Count; i++)
            {
                var page = Pages[i];

                if (ActivePage == page)
                    SpriteBatch.DrawString(Font, page.Title, Position + position + new Vector2(i * 100, 0), Color.White, ZIndex + 0.02f);
                else
                    SpriteBatch.DrawString(Font, page.Title, Position + position + new Vector2(i * 100, 0), Color.LightGray, ZIndex + 0.02f);
            }

            if (ActivePage != null)
                ActivePage.Control.Draw(Position + position + new Vector2(0, 30), gameTime);

            base.Draw(position, gameTime);
        }
    }
}
