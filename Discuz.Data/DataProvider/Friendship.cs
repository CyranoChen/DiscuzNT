using System.Data;

using Discuz.Common;
using Discuz.Entity;
using Discuz.Common.Generic;
using System.Collections;

namespace Discuz.Data
{
    public class Friendship
    {
        /// <summary>
        /// 创建好友请求信息
        /// </summary>
        /// <param name="friendshipRequestInfo"></param>
        /// <returns></returns>
        public static int CreateNewFriendshipRequest(FriendshipRequestInfo friendshipInfo)
        {
            return DatabaseProvider.GetInstance().CreateNewFriendshipRequest(friendshipInfo);
        }

        /// <summary>
        /// 通过好友的请求信息
        /// </summary>
        /// <param name="friendshipRequestInfo"></param>
        /// <returns></returns>
        public static int PassFriendship(FriendshipRequestInfo friendshipInfo, int groupId)
        {
            return DatabaseProvider.GetInstance().PassFriendship(friendshipInfo, groupId);
        }

        /// <summary>
        /// 获取用户好友数量
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int GetUserFriendsCount(int uid)
        {
            return DatabaseProvider.GetInstance().GetUserFriendsCount(uid);
        }

        /// <summary>
        /// 获取用户好友请求数量
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int GetUserFriendRequestCount(int uid)
        {
            return DatabaseProvider.GetInstance().GetUserFriendRequestCount(uid);
        }

        /// <summary>
        /// 获取用户好友列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static List<FriendshipInfo> GetUserFriendsList(int uid, int pageIndex, int pageSize)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUserFriendsList(uid, pageIndex, pageSize);
            List<FriendshipInfo> friendshipList = new List<FriendshipInfo>();
            while (reader.Read())
            {
                friendshipList.Add(LoadFriendshipInfo(reader));
            }
            reader.Close();
            return friendshipList;
        }

