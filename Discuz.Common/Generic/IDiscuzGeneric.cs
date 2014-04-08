using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Discuz.Common.Generic
{
    public interface IDiscuzCollection<T> :  ICollection<T>, IComparable
    {
        /// <summary>
        /// 固定大小
        /// </summary>
        int FixedSize { get;}

        /// <summary>
        /// 集合类是否为空
        /// </summary>
        bool IsEmpty { get;}

        /// <summary>
        /// 集合类是否已满
        /// </summary>
        bool IsFull { get;}

        /// <summary>
        /// 版本
        /// </summary>
        string Version {get;}

        /// <summary>
        /// 作者
        /// </summary>
        string Author {get;}
    }

    public interface IDiscuzVisitor<T>
    {
        /// <summary>
        /// 是否已运行
        /// </summary>
       bool HasDone { get; }

        /// <summary>
        /// 访问指定的对象
        /// </summary>
        void Visit(T obj);
    }   
}