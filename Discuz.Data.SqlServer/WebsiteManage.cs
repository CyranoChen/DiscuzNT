using System;
using System.Data;
using System.Data.Common;

using Discuz.Config;
using Discuz.Common;

namespace Discuz.Data.SqlServer
{
    public partial class DataProvider : IDataProvider
    {
        private DbParameter[] GetParms(string startDate, string endDate)
        {
            DbParameter[] parms = new DbParameter[2];
            if (startDate != "")
                parms[0] = DbHelper.MakeInParam("@startdate", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(startDate));
  
            if (endDate != "")
                parms[1] = DbHelper.MakeInParam("@enddate", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(endDate).AddDays(1));

            return parms;
        }

        public DataTable GetTopicListByCondition(string postName, int forumId, string posterList, string keyList, string startDate, string endDate, int pageSize, int currentPage)
        {
            string commandText = "";
            string condition = GetCondition(posterList, keyList, startDate, endDate);

            DbParameter[] parms = GetParms(startDate, endDate);

            int pageTop = (currentPage - 1) * pageSize;
            int minTid = DatabaseProvider.GetInstance().GetMinPostTableTid(postName);
            int maxTid = DatabaseProvider.GetInstance().GetMaxPostTableTid(postName);

            string fidCondition = forumId > 0 ? string.Format(" f.[fid]={0} OR CHARINDEX(',{0},',','+RTRIM(f.[parentidlist])+',')<>0 AND ", forumId) : "";

            if (currentPage == 1)
            {
                commandText = string.Format("SELECT TOP {0} {1},f.[name] FROM [{2}topics] t JOIN " + 
                    "(SELECT f.[fid],f.[name] FROM [{2}forums] f LEFT JOIN [{2}forumfields] ff " + 
                    "ON f.[fid]=ff.[fid] AND (ff.[viewperm] IS NULL OR CONVERT(NVARCHAR(1000),ff.[viewperm])='' " + 
                    "OR CHARINDEX(',7,',','+CONVERT(NVARCHAR(1000),ff.[viewperm])+',')<>0) WHERE {3} " +
                    "[password]='' AND [status]=1) AS f ON t.[fid]=f.[fid] " + 
                    "WHERE [tid]>={4} AND [tid]<={5} AND [closed]<>1 {6} ORDER BY [tid] DESC",
                                     pageSize, 
                                     DbFields.TOPICS_JOIN, 
                                     BaseConfigs.GetTablePrefix,
                                     fidCondition,
                                     minTid,
                                     maxTid,
                                     condition);
            }
            else
            {
                commandText = string.Format("SELECT TOP {0} {1},f.[name] FROM [{2}topics] t JOIN" + 
                    "(SELECT f.[fid],f.[name] FROM [{2}forums] f LEFT JOIN [{2}forumfields] ff ON f.[fid]=ff.[fid] AND" +  
                    "(ff.[viewperm] IS NULL OR CONVERT(NVARCHAR(1000),ff.[viewperm])='' OR CHARINDEX(',7,',','+CONVERT(NVARCHAR(1000),ff.[viewperm])+',')<>0) " +
                    "WHERE {3} [password]='' AND [status]=1) AS f ON t.[fid]=f.[fid] " +
                    "WHERE [tid]>={4} AND [tid]<(SELECT MIN([tid]) FROM (SELECT TOP {6} [tid] FROM [{2}topics] t JOIN " +
                    "(SELECT f.[fid],f.[name] FROM [{2}forums] f LEFT JOIN [{2}forumfields] ff ON f.[fid]=ff.[fid] AND" +
                    "(ff.[viewperm] IS NULL OR CONVERT(NVARCHAR(1000),ff.[viewperm])='' OR CHARINDEX(',7,',','+CONVERT(NVARCHAR(1000),ff.[viewperm])+',')<>0) " +
                    "WHERE {3} [password]='' AND [status]=1) AS f ON t.[fid]=f.[fid] " +
                    "WHERE [tid]>={4} AND [tid]<={5} AND [closed]<>1 {7} ORDER BY [tid] DESC) AS tblTmp) AND [closed]<>1 {7} ORDER BY [tid] DESC",
                                    pageSize, 
                                    DbFields.TOPICS_JOIN,
                                    BaseConfigs.GetTablePrefix,
                                    fidCondition, 
                                    minTid,
                                    maxTid,
                                    pageTop,
                                    condition);
            }
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        private static string GetCondition(string posterList, string keyList, string startDate, string endDate)
        {
            string condition = "";
     
            if (posterList != "")
            {
                condition += " AND [poster] in (";
                string tempposerlist = "";
                foreach (string p in posterList.Split(','))
                {
                    tempposerlist += "'" + p + "',";
                }
                if (tempposerlist != "")
                    tempposerlist = tempposerlist.Substring(0, tempposerlist.Length - 1);

                condition += tempposerlist + ")";
            }
            if (keyList != "")
            {
                string tempkeylist = "";
                foreach (string key in keyList.Split(','))
                {
                    tempkeylist += string.Format(" [title] LIKE '%{0}%' OR", RegEsc(key));
                }
                tempkeylist = tempkeylist.Substring(0, tempkeylist.Length - 2);
                condition += string.Format(" AND ({0})", tempkeylist);
            }
            if (startDate != "")
                condition += " AND [postdatetime]>=@startdate";

            if (endDate != "")
                condition += " AND [postdatetime]<=@enddate";

            return condition;
        }

        public int GetTopicListCountByCondition(string postName,int forumId, string posterList, string keyList, string startDate, string endDate)
        {
            string commandText = string.Format("SELECT COUNT([tid]) FROM [{0}topics] t JOIN (SELECT f.[fid],f.[name] FROM [{0}forums] f LEFT JOIN [{0}forumfields] ff ON f.[fid]=ff.[fid] AND (ff.[viewperm] IS NULL OR CONVERT(NVARCHAR(1000),ff.[viewperm])='' OR CHARINDEX(',7,',','+CONVERT(NVARCHAR(1000),ff.[viewperm])+',')<>0) WHERE f.[fid]={1} OR CHARINDEX(',{1},',','+RTRIM(f.[parentidlist])+',')<>0 AND [password]='' AND [status]=1) AS f ON t.[fid]=f.[fid]",
                BaseConfigs.GetTablePrefix,forumId);
            commandText += string.Format(" WHERE [tid]>={0} AND [tid]<={1} AND [closed]<>1", 
                                           DatabaseProvider.GetInstance().GetMinPostTableTid(postName), 
                                           DatabaseProvider.GetInstance().GetMaxPostTableTid(postName));
            string condition = GetCondition(posterList, keyList, startDate, endDate);

            if (condition != "")
                commandText += condition;

            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, GetParms(startDate, endDate)));
        }

