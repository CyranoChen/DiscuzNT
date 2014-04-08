using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Entity
{
    [Serializable]
    public class NavInfo
    {
        private int _id;
        //private int _highlight;
        private int _jsmenu;
        private int _level;
        private int _parentid;
        private string _name;
        private string _title;
        private string _url;
        private int _target;
        private int _type;
        private int _available;
        private int _displayorder;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 样式
        /// </summary>
        //public int Highlight
        //{
        //    set { _highlight = value; }
        //    get { return _highlight; }
        //}
        /// <summary>
        /// 是否启用下拉菜单 -1 禁止启用 0 不启用 1 启用
        /// </summary>
        public int Jsmenu
        {
            set { _jsmenu = value; }
            get { return _jsmenu; }
        }
        /// <summary>
        /// 使用等级  0 游客 1 会员  2 版主  3 管理员
        /// </summary>
        public int Level
        {
            set { _level = value; }
            get { return _level; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Parentid
        {
            set { _parentid = value; }
            get { return _parentid; }
        }
        /// <summary>
        /// 栏目名称
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 栏目说明
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url
        {
            set { _url = value; }
            get { return _url; }
        }
        /// <summary>
        /// 目标框架 0 本窗口  1 新窗口
        /// </summary>
        public int Target
        {
            set { _target = value; }
            get { return _target; }
        }
        /// <summary>
        /// 类型 0 系统  1 自定义
        /// </summary>
        public int Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 是否可用 0--不可用；1--可用
        /// </summary>
        public int Available
        {
            set { _available = value; }
            get { return _available; }
        }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int Displayorder
        {
            set { _displayorder = value; }
            get { return _displayorder; }
        }


    }
}
