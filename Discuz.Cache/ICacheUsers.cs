using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Discuz.Entity;

namespace Discuz.Cache.Data
{
    public interface ICacheUsers
    {
        /// <summary>
        /// 返回指定用户的完整信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>用户信息</returns>
        UserInfo GetUserInfo(int uid);
        /// <summary>
        /// 购买主题
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="tid">主题ID</param>
        /// <param name="posterid">发帖人ID</param>
        /// <param name="price">售价</param>
        /// <param name="netamount">净收入</param>
        /// <param name="creditsTrans">要更新的扩展积分ID</param>
        void BuyTopic(int uid, int tid, int posterid, int price, float netamount, int creditsTrans);
        /// <summary>
        /// 更新用户的用户组信息
        /// </summary>
        /// <param name="uidList">用户ID列表</param>
        /// <param name="groupId">用户组ID</param>
        void UpdateUserGroup(string uidList, int groupId);
        /// <summary>
        /// 根据IP查找用户
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>用户信息</returns>
        UserInfo GetUserInfoByIP(string ip);
        /// <summary>
        /// 根据用户名返回用户id
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>用户id</returns>
        UserInfo GetUserInfoByName(string username);
        /// <summary>
        /// 检测Email和安全项
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="email">email</param>
        /// <param name="userSecques">用户安全问题答案的存储数据</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        int CheckEmailAndSecques(string username, string email, string userSecques);
        /// <summary>
        /// 检测密码和安全项
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <param name="userSecques">用户安全问题答案的存储数据</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        int CheckPasswordAndSecques(string username, string password, bool originalpassword, string userSecques);
        /// <summary>
        /// 判断用户密码是否正确
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否为未MD5密码</param>
        /// <returns>如果正确则返回uid</returns>
        UserInfo CheckPassword(string username, string password, bool originalpassword);
        /// <summary>
        /// 检测密码
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <returns>如果用户密码正确则返回uid, 否则返回-1</returns>
        UserInfo CheckPassword(int uid, string password, bool originalpassword);
        /// <summary>
        /// 根据指定的email查找用户并返回用户uid
        /// </summary>
        /// <param name="email">email地址</param>
        /// <returns>用户uid</returns>
        int FindUserEmail(string email);
        /// <summary>
        /// 得到论坛中用户总数
        /// </summary>
        /// <returns>用户总数</returns>
        ///int GetUserCount();
        /// <summary>
        /// 得到论坛中用户总数
        /// </summary>
        /// <returns>用户总数</returns>
        ///int GetUserCountByAdmin();
        /// <summary>
        /// 创建新用户.
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>返回用户ID, 如果已存在该用户名则返回-1</returns>
        int CreateUser(UserInfo userinfo);  
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="userinfo">用户信息</param>
        /// <returns>是否更新成功</returns>
        bool UpdateUser(UserInfo userinfo);
        /// <summary>
        /// 更新权限验证字符串
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="authstr">验证串</param>
        /// <param name="authflag">验证标志</param>
        void UpdateAuthStr(int uid, string authstr, int authflag);
        /// <summary>
        /// 更新指定用户的个人资料
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>如果用户不存在则为false, 否则为true</returns>
        void UpdateUserProfile(UserInfo userinfo);   
        /// <summary>
        /// 更新用户论坛设置
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>如果用户不存在则返回false, 否则返回true</returns>
        void UpdateUserForumSetting(UserInfo userinfo);
        /// <summary>
        /// 修改用户自定义积分字段的值
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="extid">扩展字段序号(1-8)</param>
        /// <param name="pos">增加的数值(可以是负数)</param>
        /// <returns>执行是否成功</returns>
        void UpdateUserExtCredits(int uid, int extid, float pos);
        /// <summary>
        /// 获得指定用户的指定积分扩展字段的值
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="extid">扩展字段序号(1-8)</param>
        /// <returns>值</returns>
        float GetUserExtCredits(int uid, int extid);
        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="avatar">头像</param>
        /// <param name="avatarwidth">头像宽度</param>
        /// <param name="avatarheight">头像高度</param>
        /// <param name="templateid">模板Id</param>
        /// <returns>如果用户不存在则返回false, 否则返回true</returns>
        void UpdateUserPreference(int uid, string avatar, int avatarwidth, int avatarheight, int templateid);
        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <returns>成功返回true否则false</returns>
        void UpdateUserPassword(int uid, string password, bool originalpassword);
        /// <summary>
        /// 更新用户安全问题
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="userSecques">用户安全问题答案的存储数据</param>
        /// <returns>成功返回true否则false</returns>
        void UpdateUserSecques(int uid, string userSecques);
        /// <summary>
        /// 更新用户最后登录时间
        /// </summary>
        /// <param name="uid">用户id</param>
        void UpdateUserLastvisit(int uid, string ip);
        /// <summary>
        /// 更新用户当前的在线状态
        /// </summary>
        /// <param name="uidlist">用户uid列表</param>
        /// <param name="state">当前在线状态(0:离线,1:在线)</param>
        void UpdateUserOnlineState(string uidlist, int state, string activitytime);
        /// <summary>
        /// 更新用户当前的在线状态
        /// </summary>
        /// <param name="uid">用户uid列表</param>
        /// <param name="state">当前在线状态(0:离线,1:在线)</param>
        void UpdateUserOnlineState(int uid, int state, string activitytime);   
        /// <summary>
        /// 更新用户当前的在线时间和最后活动时间
        /// </summary>
        /// <param name="uid">用户uid</param>
        void UpdateUserOnlineTime(int uid, string activitytime);
        /// <summary>
        /// 设置用户信息表中未读短消息的数量
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="pmnum">短消息数量</param>
        /// <returns>更新记录个数</returns>
        int SetUserNewPMCount(int uid, int pmnum);
        /// <summary>
        /// 将用户的未读短信息数量减小一个指定的值
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="subval">短消息将要减小的值,负数为加</param>
        /// <returns>更新记录个数</returns>
        int DecreaseNewPMCount(int uid, int subval);
        /// <summary>
        /// 更新用户精华数
        /// </summary>
        /// <param name="useridlist">uid列表</param>
        /// <returns></returns>
        int UpdateUserDigest(string useridlist);
        /// <summary>
        /// 更新用户SpaceID
        /// </summary>
        /// <param name="spaceid">要更新的SpaceId</param>
        /// <param name="userid">要更新的UserId</param>
        /// <returns>是否更新成功</returns>
        void UpdateUserSpaceId(int spaceid, int userid);
        /// <summary>
        /// 更新用户短消息设置
        /// </summary>
        /// <param name="user">用户信息</param>
        void UpdateUserPMSetting(UserInfo user);
        /// <summary>
        /// 更新被禁止的用户
        /// </summary>
        /// <param name="groupid">用户组id</param>
        /// <param name="groupexpiry">过期时间</param>
        /// <param name="uid">用户id</param>
        void UpdateBanUser(int groupid, string groupexpiry, int uid);
        /// <summary>
        /// 得到指定用户的指定积分扩展字段的积分值
        /// </summary>
        /// <param name="uid">指定用户id</param>
        /// <param name="extnumber">指定扩展字段</param>
        /// <returns>扩展字展积分值</returns>
        int GetUserExtCreditsByUserid(int uid, int extnumber);
        /// <summary>
        /// 更新用户勋章信息
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="medals">勋章信息</param>
        void UpdateMedals(int uid, string medals);
        /// <summary>
        /// 更改用户组用户的管理权限
        /// </summary>
        /// <param name="adminId">管理组Id</param>
        /// <param name="groupId">用户组Id</param>
        void UpdateUserAdminIdByGroupId(int adminId, int groupId);     
        /// <summary>
        /// 更新用户到禁言组
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        void UpdateUserToStopTalkGroup(string uidList);
        /// <summary>
        /// 清除用户所发帖数以及精华数
        /// </summary>
        /// <param name="uid">用户Id</param>
        void ClearPosts(int uid);
        /// <summary>
        /// 更新Email验证信息
        /// </summary>
        /// <param name="authstr">验证字符串</param>
        /// <param name="authtime">验证时间</param>
        /// <param name="uid">用户Id</param>
        void UpdateEmailValidateInfo(string authstr, DateTime authTime, int uid);
        /// <summary>
        /// 更新用户积分,注:此处未实现相应方法，需要有客户端工具来重新统计用过发帖数
        /// </summary>
        /// <param name="credits">积分</param>
        void UpdateUserCredits(string credits);   
        /// <summary>
        /// 将Uid列表中的用户更新到目标组中
        /// </summary>
        /// <param name="groupid">目标组</param>
        /// <param name="uidList">用户列表</param>
        void UpdateUserGroupByUidList(int groupid, string uidList);
        /// <summary>
        /// 按用户Id列表删除用户
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        void DeleteUsers(string uidList);  
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="delPosts">是否删除帖子</param>
        /// <param name="delPms">是否删除短信</param>
        /// <returns></returns>
        bool DeleteUser(int uid, bool delPosts, bool delPms);
        /// <summary>
        /// 清空用户Id列表中的验证码
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        void ClearUsersAuthstr(string uidList);
        /// <summary>
        /// 更新用户签名，来自，简介三个字段
        /// </summary>
        /// <param name="__userinfo"></param>
        /// <returns></returns>
        void UpdateUserInfo(string location, string bio, string signature, int uid);
        /// <summary>
        /// 设置用户为版主
        /// </summary>
        /// <param name="userName">用户名</param>
        void SetUserToModerator(string userName);
        /// <summary>
        /// 合并用户
        /// </summary>
        /// <param name="postTableName">分表名称</param>
        /// <param name="targetUserInfo">目标用户</param>
        /// <param name="srcUserInfo">要合并用户</param>
        void CombinationUser(string postTableName, UserInfo targetUserInfo, UserInfo srcUserInfo);  
        /// <summary>
        /// 更新用户帖数
        /// </summary>
        /// <param name="postcount">帖子数</param>
        /// <param name="userid">用户ID</param>
        void UpdateUserPostCount(int postcount, int userid);
        /// <summary>
        /// 更新所有用户的帖子数
        /// </summary>
        /// <param name="postTableId"></param>
        void UpdateAllUserPostCount(int postTableId);  
        /// <summary>
        /// 重建用户精华帖数
        /// </summary>
        void ResetUserDigestPosts();        
        /// <summary>
        /// 更新普通用户用户组
        /// </summary>
        /// <param name="groupid">用户组id</param>
        /// <param name="userid">用户ID</param>
        void UpdateUserOtherInfo(int groupid, int userid);
        /// <summary>
        /// 更新在线表用户信息
        /// </summary>
        /// <param name="groupid">用户组id</param>
        /// <param name="userid">用户ID</param>
        void UpdateUserOnlineInfo(int groupid, int userid); 
        /// <summary>
        /// 根据pidlist中的发帖人信息移除用户信息
        /// </summary>
        /// <param name="currentPostTableId">当前分表ID</param>
        /// <param name="pidlist">pidlist串</param>
        void RemoveUser(int currentPostTableId, string pidlist);
        /// <summary>
        /// 根据tidlist中的发帖人信息更新用户的发帖数
        /// </summary>
        /// <param name="currentPostTableId">当前分表ID</param>
        /// <param name="tidlist">tidlist串</param>
        void UpdateUserPost(string currentPostTableId, string tidlist);
    }
}