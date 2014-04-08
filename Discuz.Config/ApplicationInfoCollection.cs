using System;
using System.Text;
using System.Collections;

namespace Discuz.Config
{
    /// <summary>
    /// 整合程序配置信息集合
    /// </summary>
    [Serializable]
    public class ApplicationInfoCollection : CollectionBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInfoCollection">ApplicationInfoCollection</see> class.
        /// </summary>
        public ApplicationInfoCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInfoCollection">ApplicationInfoCollection</see> class containing the elements of the specified source collection.
        /// </summary>
        /// <param name="value">A <see cref="ApplicationInfoCollection">ApplicationInfoCollection</see> with which to initialize the collection.</param>
        public ApplicationInfoCollection(ApplicationInfoCollection value)
        {
            this.AddRange(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInfoCollection">ApplicationInfoCollection</see> class containing the specified array of <see cref="ApplicationInfo">ApplicationInfo</see> Components.
        /// </summary>
        /// <param name="value">An array of <see cref="ApplicationInfo">ApplicationInfo</see> Components with which to initialize the collection. </param>
        public ApplicationInfoCollection(ApplicationInfo[] value)
        {
            this.AddRange(value);
        }

        /// <summary>
        /// Gets the <see cref="ApplicationInfoCollection">ApplicationInfoCollection</see> at the specified index in the collection.
        /// <para>
        /// In C#, this property is the indexer for the <see cref="ApplicationInfoCollection">ApplicationInfoCollection</see> class.
        /// </para>
        /// </summary>
        public ApplicationInfo this[int index]
        {
            get { return ((ApplicationInfo)(this.List[index])); }
        }

        public int Add(ApplicationInfo value)
        {
            return this.List.Add(value);
        }

        /// <summary>
        /// Copies the elements of the specified <see cref="ApplicationInfo">ApplicationInfo</see> array to the end of the collection.
        /// </summary>
        /// <param name="value">An array of type <see cref="ApplicationInfo">ApplicationInfo</see> containing the Components to add to the collection.</param>
        public void AddRange(ApplicationInfo[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }

        /// <summary>
        /// Adds the contents of another <see cref="ApplicationInfoCollection">ApplicationInfoCollection</see> to the end of the collection.
        /// </summary>
        /// <param name="value">A <see cref="ApplicationInfoCollection">ApplicationInfoCollection</see> containing the Components to add to the collection. </param>
        public void AddRange(ApplicationInfoCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add((ApplicationInfo)value.List[i]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified <see cref="ApplicationInfoCollection">ApplicationInfoCollection</see>.
        /// </summary>
        /// <param name="value">The <see cref="ApplicationInfoCollection">ApplicationInfoCollection</see> to search for in the collection.</param>
        /// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
        public bool Contains(ApplicationInfo value)
        {
            return this.List.Contains(value);
        }

        /// <summary>
        /// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
        /// <param name="index">The index of the array at which to begin inserting.</param>
        public void CopyTo(ApplicationInfo[] array, int index)
        {
            this.List.CopyTo(array, index);
        }

        /// <summary>
        /// Gets the index in the collection of the specified <see cref="ApplicationInfoCollection">ApplicationInfoCollection</see>, if it exists in the collection.
        /// </summary>
        /// <param name="value">The <see cref="ApplicationInfoCollection">ApplicationInfoCollection</see> to locate in the collection.</param>
        /// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
        public int IndexOf(ApplicationInfo value)
        {
            return this.List.IndexOf(value);
        }

        public void Insert(int index, ApplicationInfo value)
        {
            List.Insert(index, value);
        }

        public void Remove(ApplicationInfo value)
        {
            List.Remove(value);
        }

        /// <summary>
        /// Returns an enumerator that can iterate through the <see cref="ApplicationInfoCollection">ApplicationInfoCollection</see> instance.
        /// </summary>
        /// <returns>An <see cref="ApplicationInfoCollectionEnumerator">ApplicationInfoCollectionEnumerator</see> for the <see cref="ApplicationInfoCollection">ApplicationInfoCollection</see> instance.</returns>
        public new ApplicationInfoCollectionEnumerator GetEnumerator()
        {
            return new ApplicationInfoCollectionEnumerator(this);
        }

        /// <summary>
        /// Supports a simple iteration over a <see cref="ApplicationInfoCollection">ApplicationInfoCollection</see>.
        /// </summary>
        public class ApplicationInfoCollectionEnumerator : IEnumerator
        {
            private IEnumerator _enumerator;
            private IEnumerable _temp;

            /// <summary>
            /// Initializes a new instance of the <see cref="ApplicationInfoCollectionEnumerator">ApplicationInfoCollectionEnumerator</see> class referencing the specified <see cref="ApplicationInfoCollection">ApplicationInfoCollection</see> object.
            /// </summary>
            /// <param name="mappings">The <see cref="ApplicationInfoCollection">ApplicationInfoCollection</see> to enumerate.</param>
            public ApplicationInfoCollectionEnumerator(ApplicationInfoCollection mappings)
            {
                _temp = ((IEnumerable)(mappings));
                _enumerator = _temp.GetEnumerator();
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            public ApplicationInfo Current
            {
                get { return ((ApplicationInfo)(_enumerator.Current)); }
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
