using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Config
{
    /// <summary>
    /// 论坛热点单元配置类
    /// </summary>
    [Serializable]
    public class ForumHotItemInfo
    {
        private int id = 0;
        private int enabled = 1;
        private string name = "";
        private string datatype = "";
        private string sorttype = "";
        private string forumlist = "";
        private int dataitemcount = 0;
        private int forumnamelength = 0;
        private int topictitlelength = 0;
        private int cachetimeout = 0;
        private string datatimetype = "All";

        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 是否开启
        /// </summary>
        public int Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string Datatype
        {
            get { return datatype; }
            set { datatype = value; }
        }

        /// <summary>
        /// 排序类型
        /// </summary>
        public string Sorttype
        {
            get { return sorttype; }
            set { sorttype = value; }
        }

        /// <summary>
        /// 数据版块id范围
        /// </summary>
        public string Forumlist
        {
            get { return forumlist; }
            set { forumlist = value; }
        }

        /// <summary>
        /// 获取数据个数
        /// </summary>
        public int Dataitemcount
        {
            get { return dataitemcount; }
            set { dataitemcount = value; }
        }

        /// <summary>
        /// 版块名称长度显示限制
        /// </summary>
        public int Forumnamelength
        {
            get { return forumnamelength; }
            set { forumnamelength = value; }
        }

        /// <summary>
        /// 主题标题长度显示限制
        /// </summary>
        public int Topictitlelength
        {
            get { return topictitlelength; }
            set { topictitlelength = value; }
        }

        /// <summary>
        /// 数据缓存时间
        /// </summary>
        public int Cachetimeout
        {
            get { return cachetimeout; }
            set { cachetimeout = value; }
        }

        /// <summary>
        /// 数据获取时间范围
        /// </summary>
        public string Datatimetype
        {
            get { return datatimetype; }
            set { datatimetype = value; }
        }
    }
}
