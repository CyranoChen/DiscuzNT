using Discuz.Common;
using Discuz.Entity;
using Discuz.Common.Generic;
using System.Collections;
using System.Text;

namespace Discuz.Forum
{
    public class Friendship
    {
        /// <summary>
        /// 创建好友请求信息
        /// </summary>
        /// <param name="friendshipRequestInfo">好友请求信息</param>
        /// <param name="userGroupInfo">当前用户用户组信息</param>
        /// <returns></returns>
        public static CreateNewFriendshipRequestEnum CreateNewFriendshipRequest(FriendshipRequestInfo friendshipRequestInfo, UserGroupInfo userGroupInfo)
        {
            if (friendshipRequestInfo.FromUid <= 0 || friendshipRequestInfo.ToUid <= 0 || friendshipRequestInfo.FromUid == friendshipRequestInfo.ToUid)
                return CreateNewFriendshipRequestEnum.MessageError;
            //如果用户之间已经是好友或者用户之间已经有一方请求的
            IsFriendshipExistEnum existResult = IsFriendshipExist(friendshipRequestInfo.FromUid, friendshipRequestInfo.ToUid);

            if (existResult == IsFriendshipExistEnum.Exist)
                return CreateNewFriendshipRequestEnum.FriendshipAlreadyExists;

            if (existResult == IsFriendshipExistEnum.RequestExist)
                return CreateNewFriendshipRequestEnum.RequestAlreadyExists;

            if (GetUserFriendsCount(friendshipRequestInfo.FromUid) >= userGroupInfo.MaxFriendsCount)
                return CreateNewFriendshipRequestEnum.UserFriendshipOverflow;

            if (Data.Friendship.CreateNewFriendshipRequest(friendshipRequestInfo) == (int)CreateNewFriendshipRequestEnum.Success)
            {
                int olId = OnlineUsers.GetOlidByUid(friendshipRequestInfo.ToUid);

                if (olId > 0)//如果被请求用户在线，则更新其在线状态
                    OnlineUsers.UpdateNewFriendsRequest(olId, 1);

                return CreateNewFriendshipRequestEnum.Success;
            }

            return CreateNewFriendshipRequestEnum.MessageError;
        }

        /// <summary>
        /// 通过好友的请求信息
        /// </summary>
        /// <param name="fromUid">请求人ID</param>
        /// <param name="toUid">被请求人ID</param>
        /// <param name="toUserGroupInfo">被请求人用户组信息</param>
        /// <param name="groupId">被请求人将请求人划分的好友组，0表示不分组</param>
        /// <returns></returns>
        public static PassFriendshipEnum PassFriendship(int fromUid, int toUid, UserGroupInfo toUserGroupInfo, int groupId)
        {
            if (fromUid <= 0 || toUid <= 0 || fromUid == toUid)
                return PassFriendshipEnum.MessageError;

            FriendshipRequestInfo requestInfo = GetUserFriendRequestInfo(fromUid, toUid);

            if (requestInfo == null || requestInfo.FromUid == 0 || requestInfo.ToUid == 0 || requestInfo.ToUid == requestInfo.FromUid)
                return PassFriendshipEnum.MessageError;

            //如果用户之间已经是好友了
            if (IsFriendshipExist(requestInfo.FromUid, requestInfo.ToUid) == IsFriendshipExistEnum.Exist)
                return PassFriendshipEnum.FriendshipAlreadyExists;

            if (Friendship.GetUserFriendsCount(toUid) >= toUserGroupInfo.MaxFriendsCount)
                return PassFriendshipEnum.ToUserFriendshipOverflow;

            UserGroupInfo fromUserGroupInfo = UserGroups.GetUserGroupInfo(Users.GetShortUserInfo(fromUid).Groupid);

            if (Friendship.GetUserFriendsCount(fromUid) >= fromUserGroupInfo.MaxFriendsCount)
                return PassFriendshipEnum.FromUserFriendshipOverflow;

            if (Data.Friendship.PassFriendship(requestInfo, groupId) == (int)PassFriendshipEnum.Success)
                return PassFriendshipEnum.Success;

            return PassFriendshipEnum.MessageError;
        }

