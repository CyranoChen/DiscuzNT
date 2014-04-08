#if NET1

using System;
using System.Collections;

namespace Discuz.Entity
{
	
	/// <summary>
	/// 标签信息集合类
	/// </summary>
	public class TagInfoCollection : CollectionBase
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="TagInfoCollection">TagInfoCollection</see> class.
		/// </summary>
		public TagInfoCollection() 
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="TagInfoCollection">TagInfoCollection</see> class containing the elements of the specified source collection.
		/// </summary>
		/// <param name="value">A <see cref="TagInfoCollection">TagInfoCollection</see> with which to initialize the collection.</param>
		public TagInfoCollection(TagInfoCollection value)	
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="TagInfoCollection">TagInfoCollection</see> class containing the specified array of <see cref="TagInfo">TagInfo</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="TagInfo">TagInfo</see> Components with which to initialize the collection. </param>
		public TagInfoCollection(TagInfo[] value)
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Gets the <see cref="TagInfoCollection">TagInfoCollection</see> at the specified index in the collection.
		/// <para>
		/// In C#, this property is the indexer for the <see cref="TagInfoCollection">TagInfoCollection</see> class.
		/// </para>
		/// </summary>
		public TagInfo this[int index] 
		{
			get	{return ((TagInfo)(this.List[index]));}
		}
		
		public int Add(TagInfo value) 
		{
			return this.List.Add(value);
		}
		
		/// <summary>
		/// Copies the elements of the specified <see cref="TagInfo">TagInfo</see> array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="TagInfo">TagInfo</see> containing the Components to add to the collection.</param>
		public void AddRange(TagInfo[] value) 
		{
			for (int i = 0;	(i < value.Length); i = (i + 1)) 
			{
				this.Add(value[i]);
			}
		}
		
		/// <summary>
		/// Adds the contents of another <see cref="TagInfoCollection">TagInfoCollection</see> to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="TagInfoCollection">TagInfoCollection</see> containing the Components to add to the collection. </param>
		public void AddRange(TagInfoCollection value) 
		{
			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				this.Add((TagInfo)value.List[i]);
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether the collection contains the specified <see cref="TagInfoCollection">TagInfoCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="TagInfoCollection">TagInfoCollection</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(TagInfo value) 
		{
			return this.List.Contains(value);
		}
		
		/// <summary>
		/// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(TagInfo[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}
		
		/// <summary>
		/// Gets the index in the collection of the specified <see cref="TagInfoCollection">TagInfoCollection</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="TagInfoCollection">TagInfoCollection</see> to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(TagInfo value) 
		{
			return this.List.IndexOf(value);
		}
		
		public void Insert(int index, TagInfo value)	
		{
			List.Insert(index, value);
		}
		
		public void Remove(TagInfo value) 
		{
			List.Remove(value);
		}
		
		/// <summary>
		/// Returns an enumerator that can iterate through the <see cref="TagInfoCollection">TagInfoCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="TagInfoCollectionEnumerator">TagInfoCollectionEnumerator</see> for the <see cref="TagInfoCollection">TagInfoCollection</see> instance.</returns>
		public new TagInfoCollectionEnumerator GetEnumerator()	
		{
			return new TagInfoCollectionEnumerator(this);
		}
		
		/// <summary>
		/// Supports a simple iteration over a <see cref="TagInfoCollection">TagInfoCollection</see>.
		/// </summary>
		public class TagInfoCollectionEnumerator : IEnumerator	
		{
			private	IEnumerator _enumerator;
			private	IEnumerable _temp;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="TagInfoCollectionEnumerator">TagInfoCollectionEnumerator</see> class referencing the specified <see cref="TagInfoCollection">TagInfoCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="TagInfoCollection">TagInfoCollection</see> to enumerate.</param>
			public TagInfoCollectionEnumerator(TagInfoCollection mappings)
			{
				_temp =	((IEnumerable)(mappings));
				_enumerator = _temp.GetEnumerator();
			}
			
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public TagInfo Current
			{
				get {return ((TagInfo)(_enumerator.Current));}
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