#if NET1

using System;
using System.Collections;

namespace Discuz.Entity
{
	/// <summary>
	/// PhotoInfoCollection 的摘要说明。
	/// </summary>
	public class PhotoInfoCollection: CollectionBase
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="PhotoInfoCollection">PhotoInfoCollection</see> class.
		/// </summary>
		public PhotoInfoCollection() 
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="PhotoInfoCollection">PhotoInfoCollection</see> class containing the elements of the specified source collection.
		/// </summary>
		/// <param name="value">A <see cref="PhotoInfoCollection">PhotoInfoCollection</see> with which to initialize the collection.</param>
		public PhotoInfoCollection(PhotoInfoCollection value)	
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="PhotoInfoCollection">PhotoInfoCollection</see> class containing the specified array of <see cref="PhotoInfo">PhotoInfo</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="PhotoInfo">PhotoInfo</see> Components with which to initialize the collection. </param>
		public PhotoInfoCollection(PhotoInfo[] value)
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Gets the <see cref="PhotoInfoCollection">PhotoInfoCollection</see> at the specified index in the collection.
		/// <para>
		/// In C#, this property is the indexer for the <see cref="PhotoInfoCollection">PhotoInfoCollection</see> class.
		/// </para>
		/// </summary>
		public PhotoInfo this[int index] 
		{
			get	{return ((PhotoInfo)(this.List[index]));}
		}
		
		public int Add(PhotoInfo value) 
		{
			return this.List.Add(value);
		}
		
		/// <summary>
		/// Copies the elements of the specified <see cref="PhotoInfo">PhotoInfo</see> array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="PhotoInfo">PhotoInfo</see> containing the Components to add to the collection.</param>
		public void AddRange(PhotoInfo[] value) 
		{
			for (int i = 0;	(i < value.Length); i = (i + 1)) 
			{
				this.Add(value[i]);
			}
		}
		
		/// <summary>
		/// Adds the contents of another <see cref="PhotoInfoCollection">PhotoInfoCollection</see> to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="PhotoInfoCollection">PhotoInfoCollection</see> containing the Components to add to the collection. </param>
		public void AddRange(PhotoInfoCollection value) 
		{
			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				this.Add((PhotoInfo)value.List[i]);
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether the collection contains the specified <see cref="PhotoInfoCollection">PhotoInfoCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="PhotoInfoCollection">PhotoInfoCollection</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(PhotoInfo value) 
		{
			return this.List.Contains(value);
		}
		
		/// <summary>
		/// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(PhotoInfo[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}
		
		/// <summary>
		/// Gets the index in the collection of the specified <see cref="PhotoInfoCollection">PhotoInfoCollection</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="PhotoInfoCollection">PhotoInfoCollection</see> to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(PhotoInfo value) 
		{
			return this.List.IndexOf(value);
		}
		
		public void Insert(int index, PhotoInfo value)	
		{
			List.Insert(index, value);
		}
		
		public void Remove(PhotoInfo value) 
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
		/// Returns an enumerator that can iterate through the <see cref="PhotoInfoCollection">PhotoInfoCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="PhotoInfoCollectionEnumerator">PhotoInfoCollectionEnumerator</see> for the <see cref="PhotoInfoCollection">PhotoInfoCollection</see> instance.</returns>
		public new PhotoInfoCollectionEnumerator GetEnumerator()	
		{
			return new PhotoInfoCollectionEnumerator(this);
		}
		
		/// <summary>
		/// Supports a simple iteration over a <see cref="PhotoInfoCollection">PhotoInfoCollection</see>.
		/// </summary>
		public class PhotoInfoCollectionEnumerator : IEnumerator	
		{
			private	IEnumerator _enumerator;
			private	IEnumerable _temp;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="PhotoInfoCollectionEnumerator">PhotoInfoCollectionEnumerator</see> class referencing the specified <see cref="PhotoInfoCollection">PhotoInfoCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="PhotoInfoCollection">PhotoInfoCollection</see> to enumerate.</param>
			public PhotoInfoCollectionEnumerator(PhotoInfoCollection mappings)
			{
				_temp =	((IEnumerable)(mappings));
				_enumerator = _temp.GetEnumerator();
			}
			
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public PhotoInfo Current
			{
				get {return ((PhotoInfo)(_enumerator.Current));}
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