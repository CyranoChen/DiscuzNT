using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Common.Generic
{

    /// <summary>
    /// 累加数访问类
    /// </summary>
    public sealed class SumVisitor : IDiscuzVisitor<int>
    {

        private int sum;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SumVisitor() 
        { }

        /// <summary>
        /// 访问指定的对象
        /// </summary>
        public void Visit(int obj)
        {
            sum += obj;
        }

        /// <summary>
        /// 是否已运行
        /// </summary>
        public bool HasDone
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 返加累加数
        /// </summary>
        public int Sum
        {
            get
            {
                return sum;
            }
        }
    }

    /// <summary>
    /// 计数访问类
    /// </summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class CountVisitor<T> : IDiscuzVisitor<T>
    {
   
        int count;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CountVisitor()
        { }

        /// <summary>
        /// 是否已运行
        /// </summary>
        public bool HasDone
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 访问指定的对象
        /// </summary>
        public void Visit(T obj)
        {
            count++;
        }

        /// <summary>
        /// 重设计数值
        /// </summary>
        public void ResetCount()
        {
            count = 0;
        }

        /// <summary>
        /// 返回计数值
        /// </summary>
        public int Count
        {
            get
            {
                return count;
            }
        }
    }

    /// <summary>
    /// 查找指定对象访问类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class FindingVisitor<T> : IDiscuzVisitor<T> where T : IComparable
    {
   
        private bool found = false;
        private T searchObj;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="searchobj"></param>
        public FindingVisitor(T searchobj)
        {
            this.searchObj = searchobj;
        }

        /// <summary>
        /// 是否已运行
        /// </summary>
        public bool HasDone
        {
            get
            {
                return found;
            }
        }

        /// <summary>
        /// 访问指定的对象
        /// </summary>
        public void Visit(T obj)
        {
            if (obj.CompareTo(searchObj) == 0)
            {
                found = true;
            }
        }

        /// <summary>
        /// 是否发现要查换的对象
        /// </summary>
        public bool Found
        {
            get
            {
                return found;
            }
        }

        /// <summary>
        /// 返回查找到的对象
        /// </summary>
        public T SearchResult
        {
            get
            {
                return searchObj;
            }
        }
    }
}