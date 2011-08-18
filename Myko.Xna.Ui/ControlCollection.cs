using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myko.Xna.Ui
{
    public class ControlCollection: IList<Control>
    {
        Control owner;
        private List<Control> list;

        public ControlCollection(Control owner)
        {
            this.owner = owner;
            this.list = new List<Control>();
        }

        public void ForEach(Action<Control> action)
        {
            list.ForEach(action);
        }

        #region IList<Control> Members

        public int IndexOf(Control item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, Control item)
        {
            item.Parent = owner;
            list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            list[index].Parent = null;
            list.RemoveAt(index);
        }

        public Control this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                value.Parent = owner;
                list[index] = value;
            }
        }

        #endregion

        #region ICollection<Control> Members

        public void Add(Control item)
        {
            item.Parent = owner;
            list.Add(item);
        }

        public void Clear()
        {
            list.ForEach(control => control.Parent = null);
            list.Clear();
        }

        public bool Contains(Control item)
        {
            return list.Contains(item);
        }

        public void CopyTo(Control[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<Control>)list).IsReadOnly; }
        }

        public bool Remove(Control item)
        {
            if (list.Contains(item))
                item.Parent = null;
            return list.Remove(item);
        }

        #endregion

        #region IEnumerable<Control> Members

        public IEnumerator<Control> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)list).GetEnumerator();
        }

        #endregion
    }
}
