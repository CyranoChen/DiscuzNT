using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Entity
{
    public class ArchiverForumInfo
    {
        private int fid = 0;

        /// <summary>
        /// 论坛fid
        /// </summary>
        public int Fid
        {
            get { return fid; }
            set { fid = value; }
        }
        private string name;

        /// <summary>
        /// 论坛名称
        /// </summary>
        public string Name
        {
            get { return name.Trim(); }
            set { name = value; }
        }
        private string parentidList;

        /// <summary>
        /// 论坛级别所处路径id列表
        /// </summary>
        public string ParentidList
        {
            get { return parentidList.Trim(); }
            set { parentidList = value; }
        }
        private int status = 0;

        /// <summary>
        /// 是否显示
        /// </summary>
        public int Status
        {
            get { return status; }
            set { status = value; }
        }
        private int layer = 0;

        /// <summary>
        /// 论坛层次
        /// </summary>
        public int Layer
        {
            get { return layer; }
            set { layer = value; }
        }
        private string viewPerm;

        /// <summary>
        /// 浏览权限设定,格式为 groupid1,groupid2,...
        /// </summary>
        public string ViewPerm
        {
            get { return viewPerm.Trim(); }
            set { viewPerm = value; }
        }
    }
}
