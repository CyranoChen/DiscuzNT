using System;
using System.Collections;

namespace Discuz.Entity
{
	/// <summary>
	/// 在线用户信息集合类
	/// </summary>
	//[Serializable]
	public class NoticeinfoCollection: CollectionBase
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="NoticeinfoCollection">NoticeinfoCollection</see> class.
		/// </summary>
		public NoticeinfoCollection() 
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="NoticeinfoCollection">NoticeinfoCollection</see> class containing the elements of the specified source collection.
		/// </summary>
		/// <param name="value">A <see cref="NoticeinfoCollection">NoticeinfoCollection</see> with which to initialize the collection.</param>
		public NoticeinfoCollection(NoticeinfoCollection value)	
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="NoticeinfoCollection">NoticeinfoCollection</see> class containing the specified array of <see cref="Noticeinfo">Noticeinfo</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="Noticeinfo">Noticeinfo</see> Components with which to initialize the collection. </param>
		public NoticeinfoCollection(NoticeInfo[] value)
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Gets the <see cref="NoticeinfoCollection">NoticeinfoCollection</see> at the specified index in the collection.
		/// <para>
		/// In C#, this property is the indexer for the <see cref="NoticeinfoCollection">NoticeinfoCollection</see> class.
		/// </para>
		/// </summary>
		public NoticeInfo this[int index] 
		{
			get	{return ((NoticeInfo)(this.List[index]));}
		}
		
		public int Add(NoticeInfo value) 
		{
			return this.List.Add(value);
		}
		
		/// <summary>
		/// Copies the elements of the specified <see cref="Noticeinfo">Noticeinfo</see> array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="Noticeinfo">Noticeinfo</see> containing the Components to add to the collection.</param>
		public void AddRange(NoticeInfo[] value) 
		{
			for (int i = 0;	(i < value.Length); i = (i + 1)) 
			{
				this.Add(value[i]);
			}
		}
		
		/// <summary>
		/// Adds the contents of another <see cref="NoticeinfoCollection">NoticeinfoCollection</see> to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="NoticeinfoCollection">NoticeinfoCollection</see> containing the Components to add to the collection. </param>
		public void AddRange(NoticeinfoCollection value) 
		{
			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				this.Add((NoticeInfo)value.List[i]);
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether the collection contains the specified <see cref="NoticeinfoCollection">NoticeinfoCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="NoticeinfoCollection">NoticeinfoCollection</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(NoticeInfo value) 
		{
			return this.List.Contains(value);
		}
		
		/// <summary>
		/// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(NoticeInfo[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}
		
		/// <summary>
		/// Gets the index in the collection of the specified <see cref="NoticeinfoCollection">NoticeinfoCollection</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="NoticeinfoCollection">NoticeinfoCollection</see> to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(NoticeInfo value) 
		{
			return this.List.IndexOf(value);
		}
		
		public void Insert(int index, NoticeInfo value)	
		{
			List.Insert(index, value);
		}
		
		public void Remove(NoticeInfo value) 
		{
			List.Remove(value);
		}
		
		/// <summary>
		/// Returns an enumerator that can iterate through the <see cref="NoticeinfoCollection">NoticeinfoCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="NoticeinfoCollectionEnumerator">NoticeinfoCollectionEnumerator</see> for the <see cref="NoticeinfoCollection">NoticeinfoCollection</see> instance.</returns>
		public new NoticeinfoCollectionEnumerator GetEnumerator()	
		{
			return new NoticeinfoCollectionEnumerator(this);
		}
		
		/// <summary>
		/// Supports a simple iteration over a <see cref="NoticeinfoCollection">NoticeinfoCollection</see>.
		/// </summary>
		public class NoticeinfoCollectionEnumerator : IEnumerator	
		{
			private	IEnumerator _enumerator;
			private	IEnumerable _temp;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="NoticeinfoCollectionEnumerator">NoticeinfoCollectionEnumerator</see> class referencing the specified <see cref="NoticeinfoCollection">NoticeinfoCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="NoticeinfoCollection">NoticeinfoCollection</see> to enumerate.</param>
			public NoticeinfoCollectionEnumerator(NoticeinfoCollection mappings)
			{
				_temp =	((IEnumerable)(mappings));
				_enumerator = _temp.GetEnumerator();
			}
			
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public NoticeInfo Current
			{
				get {return ((NoticeInfo)(_enumerator.Current));}
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

