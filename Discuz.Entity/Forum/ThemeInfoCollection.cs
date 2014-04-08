#if NET1

using System;
using System.Collections;

namespace Discuz.Entity
{
	/// <summary>
	/// ThemeInfoCollection 的摘要说明。
	/// </summary>
	public class ThemeInfoCollection: CollectionBase
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="ThemeInfoCollection">ThemeInfoCollection</see> class.
		/// </summary>
		public ThemeInfoCollection() 
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ThemeInfoCollection">ThemeInfoCollection</see> class containing the elements of the specified source collection.
		/// </summary>
		/// <param name="value">A <see cref="ThemeInfoCollection">ThemeInfoCollection</see> with which to initialize the collection.</param>
		public ThemeInfoCollection(ThemeInfoCollection value)	
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ThemeInfoCollection">ThemeInfoCollection</see> class containing the specified array of <see cref="ThemeInfo">ThemeInfo</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="ThemeInfo">ThemeInfo</see> Components with which to initialize the collection. </param>
		public ThemeInfoCollection(ThemeInfo[] value)
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Gets the <see cref="ThemeInfoCollection">ThemeInfoCollection</see> at the specified index in the collection.
		/// <para>
		/// In C#, this property is the indexer for the <see cref="ThemeInfoCollection">ThemeInfoCollection</see> class.
		/// </para>
		/// </summary>
		public ThemeInfo this[int index] 
		{
			get	{return ((ThemeInfo)(this.List[index]));}
		}
		
		public int Add(ThemeInfo value) 
		{
			return this.List.Add(value);
		}
		
		/// <summary>
		/// Copies the elements of the specified <see cref="ThemeInfo">ThemeInfo</see> array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="ThemeInfo">ThemeInfo</see> containing the Components to add to the collection.</param>
		public void AddRange(ThemeInfo[] value) 
		{
			for (int i = 0;	(i < value.Length); i = (i + 1)) 
			{
				this.Add(value[i]);
			}
		}
		
		/// <summary>
		/// Adds the contents of another <see cref="ThemeInfoCollection">ThemeInfoCollection</see> to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="ThemeInfoCollection">ThemeInfoCollection</see> containing the Components to add to the collection. </param>
		public void AddRange(ThemeInfoCollection value) 
		{
			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				this.Add((ThemeInfo)value.List[i]);
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether the collection contains the specified <see cref="ThemeInfoCollection">ThemeInfoCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="ThemeInfoCollection">ThemeInfoCollection</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(ThemeInfo value) 
		{
			return this.List.Contains(value);
		}
		
		/// <summary>
		/// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(ThemeInfo[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}
		
		/// <summary>
		/// Gets the index in the collection of the specified <see cref="ThemeInfoCollection">ThemeInfoCollection</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="ThemeInfoCollection">ThemeInfoCollection</see> to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(ThemeInfo value) 
		{
			return this.List.IndexOf(value);
		}
		
		public void Insert(int index, ThemeInfo value)	
		{
			List.Insert(index, value);
		}
		
		public void Remove(ThemeInfo value) 
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
		/// Returns an enumerator that can iterate through the <see cref="ThemeInfoCollection">ThemeInfoCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="ThemeInfoCollectionEnumerator">ThemeInfoCollectionEnumerator</see> for the <see cref="ThemeInfoCollection">ThemeInfoCollection</see> instance.</returns>
		public new ThemeInfoCollectionEnumerator GetEnumerator()	
		{
			return new ThemeInfoCollectionEnumerator(this);
		}
		
		/// <summary>
		/// Supports a simple iteration over a <see cref="ThemeInfoCollection">ThemeInfoCollection</see>.
		/// </summary>
		public class ThemeInfoCollectionEnumerator : IEnumerator	
		{
			private	IEnumerator _enumerator;
			private	IEnumerable _temp;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="ThemeInfoCollectionEnumerator">ThemeInfoCollectionEnumerator</see> class referencing the specified <see cref="ThemeInfoCollection">ThemeInfoCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="ThemeInfoCollection">ThemeInfoCollection</see> to enumerate.</param>
			public ThemeInfoCollectionEnumerator(ThemeInfoCollection mappings)
			{
				_temp =	((IEnumerable)(mappings));
				_enumerator = _temp.GetEnumerator();
			}
			
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public ThemeInfo Current
			{
				get {return ((ThemeInfo)(_enumerator.Current));}
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