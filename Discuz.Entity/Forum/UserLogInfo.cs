using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// 用户资料变更日志信息
    /// </summary>
    public class UserLogInfo
    {
        private int uId = 0;

        public int UId
        {
            get { return uId; }
            set { uId = value; }
        }

        private UserLogActionEnum action;
        /// <summary>
        /// 事件(add,update,delete)
        /// </summary>
        public UserLogActionEnum Action
        {
            get { return action; }
            set { action = value; }
        }

        private string dateTime = "";

        public string DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }
    }

    public enum UserLogActionEnum
    {
        /// <summary>
        /// 新注册的用户
        /// </summary>
        Add,
        /// <summary>
        /// 更新个人资料的用户
        /// </summary>
        Update,
        /// <summary>
        /// 删除的用户
        /// </summary>
        Delete
    }
}
