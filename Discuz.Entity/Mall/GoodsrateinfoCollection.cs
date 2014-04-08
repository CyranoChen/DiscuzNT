using System;
using System.Collections;

namespace Discuz.Entity
{
    /// <summary>
    /// 商品评价信息集合类
    /// </summary>
    public class GoodsrateinfoCollection : CollectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see> class.
        /// </summary>
        public GoodsrateinfoCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see> class containing the elements of the specified source collection.
        /// </summary>
        /// <param name="value">A <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see> with which to initialize the collection.</param>
        public GoodsrateinfoCollection(GoodsrateinfoCollection value)
        {
            this.AddRange(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see> class containing the specified array of <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see> Components.
        /// </summary>
        /// <param name="value">An array of <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see> Components with which to initialize the collection. </param>
        public GoodsrateinfoCollection(Goodsrateinfo[] value)
        {
            this.AddRange(value);
        }

        /// <summary>
        /// Gets the <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see> at the specified index in the collection.
        /// <para>
        /// In C#, this property is the indexer for the <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see> class.
        /// </para>
        /// </summary>
        public Goodsrateinfo this[int index]
        {
            get { return ((Goodsrateinfo)(this.List[index])); }
        }

        public int Add(Goodsrateinfo value)
        {
            return this.List.Add(value);
        }

        /// <summary>
        /// Copies the elements of the specified <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see> array to the end of the collection.
        /// </summary>
        /// <param name="value">An array of type <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see> containing the Components to add to the collection.</param>
        public void AddRange(Goodsrateinfo[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }

        /// <summary>
        /// Adds the contents of another <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see> to the end of the collection.
        /// </summary>
        /// <param name="value">A <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see> containing the Components to add to the collection. </param>
        public void AddRange(GoodsrateinfoCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add((Goodsrateinfo)value.List[i]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see>.
        /// </summary>
        /// <param name="value">The <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see> to search for in the collection.</param>
        /// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
        public bool Contains(Goodsrateinfo value)
        {
            return this.List.Contains(value);
        }

        /// <summary>
        /// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
        /// <param name="index">The index of the array at which to begin inserting.</param>
        public void CopyTo(Goodsrateinfo[] array, int index)
        {
            this.List.CopyTo(array, index);
        }

        /// <summary>
        /// Gets the index in the collection of the specified <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see>, if it exists in the collection.
        /// </summary>
        /// <param name="value">The <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see> to locate in the collection.</param>
        /// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
        public int IndexOf(Goodsrateinfo value)
        {
            return this.List.IndexOf(value);
        }

        public void Insert(int index, Goodsrateinfo value)
        {
            List.Insert(index, value);
        }

        public void Remove(Goodsrateinfo value)
        {
            List.Remove(value);
        }

        /// <summary>
        /// Returns an enumerator that can iterate through the <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see> instance.
        /// </summary>
        /// <returns>An <see cref="GoodsratesinfoinfoCollectionEnumerator">GoodsratesinfoinfoCollectionEnumerator</see> for the <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see> instance.</returns>
        public new GoodsratesinfoinfoCollectionEnumerator GetEnumerator()
        {
            return new GoodsratesinfoinfoCollectionEnumerator(this);
        }

        /// <summary>
        /// Supports a simple iteration over a <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see>.
        /// </summary>
        public class GoodsratesinfoinfoCollectionEnumerator : IEnumerator
        {
            private IEnumerator _enumerator;
            private IEnumerable _temp;

            /// <summary>
            /// Initializes a new instance of the <see cref="GoodsratesinfoinfoCollectionEnumerator">GoodsratesinfoinfoCollectionEnumerator</see> class referencing the specified <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see> object.
            /// </summary>
            /// <param name="mappings">The <see cref="GoodsrateinfoCollection">GoodsrateinfoCollection</see> to enumerate.</param>
            public GoodsratesinfoinfoCollectionEnumerator(GoodsrateinfoCollection mappings)
            {
                _temp = ((IEnumerable)(mappings));
                _enumerator = _temp.GetEnumerator();
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            public Goodsrateinfo Current
            {
                get { return ((Goodsrateinfo)(_enumerator.Current)); }
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