        /// <summary>
        /// 获取用户好友列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<FriendshipInfo> GetUserFriendsList(int uid, int pageIndex, int pageSize)
        {
            if (uid < 1 || pageSize <= 0)
                return new List<FriendshipInfo>();
            if (pageIndex < 1)
                pageIndex = 1;
            return Data.Friendship.GetUserFriendsList(uid, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取用户好友数量
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int GetUserFriendsCount(int uid)
        {
            if (uid < 1)
                return 0;
            return Data.Friendship.GetUserFriendsCount(uid);
        }

        /// <summary>
        /// 获取用户好友请求数量
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int GetUserFriendRequestCount(int uid)
        {
            if (uid < 1)
                return 0;
            return Data.Friendship.GetUserFriendRequestCount(uid);
        }

        /// <summary>
        /// 获取用户好友请求信息
        /// </summary>
        /// <param name="fromUid"></param>
        /// <param name="toUid"></param>
        /// <returns></returns>
        public static FriendshipRequestInfo GetUserFriendRequestInfo(int fromUid, int toUid)
        {
            if (fromUid <= 0 || toUid <= 0)
                return null;
            return Data.Friendship.GetUserFriendRequestInfo(fromUid, toUid);
        }

        /// <summary>
        /// 获取用户好友请求列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public static List<FriendshipRequestInfo> GetUserFriendRequestList(int uid, int pageIndex, int pageSize)
        {
            if (uid < 1 || pageSize <= 0)
                return new List<FriendshipRequestInfo>();
            if (pageIndex < 1)
                pageIndex = 1;
            return Data.Friendship.GetUserFriendRequestList(uid, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取站点用户好友关系变更日志
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<FriendshipActionInfo> GetFriendshipChangeLog(int count)
        {
            if (count < 1)
                return new List<FriendshipActionInfo>();
            return Data.Friendship.GetFriendshipChangeLog(count);
        }

        /// <summary>
        /// 好友关系或请求关系是否存在（1：请求存在 2：存在 0：不存在）
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="friendUid"></param>
        /// <returns></returns>
        public static IsFriendshipExistEnum IsFriendshipExist(int uid, int friendUid)
        {
            return (IsFriendshipExistEnum)Data.Friendship.IsFriendshipExist(uid, friendUid);
        }

        /// <summary>
        /// 解除好友关系
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="friendUid"></param>
        /// <returns></returns>
        public static int DeleteFriendship(int uid, string friendUidList)
        {
            if (uid > 0 && Utils.IsNumericList(friendUidList))
                return Data.Friendship.DeleteFriendship(uid, friendUidList);
            return -1;
        }

        /// <summary>
        /// 忽略指定id的好友请求
        /// </summary>
        /// <param name="toUid"></param>
        /// <param name="fromUidList"></param>
        /// <returns></returns>
        public static int IgnoreFriendshipRequest(int toUid, string fromUidList)
        {
            if (toUid > 0 && Utils.IsNumericList(fromUidList))
                return Data.Friendship.IgnoreFriendshipRequest(toUid, fromUidList);
            return -1;
        }

        /// <summary>
        /// 按指定条件读取用户好友列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="conditionTable"></param>
        /// <returns></returns>
        public static List<FriendshipInfo> GetUserFriendsListByCondition(int uid, int pageIndex, int pageSize, Dictionary<FriendshipListSerachEnum, string> conditionTable)
        {
            if (uid < 1 || pageSize <= 0)
                return new List<FriendshipInfo>();
            return Data.Friendship.GetUserFriendsListByCondition(uid, pageIndex, pageSize, conditionTable);
        }

        /// <summary>
        /// 按指定条件读取用户好友个数
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="conditionTable"></param>
        /// <returns></returns>
        public static int GetUserFriendsCountByCondition(int uid, Dictionary<FriendshipListSerachEnum, string> conditionTable)
        {
            return Data.Friendship.GetUserFriendsCountByCondition(uid, conditionTable);
        }

        /// <summary>
        /// 更新好友的分组
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="fUid">好友id</param>
        /// <param name="groupId">组id</param>
        /// <returns></returns>
        public static int UpdateFriendGroup(int uid, int fUid, int groupId)
        {
            if (uid <= 0 || fUid <= 0 || groupId < 0 || uid == fUid)
                return -1;

            return Data.Friendship.UpdateFriendGroup(uid, fUid, groupId);
        }

        /// <summary>
        /// 创建新的好友分组
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static int CreateFriendshipGroup(int ownerId, string groupName)
        {
            if (ownerId <= 0 || groupName == string.Empty)
                return -1;

            return Data.Friendship.CreateFriendshipGroup(ownerId, groupName);
        }

        /// <summary>
        /// 更新好友分组的好友数量
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int UpdateFriendshipGroupCount(int groupId, int count)
        {
            if (groupId <= 0)
                return -1;

            return Data.Friendship.UpdateFriendshipGroupCount(groupId, count);
        }

        /// <summary>
        /// 修改好友分组名称
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static int UpdateFriendshipGroupName(int groupId, string groupName)
        {
            if (groupId <= 0 || groupName == string.Empty)
                return -1;

            return Data.Friendship.UpdateFriendshipGroupName(groupId, groupName);
        }

        /// <summary>
        /// 获取分组列表
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public static List<FriendshipGroupInfo> GetFriendshipGroupsList(int ownerId)
        {
            if (ownerId <= 0)
                return null;

            return Data.Friendship.GetFriendshipGroupsList(ownerId);
        }

        /// <summary>
        /// 获取好友分组个数
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public static int GetFriendshipGroupsCount(int ownerId)
        {
            if (ownerId <= 0)
                return -1;

            return Data.Friendship.GetFriendshipGroupsCount(ownerId);
        }

        /// <summary>
        /// 获取单条分组信息
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static FriendshipGroupInfo GetFriendshipGroupInfo(int groupId)
        {
            if (groupId <= 0)
                return null;

            return Data.Friendship.GetFriendshipGroupInfo(groupId);
        }

        /// <summary>
        /// 返回json格式的好友分组列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetFriendshipGroupJson(int userId)
        {
            List<FriendshipGroupInfo> friendshipGroupList = Friendship.GetFriendshipGroupsList(userId);

            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append("{gid:");
            sb.Append(0);
            sb.Append(",gname:'");
            sb.Append("未分组");
            sb.Append("',ownerid:");
            sb.Append(userId);
            sb.Append(",friendcount:");
            sb.Append(0);
            sb.Append("},");
            foreach (FriendshipGroupInfo groupInfo in friendshipGroupList)
            {
                sb.Append("{gid:");
                sb.Append(groupInfo.GroupId);
                sb.Append(",gname:'");
                sb.Append(groupInfo.GroupName.Trim());
                sb.Append("',ownerid:");
                sb.Append(groupInfo.OwnerId);
                sb.Append(",friendcount:");
                sb.Append(groupInfo.FriendshipCount);
                sb.Append("},");
            }
            return sb.ToString().TrimEnd(',') + "]";
        }
    }
}
