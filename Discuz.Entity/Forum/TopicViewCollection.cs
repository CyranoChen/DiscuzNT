#if NET1

using System;
using System.Collections;

namespace Discuz.Entity
{
	/// <summary>
	/// Class1 的摘要说明。
	/// </summary>
	[Serializable]
	public class TopicViewCollection: CollectionBase
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="TopicViewCollection">TopicViewCollection</see> class.
		/// </summary>
		public TopicViewCollection() 
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="TopicViewCollection">TopicViewCollection</see> class containing the elements of the specified source collection.
		/// </summary>
		/// <param name="value">A <see cref="TopicViewCollection">TopicViewCollection</see> with which to initialize the collection.</param>
		public TopicViewCollection(TopicViewCollection value)	
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="TopicViewCollection">TopicViewCollection</see> class containing the specified array of <see cref="TopicView">TopicView</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="TopicView">TopicView</see> Components with which to initialize the collection. </param>
		public TopicViewCollection(TopicView[] value)
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Gets the <see cref="TopicViewCollection">TopicViewCollection</see> at the specified index in the collection.
		/// <para>
		/// In C#, this property is the indexer for the <see cref="TopicViewCollection">TopicViewCollection</see> class.
		/// </para>
		/// </summary>
		public TopicView this[int index] 
		{
			get	{return ((TopicView)(this.List[index]));}
		}
		
		public int Add(TopicView value) 
		{
			return this.List.Add(value);
		}
		
		/// <summary>
		/// Copies the elements of the specified <see cref="TopicView">TopicView</see> array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="TopicView">TopicView</see> containing the Components to add to the collection.</param>
		public void AddRange(TopicView[] value) 
		{
			for (int i = 0;	(i < value.Length); i = (i + 1)) 
			{
				this.Add(value[i]);
			}
		}
		
		/// <summary>
		/// Adds the contents of another <see cref="TopicViewCollection">TopicViewCollection</see> to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="TopicViewCollection">TopicViewCollection</see> containing the Components to add to the collection. </param>
		public void AddRange(TopicViewCollection value) 
		{
			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				this.Add((TopicView)value.List[i]);
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether the collection contains the specified <see cref="TopicViewCollection">TopicViewCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="TopicViewCollection">TopicViewCollection</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(TopicView value) 
		{
			return this.List.Contains(value);
		}
		
		/// <summary>
		/// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(TopicView[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}
		
		/// <summary>
		/// Gets the index in the collection of the specified <see cref="TopicViewCollection">TopicViewCollection</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="TopicViewCollection">TopicViewCollection</see> to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(TopicView value) 
		{
			return this.List.IndexOf(value);
		}
		
		public void Insert(int index, TopicView value)	
		{
			List.Insert(index, value);
		}
		
		public void Remove(TopicView value) 
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

		private int _viewCount = 0;
		public int ViewCount
		{
			get
			{
				return _viewCount;
			}
			set
			{
				_viewCount = value;
			}
		}

		/// <summary>
		/// Returns an enumerator that can iterate through the <see cref="TopicViewCollection">TopicViewCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="TopicViewCollectionEnumerator">TopicViewCollectionEnumerator</see> for the <see cref="TopicViewCollection">TopicViewCollection</see> instance.</returns>
		public new TopicViewCollectionEnumerator GetEnumerator()	
		{
			return new TopicViewCollectionEnumerator(this);
		}
		
		/// <summary>
		/// Supports a simple iteration over a <see cref="TopicViewCollection">TopicViewCollection</see>.
		/// </summary>
		public class TopicViewCollectionEnumerator : IEnumerator	
		{
			private	IEnumerator _enumerator;
			private	IEnumerable _temp;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="TopicViewCollectionEnumerator">TopicViewCollectionEnumerator</see> class referencing the specified <see cref="TopicViewCollection">TopicViewCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="TopicViewCollection">TopicViewCollection</see> to enumerate.</param>
			public TopicViewCollectionEnumerator(TopicViewCollection mappings)
			{
				_temp =	((IEnumerable)(mappings));
				_enumerator = _temp.GetEnumerator();
			}
			
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public TopicView Current
			{
				get {return ((TopicView)(_enumerator.Current));}
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
