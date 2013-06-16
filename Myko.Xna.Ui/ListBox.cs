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
        public Binding<Color> Foreground { get; set; }
    }

    public class ListItemCollection<T> : IList<T> where T : class
    {
        private readonly ListBox<T> listBox;
        private readonly List<ListItem<T>> items;

        public ListItemCollection(ListBox<T> listBox, List<ListItem<T>> items)
        {
            this.listBox = listBox;
            this.items = items;
        }

        public void AddRange(IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                Add(item);
            }
        }

        public void Add(T item, object tooltip)
        {
            Add(item, tooltip, new Binding<Color>(() => listBox.Foreground));
        }

        public void Add(T item, object tooltip, Binding<Color> foreground)
        {
            items.Add(new ListItem<T>() { Value = item, Tooltip = tooltip, Foreground = foreground });
        }

        #region IList<T> Members

        public int IndexOf(T item)
        {
            return items.FindIndex(x => x.Value == item);
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
            Add(item, null);
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
            if (listBox.SelectedItem != null && listBox.SelectedItem == item)
            {
                var index = IndexOf(listBox.SelectedItem);

                if (items.RemoveAll(li => li.Value == item) > 0)
                {
                    if (items.Any())
                    {
                        while (index >= items.Count)
                            index--;

                        listBox.SelectedItem = items[index].Value;
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }

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

    public abstract class ListBoxBase<T>: Control where T: class
    {
        protected List<ListItem<T>> items;
        private ListItem<T> pressedItem;
        private ListItem<T> mouseOverItem;

        public IEnumerable<T> CheckedItems { get { return items.Where(li => li.Checked).Select(li => li.Value); } }
        public T SelectedItem { get; set; }

        public int SelectedIndex
        {
            get { return items.FindIndex(x => x.Value == SelectedItem); }
            set
            {
                var index = Math.Min(value, items.Count - 1);
                if (index >= 0)
                    SelectedItem = items[index].Value;
                else
                    SelectedItem = null;
            }
        }

        public bool EnableCheckBoxes { get; set; }
        public int ItemHeight { get; set; }
        public int ItemSpacing { get; set; }

        private int scrollOffset;
        private bool draggingScrollBar;

        public ListBoxBase()
        {
            items = new List<ListItem<T>>();
            Width = 150;
            Height = 200;
            Background = Color.Black;
            Foreground = Color.LightGray;
            ItemSpacing = 16;
        }

        public override void HandleInput(Vector2 position, GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            mouseOverItem = null;
            int visibleItems = Math.Min(items.Count, (int)ActualHeight / (ItemHeight + ItemSpacing));

            if (mouseState.X > position.X + ActualWidth - 10 && mouseState.X < position.X + ActualWidth && mouseState.Y > position.Y && mouseState.Y < position.Y + ActualHeight)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                    draggingScrollBar = true;
            }

            if (mouseState.LeftButton == ButtonState.Released)
                draggingScrollBar = false;

            if (!draggingScrollBar)
            {
                if (mouseState.X > position.X &&
                    mouseState.X < position.X + ActualWidth - 10 &&
                    mouseState.Y > position.Y &&
                    mouseState.Y < position.Y + (ItemHeight + ItemSpacing) * visibleItems)
                {
                    mouseOverItem = items[(int)((mouseState.Y - position.Y) / (ItemHeight + ItemSpacing)) + scrollOffset];
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
            }
            
            if (draggingScrollBar)
            {
                var itemScrollBarHeight = ActualHeight / items.Count;
                var pixelOffset = (mouseState.Y - position.Y - itemScrollBarHeight * visibleItems / 2) / ActualHeight;
                scrollOffset = Math.Max(0, Math.Min((int)(pixelOffset * items.Count), items.Count - visibleItems));
            }

            base.HandleInput(position, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (ItemHeight == 0)
                ItemHeight = Font.LineSpacing;
            
            if (items.Find(li => li.Value == SelectedItem) == null)
                SelectedItem = null;

            int visibleItems = Math.Min(items.Count, (int)ActualHeight / (ItemHeight + ItemSpacing));
            if (scrollOffset + visibleItems > items.Count)
                scrollOffset = items.Count - visibleItems;

            base.Update(gameTime);
        }

        public override void Draw(Vector2 position, GameTime gameTime)
        {
            DrawBackground(position);

            int visibleItems = (int)ActualHeight / (ItemHeight + ItemSpacing);

            for (int i = scrollOffset; i < items.Count && i < scrollOffset + visibleItems; i++)
            {
                var isMouseOver = items[i] == mouseOverItem;
                var isSelected = items[i].Value == SelectedItem;
                var isChecked = EnableCheckBoxes && items[i].Checked;

                var text = items[i].Value.ToString();
                var itemPosition = new Vector2(position.X, position.Y + (i - scrollOffset) * (ItemHeight + ItemSpacing));

                var color = items[i].Foreground.GetValue();
                var markerColor = Color.Gray;
                if (isSelected)
                {
                    color = color * 1.5f;
                    markerColor = Color.LightGray;
                }
                else if (isMouseOver)
                {
                    color = color * 1.25f;
                }
                                
                SpriteBatch.Draw(BlankTexture, new Rectangle((int)itemPosition.X, (int)itemPosition.Y, isChecked ? 20 : 10, ItemHeight + ItemSpacing / 2), markerColor * 0.25f, ZIndex + 0.01f);

                if (isMouseOver || isSelected)
                    SpriteBatch.Draw(BlankTexture, new Rectangle((int)itemPosition.X + (isChecked ? 26 : 16), (int)itemPosition.Y, (int)ActualWidth, ItemHeight + ItemSpacing / 2), markerColor * 0.25f, ZIndex + 0.01f);

                if (isChecked)
                    itemPosition = new Vector2(itemPosition.X + 10, itemPosition.Y);

                SpriteBatch.DrawString(Font, text, new Vector2(itemPosition.X + 16 + ItemSpacing / 4, itemPosition.Y + ItemSpacing / 4), color, ZIndex + 0.02f);
            }

            if (visibleItems < items.Count)
            {
                var itemScrollBarHeight = ActualHeight / items.Count;
                if (draggingScrollBar)
                    SpriteBatch.Draw(BlankTexture, new Rectangle((int)(position.X + ActualWidth - 10), (int)(position.Y + scrollOffset * itemScrollBarHeight), 10, (int)(itemScrollBarHeight * visibleItems)), Color.White, ZIndex + 0.02f);
                else
                    SpriteBatch.Draw(BlankTexture, new Rectangle((int)(position.X + ActualWidth - 10), (int)(position.Y + scrollOffset * itemScrollBarHeight), 10, (int)(itemScrollBarHeight * visibleItems)), Foreground, ZIndex + 0.02f);
            }

            base.Draw(position, gameTime);
        }

        public ListItem<T> GetItemContainer(T item)
        {
            return items.FirstOrDefault(x => x.Value == item);
        }

        public void CheckAllItems()
        {
            foreach (var item in items)
            {
                item.Checked = true;
            }
        }
    }

    public class ListBox<T>: ListBoxBase<T> where T: class
    {
        public ListItemCollection<T> Items { get { return new ListItemCollection<T>(this, items); } }

        public ListBox()
        {
        }
    }

    public class BoundListBox<T> : ListBoxBase<T> where T: class
    {
        public Binding<IEnumerable<T>> Source { get; set; }
        public Func<T, object> ToolTipBinder { get; set; }
        public Func<T, Binding<Color>> ColorBinder { get; set; }

        public BoundListBox()
        {
            ToolTipBinder = x => null;
            ColorBinder = x => Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            var source = Source.GetValue();
            if (source != null)
            {
                var selectedIndex = SelectedIndex;

                items.RemoveAll(x => !source.Contains(x.Value));
                for (int i = 0; i < source.Count(); i++)
                {
                    if (i < items.Count && items[i].Value == source.ElementAt(i))
                        continue;
                    else
                    {
                        var sourceItem = source.ElementAt(i);
                        items.Insert(i, new ListItem<T>() { Value = sourceItem, Tooltip = ToolTipBinder(sourceItem), Foreground = ColorBinder(sourceItem) });
                    }
                }

                if (items.Any())
                {
                    while (selectedIndex >= items.Count)
                        selectedIndex--;

                    SelectedIndex = selectedIndex;
                }
            }
            else
                items.Clear();

            base.Update(gameTime);
        }
    }
}
