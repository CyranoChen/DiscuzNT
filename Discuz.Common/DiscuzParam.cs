using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Discuz.Common
{
    /// <summary>
    /// Discuz参数类
    /// </summary>
    public class DiscuzParam : IComparable
    {
        private string name;
        private object value;

        /// <summary>
        /// 获取参数名称
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// 获取参数值
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

        /// <summary>
        /// 获取参数值
        /// </summary>
        public string EncodedValue
        {
            get
            {
                if (value is Array)
                    return HttpUtility.UrlEncode(ConvertArrayToString(value as Array));
                else
                    return HttpUtility.UrlEncode(value.ToString());
            }
        }

        /// <summary>
        /// 构造参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        protected DiscuzParam(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// 生成字符串
        /// </summary>
        /// <returns>返回字符串的名值对</returns>
        public override string ToString()
        {
            return string.Format("{0}={1}", Name, Value);
        }

        /// <summary>
        /// 生成encode字符串
        /// </summary>
        /// <returns></returns>
        public string ToEncodedString()
        {
            return string.Format("{0}={1}", Name, EncodedValue);
        }

        /// <summary>
        /// 创建Discuz参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>返回Discuz参数</returns>
        public static DiscuzParam Create(string name, object value)
        {
            return new DiscuzParam(name, value);
        }

        /// <summary>
        /// 比较参数是否相同
        /// </summary>
        /// <param name="obj">要同当前参数比较的参数</param>
        /// <returns>0相同,非0则不同</returns>
        public int CompareTo(object obj)
        {
            if (!(obj is DiscuzParam))
                return -1;

            return this.name.CompareTo((obj as DiscuzParam).name);
        }

        /// <summary>
        /// 将Discuz参数数组转换为名值串
        /// </summary>
        /// <param name="a">Discuz参数数组</param>
        /// <returns>转换的名值串,名值串之间用逗号分隔</returns>
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
