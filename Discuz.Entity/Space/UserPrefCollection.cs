using System;
using System.Collections;

namespace Discuz.Entity
{
#if NET1

	/// <summary>
	/// UserPrefCollection 的摘要说明。
	/// </summary>
	public class UserPrefCollection: CollectionBase
	{
		public UserPrefCollection() 
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="UserPrefCollection">UserPrefCollection</see> class containing the elements of the specified source collection.
		/// </summary>
		/// <param name="value">A <see cref="UserPrefCollection">UserPrefCollection</see> with which to initialize the collection.</param>
		public UserPrefCollection(UserPrefCollection value)	
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="UserPrefCollection">UserPrefCollection</see> class containing the specified array of <see cref="UserPref">UserPref</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="UserPref">UserPref</see> Components with which to initialize the collection. </param>
		public UserPrefCollection(UserPref[] value)
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Gets the <see cref="UserPrefCollection">UserPrefCollection</see> at the specified index in the collection.
		/// <para>
		/// In C#, this property is the indexer for the <see cref="UserPrefCollection">UserPrefCollection</see> class.
		/// </para>
		/// </summary>
		public UserPref this[int index] 
		{
			get	{return ((UserPref)(this.List[index]));}
		}
		
		public int Add(UserPref value) 
		{
			if (value.DataType != UserPrefDataType.HiddenType)
				this._visibleItemCount++;
			if (value.Required)
				this._showRequired = true;
			

			return this.List.Add(value);
		}
		
		/// <summary>
		/// Copies the elements of the specified <see cref="UserPref">UserPref</see> array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="UserPref">UserPref</see> containing the Components to add to the collection.</param>
		public void AddRange(UserPref[] value) 
		{
			for (int i = 0;	(i < value.Length); i = (i + 1)) 
			{
				if (value[i].DataType != UserPrefDataType.HiddenType)
					this._visibleItemCount++;
				if (value[i].Required)
					this._showRequired = true;
				this.Add(value[i]);
			}
		}
		
		/// <summary>
		/// Adds the contents of another <see cref="UserPrefCollection">UserPrefCollection</see> to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="UserPrefCollection">UserPrefCollection</see> containing the Components to add to the collection. </param>
		public void AddRange(UserPrefCollection value) 
		{
			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				if (((UserPref)value.List[i]).DataType != UserPrefDataType.HiddenType)
					this._visibleItemCount++;
				if (((UserPref)value.List[i]).Required)
					this._showRequired = true;
				this.Add((UserPref)value.List[i]);
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether the collection contains the specified <see cref="UserPrefCollection">UserPrefCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="UserPrefCollection">UserPrefCollection</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(UserPref value) 
		{
			return this.List.Contains(value);
		}
		
		/// <summary>
		/// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(UserPref[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}
		
		/// <summary>
		/// Gets the index in the collection of the specified <see cref="UserPrefCollection">UserPrefCollection</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="UserPrefCollection">UserPrefCollection</see> to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(UserPref value) 
		{
			return this.List.IndexOf(value);
		}
		
		public void Insert(int index, UserPref value)	
		{
			if (value.DataType != UserPrefDataType.HiddenType)
				this._visibleItemCount++;
			if (value.Required)
				this._showRequired = true;

			List.Insert(index, value);
		}
		
		public void Remove(UserPref value) 
		{
			if (value.DataType != UserPrefDataType.HiddenType)
				this._visibleItemCount--;
			List.Remove(value);

			this._showRequired = false;
			foreach (UserPref up in this)
			{
				if (up.Required)
				{
					this._showRequired = true;
					break;
				}
			}
		}
		
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		private bool _showRequired = false;

		public bool ShowRequired
		{
			get { return _showRequired; }
			set { _showRequired = value; }
		}

		private int _visibleItemCount = 0;

		public int VisibleItemCount
		{
			get { return _visibleItemCount; }
		}

		/// <summary>
		/// Returns an enumerator that can iterate through the <see cref="UserPrefCollection">UserPrefCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="UserPrefCollectionEnumerator">UserPrefCollectionEnumerator</see> for the <see cref="UserPrefCollection">UserPrefCollection</see> instance.</returns>
		public new UserPrefCollectionEnumerator GetEnumerator()	
		{
			return new UserPrefCollectionEnumerator(this);
		}
		
		/// <summary>
		/// Supports a simple iteration over a <see cref="UserPrefCollection">UserPrefCollection</see>.
		/// </summary>
		public class UserPrefCollectionEnumerator : IEnumerator	
		{
			private	IEnumerator _enumerator;
			private	IEnumerable _temp;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="UserPrefCollectionEnumerator">UserPrefCollectionEnumerator</see> class referencing the specified <see cref="UserPrefCollection">UserPrefCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="UserPrefCollection">UserPrefCollection</see> to enumerate.</param>
			public UserPrefCollectionEnumerator(UserPrefCollection mappings)
			{
				_temp =	((IEnumerable)(mappings));
				_enumerator = _temp.GetEnumerator();
			}
			
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public UserPref Current
			{
				get {return ((UserPref)(_enumerator.Current));}
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


#else
public class UserPrefCollection<T> : Discuz.Common.Generic.List<T> where T : UserPref, new()
{
    public UserPrefCollection() : base() { }

    public UserPrefCollection(System.Collections.Generic.IEnumerable<T> collection) : base(collection) { }

    public UserPrefCollection(int capacity) : base(capacity) { }

    new public int Add(T value)
    {
        if (value.DataType != UserPrefDataType.HiddenType)
            this._visibleItemCount++;
        if (value.Required)
            this._showRequired = true;

        base.Add(value);

        return this.Count;
    }

    public void AddRange(T[] value)
    {
        for (int i = 0; (i < value.Length); i = (i + 1))
        {
            if (value[i].DataType != UserPrefDataType.HiddenType)
                this._visibleItemCount++;
            if (value[i].Required)
                this._showRequired = true;
            base.Add(value[i]);
        }
    }

    public void AddRange(UserPrefCollection<T> value)
    {
        for (int i = 0; (i < value.Count); i = (i + 1))
        {
            if (((T)value[i]).DataType != UserPrefDataType.HiddenType)
                this._visibleItemCount++;
            if (((T)value[i]).Required)
                this._showRequired = true;
            base.Add((T)value[i]);
        }
    }



    private bool _showRequired = false;

    public bool ShowRequired
    {
        get { return _showRequired; }
        set { _showRequired = value; }
    }

    private int _visibleItemCount = 0;

    public int VisibleItemCount
    {
        get { return _visibleItemCount; }
    }

    new public void Insert(int index, T value)
    {
        if (value.DataType != UserPrefDataType.HiddenType)
            this._visibleItemCount++;
        if (value.Required)
            this._showRequired = true;

        base.Insert(index, value);
    }

    new public void Remove(T value)
    {
        if (value.DataType != UserPrefDataType.HiddenType)
            this._visibleItemCount--;
        base.Remove(value);

        this._showRequired = false;
        foreach (T up in this)
        {
            if (up.Required)
            {
                this._showRequired = true;
                break;
            }
        }
    }

}
#endif
}
