using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Discuz.Common.Generic
{
    /// <summary>
    /// LinkedList泛型类
    /// </summary>
    /// <typeparam name="T">占位符(下同)</typeparam>
    [Serializable]
    public class LinkedList<T> : System.Collections.Generic.LinkedList<T>, IDiscuzCollection<T>
    {

        #region 构造函数
        public LinkedList() : base() 
        { }

        public LinkedList(IEnumerable<T> collection) : base(collection) 
        { }

        private LinkedList(SerializationInfo info, StreamingContext context) : base(info, context) 
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

        public new void AddFirst(T value)
        {
            if (!this.IsFull)
            {
                base.AddFirst(value);
            }
        }

        /// <summary>
        /// 接受指定的访问方式(访问者模式)
        /// </summary>
        /// <param name="visitor"></param>
        public void Accept(IDiscuzVisitor<T> visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException("访问器为空");
            }

            System.Collections.Generic.LinkedList<T>.Enumerator enumerator = this.GetEnumerator();

            while (enumerator.MoveNext())
            {
                visitor.Visit(enumerator.Current);

                if (visitor.HasDone)
                {
                    return;
                }
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
                throw new ArgumentNullException("obj");
            }

            if (obj.GetType() == this.GetType())
            {
                LinkedList<T> l = obj as LinkedList<T>;

                return this.Count.CompareTo(l.Count);
            }
            else
            {
                return this.GetType().FullName.CompareTo(obj.GetType().FullName);
            }
        }
  
    }
}