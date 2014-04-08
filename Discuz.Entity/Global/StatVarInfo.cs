using System;
using System.Text;

namespace Discuz.Entity
{
    public class StatVarInfo
    {
        public StatVarInfo()
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
        /// 内容
        /// </summary>
        private string _value;
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

    }
}
