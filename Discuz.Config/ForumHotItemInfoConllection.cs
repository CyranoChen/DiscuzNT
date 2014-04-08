using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Discuz.Config
{
    public class ForumHotItemInfoConllection : CollectionBase
    {
        public ForumHotItemInfoConllection()
        {
        }

        public ForumHotItemInfoConllection(ForumHotItemInfoConllection value)
        {
            this.AddRange(value);
        }

        public ForumHotItemInfoConllection(ForumHotItemInfo[] value)
        {
            this.AddRange(value);
        }

        public ForumHotItemInfo this[int index]
        {
            get { return ((ForumHotItemInfo)(this.List[index])); }
        }

        public int Add(ForumHotItemInfo value)
        {
            return this.List.Add(value);
        }

        public void AddRange(ForumHotItemInfo[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }

        public void AddRange(ForumHotItemInfoConllection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add((ForumHotItemInfo)value.List[i]);
            }
        }

        public bool Contains(ForumHotItemInfo value)
        {
            return this.List.Contains(value);
        }

        public void CopyTo(ForumHotItemInfo[] array, int index)
        {
            this.List.CopyTo(array, index);
        }

        public int IndexOf(ForumHotItemInfo value)
        {
            return this.List.IndexOf(value);
        }

        public void Insert(int index, ForumHotItemInfo value)
        {
            List.Insert(index, value);
        }

        public void Remove(ForumHotItemInfo value)
        {
            List.Remove(value);
        }

        public new ForumHotItemInfoCollectionEnumerator GetEnumerator()
        {
            return new ForumHotItemInfoCollectionEnumerator(this);
        }
    }

    public class ForumHotItemInfoCollectionEnumerator : IEnumerator
    {
        private IEnumerator _enumerator;
        private IEnumerable _temp;


        public ForumHotItemInfoCollectionEnumerator(ForumHotItemInfoConllection mappings)
        {
            _temp = ((IEnumerable)(mappings));
            _enumerator = _temp.GetEnumerator();
        }

        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        public ForumHotItemInfo Current
        {
            get { return ((ForumHotItemInfo)(_enumerator.Current)); }
        }

        object IEnumerator.Current
        {
            get { return _enumerator.Current; }
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns><b>true</b> if the enumerator was successfully advanced to the next element; <b>false</b> if the enumerator has passed the end of the collection.</returns>
        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        bool IEnumerator.MoveNext()
        {
            return _enumerator.MoveNext();
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        public void Reset()
        {
            _enumerator.Reset();
        }

        void IEnumerator.Reset()
        {
            _enumerator.Reset();
        }
    }
}