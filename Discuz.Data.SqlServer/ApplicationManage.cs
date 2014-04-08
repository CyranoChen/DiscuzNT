using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Entity;
using System.Data;
using Discuz.Config;
using System.Data.Common;
using Discuz.Common;

namespace Discuz.Data.SqlServer
{
    public partial class DataProvider : IDataProvider
    {
        public int AddUserApplication(UserApplicationInfo userAppInfo)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@appid", (DbType)SqlDbType.Int, 4, userAppInfo.AppId),
                                        DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userAppInfo.Uid),
                                        DbHelper.MakeInParam("@appname", (DbType)SqlDbType.NChar, 30, userAppInfo.AppName),
                                        DbHelper.MakeInParam("@privacy", (DbType)SqlDbType.TinyInt, 2, (int)userAppInfo.Privacy),
                                        DbHelper.MakeInParam("@allowsidenav", (DbType)SqlDbType.TinyInt, 2, userAppInfo.AllowSideNav),
                                        DbHelper.MakeInParam("@allowfeed", (DbType)SqlDbType.TinyInt, 2, userAppInfo.AllowFeed),
                                        DbHelper.MakeInParam("@allowprofilelink", (DbType)SqlDbType.TinyInt, 2, userAppInfo.AllowProfileLink),
                                        DbHelper.MakeInParam("@narrow", (DbType)SqlDbType.TinyInt, 2, userAppInfo.Narrow),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.TinyInt, 2, userAppInfo.DisplayOrder),
                                        DbHelper.MakeInParam("@menuorder", (DbType)SqlDbType.TinyInt, 2, userAppInfo.MenuOrder),
                                        DbHelper.MakeInParam("@profilelink", (DbType)SqlDbType.Text, 2000, userAppInfo.ProfileLink),
                                        DbHelper.MakeInParam("@myml", (DbType)SqlDbType.Text, 2000, userAppInfo.MYML)
                                    };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}adduserapp", BaseConfigs.GetTablePrefix), parms);
        }

        public int UpdateUserApplication(UserApplicationInfo userAppInfo)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@appid", (DbType)SqlDbType.Int, 4, userAppInfo.AppId),
                                        DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userAppInfo.Uid),
                                        DbHelper.MakeInParam("@appname", (DbType)SqlDbType.NChar, 30, userAppInfo.AppName),
                                        DbHelper.MakeInParam("@privacy", (DbType)SqlDbType.TinyInt, 2, (int)userAppInfo.Privacy),
                                        DbHelper.MakeInParam("@allowsidenav", (DbType)SqlDbType.TinyInt, 2, userAppInfo.AllowSideNav),
                                        DbHelper.MakeInParam("@allowfeed", (DbType)SqlDbType.TinyInt, 2, userAppInfo.AllowFeed),
                                        DbHelper.MakeInParam("@allowprofilelink", (DbType)SqlDbType.TinyInt, 2, userAppInfo.AllowProfileLink),
                                        DbHelper.MakeInParam("@narrow", (DbType)SqlDbType.TinyInt, 2, userAppInfo.Narrow),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.TinyInt, 2, userAppInfo.DisplayOrder),
                                        DbHelper.MakeInParam("@menuorder", (DbType)SqlDbType.TinyInt, 2, userAppInfo.MenuOrder),
                                        DbHelper.MakeInParam("@profilelink", (DbType)SqlDbType.Text, 2000, userAppInfo.ProfileLink),
                                        DbHelper.MakeInParam("@myml", (DbType)SqlDbType.Text, 2000, userAppInfo.MYML)
                                    };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateuserapp", BaseConfigs.GetTablePrefix), parms);
        }

        public int RemoveUserApplication(int uid, string appIds)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int, 4,uid),
                                        DbHelper.MakeInParam("@appids",(DbType)SqlDbType.NVarChar, 1000,appIds)
                                  };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}removeuserapp", BaseConfigs.GetTablePrefix), parms));

        }

        public IDataReader GetUserApplicationInfo(int uid, string appIdList)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int, 4,uid),
                                        DbHelper.MakeInParam("@appidlist",(DbType)SqlDbType.NVarChar, 1000,appIdList)
                                  };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getuserappinfo", BaseConfigs.GetTablePrefix), parms);
        }

        public int SendApplicationInvite(UserApplicationInviteInfo userAppInviteInfo)
        {
            DbParameter[] parms = {
                                      DbHelper.MakeInParam("@typename",(DbType)SqlDbType.VarChar, 100,userAppInviteInfo.TypeName),
                                      DbHelper.MakeInParam("@appid",(DbType)SqlDbType.Int, 4,userAppInviteInfo.AppId),
                                      DbHelper.MakeInParam("@type",(DbType)SqlDbType.TinyInt, 1,userAppInviteInfo.Type),
                                      DbHelper.MakeInParam("@fromuid",(DbType)SqlDbType.Int, 4,userAppInviteInfo.FromUid),
                                      DbHelper.MakeInParam("@touid",(DbType)SqlDbType.Int, 4,userAppInviteInfo.ToUid),
                                      DbHelper.MakeInParam("@myml",(DbType)SqlDbType.Text, 2000,userAppInviteInfo.MYML),
                                      DbHelper.MakeInParam("@hash",(DbType)SqlDbType.Int, 4,userAppInviteInfo.Hash)
                                  };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}sendmyinvite", BaseConfigs.GetTablePrefix), parms));
        }

        public IDataReader GetApplicationInviteList(int uid, int pageIndex, int pageSize)
        {
            DbParameter[] parms = {
                                      DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int, 4,uid),
                                      DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int, 4,pageIndex),
                                      DbHelper.MakeInParam("@pagesize",(DbType)SqlDbType.Int, 4,pageSize)
                                  };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getappinvitelist", BaseConfigs.GetTablePrefix), parms);
        }

        public int GetApplicationInviteCount(int uid)
        {
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}getappinvitecount", BaseConfigs.GetTablePrefix), DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)));
        }

        public int AddUserLog(int uid, string action)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int, 4,uid),
                                        DbHelper.MakeInParam("@action",(DbType)SqlDbType.Char, 10,action)
                                  };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}adduserlog", BaseConfigs.GetTablePrefix), parms));
        }

        public IDataReader GetUserLog(int count)
        {
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getuserlog", BaseConfigs.GetTablePrefix), DbHelper.MakeInParam("@count", (DbType)SqlDbType.Int, 4, count));
        }

        public int IgnoreApplicationInvite(string idList)
        {
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}ignoremyinvite", BaseConfigs.GetTablePrefix), DbHelper.MakeInParam("@idlist", (DbType)SqlDbType.NVarChar, 1000, idList)));
        }

        public int DeleteApplicationInviteByAppId(int uid, int appId)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int, 4,uid),
                                        DbHelper.MakeInParam("@appid",(DbType)SqlDbType.Int, 4,appId)
                                  };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}deletemyinvitebyappid", BaseConfigs.GetTablePrefix), parms);
        }

        public IDataReader GetUserInstalledApplication(int uid, int pageIndex, int pageSize)
        {
            DbParameter[] parms = {
                                      DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int, 4,uid),
                                      DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int, 4,pageIndex),
                                      DbHelper.MakeInParam("@pagesize",(DbType)SqlDbType.Int, 4,pageSize)
                                  };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getuserinstalledapp", BaseConfigs.GetTablePrefix), parms);
        }

        public int UpdateApplicationInfo(ManyouApplicationInfo appInfo)
        {
            DbParameter[] parms = {
                                      DbHelper.MakeInParam("@appid",(DbType)SqlDbType.Int, 4,appInfo.AppId),
                                      DbHelper.MakeInParam("@appname",(DbType)SqlDbType.Char,20,appInfo.AppName),
                                      DbHelper.MakeInParam("@version",(DbType)SqlDbType.Int, 4,appInfo.Version),
                                      DbHelper.MakeInParam("@displaymethod",(DbType)SqlDbType.Int, 4,(int)appInfo.DisplayMethod),
                                      DbHelper.MakeInParam("@displayorder",(DbType)SqlDbType.Int, 4,appInfo.DisplayOrder),
                                  };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateapplication", BaseConfigs.GetTablePrefix), parms);
        }

        public int SetApplicationFlag(string appIdList, string appNameList, int flag)
        {
            DbParameter[] parms = {
                                      DbHelper.MakeInParam("@appidlist",(DbType)SqlDbType.NVarChar,1000,appIdList),
                                      DbHelper.MakeInParam("@appnamelist",(DbType)SqlDbType.NVarChar, 2000,appNameList),
                                      DbHelper.MakeInParam("@flag",(DbType)SqlDbType.TinyInt, 4,flag)
                                  };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}setapplicationflag", BaseConfigs.GetTablePrefix), parms);
        }

        public int RemoveApplication(string appIdList)
        {
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}removeapplication", BaseConfigs.GetTablePrefix), DbHelper.MakeInParam("@appidlist", (DbType)SqlDbType.NVarChar, 1000, appIdList));
        }

        public IDataReader GetUserInstalledApplicationId(int uid)
        {
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getuserinstalledappid", BaseConfigs.GetTablePrefix), DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid));
        }

        public IDataReader GetApplicationInfo(int appId)
        {
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getapplicationinfo", BaseConfigs.GetTablePrefix), DbHelper.MakeInParam("@appid", (DbType)SqlDbType.Int, 4, appId));
        }
    }
}
