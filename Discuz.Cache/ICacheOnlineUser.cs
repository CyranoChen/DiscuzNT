using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using Discuz.Entity;

namespace Discuz.Cache.Data
{
    public interface ICacheOnlineUser
    {
        /// <summary>
        /// 获得在线用户总数量
        /// </summary>
        /// <returns>用户数量</returns>
        int GetOnlineAllUserCount();
        /// <summary>
        /// 创建在线表记录(本方法在应用程序初始化时被调用)
        /// </summary>
        /// <returns></returns>
        int CreateOnlineTable();
        /// <summary>
        /// 获得在线注册用户总数量
        /// </summary>
        /// <returns>用户数量</returns>
        int GetOnlineUserCount();
        /// <summary>
        /// 获得在线不可见用户量
        /// </summary>
        /// <returns>用户数量</returns>
        int GetInvisibleOnlineUserCount();
        /// <summary>
        /// 获取在线用户列表
        /// </summary>
        /// <returns></returns>
        DataTable GetOnlineUserListTable();

        OnlineUserInfo GetOnlineUser(int olid);

        /// <summary>
        /// 获得指定用户的详细信息
        /// </summary>
        /// <param name="userid">在线用户ID</param>
        /// <param name="password">用户密码</param>
        /// <returns>用户的详细信息</returns>
        OnlineUserInfo GetOnlineUser(int userid, string password);
        /// <summary>
        /// 获得指定用户的详细信息
        /// </summary>
        /// <returns>用户的详细信息</returns>
        OnlineUserInfo GetOnlineUserByIP(int userid, string ip);
        /// <summary>
        /// 检查在线用户验证码是否有效
        /// </summary>
        /// <param name="olid">在组用户ID</param>
        /// <param name="verifycode">验证码</param>
        /// <param name="newverifycode">新验证码</param>
        /// <returns>在组用户ID</returns>
        bool CheckUserVerifyCode(int olid, string verifycode, string newverifycode);
        /// <summary>
        /// 执行在线用户向表及缓存中添加的操作。
        /// </summary>
        /// <param name="onlineuserinfo">在组用户信息内容</param>
        /// <returns>添加成功则返回刚刚添加的olid,失败则返回0</returns>
        int CreateOnlineUserInfo(OnlineUserInfo onlineuserinfo, int timeout);
        /// <summary>
        /// 更新用户的当前动作及相关信息
        /// </summary>
        /// <param name="olid">在线列表id</param>
        /// <param name="action">动作</param>
        /// <param name="inid">所在位置代码</param>
        void UpdateAction(int olid, int action, int inid);
        /// <summary>
        /// 更新用户动作
        /// </summary>
        /// <param name="olid">在线用户id</param>
        /// <param name="action">用户操作</param>
        /// <param name="fid">版块id</param>
        /// <param name="forumname">版块名称</param>
        /// <param name="tid">主题id</param>
        /// <param name="topictitle">主题标题</param>
        void UpdateAction(int olid, int action, int fid, string forumname, int tid, string topictitle);
        /// <summary>
        /// 更新用户最后活动时间
        /// </summary>
        /// <param name="olid">在线id</param>
        void UpdateLastTime(int olid);
        /// <summary>
        /// 更新用户最后发帖时间
        /// </summary>
        /// <param name="olid">在线id</param>
        void UpdatePostTime(int olid);
        /// <summary>
        /// 更新用户最后发短消息时间
        /// </summary>
        /// <param name="olid">在线id</param>
        void UpdatePostPMTime(int olid);
        /// <summary>
        /// 更新在线表中指定用户是否隐身
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="invisible">是否隐身</param>
        void UpdateInvisible(int olid, int invisible);
        /// <summary>
        /// 更新在线表中指定用户的用户密码
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="password">用户密码</param>
        void UpdatePassword(int olid, string password);
        /// <summary>
        /// 更新用户IP地址
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="ip">ip地址</param>
        void UpdateIP(int olid, string ip);
        /// <summary>
        /// 更新用户的用户组
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="groupid">组名</param>
        void UpdateGroupid(int userid, int groupid);

        #region 删除符合条件的用户信息
        /// <summary>
        /// 删除符合条件的一个或多个用户信息
        /// </summary>
        /// <returns>删除行数</returns>
        int DeleteRowsByIP(string ip);
        /// <summary>
        /// 删除在线表中指定在线id的行
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <returns></returns>
        int DeleteRows(int olid);
        #endregion

        /// <summary>
        /// 返回在线用户列表
        /// </summary>
        /// <param name="forumid">版块id</param>
        /// <returns></returns>
        Discuz.Common.Generic.List<OnlineUserInfo> GetForumOnlineUserCollection(int forumid);
        /// <summary>
        /// 返回在线用户列表
        /// </summary>
        Discuz.Common.Generic.List<OnlineUserInfo> GetOnlineUserCollection();
        /// <summary>
        /// 根据Uid获得Olid
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        int GetOlidByUid(int uid);
        /// <summary>
        /// 更新用户新短消息数
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="count">增加量</param>
        /// <returns></returns>
        int UpdateNewPms(int olid, int count);
        /// <summary>
        /// 更新用户新通知数
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="pluscount">增加量</param>
        /// <returns></returns>
        int UpdateNewNotices(int olid, int pluscount);
        /// <summary>
        /// 更新在线表中好友关系请求计数
        /// </summary>
        /// <param name="olId">在线id</param>
        /// <param name="count">增加量</param>
        /// <returns></returns>
        //int UpdateNewFriendsRequest(int olId, int count);
        /// <summary>
        /// 更新在线表中应用请求计数
        /// </summary>
        /// <param name="olId">在线id</param>
        /// <param name="count">更新数</param>
        /// <returns></returns>
        //int UpdateNewApplicationRequest(int olId, int count);
    }
}