        public int GetHotTopicsCount(int fid, int timeBetween)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int,4,fid),
                                        DbHelper.MakeInParam("@timebetween",(DbType)SqlDbType.Int,4,timeBetween)
							       };

            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}gethottopicscount", BaseConfigs.GetTablePrefix), parms));
        }

        public DataTable GetHotTopicsList(int pageSize, int pageIndex, int fid, string showType, int timeBetween)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@pagesize",(DbType)SqlDbType.Int,4,pageSize),
                                        DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,pageIndex),
                                        DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int,4,fid),
                                        DbHelper.MakeInParam("@showtype",(DbType)SqlDbType.VarChar,100,showType),
                                        DbHelper.MakeInParam("@timebetween",(DbType)SqlDbType.Int,4,timeBetween)
							       };

            //string commandText = "";
            //int pageTop = (pageIndex - 1) * pageSize;
            //if (pageIndex == 1)
            //{
            //    commandText = string.Format("SELECT TOP {0} {1},f.[name] FROM [{2}topics] t LEFT JOIN [{2}forums] f ON t.[fid] = f.[fid]  WHERE DATEDIFF(DAY,[postdatetime],GETDATE())<=7 ORDER BY [replies] DESC,[tid] DESC", pageSize, DbFields.TOPICS_JOIN, BaseConfigs.GetTablePrefix);
            //}
            //else
            //{
            //    commandText = string.Format("SELECT TOP {0} {1},f.[name] FROM [{2}topics] t LEFT JOIN [{2}forums] f ON t.[fid] = f.[fid] WHERE DATEDIFF(DAY,[postdatetime],GETDATE())<=7 AND [tid] NOT IN (SELECT TOP {3} [tid] FROM [{2}topics] WHERE DATEDIFF(DAY,[postdatetime],GETDATE())<=7 ORDER BY [replies] DESC,[tid] DESC) ORDER BY [replies] DESC,[tid] DESC",
            //        pageSize, DbFields.TOPICS_JOIN,BaseConfigs.GetTablePrefix,pageTop);
            //}
            return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}gethottopicslist", BaseConfigs.GetTablePrefix), parms).Tables[0];
        }

        public DataTable GetTopicListByTidlist(string postTableId, string tidList)
        {
            string commandText = string.Format("SELECT {0},t.[closed] FROM [{1}posts{2}] p LEFT JOIN [{1}topics] t ON p.[tid]=t.[tid] WHERE [layer]=0 AND [closed]<>1 and p.[tid] IN ({3}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),p.[tid]),'{3}')",
                                            DbFields.POSTS_JOIN,
                                            BaseConfigs.GetTablePrefix, 
                                            postTableId, 
                                            tidList);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }


        #region 前台聚合页相关函数

        public DataTable GetWebSiteAggForumTopicList(string showType, int topNumber)
        {
            DataTable topicList = new DataTable();
            switch (showType)
            {
                default://按版块查
                    topicList = DbHelper.ExecuteDataset(string.Format("SELECT f.[fid], f.[name], f.[lasttid] AS [tid], f.[lasttitle] AS [title] , f.[lastposterid] AS [posterid], f.[lastposter] AS [poster], f.[lastpost] AS [postdatetime], t.[views], t.[replies], ff.[rewritename] AS [rewritename] FROM [{0}forums] f LEFT JOIN [{0}topics] t  ON f.[lasttid] = t.[tid] LEFT OUTER JOIN [{0}forumfields] ff ON ff.[fid] = f.[fid] WHERE f.[status]=1 AND f.[layer]> 0 AND  ff.[password] ='' AND t.[displayorder]>=0", BaseConfigs.GetTablePrefix)).Tables[0]; break;
                case "1"://按最新主题查
                    topicList = DbHelper.ExecuteDataset(string.Format("SELECT TOP {0} t.[tid], t.[title], t.[postdatetime], t.[poster], t.[posterid], t.[fid], t.[views], t.[replies], f.[name], ff.[rewritename] AS [rewritename] FROM [{1}topics] t LEFT OUTER JOIN [{1}forums] f ON t.[fid] = f.[fid] LEFT OUTER JOIN [{1}forumfields] ff ON ff.[fid] = f.[fid] WHERE t.[displayorder]>=0 AND f.[status]=1 AND f.[layer]> 0 AND ff.[password] ='' ORDER BY t.[postdatetime] DESC", topNumber, BaseConfigs.GetTablePrefix)).Tables[0]; break;
                case "2"://按精华主题查
                    topicList = DbHelper.ExecuteDataset(string.Format("SELECT TOP {0} t.[tid], t.[title], t.[postdatetime], t.[poster], t.[posterid], t.[fid], t.[views], t.[replies], f.[name], ff.[rewritename] AS [rewritename] FROM [{1}topics] t LEFT OUTER JOIN [{1}forums] f ON t.[fid] = f.[fid] LEFT OUTER JOIN [{1}forumfields] ff ON ff.[fid] = f.[fid] WHERE t.[digest] >0 AND f.[status]=1 AND f.[layer]> 0 AND ff.[password] ='' ORDER BY t.[digest] DESC", topNumber, BaseConfigs.GetTablePrefix)).Tables[0]; break;
                case "3"://按版块查
                    topicList = DbHelper.ExecuteDataset(string.Format("SELECT f.[fid], f.[name], f.[lasttid] AS [tid], f.[lasttitle] AS [title] , f.[lastposterid] AS [posterid], f.[lastposter] AS [poster], f.[lastpost] AS [postdatetime], t.[views], t.[replies], ff.[rewritename] AS [rewritename] FROM [{0}forums] f LEFT JOIN [{0}topics] t  ON f.[lasttid] = t.[tid] LEFT OUTER JOIN [{0}forumfields] ff ON ff.[fid] = f.[fid] WHERE f.[status]=1 AND f.[layer]> 0 AND ff.[password] ='' AND t.[displayorder]>=0", BaseConfigs.GetTablePrefix)).Tables[0]; break;
            }
            return topicList;
        }


        public DataTable GetWebSiteAggHotForumList(int topNumber,string orderby,int fid)
        {
            string condition = fid > 0 ? " AND f.[fid]=" + fid : "";
            string commandText = string.Format("SELECT TOP {0} f.[fid], f.[name], f.[topics],f.[posts],f.[todayposts],f.[topics],ff.[rewritename] FROM [{1}forums] f LEFT JOIN [{1}forumfields] ff ON f.[fid]=ff.[fid] WHERE f.[status]=1 AND f.[layer]> 0 AND ff.[password]='' " + condition + " ORDER BY f.[{2}] DESC", 
                                                topNumber, 
                                                BaseConfigs.GetTablePrefix,orderby);
            return DbHelper.ExecuteDataset(commandText).Tables[0];
        }
        #endregion

        public DataTable GetWebSiteAggHotImages(int count, string orderby, string fidlist, int continuous)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@fidlist",(DbType)SqlDbType.VarChar,3000,fidlist),
                                        DbHelper.MakeInParam("@count",(DbType)SqlDbType.Int,4,count),
                                        DbHelper.MakeInParam("@orderby",(DbType)SqlDbType.VarChar,50,orderby),
                                        DbHelper.MakeInParam("@continuous",(DbType)SqlDbType.Int,4,continuous)
							       };
            return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}gethotimages", BaseConfigs.GetTablePrefix), parms).Tables[0];
        }
    }
}
