using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Discuz.Common.Generic
{
    /// <summary>
    /// 列表泛型类
    /// </summary>
    /// <typeparam name="T">占位符(下同)</typeparam>
    [Serializable]
    public class List<T> : System.Collections.Generic.List<T>, IDiscuzCollection<T> //:Collection<T>  // where T : new()
    {

        #region 构造函数
        public List() : base() 
        { }

		public List(IEnumerable<T> collection) : base(collection) 
        { }

        public List(int capacity) : base(capacity) 
        { }
        #endregion

        public List<T> Take(int count)
        {
            return this.GetRange(0, count < 0 ? 0 : count);
        }

        //public List<T> FindAll(Predicate<T> match)
        //{
        //    List<T> list = new List<T>();
        //    list.AddRange(base.FindAll(match));
        //    return list;
        //}
  
        public new List<T> GetRange(int start, int count)
        {
            List<T> list = new List<T>();
            //对传入起始位置和长度进行规则校验
            if (start < this.Count && (start + count) <= this.Count)
            {
                list.AddRange(base.GetRange(start, count));
            }
            return list;
        }

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
        public new void Add(T value)
        {
            if (!this.IsFull)
            {
                base.Add(value);
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

            //for (int i = 0; i < this.Count; i++)
            //{
            //    visitor.Visit(this[i]);

            //    if (visitor.HasCompleted)
            //    {
            //        break;
            //    }
            //}

            System.Collections.Generic.List<T>.Enumerator enumerator = this.GetEnumerator();

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
                List<T> l = obj as List<T>;

                return this.Count.CompareTo(l.Count);
            }
            else
            {
                return this.GetType().FullName.CompareTo(obj.GetType().FullName);
            }
        }

    }



    #region 注释测试代码段
       
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        #region 测试1

    //        DiscuzList<person> persons = new DiscuzList<person>();
    //        persons.Add(new person("daizhj", 31));
    //        persons.Add(new person("wohong",30));

    //        foreach (person p in persons)
    //        {
    //            Console.WriteLine(p.name+" "+p.age);
    //        }

    //        #endregion


    //        #region 测试2

    //        DiscuzList<student> studentsA = new DiscuzList<student>();
    //        studentsA.Add(new student("daizhj", 30,1,"bjpk"));
    //        studentsA.Add(new student("wohong", 31,2,"shfd"));

    //        DiscuzList<student> studentsB = new DiscuzList<student>();
    //        studentsB.Add(new student("daizhj", 32, 3, "hbhd"));
    //        studentsB.Add(new student("wohong", 33, 4, "FFBBV"));

    //        studentsA.AddRange(studentsB);

    //        Console.WriteLine("开始打印两部分学生集合");
    //        foreach (student s in studentsA)
    //        {
    //            Console.WriteLine(s.name + " " + s.age + " " + s.id + " " + s.schoolname);
    //        }

    //        #endregion


    //        Console.ReadLine();
    //    }
    //}

    //[Serializable]
    //class person
    //{
    //    public string name = "";
    //    public int age = 0;
    //    public person()
    //    {
    //    }

    //    public person(string Name, int Age)
    //    {
    //        name = Name;
    //        age = Age;
    //    }
    //}


    //[Serializable]
    //class student : person
    //{
    //    public int id = 0;
    //    public string schoolname = "";

    //    public student()
    //    {
    //    }

    //    public student(string Name, int Age, int ID, string SchoolName)
    //    {
    //        name = Name;
    //        age = Age;
    //        id = ID;
    //        schoolname = SchoolName;
    //    }
    //}
    #endregion
}