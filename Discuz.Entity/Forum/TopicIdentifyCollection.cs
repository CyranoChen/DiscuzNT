#if NET1

using System;
using System.Collections;

namespace Discuz.Entity
{
    /// <summary>
    /// 在线用户信息集合类
    /// </summary>
    public class TopicIdentifyCollection : CollectionBase
    {

        public TopicIdentifyCollection()
        {
        }

        public TopicIdentifyCollection(TopicIdentifyCollection value)
        {
            this.AddRange(value);
        }

        public TopicIdentifyCollection(TopicIdentify[] value)
        {
            this.AddRange(value);
        }

        public TopicIdentify this[int index]
        {
            get { return ((TopicIdentify)(this.List[index])); }
        }

        public int Add(TopicIdentify value)
        {
            return this.List.Add(value);
        }

        public void AddRange(TopicIdentify[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }

        public void AddRange(TopicIdentifyCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add((TopicIdentify)value.List[i]);
            }
        }

        public bool Contains(TopicIdentify value)
        {
            return this.List.Contains(value);
        }

        public void CopyTo(TopicIdentify[] array, int index)
        {
            this.List.CopyTo(array, index);
        }

        public int IndexOf(TopicIdentify value)
        {
            return this.List.IndexOf(value);
        }

        public void Insert(int index, TopicIdentify value)
        {
            List.Insert(index, value);
        }

        public void Remove(TopicIdentify value)
        {
            List.Remove(value);
        }

        public new TopicIdentifyCollectionEnumerator GetEnumerator()
        {
            return new TopicIdentifyCollectionEnumerator(this);
        }

        public class TopicIdentifyCollectionEnumerator : IEnumerator
        {
            private IEnumerator _enumerator;
            private IEnumerable _temp;

            public TopicIdentifyCollectionEnumerator(TopicIdentifyCollection mappings)
            {
                _temp = ((IEnumerable)(mappings));
                _enumerator = _temp.GetEnumerator();
            }

            public TopicIdentify Current
            {
                get { return ((TopicIdentify)(_enumerator.Current)); }
            }

            object IEnumerator.Current
            {
                get { return _enumerator.Current; }
            }
            
            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            bool IEnumerator.MoveNext()
            {
                return _enumerator.MoveNext();
            }

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