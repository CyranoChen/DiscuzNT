using System;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// 广告项
    /// </summary>
    public class AdShowInfo
    {
        private int _advid = 0;
        private int _displayorder = -1;
        private string _code = "暂无内容";
        //广告类型|资源地址|宽度|高度|链接地址|说明文字|广告投放位置|广告显示位置
        private string _parameters = "|||||||";


        /// <summary>
        /// 广告id
        /// </summary>
        public int Advid
        {
            get { return _advid; }
            set { _advid = value; }
        }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int Displayorder
        {
            get { return _displayorder; }
            set { _displayorder = value; }
        }

        /// <summary>
        /// 广告内容代码
        /// </summary>
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 广告参数
        /// </summary>
        public string Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }
    }
}
