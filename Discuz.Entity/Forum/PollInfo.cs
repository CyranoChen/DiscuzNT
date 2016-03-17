using System;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// 投票信息类
    /// </summary>
    public class PollInfo
    {
        
        private int _pollid;
        private int _tid;
        private int _displayorder;
        private int _multiple;
        private int _visible;
        private int _maxchoices;
        private string _expiration;
        private int _uid;
        private string _voternames;
        private int _allowview;
        /// <summary>
        /// 投票ID
        /// </summary>
        public int Pollid
        {
            set { _pollid = value; }
            get { return _pollid; }
        }
        /// <summary>
        /// 主题ID
        /// </summary>
        public int Tid
        {
            set { _tid = value; }
            get { return _tid; }
        }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int Displayorder
        {
            set { _displayorder = value; }
            get { return _displayorder; }
        }
        /// <summary>
        /// 是否多选
        /// </summary>
        public int Multiple
        {
            set { _multiple = value; }
            get { return _multiple; }
        }
        /// <summary>
        /// 是否投票可见
        /// </summary>
        public int Visible
        {
            set { _visible = value; }
            get { return _visible; }
        }
        /// <summary>
        /// 最大可选项数
        /// </summary>
        public int Maxchoices
        {
            set { _maxchoices = value; }
            get { return _maxchoices; }
        }
        /// <summary>
        /// 过期时间
        /// </summary>
        public string Expiration
        {
            set { _expiration = value; }
            get {
                if (_expiration == null)
                {
                    return DateTime.Now.ToString();
                }
                return _expiration; }
        }
        /// <summary>
        /// 发起投票人的ID
        /// </summary>
        public int Uid
        {
            set { _uid = value; }
            get { return _uid; }
        }
        /// <summary>
        /// 已投过票的用户
        /// </summary>
        public string Voternames
        {
            set { _voternames = value; }
            get { return _voternames == null ? "" : _voternames; }
        }
        /// <summary>
        /// 是否允许公开投票参与人
        /// </summary>
        public int Allowview
        {
            set { _allowview = value; }
            get { return _allowview; }
        }

    }
}
