#if NET1

using System;
using System.Collections;

namespace Discuz.Entity
{
	/// <summary>
	/// 在线用户信息集合类
	/// </summary>
	//[Serializable]
	public class ShowforumPageTopicInfoCollection: CollectionBase
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="ShowforumPageTopicInfoCollection">ShowforumPageTopicInfoCollection</see> class.
		/// </summary>
		public ShowforumPageTopicInfoCollection() 
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ShowforumPageTopicInfoCollection">ShowforumPageTopicInfoCollection</see> class containing the elements of the specified source collection.
		/// </summary>
		/// <param name="value">A <see cref="ShowforumPageTopicInfoCollection">ShowforumPageTopicInfoCollection</see> with which to initialize the collection.</param>
		public ShowforumPageTopicInfoCollection(ShowforumPageTopicInfoCollection value)	
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ShowforumPageTopicInfoCollection">ShowforumPageTopicInfoCollection</see> class containing the specified array of <see cref="ShowforumPageTopicInfo">ShowforumPageTopicInfo</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="ShowforumPageTopicInfo">ShowforumPageTopicInfo</see> Components with which to initialize the collection. </param>
		public ShowforumPageTopicInfoCollection(ShowforumPageTopicInfo[] value)
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Gets the <see cref="ShowforumPageTopicInfoCollection">ShowforumPageTopicInfoCollection</see> at the specified index in the collection.
		/// <para>
		/// In C#, this property is the indexer for the <see cref="ShowforumPageTopicInfoCollection">ShowforumPageTopicInfoCollection</see> class.
		/// </para>
		/// </summary>
		public ShowforumPageTopicInfo this[int index] 
		{
			get	{return ((ShowforumPageTopicInfo)(this.List[index]));}
		}
		
		/// <summary>
		/// 排序增加
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int SortAdd(ShowforumPageTopicInfo value) 
		{
			if (this.List.Count <= 0)
			{
				return this.List.Add(value);
			}
			else
			{
				for (int i = 0;	i < this.List.Count; i++) 
				{
					if ((value.Displayorder) > (this[i].Displayorder))
					{
						this.List.Insert(i, value);
						return i;
					}
				}
				return this.List.Add(value);
				
			}
		}

		public int Add(ShowforumPageTopicInfo value) 
		{
			return this.List.Add(value);
		}
		
		/// <summary>
		/// Copies the elements of the specified <see cref="ShowforumPageTopicInfo">ShowforumPageTopicInfo</see> array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="ShowforumPageTopicInfo">ShowforumPageTopicInfo</see> containing the Components to add to the collection.</param>
		public void AddRange(ShowforumPageTopicInfo[] value) 
		{
			for (int i = 0;	(i < value.Length); i = (i + 1)) 
			{
				this.Add(value[i]);
			}
		}
		
		/// <summary>
		/// Adds the contents of another <see cref="ShowforumPageTopicInfoCollection">ShowforumPageTopicInfoCollection</see> to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="ShowforumPageTopicInfoCollection">ShowforumPageTopicInfoCollection</see> containing the Components to add to the collection. </param>
		public void AddRange(ShowforumPageTopicInfoCollection value) 
		{
			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				this.Add((ShowforumPageTopicInfo)value.List[i]);
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether the collection contains the specified <see cref="ShowforumPageTopicInfoCollection">ShowforumPageTopicInfoCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="ShowforumPageTopicInfoCollection">ShowforumPageTopicInfoCollection</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(ShowforumPageTopicInfo value) 
		{
			return this.List.Contains(value);
		}
		
		/// <summary>
		/// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(ShowforumPageTopicInfo[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}
		
		/// <summary>
		/// Gets the index in the collection of the specified <see cref="ShowforumPageTopicInfoCollection">ShowforumPageTopicInfoCollection</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="ShowforumPageTopicInfoCollection">ShowforumPageTopicInfoCollection</see> to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(ShowforumPageTopicInfo value) 
		{
			return this.List.IndexOf(value);
		}
		
		public void Insert(int index, ShowforumPageTopicInfo value)	
		{
			List.Insert(index, value);
		}
		
		public void Remove(ShowforumPageTopicInfo value) 
		{
			List.Remove(value);
		}
		
		/// <summary>
		/// Returns an enumerator that can iterate through the <see cref="ShowforumPageTopicInfoCollection">ShowforumPageTopicInfoCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="ShowforumPageTopicInfoCollectionEnumerator">ShowforumPageTopicInfoCollectionEnumerator</see> for the <see cref="ShowforumPageTopicInfoCollection">ShowforumPageTopicInfoCollection</see> instance.</returns>
		public new ShowforumPageTopicInfoCollectionEnumerator GetEnumerator()	
		{
			return new ShowforumPageTopicInfoCollectionEnumerator(this);
		}
		
		/// <summary>
		/// Supports a simple iteration over a <see cref="ShowforumPageTopicInfoCollection">ShowforumPageTopicInfoCollection</see>.
		/// </summary>
		public class ShowforumPageTopicInfoCollectionEnumerator : IEnumerator	
		{
			private	IEnumerator _enumerator;
			private	IEnumerable _temp;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="ShowforumPageTopicInfoCollectionEnumerator">ShowforumPageTopicInfoCollectionEnumerator</see> class referencing the specified <see cref="ShowforumPageTopicInfoCollection">ShowforumPageTopicInfoCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="ShowforumPageTopicInfoCollection">ShowforumPageTopicInfoCollection</see> to enumerate.</param>
			public ShowforumPageTopicInfoCollectionEnumerator(ShowforumPageTopicInfoCollection mappings)
			{
				_temp =	((IEnumerable)(mappings));
				_enumerator = _temp.GetEnumerator();
			}
			
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public ShowforumPageTopicInfo Current
			{
				get {return ((ShowforumPageTopicInfo)(_enumerator.Current));}
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