        /// <summary>
        /// 获取站点用户好友关系变更日志
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<FriendshipActionInfo> GetFriendshipChangeLog(int count)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetFriendshipChangeLog(count);
            List<FriendshipActionInfo> actionList = new List<FriendshipActionInfo>();
            while (reader.Read())
            {
                actionList.Add(LoadFriendshipActionInfo(reader));
            }
            reader.Close();
            return actionList;
        }

        /// <summary>
        /// 好友关系或请求关系是否存在（1：请求存在 2：存在 0：不存在）
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="friendUid"></param>
        /// <returns></returns>
        public static int IsFriendshipExist(int uid, int friendUid)
        {
            return DatabaseProvider.GetInstance().IsFriendshipExist(uid, friendUid);
        }

        /// <summary>
        /// 获取用户好友请求列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public static List<FriendshipRequestInfo> GetUserFriendRequestList(int uid, int pageIndex, int pageSize)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUserFriendRequestList(uid, pageIndex, pageSize);
            List<FriendshipRequestInfo> actionList = new List<FriendshipRequestInfo>();
            while (reader.Read())
            {
                actionList.Add(LoadFriendshipRequestInfo(reader));
            }
            reader.Close();
            return actionList;
        }

        /// <summary>
        /// 获取用户好友请求信息
        /// </summary>
        /// <param name="fromUid"></param>
        /// <param name="toUid"></param>
        /// <returns></returns>
        public static FriendshipRequestInfo GetUserFriendRequestInfo(int fromUid, int toUid)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUserFriendRequestInfo(fromUid, toUid);
            FriendshipRequestInfo requestInfo = new FriendshipRequestInfo();

            if (reader.Read())
                requestInfo = LoadFriendshipRequestInfo(reader);
            reader.Close();
            return requestInfo;
        }

        /// <summary>
        /// 解除好友关系
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="friendUidList"></param>
        /// <returns></returns>
        public static int DeleteFriendship(int uid, string friendUidList)
        {
            return DatabaseProvider.GetInstance().DeleteFriendship(uid, friendUidList);
        }

        /// <summary>
        /// 忽略好友请求
        /// </summary>
        /// <param name="toUid"></param>
        /// <param name="fromUidList"></param>
        /// <returns></returns>
        public static int IgnoreFriendshipRequest(int toUid, string fromUidList)
        {
            return DatabaseProvider.GetInstance().IgnoreFriendshipRequest(toUid, fromUidList);
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
            IDataReader reader = DatabaseProvider.GetInstance().GetUserFriendsListByCondition(uid, pageIndex, pageSize, conditionTable);
            List<FriendshipInfo> friendshipList = new List<FriendshipInfo>();
            while (reader.Read())
            {
                friendshipList.Add(LoadFriendshipInfo(reader));
            }
            reader.Close();
            return friendshipList;
        }

        /// <summary>
        /// 按指定条件读取用户好友个数
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="conditionTable"></param>
        /// <returns></returns>
        public static int GetUserFriendsCountByCondition(int uid, Dictionary<FriendshipListSerachEnum, string> conditionTable)
        {
            return DatabaseProvider.GetInstance().GetUserFriendsCountByCondition(uid, conditionTable);
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
            return DatabaseProvider.GetInstance().UpdateFriendGroup(uid, fUid, groupId);
        }

        /// <summary>
        /// 创建新的好友分组
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static int CreateFriendshipGroup(int ownerId, string groupName)
        {
            return DatabaseProvider.GetInstance().CreateFriendshipGroup(ownerId,groupName);
        }

        /// <summary>
        /// 更新好友分组的好友数量
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int UpdateFriendshipGroupCount(int groupId, int count)
        {
            return DatabaseProvider.GetInstance().UpdateFriendshipGroupCount(groupId, count);
        }

        /// <summary>
        /// 修改好友分组名称
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static int UpdateFriendshipGroupName(int groupId, string groupName)
        {
            return DatabaseProvider.GetInstance().UpdateFriendshipGroupName(groupId,groupName);
        }

        /// <summary>
        /// 获取分组列表
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public static List<FriendshipGroupInfo> GetFriendshipGroupsList(int ownerId)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetFriendshipGroupsList(ownerId);

            List<FriendshipGroupInfo> friendshipGroupsList = new List<FriendshipGroupInfo>();
            while (reader.Read())
            {
                friendshipGroupsList.Add(LoadFriendshipGroupInfo(reader));
            }
            reader.Close();
            return friendshipGroupsList;
        }

        /// <summary>
        /// 获取好友分组个数
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public static int GetFriendshipGroupsCount(int ownerId)
        {
            return DatabaseProvider.GetInstance().GetFriendshipGroupsCount(ownerId);
        }

        /// <summary>
        /// 获取单条分组信息
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static FriendshipGroupInfo GetFriendshipGroupInfo(int groupId)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetFriendshipGroupInfo(groupId);

            FriendshipGroupInfo friendshipGroupInfo = new FriendshipGroupInfo();

            if (reader.Read())
                friendshipGroupInfo = LoadFriendshipGroupInfo(reader);
            reader.Close();

            return friendshipGroupInfo;
        }

        #region private method

        /// <summary>
        /// 从reader中装载FriendshipInfo
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static FriendshipInfo LoadFriendshipInfo(IDataReader reader)
        {
            FriendshipInfo friendshipInfo = new FriendshipInfo();
            friendshipInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
            friendshipInfo.FriendUid = TypeConverter.ObjectToInt(reader["fuid"]);
            friendshipInfo.FriendUserName = reader["fusername"].ToString();
            friendshipInfo.GroupId = TypeConverter.ObjectToInt(reader["gid"]);
            friendshipInfo.ExchangeNum = TypeConverter.ObjectToInt(reader["exchangenum"]);
            friendshipInfo.DateTime = reader["datetime"].ToString();
            return friendshipInfo;
        }

        /// <summary>
        /// 从reader中装载FriendshipRequestInfo
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static FriendshipRequestInfo LoadFriendshipRequestInfo(IDataReader reader)
        {
            FriendshipRequestInfo friendshipRequestInfo = new FriendshipRequestInfo();
            friendshipRequestInfo.FromUid = TypeConverter.ObjectToInt(reader["fromuid"]);
            friendshipRequestInfo.FromUserName = reader["fromusername"].ToString();
            friendshipRequestInfo.ToUid = TypeConverter.ObjectToInt(reader["touid"]);
            friendshipRequestInfo.GroupId = TypeConverter.ObjectToInt(reader["gid"]);
            friendshipRequestInfo.Note = reader["note"].ToString();
            friendshipRequestInfo.DateTime = reader["datetime"].ToString();
            return friendshipRequestInfo;
        }

        /// <summary>
        /// 从reader中装载FriendshipActionInfo
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static FriendshipActionInfo LoadFriendshipActionInfo(IDataReader reader)
        {
            FriendshipActionInfo actionInfo = new FriendshipActionInfo();
            actionInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
            actionInfo.FriendUid = TypeConverter.ObjectToInt(reader["fuid"]);
            actionInfo.Action = Utils.GetEnum(reader["action"].ToString(), FriendshipActionEnum.Add);
            return actionInfo;
        }

        /// <summary>
        /// 从reader中装载FriendshipGroupInfo
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static FriendshipGroupInfo LoadFriendshipGroupInfo(IDataReader reader)
        {
            FriendshipGroupInfo groupInfo = new FriendshipGroupInfo();
            groupInfo.GroupId = TypeConverter.ObjectToInt(reader["gid"]);
            groupInfo.GroupName=reader["gname"].ToString();
            groupInfo.OwnerId = TypeConverter.ObjectToInt(reader["ownerid"]);
            groupInfo.FriendshipCount = TypeConverter.ObjectToInt(reader["friendscount"]);

            return groupInfo;
        }

        #endregion
    }
}
