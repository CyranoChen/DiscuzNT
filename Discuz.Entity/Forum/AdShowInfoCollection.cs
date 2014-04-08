#if NET1

using System;
using System.Collections;

namespace Discuz.Entity
{
	/// <summary>
	/// AdShowInfoCollection 的摘要说明。
	/// </summary>
	public class AdShowInfoCollection: CollectionBase
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="AdShowInfoCollection">AdShowInfoCollection</see> class.
		/// </summary>
		public AdShowInfoCollection() 
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="AdShowInfoCollection">AdShowInfoCollection</see> class containing the elements of the specified source collection.
		/// </summary>
		/// <param name="value">A <see cref="AdShowInfoCollection">AdShowInfoCollection</see> with which to initialize the collection.</param>
		public AdShowInfoCollection(AdShowInfoCollection value)	
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="AdShowInfoCollection">AdShowInfoCollection</see> class containing the specified array of <see cref="AdShowInfo">AdShowInfo</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="AdShowInfo">AdShowInfo</see> Components with which to initialize the collection. </param>
		public AdShowInfoCollection(AdShowInfo[] value)
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Gets the <see cref="AdShowInfoCollection">AdShowInfoCollection</see> at the specified index in the collection.
		/// <para>
		/// In C#, this property is the indexer for the <see cref="AdShowInfoCollection">AdShowInfoCollection</see> class.
		/// </para>
		/// </summary>
		public AdShowInfo this[int index] 
		{
			get	{return ((AdShowInfo)(this.List[index]));}
		}
		
		public int Add(AdShowInfo value) 
		{
			return this.List.Add(value);
		}
		
		/// <summary>
		/// Copies the elements of the specified <see cref="AdShowInfo">AdShowInfo</see> array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="AdShowInfo">AdShowInfo</see> containing the Components to add to the collection.</param>
		public void AddRange(AdShowInfo[] value) 
		{
			for (int i = 0;	(i < value.Length); i = (i + 1)) 
			{
				this.Add(value[i]);
			}
		}
		
		/// <summary>
		/// Adds the contents of another <see cref="AdShowInfoCollection">AdShowInfoCollection</see> to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="AdShowInfoCollection">AdShowInfoCollection</see> containing the Components to add to the collection. </param>
		public void AddRange(AdShowInfoCollection value) 
		{
			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				this.Add((AdShowInfo)value.List[i]);
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether the collection contains the specified <see cref="AdShowInfoCollection">AdShowInfoCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="AdShowInfoCollection">AdShowInfoCollection</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(AdShowInfo value) 
		{
			return this.List.Contains(value);
		}
		
		/// <summary>
		/// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(AdShowInfo[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}
		
		/// <summary>
		/// Gets the index in the collection of the specified <see cref="AdShowInfoCollection">AdShowInfoCollection</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="AdShowInfoCollection">AdShowInfoCollection</see> to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(AdShowInfo value) 
		{
			return this.List.IndexOf(value);
		}
		
		public void Insert(int index, AdShowInfo value)	
		{
			List.Insert(index, value);
		}
		
		public void Remove(AdShowInfo value) 
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
		/// Returns an enumerator that can iterate through the <see cref="AdShowInfoCollection">AdShowInfoCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="AdShowInfoCollectionEnumerator">AdShowInfoCollectionEnumerator</see> for the <see cref="AdShowInfoCollection">AdShowInfoCollection</see> instance.</returns>
		public new AdShowInfoCollectionEnumerator GetEnumerator()	
		{
			return new AdShowInfoCollectionEnumerator(this);
		}
		
		/// <summary>
		/// Supports a simple iteration over a <see cref="AdShowInfoCollection">AdShowInfoCollection</see>.
		/// </summary>
		public class AdShowInfoCollectionEnumerator : IEnumerator	
		{
			private	IEnumerator _enumerator;
			private	IEnumerable _temp;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="AdShowInfoCollectionEnumerator">AdShowInfoCollectionEnumerator</see> class referencing the specified <see cref="AdShowInfoCollection">AdShowInfoCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="AdShowInfoCollection">AdShowInfoCollection</see> to enumerate.</param>
			public AdShowInfoCollectionEnumerator(AdShowInfoCollection mappings)
			{
				_temp =	((IEnumerable)(mappings));
				_enumerator = _temp.GetEnumerator();
			}
			
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public AdShowInfo Current
			{
				get {return ((AdShowInfo)(_enumerator.Current));}
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