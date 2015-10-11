using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Data.SqlServer
{
    public partial class DataProvider : IDataProvider
    {
        /// <summary>
        /// 创建好友请求信息
        /// </summary>
        /// <param name="friendshipInfo"></param>
        /// <returns></returns>
        public int CreateNewFriendshipRequest(FriendshipRequestInfo friendshipInfo)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@fromuid", (DbType)SqlDbType.Int, 4, friendshipInfo.FromUid),
                                        DbHelper.MakeInParam("@touid", (DbType)SqlDbType.Int, 4, friendshipInfo.ToUid),
                                        DbHelper.MakeInParam("@fromusername", (DbType)SqlDbType.Char, 20, friendshipInfo.FromUserName),
                                        DbHelper.MakeInParam("@gid", (DbType)SqlDbType.Int, 4, friendshipInfo.GroupId),
                                        DbHelper.MakeInParam("@note", (DbType)SqlDbType.NVarChar, 100, friendshipInfo.Note)
                                    };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}createnewfriendshiprequest", BaseConfigs.GetTablePrefix), parms));
        }
        /// <summary>
        /// 通过好友的请求信息
        /// </summary>
        /// <param name="friendshipInfo"></param>
        /// <returns></returns>
        public int PassFriendship(FriendshipRequestInfo friendshipInfo, int groupId)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@fromuid", (DbType)SqlDbType.Int, 4, friendshipInfo.FromUid),
                                        DbHelper.MakeInParam("@touid", (DbType)SqlDbType.Int, 4, friendshipInfo.ToUid),
                                        DbHelper.MakeInParam("@togroupid", (DbType)SqlDbType.Int, 4, groupId)
                                    };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}passfriendship", BaseConfigs.GetTablePrefix), parms));
        }
        /// <summary>
        /// 获取用户好友列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IDataReader GetUserFriendsList(int uid, int pageIndex, int pageSize)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
                                        DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int, 4, pageIndex),
                                        DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int, 4, pageSize)
                                  };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getuserfriendslist", BaseConfigs.GetTablePrefix), parms);
        }
        /// <summary>
        /// 获取用户好友数量
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int GetUserFriendsCount(int uid)
        {
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}getuserfriendscount", BaseConfigs.GetTablePrefix), DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)));
        }

        /// <summary>
        /// 获取用户好友请求信息
        /// </summary>
        /// <param name="fromUid"></param>
        /// <param name="toUid"></param>
        /// <returns></returns>
        public IDataReader GetUserFriendRequestInfo(int fromUid, int toUid)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@fromuid", (DbType)SqlDbType.Int, 4, fromUid),
                                        DbHelper.MakeInParam("@touid", (DbType)SqlDbType.Int, 4, toUid)
                                  };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getuserfriendshiprequestinfo", BaseConfigs.GetTablePrefix), parms);
        }


        /// <summary>
        /// 获取用户好友请求列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IDataReader GetUserFriendRequestList(int uid, int pageIndex, int pageSize)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
                                        DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int, 4, pageIndex),
                                        DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int, 4, pageSize)
                                  };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getuserfriendrequestlist", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 获取用户好友请求个数
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int GetUserFriendRequestCount(int uid)
        {
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}getuserfriendrequestcount", BaseConfigs.GetTablePrefix), DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)));
        }

        /// <summary>
        /// 获取站点用户好友关系变更日志
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public IDataReader GetFriendshipChangeLog(int count)
        {
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getfriendshipchangelog", BaseConfigs.GetTablePrefix), DbHelper.MakeInParam("@count", (DbType)SqlDbType.Int, 4, count));
        }

        /// <summary>
        /// 好友关系或请求关系是否存在（1：请求存在 2：存在 0：不存在）
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="friendUid"></param>
        /// <returns></returns>
        public int IsFriendshipExist(int uid, int friendUid)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
                                        DbHelper.MakeInParam("@fuid", (DbType)SqlDbType.Int, 4, friendUid)
                                    };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}isfriendshipexist", BaseConfigs.GetTablePrefix), parms));
        }

        /// <summary>
        /// 解除好友关系
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="friendUid"></param>
        /// <returns></returns>
        public int DeleteFriendship(int uid, string friendUidList)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
                                        DbHelper.MakeInParam("@fuidlist", (DbType)SqlDbType.NVarChar, 1000, friendUidList)
                                    };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}deletefriendship", BaseConfigs.GetTablePrefix), parms));
        }

        /// <summary>
        /// 忽略好友请求
        /// </summary>
        /// <param name="toUid"></param>
        /// <param name="fromUidList"></param>
        /// <returns></returns>
        public int IgnoreFriendshipRequest(int toUid, string fromUidList)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@touid", (DbType)SqlDbType.Int, 4, toUid),
                                        DbHelper.MakeInParam("@fromuidlist", (DbType)SqlDbType.NVarChar, 1000, fromUidList)
                                    };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}ignorefriendshiprequest", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 按查询条件获取好友列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="conditionTable"></param>
        /// <returns></returns>
        public IDataReader GetUserFriendsListByCondition(int uid, int pageIndex, int pageSize, Dictionary<FriendshipListSerachEnum, string> conditionTable)
        {
            string condition = GetCondition(conditionTable);
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
                                        DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int, 4, pageIndex),
                                        DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int, 4, pageSize),
                                        DbHelper.MakeInParam("@condition", (DbType)SqlDbType.NVarChar, 2000, condition)
                                  };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getuserfriendslistbycondition", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 按查询条件获取好友人数
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="conditionTable"></param>
        /// <returns></returns>
        public int GetUserFriendsCountByCondition(int uid, Dictionary<FriendshipListSerachEnum, string> conditionTable)
        {
            string condition = GetCondition(conditionTable);
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
                                        DbHelper.MakeInParam("@condition", (DbType)SqlDbType.NVarChar, 2000, condition)
                                  };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}getuserfriendscountbycondition", BaseConfigs.GetTablePrefix), parms));
        }

        /// <summary>
        /// 生成好友查询条件
        /// </summary>
        /// <param name="conditionTable"></param>
        /// <returns></returns>
        public string GetCondition(Dictionary<FriendshipListSerachEnum, string> conditionTable)
        {
            string condition = "";
            foreach (FriendshipListSerachEnum key in conditionTable.Keys)
            {
                switch (key)
                {
                    case FriendshipListSerachEnum.FriendUserName:
                        condition += "AND [fusername] Like '" + conditionTable[key] + "%' ";
                        break;
                    case FriendshipListSerachEnum.LastDateTime:
                        condition += "AND [datetime]<'" + conditionTable[key] + "' ";
                        break;
                    case FriendshipListSerachEnum.FriendGroupId:
                        condition += "AND [gid]=" + conditionTable[key] + " ";
                        break;
                }
            }
            return condition.TrimStart("AND".ToCharArray());
        }

        /// <summary>
        /// 更新好友的分组
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="fUid">好友id</param>
        /// <param name="groupId">组id</param>
        /// <returns></returns>
        public int UpdateFriendGroup(int uid, int fUid, int groupId)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
                                        DbHelper.MakeInParam("@fuid", (DbType)SqlDbType.Int, 4, fUid),
                                        DbHelper.MakeInParam("@gid", (DbType)SqlDbType.Int, 4, groupId)
                                  };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updatefriendgroup", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 创建好友分组
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public int CreateFriendshipGroup(int ownerId, string groupName)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@ownerid", (DbType)SqlDbType.Int, 4, ownerId),
                                        DbHelper.MakeInParam("@gname", (DbType)SqlDbType.Char, 20, groupName)
                                  };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}createfriendgroup", BaseConfigs.GetTablePrefix), parms));
        }

        /// <summary>
        /// 更新好友分组人数
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int UpdateFriendshipGroupCount(int groupId, int count)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@gid", (DbType)SqlDbType.Int, 4, groupId),
                                        DbHelper.MakeInParam("@count", (DbType)SqlDbType.Int,4, count)
                                  };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updatefriendgroupcount", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 更新组名
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public int UpdateFriendshipGroupName(int groupId, string groupName)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@gid", (DbType)SqlDbType.Int, 4, groupId),
                                        DbHelper.MakeInParam("@gname", (DbType)SqlDbType.Char, 20, groupName)
                                  };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updatefriendgroupname", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 获取好友分组列表
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public IDataReader GetFriendshipGroupsList(int ownerId)
        {
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getfriendgroupslist", BaseConfigs.GetTablePrefix), DbHelper.MakeInParam("@ownerid", (DbType)SqlDbType.Int, 4, ownerId));
        }

        /// <summary>
        /// 获取好友分组个数
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public int GetFriendshipGroupsCount(int ownerId)
        {
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}getfriendgroupscount", BaseConfigs.GetTablePrefix), DbHelper.MakeInParam("@ownerid", (DbType)SqlDbType.Int, 4, ownerId)));
        }
        /// <summary>
        /// 获取分组信息
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IDataReader GetFriendshipGroupInfo(int groupId)
        {
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getfriendgroupinfo", BaseConfigs.GetTablePrefix), DbHelper.MakeInParam("@gid", (DbType)SqlDbType.Int, 4, groupId));
        }
    }
}
