using System;
using System.Text;

namespace Discuz.Entity
{
    public class StatInfo
    {
        public StatInfo()
        { }

        /// <summary>
        /// 类型
        /// </summary>
        private string _type;
        public string Type
        {
            get { return _type.Trim(); }
            set { _type = value; }
        }

        /// <summary>
        /// 变量
        /// </summary>
        private string _variable;
        public string Variable
        {
            get { return _variable.Trim(); }
            set { _variable = value; }
        }

        /// <summary>
        /// 长度
        /// </summary>
        private int _count;
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
    }
}
