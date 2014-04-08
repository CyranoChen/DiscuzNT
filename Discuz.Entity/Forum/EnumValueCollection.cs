#if NET1

using System;
using System.Collections;

namespace Discuz.Entity
{
	/// <summary>
	/// EnumValueCollection 的摘要说明。
	/// </summary>
	public class EnumValueCollection: CollectionBase
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumValueCollection">EnumValueCollection</see> class.
		/// </summary>
		public EnumValueCollection() 
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="EnumValueCollection">EnumValueCollection</see> class containing the elements of the specified source collection.
		/// </summary>
		/// <param name="value">A <see cref="EnumValueCollection">EnumValueCollection</see> with which to initialize the collection.</param>
		public EnumValueCollection(EnumValueCollection value)	
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="EnumValueCollection">EnumValueCollection</see> class containing the specified array of <see cref="EnumValue">EnumValue</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="EnumValue">EnumValue</see> Components with which to initialize the collection. </param>
		public EnumValueCollection(EnumValue[] value)
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Gets the <see cref="EnumValueCollection">EnumValueCollection</see> at the specified index in the collection.
		/// <para>
		/// In C#, this property is the indexer for the <see cref="EnumValueCollection">EnumValueCollection</see> class.
		/// </para>
		/// </summary>
		public EnumValue this[int index] 
		{
			get	{return ((EnumValue)(this.List[index]));}
		}
		
		public int Add(EnumValue value) 
		{
			return this.List.Add(value);
		}
		
		/// <summary>
		/// Copies the elements of the specified <see cref="EnumValue">EnumValue</see> array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="EnumValue">EnumValue</see> containing the Components to add to the collection.</param>
		public void AddRange(EnumValue[] value) 
		{
			for (int i = 0;	(i < value.Length); i = (i + 1)) 
			{
				this.Add(value[i]);
			}
		}
		
		/// <summary>
		/// Adds the contents of another <see cref="EnumValueCollection">EnumValueCollection</see> to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="EnumValueCollection">EnumValueCollection</see> containing the Components to add to the collection. </param>
		public void AddRange(EnumValueCollection value) 
		{
			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				this.Add((EnumValue)value.List[i]);
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether the collection contains the specified <see cref="EnumValueCollection">EnumValueCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="EnumValueCollection">EnumValueCollection</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(EnumValue value) 
		{
			return this.List.Contains(value);
		}
		
		/// <summary>
		/// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(EnumValue[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}
		
		/// <summary>
		/// Gets the index in the collection of the specified <see cref="EnumValueCollection">EnumValueCollection</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="EnumValueCollection">EnumValueCollection</see> to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(EnumValue value) 
		{
			return this.List.IndexOf(value);
		}
		
		public void Insert(int index, EnumValue value)	
		{
			List.Insert(index, value);
		}
		
		public void Remove(EnumValue value) 
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
		/// Returns an enumerator that can iterate through the <see cref="EnumValueCollection">EnumValueCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="EnumValueCollectionEnumerator">EnumValueCollectionEnumerator</see> for the <see cref="EnumValueCollection">EnumValueCollection</see> instance.</returns>
		public new EnumValueCollectionEnumerator GetEnumerator()	
		{
			return new EnumValueCollectionEnumerator(this);
		}
		
		/// <summary>
		/// Supports a simple iteration over a <see cref="EnumValueCollection">EnumValueCollection</see>.
		/// </summary>
		public class EnumValueCollectionEnumerator : IEnumerator	
		{
			private	IEnumerator _enumerator;
			private	IEnumerable _temp;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="EnumValueCollectionEnumerator">EnumValueCollectionEnumerator</see> class referencing the specified <see cref="EnumValueCollection">EnumValueCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="EnumValueCollection">EnumValueCollection</see> to enumerate.</param>
			public EnumValueCollectionEnumerator(EnumValueCollection mappings)
			{
				_temp =	((IEnumerable)(mappings));
				_enumerator = _temp.GetEnumerator();
			}
			
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public EnumValue Current
			{
				get {return ((EnumValue)(_enumerator.Current));}
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