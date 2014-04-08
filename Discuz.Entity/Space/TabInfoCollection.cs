#if NET1

using System;
using System.Collections;

namespace Discuz.Entity
{
	/// <summary>
	/// TabInfoCollection 的摘要说明。
	/// </summary>
	public class TabInfoCollection: CollectionBase
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="TabInfoCollection">TabInfoCollection</see> class.
		/// </summary>
		public TabInfoCollection() 
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="TabInfoCollection">TabInfoCollection</see> class containing the elements of the specified source collection.
		/// </summary>
		/// <param name="value">A <see cref="TabInfoCollection">TabInfoCollection</see> with which to initialize the collection.</param>
		public TabInfoCollection(TabInfoCollection value)	
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="TabInfoCollection">TabInfoCollection</see> class containing the specified array of <see cref="TabInfo">TabInfo</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="TabInfo">TabInfo</see> Components with which to initialize the collection. </param>
		public TabInfoCollection(TabInfo[] value)
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Gets the <see cref="TabInfoCollection">TabInfoCollection</see> at the specified index in the collection.
		/// <para>
		/// In C#, this property is the indexer for the <see cref="TabInfoCollection">TabInfoCollection</see> class.
		/// </para>
		/// </summary>
		public TabInfo this[int index] 
		{
			get	{return ((TabInfo)(this.List[index]));}
		}
		
		public int Add(TabInfo value) 
		{
			return this.List.Add(value);
		}
		
		/// <summary>
		/// Copies the elements of the specified <see cref="TabInfo">TabInfo</see> array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="TabInfo">TabInfo</see> containing the Components to add to the collection.</param>
		public void AddRange(TabInfo[] value) 
		{
			for (int i = 0;	(i < value.Length); i = (i + 1)) 
			{
				this.Add(value[i]);
			}
		}
		
		/// <summary>
		/// Adds the contents of another <see cref="TabInfoCollection">TabInfoCollection</see> to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="TabInfoCollection">TabInfoCollection</see> containing the Components to add to the collection. </param>
		public void AddRange(TabInfoCollection value) 
		{
			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				this.Add((TabInfo)value.List[i]);
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether the collection contains the specified <see cref="TabInfoCollection">TabInfoCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="TabInfoCollection">TabInfoCollection</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(TabInfo value) 
		{
			return this.List.Contains(value);
		}
		
		/// <summary>
		/// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(TabInfo[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}
		
		/// <summary>
		/// Gets the index in the collection of the specified <see cref="TabInfoCollection">TabInfoCollection</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="TabInfoCollection">TabInfoCollection</see> to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(TabInfo value) 
		{
			return this.List.IndexOf(value);
		}
		
		public void Insert(int index, TabInfo value)	
		{
			List.Insert(index, value);
		}
		
		public void Remove(TabInfo value) 
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
		/// Returns an enumerator that can iterate through the <see cref="TabInfoCollection">TabInfoCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="TabInfoCollectionEnumerator">TabInfoCollectionEnumerator</see> for the <see cref="TabInfoCollection">TabInfoCollection</see> instance.</returns>
		public new TabInfoCollectionEnumerator GetEnumerator()	
		{
			return new TabInfoCollectionEnumerator(this);
		}
		
		/// <summary>
		/// Supports a simple iteration over a <see cref="TabInfoCollection">TabInfoCollection</see>.
		/// </summary>
		public class TabInfoCollectionEnumerator : IEnumerator	
		{
			private	IEnumerator _enumerator;
			private	IEnumerable _temp;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="TabInfoCollectionEnumerator">TabInfoCollectionEnumerator</see> class referencing the specified <see cref="TabInfoCollection">TabInfoCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="TabInfoCollection">TabInfoCollection</see> to enumerate.</param>
			public TabInfoCollectionEnumerator(TabInfoCollection mappings)
			{
				_temp =	((IEnumerable)(mappings));
				_enumerator = _temp.GetEnumerator();
			}
			
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public TabInfo Current
			{
				get {return ((TabInfo)(_enumerator.Current));}
			}
			
			object IEnumerator.Current
			{
				get {return _enumerator.Current;}
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

#endif