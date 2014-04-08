using System;
using System.Text;

namespace Discuz.Entity
{
    public enum PhotoStatus
    {
        /// <summary>
        /// 仅所有者可评论
        /// </summary>
        Owner,
        /// <summary>
        /// 好友可评论
        /// </summary>
        Buddy,
        /// <summary>
        /// 注册用户可评论
        /// </summary>
        RegisteredUser
    }
}
