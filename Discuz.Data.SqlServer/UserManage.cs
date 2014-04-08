using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using Discuz.Data;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;
using Discuz.Common.Generic;

namespace Discuz.Data.SqlServer
{
    public partial class DataProvider : IDataProvider
    {
        private static int _lastRemoveTimeout;
        private static Dictionary<TrendType, int> stat = new Dictionary<TrendType, int>();
        private static DateTime lastTrendUpdateTime = DateTime.Now;

        static DataProvider()
        {
            Initial();
        }

        private static void Initial()
        {
            stat.Clear();
            stat.Add(TrendType.Login, 0);
            stat.Add(TrendType.Register, 0);
            stat.Add(TrendType.Topic, 0);
            stat.Add(TrendType.Poll, 0);
            stat.Add(TrendType.Bonus, 0);
            stat.Add(TrendType.Debate, 0);
            stat.Add(TrendType.Post, 0);
        }

        public DataTable GetUsers(string idList)
        {
            if (!Utils.IsNumericList(idList))
                return new DataTable();

            string commandText = string.Format("SELECT [uid],[username] FROM [{0}users] WHERE [groupid] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                idList);
            return DbHelper.ExecuteDatasetInMasterDB(CommandType.Text, commandText).Tables[0];
        }

        public DataTable GetUserGroupInfoByGroupid(int groupId)
        {
            string commandText = string.Format("SELECT TOP 1 {0} FROM  [{1}usergroups] WHERE [groupid]={2}",
                                                DbFields.USER_GROUPS,
                                                BaseConfigs.GetTablePrefix,
                                                groupId);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public DataTable GetMedal()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, GetMedalSql()).Tables[0];
        }

        public string GetMedalSql()
        {
            return string.Format("SELECT {0} FROM [{1}medals]", DbFields.MEDALS, BaseConfigs.GetTablePrefix);
        }

