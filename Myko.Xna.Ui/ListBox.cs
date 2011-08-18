using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Myko.Xna.Ui
{
    public class ListItem<T> where T: class
    {
        public T Value { get; set; }
        public bool Checked { get; set; }
        public object Tooltip { get; set; }
    }

    public class ListItemCollection<T> : IList<T> where T : class
    {
        private List<ListItem<T>> items;

        public ListItemCollection(List<ListItem<T>> items)
        {
            this.items = items;
        }

        public void AddRange(IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                items.Add(new ListItem<T>() { Value = item });
            }
        }

        public void Add(T item, object tooltip)
        {
            items.Add(new ListItem<T>() { Value = item, Tooltip = tooltip });
        }

        #region IList<T> Members

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public T this[int index]
        {
            get
            {
                return items[index].Value;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            items.Add(new ListItem<T>() { Value = item });
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return items.Count; }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(T item)
        {
            return items.RemoveAll(li => li.Value == item) > 0;
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return items.Select(x => x.Value).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class ListBox<T>: Control where T: class
    {
        private List<ListItem<T>> items;
        private ListItem<T> pressedItem;
        private ListItem<T> mouseOverItem;

        public ListItemCollection<T> Items { get { return new ListItemCollection<T>(items); } }
        public IEnumerable<T> CheckedItems { get { return items.Where(li => li.Checked).Select(li => li.Value); } }
        public T SelectedItem { get; set; }
        public bool EnableCheckBoxes { get; set; }

        public ListBox()
        {
            items = new List<ListItem<T>>();
            Width = 150;
            Height = 200;
            Background = Color.Black;
            Foreground = Color.LightGray;
        }

        public override void HandleInput(Vector2 position, GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            mouseOverItem = null;

            if (mouseState.X > position.X && 
                mouseState.X < position.X + Width && 
                mouseState.Y > position.Y && 
                mouseState.Y < position.Y + Font.LineSpacing * items.Count)
            {
                mouseOverItem = items[(int)((mouseState.Y - position.Y) / Font.LineSpacing)];
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (pressedItem == null)
                    pressedItem = mouseOverItem;

                if (mouseOverItem != null)
                    SelectedItem = mouseOverItem.Value;
            }

            if (mouseState.LeftButton == ButtonState.Released)
            {
                if (pressedItem != null && pressedItem == mouseOverItem)
                {
                    mouseOverItem.Checked = !mouseOverItem.Checked;
                }

                pressedItem = null;
            }

            if (mouseOverItem != null)
                Tooltip = mouseOverItem.Tooltip;
            else
                Tooltip = null;

            base.HandleInput(position, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (items.Find(li => li.Value == SelectedItem) == null)
                SelectedItem = null;

            base.Update(gameTime);
        }

        public override void Draw(Vector2 position, GameTime gameTime)
        {
            DrawBackground(position);

            for (int i = 0; i < items.Count; i++)
            {
                var text = items[i].Value.ToString();
                var itemPosition = new Vector2(position.X, position.Y + i * Font.LineSpacing);

                var color = Foreground;
                if (items[i].Value == SelectedItem)
                    color = Color.Red;
                else if (items[i] == mouseOverItem)
                    color = Color.White;

                if (EnableCheckBoxes)
                {
                    if (items[i].Checked)
                        SpriteBatch.DrawString(Font, "[X]", itemPosition, color, ZIndex + 0.01f);
                    else
                        SpriteBatch.DrawString(Font, "[ ]", itemPosition, color, ZIndex + 0.01f);

                    itemPosition = new Vector2(itemPosition.X + 24, itemPosition.Y);
                }

                SpriteBatch.DrawString(Font, text, itemPosition, color, ZIndex + 0.01f);
            }

            base.Draw(position, gameTime);
        }

        public ListItem<T> GetItemContainer(T item)
        {
            return items.FirstOrDefault(x => x.Value == item);
        }
    }
}
