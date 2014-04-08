using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Discuz.Common.Generic
{
    /// <summary>
    /// Dictionary泛型类
    /// </summary>
    /// <typeparam name="T">占位符(下同)</typeparam>
    [Serializable]
    public class Dictionary<TKey, TValue> : System.Collections.Generic.Dictionary<TKey, TValue>, IDiscuzCollection<KeyValuePair<TKey, TValue>>
    {

        #region 构造函数
        public Dictionary() : base() 
        { }

        public Dictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) 
        { } 
        
        public Dictionary(IEqualityComparer<TKey> comparer) :base(comparer) 
        { }
      
        public Dictionary(int capacity):base(capacity)
        { }

        public Dictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer) 
        { }

        public Dictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) 
        { }

        public Dictionary(SerializationInfo info, StreamingContext context) : base(info, context) 
        { }
        #endregion

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.Count == 0;
            }
        }

       
        private int _fixedsize = default(int);
        /// <summary>
        /// 固定大小属性
        /// </summary>
        public int FixedSize
        {
            get
            {
                return _fixedsize;
            }
            set
            {
                _fixedsize = value;
            }
        }

        /// <summary>
        /// 是否已满
        /// </summary>
        public bool IsFull
        {
            get
            {
                if ((FixedSize != default(int)) && (this.Count >= FixedSize))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version
        {
            get
            {
                return "1.0";
            }
        }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author
        {
            get
            {
                return "Discuz!NT";
            }
        }

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }


        /// <summary>
        /// 追加元素
        /// </summary>
        /// <param name="value"></param>
        public new void Add(TKey tkey, TValue tvalue)
        {
            if (!this.IsFull)
            {
                base.Add(tkey, tvalue);
            }
        }

        /// <summary>
        /// 比较对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("当前数据为空");
            }

            if (obj.GetType() == this.GetType())
            {
                Dictionary<TKey, TValue> list = obj as Dictionary<TKey, TValue>;
                return this.Count.CompareTo(list.Count);
            }
            else
            {
                return this.GetType().FullName.CompareTo(obj.GetType().FullName);
            }
        }
    }
}