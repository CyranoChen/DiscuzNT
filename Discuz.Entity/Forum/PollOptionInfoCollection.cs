using System;
using System.Collections;

namespace Discuz.Entity
{
    /// <summary>
    /// PollOptionInfoCollection 的摘要说明。
    /// </summary>
    public class PollOptionInfoCollection : CollectionBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PollOptionInfoCollection">PollOptionInfoCollection</see> class.
        /// </summary>
        public PollOptionInfoCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PollOptionInfoCollection">PollOptionInfoCollection</see> class containing the elements of the specified source collection.
        /// </summary>
        /// <param name="value">A <see cref="PollOptionInfoCollection">PollOptionInfoCollection</see> with which to initialize the collection.</param>
        public PollOptionInfoCollection(PollOptionInfoCollection value)
        {
            this.AddRange(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PollOptionInfoCollection">PollOptionInfoCollection</see> class containing the specified array of <see cref="PollOptionInfo">PollOptionInfo</see> Components.
        /// </summary>
        /// <param name="value">An array of <see cref="PollOptionInfo">PollOptionInfo</see> Components with which to initialize the collection. </param>
        public PollOptionInfoCollection(PollOptionInfo[] value)
        {
            this.AddRange(value);
        }

        /// <summary>
        /// Gets the <see cref="PollOptionInfoCollection">PollOptionInfoCollection</see> at the specified index in the collection.
        /// <para>
        /// In C#, this property is the indexer for the <see cref="PollOptionInfoCollection">PollOptionInfoCollection</see> class.
        /// </para>
        /// </summary>
        public PollOptionInfo this[int index]
        {
            get { return ((PollOptionInfo)(this.List[index])); }
        }

        public int Add(PollOptionInfo value)
        {
            return this.List.Add(value);
        }

        /// <summary>
        /// Copies the elements of the specified <see cref="PollOptionInfo">PollOptionInfo</see> array to the end of the collection.
        /// </summary>
        /// <param name="value">An array of type <see cref="PollOptionInfo">PollOptionInfo</see> containing the Components to add to the collection.</param>
        public void AddRange(PollOptionInfo[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }

        /// <summary>
        /// Adds the contents of another <see cref="PollOptionInfoCollection">PollOptionInfoCollection</see> to the end of the collection.
        /// </summary>
        /// <param name="value">A <see cref="PollOptionInfoCollection">PollOptionInfoCollection</see> containing the Components to add to the collection. </param>
        public void AddRange(PollOptionInfoCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add((PollOptionInfo)value.List[i]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified <see cref="PollOptionInfoCollection">PollOptionInfoCollection</see>.
        /// </summary>
        /// <param name="value">The <see cref="PollOptionInfoCollection">PollOptionInfoCollection</see> to search for in the collection.</param>
        /// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
        public bool Contains(PollOptionInfo value)
        {
            return this.List.Contains(value);
        }

        /// <summary>
        /// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
        /// <param name="index">The index of the array at which to begin inserting.</param>
        public void CopyTo(PollOptionInfo[] array, int index)
        {
            this.List.CopyTo(array, index);
        }

        /// <summary>
        /// Gets the index in the collection of the specified <see cref="PollOptionInfoCollection">PollOptionInfoCollection</see>, if it exists in the collection.
        /// </summary>
        /// <param name="value">The <see cref="PollOptionInfoCollection">PollOptionInfoCollection</see> to locate in the collection.</param>
        /// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
        public int IndexOf(PollOptionInfo value)
        {
            return this.List.IndexOf(value);
        }

        public void Insert(int index, PollOptionInfo value)
        {
            List.Insert(index, value);
        }

        public void Remove(PollOptionInfo value)
        {
            List.Remove(value);
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Returns an enumerator that can iterate through the <see cref="PollOptionInfoCollection">PollOptionInfoCollection</see> instance.
        /// </summary>
        /// <returns>An <see cref="PollOptionInfoCollectionEnumerator">PollOptionInfoCollectionEnumerator</see> for the <see cref="PollOptionInfoCollection">PollOptionInfoCollection</see> instance.</returns>
        public new PollOptionInfoCollectionEnumerator GetEnumerator()
        {
            return new PollOptionInfoCollectionEnumerator(this);
        }

        /// <summary>
        /// Supports a simple iteration over a <see cref="PollOptionInfoCollection">PollOptionInfoCollection</see>.
        /// </summary>
        public class PollOptionInfoCollectionEnumerator : IEnumerator
        {
            private IEnumerator _enumerator;
            private IEnumerable _temp;

            /// <summary>
            /// Initializes a new instance of the <see cref="PollOptionInfoCollectionEnumerator">PollOptionInfoCollectionEnumerator</see> class referencing the specified <see cref="PollOptionInfoCollection">PollOptionInfoCollection</see> object.
            /// </summary>
            /// <param name="mappings">The <see cref="PollOptionInfoCollection">PollOptionInfoCollection</see> to enumerate.</param>
            public PollOptionInfoCollectionEnumerator(PollOptionInfoCollection mappings)
            {
                _temp = ((IEnumerable)(mappings));
                _enumerator = _temp.GetEnumerator();
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            public PollOptionInfo Current
            {
                get { return ((PollOptionInfo)(_enumerator.Current)); }
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
}