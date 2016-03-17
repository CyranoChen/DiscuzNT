using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// 悬赏帖给分日志实体
    /// </summary>
    public class BonusLogInfo
    {
        public BonusLogInfo()
        { }

        #region 私有字段
        private int _tid;
        private int _authorid;
        private int _answerid;
        private string _answername;
        private int _pid;
        private byte _extid;
        private DateTime _dateline;
        private int _bonus;
        private int _isbest;
        private string _message;

        #endregion 私有字段

        #region 属性
        /// <summary>
        /// 主题ID
        /// </summary>
        public int Tid
        {
            set { _tid = value; }
            get { return _tid; }
        }
        /// <summary>
        /// 作者ID
        /// </summary>
        public int Authorid
        {
            set { _authorid = value; }
            get { return _authorid; }
        }
        /// <summary>
        /// 回答者ID
        /// </summary>
        public int Answerid
        {
            set { _answerid = value; }
            get { return _answerid; }
        }
        /// <summary>
        /// 回答者名称
        /// </summary>
        public string Answername
        {
            get { return _answername; }
            set { _answername = value; }
        }
        /// <summary>
        /// 帖子ID
        /// </summary>
        public int Pid
        {
            get { return _pid; }
            set { _pid = value; }
        }
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime Dateline
        {
            set { _dateline = value; }
            get { return _dateline; }
        }
        /// <summary>
        /// 回答数
        /// </summary>
        public int Bonus
        {
            set { _bonus = value; }
            get { return _bonus; }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte Extid
        {
            set { _extid = value; }
            get { return _extid; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Isbest
        {
            set { _isbest = value;}
            get { return _isbest;}
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        #endregion 属性
    }
}
