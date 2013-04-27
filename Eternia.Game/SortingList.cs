using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eternia.Game
{
    public class SortingList<T>: IList<T>
    {
        private List<T> innerList;
        private IComparer<T> comparer;
        private Comparison<T> comparison;

        public SortingList()
        {
            this.innerList = new List<T>();
        }

        public SortingList(IComparer<T> comparer) : this()
        {
            this.comparer = comparer;
        }

        public SortingList(Comparison<T> comparison) : this()
        {
            this.comparison = comparison;
        }

        private void ReSort()
        {
            if (comparison != null)
                innerList.Sort(comparison);
            else
                innerList.Sort(comparer);
        }

        public T Find(Predicate<T> match)
        {
            return innerList.Find(match);
        }

        public bool Exists(Predicate<T> match)
        {
            return innerList.Exists(match);
        }

        public int RemoveAll(Predicate<T> match)
        {
            return innerList.RemoveAll(match);
        }

        #region IList<T> Members

        public int IndexOf(T item)
        {
            ReSort();
            return innerList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            innerList.Insert(index, item);
            ReSort();
        }

        public void RemoveAt(int index)
        {
            innerList.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                ReSort();
                return innerList[index];
            }
            set
            {
                ReSort();
                innerList[index] = value;
                ReSort();
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            innerList.Add(item);
            ReSort();
        }

        public void Clear()
        {
            innerList.Clear();
        }

        public bool Contains(T item)
        {
            return innerList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ReSort();
            innerList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return innerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<T>)innerList).IsReadOnly; }
        }

        public bool Remove(T item)
        {
            return innerList.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            ReSort();
            return innerList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            ReSort();
            return ((System.Collections.IEnumerable)innerList).GetEnumerator();
        }

        #endregion
    }
}
