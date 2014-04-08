using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using Discuz.Data;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;

using System.Text.RegularExpressions;
using SQLDMO;
using System.Collections.Generic;

namespace Discuz.Data.SqlServer
{
    public partial class DataProvider : IDataProvider
    {
        private string GetSqlstringByPostDatetime(string commandText, DateTime postdatetimeStart, DateTime postdatetimeEnd)
        {
            //日期需要改成参数，以后需要重构！需要先修改后台传递参数方式
            if (!Utils.StrIsNullOrEmpty(postdatetimeStart.ToString()))
                commandText += string.Format(" AND [postdatetime]>='{0}'", postdatetimeStart.ToString("yyyy-MM-dd HH:mm:ss"));

            if (!Utils.StrIsNullOrEmpty(postdatetimeEnd.ToString()))
                commandText += string.Format(" AND [postdatetime]<='{0}'", postdatetimeEnd.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"));

            return commandText;
        }


        public DataTable GetAdsTable()
        {
            string commandText = string.Format("SELECT [advid], [type], [displayorder], [targets], [parameters], [code] FROM [{0}advertisements] WHERE [available]=1 AND [starttime] <='{1}' AND [endtime] >='{1}' ORDER BY [displayorder], [advid] DESC",
                                                BaseConfigs.GetTablePrefix,
                                                DateTime.Now.ToString("yyyy-MM-dd"));
            return DbHelper.ExecuteDatasetInMasterDB(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 获得全部指定时间段内的前n条公告列表
        /// </summary>
        /// <param name="maxCount">最大记录数,小于0返回全部</param>
        /// <returns>公告列表</returns>
        public DataTable GetAnnouncementList(int maxCount)
        {
            DbParameter param = DbHelper.MakeInParam("@now", (DbType)SqlDbType.DateTime, 8, DateTime.Now);
            string commandText = string.Format("SELECT {0} {1} FROM [{2}announcements] WHERE [starttime] <=@now AND [endtime] >=@now ORDER BY [displayorder], [id] DESC",
                                              maxCount < 0 ? "" : " TOP " + maxCount,
                                              DbFields.ANNOUNCEMENTS,
                                              BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDatasetInMasterDB(CommandType.Text, commandText, param).Tables[0];
        }


        public int CreateAnnouncement(AnnouncementInfo announcementInfo)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@poster", (DbType)SqlDbType.NVarChar, 20, announcementInfo.Poster),
                                        DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.Int, 4, announcementInfo.Posterid),
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 250, announcementInfo.Title),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, announcementInfo.Displayorder),
                                        DbHelper.MakeInParam("@starttime", (DbType)SqlDbType.DateTime, 8, announcementInfo.Starttime),
                                        DbHelper.MakeInParam("@endtime", (DbType)SqlDbType.DateTime, 8, announcementInfo.Endtime),
                                        DbHelper.MakeInParam("@message", (DbType)SqlDbType.NText, 0, announcementInfo.Message)
                                    };
            string commandText = string.Format("INSERT INTO [{0}announcements] ([poster],[posterid],[title],[displayorder],[starttime],[endtime],[message]) VALUES(@poster, @posterid, @title, @displayorder, @starttime, @endtime, @message)", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public DataTable GetAnnouncements()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}announcements] ORDER BY [displayorder] DESC,[id] DESC",
                                                DbFields.ANNOUNCEMENTS,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDatasetInMasterDB(CommandType.Text, commandText).Tables[0];
        }

        public DataTable GetAnnouncements(int num, int pageId)
        {
            string commandText = string.Format("SELECT TOP {0} {1}  FROM [{2}announcements]  WHERE [id] NOT IN (SELECT TOP {3} [id] FROM [{2}announcements] ORDER BY [displayorder], [starttime] DESC, [id] DESC) ORDER BY [displayorder], [starttime] DESC, [id] DESC",
                                               num,
                                               DbFields.ANNOUNCEMENTS,
                                               BaseConfigs.GetTablePrefix,
                                               (pageId - 1) * num);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public int DeleteAnnouncements(string idList)
        {
            if (!Utils.IsNumericList(idList))
                return 0;

            string commandText = string.Format("DELETE FROM [{0}announcements] WHERE [id] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                idList);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public IDataReader GetAnnouncement(int id)
        {
            DbParameter param = DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id);
            string commandText = string.Format("SELECT {0} FROM [{1}announcements] WHERE [id]=@id",
                                                DbFields.ANNOUNCEMENTS,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText, param);
        }

        public int UpdateAnnouncement(AnnouncementInfo announcementInfo)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, announcementInfo.Id),
                                        DbHelper.MakeInParam("@poster", (DbType)SqlDbType.NVarChar, 20, announcementInfo.Poster),
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 250, announcementInfo.Title),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, announcementInfo.Displayorder),
                                        DbHelper.MakeInParam("@starttime", (DbType)SqlDbType.DateTime, 8, announcementInfo.Starttime),
                                        DbHelper.MakeInParam("@endtime", (DbType)SqlDbType.DateTime, 8, announcementInfo.Endtime),
                                        DbHelper.MakeInParam("@message", (DbType)SqlDbType.NText, 0, announcementInfo.Message)
                                    };
            string commandText = string.Format("UPDATE [{0}announcements] SET [displayorder]=@displayorder,[title]=@title, [poster]=@poster,[starttime]=@starttime,[endtime]=@endtime,[message]=@message WHERE [id]=@id",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得公共可见板块列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetVisibleForumList()
        {
            string commandText = string.Format("SELECT [name], [fid], [layer],[parentid] FROM [{0}forums] WHERE [parentid] NOT IN (SELECT fid FROM [{0}forums] WHERE [status] < 1 AND [layer] = 0) AND [status] > 0 AND [displayorder] >=0 ORDER BY [displayorder]",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDatasetInMasterDB(CommandType.Text, commandText).Tables[0];
        }

        public IDataReader GetOnlineGroupIconList()
        {
            string commandText = string.Format("SELECT [title], [img] FROM [{0}onlinelist] WHERE [img]<> '' ORDER BY [displayorder]",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReaderInMasterDB(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获得友情链接列表
        /// </summary>
        /// <returns>友情链接列表</returns>
        public DataTable GetForumLinkList()
        {
            string commandText = string.Format(@"SELECT [name],[url],[note],[displayorder]+10000 AS [displayorder],[logo] FROM [{0}forumlinks] WHERE [displayorder] > 0 AND [logo] = '' 
																	UNION SELECT [name],[url],[note],[displayorder],[logo] FROM [{0}forumlinks] WHERE [displayorder] > 0 AND [logo] <> '' ORDER BY [displayorder]",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDatasetInMasterDB(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 获得脏字过滤列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetBanWordList()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}words]", DbFields.WORDS, BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDatasetInMasterDB(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 获得勋章列表
        /// </summary>
        /// <returns>获得勋章列表</returns>
        public DataTable GetMedalsList()
        {
            string commandText = string.Format("SELECT [medalid], [name], [image],[available]  FROM [{0}medals] ORDER BY [medalid]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDatasetInMasterDB(CommandType.Text, commandText).Tables[0];
        }


        /// <summary>
        /// 获得主题类型列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTopicTypeList()
        {
            string commandText = string.Format("SELECT [typeid],[name] FROM [{0}topictypes] ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDatasetInMasterDB(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 添加积分转帐兑换记录
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="fromto">来自/到</param>
        /// <param name="sendcredits">付出积分类型</param>
        /// <param name="receivecredits">得到积分类型</param>
        /// <param name="send">付出积分数额</param>
        /// <param name="receive">得到积分数额</param>
        /// <param name="paydate">时间</param>
        /// <param name="operation">积分操作(1=兑换, 2=转帐)</param>
        /// <returns>执行影响的行</returns>
        public int AddCreditsLog(int uid, int fromTo, int sendCredits, int receiveCredits, float send, float receive, string payDate, int operation)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									   DbHelper.MakeInParam("@fromto",(DbType)SqlDbType.Int,4,fromTo),
									   DbHelper.MakeInParam("@sendcredits",(DbType)SqlDbType.Int,4,sendCredits),
									   DbHelper.MakeInParam("@receivecredits",(DbType)SqlDbType.Int,4,receiveCredits),
									   DbHelper.MakeInParam("@send",(DbType)SqlDbType.Float,4,send),
									   DbHelper.MakeInParam("@receive",(DbType)SqlDbType.Decimal,4,receive),    //使用Float会造成小数点后位数过长，导致显示不友好。所以在此使用Decimal
									   DbHelper.MakeInParam("@paydate",(DbType)SqlDbType.VarChar,0,payDate),
									   DbHelper.MakeInParam("@operation",(DbType)SqlDbType.Int,4,operation)
								   };
            string commandText = string.Format("INSERT INTO [{0}creditslog] ([uid],[fromto],[sendcredits],[receivecredits],[send],[receive],[paydate],[operation]) VALUES(@uid,@fromto,@sendcredits,@receivecredits,@send,@receive,@paydate,@operation)",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 返回指定范围的积分日志
        /// </summary>
        /// <param name="pagesize">页大小</param>
        /// <param name="currentpage">当前页数</param>
        /// <param name="uid">用户id</param>
        /// <returns>积分日志</returns>
        public DataTable GetCreditsLogList(int pageSize, int currentPage, int uid)
        {
            int pagetop = (currentPage - 1) * pageSize;
            string commandText;
            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} {3}, [ufrom].[username] AS [fromuser], [uto].[username] AS [touser] FROM [{1}creditslog] AS [cl], [{1}users] AS [ufrom], [{1}users] AS [uto] WHERE [cl].[uid]=[ufrom].[uid] AND [cl].[fromto]=[uto].[uid] AND ([cl].[uid]={2} OR [cl].[fromto] = {2})  ORDER BY [id] DESC",
                                             pageSize,
                                             BaseConfigs.GetTablePrefix,
                                             uid,
                                             DbFields.CREDITS_LOG_JOIN);
            else
                commandText = string.Format("SELECT TOP {0} {4}, [ufrom].[username] AS [fromuser], [uto].[username] AS [touser] FROM [{1}creditslog] AS [cl], [{1}users] AS [ufrom], [{1}users] AS [uto] WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP {2} [id] FROM [{1}creditslog] WHERE [{1}creditslog].[uid]={3}  OR [{1}creditslog].[fromto]={3} ORDER BY [id] DESC) AS tblTmp ) AND [cl].[uid]=[ufrom].[uid] AND [cl].[fromto]=[uto].[uid] AND ([cl].[uid]={3} OR [cl].[fromto] = {3}) ORDER BY [cl].[id] DESC",
                                             pageSize,
                                             BaseConfigs.GetTablePrefix,
                                             pagetop,
                                             uid,
                                             DbFields.CREDITS_LOG_JOIN);

            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 获得指定用户的积分交易历史记录总条数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>历史记录总条数</returns>
        public int GetCreditsLogRecordCount(int uid)
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}creditslog] WHERE [uid]={1} OR [fromto]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                uid);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText));
        }


        public void ShrinkDataBase(string shrinkSize, string dbName)
        {
            StringBuilder sqlBuilder = new StringBuilder("SET NOCOUNT ON ");

            sqlBuilder.Append("DECLARE @LogicalFileName sysname, @MaxMinutes INT, @NewSize INT ");
            sqlBuilder.AppendFormat("USE [{0}] -- 要操作的数据库名 ", dbName);
            sqlBuilder.AppendFormat("SELECT @LogicalFileName = '{0}_log', -- 日志文件名 ", dbName);
            sqlBuilder.Append("@MaxMinutes = 10, -- Limit on time allowed to wrap log. ");
            sqlBuilder.Append("@NewSize = 1 -- 你想设定的日志文件的大小(M) ");
            sqlBuilder.Append("-- Setup / initialize ");
            sqlBuilder.Append("DECLARE @OriginalSize int ");
            sqlBuilder.AppendFormat("SELECT @OriginalSize = {0}", shrinkSize);
            sqlBuilder.Append("FROM sysfiles ");
            sqlBuilder.Append("WHERE name = @LogicalFileName ");
            sqlBuilder.Append("SELECT 'Original Size of ' + db_name() + ' LOG is ' + ");
            sqlBuilder.Append("CONVERT(VARCHAR(30),@OriginalSize) + ' 8K pages or ' + ");
            sqlBuilder.Append("CONVERT(VARCHAR(30),(@OriginalSize*8/1024)) + 'MB' ");
            sqlBuilder.Append("FROM sysfiles ");
            sqlBuilder.Append("WHERE name = @LogicalFileName ");
            sqlBuilder.Append("CREATE TABLE DummyTrans ");
            sqlBuilder.Append("(DummyColumn char (8000) not null) ");
            sqlBuilder.Append("DECLARE @Counter INT, ");
            sqlBuilder.Append("@StartTime DATETIME, ");
            sqlBuilder.Append("@TruncLog VARCHAR(255) ");
            sqlBuilder.Append("SELECT @StartTime = GETDATE(), ");
            sqlBuilder.Append("@TruncLog = 'BACKUP LOG ' + db_name() + ' WITH TRUNCATE_ONLY' ");
            sqlBuilder.Append("DBCC SHRINKFILE (@LogicalFileName, @NewSize) ");
            sqlBuilder.Append("EXEC (@TruncLog) ");
            sqlBuilder.Append("-- Wrap the log if necessary. ");
            sqlBuilder.Append("WHILE @MaxMinutes > DATEDIFF (mi, @StartTime, GETDATE()) -- time has not expired ");
            sqlBuilder.Append("AND @OriginalSize = (SELECT size FROM sysfiles WHERE name = @LogicalFileName) ");
            sqlBuilder.Append("AND (@OriginalSize * 8 /1024) > @NewSize ");
            sqlBuilder.Append("BEGIN -- Outer loop. ");
            sqlBuilder.Append("SELECT @Counter = 0 ");
            sqlBuilder.Append("WHILE ((@Counter < @OriginalSize / 16) AND (@Counter < 50000)) ");
            sqlBuilder.Append("BEGIN -- update ");
            sqlBuilder.Append("INSERT DummyTrans VALUES ('Fill Log') ");
            sqlBuilder.Append("DELETE DummyTrans ");
            sqlBuilder.Append("SELECT @Counter = @Counter + 1 ");
            sqlBuilder.Append("END ");
            sqlBuilder.Append("EXEC (@TruncLog) ");
            sqlBuilder.Append("END ");
            sqlBuilder.Append("SELECT 'Final Size of ' + db_name() + ' LOG is ' + ");
            sqlBuilder.Append("CONVERT(VARCHAR(30),size) + ' 8K pages or ' + ");
            sqlBuilder.Append("CONVERT(VARCHAR(30),(size*8/1024)) + 'MB' ");
            sqlBuilder.Append("FROM sysfiles ");
            sqlBuilder.Append("WHERE name = @LogicalFileName ");
            sqlBuilder.Append("DROP TABLE DummyTrans ");
            sqlBuilder.Append("SET NOCOUNT OFF ");

            DbHelper.ExecuteDataset(CommandType.Text, sqlBuilder.ToString());
        }

        public void ClearDBLog(string dbName)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@DBName", (DbType)SqlDbType.VarChar, 50, dbName),
			};
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}shrinklog", BaseConfigs.GetTablePrefix), parms);
        }

        public string RunSql(string sql)
        {
            string errorInfo = "";
            if (sql != "")
            {
                SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
                conn.Open();
                string[] sqlArray = Utils.SplitString(sql, "--/* Discuz!NT SQL Separator */--");
                foreach (string sqlStr in sqlArray)
                {
                    if (Utils.StrIsNullOrEmpty(sqlStr))   //当读到空的Sql语句则继续
                    {
                        continue;
                    }
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            DbHelper.ExecuteNonQuery(CommandType.Text, sqlStr);
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            string message = ex.Message.Replace("'", " ");
                            message = message.Replace("\\", "/");
                            message = message.Replace("\r\n", "\\r\\n");
                            message = message.Replace("\r", "\\r");
                            message = message.Replace("\n", "\\n");
                            errorInfo += message + "<br />";
                        }
                    }
                }
                conn.Close();
            }
            return errorInfo;
        }

        public string GetDataBaseVersion()
        {
            return DbHelper.ExecuteScalar(CommandType.Text, "SELECT @@version").ToString();
        }

        //得到数据库的名称
        public string GetDbName()
        {
            foreach (string info in BaseConfigs.GetDBConnectString.Split(';'))
            {
                if (info.ToLower().IndexOf("initial catalog") >= 0 || info.ToLower().IndexOf("database") >= 0)
                    return info.Split('=')[1].Trim();
            }
            return "dnt";
        }


        /// <summary>
        /// 创建并填充指定帖子分表id全文索引
        /// </summary>
        /// <param name="DbName">数据库名称</param>
        /// <param name="postsid">当前帖子表的id</param>
        /// <returns></returns>
        public bool CreateORFillIndex(string dbName, string postsId)
        {
            StringBuilder sb;
            string currenttablename = BaseConfigs.GetTablePrefix + "posts" + postsId;

            try
            {
                string commandText = string.Format("SELECT TOP 1 [pid] FROM [{0}] WHERE CONTAINS([message],'asd') ORDER BY [pid] ASC", currenttablename);
                DbHelper.ExecuteNonQuery(commandText);

                //如果有全文索引则进行填充,如果没有就抛出异常
                sb = new StringBuilder("EXECUTE sp_fulltext_catalog 'pk_{0}_msg','start_full'; \r\n");
                sb.Append("WHILE fulltextcatalogproperty('pk_{0}_msg','populateStatus')<>0 \r\n");
                sb.Append("BEGIN \r\n");
                sb.Append("WAITFOR DELAY '0:5:30' \r\n");
                sb.Append("END \r\n");
                DbHelper.ExecuteNonQuery(string.Format(sb.ToString(), currenttablename));
                return true;
            }
            catch
            {
                try
                {
                    #region 构建全文索引

                    sb = new StringBuilder();
                    sb.AppendFormat("IF(SELECT DATABASEPROPERTY('[{0}]','isfulltextenabled'))=0  EXECUTE sp_fulltext_database 'enable';", dbName);

                    try
                    { //此处删除以确保全文索引目录和系统表中的数据同步
                        sb.Append("EXECUTE sp_fulltext_table '[{0}]', 'drop' ;");
                        sb.Append("EXECUTE sp_fulltext_catalog 'pk_{0}_msg','drop';");
                        DbHelper.ExecuteNonQuery(sb.ToString());
                    }
                    catch { ;}
                    finally
                    {
                        //执行全文填充语句
                        sb = new StringBuilder("EXECUTE sp_fulltext_catalog 'pk_{0}_msg','create';");
                        sb.Append("EXECUTE sp_fulltext_table '[{0}]','create','pk_{0}_msg','pk_{0}';");
                        sb.Append("EXECUTE sp_fulltext_column '[{0}]','message','add';");
                        sb.Append("EXECUTE sp_fulltext_table '[{0}]','activate';");
                        sb.Append("EXECUTE sp_fulltext_catalog 'pk_{0}_msg','start_full';");
                        DbHelper.ExecuteNonQuery(string.Format(sb.ToString(), currenttablename));
                    }
                    return true;

                    #endregion
                }
                catch (SqlException ex)
                {
                    string message = ex.Message.Replace("'", " ").Replace("\\", "/").Replace("\r\n", "\\r\\n").Replace("\r", "\\r").Replace("\n", "\\n");
                    return true;
                }
            }
        }


        /// <summary>
        /// 以DataReader返回自定义编辑器按钮列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetCustomEditButtonList()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}bbcodes] WHERE [available] = 1", DbFields.BBCODES, BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReaderInMasterDB(CommandType.Text, commandText);
        }


        public void UpdateAnnouncementPoster(int posterId, string poster)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.Int, 4, posterId),
                                        DbHelper.MakeInParam("@poster", (DbType)SqlDbType.VarChar, 20, poster)
                                    };
            string commandText = string.Format("UPDATE [{0}announcements] SET [poster]=@poster WHERE [posterid]=@posterid",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public DataTable GetAnnouncementsByCondition(string condition)
        {
            string commandText = string.Format("SELECT {0} FROM [{1}announcements] {2}",
                                                DbFields.ANNOUNCEMENTS,
                                                BaseConfigs.GetTablePrefix,
                                                condition);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }


        public int UpdateStatisticsLastUserName(int lastUserId, string lastUserName)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@lastuserid", (DbType)SqlDbType.Int, 4, lastUserId),
                                        DbHelper.MakeInParam("@lastusername", (DbType)SqlDbType.VarChar, 20, lastUserName)
                                    };
            string commandText = string.Format("UPDATE [{0}statistics] SET [lastusername]=@lastusername WHERE [lastuserid]=@lastuserid",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void AddVisitLog(int uid, string userName, int groupId, string groupTitle, string ip, string actions, string others)
        {
            DbParameter[] parms = {
					DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
					DbHelper.MakeInParam("@username", (DbType)SqlDbType.VarChar, 50, userName),
					DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupId),
					DbHelper.MakeInParam("@grouptitle", (DbType)SqlDbType.VarChar, 50, groupTitle),
					DbHelper.MakeInParam("@ip", (DbType)SqlDbType.VarChar, 15, ip),
					DbHelper.MakeInParam("@actions", (DbType)SqlDbType.VarChar, 100, actions),
					DbHelper.MakeInParam("@others", (DbType)SqlDbType.VarChar, 200, others)
				};
            string commandText = string.Format("INSERT INTO [{0}adminvisitlog] ([uid],[username],[groupid],[grouptitle],[ip],[actions],[others]) VALUES (@uid,@username,@groupid,@grouptitle,@ip,@actions,@others)",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void DeleteVisitLogs()
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}adminvisitlog]", BaseConfigs.GetTablePrefix));
        }

        public void DeleteVisitLogs(string condition)
        {
            string commandText = string.Format("DELETE FROM [{0}adminvisitlog] WHERE {1}", BaseConfigs.GetTablePrefix, condition);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 得到当前指定页数的后台访问日志记录(表)
        /// </summary>
        /// <param name="pagesize">当前分页的尺寸大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <returns></returns>
        public DataTable GetVisitLogList(int pageSize, int currentPage)
        {
            int pagetop = (currentPage - 1) * pageSize;
            string commandText;

            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} {1} FROM [{2}adminvisitlog] ORDER BY [visitid] DESC",
                                             pageSize,
                                             DbFields.ADMIN_VISIT_LOG,
                                             BaseConfigs.GetTablePrefix);
            else
                commandText = string.Format("SELECT TOP {0} {1} FROM [{2}adminvisitlog]  WHERE [visitid] < (SELECT MIN([visitid]) FROM (SELECT TOP {3} [visitid] FROM [{2}adminvisitlog] ORDER BY [visitid] DESC) AS tblTmp )  ORDER BY [visitid] DESC",
                                             pageSize,
                                             DbFields.ADMIN_VISIT_LOG,
                                             BaseConfigs.GetTablePrefix,
                                             pagetop);

            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 得到当前指定条件和页数的后台访问日志记录(表)
        /// </summary>
        /// <param name="pagesize">当前分页的尺寸大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public DataTable GetVisitLogList(int pageSize, int currentPage, string condition)
        {
            int pagetop = (currentPage - 1) * pageSize;
            string commandText;

            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} {1} FROM [{2}adminvisitlog] WHERE {3} ORDER BY [visitid] DESC",
                                             pageSize,
                                             DbFields.ADMIN_VISIT_LOG,
                                             BaseConfigs.GetTablePrefix,
                                             condition);
            else
                commandText = string.Format("SELECT TOP {0} {1} FROM [{2}adminvisitlog]  WHERE [visitid] < (SELECT MIN([visitid])  FROM (SELECT TOP {3} [visitid] FROM [{2}adminvisitlog] WHERE {4} ORDER BY [visitid] DESC) AS tblTmp ) AND {4} ORDER BY [visitid] DESC",
                                             pageSize,
                                             DbFields.ADMIN_VISIT_LOG,
                                             BaseConfigs.GetTablePrefix,
                                             pagetop,
                                             condition);

            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public int GetVisitLogCount()
        {
            string commandText = string.Format("SELECT COUNT(visitid) FROM [{0}adminvisitlog]", BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows[0][0]);
        }

        public int GetVisitLogCount(string condition)
        {
            string commandText = string.Format("SELECT COUNT(visitid) FROM [{0}adminvisitlog] WHERE {1}", BaseConfigs.GetTablePrefix, condition);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows[0][0]);
        }

        public void UpdateForumAndUserTemplateId(string templateIdList)
        {
            string commandText = string.Format("UPDATE [{0}forums] SET [templateid]=0 WHERE [templateid] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                templateIdList);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);

            commandText = string.Format("UPDATE [{0}users] SET [templateid]=0 WHERE [templateid] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                templateIdList);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public void AddTemplate(string name, string directory, string copyRight, string author, string createDate, string ver, string forDntVer)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar, 50, name),
                                        DbHelper.MakeInParam("@directory", (DbType)SqlDbType.NVarChar, 100, directory),
                                        DbHelper.MakeInParam("@copyright", (DbType)SqlDbType.NVarChar, 100, copyRight),
                                        DbHelper.MakeInParam("@author", (DbType)SqlDbType.NVarChar, 100, author),
                                        DbHelper.MakeInParam("@createdate", (DbType)SqlDbType.NVarChar, 50, createDate),
                                        DbHelper.MakeInParam("@ver", (DbType)SqlDbType.NVarChar, 100, ver),
                                        DbHelper.MakeInParam("@fordntver", (DbType)SqlDbType.NVarChar, 100, forDntVer)
                                    };
            string commandText = string.Format("INSERT INTO [{0}templates] ([name],[directory],[copyright],[author],[createdate],[ver],[fordntver]) VALUES(@name,@directory,@copyright,@author,@createdate,@ver,@fordntver)",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 添加新的模板项
        /// </summary>
        /// <param name="templateName">模板名称</param>
        /// <param name="directory">模板文件所在目录</param>
        /// <param name="copyright">模板版权文字</param>
        /// <returns>模板id</returns>
        public int AddTemplate(string templateName, string directory, string copyRight)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@templatename", (DbType)SqlDbType.VarChar, 0, templateName),
				DbHelper.MakeInParam("@directory", (DbType)SqlDbType.VarChar, 0, directory),
				DbHelper.MakeInParam("@copyright", (DbType)SqlDbType.VarChar, 0, copyRight)
			};
            string commandText = string.Format("INSERT INTO [{0}templates]([templatename],[directory],[copyright]) VALUES(@templatename, @directory, @copyright);SELECT SCOPE_IDENTITY()",
                                                BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms), -1);
        }

        /// <summary>
        /// 删除指定的模板项
        /// </summary>
        /// <param name="templateid">模板id</param>
        public void DeleteTemplateItem(int templateId)
        {
            string commandText = string.Format("DELETE FROM [{0}templates] WHERE [templateid]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                templateId);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 删除指定的模板项列表,
        /// </summary>
        /// <param name="templateidlist">格式为： 1,2,3</param>
        public void DeleteTemplateItem(string templateIdList)
        {
            string commandText = string.Format("DELETE FROM [{0}templates] WHERE [templateid] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                templateIdList);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获得所有在模板目录下的模板列表(即:子目录名称)
        /// </summary>
        /// <param name="templatePath">模板所在路径</param>
        /// <example>GetAllTemplateList(Utils.GetMapPath(@"..\..\templates\"))</example>
        /// <returns>模板列表</returns>
        public DataTable GetAllTemplateList()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}templates] ORDER BY [templateid]",
                                                DbFields.TEMPLATES,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }


        /// <summary>
        /// 插入版主管理日志记录
        /// </summary>
        /// <param name="moderatorname">版主名</param>
        /// <param name="grouptitle">所属组的ID</param>
        /// <param name="ip">客户端的IP</param>
        /// <param name="fname">版块的名称</param>
        /// <param name="title">主题的名称</param>
        /// <param name="actions">动作</param>
        /// <param name="reason">原因</param>
        /// <returns></returns>
        public bool InsertModeratorLog(string moderatorUid, string moderatorName, string groupId, string groupTitle, string ip, string postDateTime, string fid, string fname, string tid, string title, string actions, string reason)
        {
            try
            {
                DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@moderatoruid", (DbType)SqlDbType.Int, 4, moderatorUid),
                                        DbHelper.MakeInParam("@moderatorname", (DbType)SqlDbType.NVarChar, 50, moderatorName),
                                        DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupId),
                                        DbHelper.MakeInParam("@grouptitle", (DbType)SqlDbType.NVarChar, 50, groupTitle),
                                        DbHelper.MakeInParam("@ip", (DbType)SqlDbType.VarChar, 15, ip),
                                        DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(postDateTime)),
                                        DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid),
                                        DbHelper.MakeInParam("@fname", (DbType)SqlDbType.NVarChar, 100, fname),
                                        DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 8, tid),
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.VarChar, 200, title),
                                        DbHelper.MakeInParam("@actions", (DbType)SqlDbType.VarChar, 50, actions),
                                        DbHelper.MakeInParam("@reason", (DbType)SqlDbType.NVarChar, 200, reason)
                                    };
                string commandText = string.Format("INSERT INTO [{0}moderatormanagelog] ([moderatoruid],[moderatorname],[groupid],[grouptitle],[ip],[postdatetime],[fid],[fname],[tid],[title],[actions],[reason]) VALUES (@moderatoruid, @moderatorname, @groupid, @grouptitle,@ip,@postdatetime,@fid,@fname,@tid,@title,@actions,@reason)",
                                                    BaseConfigs.GetTablePrefix);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
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
        public bool DeleteModeratorLog(string condition)
        {
            try
            {
                string commandText = string.Format("DELETE FROM [{0}moderatormanagelog] WHERE {1}", BaseConfigs.GetTablePrefix, condition);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 得到当前指定条件和页数的前台管理日志记录(表)
        /// </summary>
        /// <param name="pagesize">当前分页的尺寸大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public DataTable GetModeratorLogList(int pageSize, int currentPage, string condition)
        {
            int pagetop = (currentPage - 1) * pageSize;
            string commandText;

            if (condition != "")
                condition = " WHERE " + condition;

            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} {1}  FROM [{2}moderatormanagelog]  {3}  ORDER BY [id] DESC",
                                            pageSize,
                                            DbFields.MODERATOR_MANAGE_LOG,
                                            BaseConfigs.GetTablePrefix,
                                            condition);
            else
                commandText = string.Format("SELECT TOP {0} {1}  FROM [{2}moderatormanagelog]  WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP {3} [id] FROM [{2}moderatormanagelog]  {4} ORDER BY [id] DESC) AS tblTmp ) {5} ORDER BY [id] DESC",
                                            pageSize,
                                            DbFields.MODERATOR_MANAGE_LOG,
                                            BaseConfigs.GetTablePrefix,
                                            pagetop,
                                            condition,
                                            condition.Replace("WHERE", "") == "" ? "" : "AND " + condition.Replace("WHERE", ""));

            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 得到前台管理日志记录数
        /// </summary>
        /// <returns></returns>
        public int GetModeratorLogListCount()
        {
            string commandText = string.Format("SELECT COUNT([id]) FROM [{0}moderatormanagelog]", BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 得到指定查询条件下的前台管理日志数
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public int GetModeratorLogListCount(string condition)
        {
            string commandText = string.Format("SELECT COUNT([id]) FROM [{0}moderatormanagelog] WHERE {1}",
                                                BaseConfigs.GetTablePrefix,
                                                condition);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <returns></returns>
        public bool DeleteMedalLog()
        {
            try
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}medalslog]", BaseConfigs.GetTablePrefix));
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
        public bool DeleteMedalLog(string condition)
        {
            try
            {
                string commandText = string.Format("DELETE FROM [{0}medalslog] WHERE {1}", BaseConfigs.GetTablePrefix, condition);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 得到当前指定页数的勋章日志记录(表)
        /// </summary>
        /// <param name="pagesize">当前分页的尺寸大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <returns></returns>
        public DataTable GetMedalLogList(int pageSize, int currentPage)
        {
            int pagetop = (currentPage - 1) * pageSize;
            string commandText;

            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} {1} FROM [{2}medalslog] ORDER BY [id] DESC",
                                             pageSize,
                                             DbFields.MEDALS_LOG,
                                             BaseConfigs.GetTablePrefix);
            else
                commandText = string.Format("SELECT TOP {0} {1} FROM [{2}medalslog]  WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP {3} [id] FROM [{2}medalslog] ORDER BY [id] DESC) AS tblTmp )  ORDER BY [id] DESC",
                                             pageSize,
                                             DbFields.MEDALS_LOG,
                                             BaseConfigs.GetTablePrefix,
                                             pagetop);

            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 得到当前指定条件和页数的勋章日志记录(表)
        /// </summary>
        /// <param name="pagesize">当前分页的尺寸大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public DataTable GetMedalLogList(int pageSize, int currentPage, string condition)
        {
            int pagetop = (currentPage - 1) * pageSize;
            string commandText;

            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} {1} FROM [{2}medalslog] WHERE {3} ORDER BY [id] DESC",
                                              pageSize,
                                              DbFields.MEDALS_LOG,
                                              BaseConfigs.GetTablePrefix,
                                              condition);
            else
                commandText = string.Format("SELECT TOP {0} {1} FROM [{2}medalslog] WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP {3} [id] FROM [{2}medalslog] WHERE {4} ORDER BY [id] DESC) AS tblTmp ) AND {4} ORDER BY [id] DESC",
                                              pageSize,
                                              DbFields.MEDALS_LOG,
                                              BaseConfigs.GetTablePrefix,
                                              pagetop,
                                              condition);

            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 得到缓存日志记录数
        /// </summary>
        /// <returns></returns>
        public int GetMedalLogListCount()
        {
            string commandText = string.Format("SELECT COUNT([id]) FROM [{0}medalslog]", BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 得到指定查询条件下的勋章日志数
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public int GetMedalLogListCount(string condition)
        {
            string commandText = string.Format("SELECT COUNT([id]) FROM [{0}medalslog] WHERE {1}", BaseConfigs.GetTablePrefix, condition);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows[0][0]);
        }


        /// <summary>
        /// 根据IP获取错误登录记录
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public DataTable GetErrLoginRecordByIP(string ip)
        {
            DbParameter[] parms = {
										 DbHelper.MakeInParam("@ip",(DbType)SqlDbType.Char,15, ip),
			                        };
            string commandText = string.Format("SELECT TOP 1 [errcount], [lastupdate] FROM [{0}failedlogins] WHERE [ip]=@ip", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        /// <summary>
        /// 增加指定IP的错误记录数
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int AddErrLoginCount(string ip)
        {
            DbParameter[] parms = {
										 DbHelper.MakeInParam("@ip",(DbType)SqlDbType.Char,15, ip),
			                        };
            string commandText = string.Format("UPDATE [{0}failedlogins] SET [errcount]=[errcount]+1, [lastupdate]=GETDATE() WHERE [ip]=@ip",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 增加指定IP的错误记录
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int AddErrLoginRecord(string ip)
        {
            DbParameter[] parms = {
										 DbHelper.MakeInParam("@ip",(DbType)SqlDbType.Char,15, ip),
			                        };
            string commandText = string.Format("DELETE FROM [{0}failedlogins] WHERE [ip]=@ip;INSERT INTO [{0}failedlogins] ([ip], [errcount], [lastupdate]) VALUES(@ip, 1, GETDATE())",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 将指定IP的错误登录次数重置为1
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int ResetErrLoginCount(string ip)
        {
            DbParameter[] parms = {
										 DbHelper.MakeInParam("@ip",(DbType)SqlDbType.Char,15, ip),
			                        };
            string commandText = string.Format("UPDATE [{0}failedlogins] SET [errcount]=1, [lastupdate]=GETDATE() WHERE [ip]=@ip",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 删除指定IP或者超过15分钟的记录
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int DeleteErrLoginRecord(string ip)
        {
            DbParameter[] parms = {
										 DbHelper.MakeInParam("@ip",(DbType)SqlDbType.Char,15, ip),
			                      };
            string commandText = string.Format("DELETE FROM [{0}failedlogins] WHERE [ip]=@ip OR DATEDIFF(n,[lastupdate], GETDATE()) > 15",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public int GetPostCount(string postTableName)
        {
            return TypeConverter.ObjectToInt(
                                 DbHelper.ExecuteDataset(CommandType.Text,
                                                         string.Format("SELECT COUNT([pid]) FROM [{0}]", postTableName)).Tables[0].Rows[0][0]);
        }

        public DataTable GetPostTableList()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}tablelist]", DbFields.TABLE_LIST, BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public int UpdateDetachTable(int fid, string description)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid),
                                        DbHelper.MakeInParam("@description", (DbType)SqlDbType.NVarChar, 50, description)
                                    };
            string commandText = string.Format("UPDATE [{0}tablelist] SET [description]=@description  Where [id]=@fid", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public int StartFullIndex(string dbName)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("USE {0};EXECUTE sp_fulltext_database 'enable';", dbName));
        }
        public void CreatePostTableAndIndex(string tableName)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, GetSpecialTableFullIndexSQL(tableName, GetDbName()));
        }


        /// <summary>
        /// 得到指定帖子分表的全文索引建立(填充)语句
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public string GetSpecialTableFullIndexSQL(string tableName, string dbName)
        {
            #region 建表

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[{0}]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)  DROP TABLE [{0}];");
            sqlBuilder.Append("CREATE TABLE [{0}] ([pid] [int] NOT NULL ,[fid] [int] NOT NULL ,");
            sqlBuilder.Append("[tid] [int] NOT NULL ,[parentid] [int] NOT NULL ,[layer] [int] NOT NULL ,[poster] [nvarchar] (20) NOT NULL ,");
            sqlBuilder.Append("[posterid] [int] NOT NULL ,[title] [nvarchar] (80) NOT NULL ,[postdatetime] [smalldatetime] NOT NULL ,");
            sqlBuilder.Append("[message] [ntext] NOT NULL ,[ip] [nvarchar] (15) NOT NULL ,");
            sqlBuilder.Append("[lastedit] [nvarchar] (50) NOT NULL ,[invisible] [int] NOT NULL ,[usesig] [int] NOT NULL ,[htmlon] [int] NOT NULL ,");
            sqlBuilder.Append("[smileyoff] [int] NOT NULL ,[parseurloff] [int] NOT NULL ,[bbcodeoff] [int] NOT NULL ,[attachment] [int] NOT NULL ,[rate] [int] NOT NULL ,");
            sqlBuilder.Append("[ratetimes] [int] NOT NULL ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];");
            sqlBuilder.Append("ALTER TABLE [{0}] WITH NOCHECK ADD CONSTRAINT [PK_{0}] PRIMARY KEY  CLUSTERED ([pid])  ON [PRIMARY];");

            sqlBuilder.Append("ALTER TABLE [{0}] ADD ");
            sqlBuilder.Append("CONSTRAINT [DF_{0}_pid] DEFAULT (0) FOR [pid],");
            sqlBuilder.Append("CONSTRAINT [DF_{0}_parentid] DEFAULT (0) FOR [parentid],CONSTRAINT [DF_{0}_layer] DEFAULT (0) FOR [layer],");
            sqlBuilder.Append("CONSTRAINT [DF_{0}_poster] DEFAULT ('') FOR [poster],CONSTRAINT [DF_{0}_posterid] DEFAULT (0) FOR [posterid],");
            sqlBuilder.Append("CONSTRAINT [DF_{0}_postdatetime] DEFAULT (getdate()) FOR [postdatetime],CONSTRAINT [DF_{0}_message] DEFAULT ('') FOR [message],");
            sqlBuilder.Append("CONSTRAINT [DF_{0}_ip] DEFAULT ('') FOR [ip],CONSTRAINT [DF_{0}_lastedit] DEFAULT ('') FOR [lastedit],");
            sqlBuilder.Append("CONSTRAINT [DF_{0}_invisible] DEFAULT (0) FOR [invisible],CONSTRAINT [DF_{0}_usesig] DEFAULT (0) FOR [usesig],");
            sqlBuilder.Append("CONSTRAINT [DF_{0}_htmlon] DEFAULT (0) FOR [htmlon],CONSTRAINT [DF_{0}_smileyoff] DEFAULT (0) FOR [smileyoff],");
            sqlBuilder.Append("CONSTRAINT [DF_{0}_parseurloff] DEFAULT (0) FOR [parseurloff],CONSTRAINT [DF_{0}_bbcodeoff] DEFAULT (0) FOR [bbcodeoff],");
            sqlBuilder.Append("CONSTRAINT [DF_{0}_attachment] DEFAULT (0) FOR [attachment],CONSTRAINT [DF_{0}_rate] DEFAULT (0) FOR [rate],");
            sqlBuilder.Append("CONSTRAINT [DF_{0}_ratetimes] DEFAULT (0) FOR [ratetimes];");

            sqlBuilder.Append("CREATE  INDEX [parentid] ON [{0}]([parentid]) ON [PRIMARY];");
            sqlBuilder.Append("CREATE  UNIQUE  INDEX [showtopic] ON [{0}]([tid], [invisible], [pid]) ON [PRIMARY];");
            //sqlBuilder.Append("CREATE  INDEX [treelist] ON [{0}]([tid], [invisible], [parentid]) ON [PRIMARY];");

            #endregion

            #region 建全文索引

            sqlBuilder.Append("USE {1} \r\n");
            sqlBuilder.Append("EXECUTE sp_fulltext_database 'enable'; \r\n");
            sqlBuilder.Append("IF(SELECT DATABASEPROPERTY('[{1}]','isfulltextenabled'))=0  EXECUTE sp_fulltext_database 'enable';");
            sqlBuilder.Append("IF EXISTS (SELECT * FROM sysfulltextcatalogs WHERE name ='pk_{0}_msg')  EXECUTE sp_fulltext_catalog 'pk_{0}_msg','drop';");
            sqlBuilder.Append("IF EXISTS (SELECT * FROM sysfulltextcatalogs WHERE name ='pk_{0}_msg')  EXECUTE sp_fulltext_table '[{0}]', 'drop' ;");
            sqlBuilder.Append("EXECUTE sp_fulltext_catalog 'pk_{0}_msg','create';");
            sqlBuilder.Append("EXECUTE sp_fulltext_table '[{0}]','create','pk_{0}_msg','pk_{0}';");
            sqlBuilder.Append("EXECUTE sp_fulltext_column '[{0}]','message','add';");
            sqlBuilder.Append("EXECUTE sp_fulltext_table '[{0}]','activate';");
            sqlBuilder.Append("EXECUTE sp_fulltext_catalog 'pk_{0}_msg','start_full';");

            #endregion

            return string.Format(sqlBuilder.ToString(), tableName, dbName);
        }

        public void AddPostTableToTableList(string description, int mintid, int maxtid)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@description", (DbType)SqlDbType.NVarChar, 50, description),
                                        DbHelper.MakeInParam("@mintid", (DbType)SqlDbType.Int, 4, mintid),
                                        DbHelper.MakeInParam("@maxtid", (DbType)SqlDbType.Int, 4, maxtid)
                                    };
            string commandText = string.Format("INSERT INTO [{0}tablelist] ([description],[mintid],[maxtid]) VALUES(@description, @mintid, @maxtid)",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void CreatePostProcedure(string sqlTemplate)
        {
            foreach (string sql in sqlTemplate.Split('~'))
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, sql);
            }
        }

        public DataTable GetMaxTid()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT MAX([tid]) FROM [{0}topics]", BaseConfigs.GetTablePrefix)).Tables[0];
        }

        public DataTable GetPostCountFromIndex(string postsId)
        {
            string commandText = string.Format("SELECT TOP 1 [rows] FROM [sysindexes] WHERE [name]='PK_{0}posts{1}'",
                                                BaseConfigs.GetTablePrefix,
                                                postsId);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public DataTable GetPostCountTable(string postsId)
        {
            string commandText = string.Format("SELECT COUNT(pid) FROM [{0}posts{1}]", BaseConfigs.GetTablePrefix, postsId);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public void TestFullTextIndex(int postTableId)
        {
            string commandText = string.Format("SELECT TOP 1 [pid] FROM [{0}posts{1}] WHERE CONTAINS([message],'asd') ORDER BY [pid] ASC",
                                                BaseConfigs.GetTablePrefix,
                                                postTableId);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public DataTable GetRateRange(int scoreId)
        {
            string commandText = string.Format("SELECT [groupid], [raterange] FROM [{0}usergroups] WHERE [raterange] LIKE '%{1},True,%'",
                                                BaseConfigs.GetTablePrefix,
                                                scoreId);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public void UpdateRateRange(string rateRange, int groupId)
        {
            string commandText = string.Format("UPDATE [{0}usergroups] SET [raterange]='{1}' WHERE [groupid]={2}",
                                                BaseConfigs.GetTablePrefix,
                                                rateRange,
                                                groupId);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public int GetMaxTableListId()
        {
            string commandText = string.Format("SELECT ISNULL(MAX([id]), 0) FROM [{0}tablelist]", BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        public int GetMaxPostTableTid(string postTableName)
        {
            string commandText = string.Format("SELECT ISNULL(MAX([tid]), 0) FROM [{0}]", postTableName);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText)) + 1;
        }

        public int GetMinPostTableTid(string postTableName)
        {
            string commandText = string.Format("SELECT ISNULL(MIN([tid]), 0) FROM [{0}]", postTableName);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText)) + 1;
        }

        public void AddAdInfo(int available, string type, int displayOrder, string title, string targets, string parameters, string code, string startTime, string endTime)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@available", (DbType)SqlDbType.Int, 4, available),
                                        DbHelper.MakeInParam("@type", (DbType)SqlDbType.NVarChar, 50, type),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayOrder),
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 50, title),
                                        DbHelper.MakeInParam("@targets", (DbType)SqlDbType.NVarChar, 255, targets),
                                        DbHelper.MakeInParam("@parameters", (DbType)SqlDbType.NText, 0, parameters),
                                        DbHelper.MakeInParam("@code", (DbType)SqlDbType.NText, 0, code),
                                        DbHelper.MakeInParam("@starttime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(startTime)),
                                        DbHelper.MakeInParam("@endtime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(endTime))                                        
                                    };
            string commandText = string.Format("INSERT INTO  [{0}advertisements] ([available],[type],[displayorder],[title],[targets],[parameters],[code],[starttime],[endtime]) VALUES(@available,@type,@displayorder,@title,@targets,@parameters,@code,@starttime,@endtime)",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public DataTable GetAdvertisements()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}advertisements] ORDER BY [displayorder]",
                                                DbFields.ADVERTISEMENTS,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(commandText).Tables[0];
        }

        public DataTable GetAdvertisements(int type)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@type", (DbType)SqlDbType.Int, 4, type)
                                   
                                   };
            string commandText = string.Format("SELECT {0} FROM [{1}advertisements] WHERE [type]=@type ORDER BY [displayorder]",
                                                DbFields.ADVERTISEMENTS,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        public int UpdateAdvertisementAvailable(string aidList, int available)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@available", (DbType)SqlDbType.Int, 4, available)
                                    };
            string commandText = string.Format("UPDATE [{0}advertisements] SET [available]=@available  WHERE [advid] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                aidList);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public int UpdateAdvertisement(int aid, int available, string type, int displayOrder, string title, string targets, string parameters, string code, string startTime, string endTime)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@aid", (DbType)SqlDbType.Int, 4, aid),
                                        DbHelper.MakeInParam("@available", (DbType)SqlDbType.Int, 4, available),
                                        DbHelper.MakeInParam("@type", (DbType)SqlDbType.NVarChar, 50, type),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayOrder),
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 50, title),
                                        DbHelper.MakeInParam("@targets", (DbType)SqlDbType.NVarChar, 255, targets),
                                        DbHelper.MakeInParam("@parameters", (DbType)SqlDbType.NText, 0, parameters),
                                        DbHelper.MakeInParam("@code", (DbType)SqlDbType.NText, 0, code),
                                        DbHelper.MakeInParam("@starttime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(startTime)),
                                        DbHelper.MakeInParam("@endtime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(endTime))
                                    };
            string commandText = string.Format("UPDATE [{0}advertisements] SET [available]=@available,[type]=@type, [displayorder]=@displayorder,[title]=@title,[targets]=@targets,[parameters]=@parameters,[code]=@code,[starttime]=@starttime,[endtime]=@endtime WHERE [advid]=@aid",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void DeleteAdvertisement(string aidList)
        {
            string commandText = string.Format("DELETE FROM [{0}advertisements] WHERE [advid] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                aidList);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public void BuyTopic(int uid, int tid, int posterId, int price, float netAmount, int creditsTrans)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid),
									   DbHelper.MakeInParam("@authorid",(DbType)SqlDbType.Int,4,posterId),
									   DbHelper.MakeInParam("@buydate",(DbType)SqlDbType.DateTime,4,DateTime.Now),
									   DbHelper.MakeInParam("@amount",(DbType)SqlDbType.Int,4,price),
									   DbHelper.MakeInParam("@netamount",(DbType)SqlDbType.Float,8,netAmount)
								   };
            string commandText = string.Format("UPDATE [{0}users] SET [extcredits{1}] = [extcredits{1}] - {2} WHERE [uid] = @uid",
                                                BaseConfigs.GetTablePrefix,
                                                creditsTrans,
                                                price);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            commandText = string.Format("UPDATE [{0}users] SET [extcredits{1}] = [extcredits{1}] + @netamount WHERE [uid] = @authorid",
                                         BaseConfigs.GetTablePrefix,
                                         creditsTrans);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public int CreatePaymentLog(int uid, int tid, int posterId, int price, float netAmount)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid),
									   DbHelper.MakeInParam("@authorid",(DbType)SqlDbType.Int,4,posterId),
									   DbHelper.MakeInParam("@buydate",(DbType)SqlDbType.DateTime,4,DateTime.Now),
									   DbHelper.MakeInParam("@amount",(DbType)SqlDbType.Int,4,price),
									   DbHelper.MakeInParam("@netamount",(DbType)SqlDbType.Float,8,netAmount)
								   };
            string commandText = string.Format("INSERT INTO [{0}paymentlog] ([uid],[tid],[authorid],[buydate],[amount],[netamount]) VALUES(@uid,@tid,@authorid,@buydate,@amount,@netamount)",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 判断用户是否已购买主题
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public bool IsBuyer(int tid, int uid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
								   };
            string commandText = string.Format("SELECT [id] FROM [{0}paymentlog] WHERE [tid] = @tid AND [uid]=@uid", BaseConfigs.GetTablePrefix);
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms), 0) > 0;
        }

        public DataTable GetPayLogInList(int pageSize, int currentPage, int uid)
        {
            int pagetop = (currentPage - 1) * pageSize;
            string commandText;
            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} {1}, [{2}topics].[fid] AS fid ,[{2}topics].[postdatetime] AS postdatetime ,[{2}topics].[poster] AS authorname, [{2}topics].[title] AS title,[{2}users].[username] AS UserName FROM [{2}paymentlog] AS [pl] LEFT OUTER JOIN [{2}topics] ON [pl].[tid] = [{2}topics].[tid] LEFT OUTER JOIN [{2}users] ON [{2}users].[uid] = [pl].[uid] WHERE [pl].[authorid]={3}  ORDER BY [pl].[id] DESC",
                                             pageSize,
                                             DbFields.PAYMENT_LOG_JOIN,
                                             BaseConfigs.GetTablePrefix,
                                             uid);
            else
                commandText = string.Format("SELECT TOP {0} {1}, [{2}topics].[fid] AS fid ,[{2}topics].[postdatetime] AS postdatetime ,[{2}topics].[poster] AS authorname, [{2}topics].[title] AS title,[{2}users].[username] AS UserName FROM [{2}paymentlog] AS [pl] LEFT OUTER JOIN [{2}topics] ON [pl].[tid] = [{2}topics].[tid] LEFT OUTER JOIN [{2}users] ON [{2}users].[uid] = [pl].[uid] WHERE [id] < (SELECT MIN([id]) FROM (SELECT TOP {3} [id] FROM [{2}paymentlog] WHERE [{2}paymentlog].[authorid]={4} ORDER BY [id] DESC) AS tblTmp ) AND [pl].[authorid]={4} ORDER BY [pl].[id] DESC",
                                             pageSize,
                                             DbFields.PAYMENT_LOG_JOIN,
                                             BaseConfigs.GetTablePrefix,
                                             pagetop,
                                             uid);

            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 获取指定用户的收入日志记录数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public int GetPaymentLogInRecordCount(int uid)
        {
            string commandText = string.Format("SELECT COUNT([id]) FROM [{0}paymentlog] WHERE [authorid]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                uid);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 返回指定用户的支出日志记录数
        /// </summary>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="currentpage">当前页</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public DataTable GetPayLogOutList(int pageSize, int currentPage, int uid)
        {
            int pagetop = (currentPage - 1) * pageSize;
            string commandText;

            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} {1}, [{2}topics].[fid] AS fid ,[{2}topics].[postdatetime] AS postdatetime ,[{2}topics].[poster] AS authorname, [{2}topics].[title] AS title,[{2}users].[username] AS UserName FROM [{2}paymentlog] AS [pl] LEFT OUTER JOIN [{2}topics] ON [pl].[tid] = [{2}topics].[tid] LEFT OUTER JOIN [{2}users] ON [{2}users].[uid] = [pl].[uid] WHERE [pl].[uid]={3}  ORDER BY [pl].[id] DESC",
                                             pageSize,
                                             DbFields.PAYMENT_LOG_JOIN,
                                             BaseConfigs.GetTablePrefix,
                                             uid);
            else
                commandText = string.Format("SELECT TOP {0} {1}, [{2}topics].[fid] AS fid ,[{2}topics].[postdatetime] AS postdatetime ,[{2}topics].[poster] AS authorname, [{2}topics].[title] AS title,[{2}users].[username] AS UserName FROM [{2}paymentlog] AS [pl] LEFT OUTER JOIN [{2}topics] ON [pl].[tid] = [{2}topics].[tid] LEFT OUTER JOIN [{2}users] ON [{2}users].[uid] = [pl].[uid] WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP {3} [id] FROM [{2}paymentlog] WHERE [{2}paymentlog].[uid]={4} ORDER BY [id] DESC) AS tblTmp ) AND [pl].[uid]={4} ORDER BY [pl].[id] DESC",
                                             pageSize,
                                             DbFields.PAYMENT_LOG_JOIN,
                                             BaseConfigs.GetTablePrefix,
                                             pagetop,
                                             uid);

            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 返回指定用户支出日志总数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public int GetPaymentLogOutRecordCount(int uid)
        {
            string commandText = string.Format("SELECT COUNT([id]) FROM [{0}paymentlog] WHERE [uid]={1}", BaseConfigs.GetTablePrefix, uid);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 获取指定主题的购买记录
        /// </summary>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="currentpage">当前页数</param>
        /// <param name="tid">主题id</param>
        /// <returns></returns>
        public DataTable GetPaymentLogByTid(int pageSize, int currentPage, int tid)
        {
            int pagetop = (currentPage - 1) * pageSize;
            string commandText;
            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} {1}, [{2}users].[username] AS username FROM [{2}paymentlog] AS [pl] LEFT OUTER JOIN [{2}topics] ON [pl].[tid] = [{2}topics].[tid] LEFT OUTER JOIN [{2}users] ON [{2}users].[uid] = [pl].[uid] WHERE [pl].[tid]={3}  ORDER BY [pl].[id] DESC",
                                             pageSize,
                                             DbFields.PAYMENT_LOG_JOIN,
                                             BaseConfigs.GetTablePrefix,
                                             tid);
            else
                commandText = string.Format("SELECT TOP {0} {1}, [{2}users].[username] AS username FROM [{2}paymentlog] AS [pl] LEFT OUTER JOIN [{2}topics] ON [pl].[tid] = [{2}topics].[tid] LEFT OUTER JOIN [{2}users] ON [{2}users].[uid] = [pl].[uid] WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP {3} [id] FROM [{2}paymentlog] WHERE [{2}paymentlog].[tid]={4} ORDER BY [id] DESC) AS tblTmp ) AND [pl].[tid]={4} ORDER BY [pl].[id] DESC",
                                             pageSize,
                                             DbFields.PAYMENT_LOG_JOIN,
                                             BaseConfigs.GetTablePrefix,
                                             pagetop,
                                             tid);

            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 主题购买总次数
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns></returns>
        public int GetPaymentLogByTidCount(int tid)
        {
            string commandText = string.Format("SELECT COUNT([id]) FROM [{0}paymentlog] WHERE [tid]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                tid);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText));
        }


        public void AddSmiles(int id, int displayOrder, int type, string code, string url)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayOrder),
                                        DbHelper.MakeInParam("@type", (DbType)SqlDbType.Int, 4, type),
                                        DbHelper.MakeInParam("@code", (DbType)SqlDbType.NVarChar, 30, code),
                                        DbHelper.MakeInParam("@url", (DbType)SqlDbType.VarChar, 60, url)
                                    };

            string commandText = string.Format("INSERT INTO [{0}smilies] ([id],[displayorder],[type],[code],[url]) Values (@id,@displayorder,@type,@code,@url)",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void DeleteSmilyByType(int type)
        {
            string commandText = string.Format("DELETE FROM [{0}smilies] WHERE [type]={1}", BaseConfigs.GetTablePrefix, type);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public int UpdateSmilies(int id, int displayOrder, int type, string code)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayOrder),
                                        DbHelper.MakeInParam("@type", (DbType)SqlDbType.Int, 4, type),
                                        DbHelper.MakeInParam("@code", (DbType)SqlDbType.NVarChar, 30, code)
                                    };
            string commandText = string.Format("UPDATE [{0}smilies] SET [displayorder]=@displayorder,[type]=@type,[code]=@code Where [id]=@id",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public int UpdateSmiliesPart(string code, int displayOrder, int id)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayOrder),
                                        DbHelper.MakeInParam("@code", (DbType)SqlDbType.NVarChar, 30, code)
                                    };
            string commandText = string.Format("UPDATE [{0}smilies] SET [code]=@code,[displayorder]=@displayorder WHERE [id]=@id",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public int DeleteSmilies(string idList)
        {
            string commandText = string.Format("DELETE FROM [{0}smilies]  WHERE [ID] IN({1})", BaseConfigs.GetTablePrefix, idList);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public DataTable GetSmilies()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}smilies] WHERE [type]=0", DbFields.SMILIES, BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(commandText).Tables[0];
        }

        public int GetMaxSmiliesId()
        {
            string commandText = string.Format("SELECT ISNULL(MAX(id), 0) FROM [{0}smilies]", BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText)) + 1;
        }

        public DataTable GetSmiliesInfoByType(int type)
        {
            string commandText = string.Format("SELECT {0} FROM [{1}smilies] WHERE [type]={2}",
                                                DbFields.SMILIES,
                                                BaseConfigs.GetTablePrefix,
                                                type);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 得到表情符数据
        /// </summary>
        /// <returns>表情符数据</returns>
        public IDataReader GetSmiliesList()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}smilies] WHERE [type]=0 ORDER BY [displayorder] DESC,[id] ASC",
                                                DbFields.SMILIES,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(System.Data.CommandType.Text, commandText);
        }

        /// <summary>
        /// 得到表情符数据
        /// </summary>
        /// <returns>表情符表</returns>
        public DataTable GetSmiliesListDataTable()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}smilies] ORDER BY [type],[displayorder],[id]",
                                                DbFields.SMILIES,
                                                BaseConfigs.GetTablePrefix);
            DataSet ds = DbHelper.ExecuteDatasetInMasterDB(CommandType.Text, commandText);
            return (ds != null && ds.Tables.Count > 0) ? ds.Tables[0] : new DataTable();
        }

        /// <summary>
        /// 得到不带分类的表情符数据
        /// </summary>
        /// <returns>表情符表</returns>
        public DataTable GetSmiliesListWithoutType()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}smilies] WHERE [type]<>0 ORDER BY [type],[displayorder],[id]",
                                                DbFields.SMILIES,
                                                BaseConfigs.GetTablePrefix);
            DataSet ds = DbHelper.ExecuteDatasetInMasterDB(System.Data.CommandType.Text, commandText);
            return (ds != null && ds.Tables.Count > 0) ? ds.Tables[0] : new DataTable();
        }

        /// <summary>
        /// 获得表情分类列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetSmilieTypes()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}smilies] WHERE [type]=0 ORDER BY [displayorder],[id]",
                                                DbFields.SMILIES,
                                                BaseConfigs.GetTablePrefix);
            DataSet ds = DbHelper.ExecuteDatasetInMasterDB(CommandType.Text, commandText);
            return (ds != null && ds.Tables.Count > 0) ? ds.Tables[0] : new DataTable();
        }


        /// <summary>
        /// 获得统计列
        /// </summary>
        /// <returns>统计列</returns>
        public DataTable GetStatisticsRow()
        {
            string commandText = string.Format("SELECT TOP 1 {0} FROM [{1}statistics]", DbFields.STATISTICS, BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDatasetInMasterDB(CommandType.Text, commandText).Tables[0];
        }

        public void UpdateYesterdayPosts(string postTableId)
        {
            int max = Convert.ToInt32(postTableId);
            int min = max > 4 ? (max - 3) : 1;

            StringBuilder sqlBuilder = new StringBuilder();

            for (int i = max; i >= min; i--)
            {
                if (i < max)
                    sqlBuilder.Append(" UNION ");

                sqlBuilder.AppendFormat("SELECT COUNT(1) AS [c] FROM [{0}posts{1}] WHERE [postdatetime] < '{2}' AND [postdatetime] > '{3}' AND [invisible]=0",
                                         BaseConfigs.GetTablePrefix,
                                         i,
                                         DateTime.Now.ToString("yyyy-MM-dd"),
                                         DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            }
            string commandText = string.Format("SELECT SUM([c]) FROM ({0})t", sqlBuilder.ToString());
            int yesterdayposts = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText), 0);

            //更新每日发帖量统计
            UpdateStatVars("dayposts", DateTime.Now.ToString("yyyyMMdd"), yesterdayposts.ToString());

            int highestposts = Utils.StrToInt(GetStatisticsRow().Rows[0]["highestposts"], 0);
            sqlBuilder.Remove(0, sqlBuilder.Length);
            sqlBuilder.AppendFormat("UPDATE [{0}statistics] SET [yesterdayposts]=", BaseConfigs.GetTablePrefix);
            sqlBuilder.Append(yesterdayposts.ToString());
            if (yesterdayposts > highestposts)
            {
                sqlBuilder.AppendFormat(", [highestposts]={0}", yesterdayposts);
                sqlBuilder.AppendFormat(", [highestpostsdate]='{0}'", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            }
            DbHelper.ExecuteNonQuery(sqlBuilder.ToString());
        }

        public IDataReader GetAllForumStatistics()
        {
            string commandText = string.Format("SELECT SUM([topics]) AS [topiccount],SUM([posts]) AS [postcount],SUM([todayposts])-(SELECT SUM([todayposts]) FROM [{0}forums] WHERE [lastpost] < CONVERT(CHAR(12),GETDATE(),101) AND [layer]=1) AS [todaypostcount] FROM [{0}forums] WHERE [layer]=1",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetForumStatistics(int fid)
        {
            string commandText = string.Format("SELECT SUM([topics]) AS [topiccount],SUM([posts]) AS [postcount],SUM([todayposts])-(SELECT SUM([todayposts]) FROM [{0}forums] WHERE [lastpost] < CONVERT(CHAR(12),GETDATE(),101) AND [layer]=1 AND [fid] = @fid) AS [todaypostcount] FROM [{0}forums] WHERE [fid] = {1} AND [layer]=1",
                                                BaseConfigs.GetTablePrefix,
                                                fid);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }


        /// <summary>
        /// 更新指定名称的统计项
        /// </summary>
        /// <param name="param">项目名称</param>
        /// <param name="Value">指定项的值</param>
        /// <returns>更新数</returns>
        public int UpdateStatistics(string param, string strValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (!Utils.StrIsNullOrEmpty(param))
            {
                sb.AppendFormat("UPDATE [{0}statistics] SET {1}= '{2}'", BaseConfigs.GetTablePrefix, param, strValue);
                return DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
            }
            return 0;
        }

        /// <summary>
        /// 获得前台有效的模板列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetValidTemplateList()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}templates] ORDER BY [templateid] ASC",
                                                DbFields.TEMPLATES,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDatasetInMasterDB(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 获得前台有效的模板ID列表
        /// </summary>
        /// <returns>模板ID列表</returns>
        public IDataReader GetValidTemplateIDList()
        {
            string commandText = string.Format("SELECT [templateid] FROM [{0}templates] ORDER BY [templateid]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReaderInMasterDB(CommandType.Text, commandText);
        }

        public DataTable GetPost(string postTableName, int pid)
        {
            string commandText = string.Format("SELECT TOP 1 {0} FROM [{1}] WHERE [pid]={2}",
                                                DbFields.POSTS,
                                                postTableName,
                                                pid);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public DataTable GetMainPostByTid(string postTableName, int tid)
        {
            string commandText = string.Format("SELECT TOP 1 {0} FROM [{1}] WHERE [layer]=0  AND [tid]={2}",
                                                DbFields.POSTS,
                                                postTableName,
                                                tid);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public DataTable GetAdvertisement(int aid)
        {
            //此函数放在Advs.cs文件中较好
            string commandText = string.Format("SELECT {0} FROM [{1}advertisements] WHERE [advid]={2}",
                                                DbFields.ADVERTISEMENTS,
                                                BaseConfigs.GetTablePrefix, aid);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 获取查询主题标题的sql语句
        /// </summary>
        /// <param name="posterId">作者UID</param>
        /// <param name="searchForumId">查询的版块id</param>
        /// <param name="resultOrder">排序的字段号码（1：tid，2：replies，3：views，default：postdatetime）</param>
        /// <param name="resultOrderType">asc or desc</param>
        /// <param name="digest">是否是精华</param>
        /// <param name="keyWord">关键词</param>
        /// <returns></returns>
        private string GetSearchTopicTitleSQL(int posterId, string searchForumId, int resultOrder, int resultOrderType, int searchTime, int searchTimeType, int digest, string keyWord, int postTableId)
        {
            keyWord = Regex.Replace(keyWord, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

            StringBuilder strKeyWord = new StringBuilder(keyWord);

            // 替换转义字符
            strKeyWord.Replace("'", "''");
            strKeyWord.Replace("%", "[%]");
            strKeyWord.Replace("_", "[_]");
            strKeyWord.Replace("[", "[[]");

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT [tid] FROM [{0}topics] WHERE [displayorder]>=0", BaseConfigs.GetTablePrefix);

            if (posterId > 0)
                sqlBuilder.AppendFormat(" AND [posterid]={0} ", posterId);

            if (digest > 0)
                sqlBuilder.Append(" AND [digest]>0 ");

            if (searchTime != 0)
                sqlBuilder.AppendFormat(" AND [postdatetime] {2} '{3}'",
                                          BaseConfigs.GetTablePrefix,
                                          postTableId,
                                          searchTimeType == 1 ? "<" : ">",
                                          DateTime.Now.AddDays(searchTime).ToString("yyyy-MM-dd 00:00:00"));

            if (searchForumId != string.Empty)
                sqlBuilder.AppendFormat(" AND [fid] IN ({0}) ", searchForumId);

            string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
            strKeyWord = new StringBuilder();

            if (keyWord.Length > 0)
            {
                strKeyWord.Append(" AND (1=0 ");
                for (int i = 0; i < keywordlist.Length; i++)
                {
                    strKeyWord.AppendFormat(" OR [title] LIKE '%{0}%' ", RegEsc(keywordlist[i]));
                }
                strKeyWord.Append(")");
            }

            sqlBuilder.Append(strKeyWord.ToString());
            sqlBuilder.Append(" ORDER BY ");
            switch (resultOrder)
            {
                case 1:
                    sqlBuilder.Append("[tid]");
                    break;
                case 2:
                    sqlBuilder.Append("[replies]");
                    break;
                case 3:
                    sqlBuilder.Append("[views]");
                    break;
                default:
                    sqlBuilder.Append("[lastpostid]");
                    break;
            }

            return sqlBuilder.Append(resultOrderType == 1 ? " ASC" : " DESC").ToString();
        }

        #region sphinx SQL服务
        private static SphinxConfig.ISqlService sphinxSqlService;

        private static SphinxConfig.ISqlService GetSphinxSqlService()
        {
            if (sphinxSqlService == null)
            {
                try
                {
                    sphinxSqlService = (SphinxConfig.ISqlService)Activator.CreateInstance(Type.GetType("Discuz.EntLib.SphinxClient.SphinxSqlService, Discuz.EntLib", false, true));
                }
                catch
                {
                    throw new Exception("请检查BIN目录下有无Discuz.EntLib.dll文件");
                }
            }
            return sphinxSqlService;
        }
        #endregion

        private string GetSearchPostContentSQL(int posterId, string searchForumId, int resultOrder, int resultOrderType, int searchTime, int searchTimeType, int postTableId, StringBuilder strKeyWord)
        {
            //如果开启sphinx全文搜索时
            if (!string.IsNullOrEmpty(strKeyWord.ToString()) && EntLibConfigs.GetConfig() != null && EntLibConfigs.GetConfig().Sphinxconfig.Enable)
            {
                return GetSphinxSqlService().GetSearchPostContentSQL(posterId, searchForumId, resultOrder, resultOrderType, searchTime, searchTimeType, postTableId, strKeyWord);
            }

            StringBuilder sqlBuilder = new StringBuilder();

            string orderfield = "lastpostid";
            switch (resultOrder)
            {
                case 1:
                    orderfield = "tid";
                    break;
                case 2:
                    orderfield = "replies";
                    break;
                case 3:
                    orderfield = "views";
                    break;
                default:
                    orderfield = "lastpostid";
                    break;
            }

            sqlBuilder.AppendFormat("SELECT DISTINCT [{0}posts{1}].[tid],[{0}topics].[{2}] FROM [{0}posts{1}] LEFT JOIN [{0}topics] ON [{0}topics].[tid]=[{0}posts{1}].[tid] WHERE [{0}topics].[displayorder]>=0 ",
                                     BaseConfigs.GetTablePrefix,
                                     postTableId,
                                     orderfield);

            if (searchForumId != "")
                sqlBuilder.AppendFormat(" AND [{0}posts{1}].[fid] IN ({2})", BaseConfigs.GetTablePrefix, postTableId, searchForumId);

            if (posterId != -1)
                sqlBuilder.AppendFormat(" AND [{0}posts{1}].[posterid]={2}", BaseConfigs.GetTablePrefix, postTableId, posterId);

            if (searchTime != 0)
                sqlBuilder.AppendFormat(" AND [{0}posts{1}].[postdatetime] {2} '{3}'",
                                          BaseConfigs.GetTablePrefix,
                                          postTableId,
                                          searchTimeType == 1 ? "<" : ">",
                                          DateTime.Now.AddDays(searchTime).ToString("yyyy-MM-dd 00:00:00"));

            string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
            strKeyWord = new StringBuilder();

            for (int i = 0; i < keywordlist.Length; i++)
            {
                if (strKeyWord.Length > 0)
                    strKeyWord.Append(" OR ");

                if (GeneralConfigs.GetConfig().Fulltextsearch == 1)
                {
                    //strKeyWord.AppendFormat("CONTAINS(message, '\"*", BaseConfigs.GetTablePrefix, postTableId);
                    //strKeyWord.Append(keywordlist[i]);
                    //strKeyWord.Append("*\"') ");
                    strKeyWord.AppendFormat("CONTAINS(message, '\"*{0}*\"') ", keywordlist[i]);
                }
                else
                    strKeyWord.AppendFormat("[{0}posts{1}].[message] LIKE '%{2}%' ",
                                             BaseConfigs.GetTablePrefix,
                                             postTableId,
                                             RegEsc(keywordlist[i]));
            }

            if (keywordlist.Length > 0)
                sqlBuilder.Append(" AND " + strKeyWord.ToString());

            sqlBuilder.AppendFormat("ORDER BY [{0}topics].", BaseConfigs.GetTablePrefix);

            switch (resultOrder)
            {
                case 1:
                    sqlBuilder.Append("[tid]");
                    break;
                case 2:
                    sqlBuilder.Append("[replies]");
                    break;
                case 3:
                    sqlBuilder.Append("[views]");
                    break;
                default:
                    sqlBuilder.Append("[lastpostid]");
                    break;
            }

            return sqlBuilder.Append(resultOrderType == 1 ? " ASC" : " DESC").ToString();
        }

        private string GetSearchSpacePostTitleSQL(int posterId, int resultOrder, int resultOrderType, int searchTime, int searchTimeType, string keyWord)
        {
            keyWord = Regex.Replace(keyWord, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

            StringBuilder strKeyWord = new StringBuilder(keyWord);

            // 替换转义字符
            strKeyWord.Replace("'", "''");
            strKeyWord.Replace("%", "[%]");
            strKeyWord.Replace("_", "[_]");
            strKeyWord.Replace("[", "[[]");

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT [postid] FROM [{0}spaceposts] WHERE [{0}spaceposts].[poststatus]=1 ", BaseConfigs.GetTablePrefix);

            if (posterId > 0)
                sqlBuilder.AppendFormat(" AND [uid]={0}", posterId);

            if (searchTime != 0)
            {
                sqlBuilder.AppendFormat(" AND [{0}spaceposts].[postdatetime] {1} '{2}'",
                                      BaseConfigs.GetTablePrefix,
                                      searchTimeType == 1 ? "<" : ">",
                                      DateTime.Now.AddDays(searchTime).ToString("yyyy-MM-dd 00:00:00"));
            }

            string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
            strKeyWord = new StringBuilder();

            if (keyWord.Length > 0)
            {
                strKeyWord.Append(" AND (1=0 ");
                for (int i = 0; i < keywordlist.Length; i++)
                {
                    strKeyWord.AppendFormat(" OR [title] LIKE '%{0}%' ", RegEsc(keywordlist[i]));
                }
                strKeyWord.Append(")");
            }

            sqlBuilder.Append(strKeyWord.ToString());
            sqlBuilder.Append(" ORDER BY ");
            switch (resultOrder)
            {
                case 1:
                    sqlBuilder.Append("[commentcount]");
                    break;
                case 2:
                    sqlBuilder.Append("[views]");
                    break;
                default:
                    sqlBuilder.Append("[postdatetime]");
                    break;
            }
            return sqlBuilder.Append(resultOrderType == 1 ? " ASC" : " DESC").ToString();
        }

        private string GetSearchAlbumTitleSQL(int posterId, int resultOrder, int resultOrderType, int searchTime, int searchTimeType, string keyWord)
        {
            keyWord = Regex.Replace(keyWord, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

            StringBuilder strKeyWord = new StringBuilder(keyWord);

            // 替换转义字符
            strKeyWord.Replace("'", "''");
            strKeyWord.Replace("%", "[%]");
            strKeyWord.Replace("_", "[_]");
            strKeyWord.Replace("[", "[[]");

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT [albumid] FROM [{0}albums] WHERE [{0}albums].[type]=0 ", BaseConfigs.GetTablePrefix);

            if (posterId > 0)
                sqlBuilder.AppendFormat(" AND [userid]={0}", posterId);

            if (searchTime != 0)
            {
                sqlBuilder.AppendFormat(" AND [{0}albums].[createdatetime] {1} '{2}'",
                                          BaseConfigs.GetTablePrefix,
                                          searchTimeType == 1 ? "<" : ">",
                                          DateTime.Now.AddDays(searchTime).ToString("yyyy-MM-dd 00:00:00"));
            }

            string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
            strKeyWord = new StringBuilder();

            if (keyWord.Length > 0)
            {
                strKeyWord.Append(" AND (1=0 ");
                for (int i = 0; i < keywordlist.Length; i++)
                {
                    strKeyWord.AppendFormat(" OR [title] LIKE  '%{0}%'  ", RegEsc(keywordlist[i]));
                }
                strKeyWord.Append(")");
            }

            sqlBuilder.Append(strKeyWord.ToString());

            sqlBuilder.Append(" ORDER BY ");
            switch (resultOrder)
            {
                case 1:
                    sqlBuilder.Append("[albumid]");
                    break;
                default:
                    sqlBuilder.Append("[createdatetime]");
                    break;
            }
            return sqlBuilder.Append(resultOrderType == 1 ? " ASC" : " DESC").ToString();
        }

        private string GetSearchByPosterSQL(bool spaceEnabled, bool albumEnabled, int posterId, int postTableId)
        {
            if (posterId > 0)
            {
                StringBuilder sqlBuilder = new StringBuilder();

                sqlBuilder.AppendFormat(@"SELECT DISTINCT [tid], 'forum' AS [datafrom] FROM [{0}posts{1}] WHERE [posterid]={2} AND [tid] NOT IN (SELECT [tid] FROM [{0}topics] WHERE [posterid]={2} AND [displayorder]<0)",
                                          BaseConfigs.GetTablePrefix,
                                          postTableId,
                                          posterId);

                return sqlBuilder.ToString();
            }
            return "";
        }

        private StringBuilder GetSearchByPosterResult(IDataReader reader)
        {
            StringBuilder strTids = new StringBuilder("<ForumTopics>");

            while (reader.Read())
            {
                strTids.AppendFormat("{0},", reader[0].ToString());
            }
            reader.Close();

            if (strTids.ToString().EndsWith(","))
                strTids.Length--;

            return strTids.Append("</ForumTopics>");
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="spaceenabled">空间是否开启</param>
        /// <param name="albumenable">相册是否开启</param>
        /// <param name="posttableid">帖子表id</param>
        /// <param name="userid">用户id</param>
        /// <param name="usergroupid">用户组id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="posterid">发帖者id</param>
        /// <param name="searchType">搜索类型</param>
        /// <param name="searchforumid">搜索版块id</param>
        /// <param name="searchtime">搜索时间</param>
        /// <param name="searchtimetype">搜索时间类型</param>
        /// <param name="resultorder">结果排序方式</param>
        /// <param name="resultordertype">结果类型类型</param>
        /// <returns>如果成功则返回searchid, 否则返回-1</returns>
        public int Search(bool spaceEnabled, bool albumEnabled, int postTableId, int userId, int userGroupId, string keyWord, int posterId, SearchType searchType, string searchForumId, int searchTime, int searchTimeType, int resultOrder, int resultOrderType)
        {
            // 超过30分钟的缓存纪录将被删除
            DatabaseProvider.GetInstance().DeleteExpriedSearchCache();
            string sql = string.Empty;
            StringBuilder strTids = new StringBuilder();

            switch (searchType)
            {
                case SearchType.TopicTitle:
                    sql = GetSearchTopicTitleSQL(posterId, searchForumId, resultOrder, resultOrderType, searchTime, searchTimeType, 0, keyWord, postTableId);
                    break;
                case SearchType.DigestTopic:
                    sql = GetSearchTopicTitleSQL(posterId, searchForumId, resultOrder, resultOrderType, searchTime, searchTimeType, 1, keyWord, postTableId);
                    break;
                case SearchType.ByPoster:
                    sql = GetSearchByPosterSQL(spaceEnabled, albumEnabled, posterId, postTableId);
                    break;
                case SearchType.PostContent:
                    sql = GetSearchPostContentSQL(posterId, searchForumId, resultOrder, resultOrderType, searchTime, searchTimeType, postTableId, new StringBuilder(keyWord));
                    break;
                case SearchType.SpacePostTitle:
                    sql = GetSearchSpacePostTitleSQL(posterId, resultOrder, resultOrderType, searchTime, searchTimeType, keyWord);
                    break;
                case SearchType.AlbumTitle:
                    sql = GetSearchAlbumTitleSQL(posterId, resultOrder, resultOrderType, searchTime, searchTimeType, keyWord);
                    break;
                default:
                    sql = GetSearchTopicTitleSQL(posterId, searchForumId, resultOrder, resultOrderType, searchTime, searchTimeType, 0, keyWord, postTableId);
                    break;
            }

            if (Utils.StrIsNullOrEmpty(sql))
                return -1;

            DbParameter[] prams2 = {
										DbHelper.MakeInParam("@searchstring",(DbType)SqlDbType.VarChar,255, sql),
										DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,userId),
										DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.Int,4,userGroupId)
									};
            int searchid = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format(@"SELECT TOP 1 [searchid] FROM [{0}searchcaches] WHERE [searchstring]=@searchstring AND [groupid]=@groupid", BaseConfigs.GetTablePrefix), prams2), -1);

            if (searchid > -1)
                return searchid;

            IDataReader reader;
            try
            {
                reader = DbHelper.ExecuteReader(CommandType.Text, sql);
            }
            catch
            {
                ConfirmFullTextEnable();
                reader = DbHelper.ExecuteReader(CommandType.Text, sql);
            }

            int rowcount = 0;
            if (reader != null)
            {
                switch (searchType)
                {
                    case SearchType.DigestTopic:
                    case SearchType.TopicTitle:
                    case SearchType.PostContent:
                        strTids.Append("<ForumTopics>");
                        break;
                    case SearchType.SpacePostTitle:
                        strTids.Append("<SpacePosts>");
                        break;
                    case SearchType.AlbumTitle:
                        strTids.Append("<Albums>");
                        break;
                    case SearchType.ByPoster:
                        strTids = GetSearchByPosterResult(reader);
                        SearchCacheInfo cacheinfo = new SearchCacheInfo();
                        cacheinfo.Keywords = keyWord;
                        cacheinfo.Searchstring = sql;
                        cacheinfo.Postdatetime = Utils.GetDateTime();
                        cacheinfo.Topics = rowcount;
                        cacheinfo.Tids = strTids.ToString();
                        cacheinfo.Uid = userId;
                        cacheinfo.Groupid = userGroupId;
                        cacheinfo.Ip = DNTRequest.GetIP();
                        cacheinfo.Expiration = Utils.GetDateTime();

                        reader.Close();

                        return CreateSearchCache(cacheinfo);
                }
                while (reader.Read())
                {
                    strTids.Append(reader[0].ToString());
                    strTids.Append(",");
                    rowcount++;
                }
                if (rowcount > 0)
                {
                    strTids.Remove(strTids.Length - 1, 1);
                    switch (searchType)
                    {
                        case SearchType.DigestTopic:
                        case SearchType.TopicTitle:
                        case SearchType.PostContent:
                            strTids.Append("</ForumTopics>");
                            break;
                        case SearchType.SpacePostTitle:
                            strTids.Append("</SpacePosts>");
                            break;
                        case SearchType.AlbumTitle:
                            strTids.Append("</Albums>");
                            break;

                    }
                    SearchCacheInfo cacheinfo = new SearchCacheInfo();
                    cacheinfo.Keywords = keyWord;
                    cacheinfo.Searchstring = sql;
                    cacheinfo.Postdatetime = Utils.GetDateTime();
                    cacheinfo.Topics = rowcount;
                    cacheinfo.Tids = strTids.ToString();
                    cacheinfo.Uid = userId;
                    cacheinfo.Groupid = userGroupId;
                    cacheinfo.Ip = DNTRequest.GetIP();
                    cacheinfo.Expiration = Utils.GetDateTime();

                    reader.Close();

                    return CreateSearchCache(cacheinfo);
                }
                reader.Close();
            }
            return -1;
        }

        public string BackUpDatabase(string backUpPath, string serverName, string userName, string password, string strDbName, string strFileName)
        {
            SQLServer svr = new SQLServerClass();
            try
            {
                svr.Connect(serverName, userName, password);
                Backup bak = new BackupClass();
                bak.Action = 0;
                bak.Initialize = true;
                bak.Files = backUpPath + strFileName + ".config";
                bak.Database = strDbName;
                bak.SQLBackup(svr);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message.Replace("'", " ").Replace("\n", " ").Replace("\\", "/");
            }
            finally
            {
                svr.DisConnect();
            }
        }

        public string RestoreDatabase(string backUpPath, string serverName, string userName, string password, string strDbName, string strFileName)
        {
            #region 数据库的恢复的代码

            SQLServer svr = new SQLServerClass();
            try
            {
                svr.Connect(serverName, userName, password);
                QueryResults qr = svr.EnumProcesses(-1);
                int iColPIDNum = -1;
                int iColDbName = -1;
                for (int i = 1; i <= qr.Columns; i++)
                {
                    string strName = qr.get_ColumnName(i);
                    if (strName.ToUpper().Trim() == "SPID")
                        iColPIDNum = i;
                    else if (strName.ToUpper().Trim() == "DBNAME")
                        iColDbName = i;

                    if (iColPIDNum != -1 && iColDbName != -1)
                        break;
                }

                for (int i = 1; i <= qr.Rows; i++)
                {
                    string strDBName = qr.GetColumnString(i, iColDbName);
                    if (strDBName.ToUpper() == strDbName.ToUpper())
                        svr.KillProcess(qr.GetColumnLong(i, iColPIDNum));
                }

                Restore res = new RestoreClass();
                res.Action = 0;
                res.Files = backUpPath + strFileName + ".config";
                res.Database = strDbName;
                res.ReplaceDatabase = true;
                res.SQLRestore(svr);
                return "";
            }
            catch (Exception err)
            {
                return err.Message.Replace("'", " ").Replace("\n", " ").Replace("\\", "/");
            }
            finally
            {
                svr.DisConnect();
            }
            #endregion
        }

        public string SearchVisitLog(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string others)
        {
            string commandText = GetSqlstringByPostDatetime(" [visitid]>0", postDateTimeStart, postDateTimeEnd);

            if (!Utils.StrIsNullOrEmpty(others))
                commandText += string.Format(" AND [others] LIKE '%{0}%'", RegEsc(others));

            if (!Utils.StrIsNullOrEmpty(userName))
            {
                commandText += " AND (";
                foreach (string word in userName.Split(','))
                {
                    if (!Utils.StrIsNullOrEmpty(word))
                        commandText += string.Format(" [username] LIKE '%{0}%' OR ", RegEsc(word));
                }
                commandText = commandText.Substring(0, commandText.Length - 3) + ")";
            }
            return commandText;
        }


        public string SearchMedalLog(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string reason)
        {
            string commandText = GetSqlstringByPostDatetime(" [id]>0", postDateTimeStart, postDateTimeEnd);

            if (!Utils.StrIsNullOrEmpty(reason))
                commandText += string.Format(" AND [reason] LIKE '%{0}%'", RegEsc(reason));

            if (!Utils.StrIsNullOrEmpty(userName))
            {
                commandText += " AND (";
                foreach (string word in userName.Split(','))
                {
                    if (!Utils.StrIsNullOrEmpty(word))
                        commandText += string.Format(" [username] LIKE '%{0}%' OR ", RegEsc(word));
                }
                commandText = commandText.Substring(0, commandText.Length - 3) + ")";
            }
            return commandText;
        }

        public string SearchModeratorManageLog(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string others)
        {
            string commandText = GetSqlstringByPostDatetime(" [id]>0", postDateTimeStart, postDateTimeEnd);

            if (!Utils.StrIsNullOrEmpty(others))
                commandText += string.Format(" AND [reason] LIKE '%{0}%'", RegEsc(others));

            if (!Utils.StrIsNullOrEmpty(userName))
            {
                commandText += " AND (";
                foreach (string word in userName.Split(','))
                {
                    if (!Utils.StrIsNullOrEmpty(word))
                        commandText += string.Format(" [moderatorname] LIKE '%{0}%' OR ", RegEsc(word));
                }
                commandText = commandText.Substring(0, commandText.Length - 3) + ")";
            }
            return commandText;
        }


        public string SearchModeratorManageLog(string keyWord)
        {
            return string.Format("[actions] LIKE '%{0}%' or [reason] LIKE '%{0}%' or  [moderatorname] LIKE '%{0}%' OR [grouptitle] LIKE '%{0}%' OR [fname] LIKE '%{0}%'",
                                  RegEsc(keyWord));
        }

        public string SearchPaymentLog(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName)
        {
            string commandText = " [pl].[id]>0";

            if (!Utils.StrIsNullOrEmpty(postDateTimeStart.ToString()))
                commandText += string.Format(" AND [pl].[buydate]>='{0}'", postDateTimeStart.ToString("yyyy-MM-dd HH:mm:ss"));

            if (!Utils.StrIsNullOrEmpty(postDateTimeEnd.ToString()))
                commandText += string.Format(" AND [pl].[buydate]<='{0}'", postDateTimeEnd.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"));

            if (!Utils.StrIsNullOrEmpty(userName))
            {
                string usernamesearch = " WHERE (";
                foreach (string word in userName.Split(','))
                {
                    if (!Utils.StrIsNullOrEmpty(word))
                        usernamesearch += string.Format(" [username] LIKE '%{0}%' OR ", RegEsc(word));
                }
                usernamesearch = usernamesearch.Substring(0, usernamesearch.Length - 3) + ")";

                //找出当前用户名所属的UID
                DataTable dt = DbHelper.ExecuteDataset(string.Format("SELECT [uid] From [{0}users] {1}",
                                                                      BaseConfigs.GetTablePrefix,
                                                                      usernamesearch)).Tables[0];
                string uid = "-1";
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        uid += "," + dr["uid"].ToString();
                    }
                }
                commandText += " AND [pl].[uid] IN (" + uid + ")";
            }
            return commandText;
        }

        public string SearchRateLog(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string others)
        {
            string commandText = GetSqlstringByPostDatetime(" [id]>0", postDateTimeStart, postDateTimeEnd);

            if (!Utils.StrIsNullOrEmpty(others))
                commandText += string.Format(" AND [reason] LIKE '%{0}%'", RegEsc(others));

            if (!Utils.StrIsNullOrEmpty(userName))
            {
                commandText += " AND (";
                foreach (string word in userName.Split(','))
                {
                    if (!Utils.StrIsNullOrEmpty(word))
                        commandText += string.Format(" [username] LIKE '%{0}%' OR ", RegEsc(word));
                }
                commandText = commandText.Substring(0, commandText.Length - 3) + ")";
            }
            return commandText;
        }

        public int DeletePrivateMessages(bool isNew, string postDateTime, string msgFromList, bool lowerUpper, string subject, string message, bool isUpdateUserNewPm)
        {
            string commandText = "WHERE [pmid]>0";

            if (isNew)
                commandText += " AND [new]=0";

            if (!Utils.StrIsNullOrEmpty(postDateTime))
                commandText += string.Format(" AND DATEDIFF(day,postdatetime,getdate())>={0}", postDateTime);

            if (msgFromList != "")
            {
                commandText += " AND (";
                foreach (string msgfrom in msgFromList.Split(','))
                {
                    if (!Utils.StrIsNullOrEmpty(msgfrom))
                    {
                        if (lowerUpper)
                            commandText += string.Format(" [msgfrom]='{0}' OR", msgfrom);
                        else
                            commandText += string.Format(" [msgfrom] COLLATE Chinese_PRC_CS_AS_WS ='{0}' OR", msgfrom);
                    }
                }
                commandText = commandText.Substring(0, commandText.Length - 3) + ")";
            }

            if (subject != "")
            {
                commandText += " AND (";
                foreach (string sub in subject.Split(','))
                {
                    if (!Utils.StrIsNullOrEmpty(sub))
                        commandText += string.Format(" [subject] LIKE '%{0}%' OR ", RegEsc(sub));
                }
                commandText = commandText.Substring(0, commandText.Length - 3) + ")";
            }

            if (message != "")
            {
                commandText += " AND (";
                foreach (string mess in message.Split(','))
                {
                    if (!Utils.StrIsNullOrEmpty(mess))
                        commandText += string.Format(" [message] LIKE '%{0}%' OR ", RegEsc(mess));
                }
                commandText = commandText.Substring(0, commandText.Length - 3) + ")";
            }
            //最多每次只更新100条记录
            if (isUpdateUserNewPm)
            {
                DbHelper.ExecuteNonQuery(string.Format("UPDATE [{0}users] SET [newpm]=0 WHERE [uid] IN (SELECT TOP 100 [msgtoid] FROM [{0}pms] {1} Order By [pmid] ASC)", BaseConfigs.GetTablePrefix, commandText));
            }
            //最多每次只删除100条记录
            return DbHelper.ExecuteNonQuery(string.Format("DELETE FROM [{0}pms] WHERE [pmid] IN (SELECT TOP 100 [pmid] FROM [{0}pms] {1} Order By [pmid] ASC)", BaseConfigs.GetTablePrefix, commandText));            
        }

        public DataTable GetAdminGroups()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}usergroups] WHERE [groupid]<=3 ORDER BY [groupid]",
                                                DbFields.USER_GROUPS,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(commandText).Tables[0];
        }

        public string Global_UserGrid_GetCondition(string getString)
        {
            return string.Format("[{0}users].[username]='{1}'", BaseConfigs.GetTablePrefix, getString);
        }


        public int Global_UserGrid_RecordCount(string condition)
        {
            string commandText = string.Format("SELECT COUNT(uid) FROM [{0}users]  WHERE {1}", BaseConfigs.GetTablePrefix, condition);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(commandText).Tables[0].Rows[0][0]);
        }

        public string Global_UserGrid_SearchCondition(bool isLike, bool isPostDateTime, string userName, string nickName, string userGroup, string email, string creditsStart, string creditsEnd, string lastIp, string posts, string digestPosts, string uid, string joinDateStart, string joinDateEnd)
        {
            string tableName = string.Format("[{0}users]", BaseConfigs.GetTablePrefix);
            StringBuilder sqlBuilder = new StringBuilder(" " + tableName + ".[uid]>0 ");

            if (isLike)
            {
                if (!Utils.StrIsNullOrEmpty(userName))
                    sqlBuilder.AppendFormat(" AND {1}.[username] like'%{0}%'", RegEsc(userName), tableName);
                if (!Utils.StrIsNullOrEmpty(nickName))
                    sqlBuilder.AppendFormat(" AND {1}.[nickname] like'%{0}%'", RegEsc(nickName), tableName);
            }
            else
            {
                if (!Utils.StrIsNullOrEmpty(userName))
                    sqlBuilder.AppendFormat(" AND {1}.[username] ='{0}'", userName, tableName);
                if (!Utils.StrIsNullOrEmpty(nickName))
                    sqlBuilder.AppendFormat(" AND {1}.[nickname] ='{0}'", nickName, tableName);
            }

            if (TypeConverter.StrToInt(userGroup) > 0)
                sqlBuilder.AppendFormat(" AND {1}.[groupid]={0}", userGroup, tableName);

            if (!Utils.StrIsNullOrEmpty(email))
                sqlBuilder.AppendFormat(" AND {1}.[email] LIKE '%{0}%'", RegEsc(email), tableName);

            if (!Utils.StrIsNullOrEmpty(creditsStart))
                sqlBuilder.AppendFormat(" AND {1}.[credits] >={0}", creditsStart, tableName);

            if (!Utils.StrIsNullOrEmpty(creditsEnd))
                sqlBuilder.AppendFormat(" AND {1}.[credits] <={0}", creditsEnd, tableName);

            if (!Utils.StrIsNullOrEmpty(lastIp))
                sqlBuilder.AppendFormat(" AND {1}.[lastip] LIKE '%{0}%'", RegEsc(lastIp), tableName);

            if (!Utils.StrIsNullOrEmpty(posts))
                sqlBuilder.AppendFormat(" AND {1}.[posts] >={0}", posts, tableName);

            if (!Utils.StrIsNullOrEmpty(digestPosts))
                sqlBuilder.AppendFormat(" AND {1}.[digestposts] >={0}", digestPosts, tableName);

            if (uid != "")
            {
                uid = uid.Replace(", ", ",");

                if (uid.IndexOf(",") == 0)
                    uid = uid.Substring(1, uid.Length - 1);

                if (uid.LastIndexOf(",") == (uid.Length - 1))
                    uid = uid.Substring(0, uid.Length - 1);

                if (uid != "")
                    sqlBuilder.AppendFormat(" AND {1}.[uid] IN({0})", uid, tableName);
            }

            if (isPostDateTime)
            {
                sqlBuilder.AppendFormat(" AND {1}.[joindate] >='{0}'", DateTime.Parse(joinDateStart).ToString("yyyy-MM-dd HH:mm:ss"), tableName);
                sqlBuilder.AppendFormat(" AND {1}.[joindate] <='{0}'", DateTime.Parse(joinDateEnd).ToString("yyyy-MM-dd HH:mm:ss"), tableName);
            }
            return sqlBuilder.ToString();
        }


        public DataTable Global_UserGrid(string searchCondition)
        {
            string commandText = string.Format("SELECT {0} FROM [{1}users] WHERE {2}",
                                                DbFields.USERS,
                                                BaseConfigs.GetTablePrefix,
                                                searchCondition);
            return DbHelper.ExecuteDataset(commandText).Tables[0];
        }

        public DataTable MailListTable(string userNameList)
        {
            string commandText = " WHERE [Email] Is Not null AND (";
            foreach (string username in Utils.ChkSQL(userNameList).Split(','))
            {
                if (!Utils.StrIsNullOrEmpty(username))
                    commandText += " [username] LIKE '%" + RegEsc(username.Trim()) + "%' OR ";
            }
            commandText = commandText.Substring(0, commandText.Length - 3) + ")";

            commandText = string.Format("SELECT [username],[Email]  FROM [{0}users] {1}", BaseConfigs.GetTablePrefix, commandText);

            return DbHelper.ExecuteDataset(commandText).Tables[0];
        }

        public DataTable GetMailTable(string uids)
        {
            if (!Utils.IsNumericList(uids))
                return new DataTable();

            string commandText = string.Format("SELECT [uid],[username],[email] FROM [{0}users]  WHERE [email] IS NOT NULL AND [uid] IN ({1})",
                                               BaseConfigs.GetTablePrefix,
                                               uids);
            return DbHelper.ExecuteDataset(commandText).Tables[0];
        }

        public IDataReader GetTagsListByTopic(int topicId)
        {
            DbParameter parm = DbHelper.MakeInParam("@topicid", (DbType)SqlDbType.Int, 4, topicId);
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}gettopictags", BaseConfigs.GetTablePrefix), parm);
        }

        public IDataReader GetTagInfo(int tagId)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@tagid",(DbType)SqlDbType.Int,4,tagId)
                                  };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}gettaginfo", BaseConfigs.GetTablePrefix), parms);
        }

        public void SetLastExecuteScheduledEventDateTime(string key, string serverName, DateTime lastExecuted)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@key", (DbType)SqlDbType.VarChar, 100, key),
                DbHelper.MakeInParam("@servername", (DbType)SqlDbType.VarChar, 100, serverName),
                DbHelper.MakeInParam("@lastexecuted", (DbType)SqlDbType.DateTime, 8, lastExecuted)
            };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                     string.Format("{0}setlastexecutescheduledeventdatetime", BaseConfigs.GetTablePrefix),
                                     parms);
        }

        public DateTime GetLastExecuteScheduledEventDateTime(string key, string serverName)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@key", (DbType)SqlDbType.VarChar, 100, key),
                DbHelper.MakeInParam("@servername", (DbType)SqlDbType.VarChar, 100, serverName),
                DbHelper.MakeOutParam("@lastexecuted", (DbType)SqlDbType.DateTime, 8)
            };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                     string.Format("{0}getlastexecutescheduledeventdatetime", BaseConfigs.GetTablePrefix),
                                     parms);

            return Convert.IsDBNull(parms[2].Value) ? DateTime.MinValue : Convert.ToDateTime(parms[2].Value);
        }

        public void UpdateStats(string type, string variable, int count)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@type", (DbType)SqlDbType.Char, 10, type),
                DbHelper.MakeInParam("@variable", (DbType)SqlDbType.Char, 20, variable),
                DbHelper.MakeInParam("@count", (DbType)SqlDbType.Int, 4, count)
            };
            string commandText = string.Format("UPDATE [{0}stats] SET [count]=[count]+@count WHERE [type]=@type AND [variable]=@variable",
                                                BaseConfigs.GetTablePrefix);
            if (DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms) == 0)
            {
                if (count == 0)
                    parms[2].Value = 1;

                commandText = string.Format("INSERT INTO [{0}stats] ([type],[variable],[count]) VALUES(@type, @variable, @count)",
                                             BaseConfigs.GetTablePrefix);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            }
        }

        public void UpdateStatVars(string type, string variable, string value)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@type", (DbType)SqlDbType.Char, 20, type),
                DbHelper.MakeInParam("@variable", (DbType)SqlDbType.Char, 20, variable),
                DbHelper.MakeInParam("@value", (DbType)SqlDbType.Text, 0, value)
            };

            if (DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}statvars] SET [value]=@value WHERE [type]=@type AND [variable]=@variable", BaseConfigs.GetTablePrefix), parms) == 0)
                DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("INSERT INTO [{0}statvars] ([type],[variable],[value]) VALUES(@type, @variable, @value)", BaseConfigs.GetTablePrefix), parms);
        }

        public IDataReader GetAllStats()
        {
            string commandText = string.Format("SELECT [type], [variable], [count] FROM [{0}stats] ORDER BY [type],[variable]",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetAllStatVars()
        {
            string commandText = string.Format("SELECT [type], [variable], [value] FROM [{0}statvars] ORDER BY [type],[variable]",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public void DeleteOldDayposts()
        {
            string commandText = string.Format("DELETE FROM {0}statvars WHERE [type]='dayposts' AND [variable]<'{1}'",
                                                BaseConfigs.GetTablePrefix,
                                                DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"));
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public int GetForumCount()
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}forums] WHERE [layer]>0 AND [status]>0", BaseConfigs.GetTablePrefix);
            return (int)DbHelper.ExecuteScalar(CommandType.Text, commandText);
        }

        public int GetTodayPostCount(string postTableId)
        {
            if (!Utils.IsNumeric(postTableId))
                postTableId = "1";

            string commandText = string.Format("SELECT COUNT(1) FROM [{0}posts{1}] WHERE [postdatetime]>='{2}' AND [invisible]=0",
                                                BaseConfigs.GetTablePrefix,
                                                postTableId,
                                                DateTime.Now.ToString("yyyy-MM-dd"));
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        public int GetTodayNewMemberCount()
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}users] WHERE [joindate]>='{1}'",
                                                BaseConfigs.GetTablePrefix,
                                                DateTime.Now.ToString("yyyy-MM-dd"));
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        public int GetAdminCount()
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}users] WHERE [adminid]>0",
                                                BaseConfigs.GetTablePrefix,
                                                DateTime.Now.ToString("yyyy-MM-dd"));
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        public int GetNonPostMemCount()
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}users] WHERE [posts]=0",
                                                BaseConfigs.GetTablePrefix,
                                                DateTime.Now.ToString("yyyy-MM-dd"));
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        public IDataReader GetBestMember(string postTableId)
        {
            if (!Utils.IsNumeric(postTableId))
                postTableId = "1";

            string commandText = string.Format("SELECT TOP 1 [poster], COUNT(1) AS [posts] FROM [{0}posts{1}] WHERE [postdatetime]>='{2}' AND [invisible]=0 AND [posterid]>0 GROUP BY [poster] ORDER BY [posts] DESC",
                                                BaseConfigs.GetTablePrefix,
                                                postTableId,
                                                DateTime.Now.ToString("yyyy-MM-dd"));
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetMonthPostsStats(string postTableId)
        {
            if (!Utils.IsNumeric(postTableId))
                postTableId = "1";

            string commandText = string.Format("SELECT COUNT(1) AS [count],MONTH([postdatetime]) AS [month],YEAR([postdatetime]) AS [year] FROM [{0}posts{1}] GROUP BY MONTH([postdatetime]),YEAR([postdatetime]) ORDER BY YEAR([postdatetime]),MONTH([postdatetime])",
                                                BaseConfigs.GetTablePrefix,
                                                postTableId);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetDayPostsStats(string postTableId)
        {
            if (!Utils.IsNumeric(postTableId))
                postTableId = "1";

            string commandText = string.Format("SELECT COUNT(1) AS [count],YEAR([postdatetime]) AS [year],MONTH([postdatetime]) AS [month],DAY([postdatetime]) AS [day] FROM [{0}posts{1}] WHERE [invisible]=0 AND [postdatetime] > '{2}' GROUP BY DAY([postdatetime]), MONTH([postdatetime]),YEAR([postdatetime]) ORDER BY YEAR([postdatetime]),MONTH([postdatetime]),DAY([postdatetime])",
                                                BaseConfigs.GetTablePrefix,
                                                postTableId,
                                                DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"));
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetForumsByTopicCount(int count)
        {
            string commandText = string.Format("SELECT TOP {0} [fid], [name], [topics] FROM [{1}forums] WHERE [status]>0 AND [layer]>0 ORDER BY [topics] DESC",
                                                count,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetForumsByPostCount(int count)
        {
            string commandText = string.Format("SELECT TOP {0} [fid], [name], [posts] FROM [{1}forums] WHERE [status]>0 AND [layer]>0 ORDER BY [posts] DESC",
                                                count,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetForumsByMonthPostCount(int count, string postTableId)
        {
            if (!Utils.IsNumeric(postTableId))
                postTableId = "1";

            string commandText = string.Format("SELECT DISTINCT TOP {0} [p].[fid], [f].[name], COUNT([pid]) AS [posts] FROM [{1}posts{2}] [p] LEFT JOIN [{1}forums] [f] ON [p].[fid]=[f].[fid] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [p].[fid], [f].[name] ORDER BY [posts] DESC",
                                                count,
                                                BaseConfigs.GetTablePrefix,
                                                postTableId,
                                                DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"));
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetForumsByDayPostCount(int count, string postTableId)
        {
            if (!Utils.IsNumeric(postTableId))
                postTableId = "1";

            string commandText = string.Format("SELECT DISTINCT TOP {0} [p].[fid], [f].[name], COUNT([pid]) AS [posts] FROM [{1}posts{2}] [p] LEFT JOIN [{1}forums] [f] ON [p].[fid]=[f].[fid] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [p].[fid], [f].[name] ORDER BY [posts] DESC",
                                                count,
                                                BaseConfigs.GetTablePrefix,
                                                postTableId,
                                                DateTime.Now.ToString("yyyy-MM-dd"));
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetUsersRank(int count, string postTableId, string type)
        {
            if (!Utils.IsNumeric(postTableId))
                postTableId = "1";

            string commandText = "";
            switch (type)
            {
                case "posts":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [posts] FROM [{1}users] WHERE [posts]>0 ORDER BY [posts] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "digestposts":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [digestposts] FROM [{1}users] WHERE [digestposts]>0 ORDER BY [digestposts] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "thismonth":
                    commandText = string.Format("SELECT DISTINCT TOP {0} [poster] AS [username], [posterid] AS [uid], COUNT(pid) AS [posts] FROM [{1}posts{2}] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [poster], [posterid] ORDER BY [posts] DESC", count, BaseConfigs.GetTablePrefix, postTableId, DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"));
                    break;
                case "thisweek":
                    commandText = string.Format("SELECT DISTINCT TOP {0} [poster] AS [username], [posterid] AS [uid], COUNT(pid) AS [posts] FROM [{1}posts{2}] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [poster], [posterid] ORDER BY [posts] DESC", count, BaseConfigs.GetTablePrefix, postTableId, DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
                    break;
                case "today":
                    commandText = string.Format("SELECT DISTINCT TOP {0} [poster] AS [username], [posterid] AS [uid], COUNT(pid) AS [posts] FROM [{1}posts{2}] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [poster], [posterid] ORDER BY [posts] DESC", count, BaseConfigs.GetTablePrefix, postTableId, DateTime.Now.ToString("yyyy-MM-dd"));
                    break;
                case "credits":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [credits] FROM [{1}users] ORDER BY [credits] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits1":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits1] FROM [{1}users] ORDER BY [extcredits1] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits2":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits2] FROM [{1}users] ORDER BY [extcredits2] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits3":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits3] FROM [{1}users] ORDER BY [extcredits3] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits4":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits4] FROM [{1}users] ORDER BY [extcredits4] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits5":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits5] FROM [{1}users] ORDER BY [extcredits5] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits6":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits6] FROM [{1}users] ORDER BY [extcredits6] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits7":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits7] FROM [{1}users] ORDER BY [extcredits7] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                case "extcredits8":
                    commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits8] FROM [{1}users] ORDER BY [extcredits8] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                    break;
                //case "oltime":
                //    commandText = string.Format("SELECT TOP {0} [username], [uid], [oltime] FROM [{1}users] ORDER BY [oltime] DESC, [uid]", count, BaseConfigs.GetTablePrefix);
                //    break;
                default:
                    return null;

            }
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }


        public void UpdateStatCount(string browser, string os, string visitorsAdd)
        {
            string month = DateTime.Now.Year + DateTime.Now.Month.ToString("00");
            string dayofweek = ((int)DateTime.Now.DayOfWeek).ToString();

            string commandText = string.Format("UPDATE [{0}stats] SET [count]=[count]+1 WHERE ([type]='total' AND [variable]='hits') {1} OR ([type]='month' AND [variable]='{2}') OR ([type]='week' AND [variable]='{3}') OR ([type]='hour' AND [variable]='{4}')",
                                                BaseConfigs.GetTablePrefix,
                                                visitorsAdd,
                                                month,
                                                dayofweek,
                                                DateTime.Now.Hour.ToString("00"));
            int affectedrows = DbHelper.ExecuteNonQuery(CommandType.Text, commandText);

            int updaterows = Utils.StrIsNullOrEmpty(visitorsAdd) ? 4 : 7;
            if (updaterows > affectedrows)
            {
                UpdateStats("browser", browser, 0);
                UpdateStats("os", os, 0);
                UpdateStats("total", "members", 0);
                UpdateStats("total", "guests", 0);
                UpdateStats("total", "hits", 0);
                UpdateStats("month", month, 0);
                UpdateStats("week", dayofweek, 0);
                UpdateStats("hour", DateTime.Now.Hour.ToString("00"), 0);
            }
        }

        public IDataReader GetNoticeByUid(int uid, NoticeType noticeType)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
                                        DbHelper.MakeInParam("@type", (DbType)SqlDbType.Int, 4, noticeType),
                                  };
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("{0}getnoticebyuid", BaseConfigs.GetTablePrefix), parms);
        }


        public IDataReader GetNoticeByUid(int uid, NoticeType noticeType, int pageId, int pageSize)
        {
            string commandText = "";
            if (pageId == 1)
            {
                commandText = string.Format("SELECT TOP {0} [nid], [uid], [type], [new], [posterid], [poster], [note], [postdatetime]  FROM [{1}notices] WHERE [uid]={2} {3} ORDER BY [postdatetime] DESC",
                                            pageSize,
                                            BaseConfigs.GetTablePrefix,
                                            uid,
                                            noticeType == NoticeType.All ? "" : " AND [type]=" + (int)noticeType);
            }
            else
            {
                commandText = string.Format("SELECT TOP {0} [nid], [uid], [type], [new], [posterid], [poster], [note], [postdatetime]  FROM [{1}notices] WHERE [nid] < (SELECT MIN([nid])  FROM (SELECT TOP {2} [nid] FROM [{1}notices] WHERE [uid]={3} {4} ORDER BY [postdatetime] DESC) AS tblTmp ) AND [uid]={3} {4} ORDER BY [postdatetime] DESC",
                                            pageSize,
                                            BaseConfigs.GetTablePrefix,
                                            (pageId - 1) * pageSize,
                                            uid,
                                            noticeType == NoticeType.All ? "" : " AND [type]=" + (int)noticeType);
            }
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetNewNotices(int userId)
        {
            string commandText = string.Format("SELECT Top 5 [nid], [uid], [type], [new], [posterid], [poster], [note], [postdatetime]  FROM [{0}notices] WHERE [uid] = @uid AND [new] = 1 ORDER BY [postdatetime] DESC",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText, DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userId));
        }


        public IDataReader GetNoticeByNid(int noticeId)
        {
            string commandText = string.Format("SELECT [nid], [uid], [type], [new], [posterid], [poster], [note], [postdatetime]  FROM [{0}notices] WHERE [nid] = @nid ORDER BY [postdatetime] DESC",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText,
                                          DbHelper.MakeInParam("@nid", (DbType)SqlDbType.Int, 4, noticeId));
        }

        public int GetNoticeCountByUid(int uid, NoticeType noticeType)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
                                        DbHelper.MakeInParam("@type", (DbType)SqlDbType.Int, 4, (int)noticeType)
                                  };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                    string.Format("{0}getnoticecountbyuid", BaseConfigs.GetTablePrefix),
                                                                    parms));
        }


        public int GetNewNoticeCountByUid(int uid)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)                                        
                                  };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                    string.Format("{0}getnewnoticecountbyuid", BaseConfigs.GetTablePrefix),
                                                                    parms));
        }


        public int CreateNoticeInfo(NoticeInfo noticeInfo)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, noticeInfo.Uid),
                                        DbHelper.MakeInParam("@type", (DbType)SqlDbType.Int, 4, noticeInfo.Type),
                                        DbHelper.MakeInParam("@new", (DbType)SqlDbType.Int, 4, noticeInfo.New),
                                        DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.Int, 4, noticeInfo.Posterid),
                                        DbHelper.MakeInParam("@poster", (DbType)SqlDbType.NChar, 20, noticeInfo.Poster),
                                        DbHelper.MakeInParam("@note", (DbType)SqlDbType.NText, 0, noticeInfo.Note),
                                        DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, noticeInfo.Postdatetime),
                                        DbHelper.MakeInParam("@fromid", (DbType)SqlDbType.Int, 4, noticeInfo.Fromid)
                                    };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                    string.Format("{0}createnotice", BaseConfigs.GetTablePrefix),
                                                                    parms), -1);
        }

        //(注释无用方法 2011-04-12)
        //public bool UpdateNoticeInfo(NoticeInfo noticeInfo)
        //{
        //    DbParameter[] parms = { 
        //                                DbHelper.MakeInParam("@nid", (DbType)SqlDbType.Int, 4, noticeInfo.Nid),
        //                                DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, noticeInfo.Uid),
        //                                DbHelper.MakeInParam("@type", (DbType)SqlDbType.Int, 4, noticeInfo.Type),
        //                                DbHelper.MakeInParam("@new", (DbType)SqlDbType.Int, 4, noticeInfo.New),
        //                                DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.Int, 4, noticeInfo.Posterid),
        //                                DbHelper.MakeInParam("@poster", (DbType)SqlDbType.NChar, 20, noticeInfo.Poster),
        //                                DbHelper.MakeInParam("@note", (DbType)SqlDbType.NText, 0, noticeInfo.Note),
        //                                DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, noticeInfo.Postdatetime)
        //                            };
        //    string commandText = string.Format("UPDATE [{0}notices] SET  [uid] = @uid, [type] = @type, [new] = @new, [posterid] = @posterid, [poster] = @poster, [note] = @note, [postdatetime] = @postdatetime  WHERE [nid] = @nid",
        //                                        BaseConfigs.GetTablePrefix);
        //    return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms) > 0;
        //}

        public bool DeleteNoticeByNid(int nid)
        {
            string commandText = string.Format("DELETE FROM [{0}notices] WHERE [nid] = @nid", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText,
                                            DbHelper.MakeInParam("@nid", (DbType)SqlDbType.Int, 4, nid)) > 0;
        }

        public bool DeleteNoticeByUid(int uid)
        {
            string commandText = string.Format("DELETE FROM [{0}notices] WHERE [uid] = @uid", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText,
                                            DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)) > 0;
        }


        public void UpdateNoticeNewByUid(int uid, int newType)
        {
            DbParameter[] parms = {                                    
                                    DbHelper.MakeInParam("@new", (DbType)SqlDbType.Int, 4, newType),
                                    DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
                                };
            string commandText = string.Format("Update [{0}notices] SET [new] = @new WHERE [uid] = @uid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void DeleteNotice(NoticeType noticeType, int days)
        {
            string commandText;
            if (noticeType == NoticeType.All)
            {
                commandText = string.Format("DELETE FROM [{0}notices] WHERE DATEDIFF(d,[postdatetime], GETDATE()) > {1}",
                                             BaseConfigs.GetTablePrefix,
                                             days);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
            }
            else
            {
                commandText = string.Format("DELETE FROM [{0}notices] WHERE [type] = {1}  AND DATEDIFF(d,[postdatetime], GETDATE()) > {2}",
                                             BaseConfigs.GetTablePrefix,
                                             (int)noticeType, days);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
            }
        }

        public void AddBannedIp(IpInfo info)
        {
            DbParameter[] parameters = {
                                         DbHelper.MakeInParam("@ip1",(DbType)SqlDbType.Int, 4, info.Ip1),
                                         DbHelper.MakeInParam("@ip2",(DbType)SqlDbType.Int, 4, info.Ip2),
                                         DbHelper.MakeInParam("@ip3",(DbType)SqlDbType.Int, 4, info.Ip3),
                                         DbHelper.MakeInParam("@ip4",(DbType)SqlDbType.Int, 4, info.Ip4),
                                         DbHelper.MakeInParam("@admin",(DbType)SqlDbType.NVarChar,50,info.Username),
                                         DbHelper.MakeInParam("@dateline",(DbType)SqlDbType.NVarChar,50,info.Dateline),
                                         DbHelper.MakeInParam("@expiration",(DbType)SqlDbType.NVarChar,50,info.Expiration)
                                       };

            string sql = string.Format("INSERT INTO [{0}banned](ip1,ip2,ip3,ip4,admin,dateline,expiration) VALUES(@ip1,@ip2,@ip3,@ip4,@admin,@dateline,@expiration)",
                                        BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
        }

        public int GetBannedIpCount()
        {
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(id) FROM [{0}banned]", BaseConfigs.GetTablePrefix)));
        }

        public IDataReader GetBannedIpList()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}banned] ORDER BY [id] DESC",
                                                DbFields.BANNED,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReaderInMasterDB(CommandType.Text, commandText);
        }

        public IDataReader GetBannedIpList(int num, int pageId)
        {
            string commandText = string.Format("SELECT TOP {0} {1} FROM [{2}banned]  WHERE [id] NOT IN (SELECT TOP {3} [id] FROM [{2}banned] ORDER BY [id] DESC) ORDER BY [id] DESC",
                                                num,
                                                DbFields.BANNED,
                                                BaseConfigs.GetTablePrefix,
                                                (pageId - 1) * num);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public int UpdateBanIpExpiration(int id, string endTime)
        {
            DbParameter[] parameters = {
                                         DbHelper.MakeInParam("@id",(DbType)SqlDbType.Int, 4, id),
                                         DbHelper.MakeInParam("@expiration",(DbType)SqlDbType.DateTime, 8, DateTime.Parse(endTime)),
                                       };
            string commandText = string.Format("UPDATE [{0}banned] SET [expiration] = @expiration WHERE [id]=@id", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parameters);
        }

        public int DeleteBanIp(string bannedIdList)
        {
            string commandText = string.Format("DELETE FROM [{0}banned] WHERE [id] IN({1})", BaseConfigs.GetTablePrefix, bannedIdList);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public int UpdateAnnouncementDisplayOrder(int displayOrder, int aid)
        {
            DbParameter[] parameters = {
                                         DbHelper.MakeInParam("@displayorder",(DbType)SqlDbType.Int, 4, displayOrder),
                                         DbHelper.MakeInParam("@aid",(DbType)SqlDbType.Int, 4, aid),
                                       };
            string commandText = string.Format("UPDATE  [{0}announcements] set [displayorder]=@displayorder WHERE [id]=@aid",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parameters);
        }

        public IDataReader GetNavigationData(bool getAllNavigation)
        {
            string commandText = string.Format("SELECT {0} FROM [{1}navs] {2} ORDER BY [parentid],[displayorder],[id]",
                                                DbFields.NAVS,
                                                BaseConfigs.GetTablePrefix,
                                                getAllNavigation ? "" : " WHERE [available]=1 ");
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public void InsertNavigation(NavInfo nav)
        {
            DbParameter[] param = {
                                         DbHelper.MakeInParam("@parentid",(DbType)SqlDbType.Int,4,nav.Parentid),
                                         DbHelper.MakeInParam("@name",(DbType)SqlDbType.NChar, 50, nav.Name),
                                         DbHelper.MakeInParam("@title",(DbType)SqlDbType.NChar, 255, nav.Title),
                                         DbHelper.MakeInParam("@url",(DbType)SqlDbType.Char, 255, nav.Url),
                                         DbHelper.MakeInParam("@target",(DbType)SqlDbType.TinyInt, 1, nav.Target),
                                         DbHelper.MakeInParam("@available",(DbType)SqlDbType.TinyInt,1,nav.Available),
                                         DbHelper.MakeInParam("@displayorder",(DbType)SqlDbType.SmallInt,2,nav.Displayorder),
                                         DbHelper.MakeInParam("@level",(DbType)SqlDbType.TinyInt,1,nav.Level)
                                       };
            string commandText = String.Format("INSERT INTO [{0}navs] ([parentid],[name],[title],[url],[target],[type],[available],[displayorder],[level]) VALUES(@parentid,@name,@title,@url,@target,1,@available,@displayorder,@level)",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, param);
        }

        public void UpdateNavigation(NavInfo nav)
        {
            DbParameter[] param = {
                                         DbHelper.MakeInParam("@name",(DbType)SqlDbType.NChar, 50, nav.Name),
                                         DbHelper.MakeInParam("@title",(DbType)SqlDbType.NChar, 255, nav.Title),
                                         DbHelper.MakeInParam("@url",(DbType)SqlDbType.Char, 255, nav.Url),
                                         DbHelper.MakeInParam("@target",(DbType)SqlDbType.TinyInt, 1, nav.Target),
                                         DbHelper.MakeInParam("@available",(DbType)SqlDbType.TinyInt,1,nav.Available),
                                         DbHelper.MakeInParam("@displayorder",(DbType)SqlDbType.SmallInt,2,nav.Displayorder),
                                         DbHelper.MakeInParam("@level",(DbType)SqlDbType.TinyInt,1,nav.Level),
                                         DbHelper.MakeInParam("@id",(DbType)SqlDbType.Int,4,nav.Id)
                                       };
            string commandText = String.Format("UPDATE [{0}navs] set [name]=@name,[title]=@title,[url]=@url,[target]=@target,[available]=@available,[displayorder]=@displayorder,[level]=@level WHERE id=@id",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, param);
        }

        public void DeleteNavigation(int id)
        {
            string commandText = String.Format("DELETE FROM [{0}navs] WHERE [id] = {1}", BaseConfigs.GetTablePrefix, id);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public IDataReader GetNavigationHasSub()
        {
            string commandText = string.Format("SELECT DISTINCT [parentid] FROM [{0}navs] ORDER BY [parentid]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public int GetNoticeCount(int userId, int state)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@userid",(DbType)SqlDbType.Int,4, userId),
									   DbHelper.MakeInParam("@type",(DbType)SqlDbType.Int,4, -1),
									   DbHelper.MakeInParam("@state",(DbType)SqlDbType.Int,4,state)
								   };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                    string.Format("{0}getnoticecount", BaseConfigs.GetTablePrefix),
                                                                    parms));
        }


        public int ReNewNotice(int type, int uid)
        {
            DbParameter[] parms = {
						   DbHelper.MakeInParam("@type",(DbType)SqlDbType.Int, 4, type),
                           DbHelper.MakeInParam("@date",(DbType)SqlDbType.DateTime, 4, DateTime.Now),
						   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int, 4, uid)
					   };
            string commandText = string.Format("UPDATE [{0}notices] SET [new]=1,[postdatetime]=@date WHERE [type]=@type AND [uid]=@uid",
                                                BaseConfigs.GetTablePrefix);
            int noticecount = DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            if (noticecount > 1)
            {
                commandText = string.Format("DELETE FROM [{0}notices] WHERE [type]=@type AND [uid]=@uid", BaseConfigs.GetTablePrefix);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
                return 0;
            }
            else
                return noticecount;
        }

        public int CreateAttachPaymetLog(AttachPaymentlogInfo attachPaymentLogInfo)
        {
            DbParameter[] parms = {
						   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int, 4, attachPaymentLogInfo.Uid),
                           DbHelper.MakeInParam("@username",(DbType)SqlDbType.NChar, 20, attachPaymentLogInfo.UserName),
                           DbHelper.MakeInParam("@postdatetime",(DbType)SqlDbType.DateTime, 8, attachPaymentLogInfo.PostDateTime),
                           DbHelper.MakeInParam("@amount",(DbType)SqlDbType.Int, 4, attachPaymentLogInfo.Amount),
                           DbHelper.MakeInParam("@netamount",(DbType)SqlDbType.Int, 4, attachPaymentLogInfo.NetAmount),
                           DbHelper.MakeInParam("@aid",(DbType)SqlDbType.Int, 4, attachPaymentLogInfo.Aid),
						   DbHelper.MakeInParam("@authorid",(DbType)SqlDbType.Int, 4, attachPaymentLogInfo.Authorid)
					   };
            string commandText = string.Format("INSERT INTO [{0}attachpaymentlog] ([uid],[postdatetime],[username],[amount],[netamount],[aid],[authorid]) VALUES (@uid,@postdatetime,@username,@amount,@netamount,@aid,@authorid);SELECT SCOPE_IDENTITY()",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public int UpdateAttachPaymetLog(AttachPaymentlogInfo attachPaymentLogInfo)
        {
            DbParameter[] parms = {
                           DbHelper.MakeInParam("@id",(DbType)SqlDbType.Int, 4, attachPaymentLogInfo.Id),
						   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int, 4, attachPaymentLogInfo.Uid),
                           DbHelper.MakeInParam("@username",(DbType)SqlDbType.NChar, 20, attachPaymentLogInfo.UserName),
                           DbHelper.MakeInParam("@postdatetime",(DbType)SqlDbType.DateTime, 8, attachPaymentLogInfo.PostDateTime),
                           DbHelper.MakeInParam("@amount",(DbType)SqlDbType.Int, 4, attachPaymentLogInfo.Amount),
                           DbHelper.MakeInParam("@netamount",(DbType)SqlDbType.Int, 4, attachPaymentLogInfo.NetAmount),
                           DbHelper.MakeInParam("@aid",(DbType)SqlDbType.Int, 4, attachPaymentLogInfo.Aid),
						   DbHelper.MakeInParam("@authorid",(DbType)SqlDbType.Int, 4, attachPaymentLogInfo.Authorid)
					   };
            string commandText = string.Format("UPDATE [{0}attachpaymentlog] SET [uid]=@uid,[postdatetime]=@postdatetime,[username]=@username,[amount]=@amount,[netamount]=@netamount,[aid]=@aid,[authorid]=@authorid WHERE [id] = @id",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }


        public bool HasBoughtAttach(int userId, int aid)
        {
            DbParameter[] parms = {
                           DbHelper.MakeInParam("@aid",(DbType)SqlDbType.Int, 4, aid),
						   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int, 4, userId)
					   };
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}attachpaymentlog] WHERE [aid]=@aid AND [uid] = @uid",
                                                BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms)) > 0;
        }
    }
}
