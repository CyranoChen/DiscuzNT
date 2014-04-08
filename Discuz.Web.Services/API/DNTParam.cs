using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Web.Services.API
{
    public class DNTParam : IComparable
    {
        private string name;
        private object value;

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// 参数值
        /// </summary>
        public string Value
        {
            get
            {
                if (value is Array)
                    return ConvertArrayToString(value as Array);
                else
                    return value.ToString();
            }
        }

        protected DNTParam(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}={1}", Name, Value);
        }

        /// <summary>
        /// 创建参数对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DNTParam Create(string name, object value)
        {
            return new DNTParam(name, value);
        }

        public int CompareTo(object obj)
        {
            if (!(obj is DNTParam))
                return -1;

            return this.name.CompareTo((obj as DNTParam).name);
        }

        /// <summary>
        /// 将数组转为字符串
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private static string ConvertArrayToString(Array a)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < a.Length; i++)
            {
                if (i > 0)
                    builder.Append(",");

                builder.Append(a.GetValue(i).ToString());
            }

            return builder.ToString();
        }
    }
}