        public DataTable GetExistMedalList()
        {
            string commandText = string.Format("SELECT [medalid],[image] FROM [{0}medals] WHERE [image]<>''",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public void AddMedal(string name, int available, string image)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@medalid", (DbType)SqlDbType.SmallInt,2, GetMaxMedalId()),
				DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar,50, name),
                DbHelper.MakeInParam("@available", (DbType)SqlDbType.Int, 4, available),
				DbHelper.MakeInParam("@image",(DbType)SqlDbType.VarChar,30,image)
			};
            string commandText = string.Format("INSERT INTO [{0}medals] (medalid,name,available,image) Values (@medalid,@name,@available,@image)",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void UpdateMedal(int medalId, string name, string image)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@medalid", (DbType)SqlDbType.SmallInt,2, medalId),
				DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar,50, name),
				DbHelper.MakeInParam("@image",(DbType)SqlDbType.VarChar,30,image)
			};
            string commandText = string.Format("UPDATE [{0}medals] SET [name]=@name,[image]=@image  Where [medalid]=@medalid",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void SetAvailableForMedal(int available, string medalIdList)
        {
            DbParameter[] parms = {
				                       DbHelper.MakeInParam("@available", (DbType)SqlDbType.Int, 4, available)
			                      };
            string commandText = string.Format("UPDATE [{0}medals] SET [available]=@available WHERE [medalid] IN({1})",
                                                BaseConfigs.GetTablePrefix,
                                                medalIdList);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        private int GetMaxMedalId()
        {
            string commandText = string.Format("SELECT ISNULL(MAX(medalid), 0) FROM [{0}medals]", BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText)) + 1;
        }

        /// <summary>
        /// 获得到指定管理组信息
        /// </summary>
        /// <returns>管理组信息</returns>
        public DataTable GetAdminGroupList()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}admingroups]",
                                                DbFields.ADMIN_GROUPS,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDatasetInMasterDB(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 设置管理组信息
        /// </summary>
        /// <param name="__admingroupsInfo">管理组信息</param>
        /// <returns>更改记录数</returns>
        public int SetAdminGroupInfo(AdminGroupInfo adminGroupsInfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@admingid",(DbType)SqlDbType.SmallInt,2,adminGroupsInfo.Admingid),
									   DbHelper.MakeInParam("@alloweditpost",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Alloweditpost),
									   DbHelper.MakeInParam("@alloweditpoll",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Alloweditpoll),
									   DbHelper.MakeInParam("@allowstickthread",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowstickthread),
									   DbHelper.MakeInParam("@allowmodpost",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowmodpost),
									   DbHelper.MakeInParam("@allowdelpost",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowdelpost),
									   DbHelper.MakeInParam("@allowmassprune",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowmassprune),
									   DbHelper.MakeInParam("@allowrefund",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowrefund),
									   DbHelper.MakeInParam("@allowcensorword",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowcensorword),
									   DbHelper.MakeInParam("@allowviewip",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowviewip),
									   DbHelper.MakeInParam("@allowbanip",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowbanip),
									   DbHelper.MakeInParam("@allowedituser",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowedituser),
									   DbHelper.MakeInParam("@allowmoduser",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowmoduser),
									   DbHelper.MakeInParam("@allowbanuser",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowbanuser),
									   DbHelper.MakeInParam("@allowpostannounce",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowpostannounce),
									   DbHelper.MakeInParam("@allowviewlog",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowviewlog),
									   DbHelper.MakeInParam("@disablepostctrl",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Disablepostctrl),
                                       DbHelper.MakeInParam("@allowviewrealname",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowviewrealname)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateadmingroup", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 创建一个新的管理组信息
        /// </summary>
        /// <param name="__admingroupsInfo">要添加的管理组信息</param>
        /// <returns>更改记录数</returns>
        public int CreateAdminGroupInfo(AdminGroupInfo adminGroupsInfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@admingid",(DbType)SqlDbType.SmallInt,2,adminGroupsInfo.Admingid),
									   DbHelper.MakeInParam("@alloweditpost",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Alloweditpost),
									   DbHelper.MakeInParam("@alloweditpoll",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Alloweditpoll),
									   DbHelper.MakeInParam("@allowstickthread",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowstickthread),
									   DbHelper.MakeInParam("@allowmodpost",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowmodpost),
									   DbHelper.MakeInParam("@allowdelpost",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowdelpost),
									   DbHelper.MakeInParam("@allowmassprune",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowmassprune),
									   DbHelper.MakeInParam("@allowrefund",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowrefund),
									   DbHelper.MakeInParam("@allowcensorword",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowcensorword),
									   DbHelper.MakeInParam("@allowviewip",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowviewip),
									   DbHelper.MakeInParam("@allowbanip",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowbanip),
									   DbHelper.MakeInParam("@allowedituser",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowedituser),
									   DbHelper.MakeInParam("@allowmoduser",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowmoduser),
									   DbHelper.MakeInParam("@allowbanuser",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowbanuser),
									   DbHelper.MakeInParam("@allowpostannounce",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowpostannounce),
									   DbHelper.MakeInParam("@allowviewlog",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowviewlog),
									   DbHelper.MakeInParam("@disablepostctrl",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Disablepostctrl),
                                       DbHelper.MakeInParam("@allowviewrealname",(DbType)SqlDbType.TinyInt,1,adminGroupsInfo.Allowviewrealname)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}createadmingroup", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 删除指定的管理组信息
        /// </summary>
        /// <param name="admingid">管理组ID</param>
        /// <returns>更改记录数</returns>
        public int DeleteAdminGroupInfo(short adminGid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@admingid",(DbType)SqlDbType.SmallInt,2,adminGid),
								   };
            string commandText = string.Format("DELETE FROM [{0}admingroups] WHERE [admingid] = @admingid",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }


        public void UpdateRaterangeByGroupid(string rateRange, int groupId)
        {
            DbParameter[] parms = 
			{
                DbHelper.MakeInParam("@raterange",(DbType)SqlDbType.NChar, 500,rateRange),
				DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.Int, 4,groupId)
			};
            string commandText = string.Format("UPDATE [{0}usergroups] SET [raterange]=@raterange WHERE [groupid]=@groupid",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void ClearAuthstrByUidlist(string uidList)
        {
            string commandText = string.Format("UPDATE [{0}userfields] SET [authstr]='' WHERE [uid] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                uidList);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }


        public void DeleteUserByUidlist(string uidList)
        {
            string commandText = string.Format("DELETE FROM [{0}userfields] WHERE [uid] IN({1})",
                                                BaseConfigs.GetTablePrefix,
                                                uidList);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);

            commandText = string.Format("DELETE FROM [{0}users] WHERE [uid] IN({1})",
                                         BaseConfigs.GetTablePrefix,
                                         uidList);
            int deleteUserCount = DbHelper.ExecuteNonQuery(CommandType.Text, commandText);

            commandText = string.Format("UPDATE [{0}statistics] SET [totalusers]=[totalusers]-{1}",
                                         BaseConfigs.GetTablePrefix,
                                         deleteUserCount);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }


        public DataTable GetUsersByUidlLst(string uidList)
        {
            string commandText = string.Format("SELECT {0} FROM [{1}users] WHERE [uid] IN ({2})",
                                                DbFields.USERS,
                                                BaseConfigs.GetTablePrefix,
                                                uidList);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public DataTable GetUserGroup()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}usergroups] WHERE [radminid]= 0 AND [groupid]>8 AND [system]=0 ORDER BY [groupid]",
                                                DbFields.USER_GROUPS,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public string GetUserGroupTitle()
        {
            return string.Format("SELECT [groupid],[grouptitle] FROM [{0}usergroups] WHERE [radminid]= 0 And [groupid]>8 ORDER BY [groupid]",
                                  BaseConfigs.GetTablePrefix);
        }

        public DataTable GetUserGroupWithOutGuestTitle()
        {
            string commandText = string.Format("SELECT [groupid],[grouptitle] FROM [{0}usergroups] WHERE [groupid]<>7  ORDER BY [groupid] ASC",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(commandText).Tables[0];
        }

        public void DeleteUserGroupInfo(int groupId)
        {
            string commandText = string.Format("DELETE FROM [{0}usergroups] Where [groupid]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                groupId);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public void ChangeUsergroup(int sourceUserGroupId, int targetUserGroupId)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@soureceusergroupid",(DbType)SqlDbType.Int, 4,sourceUserGroupId),
                DbHelper.MakeInParam("@targetusergroupid",(DbType)SqlDbType.Int, 4,targetUserGroupId)
			};
            string commandText = string.Format("UPDATE [{0}users] SET [groupid]=@targetusergroupid WHERE [groupid]=@soureceusergroupid",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void UpdateUserAdminIdByGroupId(int adminId, int groupId)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@adminid",(DbType)SqlDbType.Int, 4,adminId),
                DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.Int, 4,groupId)
			};
            string commandText = string.Format("UPDATE [{0}users] SET [adminid]=@adminid WHERE [groupid]=@groupid",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public bool IsExistMedalAwardRecord(int medalId, int userId)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@medalid", (DbType)SqlDbType.Int,4, medalId),
				DbHelper.MakeInParam("@userid",(DbType)SqlDbType.Int,4,userId)
			};
            string commandText = string.Format("SELECT TOP 1 ID FROM [{0}medalslog] WHERE [medals]=@medalid AND [uid]=@userid",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0].Rows.Count != 0;
        }

        public void AddMedalslog(int adminId, string adminName, string ip, string userName, int uid, string actions, int medals, string reason)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@adminid", (DbType)SqlDbType.Int,4, adminId),
				DbHelper.MakeInParam("@adminname",(DbType)SqlDbType.NVarChar,50,adminName),
                DbHelper.MakeInParam("@ip", (DbType)SqlDbType.NVarChar,15, ip),
				DbHelper.MakeInParam("@username",(DbType)SqlDbType.NVarChar,50,userName),
                DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int,4, uid),
				DbHelper.MakeInParam("@actions",(DbType)SqlDbType.NVarChar,100,actions),
                DbHelper.MakeInParam("@medals", (DbType)SqlDbType.Int,4, medals),
				DbHelper.MakeInParam("@reason",(DbType)SqlDbType.NVarChar,100,reason)
			};
            string commandText = string.Format("INSERT INTO [{0}medalslog] (adminid,adminname,ip,username,uid,actions,medals,reason) VALUES (@adminid,@adminname,@ip,@username,@uid,@actions,@medals,@reason)",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void UpdateMedalslog(string newActions, DateTime postDateTime, string reason, string oldActions, int medals, int uid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@newactions",(DbType)SqlDbType.NVarChar,100,newActions),
                DbHelper.MakeInParam("@postdatetime",(DbType)SqlDbType.DateTime,8,postDateTime),
				DbHelper.MakeInParam("@reason",(DbType)SqlDbType.NVarChar,100,reason),
                DbHelper.MakeInParam("@oldactions",(DbType)SqlDbType.NVarChar,100,oldActions),
                DbHelper.MakeInParam("@medals", (DbType)SqlDbType.Int,4, medals),
                DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int,4, uid)
			};
            string commandText = string.Format("UPDATE [{0}medalslog] SET [actions]=@newactions ,[postdatetime]=@postdatetime, reason=@reason  WHERE [actions]=@oldactions AND [medals]=@medals  AND [uid]=@uid",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void UpdateMedalslog(string actions, DateTime postDateTime, string reason, int uid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@actions",(DbType)SqlDbType.NVarChar,100,actions),
                DbHelper.MakeInParam("@postdatetime",(DbType)SqlDbType.DateTime,8,postDateTime),
				DbHelper.MakeInParam("@reason",(DbType)SqlDbType.NVarChar,100,reason),
                DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int,4, uid)
			};
            string commandText = string.Format("Update [{0}medalslog] SET [actions]=@actions ,[postdatetime]=@postdatetime,[reason]=@reason  WHERE [uid]=@uid",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void SetStopTalkUser(string uidList)
        {
            string commandText = string.Format("UPDATE [{0}users] SET [groupid]=4, [adminid]=0, [groupexpiry]=0 WHERE [uid] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                uidList);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public void ChangeUserGroupByUid(int groupId, string uidList)
        {
            string commandText = string.Format("UPDATE [{0}users] SET [groupid]={1}  WHERE [uid] IN ({2})",
                                                BaseConfigs.GetTablePrefix,
                                                groupId,
                                                uidList);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public void DeletePostByPosterid(int tableId, int posterId)
        {
            string commandText = string.Format("DELETE FROM  [{0}posts{1}]  WHERE [posterid]={2}",
                                                BaseConfigs.GetTablePrefix,
                                                tableId,
                                                posterId);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 删除指定用户附件
        /// </summary>
        /// <param name="uid"></param>
        public void DeleteAttachmentByUid(int uid, int days)
        {
            DbParameter[] parms = {
                                      DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
                                      DbHelper.MakeInParam("@days", (DbType)SqlDbType.Int, 4, days)
                                  };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}deleteAttachmentByUid", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 删除用户指定天数内的帖子
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="days">天数</param>
        public void DeletePostByUidAndDays(int uid, int days)
        {
            DbParameter[] parms = { 
                                    DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
                                    DbHelper.MakeInParam("@days",(DbType)SqlDbType.Int,4,days)
                                  };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}deletepostsbyuidanddays", BaseConfigs.GetTablePrefix), parms);
        }

        public void DeleteTopicByPosterid(int posterId)
        {
            string commandText = string.Format("DELETE FROM [{0}topics] WHERE [posterid]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                posterId);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public void ClearPosts(int uid)
        {
            string commandText = string.Format("UPDATE [{0}users] SET [digestposts]=0 , [posts]=0  WHERE [uid]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                uid);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public void UpdateEmailValidateInfo(string authStr, DateTime authTime, int uid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@authstr",(DbType)SqlDbType.VarChar,20,authStr),
                DbHelper.MakeInParam("@authtime",(DbType)SqlDbType.DateTime,8,authTime),
                DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int,4, uid)
			};
            string commandText = string.Format("UPDATE [{0}userfields] SET [Authstr]=@authstr,[Authtime]=@authtime ,[Authflag]=1  WHERE [uid]=@uid",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }


        public DataTable GetUserEmailByGroupid(string groupIdList)
        {
            string commandText = string.Format("SELECT [username],[Email]  From [{0}users] WHERE [Email] Is Not null AND [Email]<>'' AND [groupid] IN({1})",
                                                BaseConfigs.GetTablePrefix,
                                                groupIdList);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public DataTable GetUserGroupExceptGroupid(int groupId)
        {
            string commandText = string.Format("SELECT [groupid] FROM [{0}usergroups] WHERE [radminid]=0 And [groupid]>8 AND [groupid]<>{1}",
                                                BaseConfigs.GetTablePrefix,
                                                groupId);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }


        /// <summary>
        /// 创建收藏信息
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="tid">主题ID</param>
        /// <param name="type">收藏类型，0=主题，1=相册，2=博客日志</param>
        /// <returns>创建成功返回 1 否则返回 0</returns>	
        public int CreateFavorites(int uid, int tid, byte type)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid),
                                       DbHelper.MakeInParam("@type", (DbType)SqlDbType.TinyInt, 4, type)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}createfavorite", BaseConfigs.GetTablePrefix), parms);
        }



        /// <summary>
        /// 删除指定用户的收藏信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="fitemid">要删除的收藏信息id列表,以英文逗号分割</param>
        /// <returns>删除的条数．出错时返回 -1</returns>
        public int DeleteFavorites(int uid, string fidList, byte type)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int,4, uid),
                                       DbHelper.MakeInParam("@typeid", (DbType)SqlDbType.TinyInt, 1, type)
			                        };
            string commandText = string.Format("DELETE FROM [{0}favorites] WHERE [tid] IN ({1}) AND [uid] = @uid AND [typeid]=@typeid",
                                                BaseConfigs.GetTablePrefix,
                                                fidList);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }


        /// <summary>
        /// 得到用户收藏信息列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="pagesize">分页时每页的记录数</param>
        /// <param name="pageindex">当前页码</param>
        /// <param name="typeid">收藏类型id</param>
        /// <returns>用户信息列表</returns>
        public DataTable GetFavoritesList(int uid, int pageSize, int pageIndex, int typeId)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									   DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pageSize),
									   DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,pageIndex)								   
								   };
            switch (typeId)
            {
                case 1:
                    return DbHelper.ExecuteDataset(CommandType.StoredProcedure,
                                                   string.Format("{0}getfavoriteslistbyalbum", BaseConfigs.GetTablePrefix),
                                                   parms).Tables[0];
                case 2:
                    return DbHelper.ExecuteDataset(CommandType.StoredProcedure,
                                                   string.Format("{0}getfavoriteslistbyspacepost", BaseConfigs.GetTablePrefix),
                                                   parms).Tables[0];
                case 3:
                    {   //获取收藏的商品信息
                        string commandText = string.Format("SELECT [f].[tid], [f].[uid], [goodsid], [shopid], [categoryid] , [title] , [price], [selleruid], [seller], [dateline], [expiration]  FROM [{0}favorites] [f],[{0}goods] [goods] WHERE [f].[tid]=[goods].[goodsid] AND [f].[typeid]=3  AND [f].[uid]={1} ",
                                                            BaseConfigs.GetTablePrefix,
                                                            uid);
                        if (pageIndex == 1)
                            commandText = string.Format("SELECT TOP {0}  [tid], [uid], [goodsid], [shopid], [categoryid] , [title] , [price], [selleruid] AS [posterid], [seller] AS [poster], [dateline] AS [postdatetime], [expiration]  FROM ( {1} ) f  ORDER BY [tid] DESC",
                                                         pageSize,
                                                         commandText);
                        else
                            commandText = string.Format("SELECT TOP {0}  [tid], [uid], [goodsid], [shopid], [categoryid] , [title] , [price], [selleruid] AS [posterid], [seller] AS [poster], [dateline] AS [postdatetime], [expiration]  FROM ( {1} ) f1 WHERE [tid] < (SELECT MIN([tid]) FROM (SELECT TOP {2} [tid] FROM ({1}) f2  ORDER BY [tid] DESC) AS tblTmp) ORDER BY [tid] DESC",
                                                         pageSize,
                                                         commandText,
                                                         (pageIndex - 1) * pageSize);
                        return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
                    }
                default:
                    return DbHelper.ExecuteDataset(CommandType.StoredProcedure,
                                                   string.Format("{0}getfavoriteslist", BaseConfigs.GetTablePrefix),
                                                   parms).Tables[0];
            }
        }

        public int GetFavoritesCount(int uid, int typeId)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int, 4, uid),
                                       DbHelper.MakeInParam("@typeid",(DbType)SqlDbType.SmallInt, 2, typeId)
								   };
            return TypeConverter.ObjectToInt(
                         DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                string.Format("{0}getfavoritescount", BaseConfigs.GetTablePrefix),
                                                parms));
        }

        /// <summary>
        /// 收藏夹里是否包含了指定的主题
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="tid">主题id</param>
        /// <returns></returns>
        public int CheckFavoritesIsIN(int uid, int tid, byte type)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid),
                                        DbHelper.MakeInParam("@type", (DbType)SqlDbType.TinyInt, 1, type)
			};
            string commandText = string.Format("SELECT COUNT([tid]) AS [tidcount] FROM [{0}favorites] WHERE [tid]=@tid AND [uid]=@uid AND [typeid]=@type",
                                                BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms));
        }

        /// <summary>
        /// 更新用户收藏条目的查看时间
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="tid">主题id</param>
        /// <returns></returns>
        public int UpdateUserFavoriteViewTime(int uid, int tid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
			};

            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateuserfavoriteviewtime", BaseConfigs.GetTablePrefix), parms);
        }

        public void UpdateUserShortInfo(string location, string bio, string signature, int uid)
        {
            //更新签名 location来自 bio个人介绍
            DbParameter[] parms ={
                                        DbHelper.MakeInParam("@signature",(DbType)SqlDbType.NVarChar,500,signature),
                                        DbHelper.MakeInParam("@location",(DbType)SqlDbType.NVarChar,50,location),
                                        DbHelper.MakeInParam("@bio",(DbType)SqlDbType.NVarChar,50,bio),
                                        DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid)        
                                    };
            string commandText = string.Format("Update [{0}userfields] SET signature=@signature, location=@location,bio=@bio WHERE uid=@uid",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void DeleteModerator(int uid)
        {
            string commandText = string.Format("DELETE FROM [{0}moderators] WHERE [uid]={1}", BaseConfigs.GetTablePrefix, uid);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }


        public void UpdatePMSenderAndReceiver(int uid, string newUserName)
        {
            DbParameter[] parms =  { 
                                        DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
                                        DbHelper.MakeInParam("@username", (DbType)SqlDbType.VarChar, 20, newUserName)
                                    };
            string commandText = string.Format("UPDATE [{0}pms] SET [msgfrom]=@username WHERE [msgfromid]=@uid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            commandText = string.Format("UPDATE [{0}pms] SET [msgto]=@username  WHERE [msgtoid]=@uid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        //public DataTable GetModerators(string oldUserName)
        //{
        //    DbParameter[] parms = {
        //        DbHelper.MakeInParam("@oldusername", (DbType)SqlDbType.VarChar, 20, RegEsc(oldUserName))
        //                          };
        //    string commandText = string.Format("SELECT [fid],[moderators] FROM  [{0}forumfields] WHERE [moderators] LIKE '% @oldusername %'",
        //                                        BaseConfigs.GetTablePrefix);
        //    return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        //}


        public void CombinationUser(string postTableName, UserInfo targetUserInfo, UserInfo srcUserInfo)
        {
            DbParameter[] parms = {
					                DbHelper.MakeInParam("@target_uid", (DbType)SqlDbType.Int, 4, targetUserInfo.Uid),
					                DbHelper.MakeInParam("@target_username", (DbType)SqlDbType.NChar, 20, targetUserInfo.Username.Trim()),
					                DbHelper.MakeInParam("@src_uid", (DbType)SqlDbType.Int, 4, srcUserInfo.Uid)
				                  };
            string commandText = string.Format("UPDATE  [{0}topics] SET [posterid]=@target_uid,[poster]=@target_username  WHERE [posterid]=@src_uid",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            commandText = string.Format("UPDATE  [{0}users] SET [posts]={1} WHERE [uid]=@target_uid",
                                         BaseConfigs.GetTablePrefix, srcUserInfo.Posts + targetUserInfo.Posts);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            commandText = string.Format("UPDATE  [{0}] SET [posterid]=@target_uid,[poster]=@target_username  WHERE [posterid]=@src_uid", postTableName);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            commandText = string.Format("UPDATE  [{0}pms] SET [msgtoid]=@target_uid,[msgto]=@target_username  WHERE [msgtoid]=@src_uid",
                                         BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            commandText = string.Format("UPDATE  [{0}attachments] SET [uid]=@target_uid WHERE [uid]=@src_uid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 通过用户名得到UID
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GetUidByUserName(string userName)
        {
            DbParameter[] parms = 
            {
				DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar, 20, userName)
			};

            string commandText = string.Format("SELECT TOP 1 [uid] FROM [{0}users] WHERE [username]=@username", BaseConfigs.GetTablePrefix);
            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
            return dt.Rows.Count > 0 ? TypeConverter.ObjectToInt(dt.Rows[0][0]) : 0;
        }

        /// <summary>
        /// 删除指定用户的所有信息
        /// </summary>
        /// <param name="uid">指定的用户uid</param>
        /// <param name="delposts">是否删除帖子</param>
        /// <param name="delpms">是否删除短消息</param>
        /// <returns></returns>
        public bool DelUserAllInf(int uid, bool delPosts, bool delPms)
        {
            SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
            conn.Open();
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    DbParameter[] parms = {
                                            DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
						                    DbHelper.MakeInParam("@delPosts",(DbType)SqlDbType.Bit,1, delPosts ? 1 : 0),
						                    DbHelper.MakeInParam("@delPms",(DbType)SqlDbType.Bit,1,delPms ? 1 : 0)
                                          };
                    DbHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, string.Format("{0}deluserallinf", BaseConfigs.GetTablePrefix), parms);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            conn.Close();
            return true;
        }

        public DataTable GetUserGroup(int groupId)
        {
            string commandText = string.Format("SELECT {0} FROM [{1}usergroups] WHERE [groupid]={2}",
                                                DbFields.USER_GROUPS,
                                                BaseConfigs.GetTablePrefix,
                                                groupId);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }


        public void AddUserGroup(UserGroupInfo userGroupInfo)
        {
            DbParameter[] parms = 
					{
						DbHelper.MakeInParam("@Radminid",(DbType)SqlDbType.Int,4,userGroupInfo.Radminid),
						DbHelper.MakeInParam("@Grouptitle",(DbType)SqlDbType.NVarChar,50, Utils.RemoveFontTag(userGroupInfo.Grouptitle)),
						DbHelper.MakeInParam("@Creditshigher",(DbType)SqlDbType.Int,4,userGroupInfo.Creditshigher),
						DbHelper.MakeInParam("@Creditslower",(DbType)SqlDbType.Int,4,userGroupInfo.Creditslower),
						DbHelper.MakeInParam("@Stars",(DbType)SqlDbType.Int,4,userGroupInfo.Stars),
						DbHelper.MakeInParam("@Color",(DbType)SqlDbType.Char,7,userGroupInfo.Color),
						DbHelper.MakeInParam("@Groupavatar",(DbType)SqlDbType.NVarChar,60,userGroupInfo.Groupavatar),
						DbHelper.MakeInParam("@Readaccess",(DbType)SqlDbType.Int,4,userGroupInfo.Readaccess),
						DbHelper.MakeInParam("@Allowvisit",(DbType)SqlDbType.Int,4,userGroupInfo.Allowvisit),
						DbHelper.MakeInParam("@Allowpost",(DbType)SqlDbType.Int,4,userGroupInfo.Allowpost),
						DbHelper.MakeInParam("@Allowreply",(DbType)SqlDbType.Int,4,userGroupInfo.Allowreply),
						DbHelper.MakeInParam("@Allowpostpoll",(DbType)SqlDbType.Int,4,userGroupInfo.Allowpostpoll),
						DbHelper.MakeInParam("@Allowdirectpost",(DbType)SqlDbType.Int,4,userGroupInfo.Allowdirectpost),
						DbHelper.MakeInParam("@Allowgetattach",(DbType)SqlDbType.Int,4,userGroupInfo.Allowgetattach),
						DbHelper.MakeInParam("@Allowpostattach",(DbType)SqlDbType.Int,4,userGroupInfo.Allowpostattach),
						DbHelper.MakeInParam("@Allowvote",(DbType)SqlDbType.Int,4,userGroupInfo.Allowvote),
						DbHelper.MakeInParam("@Allowmultigroups",(DbType)SqlDbType.Int,4,userGroupInfo.Allowmultigroups),
						DbHelper.MakeInParam("@Allowsearch",(DbType)SqlDbType.Int,4,userGroupInfo.Allowsearch),
						DbHelper.MakeInParam("@Allowavatar",(DbType)SqlDbType.Int,4,userGroupInfo.Allowavatar),
						DbHelper.MakeInParam("@Allowcstatus",(DbType)SqlDbType.Int,4,userGroupInfo.Allowcstatus),
						DbHelper.MakeInParam("@Allowuseblog",(DbType)SqlDbType.Int,4,userGroupInfo.Allowuseblog),
						DbHelper.MakeInParam("@Allowinvisible",(DbType)SqlDbType.Int,4,userGroupInfo.Allowinvisible),
						DbHelper.MakeInParam("@Allowtransfer",(DbType)SqlDbType.Int,4,userGroupInfo.Allowtransfer),
						DbHelper.MakeInParam("@Allowsetreadperm",(DbType)SqlDbType.Int,4,userGroupInfo.Allowsetreadperm),
						DbHelper.MakeInParam("@Allowsetattachperm",(DbType)SqlDbType.Int,4,userGroupInfo.Allowsetattachperm),
						DbHelper.MakeInParam("@Allowhidecode",(DbType)SqlDbType.Int,4,userGroupInfo.Allowhidecode),
						DbHelper.MakeInParam("@Allowhtml",(DbType)SqlDbType.Int,4,userGroupInfo.Allowhtml),
                        DbHelper.MakeInParam("@Allowhtmltitle",(DbType)SqlDbType.Int,4,userGroupInfo.Allowhtmltitle),
						DbHelper.MakeInParam("@Allowcusbbcode",(DbType)SqlDbType.Int,4,userGroupInfo.Allowcusbbcode),
						DbHelper.MakeInParam("@Allownickname",(DbType)SqlDbType.Int,4,userGroupInfo.Allownickname),
						DbHelper.MakeInParam("@Allowsigbbcode",(DbType)SqlDbType.Int,4,userGroupInfo.Allowsigbbcode),
						DbHelper.MakeInParam("@Allowsigimgcode",(DbType)SqlDbType.Int,4,userGroupInfo.Allowsigimgcode),
						DbHelper.MakeInParam("@Allowviewpro",(DbType)SqlDbType.Int,4,userGroupInfo.Allowviewpro),
						DbHelper.MakeInParam("@Allowviewstats",(DbType)SqlDbType.Int,4,userGroupInfo.Allowviewstats),
                        DbHelper.MakeInParam("@Allowtrade",(DbType)SqlDbType.Int,4,userGroupInfo.Allowtrade),
                        DbHelper.MakeInParam("@Allowdiggs",(DbType)SqlDbType.Int,4,userGroupInfo.Allowdiggs),
                        DbHelper.MakeInParam("@Allowdebate",(DbType)SqlDbType.Int,4,userGroupInfo.Allowdebate),
                        DbHelper.MakeInParam("@Allowbonus",(DbType)SqlDbType.Int,4,userGroupInfo.Allowbonus),
                        DbHelper.MakeInParam("@Minbonusprice",(DbType)SqlDbType.Int,4,userGroupInfo.Minbonusprice),
                        DbHelper.MakeInParam("@Maxbonusprice",(DbType)SqlDbType.Int,4,userGroupInfo.Maxbonusprice),
						DbHelper.MakeInParam("@Disableperiodctrl",(DbType)SqlDbType.Int,4,userGroupInfo.Disableperiodctrl),
						DbHelper.MakeInParam("@Reasonpm",(DbType)SqlDbType.Int,4,userGroupInfo.Reasonpm),
						DbHelper.MakeInParam("@Maxprice",(DbType)SqlDbType.SmallInt,2,userGroupInfo.Maxprice),
						DbHelper.MakeInParam("@Maxpmnum",(DbType)SqlDbType.SmallInt,2,userGroupInfo.Maxpmnum),
						DbHelper.MakeInParam("@Maxsigsize",(DbType)SqlDbType.SmallInt,2,userGroupInfo.Maxsigsize),
						DbHelper.MakeInParam("@Maxattachsize",(DbType)SqlDbType.Int,4,userGroupInfo.Maxattachsize),
						DbHelper.MakeInParam("@Maxsizeperday",(DbType)SqlDbType.Int,4,userGroupInfo.Maxsizeperday),
						DbHelper.MakeInParam("@Attachextensions",(DbType)SqlDbType.Char,100,userGroupInfo.Attachextensions),
                        DbHelper.MakeInParam("@Maxspaceattachsize",(DbType)SqlDbType.Int,4,userGroupInfo.Maxspaceattachsize),
                        DbHelper.MakeInParam("@Maxspacephotosize",(DbType)SqlDbType.Int,4,userGroupInfo.Maxspacephotosize),
						DbHelper.MakeInParam("@Raterange",(DbType)SqlDbType.Char,100,userGroupInfo.Raterange),
                        //DbHelper.MakeInParam("@Maxfriendscount",(DbType)SqlDbType.Int,4,userGroupInfo.MaxFriendsCount),
                        DbHelper.MakeInParam("@ModNewTopics",(DbType)SqlDbType.SmallInt,2,userGroupInfo.ModNewTopics),
                        DbHelper.MakeInParam("@ModNewPosts",(DbType)SqlDbType.SmallInt,2,userGroupInfo.ModNewPosts),
                        DbHelper.MakeInParam("@Ignoreseccode",(DbType)SqlDbType.Int,4,userGroupInfo.Ignoreseccode)
					};

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}addusergroup", BaseConfigs.GetTablePrefix), parms);
        }

        public void AddOnlineList(string groupTitle)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, GetMaxUserGroupId()),
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 50, groupTitle)
                                    };
            string commandText = string.Format("INSERT INTO [{0}onlinelist] ([groupid], [title], [img]) VALUES(@groupid,@title, '')",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public DataTable GetMinCreditHigher()
        {
            string commandText = string.Format("SELECT MIN(Creditshigher) FROM [{0}usergroups] WHERE [groupid]>8 AND [radminid]=0 ",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public DataTable GetMaxCreditLower()
        {
            string commandText = string.Format("SELECT MAX(Creditslower) FROM [{0}usergroups] WHERE [groupid]>8 AND [radminid]=0 ",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public DataTable GetUserGroupByCreditshigher(int creditsHigher)
        {
            DbParameter parm = DbHelper.MakeInParam("@Creditshigher", (DbType)SqlDbType.Int, 4, creditsHigher);
            string commandText = string.Format("SELECT TOP 1 [groupid],[creditshigher],[creditslower] FROM [{0}usergroups] WHERE [groupid]>8 AND [radminid]=0  AND [Creditshigher]<=@Creditshigher AND @Creditshigher<[Creditslower]",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parm).Tables[0];
        }


        public void UpdateUserGroupCreidtsLower(int currentCreditsHigher, int creditsHigher)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@creditslower", (DbType)SqlDbType.Int, 4, creditsHigher),
                                        DbHelper.MakeInParam("@creditshigher", (DbType)SqlDbType.Int, 4, currentCreditsHigher)
                                    };
            string commandText = string.Format("UPDATE [{0}usergroups] SET [creditslower]=@creditslower WHERE [groupid]>8 AND [radminid]=0 AND [creditshigher]=@creditshigher",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public DataTable GetUserGroupByCreditsHigherAndLower(int creditsHigher, int creditsLower)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@Creditshigher", (DbType)SqlDbType.Int, 4, creditsHigher),
                                        DbHelper.MakeInParam("@Creditslower", (DbType)SqlDbType.Int, 4, creditsLower)
                                    };
            string commandText = string.Format("SELECT [groupid] FROM [{0}usergroups] WHERE [groupid]>8 AND [radminid]=0 AND [Creditshigher]=@Creditshigher AND [Creditslower]=@Creditslower",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        public int GetGroupCountByCreditsLower(int creditsHigher)
        {
            string commandText = string.Format("SELECT [groupid] FROM [{0}usergroups] WHERE [groupid]>8 AND [radminid]=0 AND [creditslower]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                creditsHigher);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows.Count;
        }

        public void UpdateUserGroupsCreditsLowerByCreditsLower(int creditsLower, int creditsHigher)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@Creditshigher", (DbType)SqlDbType.Int, 4, creditsHigher),
                                        DbHelper.MakeInParam("@Creditslower", (DbType)SqlDbType.Int, 4, creditsLower)
                                    };
            string commandText = string.Format("UPDATE [{0}usergroups] SET [creditslower]=@Creditslower WHERE [groupid]>8 AND [radminid]=0 AND [creditslower]=@Creditshigher",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteDataset(CommandType.Text, commandText, parms);
        }


        public void UpdateUserGroupsCreditsHigherByCreditsHigher(int creditsHigher, int creditsLower)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@Creditshigher", (DbType)SqlDbType.Int, 4, creditsHigher),
                                        DbHelper.MakeInParam("@Creditslower", (DbType)SqlDbType.Int, 4, creditsLower)
            };
            string commandText = string.Format("UPDATE [{0}usergroups] SET [Creditshigher]=@Creditshigher WHERE [groupid]>8 AND [radminid]=0 AND [Creditshigher]=@Creditslower",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteDataset(CommandType.Text, commandText, parms);
        }

        public DataTable GetUserGroupCreditsLowerAndHigher(int groupiId)
        {
            string commandText = string.Format("SELECT TOP 1 [groupid],[creditshigher],[creditslower] FROM [{0}usergroups]  WHERE [groupid]= {1}",
                                                BaseConfigs.GetTablePrefix,
                                                groupiId);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public void UpdateUserGroup(UserGroupInfo userGroupInfo)
        {
            DbParameter[] parms = 
					{
						DbHelper.MakeInParam("@Radminid",(DbType)SqlDbType.Int,4,(userGroupInfo.Groupid == 1) ? 1 : userGroupInfo.Radminid),
						DbHelper.MakeInParam("@Grouptitle",(DbType)SqlDbType.NVarChar,50, Utils.RemoveFontTag(userGroupInfo.Grouptitle)),
						DbHelper.MakeInParam("@Creditshigher",(DbType)SqlDbType.Int,4,userGroupInfo.Creditshigher),
						DbHelper.MakeInParam("@Creditslower",(DbType)SqlDbType.Int,4, userGroupInfo.Creditslower),
						DbHelper.MakeInParam("@Stars",(DbType)SqlDbType.Int,4,userGroupInfo.Stars),
						DbHelper.MakeInParam("@Color",(DbType)SqlDbType.Char,7,userGroupInfo.Color),
						DbHelper.MakeInParam("@Groupavatar",(DbType)SqlDbType.NVarChar,60,userGroupInfo.Groupavatar),
						DbHelper.MakeInParam("@Readaccess",(DbType)SqlDbType.Int,4,userGroupInfo.Readaccess),
						DbHelper.MakeInParam("@Allowvisit",(DbType)SqlDbType.Int,4,userGroupInfo.Allowvisit),
						DbHelper.MakeInParam("@Allowpost",(DbType)SqlDbType.Int,4,userGroupInfo.Allowpost),
						DbHelper.MakeInParam("@Allowreply",(DbType)SqlDbType.Int,4,userGroupInfo.Allowreply),
						DbHelper.MakeInParam("@Allowpostpoll",(DbType)SqlDbType.Int,4,userGroupInfo.Allowpostpoll),
						DbHelper.MakeInParam("@Allowdirectpost",(DbType)SqlDbType.Int,4,userGroupInfo.Allowdirectpost),
						DbHelper.MakeInParam("@Allowgetattach",(DbType)SqlDbType.Int,4,userGroupInfo.Allowgetattach),
						DbHelper.MakeInParam("@Allowpostattach",(DbType)SqlDbType.Int,4,userGroupInfo.Allowpostattach),
						DbHelper.MakeInParam("@Allowvote",(DbType)SqlDbType.Int,4,userGroupInfo.Allowvote),
						DbHelper.MakeInParam("@Allowmultigroups",(DbType)SqlDbType.Int,4,userGroupInfo.Allowmultigroups),
						DbHelper.MakeInParam("@Allowsearch",(DbType)SqlDbType.Int,4,userGroupInfo.Allowsearch),
						DbHelper.MakeInParam("@Allowavatar",(DbType)SqlDbType.Int,4,userGroupInfo.Allowavatar),
						DbHelper.MakeInParam("@Allowcstatus",(DbType)SqlDbType.Int,4,userGroupInfo.Allowcstatus),
						DbHelper.MakeInParam("@Allowuseblog",(DbType)SqlDbType.Int,4,userGroupInfo.Allowuseblog),
						DbHelper.MakeInParam("@Allowinvisible",(DbType)SqlDbType.Int,4,userGroupInfo.Allowinvisible),
						DbHelper.MakeInParam("@Allowtransfer",(DbType)SqlDbType.Int,4,userGroupInfo.Allowtransfer),
						DbHelper.MakeInParam("@Allowsetreadperm",(DbType)SqlDbType.Int,4,userGroupInfo.Allowsetreadperm),
						DbHelper.MakeInParam("@Allowsetattachperm",(DbType)SqlDbType.Int,4,userGroupInfo.Allowsetattachperm),
						DbHelper.MakeInParam("@Allowhidecode",(DbType)SqlDbType.Int,4,userGroupInfo.Allowhidecode),
						DbHelper.MakeInParam("@Allowhtml",(DbType)SqlDbType.Int,4,userGroupInfo.Allowhtml),
                        DbHelper.MakeInParam("@Allowhtmltitle",(DbType)SqlDbType.Int,4,userGroupInfo.Allowhtmltitle),
						DbHelper.MakeInParam("@Allowcusbbcode",(DbType)SqlDbType.Int,4,userGroupInfo.Allowcusbbcode),
						DbHelper.MakeInParam("@Allownickname",(DbType)SqlDbType.Int,4,userGroupInfo.Allownickname),
						DbHelper.MakeInParam("@Allowsigbbcode",(DbType)SqlDbType.Int,4,userGroupInfo.Allowsigbbcode),
						DbHelper.MakeInParam("@Allowsigimgcode",(DbType)SqlDbType.Int,4,userGroupInfo.Allowsigimgcode),
						DbHelper.MakeInParam("@Allowviewpro",(DbType)SqlDbType.Int,4,userGroupInfo.Allowviewpro),
						DbHelper.MakeInParam("@Allowviewstats",(DbType)SqlDbType.Int,4,userGroupInfo.Allowviewstats),
                        DbHelper.MakeInParam("@Allowtrade",(DbType)SqlDbType.Int,4,userGroupInfo.Allowtrade),
                        DbHelper.MakeInParam("@Allowdiggs",(DbType)SqlDbType.Int,4,userGroupInfo.Allowdiggs),
						DbHelper.MakeInParam("@Disableperiodctrl",(DbType)SqlDbType.Int,4,userGroupInfo.Disableperiodctrl),
                        DbHelper.MakeInParam("@Allowdebate",(DbType)SqlDbType.Int,4,userGroupInfo.Allowdebate),
                        DbHelper.MakeInParam("@Allowbonus",(DbType)SqlDbType.Int,4,userGroupInfo.Allowbonus),
                        DbHelper.MakeInParam("@Minbonusprice",(DbType)SqlDbType.Int,4,userGroupInfo.Minbonusprice),
                        DbHelper.MakeInParam("@Maxbonusprice",(DbType)SqlDbType.Int,4,userGroupInfo.Maxbonusprice),
						DbHelper.MakeInParam("@Reasonpm",(DbType)SqlDbType.Int,4,userGroupInfo.Reasonpm),
						DbHelper.MakeInParam("@Maxprice",(DbType)SqlDbType.SmallInt,2,userGroupInfo.Maxprice),
						DbHelper.MakeInParam("@Maxpmnum",(DbType)SqlDbType.SmallInt,2,userGroupInfo.Maxpmnum),
						DbHelper.MakeInParam("@Maxsigsize",(DbType)SqlDbType.SmallInt,2,userGroupInfo.Maxsigsize),
						DbHelper.MakeInParam("@Maxattachsize",(DbType)SqlDbType.Int,4,userGroupInfo.Maxattachsize),
						DbHelper.MakeInParam("@Maxsizeperday",(DbType)SqlDbType.Int,4,userGroupInfo.Maxsizeperday),
						DbHelper.MakeInParam("@Attachextensions",(DbType)SqlDbType.Char,100,userGroupInfo.Attachextensions),
                        DbHelper.MakeInParam("@Maxspaceattachsize",(DbType)SqlDbType.Int,4,userGroupInfo.Maxspaceattachsize),
                        DbHelper.MakeInParam("@Maxspacephotosize",(DbType)SqlDbType.Int,4,userGroupInfo.Maxspacephotosize),
						DbHelper.MakeInParam("@Groupid",(DbType)SqlDbType.Int,4,userGroupInfo.Groupid),
                        //DbHelper.MakeInParam("@Maxfriendscount",(DbType)SqlDbType.Int,4,userGroupInfo.MaxFriendsCount),
                        DbHelper.MakeInParam("@ModNewTopics",(DbType)SqlDbType.SmallInt,2,userGroupInfo.ModNewTopics),
                        DbHelper.MakeInParam("@ModNewPosts",(DbType)SqlDbType.SmallInt,2,userGroupInfo.ModNewPosts),
                        DbHelper.MakeInParam("@Ignoreseccode",(DbType)SqlDbType.Int,4,userGroupInfo.Ignoreseccode)
			};
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateusergroup", BaseConfigs.GetTablePrefix), parms);
        }


        public void UpdateOnlineList(UserGroupInfo userGroupInfo)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, userGroupInfo.Groupid),
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 50, Utils.RemoveFontTag(userGroupInfo.Grouptitle))
                                    };
            DbHelper.ExecuteNonQueryInMasterDB(CommandType.StoredProcedure, string.Format("{0}updateonlinelist", BaseConfigs.GetTablePrefix), parms);
        }

        public bool IsSystemOrTemplateUserGroup(int groupId)
        {
            string commandText = string.Format("SELECT TOP 1 {0}  FROM [{1}usergroups] WHERE ([system]=1 OR [type]=1) AND [groupid]={2}",
                                                DbFields.USER_GROUPS,
                                                BaseConfigs.GetTablePrefix,
                                                groupId);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows.Count > 0;
        }

        public string GetUserGroupRAdminId(int groupId)
        {
            string commandText = string.Format("SELECT TOP 1 [radminid] FROM [{0}usergroups] WHERE  [groupid]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                groupId);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows[0][0].ToString();
        }

        public void UpdateUserGroupLowerAndHigherToLimit(int groupId)
        {
            string commandText = string.Format("UPDATE [{0}usergroups] SET [creditshigher]=-9999999 ,creditslower=9999999  WHERE [groupid]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                groupId);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }


        public void DeleteOnlineList(int groupId)
        {
            string commandText = string.Format("DELETE FROM [{0}onlinelist] WHERE [groupid]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                groupId);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public int GetMaxUserGroupId()
        {
            string commandText = string.Format("SELECT ISNULL(MAX(groupid), 0) FROM [{0}usergroups]", BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText));
        }


        public bool DeletePaymentLog()
        {
            try
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}paymentlog]", BaseConfigs.GetTablePrefix));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 按指定条件删除日志
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public bool DeletePaymentLog(string condition)
        {
            try
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}paymentlog] WHERE {1}", BaseConfigs.GetTablePrefix, condition));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public DataTable GetPaymentLogList(int pageSize, int currentPage)
        {
            int pagetop = (currentPage - 1) * pageSize;
            string commandText;
            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} {1}, {2}topics.fid AS fid ,{2}topics.postdatetime AS postdatetime ,{2}topics.poster AS authorname, {2}topics.title AS title,{2}users.username As UserName  FROM [{2}paymentlog] AS [pl] LEFT OUTER JOIN {2}topics ON pl.tid = {2}topics.tid LEFT OUTER JOIN {2}users ON {2}users.uid = pl.uid ORDER BY [pl].[id] DESC",
                                             pageSize,
                                             DbFields.PAYMENT_LOG_JOIN,
                                             BaseConfigs.GetTablePrefix);
            else
                commandText = string.Format("SELECT TOP {0} {1}, {2}topics.fid AS fid ,{2}topics.postdatetime AS postdatetime ,{2}topics.poster AS authorname, {2}topics.title AS title,{2}users.username As UserName  FROM [{2}paymentlog] AS [pl] LEFT OUTER JOIN {2}topics ON pl.tid = {2}topics.tid LEFT OUTER JOIN {2}users ON {2}users.uid = pl.uid WHERE [id] < (SELECT min([id])  FROM (SELECT TOP {3} [id] FROM [{2}paymentlog] ORDER BY [id] DESC) AS tblTmp )  ORDER BY {2}[pl].[id] DESC",
                                             pageSize,
                                             DbFields.PAYMENT_LOG_JOIN,
                                             BaseConfigs.GetTablePrefix,
                                             pagetop);

            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public DataTable GetPaymentLogList(int pageSize, int currentPage, string condition)
        {
            int pagetop = (currentPage - 1) * pageSize;
            string commandText;
            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} {1}, {2}topics.fid AS fid ,{2}topics.postdatetime AS postdatetime ,{2}topics.poster AS authorname, {2}topics.title AS title,{2}users.username As UserName  FROM [{2}paymentlog] AS [pl] LEFT OUTER JOIN {2}topics ON pl.tid = {2}topics.tid LEFT OUTER JOIN {2}users ON {2}users.uid = pl.uid WHERE {3}  Order by [pl].[id] DESC",
                                             pageSize,
                                             DbFields.PAYMENT_LOG_JOIN,
                                             BaseConfigs.GetTablePrefix,
                                             condition);
            else
                commandText = string.Format("SELECT TOP {0} {1}, {2}topics.fid AS fid ,{2}topics.postdatetime AS postdatetime ,{2}topics.poster AS authorname, {2}topics.title AS title,{2}users.username As UserName  FROM [{2}paymentlog] AS [pl] LEFT OUTER JOIN {2}topics ON pl.tid = {2}topics.tid LEFT OUTER JOIN {2}users ON {2}users.uid = pl.uid  WHERE [id] < (SELECT min([id])  FROM (SELECT TOP {3} [id] FROM [{2}paymentlog] WHERE {4} ORDER BY [id] DESC) AS tblTmp ) AND {4} ORDER BY [pl].[id] DESC",
                                             pageSize,
                                             DbFields.PAYMENT_LOG_JOIN,
                                             BaseConfigs.GetTablePrefix,
                                             pagetop,
                                             condition);

            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 得到积分交易日志记录数
        /// </summary>
        /// <returns></returns>
        public int GetPaymentLogListCount()
        {
            string commandText = string.Format("SELECT count(id) FROM [{0}paymentlog]", BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 得到指定查询条件下的积分交易日志数
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public int GetPaymentLogListCount(string condition)
        {
            string commandText = string.Format("SELECT count(pl.id) FROM [{0}paymentlog] AS [pl] WHERE {1}",
                                                BaseConfigs.GetTablePrefix,
                                                condition);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows[0][0]);
        }

        public void DeleteModeratorByFid(int fid)
        {
            string commandText = string.Format("DELETE FROM [{0}moderators] WHERE [fid]={1}", BaseConfigs.GetTablePrefix, fid);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public DataTable GetUidModeratorByFid(string fidList)
        {
            string commandText = string.Format("SELECT distinct [uid] FROM [{0}moderators] WHERE [fid] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                fidList);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public void AddModerator(int uid, int fid, int displayOrder, int inherited)
        {
            DbParameter[] parms = 
            {
                DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
                DbHelper.MakeInParam("@fid", (DbType)SqlDbType.SmallInt, 2, fid),
                DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.SmallInt, 2, displayOrder),
                DbHelper.MakeInParam("@inherited", (DbType)SqlDbType.SmallInt, 2, inherited)
		    };
            string commandText = string.Format("INSERT INTO [{0}moderators] (uid,fid,displayorder,inherited) VALUES(@uid,@fid,@displayorder,@inherited)",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public DataTable GetModeratorInfo(string moderator)
        {
            DbParameter[] parms = {
				                      DbHelper.MakeInParam("@username", (DbType)SqlDbType.VarChar, 20, moderator.Trim())
			                      };
            string commandText = string.Format("SELECT TOP 1 [uid],[groupid]  FROM [{0}users] Where [groupid]<>7 AND [groupid]<>8 AND [username]=@username",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        public void SetModerator(string moderator)
        {
            DbParameter[] parms = 
            {
				DbHelper.MakeInParam("@username", (DbType)SqlDbType.VarChar, 20, moderator.Trim())
			};
            string commandText = string.Format("UPDATE [{0}users] SET [adminid]=3,[groupid]=3 WHERE [username]=@username",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            commandText = string.Format("UPDATE [{0}online] SET [adminid]=3,[groupid]=3 WHERE [username]=@username",
                                         BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }


        public DataTable GetUidAdminIdByUsername(string userName)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@username", (DbType)SqlDbType.VarChar, 20, userName)
			};
            string commandText = string.Format("SELECT TOP 1 [uid],[adminid] FROM [{0}users] WHERE [username] = @username",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        public DataTable GetUidInModeratorsByUid(int currentFid, int uid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@currentfid", (DbType)SqlDbType.Int, 4, currentFid),
                DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
			};
            string commandText = string.Format("SELECT TOP 1 [uid]  FROM [{0}moderators] WHERE [fid]<>@currentfid AND [uid]=@uid",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        public void UpdateUserOnlineInfo(int groupId, int userId)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupId),
                DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userId)
			};
            string commandText = string.Format("UPDATE [{0}online] SET [groupid]=@groupid WHERE [userid]=@userid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void UpdateUserOtherInfo(int groupId, int userId)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupId),
                DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userId)
			};
            string commandText = string.Format("UPDATE [{0}users] SET [groupid]=@groupid ,[adminid]=0 WHERE [uid]=@userid",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 得到论坛中最后注册的用户ID和用户名
        /// </summary>
        /// <param name="lastuserid">输出参数：最后注册的用户ID</param>
        /// <param name="lastusername">输出参数：最后注册的用户名</param>
        /// <returns>存在返回true,不存在返回false</returns>
        public bool GetLastUserInfo(out string lastUserId, out string lastUserName)
        {
            lastUserId = "";
            lastUserName = "";
            string commandText = string.Format("SELECT TOP 1 [uid],[username] FROM [{0}users] ORDER BY [uid] DESC", BaseConfigs.GetTablePrefix);
            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, commandText);
            if (reader.Read())
            {
                lastUserId = reader["uid"].ToString();
                lastUserName = reader["username"].ToString().Trim();
                reader.Close();
                return true;
            }
            reader.Close();
            return false;
        }

        public IDataReader GetTopUsers(int statCount, int lastUid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@statcont", (DbType)SqlDbType.Int, 4, statCount),
                DbHelper.MakeInParam("@lastuid", (DbType)SqlDbType.Int, 4, lastUid)
			};
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}gettopusers", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 重建所有用户的精华帖数
        /// </summary>
        public void ResetUserDigestPosts()
        {
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}resetuserdigestposts", BaseConfigs.GetTablePrefix));
        }

        public IDataReader GetUsers(int startUid, int endUid)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@start_uid", (DbType)SqlDbType.Int, 4, startUid),
				DbHelper.MakeInParam("@end_uid", (DbType)SqlDbType.Int, 4, endUid)
			};
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getusers", BaseConfigs.GetTablePrefix), parms);
        }

        public void UpdateUserPostCount(int postCount, int userId)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@postcount", (DbType)SqlDbType.Int, 4, postCount),
                                        DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userId)
                                    };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateuserpostcount", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 更新所有用户的帖子数
        /// </summary>
        /// <param name="postTableID"></param>
        public void UpdateAllUserPostCount(int postTableID)
        {
            if (postTableID == 1)
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET posts = 0 WHERE posts > 0", BaseConfigs.GetTablePrefix));
            }
            DbParameter parm = DbHelper.MakeInParam("@posttablename", (DbType)SqlDbType.NVarChar, 50, string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, postTableID));
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}resetuserspostcount", BaseConfigs.GetTablePrefix), parm);
        }

        /// <summary>
        /// 更新我的帖子
        /// </summary>
        /// <param name="postTableID"></param>
        public void UpdateMyPost(int postTableID)
        {
            DbParameter parm = DbHelper.MakeInParam("@tablename", (DbType)SqlDbType.NVarChar, 50, string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, postTableID));
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updatemypost", BaseConfigs.GetTablePrefix), parm);
        }
        /// <summary>
        /// 获得所有版主列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetModeratorList()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}moderators]", DbFields.MODERATORS, BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDatasetInMasterDB(CommandType.Text, commandText).Tables[0];
        }


        /// <summary>
        /// 获得全部在线用户数
        /// </summary>
        /// <returns></returns>
        public int GetOnlineAllUserCount()
        {
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalarInMasterDB(CommandType.StoredProcedure,
                                                                    string.Format("{0}getonlineuercount", BaseConfigs.GetTablePrefix)), 1);
        }

        /// <summary>
        /// 创建在线表
        /// </summary>
        /// <returns></returns>
        public int CreateOnlineTable()
        {
            try
            {
                //当使用读写分离方案时，无法使用TRUNCATE TABLE (数据表分割),要用下面sql语句来重新初始化在线表
                if (DbSnapConfigs.GetConfig() != null && DbSnapConfigs.GetConfig().AppDbSnap)
                    return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}online];DBCC CHECKIDENT([{0}online],RESEED,0)", BaseConfigs.GetTablePrefix));
                else
                    return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("TRUNCATE TABLE [{0}online]", BaseConfigs.GetTablePrefix));
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获得在线注册用户总数量
        /// </summary>
        /// <returns>用户数量</returns>
        public int GetOnlineUserCount()
        {
            return TypeConverter.ObjectToInt(
                                 DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getonlineregistercount", BaseConfigs.GetTablePrefix)).Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 获得全部在线用户列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetOnlineUserListTable()
        {
            return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getonlineuserlist", BaseConfigs.GetTablePrefix)).Tables[0];
        }

        /// <summary>
        /// 获得版块在线用户列表
        /// </summary>
        /// <param name="forumid">版块Id</param>
        /// <returns></returns>
        public IDataReader GetForumOnlineUserList(int forumId)
        {
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getonlineuserlistbyfid", BaseConfigs.GetTablePrefix),
                                          DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, forumId));
        }

        /// <summary>
        /// 获得全部在线用户列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetOnlineUserList()
        {
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getonlineuserlist", BaseConfigs.GetTablePrefix));
        }

        /// <summary>
        /// 返回在线用户图例
        /// </summary>
        /// <returns></returns>
        public DataTable GetOnlineGroupIconTable()
        {
            return DbHelper.ExecuteDatasetInMasterDB(CommandType.StoredProcedure, string.Format("{0}getonlinegroupicontable", BaseConfigs.GetTablePrefix)).Tables[0];
        }

        /// <summary>
        /// 根据uid获得olid
        /// </summary>
        /// <param name="uid">uid</param>
        /// <returns>olid</returns>
        public int GetOlidByUid(int uid)
        {
            string commandText = string.Format("SELECT [olid] FROM [{0}online] WHERE [userid]={1}", BaseConfigs.GetTablePrefix, uid);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalarToStr(CommandType.Text, commandText), -1);
        }

        /// <summary>
        /// 获得指定在线用户
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <returns>在线用户的详细信息</returns>
        public IDataReader GetOnlineUser(int olId)
        {
            string commandText = string.Format("SELECT {0} FROM [{1}online] WHERE [olid]={2}",
                                                DbFields.ONLINE,
                                                BaseConfigs.GetTablePrefix,
                                                olId);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获得指定用户的详细信息
        /// </summary>
        /// <param name="userId">在线用户ID</param>
        /// <param name="password">用户密码</param>
        /// <returns>用户的详细信息</returns>
        public DataTable GetOnlineUser(int userId, string passWord)
        {
            DbParameter[] parms =  { 
                                        DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userId),
                                        DbHelper.MakeInParam("@password", (DbType)SqlDbType.Char, 32, passWord)
                                    };
            return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getonlineuser", BaseConfigs.GetTablePrefix), parms).Tables[0];
        }

        /// <summary>
        /// 获得指定用户的详细信息
        /// </summary>
        /// <param name="userId">在线用户ID</param>
        /// <param name="ip">IP</param>
        /// <returns></returns>
        public DataTable GetOnlineUserByIP(int userId, string ip)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userId),
                                        DbHelper.MakeInParam("@ip", (DbType)SqlDbType.VarChar, 15, ip)
                                    };
            return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getonlineuserbyip", BaseConfigs.GetTablePrefix), parms).Tables[0];
        }

        /// <summary>
        /// 检查在线用户验证码是否有效
        /// </summary>
        /// <param name="olid">在组用户ID</param>
        /// <param name="verifycode">验证码</param>
        /// <returns>在组用户ID</returns>
        public bool CheckUserVerifyCode(int olId, string verifyCode, string newVerifyCode)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@olid", (DbType)SqlDbType.Int, 4, olId),
                                        DbHelper.MakeInParam("@verifycode", (DbType)SqlDbType.VarChar, 10, verifyCode)
                                    };
            string commandText = string.Format("SELECT TOP 1 [olid] FROM [{0}online] WHERE [olid]=@olid and [verifycode]=@verifycode",
                                                BaseConfigs.GetTablePrefix);
            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
            parms[1].Value = newVerifyCode;

            commandText = string.Format("UPDATE [{0}online] SET [verifycode]=@verifycode WHERE [olid]=@olid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            return dt.Rows.Count > 0;
        }

        /// <summary>
        /// 设置用户在线状态
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="onlinestate">在线状态，1在线</param>
        /// <returns></returns>
        public int SetUserOnlineState(int uid, int onlineState)
        {
            string commandText = string.Format("UPDATE [{0}users] SET [onlinestate]={1},[lastactivity]=GETDATE(),[lastvisit]=GETDATE() WHERE [uid]={2}",
                                                BaseConfigs.GetTablePrefix,
                                                onlineState,
                                                uid);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 删除符合条件的一个或多个用户信息
        /// </summary>
        /// <returns>删除行数</returns>
        public int DeleteRowsByIP(string ip)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@ip",(DbType)SqlDbType.VarChar,15,ip)
								   };

            //当在线表被布署到别的数据库时
            if (EntLibConfigs.GetConfig() != null)
            {
                OnlineTableConnect onlineTableConnect = EntLibConfigs.GetConfig().Onlinetableconnect;
                if (onlineTableConnect.Enable && !string.IsNullOrEmpty(onlineTableConnect.SqlServerConn))
                {   //分解上面的SQL语句，避免当online表被布署到别的机器上时出现错误情况。
                    DbDataReader dataReader = DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT [userid] FROM [{0}online] WHERE [userid]>0 AND [ip]=@ip", BaseConfigs.GetTablePrefix), parms);
                    string uidlist = "";
                    while (dataReader.Read())
                        uidlist += TypeConverter.ObjectToInt(dataReader["userid"], 0) + ",";
                    dataReader.Close();

                    if (!string.IsNullOrEmpty(uidlist.TrimEnd(',')))
                        DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [onlinestate]=0,[lastactivity]=GETDATE() WHERE [uid] IN ({1})", BaseConfigs.GetTablePrefix, uidlist.TrimEnd(',')));
                }
            }
            else
                DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [onlinestate]=0,[lastactivity]=GETDATE() WHERE [uid] IN (SELECT [userid] FROM [{0}online] WHERE [userid]>0 AND [ip]=@ip)",
                                                BaseConfigs.GetTablePrefix), parms);

            if (ip != "0.0.0.0")
                return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}online] WHERE [userid]=-1 AND [ip]=@ip", BaseConfigs.GetTablePrefix), parms);

            return 0;
        }

        /// <summary>
        /// 删除在线表中指定在线id的行
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <returns></returns>
        public int DeleteRows(int olId)
        {
            string commandText = string.Format("DELETE FROM [{0}online] WHERE [olid]={1}", BaseConfigs.GetTablePrefix, olId);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 更新用户的当前动作及相关信息
        /// </summary>
        /// <param name="olid">在线列表id</param>
        /// <param name="action">动作</param>
        /// <param name="inid">所在位置代码</param>
        public void UpdateAction(int olId, int action, int inid)
        {
            DbParameter[] parms = {
										   DbHelper.MakeInParam("@action",(DbType)SqlDbType.SmallInt,2,action),
                                           DbHelper.MakeInParam("@lastupdatetime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))),
										   DbHelper.MakeInParam("@forumid",(DbType)SqlDbType.Int,4,inid),
										   DbHelper.MakeInParam("@forumname",(DbType)SqlDbType.NVarChar,100,""),
										   DbHelper.MakeInParam("@titleid",(DbType)SqlDbType.Int,4,inid),
										   DbHelper.MakeInParam("@title",(DbType)SqlDbType.NVarChar,160,""),
										   DbHelper.MakeInParam("@olid",(DbType)SqlDbType.Int,4,olId)
								  };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateonlineaction", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 更新用户的当前动作及相关信息
        /// </summary>
        /// <param name="olid">在线列表id</param>
        /// <param name="action">动作id</param>
        /// <param name="fid">版块id</param>
        /// <param name="forumname">版块名</param>
        /// <param name="tid">主题id</param>
        /// <param name="topictitle">主题名</param>
        public void UpdateAction(int olId, int action, int fid, string forumName, int tid, string topicTitle)
        {
            DbParameter[] parms = {
										   DbHelper.MakeInParam("@action",(DbType)SqlDbType.SmallInt,2,action),
                                           DbHelper.MakeInParam("@lastupdatetime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))),
										   DbHelper.MakeInParam("@forumid",(DbType)SqlDbType.Int,4,fid),
										   DbHelper.MakeInParam("@forumname",(DbType)SqlDbType.NVarChar,100,forumName),
										   DbHelper.MakeInParam("@titleid",(DbType)SqlDbType.Int,4,tid),
										   DbHelper.MakeInParam("@title",(DbType)SqlDbType.NVarChar,160,topicTitle),
										   DbHelper.MakeInParam("@olid",(DbType)SqlDbType.Int,4,olId)
								  };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateonlineaction", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 更新用户最后活动时间
        /// </summary>
        /// <param name="olid">在线id</param>
        public void UpdateLastTime(int olId)
        {
            DbParameter[] parms = {
                                           DbHelper.MakeInParam("@lastupdatetime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))),
										   DbHelper.MakeInParam("@olid",(DbType)SqlDbType.Int,4,olId)
									   };
            string commandText = string.Format("UPDATE [{0}online] SET [lastupdatetime]=@lastupdatetime WHERE [olid]=@olid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新用户最后发帖时间
        /// </summary>
        /// <param name="olid">在线id</param>
        public void UpdatePostTime(int olId)
        {
            string commandText = string.Format("UPDATE [{0}online] SET [lastposttime]='{1}' WHERE [olid]={2}",
                                                BaseConfigs.GetTablePrefix,
                                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), olId);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 更新用户最后发短消息时间
        /// </summary>
        /// <param name="olid">在线id</param>
        public void UpdatePostPMTime(int olId)
        {
            string commandText = string.Format("UPDATE [{0}online] SET [lastpostpmtime]='{1}' WHERE [olid]={2}",
                                                BaseConfigs.GetTablePrefix,
                                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), olId);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 更新在线表中指定用户是否隐身
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="invisible">是否隐身</param>
        public void UpdateInvisible(int olId, int invisible)
        {
            string commandText = string.Format("UPDATE [{0}online] SET [invisible]={1} WHERE [olid]={2}",
                                                BaseConfigs.GetTablePrefix,
                                                invisible,
                                                olId);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 更新在线表中指定用户的用户密码
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="password">用户密码</param>
        public void UpdatePassword(int olId, string passWord)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@password",(DbType)SqlDbType.Char,32,passWord),
									   DbHelper.MakeInParam("@olid",(DbType)SqlDbType.Int,4,olId)
								   };
            string commandText = string.Format("UPDATE [{0}online] SET [password]=@password WHERE [olid]=@olid",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新用户IP地址
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="ip">ip地址</param>
        public void UpdateIP(int olId, string ip)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@ip",(DbType)SqlDbType.VarChar,15,ip),
									   DbHelper.MakeInParam("@olid",(DbType)SqlDbType.Int,4,olId)
								   };
            string commandText = string.Format("UPDATE [{0}online] SET [ip]=@ip WHERE [olid]=@olid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新用户最后搜索时间
        /// </summary>
        /// <param name="olid">在线id</param>
        public void UpdateSearchTime(int olId)
        {
            string commandText = string.Format("UPDATE [{0}online] SET [lastsearchtime]={1} WHERE [olid]={2}",
                                                BaseConfigs.GetTablePrefix,
                                                olId);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 更新用户的用户组
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="groupid">组名</param>
        public void UpdateGroupid(int userId, int groupId)
        {
            string commandText = string.Format("UPDATE [{0}online] SET [groupid]={1} WHERE [userid]={2}",
                                                BaseConfigs.GetTablePrefix,
                                                groupId,
                                                userId);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }


        /// <summary>
        /// 获得指定ID的短消息的内容
        /// </summary>
        /// <param name="pmid">短消息pmid</param>
        /// <returns>短消息内容</returns>
        public IDataReader GetPrivateMessageInfo(int pmId)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@pmid", (DbType)SqlDbType.Int,4, pmId),
			                        };
            string commandText = string.Format("SELECT TOP 1 {0} FROM [{1}pms] WHERE [pmid]=@pmid",
                                                DbFields.PMS,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得指定用户的短信息列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="folder">短信息类型(0:收件箱,1:发件箱,2:草稿箱)</param>
        /// <param name="pagesize">每页显示短信息数</param>
        /// <param name="pageindex">当前要显示的页数</param>
        /// <param name="inttype">筛选条件1为未读</param>
        /// <returns>短信息列表</returns>
        public IDataReader GetPrivateMessageList(int userId, int folder, int pageSize, int pageIndex, int intType)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@userid",(DbType)SqlDbType.Int,4,userId),
									   DbHelper.MakeInParam("@folder",(DbType)SqlDbType.Int,4,folder),
									   DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pageSize),
									   DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,pageIndex),
								       DbHelper.MakeInParam("@inttype",(DbType)SqlDbType.VarChar,500,intType)
								   };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getpmlist", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 得到当用户的短消息数量
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="folder">所属文件夹(0:收件箱,1:发件箱,2:草稿箱)</param>
        /// <param name="state">短消息状态(0:已读短消息、1:未读短消息、-1:全部短消息)</param>
        /// <returns>短消息数量</returns>
        public int GetPrivateMessageCount(int userId, int folder, int state)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@userid",(DbType)SqlDbType.Int,4,userId),
									   DbHelper.MakeInParam("@folder",(DbType)SqlDbType.Int,4,folder),								   
									   DbHelper.MakeInParam("@state",(DbType)SqlDbType.Int,4,state)
								   };
            return TypeConverter.ObjectToInt(
                                 DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                        string.Format("{0}getpmcount", BaseConfigs.GetTablePrefix),
                                                        parms));
        }

        /// <summary>
        /// 得到公共消息数量
        /// </summary>
        /// <returns>公共消息数量</returns>
        public int GetAnnouncePrivateMessageCount()
        {
            return TypeConverter.ObjectToInt(
                         DbHelper.ExecuteScalarInMasterDB(CommandType.Text,
                                                             string.Format("SELECT COUNT(pmid) FROM [{0}pms] WHERE [msgtoid] = 0", BaseConfigs.GetTablePrefix)));
        }


        /// <summary>
        /// 获得指定用户的短信息列表
        /// </summary>
        /// <param name="pagesize">每页显示短信息数,为-1时返回全部</param>
        /// <param name="pageindex">当前要显示的页数</param>
        /// <returns>短信息列表</returns>
        public IDataReader GetAnnouncePrivateMessageList(int pageSize, int pageIndex)
        {
            string commandText = "";
            if (pageSize == -1)
                commandText = string.Format("SELECT {0} FROM [{1}pms] WHERE [msgtoid] = 0 ORDER BY [pmid] DESC",
                                             DbFields.PMS, BaseConfigs.GetTablePrefix);
            else if (pageIndex <= 1)
                commandText = string.Format("SELECT TOP {0} {1} FROM [{2}pms] WHERE [msgtoid] = 0  ORDER BY [pmid] DESC",
                                             pageSize, DbFields.PMS, BaseConfigs.GetTablePrefix);
            else
                commandText = string.Format("SELECT TOP {0} {1} FROM [{2}pms] WHERE [msgtoid] = 0 AND [pmid] < (SELECT MIN([pmid]) FROM (SELECT TOP {3} [pmid] FROM [{2}pms] WHERE [msgtoid] = 0  ORDER BY [pmid] DESC) AS tblTmp)  ORDER BY [pmid] DESC",
                                             pageSize, DbFields.PMS, BaseConfigs.GetTablePrefix, (pageIndex - 1) * pageSize);

            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 创建短消息
        /// </summary>
        /// <param name="__privatemessageinfo">短消息内容</param>
        /// <param name="savetosentbox">设置短消息是否在发件箱保留(0为不保留, 1为保留)</param>
        /// <returns>短消息在数据库中的pmid</returns>
        public int CreatePrivateMessage(PrivateMessageInfo privateMessageInfo, int saveToSentBox)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@pmid",(DbType)SqlDbType.Int,4,privateMessageInfo.Pmid),
									   DbHelper.MakeInParam("@msgfrom",(DbType)SqlDbType.NVarChar,20,privateMessageInfo.Msgfrom),
									   DbHelper.MakeInParam("@msgfromid",(DbType)SqlDbType.Int,4,privateMessageInfo.Msgfromid),
									   DbHelper.MakeInParam("@msgto",(DbType)SqlDbType.NVarChar,20,privateMessageInfo.Msgto),
									   DbHelper.MakeInParam("@msgtoid",(DbType)SqlDbType.Int,4,privateMessageInfo.Msgtoid),
									   DbHelper.MakeInParam("@folder",(DbType)SqlDbType.SmallInt,2,privateMessageInfo.Folder),
									   DbHelper.MakeInParam("@new",(DbType)SqlDbType.Int,4,privateMessageInfo.New),
									   DbHelper.MakeInParam("@subject",(DbType)SqlDbType.NVarChar,80,privateMessageInfo.Subject),
									   DbHelper.MakeInParam("@postdatetime",(DbType)SqlDbType.DateTime,8,DateTime.Parse(privateMessageInfo.Postdatetime)),
									   DbHelper.MakeInParam("@message",(DbType)SqlDbType.NText,0,privateMessageInfo.Message),
									   DbHelper.MakeInParam("@savetosentbox",(DbType)SqlDbType.Int,4,saveToSentBox)
								   };
            return TypeConverter.ObjectToInt(
                                 DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                        string.Format("{0}createpm", BaseConfigs.GetTablePrefix), parms), -1);
        }

        /// <summary>
        /// 删除指定用户的短信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pmitemid">要删除的短信息列表(数组)</param>
        /// <returns>删除记录数</returns>
        public int DeletePrivateMessages(int userId, string pmIdList)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int,4, userId)
			                      };
            string commandText = string.Format("DELETE FROM [{0}pms] WHERE [pmid] IN ({1}) AND ([msgtoid] = @userid OR [msgfromid] = @userid)",
                                                BaseConfigs.GetTablePrefix,
                                                pmIdList);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得新短消息数
        /// </summary>
        /// <returns></returns>
        public int GetNewPMCount(int userId)
        {
            string commandText = string.Format("SELECT COUNT([pmid]) AS [pmcount] FROM [{0}pms] WHERE [new] = 1 AND [folder] = 0 AND [msgtoid] = {1}",
                                                BaseConfigs.GetTablePrefix,
                                                userId);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        /// <summary>
        /// 删除指定用户的一条短消息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="pmid">pmid</param>
        /// <returns></returns>
        public int DeletePrivateMessage(int userId, int pmId)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int,4, userId),
									   DbHelper.MakeInParam("@pmid", (DbType)SqlDbType.Int,4, pmId)
			                      };
            string commandText = string.Format("DELETE FROM [{0}pms] WHERE [pmid]=@pmid AND ([msgtoid] = @userid OR [msgfromid] = @userid)",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 设置短信息状态
        /// </summary>
        /// <param name="pmid">短信息ID</param>
        /// <param name="state">状态值</param>
        /// <returns>更新记录数</returns>
        public int SetPrivateMessageState(int pmId, byte state)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@pmid", (DbType)SqlDbType.Int,1,pmId),
									   DbHelper.MakeInParam("@state",(DbType)SqlDbType.TinyInt,1,state)
								   };
            string commandText = string.Format("UPDATE [{0}pms] SET [new]=@state WHERE [pmid]=@pmid", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }


        public string GetUserGroupsStr()
        {
            return string.Format("SELECT [groupid], [grouptitle] FROM [{0}usergroups] ORDER BY [groupid]", BaseConfigs.GetTablePrefix);
        }


        public DataTable GetUserListByGroupid(string groupIdList)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupIdList", (DbType)SqlDbType.VarChar, 500, groupIdList);
            return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getuserlistbygroupid", BaseConfigs.GetTablePrefix), parm).Tables[0];
        }

        //仅限于后台发送短消息时使用
        public DataTable GetUserListByGroupid(string groupIdList, int topNumber, int start_uid)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP {0} [uid],[username],[email] FROM [{1}users] WHERE [groupid] IN ({2}) And [uid] > {3} ORDER BY [uid] ASC", topNumber, BaseConfigs.GetTablePrefix, groupIdList, start_uid)).Tables[0];
        }

        public DataTable GetUserNameByUid(int uid)
        {
            string commandText = string.Format("SELECT TOP 1 [username] FROM [{0}users] WHERE [uid]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                uid);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }


        public string GetSystemGroupInfoSql()
        {
            return string.Format("SELECT {0} FROM [{1}usergroups] WHERE [groupid]<=8 ORDER BY [groupid]",
                                  DbFields.USER_GROUPS,
                                  BaseConfigs.GetTablePrefix);
        }

        public void UpdateUserCredits(int uid)
        {
            //string commandText = string.Format("UPDATE [{0}users] SET [credits] = {1} WHERE [uid]={2}", 
            //                                    BaseConfigs.GetTablePrefix, 
            //                                    credits, 
            //                                    uid);
            //DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
            DbParameter parm = DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 1, uid);
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateusercredits", BaseConfigs.GetTablePrefix), parm);
        }

        public bool CheckUserCreditsIsEnough(int uid, float[] values)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
									   DbHelper.MakeInParam("@extcredits1", (DbType)SqlDbType.Float, 8, values[0]),
									   DbHelper.MakeInParam("@extcredits2", (DbType)SqlDbType.Float, 8, values[1]),
									   DbHelper.MakeInParam("@extcredits3", (DbType)SqlDbType.Float, 8, values[2]),
									   DbHelper.MakeInParam("@extcredits4", (DbType)SqlDbType.Float, 8, values[3]),
									   DbHelper.MakeInParam("@extcredits5", (DbType)SqlDbType.Float, 8, values[4]),
									   DbHelper.MakeInParam("@extcredits6", (DbType)SqlDbType.Float, 8, values[5]),
									   DbHelper.MakeInParam("@extcredits7", (DbType)SqlDbType.Float, 8, values[6]),
									   DbHelper.MakeInParam("@extcredits8", (DbType)SqlDbType.Float, 8, values[7])
								   };
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}users] WHERE [uid]=@uid AND"
                                                + "	[extcredits1]>= (case when @extcredits1<0 then abs(@extcredits1) else [extcredits1] end) AND "
                                                + "	[extcredits2]>= (case when @extcredits2<0 then abs(@extcredits2) else [extcredits2] end) AND "
                                                + "	[extcredits3]>= (case when @extcredits3<0 then abs(@extcredits3) else [extcredits3] end) AND "
                                                + "	[extcredits4]>= (case when @extcredits4<0 then abs(@extcredits4) else [extcredits4] end) AND "
                                                + "	[extcredits5]>= (case when @extcredits5<0 then abs(@extcredits5) else [extcredits5] end) AND "
                                                + "	[extcredits6]>= (case when @extcredits6<0 then abs(@extcredits6) else [extcredits6] end) AND "
                                                + "	[extcredits7]>= (case when @extcredits7<0 then abs(@extcredits7) else [extcredits7] end) AND "
                                                + "	[extcredits8]>= (case when @extcredits8<0 then abs(@extcredits8) else [extcredits8] end) ",
                                                BaseConfigs.GetTablePrefix);

            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms)) != 0;
        }

        public void UpdateUserCredits(int uid, float[] values)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
									   DbHelper.MakeInParam("@extcredits1", (DbType)SqlDbType.Float, 8, values[0]),
									   DbHelper.MakeInParam("@extcredits2", (DbType)SqlDbType.Float, 8, values[1]),
									   DbHelper.MakeInParam("@extcredits3", (DbType)SqlDbType.Float, 8, values[2]),
									   DbHelper.MakeInParam("@extcredits4", (DbType)SqlDbType.Float, 8, values[3]),
									   DbHelper.MakeInParam("@extcredits5", (DbType)SqlDbType.Float, 8, values[4]),
									   DbHelper.MakeInParam("@extcredits6", (DbType)SqlDbType.Float, 8, values[5]),
									   DbHelper.MakeInParam("@extcredits7", (DbType)SqlDbType.Float, 8, values[6]),
									   DbHelper.MakeInParam("@extcredits8", (DbType)SqlDbType.Float, 8, values[7])
								   };

            string commandText = string.Format("UPDATE [{0}users] SET "
                                                + "		[extcredits1]=[extcredits1] + @extcredits1, "
                                                + "		[extcredits2]=[extcredits2] + @extcredits2, "
                                                + "		[extcredits3]=[extcredits3] + @extcredits3, "
                                                + "		[extcredits4]=[extcredits4] + @extcredits4, "
                                                + "		[extcredits5]=[extcredits5] + @extcredits5, "
                                                + "		[extcredits6]=[extcredits6] + @extcredits6, "
                                                + "		[extcredits7]=[extcredits7] + @extcredits7, "
                                                + "		[extcredits8]=[extcredits8] + @extcredits8  WHERE [uid]=@uid",
                                                BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public bool CheckUserCreditsIsEnough(int uid, float[] values, int pos, int mount)
        {
            //出现减积分操作时
            if (pos < 0)
            {
                DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
									   DbHelper.MakeInParam("@extcredits1", (DbType)SqlDbType.Float, 8, values[0] * mount),
									   DbHelper.MakeInParam("@extcredits2", (DbType)SqlDbType.Float, 8, values[1] * mount),
									   DbHelper.MakeInParam("@extcredits3", (DbType)SqlDbType.Float, 8, values[2] * mount),
									   DbHelper.MakeInParam("@extcredits4", (DbType)SqlDbType.Float, 8, values[3] * mount),
									   DbHelper.MakeInParam("@extcredits5", (DbType)SqlDbType.Float, 8, values[4] * mount),
									   DbHelper.MakeInParam("@extcredits6", (DbType)SqlDbType.Float, 8, values[5] * mount),
									   DbHelper.MakeInParam("@extcredits7", (DbType)SqlDbType.Float, 8, values[6] * mount),
									   DbHelper.MakeInParam("@extcredits8", (DbType)SqlDbType.Float, 8, values[7] * mount)
								   };
                string commandText = string.Format("SELECT COUNT(1) FROM [{0}users] WHERE [uid]=@uid AND"
                                                    + "	[extcredits1]>= (case when @extcredits1 < 0 then abs(@extcredits1) else 0 end) AND "
                                                    + "	[extcredits2]>= (case when @extcredits2 < 0 then abs(@extcredits2) else 0 end) AND "
                                                    + "	[extcredits3]>= (case when @extcredits3 < 0 then abs(@extcredits3) else 0 end) AND "
                                                    + "	[extcredits4]>= (case when @extcredits4 < 0 then abs(@extcredits4) else 0 end) AND "
                                                    + "	[extcredits5]>= (case when @extcredits5 < 0 then abs(@extcredits5) else 0 end) AND "
                                                    + "	[extcredits6]>= (case when @extcredits6 < 0 then abs(@extcredits6) else 0 end) AND "
                                                    + "	[extcredits7]>= (case when @extcredits7 < 0 then abs(@extcredits7) else 0 end) AND "
                                                    + "	[extcredits8]>= (case when @extcredits8 < 0 then abs(@extcredits8) else 0 end) ",
                                                    BaseConfigs.GetTablePrefix);
                return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms)) != 0;
            }
            return true;
        }

        public void UpdateUserCredits(int uid, float[] values, int pos, int mount)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
									   DbHelper.MakeInParam("@extcredits1", (DbType)SqlDbType.Float, 8, values[0] * pos * mount),
									   DbHelper.MakeInParam("@extcredits2", (DbType)SqlDbType.Float, 8, values[1] * pos * mount),
									   DbHelper.MakeInParam("@extcredits3", (DbType)SqlDbType.Float, 8, values[2] * pos * mount),
									   DbHelper.MakeInParam("@extcredits4", (DbType)SqlDbType.Float, 8, values[3] * pos * mount),
									   DbHelper.MakeInParam("@extcredits5", (DbType)SqlDbType.Float, 8, values[4] * pos * mount),
									   DbHelper.MakeInParam("@extcredits6", (DbType)SqlDbType.Float, 8, values[5] * pos * mount),
									   DbHelper.MakeInParam("@extcredits7", (DbType)SqlDbType.Float, 8, values[6] * pos * mount),
									   DbHelper.MakeInParam("@extcredits8", (DbType)SqlDbType.Float, 8, values[7] * pos * mount)								   };
            if (pos < 0 && mount < 0)
            {
                for (int i = 1; i < parms.Length; i++)
                {
                    parms[i].Value = -Convert.ToInt32(parms[i].Value);
                }
            }

            string commandText = string.Format("UPDATE [{0}users] SET "
                                                + "	[extcredits1]=[extcredits1] + @extcredits1, "
                                                + "	[extcredits2]=[extcredits2] + @extcredits2, "
                                                + "	[extcredits3]=[extcredits3] + @extcredits3, "
                                                + "	[extcredits4]=[extcredits4] + @extcredits4, "
                                                + "	[extcredits5]=[extcredits5] + @extcredits5, "
                                                + "	[extcredits6]=[extcredits6] + @extcredits6, "
                                                + "	[extcredits7]=[extcredits7] + @extcredits7, "
                                                + "	[extcredits8]=[extcredits8] + @extcredits8 "
                                                + "WHERE [uid]=@uid",
                                                BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }


        public DataTable GetUserGroups()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}usergroups] ORDER BY [groupid]", DbFields.USER_GROUPS, BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDatasetInMasterDB(CommandType.Text, commandText).Tables[0];
        }

        public DataTable GetUserGroupRateRange(int groupId)
        {
            string commandText = string.Format("SELECT TOP 1 [raterange] FROM [{0}usergroups] WHERE [groupid]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                groupId);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public IDataReader GetUserTodayRate(int uid)
        {
            string commandText = string.Format("SELECT [extcredits], SUM(ABS([score])) AS [todayrate] FROM [{0}ratelog] WHERE DATEDIFF(d,[postdatetime],getdate()) = 0 AND [uid] = {1} GROUP BY [extcredits]",
                                                BaseConfigs.GetTablePrefix,
                                                uid);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public DataTable GetSpecialUserGroup()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}usergroups] WHERE [radminid]=-1 AND [groupid]>8 ORDER BY [groupid]",
                                                DbFields.USER_GROUPS,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(commandText).Tables[0];
        }



        /// <summary>
        /// 返回指定用户的信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>用户信息</returns>
        public IDataReader GetUserInfoToReader(int uid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int,4, uid)
			                      };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getuserinfo", BaseConfigs.GetTablePrefix), parms);
        }

        ///<summary>
        /// 根据IP查找用户
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>用户信息</returns>
        public IDataReader GetUserInfoByIP(string ip)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@regip", (DbType)SqlDbType.Char,15, ip),
			                      };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getuserinfobyip", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public IDataReader GetUserInfoToReader(string userName)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar,20, userName)
			                      };
            string commandText = string.Format("SELECT TOP 1 {1} FROM [{0}users] AS [u] LEFT JOIN [{0}userfields] AS [uf] ON [u].[uid]=[uf].[uid] WHERE [u].[username]=@username",
                                                BaseConfigs.GetTablePrefix,
                                                DbFields.USERS_JOIN_FIELDS);
            return DbHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public IDataReader GetUserInfoToReader(int uid, string userName)
        {
            DbParameter[] parms = {
                                       DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4, uid),
									   DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar,20, userName)
			                      };
            string commandText = string.Format("SELECT TOP 1 {1} FROM [{0}users] AS [u] LEFT JOIN [{0}userfields] AS [uf] ON [u].[uid]=[uf].[uid] WHERE [u].[uid]=@uid AND [u].[username]=@username",
                                                BaseConfigs.GetTablePrefix,
                                                DbFields.USERS_JOIN_FIELDS);
            return DbHelper.ExecuteReader(CommandType.Text, commandText, parms);

        }
        /// <summary>
        /// 获取简短用户信息
        /// </summary>
        /// <param name="uid">用id</param>
        /// <returns>用户简短信息</returns>
        public IDataReader GetShortUserInfoToReader(int uid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int,4, uid),
			                      };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getshortuserinfo", BaseConfigs.GetTablePrefix), parms);
        }


        public IDataReader GetUserName(int uid)
        {
            string commandText = string.Format("SELECT TOP 1 [username] FROM [{0}users] WHERE [{0}users].[uid]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                uid);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetUserJoinDate(int uid)
        {
            string commandText = string.Format("SELECT TOP 1 [joindate] FROM [{0}users] WHERE [{0}users].[uid]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                uid);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetShortUserInfoByName(string userName)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@username",(DbType)SqlDbType.VarChar,20,userName),
			                      };
            string commandText = string.Format("SELECT TOP 1 {0} FROM [{1}users] WHERE [{1}users].[username]=@username",
                                                DbFields.USERS,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        public DataTable GetUserList(int pageSize, int currentPage)
        {
            int pagetop = (currentPage - 1) * pageSize;
            string commandText;
            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} [{1}users].[uid], [{1}users].[username],[{1}users].[nickname], [{1}users].[joindate], [{1}users].[credits], [{1}users].[posts], [{1}users].[lastactivity], [{1}users].[email],[{1}users].[lastvisit],[{1}users].[lastvisit],[{1}users].[accessmasks], [{1}userfields].[location],[{1}usergroups].[grouptitle] FROM [{1}users] LEFT JOIN [{1}userfields] ON [{1}userfields].[uid] = [{1}users].[uid] LEFT JOIN [{1}usergroups] ON [{1}usergroups].[groupid]=[{1}users].[groupid] ORDER BY [{1}users].[uid] DESC",
                                             pageSize,
                                             BaseConfigs.GetTablePrefix);
            else
                commandText = string.Format("SELECT TOP {0} [{1}users].[uid], [{1}users].[username],[{1}users].[nickname], [{1}users].[joindate], [{1}users].[credits], [{1}users].[posts], [{1}users].[lastactivity], [{1}users].[email],[{1}users].[lastvisit],[{1}users].[lastvisit],[{1}users].[accessmasks], [{1}userfields].[location],[{1}usergroups].[grouptitle] FROM [{1}users],[{1}userfields],[{1}usergroups] WHERE [{1}userfields].[uid] = [{1}users].[uid] AND  [{1}usergroups].[groupid]=[{1}users].[groupid] AND [{1}users].[uid] < (SELECT min([uid])  FROM (SELECT TOP {2} [uid] FROM [{1}users] ORDER BY [uid] DESC) AS tblTmp )  ORDER BY [{1}users].[uid] DESC",
                                             pageSize,
                                             BaseConfigs.GetTablePrefix,
                                             pagetop);

            return DbHelper.ExecuteDataset(commandText).Tables[0];
        }

        /// <summary>
        /// 获得用户列表DataTable
        /// </summary>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="pageindex">当前页数</param>
        /// <returns>用户列表DataTable</returns>
        public DataTable GetUserList(int pageSize, int pageIndex, string column, string orderType)
        {
            string[] arrayorderby = new string[] { "username", "credits", "posts", "admin", "lastactivity", "joindate", "oltime" };
            int i = Array.IndexOf(arrayorderby, column);
            column = (i > 6 || i < 0) ? "uid" : arrayorderby[i];

            DbParameter[] parms = {
									   DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pageSize),
									   DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,pageIndex),
									   DbHelper.MakeInParam("@column",(DbType)SqlDbType.VarChar,1000,column),
                                       DbHelper.MakeInParam("@ordertype",(DbType)SqlDbType.VarChar,5,orderType)
								   };
            return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getuserlist", BaseConfigs.GetTablePrefix), parms).Tables[0];
        }

        /// <summary>
        /// 检测Email和安全项
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="email">email</param>
        /// <param name="questionid">问题id</param>
        /// <param name="answer">答案</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public IDataReader CheckEmailAndSecques(string userName, string email, string secques)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@username",(DbType)SqlDbType.Char,20,userName),
									   DbHelper.MakeInParam("@email",(DbType)SqlDbType.Char,50, email),
									   DbHelper.MakeInParam("@secques",(DbType)SqlDbType.Char,8, secques)
								   };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}checkemailandsecques", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 检测密码和安全项
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <param name="questionid">问题id</param>
        /// <param name="answer">答案</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public IDataReader CheckPasswordAndSecques(string userName, string passWord, bool originalPassWord, string secques)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@username",(DbType)SqlDbType.Char,20,userName),
									   DbHelper.MakeInParam("@password",(DbType)SqlDbType.Char,32, originalPassWord ? Utils.MD5(passWord) : passWord),
									   DbHelper.MakeInParam("@secques",(DbType)SqlDbType.Char,8, secques)
								   };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}checkpasswordandsecques", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public IDataReader CheckPassword(string userName, string passWord, bool originalPassWord)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@username",(DbType)SqlDbType.Char,20, userName),
									   DbHelper.MakeInParam("@password",(DbType)SqlDbType.Char,32, originalPassWord ? Utils.MD5(passWord) : passWord)
								   };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}checkpasswordbyusername", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 检测密码
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <param name="groupid">用户组id</param>
        /// <param name="adminid">管理id</param>
        /// <returns>如果用户密码正确则返回uid, 否则返回-1</returns>
        public IDataReader CheckPassword(int uid, string passWord, bool originalPassWord)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									   DbHelper.MakeInParam("@password",(DbType)SqlDbType.Char,32, originalPassWord ? Utils.MD5(passWord) : passWord)
								   };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}checkpasswordbyuid", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 根据指定的email查找用户并返回用户uid
        /// </summary>
        /// <param name="email">email地址</param>
        /// <returns>用户uid</returns>
        public IDataReader FindUserEmail(string email)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@email",(DbType)SqlDbType.Char,50, email),
								   };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getuseridbyemail", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 得到论坛中用户总数
        /// </summary>
        /// <returns>用户总数</returns>
        public int GetUserCount()
        {
            string commandText = string.Format("SELECT COUNT(uid) FROM [{0}users]", BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        /// <summary>
        /// 得到论坛中用户总数
        /// </summary>
        /// <returns>用户总数</returns>
        public int GetUserCountByAdmin()
        {
            string commandText = string.Format("SELECT COUNT(uid) FROM [{0}users] WHERE [{0}users].[adminid] > 0", BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        /// <summary>
        /// 创建新用户.
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>返回用户ID, 如果已存在该用户名则返回-1</returns>
        public int CreateUser(UserInfo userInfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@username",(DbType)SqlDbType.Char,20,userInfo.Username),
									   DbHelper.MakeInParam("@nickname",(DbType)SqlDbType.Char,20,userInfo.Nickname),
									   DbHelper.MakeInParam("@password",(DbType)SqlDbType.Char,32,userInfo.Password),
									   DbHelper.MakeInParam("@secques",(DbType)SqlDbType.Char,8,userInfo.Secques),
									   DbHelper.MakeInParam("@gender",(DbType)SqlDbType.Int,4,userInfo.Gender),
									   DbHelper.MakeInParam("@adminid",(DbType)SqlDbType.Int,4,userInfo.Adminid),
									   DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.SmallInt,2,userInfo.Groupid),
									   DbHelper.MakeInParam("@groupexpiry",(DbType)SqlDbType.Int,4,userInfo.Groupexpiry),
									   DbHelper.MakeInParam("@extgroupids",(DbType)SqlDbType.Char,60,userInfo.Extgroupids),
									   DbHelper.MakeInParam("@regip",(DbType)SqlDbType.VarChar,0,userInfo.Regip),
									   DbHelper.MakeInParam("@joindate",(DbType)SqlDbType.VarChar,0,userInfo.Joindate),
									   DbHelper.MakeInParam("@lastip",(DbType)SqlDbType.Char,15,userInfo.Lastip),
									   DbHelper.MakeInParam("@lastvisit",(DbType)SqlDbType.VarChar,0,userInfo.Lastvisit),
									   DbHelper.MakeInParam("@lastactivity",(DbType)SqlDbType.VarChar,0,userInfo.Lastactivity),
									   DbHelper.MakeInParam("@lastpost",(DbType)SqlDbType.VarChar,0,userInfo.Lastpost),
									   DbHelper.MakeInParam("@lastpostid",(DbType)SqlDbType.Int,4,userInfo.Lastpostid),
									   DbHelper.MakeInParam("@lastposttitle",(DbType)SqlDbType.VarChar,0,userInfo.Lastposttitle),
									   DbHelper.MakeInParam("@posts",(DbType)SqlDbType.Int,4,userInfo.Posts),
									   DbHelper.MakeInParam("@digestposts",(DbType)SqlDbType.SmallInt,2,userInfo.Digestposts),
									   DbHelper.MakeInParam("@oltime",(DbType)SqlDbType.Int,2,userInfo.Oltime),
									   DbHelper.MakeInParam("@pageviews",(DbType)SqlDbType.Int,4,userInfo.Pageviews),
									   DbHelper.MakeInParam("@credits",(DbType)SqlDbType.Int,4,userInfo.Credits),
									   DbHelper.MakeInParam("@extcredits1",(DbType)SqlDbType.Float,8,userInfo.Extcredits1),
									   DbHelper.MakeInParam("@extcredits2",(DbType)SqlDbType.Float,8,userInfo.Extcredits2),
									   DbHelper.MakeInParam("@extcredits3",(DbType)SqlDbType.Float,8,userInfo.Extcredits3),
									   DbHelper.MakeInParam("@extcredits4",(DbType)SqlDbType.Float,8,userInfo.Extcredits4),
									   DbHelper.MakeInParam("@extcredits5",(DbType)SqlDbType.Float,8,userInfo.Extcredits5),
									   DbHelper.MakeInParam("@extcredits6",(DbType)SqlDbType.Float,8,userInfo.Extcredits6),
									   DbHelper.MakeInParam("@extcredits7",(DbType)SqlDbType.Float,8,userInfo.Extcredits7),
									   DbHelper.MakeInParam("@extcredits8",(DbType)SqlDbType.Float,8,userInfo.Extcredits8),
									   DbHelper.MakeInParam("@avatarshowid",(DbType)SqlDbType.Int,4,0),
									   DbHelper.MakeInParam("@email",(DbType)SqlDbType.Char,50,userInfo.Email),
									   DbHelper.MakeInParam("@bday",(DbType)SqlDbType.VarChar,0,userInfo.Bday),
									   DbHelper.MakeInParam("@sigstatus",(DbType)SqlDbType.Int,4,userInfo.Sigstatus),
									   DbHelper.MakeInParam("@tpp",(DbType)SqlDbType.Int,4,userInfo.Tpp),
									   DbHelper.MakeInParam("@ppp",(DbType)SqlDbType.Int,4,userInfo.Ppp),
									   DbHelper.MakeInParam("@templateid",(DbType)SqlDbType.SmallInt,2,userInfo.Templateid),
									   DbHelper.MakeInParam("@pmsound",(DbType)SqlDbType.Int,4,userInfo.Pmsound),
									   DbHelper.MakeInParam("@showemail",(DbType)SqlDbType.Int,4,userInfo.Showemail),
									   DbHelper.MakeInParam("@newsletter",(DbType)SqlDbType.Int,4,userInfo.Newsletter),
									   DbHelper.MakeInParam("@invisible",(DbType)SqlDbType.Int,4,userInfo.Invisible),
									   DbHelper.MakeInParam("@newpm",(DbType)SqlDbType.Int,4,userInfo.Newpm),
									   DbHelper.MakeInParam("@accessmasks",(DbType)SqlDbType.Int,4,userInfo.Accessmasks),
                                       DbHelper.MakeInParam("@salt",(DbType)SqlDbType.NChar,6,userInfo.Salt),
									   DbHelper.MakeInParam("@website",(DbType)SqlDbType.VarChar,80,userInfo.Website),
									   DbHelper.MakeInParam("@icq",(DbType)SqlDbType.VarChar,12,userInfo.Icq),
									   DbHelper.MakeInParam("@qq",(DbType)SqlDbType.VarChar,12,userInfo.Qq),
									   DbHelper.MakeInParam("@yahoo",(DbType)SqlDbType.VarChar,40,userInfo.Yahoo),
									   DbHelper.MakeInParam("@msn",(DbType)SqlDbType.VarChar,40,userInfo.Msn),
									   DbHelper.MakeInParam("@skype",(DbType)SqlDbType.VarChar,40,userInfo.Skype),
									   DbHelper.MakeInParam("@location",(DbType)SqlDbType.VarChar,30,userInfo.Location),
									   DbHelper.MakeInParam("@customstatus",(DbType)SqlDbType.VarChar,30,userInfo.Customstatus),
									   DbHelper.MakeInParam("@avatar",(DbType)SqlDbType.VarChar,255,""),
									   DbHelper.MakeInParam("@avatarwidth",(DbType)SqlDbType.Int,4,0),
									   DbHelper.MakeInParam("@avatarheight",(DbType)SqlDbType.Int,4,0),
									   DbHelper.MakeInParam("@medals",(DbType)SqlDbType.VarChar,40, userInfo.Medals),
									   DbHelper.MakeInParam("@bio",(DbType)SqlDbType.NVarChar,500,userInfo.Bio),
									   DbHelper.MakeInParam("@signature",(DbType)SqlDbType.NVarChar,500,userInfo.Signature),
									   DbHelper.MakeInParam("@sightml",(DbType)SqlDbType.NVarChar,1000,userInfo.Sightml),
									   DbHelper.MakeInParam("@authstr",(DbType)SqlDbType.VarChar,20,userInfo.Authstr),
                                       DbHelper.MakeInParam("@realname",(DbType)SqlDbType.NVarChar,10,userInfo.Realname),
                                       DbHelper.MakeInParam("@idcard",(DbType)SqlDbType.VarChar,20,userInfo.Idcard),
                                       DbHelper.MakeInParam("@mobile",(DbType)SqlDbType.VarChar,20,userInfo.Mobile),
                                       DbHelper.MakeInParam("@phone",(DbType)SqlDbType.VarChar,20,userInfo.Phone)
								   };

            int uid = TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                    string.Format("{0}createuser", BaseConfigs.GetTablePrefix),
                                                                 parms), -1);
            if (uid != -1)
                UpdateTrendStat(TrendType.Register);
            return uid;
        }

        /// <summary>
        /// 更新用户信息.
        /// </summary>
        /// <param name="userinfo">用户信息</param>
        /// <returns>返回是否成功</returns>
        public bool UpdateUser(UserInfo userInfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@username",(DbType)SqlDbType.NChar,20,userInfo.Username),
									   DbHelper.MakeInParam("@nickname",(DbType)SqlDbType.NChar,20,userInfo.Nickname),
									   DbHelper.MakeInParam("@password",(DbType)SqlDbType.Char,32,userInfo.Password),
									   DbHelper.MakeInParam("@secques",(DbType)SqlDbType.Char,8,userInfo.Secques),
									   DbHelper.MakeInParam("@spaceid",(DbType)SqlDbType.Int,4,userInfo.Spaceid),
									   DbHelper.MakeInParam("@gender",(DbType)SqlDbType.Int,4,userInfo.Gender),
									   DbHelper.MakeInParam("@adminid",(DbType)SqlDbType.Int,4,userInfo.Adminid),
									   DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.SmallInt,2,userInfo.Groupid),
									   DbHelper.MakeInParam("@groupexpiry",(DbType)SqlDbType.NVarChar,50,userInfo.Groupexpiry),
									   DbHelper.MakeInParam("@extgroupids",(DbType)SqlDbType.Char,60,userInfo.Extgroupids),
									   DbHelper.MakeInParam("@regip",(DbType)SqlDbType.Char,15,userInfo.Regip),
									   DbHelper.MakeInParam("@joindate",(DbType)SqlDbType.Char,19,userInfo.Joindate),
									   DbHelper.MakeInParam("@lastip",(DbType)SqlDbType.Char,15,userInfo.Lastip),
									   DbHelper.MakeInParam("@lastvisit",(DbType)SqlDbType.Char,19,userInfo.Lastvisit),
									   DbHelper.MakeInParam("@lastactivity",(DbType)SqlDbType.Char,19,userInfo.Lastactivity),
									   DbHelper.MakeInParam("@lastpost",(DbType)SqlDbType.Char,19,userInfo.Lastpost),
									   DbHelper.MakeInParam("@lastpostid",(DbType)SqlDbType.Int,4,userInfo.Lastpostid),
									   DbHelper.MakeInParam("@lastposttitle",(DbType)SqlDbType.NChar,60,userInfo.Lastposttitle),
									   DbHelper.MakeInParam("@posts",(DbType)SqlDbType.Int,4,userInfo.Posts),
									   DbHelper.MakeInParam("@digestposts",(DbType)SqlDbType.SmallInt,2,userInfo.Digestposts),
									   DbHelper.MakeInParam("@oltime",(DbType)SqlDbType.Int,2,userInfo.Oltime),
									   DbHelper.MakeInParam("@pageviews",(DbType)SqlDbType.Int,4,userInfo.Pageviews),
									   DbHelper.MakeInParam("@credits",(DbType)SqlDbType.Int,4,userInfo.Credits),
									   DbHelper.MakeInParam("@extcredits1",(DbType)SqlDbType.Float,8,userInfo.Extcredits1),
									   DbHelper.MakeInParam("@extcredits2",(DbType)SqlDbType.Float,8,userInfo.Extcredits2),
									   DbHelper.MakeInParam("@extcredits3",(DbType)SqlDbType.Float,8,userInfo.Extcredits3),
									   DbHelper.MakeInParam("@extcredits4",(DbType)SqlDbType.Float,8,userInfo.Extcredits4),
									   DbHelper.MakeInParam("@extcredits5",(DbType)SqlDbType.Float,8,userInfo.Extcredits5),
									   DbHelper.MakeInParam("@extcredits6",(DbType)SqlDbType.Float,8,userInfo.Extcredits6),
									   DbHelper.MakeInParam("@extcredits7",(DbType)SqlDbType.Float,8,userInfo.Extcredits7),
									   DbHelper.MakeInParam("@extcredits8",(DbType)SqlDbType.Float,8,userInfo.Extcredits8),
									   DbHelper.MakeInParam("@avatarshowid",(DbType)SqlDbType.Int,4,0),
									   DbHelper.MakeInParam("@email",(DbType)SqlDbType.Char,50,userInfo.Email),
									   DbHelper.MakeInParam("@bday",(DbType)SqlDbType.Char,19,userInfo.Bday),
									   DbHelper.MakeInParam("@sigstatus",(DbType)SqlDbType.Int,4,userInfo.Sigstatus),
									   DbHelper.MakeInParam("@tpp",(DbType)SqlDbType.Int,4,userInfo.Tpp),
									   DbHelper.MakeInParam("@ppp",(DbType)SqlDbType.Int,4,userInfo.Ppp),
									   DbHelper.MakeInParam("@templateid",(DbType)SqlDbType.SmallInt,2,userInfo.Templateid),
									   DbHelper.MakeInParam("@pmsound",(DbType)SqlDbType.Int,4,userInfo.Pmsound),
									   DbHelper.MakeInParam("@showemail",(DbType)SqlDbType.Int,4,userInfo.Showemail),
									   DbHelper.MakeInParam("@newsletter",(DbType)SqlDbType.Int,4,userInfo.Newsletter),
									   DbHelper.MakeInParam("@invisible",(DbType)SqlDbType.Int,4,userInfo.Invisible),
									   DbHelper.MakeInParam("@newpm",(DbType)SqlDbType.Int,4,userInfo.Newpm),
									   DbHelper.MakeInParam("@newpmcount",(DbType)SqlDbType.Int,4,userInfo.Newpmcount),
									   DbHelper.MakeInParam("@accessmasks",(DbType)SqlDbType.Int,4,userInfo.Accessmasks),
									   DbHelper.MakeInParam("@onlinestate",(DbType)SqlDbType.Int,4,userInfo.Onlinestate),
									   DbHelper.MakeInParam("@website",(DbType)SqlDbType.VarChar,80,userInfo.Website),
									   DbHelper.MakeInParam("@icq",(DbType)SqlDbType.VarChar,12,userInfo.Icq),
									   DbHelper.MakeInParam("@qq",(DbType)SqlDbType.VarChar,12,userInfo.Qq),
									   DbHelper.MakeInParam("@yahoo",(DbType)SqlDbType.VarChar,40,userInfo.Yahoo),
									   DbHelper.MakeInParam("@msn",(DbType)SqlDbType.VarChar,40,userInfo.Msn),
									   DbHelper.MakeInParam("@skype",(DbType)SqlDbType.VarChar,40,userInfo.Skype),
									   DbHelper.MakeInParam("@location",(DbType)SqlDbType.VarChar,30,userInfo.Location),
									   DbHelper.MakeInParam("@customstatus",(DbType)SqlDbType.VarChar,30,userInfo.Customstatus),
									   DbHelper.MakeInParam("@avatar",(DbType)SqlDbType.VarChar,255,""),
									   DbHelper.MakeInParam("@avatarwidth",(DbType)SqlDbType.Int,4,0),
									   DbHelper.MakeInParam("@avatarheight",(DbType)SqlDbType.Int,4,0),
									   DbHelper.MakeInParam("@medals",(DbType)SqlDbType.VarChar,300, userInfo.Medals),
									   DbHelper.MakeInParam("@bio",(DbType)SqlDbType.NVarChar,500,userInfo.Bio),
									   DbHelper.MakeInParam("@signature",(DbType)SqlDbType.NVarChar,500,userInfo.Signature),
									   DbHelper.MakeInParam("@sightml",(DbType)SqlDbType.NVarChar,1000,userInfo.Sightml),
									   DbHelper.MakeInParam("@authstr",(DbType)SqlDbType.VarChar,20,userInfo.Authstr),
									   DbHelper.MakeInParam("@authtime",(DbType)SqlDbType.SmallDateTime,4,userInfo.Authtime),
									   DbHelper.MakeInParam("@authflag",(DbType)SqlDbType.TinyInt,1,userInfo.Authflag),
                                       DbHelper.MakeInParam("@realname",(DbType)SqlDbType.NVarChar,10,userInfo.Realname),
                                       DbHelper.MakeInParam("@idcard",(DbType)SqlDbType.VarChar,20,userInfo.Idcard),
                                       DbHelper.MakeInParam("@mobile",(DbType)SqlDbType.VarChar,20,userInfo.Mobile),
                                       DbHelper.MakeInParam("@phone",(DbType)SqlDbType.VarChar,20,userInfo.Phone),
                                       DbHelper.MakeInParam("@ignorepm",(DbType)SqlDbType.NVarChar,1000,userInfo.Ignorepm),
                                       DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,userInfo.Uid)
								   };

            return TypeConverter.ObjectToInt(DbHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                                                      string.Format("{0}updateuser", BaseConfigs.GetTablePrefix),
                                                                      parms), -1) == 2;
        }

        /// <summary>
        /// 更新权限验证字符串
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="authstr">验证串</param>
        /// <param name="authflag">验证标志</param>
        public void UpdateAuthStr(int uid, string authStr, int authFlag)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
									   DbHelper.MakeInParam("@authstr", (DbType)SqlDbType.Char, 20, authStr),
									   DbHelper.MakeInParam("@authflag", (DbType)SqlDbType.Int, 4, authFlag) 
								   };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateuserauthstr", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 更新指定用户的个人资料
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>如果用户不存在则为false, 否则为true</returns>
        public void UpdateUserProfile(UserInfo userInfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userInfo.Uid), 
									   DbHelper.MakeInParam("@nickname",(DbType)SqlDbType.Char,20,userInfo.Nickname),
									   DbHelper.MakeInParam("@gender", (DbType)SqlDbType.Int, 4, userInfo.Gender), 
									   DbHelper.MakeInParam("@email", (DbType)SqlDbType.Char, 50, userInfo.Email), 
									   DbHelper.MakeInParam("@bday", (DbType)SqlDbType.Char, 10, userInfo.Bday), 
									   DbHelper.MakeInParam("@showemail", (DbType)SqlDbType.Int, 4, userInfo.Showemail),
									   DbHelper.MakeInParam("@website", (DbType)SqlDbType.VarChar, 80, userInfo.Website), 
									   DbHelper.MakeInParam("@icq", (DbType)SqlDbType.VarChar, 12, userInfo.Icq), 
									   DbHelper.MakeInParam("@qq", (DbType)SqlDbType.VarChar, 12, userInfo.Qq), 
									   DbHelper.MakeInParam("@yahoo", (DbType)SqlDbType.VarChar, 40, userInfo.Yahoo), 
									   DbHelper.MakeInParam("@msn", (DbType)SqlDbType.VarChar, 40, userInfo.Msn), 
									   DbHelper.MakeInParam("@skype", (DbType)SqlDbType.VarChar, 40, userInfo.Skype), 
									   DbHelper.MakeInParam("@location", (DbType)SqlDbType.NVarChar, 30, userInfo.Location), 
									   DbHelper.MakeInParam("@bio", (DbType)SqlDbType.NVarChar, 500, userInfo.Bio),
                                       DbHelper.MakeInParam("@signature", (DbType)SqlDbType.NVarChar, 500, userInfo.Signature),
                                       DbHelper.MakeInParam("@sigstatus", (DbType)SqlDbType.Int, 4, userInfo.Sigstatus),
                                       DbHelper.MakeInParam("@sightml", (DbType)SqlDbType.NVarChar, 1000, userInfo.Sightml),
                                       DbHelper.MakeInParam("@realname",(DbType)SqlDbType.NVarChar,10,userInfo.Realname),
                                       DbHelper.MakeInParam("@idcard",(DbType)SqlDbType.VarChar,20,userInfo.Idcard),
                                       DbHelper.MakeInParam("@mobile",(DbType)SqlDbType.VarChar,20,userInfo.Mobile),
                                       DbHelper.MakeInParam("@phone",(DbType)SqlDbType.VarChar,20,userInfo.Phone)
								   };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateuserprofile", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 更新用户论坛设置
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>如果用户不存在则返回false, 否则返回true</returns>
        public void UpdateUserForumSetting(UserInfo userInfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,userInfo.Uid),
									   DbHelper.MakeInParam("@tpp",(DbType)SqlDbType.Int,4,userInfo.Tpp),
									   DbHelper.MakeInParam("@ppp",(DbType)SqlDbType.Int,4,userInfo.Ppp),
									   DbHelper.MakeInParam("@invisible",(DbType)SqlDbType.Int,4,userInfo.Invisible),
									   DbHelper.MakeInParam("@customstatus",(DbType)SqlDbType.VarChar,30,userInfo.Customstatus)
								   };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateuserforumsetting", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 修改用户自定义积分字段的值
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="extid">扩展字段序号(1-8)</param>
        /// <param name="pos">增加的数值(可以是负数)</param>
        /// <returns>执行是否成功</returns>
        public void UpdateUserExtCredits(int uid, int extId, float pos)
        {
            string commandText = string.Format("UPDATE [{0}users] SET [extcredits{1}]=[extcredits{1}] + {2} WHERE [uid]={3}",
                                                BaseConfigs.GetTablePrefix,
                                                extId,
                                                pos,
                                                uid);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获得指定用户的指定积分扩展字段的值
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="extid">扩展字段序号(1-8)</param>
        /// <returns>值</returns>
        public float GetUserExtCredits(int uid, int extId)
        {
            string commandText = string.Format("SELECT [extcredits{0}] FROM [{1}users] WHERE [uid]={2}",
                                                extId,
                                                BaseConfigs.GetTablePrefix,
                                                uid);
            return TypeConverter.ObjectToFloat(DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="avatar">头像</param>
        /// <param name="avatarwidth">头像宽度</param>
        /// <param name="avatarheight">头像高度</param>
        /// <returns>如果用户不存在则返回false, 否则返回true</returns>
        public void UpdateUserPreference(int uid, string avatar, int avatarWidth, int avatarHeight, int templateId)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									   DbHelper.MakeInParam("@avatar",(DbType)SqlDbType.VarChar,255,avatar),
									   DbHelper.MakeInParam("@avatarwidth",(DbType)SqlDbType.Int,4,avatarWidth),
									   DbHelper.MakeInParam("@avatarheight",(DbType)SqlDbType.Int,4,avatarHeight),
                                       DbHelper.MakeInParam("@templateid", (DbType)SqlDbType.SmallInt, 4, templateId)
								   };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateuserpreference", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <returns>成功返回true否则false</returns>
        public void UpdateUserPassword(int uid, string passWord, bool originalPassWord)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
									   DbHelper.MakeInParam("@password", (DbType)SqlDbType.Char, 32, originalPassWord ? Utils.MD5(passWord) : passWord)
								   };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateuserpassword", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 更新用户安全问题
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="questionid">问题id</param>
        /// <param name="answer">答案</param>
        /// <returns>成功返回true否则false</returns>
        public void UpdateUserSecques(int uid, string secques)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
									   DbHelper.MakeInParam("@secques", (DbType)SqlDbType.Char, 8, secques)
								   };
            string commandText = string.Format("UPDATE [{0}users] SET [secques]=@secques WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新用户最后登录时间
        /// </summary>
        /// <param name="uid">用户id</param>
        public void UpdateUserLastvisit(int uid, string ip)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
									   DbHelper.MakeInParam("@ip", (DbType)SqlDbType.Char,15, ip)
								   };
            string commandText = string.Format("UPDATE [{0}users] SET [lastvisit]=GETDATE(), [lastip]=@ip WHERE [uid] =@uid",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void UpdateUserOnlineStateAndLastActivity(string uidList, int onlineState, string activityTime)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@onlinestate", (DbType)SqlDbType.Int, 4, onlineState),
									    DbHelper.MakeInParam("@activitytime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(activityTime))
								   };
            string commandText = string.Format("UPDATE [{0}users] SET [onlinestate]=@onlinestate,[lastactivity] = @activitytime WHERE [uid] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                uidList);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void UpdateUserOnlineStateAndLastActivity(int uid, int onlineState, string activityTime)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
                                        DbHelper.MakeInParam("@onlinestate", (DbType)SqlDbType.Int, 4, onlineState),
									    DbHelper.MakeInParam("@activitytime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(activityTime))
								   };
            string commandText = string.Format("UPDATE [{0}users] SET [onlinestate]=@onlinestate,[lastactivity] = @activitytime WHERE [uid]=@uid",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void UpdateUserOnlineStateAndLastVisit(string uidList, int onlineState, string activityTime)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@onlinestate", (DbType)SqlDbType.Int, 4, onlineState),
									    DbHelper.MakeInParam("@activitytime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(activityTime))
								   };
            string commandText = string.Format("UPDATE [{0}users] SET [onlinestate]=@onlinestate,[lastvisit] = @activitytime WHERE [uid] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                uidList);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void UpdateUserOnlineStateAndLastVisit(int uid, int onlineState, string activityTime)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
                                        DbHelper.MakeInParam("@onlinestate", (DbType)SqlDbType.Int, 4, onlineState),
									    DbHelper.MakeInParam("@activitytime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(activityTime))
								   };
            string commandText = string.Format("UPDATE [{0}users] SET [onlinestate]=@onlinestate,[lastvisit] = @activitytime WHERE [uid]=@uid",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新用户当前的在线时间和最后活动时间
        /// </summary>
        /// <param name="uid">用户uid</param>
        public void UpdateUserLastActivity(int uid, string activityTime)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
									   DbHelper.MakeInParam("@activitytime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(activityTime))
								   };
            string commandText = string.Format("UPDATE [{0}users] SET [lastactivity] = @activitytime  WHERE [uid] = @uid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 设置用户信息表中未读短消息的数量
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="pmnum">短消息数量</param>
        /// <returns>更新记录个数</returns>
        public int SetUserNewPMCount(int uid, int pmNum)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
									   DbHelper.MakeInParam("@value", (DbType)SqlDbType.Int, 4, pmNum)
			                      };
            string commandText = string.Format("UPDATE [{0}users] SET [newpmcount]=@value WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新指定用户的勋章信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="medals">勋章信息</param>
        public void UpdateMedals(int uid, string medals)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
									   DbHelper.MakeInParam("@medals", (DbType)SqlDbType.VarChar, 300, medals)
								   };
            string commandText = string.Format("UPDATE [{0}userfields] SET [medals]=@medals WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

        }

        public int DecreaseNewPMCount(int uid, int subVal)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
									   DbHelper.MakeInParam("@subval", (DbType)SqlDbType.Int, 4, subVal)
			                        };
            try
            {
                string commandText = string.Format("UPDATE [{0}users] SET [newpmcount]=CASE WHEN [newpmcount] >= 0 THEN [newpmcount]-@subval ELSE 0 END WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);
                return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            }
            catch
            {
                return -1;
            }
        }


        /// <summary>
        /// 更新用户精华数
        /// </summary>
        /// <param name="useridlist">uid列表</param>
        /// <returns></returns>
        public int UpdateUserDigest(string userIdList)
        {
            string commandText = string.Format("UPDATE [{0}users] SET [digestposts] = (SELECT COUNT([tid]) AS [digest] FROM [{0}topics] WHERE [{0}topics].[posterid] = [{0}users].[uid] AND [digest]>0) WHERE [uid] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                userIdList);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 更新用户SpaceID
        /// </summary>
        /// <param name="spaceid">要更新的SpaceId</param>
        /// <param name="userId">要更新的UserId</param>
        /// <returns></returns>
        public void UpdateUserSpaceId(int spaceId, int userId)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@spaceid",(DbType)SqlDbType.Int,4,spaceId),
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,userId)
								   };
            string commandText = string.Format("UPDATE [{0}users] SET [spaceid]=@spaceid WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public DataTable GetUserIdByAuthStr(string authStr)
        {
            DbParameter[] parms = {
										  DbHelper.MakeInParam("@authstr",(DbType)SqlDbType.VarChar,20,authStr)
				                    };
            string commandText = string.Format("SELECT [uid] FROM [{0}userfields] WHERE DateDiff(d,[authtime],getdate())<=3  AND [authstr]=@authstr",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        /// <summary>
        /// 执行在线用户向表及缓存中添加的操作。
        /// </summary>
        /// <param name="onlineuserinfo">在组用户信息内容</param>
        /// <param name="timeout">系统设置用户多少时间即算做离线</param>
        /// <param name="deletingfrequency">删除过期用户频率(单位:分钟)</param>
        /// <returns>添加成功则返回刚刚添加的olid,失败则返回0</returns>
        public int AddOnlineUser(OnlineUserInfo onlineUserInfo, int timeOut, int deletingFrequency)
        {
            //标识需要更新用户在线状态，0表示需要更新
            int onlinestate = 1;

            // 如果timeout为负数则代表不需要精确更新用户是否在线的状态
            if (timeOut > 0)
            {
                if (onlineUserInfo.Userid > 0)
                    onlinestate = 0;
            }
            else
                timeOut = timeOut * -1;

            if (timeOut > 9999)
                timeOut = 9999;

            DbParameter[] parms = {
									   DbHelper.MakeInParam("@onlinestate",(DbType)SqlDbType.Int,4,onlinestate),
									   DbHelper.MakeInParam("@userid",(DbType)SqlDbType.Int,4,onlineUserInfo.Userid),
									   DbHelper.MakeInParam("@ip",(DbType)SqlDbType.VarChar,15,onlineUserInfo.Ip),
									   DbHelper.MakeInParam("@username",(DbType)SqlDbType.NVarChar,40,onlineUserInfo.Username),
									   DbHelper.MakeInParam("@nickname",(DbType)SqlDbType.NVarChar,40,onlineUserInfo.Nickname),
									   DbHelper.MakeInParam("@password",(DbType)SqlDbType.Char,32,onlineUserInfo.Password),
									   DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.SmallInt,2,onlineUserInfo.Groupid),
									   DbHelper.MakeInParam("@olimg",(DbType)SqlDbType.VarChar,80,onlineUserInfo.Olimg),
									   DbHelper.MakeInParam("@adminid",(DbType)SqlDbType.SmallInt,2,onlineUserInfo.Adminid),
									   DbHelper.MakeInParam("@invisible",(DbType)SqlDbType.SmallInt,2,onlineUserInfo.Invisible),
									   DbHelper.MakeInParam("@action",(DbType)SqlDbType.SmallInt,2,onlineUserInfo.Action),
									   DbHelper.MakeInParam("@lastactivity",(DbType)SqlDbType.SmallInt,2,onlineUserInfo.Lastactivity),
									   DbHelper.MakeInParam("@lastposttime",(DbType)SqlDbType.DateTime,8,TypeConverter.StrToDateTime(onlineUserInfo.Lastposttime)),
									   DbHelper.MakeInParam("@lastpostpmtime",(DbType)SqlDbType.DateTime,8,TypeConverter.StrToDateTime(onlineUserInfo.Lastpostpmtime)),
									   DbHelper.MakeInParam("@lastsearchtime",(DbType)SqlDbType.DateTime,8,TypeConverter.StrToDateTime(onlineUserInfo.Lastsearchtime)),
									   DbHelper.MakeInParam("@lastupdatetime",(DbType)SqlDbType.DateTime,8,TypeConverter.StrToDateTime(onlineUserInfo.Lastupdatetime)),
									   DbHelper.MakeInParam("@forumid",(DbType)SqlDbType.Int,4,onlineUserInfo.Forumid),
									   DbHelper.MakeInParam("@forumname",(DbType)SqlDbType.NVarChar,50,""),
									   DbHelper.MakeInParam("@titleid",(DbType)SqlDbType.Int,4,onlineUserInfo.Titleid),
									   DbHelper.MakeInParam("@title",(DbType)SqlDbType.NVarChar,80,""),
									   DbHelper.MakeInParam("@verifycode",(DbType)SqlDbType.VarChar,10,onlineUserInfo.Verifycode),
									   DbHelper.MakeInParam("@newpms",(DbType)SqlDbType.SmallInt,2,onlineUserInfo.Newpms),
									   DbHelper.MakeInParam("@newnotices",(DbType)SqlDbType.SmallInt,2,onlineUserInfo.Newnotices)
                                       //DbHelper.MakeInParam("@newfriendrequest",(DbType)SqlDbType.SmallInt,2,onlineUserInfo.Newfriendrequest),
                                       //DbHelper.MakeInParam("@newapprequest",(DbType)SqlDbType.SmallInt,2,onlineUserInfo.Newapprequest)
								   };

            //当在线表被布署到别的数据库时
            if (EntLibConfigs.GetConfig() != null)
            {
                OnlineTableConnect onlineTableConnect = EntLibConfigs.GetConfig().Onlinetableconnect;
                if (onlineTableConnect.Enable && !string.IsNullOrEmpty(onlineTableConnect.SqlServerConn))
                {
                    if (onlinestate == 0)
                        DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [onlinestate]=1 WHERE [uid]=@userid", BaseConfigs.GetTablePrefix), parms);

                    parms[0].Value = 1;//HACK：这个就不会在存储过程中运行更新users表信息的语句了，因为可能在分离的dnt_online表所在数据库中没有USER表。 
                }
            }

            int olid = TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                        string.Format("{0}createonlineuser", BaseConfigs.GetTablePrefix),
                                                                        parms));

            //按照系统设置频率(默认5分钟)清除过期用户
            if (_lastRemoveTimeout == 0 || (System.Environment.TickCount - _lastRemoveTimeout) > 60000 * deletingFrequency)
            {
                DeleteExpiredOnlineUsers(timeOut);
                _lastRemoveTimeout = System.Environment.TickCount;
            }
            // 如果id值太大则重建在线表
            if (olid > 2147483000)
            {
                CreateOnlineTable();

                olid = TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                        string.Format("{0}createonlineuser", BaseConfigs.GetTablePrefix),
                                                                        parms));

                //DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}createonlineuser", BaseConfigs.GetTablePrefix), parms);
                //  return olid;
            }
            return olid;
        }

        private void DeleteExpiredOnlineUsers(int timeOut)
        {
            System.Text.StringBuilder timeoutStrBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder memberStrBuilder = new System.Text.StringBuilder();

            DbParameter param = DbHelper.MakeInParam("@expires", (DbType)SqlDbType.DateTime, 8, DateTime.Now.AddMinutes(timeOut * -1));
            IDataReader dr = DbHelper.ExecuteReader(CommandType.StoredProcedure,
                                                    string.Format("{0}getexpiredonlineuserlist", BaseConfigs.GetTablePrefix),
                                                    param);
            while (dr.Read())
            {
                timeoutStrBuilder.Append(",");
                timeoutStrBuilder.Append(dr["olid"].ToString());
                if (dr["userid"].ToString() != "-1")
                {
                    memberStrBuilder.Append(",");
                    memberStrBuilder.Append(dr["userid"].ToString());
                }
            }
            dr.Close();

            if (timeoutStrBuilder.Length > 0)
            {
                timeoutStrBuilder.Remove(0, 1);
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                         string.Format("{0}deleteonlineusers", BaseConfigs.GetTablePrefix),
                                         DbHelper.MakeInParam("@olidlist", (DbType)SqlDbType.VarChar, 5000, timeoutStrBuilder.Length <= 5000 ? timeoutStrBuilder.ToString() : timeoutStrBuilder.ToString().Substring(0, 5000).TrimEnd(',')));
            }
            if (memberStrBuilder.Length > 0)
            {
                memberStrBuilder.Remove(0, 1);
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                         string.Format("{0}updateuseronlinestates", BaseConfigs.GetTablePrefix),
                                         DbHelper.MakeInParam("@uidlist", (DbType)SqlDbType.VarChar, 5000, memberStrBuilder.Length <= 5000 ? memberStrBuilder.ToString() : memberStrBuilder.ToString().Substring(0, 5000).TrimEnd(',')));
            }
        }

        public DataTable GetUserInfo(int userId)
        {
            string commandText = string.Format("SELECT TOP 1 {0} FROM [{1}users] WHERE [uid]={2}",
                                                DbFields.USERS,
                                                BaseConfigs.GetTablePrefix,
                                                userId);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public DataTable GetUserInfo(string userName, string passWord)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar, 20, userName),
                                        DbHelper.MakeInParam("@password", (DbType)SqlDbType.Char, 32, passWord)
                                    };
            string commandText = string.Format("SELECT TOP 1 {0} FROM [{1}users] WHERE [username]=@username AND [password]=@password",
                                                DbFields.USERS,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        public void UpdateUserSpaceId(int userId)
        {
            string commandText = string.Format("UPDATE [{0}users] SET [spaceid]=ABS([spaceid]) WHERE [uid]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                userId);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public int GetUserIdByRewriteName(string rewriteName)
        {
            DbParameter parm = DbHelper.MakeInParam("@rewritename", (DbType)SqlDbType.Char, 100, rewriteName);

            string commandText = string.Format("SELECT [userid] FROM [{0}spaceconfigs] WHERE [rewritename]=@rewritename",
                                                BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parm), -1);
        }

        public void UpdateUserPMSetting(UserInfo user)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, user.Uid),
                                    DbHelper.MakeInParam("@pmsound", (DbType)SqlDbType.Int, 4, user.Pmsound),
                                    DbHelper.MakeInParam("@newsletter", (DbType)SqlDbType.Int, 4, (int)user.Newsletter)
                                  };
            string commandText = string.Format(@"UPDATE [{0}users] SET [pmsound]=@pmsound, [newsletter]=@newsletter WHERE [uid]=@uid",
                                                 BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            parms = new DbParameter[] {
                                    DbHelper.MakeInParam("@ignorepm", (DbType)SqlDbType.NVarChar, 1000, user.Ignorepm),
                                    DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, user.Uid)
                                     };
            commandText = string.Format(@"UPDATE [{0}userfields] SET [ignorepm]=@ignorepm WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void ClearUserSpace(int uid)
        {
            string commandText = string.Format("UPDATE [{0}users] SET [spaceid]=0 WHERE [uid]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                uid);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }


        public IDataReader GetUserInfoByName(string userName)
        {
            string commandText = string.Format("SELECT [uid], [username] FROM [{0}users] WHERE [username] LIKE '%{1}%'",
                                                BaseConfigs.GetTablePrefix,
                                                RegEsc(userName));
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }


        public DataTable UserList(int pageSize, int currentPage, string condition)
        {
            int pagetop = (currentPage - 1) * pageSize;
            string commandText;
            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} [{1}users].[uid], [{1}users].[username],[{1}users].[nickname], [{1}users].[joindate], [{1}users].[credits], [{1}users].[posts], [{1}users].[lastactivity], [{1}users].[email],[{1}users].[lastvisit],[{1}users].[lastvisit],[{1}users].[accessmasks], [{1}userfields].[location],[{1}usergroups].[grouptitle] FROM [{1}users] LEFT JOIN [{1}userfields] ON [{1}userfields].[uid] = [{1}users].[uid]  LEFT JOIN [{1}usergroups] ON [{1}usergroups].[groupid]=[{1}users].[groupid] WHERE {2} ORDER BY [{1}users].[uid] DESC",
                                             pageSize,
                                             BaseConfigs.GetTablePrefix,
                                             condition);
            else
                commandText = string.Format("SELECT TOP {0} [{1}users].[uid], [{1}users].[username],[{1}users].[nickname], [{1}users].[joindate], [{1}users].[credits], [{1}users].[posts], [{1}users].[lastactivity], [{1}users].[email],[{1}users].[lastvisit],[{1}users].[lastvisit],[{1}users].[accessmasks], [{1}userfields].[location],[{1}usergroups].[grouptitle] FROM [{1}users],[{1}userfields],[{1}usergroups]  WHERE [{1}userfields].[uid] = [{1}users].[uid] AND  [{1}usergroups].[groupid]=[{1}users].[groupid] AND [{1}users].[uid] < (SELECT min([uid])  FROM (SELECT TOP {3} [uid] FROM [{1}users] WHERE {4} ORDER BY [uid] DESC) AS tblTmp ) AND {4} ORDER BY [{1}users].[uid] DESC",
                                             pageSize,
                                             BaseConfigs.GetTablePrefix,
                                             condition,
                                             pagetop,
                                             condition);

            return DbHelper.ExecuteDataset(commandText).Tables[0];
        }

        public void UpdateOnlineTime(int oltimeSpan, int uid)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
                                    DbHelper.MakeInParam("@oltimespan", (DbType)SqlDbType.SmallInt, 2, oltimeSpan),
                                    DbHelper.MakeInParam("@lastupdate", (DbType)SqlDbType.DateTime, 8, DateTime.Now),
                                    DbHelper.MakeInParam("@expectedlastupdate", (DbType)SqlDbType.DateTime, 8, DateTime.Now.AddMinutes(0 - oltimeSpan))
                                };
            string commandText = string.Format("UPDATE [{0}onlinetime] SET [thismonth]=[thismonth]+@oltimespan, [total]=[total]+@oltimespan, [lastupdate]=@lastupdate WHERE [uid]=@uid AND [lastupdate]<=@expectedlastupdate",
                                                BaseConfigs.GetTablePrefix);
            if (DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms) < 1)
            {
                try
                {
                    commandText = string.Format("INSERT INTO [{0}onlinetime]([uid], [thismonth], [total], [lastupdate]) VALUES(@uid, @oltimespan, @oltimespan, @lastupdate)",
                                                 BaseConfigs.GetTablePrefix);
                    DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
                }
                catch { }
            }
        }

        /// <summary>
        /// 重置每月在线时间(清零)
        /// </summary>
        public void ResetThismonthOnlineTime()
        {
            DbHelper.ExecuteNonQuery(string.Format("UPDATE [{0}onlinetime] SET [thismonth]=0", BaseConfigs.GetTablePrefix));
        }

        public void SynchronizeOnlineTime(int uid)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
                                  };
            string commandText = string.Format("SELECT [total] FROM [{0}onlinetime] WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);
            int total = TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms));

            commandText = string.Format("UPDATE [{0}users] SET [oltime]={1} WHERE [oltime]<{1} AND [uid]=@uid", BaseConfigs.GetTablePrefix, total);
            if (DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms) < 1)
            {
                try
                {
                    commandText = string.Format("UPDATE [{0}onlinetime] SET [total]=(SELECT [oltime] FROM [{0}users] WHERE [uid]=@uid) WHERE [uid]=@uid",
                                                 BaseConfigs.GetTablePrefix);
                    DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
                }
                catch { }
            }
        }

        public IDataReader GetUserByOnlineTime(string field)
        {
            string commandText = string.Format("SELECT TOP 20 [o].[uid], [u].[username], [o].[{0}] FROM [{1}onlinetime] [o] LEFT JOIN [{1}users] [u] ON [o].[uid]=[u].[uid] ORDER BY [o].[{0}] DESC",
                                                field,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }


        public void UpdateBanUser(int groupId, string groupExpiry, int uid)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupId),
                                    DbHelper.MakeInParam("@groupexpiry", (DbType)SqlDbType.NVarChar, 50, groupExpiry),
                                    DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
                                };
            //string commandText = string.Format("UPDATE [{0}users] SET [groupid]=@groupid, [groupexpiry]=@groupexpiry WHERE [uid]=@uid",
            //                                    BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateBanUser", BaseConfigs.GetTablePrefix), parms);
        }


        public DataTable SearchSpecialUser(int fid)
        {
            DbParameter[] parms = { 
                                     DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int,4,fid)
                                  };
            string commandText = string.Format("SELECT {0} FROM [{1}forums] f JOIN [{1}forumfields] ff ON [f].[fid]=[ff].[fid] where [f].[fid]=@fid",
                                                DbFields.FORUMS_JOIN_FIELDS,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        public void UpdateSpecialUser(string permUserList, int fid)
        {
            DbParameter[] parms = { 
                                     DbHelper.MakeInParam("@permuserlist",(DbType)SqlDbType.NText, 0, permUserList),
                                     DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int, 4, fid)
                                  };
            string commandText = string.Format("UPDATE [{0}forumfields] SET [permuserlist]=@permuserlist WHERE [fid]=@fid",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public int UpdateNewPms(int olid, int count)
        {
            DbParameter[] parms = {
                                     DbHelper.MakeInParam("@action",(DbType)SqlDbType.NChar,30,"newpms"),
                                     DbHelper.MakeInParam("@count",(DbType)SqlDbType.SmallInt, 2, short.Parse(count.ToString())),
                                     DbHelper.MakeInParam("@olid",(DbType)SqlDbType.Int, 4, olid)
                                  };
            //string commandText = string.Format("UPDATE [{0}online] SET [newpms]=@count WHERE [olid]=@olid", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateonlinenewinfo", BaseConfigs.GetTablePrefix), parms);
            //return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public int UpdateNewNotices(int olId, int plusCount)
        {
            DbParameter[] parms = { 
                                     DbHelper.MakeInParam("@action",(DbType)SqlDbType.NChar,30,"newnotice"),
                                     DbHelper.MakeInParam("@count",(DbType)SqlDbType.SmallInt, 2, short.Parse(plusCount.ToString())),
                                     DbHelper.MakeInParam("@olid",(DbType)SqlDbType.Int, 4, olId)
                                  };
            //string commandText = string.Format("UPDATE [{0}online] SET [newnotices]=[newnotices]+@pluscount WHERE [olid]=@olid",
            //                                    BaseConfigs.GetTablePrefix);
            //return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateonlinenewinfo", BaseConfigs.GetTablePrefix), parms);
        }

        //public int UpdateNewFriendsRequest(int olId, int count)
        //{
        //    DbParameter[] parms = { 
        //                             DbHelper.MakeInParam("@action",(DbType)SqlDbType.NChar,30,"newfriendrequest"),
        //                             DbHelper.MakeInParam("@count",(DbType)SqlDbType.SmallInt, 2, short.Parse(count.ToString())),
        //                             DbHelper.MakeInParam("@olid",(DbType)SqlDbType.Int, 4, olId)
        //                          };
        //    return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateonlinenewinfo", BaseConfigs.GetTablePrefix), parms);
        //}

        //public int UpdateNewApplicationRequest(int olId, int count)
        //{
        //    DbParameter[] parms = { 
        //                             DbHelper.MakeInParam("@action",(DbType)SqlDbType.NChar,30,"newapprequest"),
        //                             DbHelper.MakeInParam("@count",(DbType)SqlDbType.SmallInt, 2, short.Parse(count.ToString())),
        //                             DbHelper.MakeInParam("@olid",(DbType)SqlDbType.Int, 4, olId)
        //                          };
        //    return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateonlinenewinfo", BaseConfigs.GetTablePrefix), parms);
        //}

        /// <summary>
        /// 得到指定用户的指定积分扩展字段的积分值
        /// </summary>
        /// <returns>扩展字展积分值</returns>
        public int GetUserExtCreditsByUserid(int uid, int extNumber)
        {
            string commandText = string.Format("SELECT TOP 1 [extcredits{0}] FROM [{1}users] WHERE [uid] = {2}",
                                                extNumber,
                                                BaseConfigs.GetTablePrefix,
                                                uid);
            return Convert.ToInt32(TypeConverter.ObjectToFloat(DbHelper.ExecuteScalar(CommandType.Text, commandText), 0));
        }

        /// <summary>
        /// 建立更新用户积分存储过程的方法
        /// </summary>
        /// <param name="creditExpression">总积分计算公式</param>
        /// <param name="testCreditExpression">是否需要测试总积分计算公式是否正确</param>
        /// <returns></returns>
        public bool CreateUpdateUserCreditsProcedure(string creditExpression, bool testCreditExpression)
        {
            if (testCreditExpression)
            {
                try
                {
                    DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [credits] = {1} WHERE [uid] = 0",
                        BaseConfigs.GetTablePrefix, creditExpression));
                }
                catch
                {
                    return false;
                }
            }
            string sql = string.Format(@"IF OBJECT_ID('{0}updateusercredits','P') IS NOT NULL
                            DROP PROC [{0}updateusercredits]", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
            sql = string.Format(@"CREATE PROCEDURE [{0}updateusercredits]
    @uid INT
    AS
    UPDATE [{0}users] SET [credits] = {1} WHERE [uid] = @uid", BaseConfigs.GetTablePrefix, creditExpression);
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
            return true;
        }

        /// <summary>
        /// 通过email获取用户列表
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public IDataReader GetUserListByEmail(string email)
        {
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getuserlistbyemail", BaseConfigs.GetTablePrefix), DbHelper.MakeInParam("@email", (DbType)SqlDbType.NVarChar, 50, email));
        }

        public DataTable GetUserInfoByEmail(string email)
        {
            return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getuserinfobyemail", BaseConfigs.GetTablePrefix), DbHelper.MakeInParam("@email", (DbType)SqlDbType.NVarChar, 50, email)).Tables[0];
        }

        //public DataTable GetUserInfoByEmail(string email)
        //{
        //    return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("SELECT [username],[email] FROM [{0}users] WHERE [email]=@email", BaseConfigs.GetTablePrefix),DbHelper.MakeInParam("@email", (DbType)SqlDbType.NVarChar, 50, email)).Tables[0];
        //}

        /// <summary>
        /// 论坛每日信息统计
        /// </summary>
        /// <param name="trendType">统计项名称</param>
        public void UpdateTrendStat(TrendType trendType)
        {
            lock (stat)
            {
                stat[trendType]++;
                //当距上次更新时间时差五分钟再次更新
                if (lastTrendUpdateTime.AddMinutes(5) <= DateTime.Now)
                {
                    DbParameter[] parms = { 
                                     DbHelper.MakeInParam("@daytime",(DbType)SqlDbType.Int,4,DateTime.Now.ToString("yyyyMMdd")),
                                     DbHelper.MakeInParam("@login",(DbType)SqlDbType.Int,4, stat[TrendType.Login]),
                                     DbHelper.MakeInParam("@register",(DbType)SqlDbType.Int,4, stat[TrendType.Register]),
                                     DbHelper.MakeInParam("@topic",(DbType)SqlDbType.Int,4, stat[TrendType.Topic]),
                                     DbHelper.MakeInParam("@post",(DbType)SqlDbType.Int,4, stat[TrendType.Post]),
                                     DbHelper.MakeInParam("@poll",(DbType)SqlDbType.Int,4, stat[TrendType.Poll]),
                                     DbHelper.MakeInParam("@bonus",(DbType)SqlDbType.Int,4, stat[TrendType.Bonus]),
                                     DbHelper.MakeInParam("@debate",(DbType)SqlDbType.Int,4, stat[TrendType.Debate])
                                  };
                    DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updatetrendstat", BaseConfigs.GetTablePrefix), parms);
                    Initial();
                    //将当前时间设置为最后更新时间
                    lastTrendUpdateTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 更新用户积分
        /// </summary>
        /// <param name="credits">积分</param>
        /// <param name="startuid">更新的用户uid起始值</param>
        public int UpdateUserCredits(string formula, int startuid)
        {
            return DbHelper.ExecuteNonQuery(string.Format("UPDATE [{0}users] SET [credits]={1} WHERE [uid] IN (Select TOP 100 [uid] FROM [{0}users] WHERE [uid] > {2} ORDER BY [uid] ASC)", BaseConfigs.GetTablePrefix, formula, startuid));
        }

    }
}
