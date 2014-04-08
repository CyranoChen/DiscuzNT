using System;

namespace Discuz.Entity
{
    /// <summary>
    /// 店铺主题信息类
    /// </summary>
    public class Shopthemeinfo
    {
        private int _themeid;//主题id
        /// <summary> 
        /// 主题id
        /// </summary>
        public int Themeid
        {
            get { return _themeid; }
            set { _themeid = value; }
        }

        private string _directory;//路径
        /// <summary> 
        /// 路径
        /// </summary>
        public string Directory
        {
            get { return _directory; }
            set { _directory = value; }
        }

        private string _name;//名称
        /// <summary> 
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _author;//作者
        /// <summary> 
        /// 作者
        /// </summary>
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        private string _createdate;//创建时间
        /// <summary> 
        /// 创建时间
        /// </summary>
        public string Createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }

        private string _copyright;//版权信息
        /// <summary> 
        /// 版权信息
        /// </summary>
        public string Copyright
        {
            get { return _copyright; }
            set { _copyright = value; }
        }
		
    }
}
