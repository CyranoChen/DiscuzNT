using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using Discuz.Data;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;

namespace Discuz.Data.SqlServer
{
    public partial class DataProvider : IDataProvider
    {
        public string SearchTopicAudit(int fid, string poster, string title, string moderatorName, DateTime postDateTimeStart, DateTime postDateTimeEnd, DateTime delDateTimeStart, DateTime delDateTimeEnd)
        {
            StringBuilder sqlBuilder = new StringBuilder(" [displayorder]<0");

            if (fid != 0)
                sqlBuilder.AppendFormat(" AND [fid]={0}", fid);

            if (poster != "")
            {
                sqlBuilder.Append(" AND (");
                foreach (string postername in poster.Split(','))
                {
                    if (postername.Trim() != "")
                        sqlBuilder.AppendFormat(" [poster]='{0}'  OR", postername);
                }
                sqlBuilder = sqlBuilder.Remove(sqlBuilder.Length - 3, 3).Append(")");
            }

            if (title != "")
            {
                sqlBuilder.Append(" AND (");
                foreach (string titlename in title.Split(','))
                {
                    if (titlename.Trim() != "")
                        sqlBuilder.AppendFormat(" [title] LIKE '%{0}%' OR", RegEsc(titlename));
                }
                sqlBuilder = sqlBuilder.Remove(sqlBuilder.Length - 3, 3).Append(")");
            }

            if (moderatorName != "")
            {
                string logtidlist = "";
                DataTable dt = DatabaseProvider.GetInstance().GetModeratorLogByName(moderatorName);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        logtidlist += dr["tid"].ToString() + ",";
                    }
                    sqlBuilder.AppendFormat(" AND tid IN ({0}) ", logtidlist.Substring(0, logtidlist.Length - 1));
                }
            }

            if (postDateTimeStart.ToString().IndexOf("1900") < 0)
                sqlBuilder.AppendFormat(" AND [postdatetime]>='{0}'", postDateTimeStart.ToString("yyyy-MM-dd HH:mm:ss"));

            if (postDateTimeEnd.ToString().IndexOf("1900") < 0)
                sqlBuilder.AppendFormat(" AND [postdatetime]<='{0}'", postDateTimeEnd.ToString("yyyy-MM-dd HH:mm:ss"));

            if ((delDateTimeStart.ToString().IndexOf("1900") < 0) && (delDateTimeStart.ToString().IndexOf("1900") < 0))
            {
                string logtidlist2 = "";
                DataTable dt = DatabaseProvider.GetInstance().GetModeratorLogByPostDate(delDateTimeStart, delDateTimeStart);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        logtidlist2 += dr["title"].ToString() + ",";
                    }
                    sqlBuilder.AppendFormat(" AND tid IN ({0}) ", logtidlist2.Substring(0, logtidlist2.Length - 1));
                }
            }
            return sqlBuilder.ToString();
        }

        public void AddBBCCode(int available, string tag, string icon, string replacement, string example, string explanation, string param, string nest, string paramsDescript, string paramsDefvalue)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@available", (DbType)SqlDbType.Int, 4, available),
				DbHelper.MakeInParam("@tag", (DbType)SqlDbType.VarChar, 100, tag),
				DbHelper.MakeInParam("@icon", (DbType)SqlDbType.VarChar,50, icon),
				DbHelper.MakeInParam("@replacement", (DbType)SqlDbType.NText,0, replacement),
				DbHelper.MakeInParam("@example", (DbType)SqlDbType.NVarChar, 255, example),
				DbHelper.MakeInParam("@explanation", (DbType)SqlDbType.NText, 0, explanation),
				DbHelper.MakeInParam("@params", (DbType)SqlDbType.Int, 4, param),
				DbHelper.MakeInParam("@nest", (DbType)SqlDbType.Int, 4, nest),
				DbHelper.MakeInParam("@paramsdescript", (DbType)SqlDbType.NText, 0, paramsDescript),
				DbHelper.MakeInParam("@paramsdefvalue", (DbType)SqlDbType.NText, 0, paramsDefvalue)
			};
            string commandText = string.Format("INSERT INTO [{0}bbcodes] ([available],[tag],[icon],[replacement],[example],[explanation],[params],[nest],[paramsdescript],[paramsdefvalue]) VALUES(@available,@tag,@icon,@replacement,@example,@explanation,@params,@nest,@paramsdescript,@paramsdefvalue)",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }


        /// <summary>
        /// 产生附件
        /// </summary>
        /// <param name="attachmentinfo">附件描述类实体</param>
        /// <returns>附件id</returns>
        public int CreateAttachment(AttachmentInfo attachmentInfo)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,attachmentInfo.Uid),
                                        DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,attachmentInfo.Tid),
                                        DbHelper.MakeInParam("@pid",(DbType)SqlDbType.Int,4,attachmentInfo.Pid),
                                        DbHelper.MakeInParam("@postdatetime",(DbType)SqlDbType.DateTime, 8, DateTime.Parse(attachmentInfo.Postdatetime)),
                                        DbHelper.MakeInParam("@readperm",(DbType)SqlDbType.Int,4,attachmentInfo.Readperm),
                                        DbHelper.MakeInParam("@filename",(DbType)SqlDbType.VarChar,100,attachmentInfo.Filename),
                                        DbHelper.MakeInParam("@description",(DbType)SqlDbType.VarChar,100,attachmentInfo.Description),
                                        DbHelper.MakeInParam("@filetype",(DbType)SqlDbType.VarChar,50,attachmentInfo.Filetype),
                                        DbHelper.MakeInParam("@filesize",(DbType)SqlDbType.Int,4,attachmentInfo.Filesize),
                                        DbHelper.MakeInParam("@attachment",(DbType)SqlDbType.VarChar,255,attachmentInfo.Attachment),
                                        DbHelper.MakeInParam("@downloads",(DbType)SqlDbType.Int,4,attachmentInfo.Downloads),
                                        DbHelper.MakeInParam("@extname",(DbType)SqlDbType.NVarChar,50,Utils.GetFileExtName(attachmentInfo.Attachment)),
                                        DbHelper.MakeInParam("@attachprice",(DbType)SqlDbType.Int,4,attachmentInfo.Attachprice),
                                        DbHelper.MakeInParam("@width",(DbType)SqlDbType.Int,4,attachmentInfo.Width),
                                        DbHelper.MakeInParam("@height",(DbType)SqlDbType.Int,4,attachmentInfo.Height),
                                        DbHelper.MakeInParam("@isimage",(DbType)SqlDbType.TinyInt,1,attachmentInfo.Isimage)
							       };
            return TypeConverter.ObjectToInt(
                                 DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                        string.Format("{0}createattachment", BaseConfigs.GetTablePrefix),
                                                        parms), -1);
        }

        /// <summary>
        /// 更新主题附件类型
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <param name="attType">附件类型,1普通附件,2为图片附件</param>
        /// <returns></returns>
        public int UpdateTopicAttachmentType(int tid, int attType)
        {
            string commandText = string.Format("UPDATE [{0}topics] SET [attachment]={1} WHERE [tid]={2}",
                                                BaseConfigs.GetTablePrefix,
                                                attType,
                                                tid);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 更新帖子附件类型
        /// </summary>
        /// <param name="pid">帖子Id</param>
        /// <param name="postTableId">所在帖子表Id</param>
        /// <param name="attType">附件类型,1普通附件,2为图片附件</param>
        /// <returns></returns>
        public int UpdatePostAttachmentType(int pid, string postTableId, int attType)
        {
            string commandText = string.Format("UPDATE [{0}posts{1}] SET [attachment]={2} WHERE [pid]={3}",
                                                BaseConfigs.GetTablePrefix,
                                                postTableId,
                                                attType,
                                                pid);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获取指定附件信息
        /// </summary>
        /// <param name="aId">附件Id</param>
        /// <returns></returns>
        public IDataReader GetAttachmentInfo(int aid)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@aid",(DbType)SqlDbType.Int,4,aid)
                                  };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure,
                                          string.Format("{0}getattachenmtinfobyaid", BaseConfigs.GetTablePrefix),
                                          parms);
        }

        /// <summary>
        /// 获得指定帖子的附件个数
        /// </summary>
        /// <param name="pid">帖子ID</param>
        /// <returns>附件个数</returns>
        public int GetAttachmentCountByPid(int pid)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@pid",(DbType)SqlDbType.Int,4,pid)
                                  };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                            string.Format("{0}getattachmentcountbypid", BaseConfigs.GetTablePrefix),
                                            parms));
        }

        /// <summary>
        /// 获得指定主题的附件个数
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>附件个数</returns>
        public int GetAttachmentCountByTid(int tid)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
                                  };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                            string.Format("{0}getattachmentcountbytid", BaseConfigs.GetTablePrefix),
                                            parms);
        }

        /// <summary>
        /// 获得指定帖子的附件
        /// </summary>
        /// <param name="pid">帖子ID</param>
        /// <returns>帖子信息</returns>
        public DataTable GetAttachmentListByPid(int pid)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@pid",(DbType)SqlDbType.Int,4,pid)
                                  };
            DataSet ds = DbHelper.ExecuteDataset(CommandType.StoredProcedure,
                                                 string.Format("{0}getattachmentlistbypid", BaseConfigs.GetTablePrefix),
                                                 parms);
            return (ds != null && ds.Tables.Count > 0) ? ds.Tables[0] : new DataTable();
        }

        /// <summary>
        /// 获得系统设置的附件类型
        /// </summary>
        /// <returns>系统设置的附件类型</returns>
        public DataTable GetAttachmentType()
        {
            DataSet ds = DbHelper.ExecuteDatasetInMasterDB(CommandType.Text, string.Format("SELECT [id], [extension], [maxsize] FROM [{0}attachtypes]",
                                                 BaseConfigs.GetTablePrefix));
            return (ds != null) ? ds.Tables[0] : new DataTable();
        }

        /// <summary>
        /// 更新附件下载次数
        /// </summary>
        /// <param name="aId">附件id</param>
        public void UpdateAttachmentDownloads(int aid)
        {
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateattachmentdownloads", BaseConfigs.GetTablePrefix),
                                     DbHelper.MakeInParam("@aid", (DbType)SqlDbType.Int, 4, aid));
        }

        /// <summary>
        /// 更新主题是否包含附件
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <param name="hasAttachment">是否包含附件,0不包含,1包含</param>
        /// <returns></returns>
        public int UpdateTopicAttachment(int tid, int hasAttachment)
        {
            string commandText = string.Format("UPDATE [{0}topics] SET [attachment]={1} WHERE [tid]={2}",
                                                BaseConfigs.GetTablePrefix,
                                                hasAttachment,
                                                tid);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获得指定主题的所有附件
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <returns></returns>
        public IDataReader GetAttachmentListByTid(int tid)
        {
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getattachmentlistbytid", BaseConfigs.GetTablePrefix),
                                          DbHelper.MakeInParam("@tid", (DbType)SqlDbType.VarChar, 500, tid));
        }

        /// <summary>
        /// 获得指定主题的所有附件
        /// </summary>
        /// <param name="tidlist">主题Id列表，以英文逗号分割</param>
        /// <returns></returns>
        public IDataReader GetAttachmentListByTid(string tidList)
        {
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getattachmentlistbytidlist", BaseConfigs.GetTablePrefix),
                                          DbHelper.MakeInParam("@tidlist", (DbType)SqlDbType.VarChar, 500, tidList));
        }

        /// <summary>
        /// 获取指定用户的所有附件
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public IDataReader GetAttachmentListByUid(int uid, int days)
        {
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getattachmentlistbyuid", BaseConfigs.GetTablePrefix),
                                          DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
                                          DbHelper.MakeInParam("@days", (DbType)SqlDbType.Int, 4, days)
                                          );
        }

        //public IDataReader GetUnusedAttachmentListByUid(int uid)
        //{
        //    return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getunusedattachmentlistbyuid", BaseConfigs.GetTablePrefix),
        //                                  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid));
        //}

        /// <summary>
        /// 删除指定主题的所有附件
        /// </summary>
        /// <param name="tid">版块tid</param>
        /// <returns>删除个数</returns>
        public int DeleteAttachmentByTid(int tid)
        {
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}deleteattachmentbytid", BaseConfigs.GetTablePrefix),
                                            DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid));
        }

        /// <summary>
        /// 删除指定主题的所有附件
        /// </summary>
        /// <param name="tid">主题Id列表，以英文逗号分割</param>
        /// <returns>删除个数</returns>
        public int DeleteAttachmentByTid(string tidList)
        {
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}deleteattachmentbytidlist", BaseConfigs.GetTablePrefix),
                                            DbHelper.MakeInParam("@tidlist", (DbType)SqlDbType.VarChar, 500, tidList));
        }

        /// <summary>
        /// 删除指定附件
        /// </summary>
        /// <param name="aId"></param>
        /// <returns></returns>
        public int DeleteAttachment(int aid)
        {
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}deleteattachmentbyaid", BaseConfigs.GetTablePrefix),
                                            DbHelper.MakeInParam("@aid", (DbType)SqlDbType.Int, 4, aid));
        }

        /// <summary>
        /// 批量删除附件
        /// </summary>
        /// <param name="aidList">附件Id列表，以英文逗号分割</param>
        /// <returns></returns>
        public int DeleteAttachment(string aidList)
        {
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}deleteattachmentbyaidlist", BaseConfigs.GetTablePrefix),
                                            DbHelper.MakeInParam("@aidlist", (DbType)SqlDbType.VarChar, 500, aidList));
        }

        public int UpdatePostAttachment(int pid, string postTableId, int hasAttachment)
        {
            string commandText = string.Format("UPDATE [{0}posts{1}] SET [attachment]={2} WHERE [pid]={3}",
                                                BaseConfigs.GetTablePrefix,
                                                postTableId,
                                                hasAttachment,
                                                pid);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 根据帖子Id删除附件
        /// </summary>
        /// <param name="pid">帖子Id</param>
        /// <returns></returns>
        public int DeleteAttachmentByPid(int pid)
        {
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}deleteattachmentbypid", BaseConfigs.GetTablePrefix),
                                            DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 4, pid));
        }

        /// <summary>
        /// 更新附件信息
        /// </summary>
        /// <param name="attachmentInfo">附件对象</param>
        /// <returns>返回被更新的数量</returns>
        public int UpdateAttachment(AttachmentInfo attachmentInfo)
        {
            DbParameter[] parms = {
										DbHelper.MakeInParam("@aid",(DbType)SqlDbType.Int, 4,attachmentInfo.Aid),
										DbHelper.MakeInParam("@postdatetime",(DbType)SqlDbType.DateTime, 8, DateTime.Parse(attachmentInfo.Postdatetime)),
										DbHelper.MakeInParam("@readperm", (DbType)SqlDbType.Int, 4, attachmentInfo.Readperm),
										DbHelper.MakeInParam("@filename", (DbType)SqlDbType.NChar, 100, attachmentInfo.Filename),
										DbHelper.MakeInParam("@description", (DbType)SqlDbType.NChar, 100, attachmentInfo.Description),
										DbHelper.MakeInParam("@filetype", (DbType)SqlDbType.NChar, 50, attachmentInfo.Filetype),
										DbHelper.MakeInParam("@filesize", (DbType)SqlDbType.Int, 4, attachmentInfo.Filesize),
										DbHelper.MakeInParam("@attachment", (DbType)SqlDbType.NChar, 255, attachmentInfo.Attachment),
										DbHelper.MakeInParam("@downloads", (DbType)SqlDbType.Int, 4, attachmentInfo.Downloads),
                                        DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, attachmentInfo.Tid),
                                        DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 4, attachmentInfo.Pid),
                                        DbHelper.MakeInParam("@attachprice",(DbType)SqlDbType.Int,4,attachmentInfo.Attachprice),
                                        DbHelper.MakeInParam("@width",(DbType)SqlDbType.Int,4,attachmentInfo.Width),
                                        DbHelper.MakeInParam("@height",(DbType)SqlDbType.Int,4,attachmentInfo.Height),
                                        DbHelper.MakeInParam("@isimage",(DbType)SqlDbType.TinyInt,1,attachmentInfo.Isimage)
								   };

            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                            string.Format("{0}updateallfieldattachmentinfo", BaseConfigs.GetTablePrefix),
                                            parms);
        }


        public IDataReader GetAttachmentList(string aidList)
        {
            return DbHelper.ExecuteReader(CommandType.StoredProcedure,
                                          string.Format("{0}getattachmentlistbyaid", BaseConfigs.GetTablePrefix),
                                          DbHelper.MakeInParam("@aidlist", (DbType)SqlDbType.VarChar, 500, aidList));
        }

        public IDataReader GetAttachmentListByPid(string pidList)
        {
            return DbHelper.ExecuteReader(CommandType.StoredProcedure,
                                          string.Format("{0}getattachmentlistbypidlist", BaseConfigs.GetTablePrefix),
                                          DbHelper.MakeInParam("@pidlist", (DbType)SqlDbType.VarChar, 500, pidList));
        }

        /// <summary>
        /// 获得上传附件文件的大小
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public int GetUploadFileSizeByUserId(int uid)
        {
            return TypeConverter.ObjectToInt(
                        DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                               string.Format("{0}gettodayuploadedfilesize", BaseConfigs.GetTablePrefix),
                                               DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)));
        }

        /// <summary>
        /// 取得主题帖的第一个图片附件
        /// </summary>
        /// <param name="tid">主题id</param>
        public IDataReader GetFirstImageAttachByTid(int tid)
        {
            return DbHelper.ExecuteReader(CommandType.StoredProcedure,
                                          string.Format("{0}getfirstimageattachbytid", BaseConfigs.GetTablePrefix),
                                          DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid));
        }

        public void AddAttchType(string extension, string maxsize)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@extension", (DbType)SqlDbType.VarChar,256, extension),
				DbHelper.MakeInParam("@maxsize", (DbType)SqlDbType.Int, 4, maxsize)
			};
            string commandText = string.Format("INSERT INTO [{0}attachtypes] ([extension], [maxsize]) VALUES (@extension,@maxsize)",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }



        public void UpdateAttchType(string extension, string maxSize, int id)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@extension", (DbType)SqlDbType.VarChar,256, extension),
				DbHelper.MakeInParam("@maxsize", (DbType)SqlDbType.Int, 4, maxSize),
				DbHelper.MakeInParam("@id",(DbType)SqlDbType.Int, 4,id)
			};
            string commandText = string.Format("UPDATE [{0}attachtypes] SET [extension]=@extension ,[maxsize]=@maxsize WHERE [id]=@id",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void DeleteAttchType(string attchTypeIdList)
        {
            string commandText = string.Format("DELETE FROM [{0}attachtypes] WHERE [id] IN ({1})", BaseConfigs.GetTablePrefix, attchTypeIdList);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }


        public DataTable GetModeratorLogByName(string moderatorName)
        {
            string commandText = string.Format("SELECT [tid] FROM [{0}moderatormanagelog] WHERE ([moderatorname] = '{1}') AND ([actions] = 'DELETE')",
                                                BaseConfigs.GetTablePrefix,
                                                moderatorName);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public DataTable GetModeratorLogByPostDate(DateTime startDateTime, DateTime endDateTime)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@startDateTime", (DbType)SqlDbType.DateTime, 8, startDateTime),
                                        DbHelper.MakeInParam("@endDateTime", (DbType)SqlDbType.DateTime, 8, endDateTime)
            };
            string commandText = string.Format("SELECT [title] FROM [{0}moderatormanagelog] WHERE (postdatetime >= @startDateTime) AND ([postdatetime]<= @endDateTime) AND ([actions] = 'DELETE')",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        public DataTable GetModeratorLogByPostDate(DateTime postDateTime)
        {
            string commandText = string.Format("SELECT [tid] FROM [{0}topics] WHERE [displayorder]=-1 AND [postdatetime]<=@postdatetime",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText,
                                           DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, postDateTime)).Tables[0];
        }


        //public DataTable GetUnauditNewTopic()
        //{
        //    return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getunauditnewtopic", BaseConfigs.GetTablePrefix)).Tables[0];
        //}

        public IDataReader GetUnauditNewTopic(string fidList, int tpp, int pageId, int filter)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@fidlist", (DbType)SqlDbType.VarChar, 255, fidList),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, filter),
                                        DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int, 4, tpp),
                                        DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int, 4, pageId)
                                  };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getunauditnewtopicbycondition", BaseConfigs.GetTablePrefix), parms);
        }

        public int GetUnauditNewTopicCount(string fidList, int filter)
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}topics] WHERE [displayorder]=@filter {1}",
                                                BaseConfigs.GetTablePrefix,
                                                fidList != "0" ? " AND fid IN (" + fidList + ")" : "");
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalarToStr(CommandType.Text, commandText, DbHelper.MakeInParam("@filter", (DbType)SqlDbType.Int, 4, filter)));
        }


        public IDataReader GetUnauditNewPost(string fidList, int ppp, int pageId, int tableId, int filter)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@fidlist", (DbType)SqlDbType.VarChar, 255, fidList),
                                        DbHelper.MakeInParam("@tableid",(DbType)SqlDbType.Int,4,tableId),
                                        DbHelper.MakeInParam("@filter", (DbType)SqlDbType.Int, 4, filter),
                                        DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int, 4, ppp),
                                        DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int, 4, pageId),
                                  };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getUnAuditPostList", BaseConfigs.GetTablePrefix), parms);
        }

        public int GetUnauditNewPostCount(string fidList, int tableId, int filter)
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}posts{1}] WHERE [invisible]=@filter AND [layer]>0 {2}",
                                                BaseConfigs.GetTablePrefix,
                                                tableId,
                                                fidList != "0" ? " AND fid IN (" + fidList + ")" : "");
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalarToStr(CommandType.Text, commandText, DbHelper.MakeInParam("@filter", (DbType)SqlDbType.Int, 4, filter)));
        }

        /// <summary>
        /// 设置待验证的主题,包括通过,忽略,删除等操作
        /// </summary>
        /// <param name="tableId">回复表ID</param>
        /// <param name="ignore">忽略的主题列表</param>
        /// <param name="validate">验证通过的主题列表</param>
        /// <param name="delete">删除的主题列表</param>
        /// <param name="fidList">版块列表</param>
        public void PassAuditNewTopic(string tableId, string ignore, string validate, string delete, string fidList)
        {
            if (!Utils.StrIsNullOrEmpty(validate))
            {
                PassAuditNewTopic(string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, tableId), validate);
                //DbHelper.ExecuteNonQuery(string.Format("UPDATE  [{0}topics]  SET [displayorder]=0,[lastpostid]=(SELECT TOP 1 [pid] FROM [{0}posts{2}] WHERE [tid]=[{0}topics].[tid] ORDER BY [pid] DESC) WHERE [tid] IN ({1})",
                //                                         BaseConfigs.GetTablePrefix,
                //                                         validate, tableId));
                //DbHelper.ExecuteNonQuery(string.Format("UPDATE  [{0}posts{1}]  SET [invisible]=0 WHERE [layer]=0  AND [tid] IN({2})",
                //                                         BaseConfigs.GetTablePrefix,
                //                                         tableId,
                //                                         validate));
                //foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT {0} FROM [{1}topics] WHERE [tid] IN({2}) ORDER BY [tid] ASC", DbFields.TOPICS, BaseConfigs.GetTablePrefix, validate)).Tables[0].Rows)
                //{
                //    DbHelper.ExecuteNonQuery(CommandType.Text,
                //        string.Format("UPDATE [{0}forums] SET [topics] = [topics] + 1, [curtopics] = [curtopics] + 1, [posts]=[posts] + 1, [todayposts]=CASE WHEN DATEPART(yyyy, [lastpost])=DATEPART(yyyy,GETDATE()) AND DATEPART(mm,[lastpost])=DATEPART(mm,GETDATE()) AND DATEPART(dd, [lastpost])=DATEPART(dd,GETDATE()) THEN [todayposts]*1 + 1	ELSE 1 END,[lasttid]={1} ,	[lasttitle]='{2}',[lastpost]='{3}',[lastposter]='{4}',[lastposterid]={5}  WHERE [fid]={6}",
                //                                             BaseConfigs.GetTablePrefix,
                //                                             dr["tid"],
                //                                             dr["title"].ToString().Replace("'", "''"),
                //                                             dr["postdatetime"],
                //                                             dr["poster"].ToString().Replace("'", "''"),
                //                                             dr["posterid"],
                //                                             dr["fid"]));
                //    DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [lastpost] = '{1}', [lastpostid] ={2}, [lastposttitle] ='{3}', [posts] = [posts] + 1	WHERE [uid] = {4}",
                //                                             BaseConfigs.GetTablePrefix,
                //                                             dr["postdatetime"],
                //                                             dr["posterid"],
                //                                             dr["title"].ToString().Replace("'", "''"),
                //                                             dr["posterid"]));
                //}
            }

            if (!Utils.StrIsNullOrEmpty(delete))
            {
                DbHelper.ExecuteNonQuery(string.Format("DELETE  FROM  [{0}topics]  WHERE [tid] IN ({1})",
                                                        BaseConfigs.GetTablePrefix,
                                                        delete));
                DbHelper.ExecuteNonQuery(string.Format("DELETE  FROM  [{0}posts{1}]  WHERE [layer]=0  AND [tid] IN ({2})",
                                                        BaseConfigs.GetTablePrefix,
                                                        tableId,
                                                        delete));
            }

            if (!Utils.StrIsNullOrEmpty(ignore))
                DbHelper.ExecuteNonQuery(string.Format("UPDATE  [{0}topics]  SET [displayorder]=-3 WHERE [tid] IN ({1})",
                                         BaseConfigs.GetTablePrefix,
                                         ignore));
        }

        public int GetModTopicCountByTidList(string fidList, string tidList)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@fidlist", (DbType)SqlDbType.NChar, 500, fidList),
                                        DbHelper.MakeInParam("@tidlist", (DbType)SqlDbType.NChar, 500, tidList)
                                  };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalarToStr(CommandType.StoredProcedure, string.Format("{0}getmodtopiccountbytidlist", BaseConfigs.GetTablePrefix), parms));
        }

        public void PassAuditNewTopic(string postTableName, string tidList)
        {
            if (!Utils.IsNumericList(tidList))
                return;

            postTableName = Utils.ChkSQL(postTableName);

            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE  [{0}]  SET [invisible]=0 WHERE [layer]=0  AND [tid] IN({1})",
                                                                      postTableName,
                                                                      tidList));
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE  [{0}topics]  SET [displayorder]=0,[lastpostid]=(SELECT TOP 1 [pid] FROM [{2}] WHERE [tid]=[{0}topics].[tid] ORDER BY [pid] DESC) WHERE [tid] IN({1})",
                                                                      BaseConfigs.GetTablePrefix,
                                                                      tidList, postTableName));
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE  [{0}statistics] SET [totaltopic]=[totaltopic] + {1}",
                                                                      BaseConfigs.GetTablePrefix,
                                                                      tidList.Split(',').Length));
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE  [{0}statistics] SET [totalpost]=[totalpost] + {1}",
                                                                      BaseConfigs.GetTablePrefix,
                                                                      tidList.Split(',').Length));

            //更新相关的版块统计信息
            foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT {2} FROM [{0}topics] WHERE [tid] IN({1}) ORDER BY [tid] ASC", BaseConfigs.GetTablePrefix, tidList, DbFields.TOPICS)).Tables[0].Rows)
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}forums] SET [topics] = [topics] + 1, [curtopics] = [curtopics] + 1, [posts]=[posts] + 1, [todayposts]=CASE WHEN DATEPART(yyyy, [lastpost])=DATEPART(yyyy,GETDATE()) AND DATEPART(mm,[lastpost])=DATEPART(mm,GETDATE()) AND DATEPART(dd, [lastpost])=DATEPART(dd,GETDATE()) THEN [todayposts]*1 + 1	ELSE 1 END,[lasttid]={1} ,	[lasttitle]='{2}',[lastpost]='{3}',[lastposter]='{4}',[lastposterid]={5}  WHERE [fid]={6}",
                                                           BaseConfigs.GetTablePrefix,
                                                           dr["tid"].ToString(),
                                                           dr["title"].ToString().Replace("'", "''"),
                                                           dr["postdatetime"].ToString(),
                                                           dr["poster"].ToString().Replace("'", "''"),
                                                           dr["posterid"].ToString(),
                                                           dr["fid"].ToString()));
                DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [lastpost] = '{1}', [lastpostid] ={2}, [lastposttitle] ='{3}', [posts] = [posts] + 1 WHERE [uid] = {4}",
                                                           BaseConfigs.GetTablePrefix,
                                                           dr["postdatetime"].ToString(),
                                                           dr["posterid"].ToString(),
                                                           dr["title"].ToString().Replace("'", "''"),
                                                           dr["posterid"].ToString()));
            }
        }

        //public DataTable GetUnauditPost(int currentPostTableId)
        //{
        //    string commandText = string.Format("SELECT {0} FROM [{1}posts{2}] WHERE ([invisible]=1 OR [invisible]=-3) AND [layer]>0 ORDER BY [pid] DESC",
        //                                        DbFields.POSTS,
        //                                        BaseConfigs.GetTablePrefix,
        //                                        currentPostTableId);
        //    return DbHelper.ExecuteDataset(commandText).Tables[0];
        //}


        public void AuditPost(int tableId, string validate, string delete, string ignore, string fidList)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@validate", (DbType)SqlDbType.VarChar, 100, validate),
                DbHelper.MakeInParam("@ignore", (DbType)SqlDbType.VarChar, 100, ignore),
                DbHelper.MakeInParam("@delete", (DbType)SqlDbType.VarChar,100, delete)
            };
            if (!Utils.StrIsNullOrEmpty(validate))
            {
                //更新帖子显示状态为显示
                //DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}posts{1}] SET [invisible]=0 WHERE [pid] IN ({2})",
                //                                                  BaseConfigs.GetTablePrefix,
                //                                                  tableId,
                //                                                  validate), parms);
                PassPost(tableId, validate);
            }
            else if (!Utils.StrIsNullOrEmpty(delete))
                DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}posts{1}] WHERE [pid] IN ({2})",
                                                                  BaseConfigs.GetTablePrefix,
                                                                  tableId,
                                                                  delete), parms);
            else if (!Utils.StrIsNullOrEmpty(ignore))
                DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}posts{1}] SET [invisible]=-3 WHERE  [pid] IN ({2})",
                                                                  BaseConfigs.GetTablePrefix,
                                                                  tableId,
                                                                  ignore), parms);
        }

        public int GetModPostCountByPidList(string fidList, string postTableId, string pidList)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@fidlist", (DbType)SqlDbType.NVarChar, 500, fidList),
                                        DbHelper.MakeInParam("@postTableId",(DbType)SqlDbType.NVarChar, 5, postTableId),
                                        DbHelper.MakeInParam("@pidList", (DbType)SqlDbType.NVarChar, 500, pidList)
                                  };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalarToStr(CommandType.StoredProcedure, string.Format("{0}getmodpostcountbypidlist", BaseConfigs.GetTablePrefix), parms));
        }

        public void PassPost(int currentPostTableId, string pidList)
        {
            if (!Utils.IsNumericList(pidList))
                return;
            int postcount = pidList.Split(',').Length;//待审核帖子的数量
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@currentPostTableId",(DbType)SqlDbType.Int,4,currentPostTableId),
                                    DbHelper.MakeInParam("@postcount",(DbType)SqlDbType.Int,4,postcount),
                                    DbHelper.MakeInParam("@pidList",(DbType)SqlDbType.NVarChar,500,pidList)
                                  };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}passpost", BaseConfigs.GetTablePrefix), parms);
        }

        public DataTable GetPostLayer(int currentPostTableId, int postId)
        {
            string commandText = string.Format("SELECT TOP 1 [layer],[tid]  FROM [{0}posts{1}] WHERE [pid]={2}",
                                        BaseConfigs.GetTablePrefix,
                                        currentPostTableId,
                                        postId);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public void UpdateBBCCode(int available, string tag, string icon, string replacement, string example, string explanation, string param, string nest, string paramsDescript, string paramsDefvalue, int id)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@available", (DbType)SqlDbType.Int, 4, available),
				DbHelper.MakeInParam("@tag", (DbType)SqlDbType.VarChar, 100, tag),
				DbHelper.MakeInParam("@icon", (DbType)SqlDbType.VarChar,50, icon),
				DbHelper.MakeInParam("@replacement", (DbType)SqlDbType.NText,0, replacement),
				DbHelper.MakeInParam("@example", (DbType)SqlDbType.NVarChar, 255, example),
				DbHelper.MakeInParam("@explanation", (DbType)SqlDbType.NText, 0, explanation),
				DbHelper.MakeInParam("@params", (DbType)SqlDbType.Int, 4, param),
				DbHelper.MakeInParam("@nest", (DbType)SqlDbType.Int, 4, nest),
				DbHelper.MakeInParam("@paramsdescript", (DbType)SqlDbType.NText, 0, paramsDescript),
				DbHelper.MakeInParam("@paramsdefvalue", (DbType)SqlDbType.NText, 0, paramsDefvalue),
				DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id)
			};
            string commandText = string.Format("UPDATE [{0}bbcodes] SET [available]=@available,tag=@tag, icon=@icon,replacement=@replacement,example=@example,explanation=@explanation,params=@params,nest=@nest,paramsdescript=@paramsdescript,paramsdefvalue=@paramsdefvalue WHERE [id]=@id",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public DataTable GetBBCode()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("Select {0} From [{1}bbcodes] Order BY [id] ASC",
                                                             DbFields.BBCODES,
                                                             BaseConfigs.GetTablePrefix)).Tables[0];
        }

        /// <summary>
        /// 注：该方法与GetBBCCodeById方法可以合并
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetBBCode(int id)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT {0} FROM [{1}bbcodes] WHERE [id]={2}",
                                                             DbFields.BBCODES,
                                                             BaseConfigs.GetTablePrefix,
                                                             id)).Tables[0];
        }


        public void DeleteBBCode(string idList)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}bbcodes]  WHERE [id] IN ({1})",
                                                              BaseConfigs.GetTablePrefix,
                                                              idList));
        }

        public void SetBBCodeAvailableStatus(string idList, int status)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}bbcodes] SET [available]={1} WHERE [id] IN ({2})",
                                                              BaseConfigs.GetTablePrefix,
                                                              status,
                                                              idList));
        }

        /// <summary>
        /// 获取关注主题列表
        /// </summary>
        /// <param name="count">数量</param>
        /// <param name="views">最小浏览量</param>
        /// <param name="fid">板块ID</param>
        /// <param name="typeidlist">主题分类ID</param>
        /// <param name="startDateTime">起始时间</param>
        /// <param name="orderfieldname">排序字段</param>
        /// <param name="visibleForum">板块范围(逗号分隔)</param>
        /// <param name="isdigest">是否精华</param>
        /// <param name="onlyimg">是否仅取带有图片附件的帖子</param>
        /// <returns></returns>
        public DataTable GetFocusTopicList(int count, int views, int fid, string typeIdList, string startTime, string orderFieldName, string visibleForum, bool isDigest, bool onlyImg)
        {
            DbParameter param = DbHelper.MakeInParam("@starttime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(startTime));

            string digestParam = isDigest ? " AND [t].[digest] > 0" : "";

            string fidParam = (fid <= 0 ? "" : string.Format(" AND ([t].[fid] = {0} OR CHARINDEX(',{0},' , ',' + RTRIM([f].[parentidlist]) + ',') > 0 ) ", fid));

            if (count < 0)
                count = 0;

            string attParam = onlyImg ? " AND [t].[attachment]=2" : "";

            if (!Utils.StrIsNullOrEmpty(visibleForum))
                visibleForum = string.Format(" AND [t].[fid] IN ({0})", visibleForum);

            string typeidParam = "";
            if (!string.IsNullOrEmpty(typeIdList) && Utils.IsNumericList(typeIdList))
            {
                typeidParam = string.Format(" AND [t].[typeid] IN ({0})", typeIdList);
            }

            string sqlstr = string.Format(@"SELECT TOP {0} {1}, [f].[name], [ff].[rewritename] FROM [{2}topics] AS [t] LEFT JOIN [{2}forums] AS [f] ON [t].[fid] = [f].[fid] LEFT JOIN [{2}forumfields] ff ON [f].[fid] = [ff].[fid] WHERE [t].[closed]<>1 AND  [t].[displayorder] >=0 AND [t].[views] > {3} AND [t].[postdatetime] > @starttime{4}{5} AND [ff].[password] ='' ORDER BY [t].[{6}] DESC",
                                   count,
                                   DbFields.TOPICS_JOIN,
                                   BaseConfigs.GetTablePrefix,
                                   views,
                                   fidParam + digestParam + visibleForum + typeidParam,
                                   attParam,
                                   orderFieldName);

            return DbHelper.ExecuteDataset(CommandType.Text, sqlstr, param).Tables[0];
        }

        public void UpdateTopicLastPoster(int lastPosterId, string lastPoster)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@lastposter", (DbType)SqlDbType.VarChar, 20, lastPoster),
                                        DbHelper.MakeInParam("@lastposterid", (DbType)SqlDbType.Int, 4, lastPosterId)
                                    };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}getunauditpost", BaseConfigs.GetTablePrefix), parms);
        }

        public void UpdateTopicPoster(int posterId, string poster)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.Int, 4, posterId),
                                        DbHelper.MakeInParam("@poster", (DbType)SqlDbType.VarChar, 20, poster)
                                    };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updatetopicposter", BaseConfigs.GetTablePrefix), parms);
        }

        public void UpdatePostPoster(int posterId, string poster)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.Int, 4, posterId),
                                        DbHelper.MakeInParam("@poster", (DbType)SqlDbType.VarChar, 20, poster)
                                    };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updatepostposter", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 根据主题ID删除相应的主题信息
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public bool DeleteTopicByTid(int tid, string postTableName)
        {
            SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
            conn.Open();
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    DbHelper.ExecuteNonQuery(trans, string.Format("DELETE FROM [{0}attachments] WHERE [tid]={1}", BaseConfigs.GetTablePrefix, tid));
                    DbHelper.ExecuteNonQuery(trans, string.Format("DELETE FROM [{0}favorites] WHERE [tid]={1}", BaseConfigs.GetTablePrefix, tid));
                    DbHelper.ExecuteNonQuery(trans, string.Format("DELETE FROM [{0}polls] WHERE [tid]={1}", BaseConfigs.GetTablePrefix, tid));
                    DbHelper.ExecuteNonQuery(trans, string.Format("DELETE FROM [{0}] WHERE [tid]={1}", postTableName, tid));
                    DbHelper.ExecuteNonQuery(trans, string.Format("DELETE FROM [{0}topics] WHERE [tid]={1}", BaseConfigs.GetTablePrefix, tid));
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

        public bool SetTypeid(string topicList, int value)
        {
            SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
            conn.Open();
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    DbHelper.ExecuteNonQuery(trans, string.Format("UPDATE [{0}topics] SET [typeid]={1}  WHERE [tid] IN ({2})",
                                                                   BaseConfigs.GetTablePrefix,
                                                                   value,
                                                                   topicList));
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

        public DataSet GetPosts(int tid, int pageSize, int pageIndex, string postTableId)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid),
				DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int, 4, pageSize),
				DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int, 4, pageIndex)
			};
            return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getpostlist{1}", BaseConfigs.GetTablePrefix, postTableId), parms);
        }


        public bool SetDisplayorder(string topicList, int value)
        {
            string commandText = string.Format("UPDATE [{0}topics] SET [displayorder]={1} WHERE [tid] IN ({2})",
                                                BaseConfigs.GetTablePrefix,
                                                value,
                                                topicList);
            DbHelper.ExecuteNonQuery(commandText);
            return true;
        }

        /// <summary>
        /// 添加评分记录
        /// </summary>
        /// <param name="postidlist">被评分帖子pid</param>
        /// <param name="userId">评分者uid</param>
        /// <param name="username">评分者用户名</param>
        /// <param name="extid">分的积分类型</param>
        /// <param name="score">积分数值</param>
        /// <param name="reason">评分理由</param>
        /// <returns>更新数据行数</returns>
        public int InsertRateLog(int pid, int userId, string userName, int extId, float score, string reason)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 4, pid),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userId),
				DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar, 20, userName),
				DbHelper.MakeInParam("@extcredits", (DbType)SqlDbType.TinyInt, 1, extId),
				DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, DateTime.Now),
				DbHelper.MakeInParam("@score", (DbType)SqlDbType.SmallInt, 2, score),
				DbHelper.MakeInParam("@reason", (DbType)SqlDbType.NVarChar, 50, reason)
			};

            string CommandText = string.Format("INSERT INTO [{0}ratelog] ([pid],[uid],[username],[extcredits],[postdatetime],[score],[reason]) VALUES (@pid,@uid,@username,@extcredits,@postdatetime,@score,@reason)",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, CommandText, parms);
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <returns></returns>
        public bool DeleteRateLog()
        {
            try
            {
                if (DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}ratelog]", BaseConfigs.GetTablePrefix)) > 1)
                    return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="pid"></param>
        public void DeleteRateLog(int pid)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}ratelog] WHERE [pid]={1}",
                                                                      BaseConfigs.GetTablePrefix,
                                                                      pid));
        }

        /// <summary>
        /// 按指定条件删除日志
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public bool DeleteRateLog(string condition)
        {
            try
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}ratelog] WHERE {1}",
                                                                          BaseConfigs.GetTablePrefix,
                                                                          condition));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 得到当前指定页数的评分日志记录(表)
        /// </summary>
        /// <param name="pagesize">当前分页的尺寸大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <returns></returns>
        public DataTable RateLogList(int pageSize, int currentPage, string postTableName)
        {
            int pagetop = (currentPage - 1) * pageSize;
            string commandText = "SELECT TOP {0} {1}  FROM [{2}ratelog] ";

            if (currentPage == 1)
                commandText += "ORDER BY [id] DESC";
            else
                commandText += "WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [{2}ratelog] ORDER BY [id] DESC) AS tblTmp )  ORDER BY [id] DESC";

            commandText = "SELECT {3} ,p.[title] AS title,p.[poster] AS poster , p.[posterid] AS posterid,  ug.[grouptitle] AS grouptitle FROM (" + commandText + ") r LEFT JOIN [" + postTableName + "] p ON r.[pid] = p.[pid] LEFT JOIN [{2}users] u ON u.[uid] = r.[uid] LEFT JOIN [{2}usergroups] ug ON ug.[groupid] = u.[groupid]";
            commandText = string.Format(commandText, pageSize, DbFields.RATE_LOG, BaseConfigs.GetTablePrefix, DbFields.RATE_LOG_JOIN);

            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 得到当前指定条件和页数的评分日志记录(表)
        /// </summary>
        /// <param name="pagesize">当前分页的尺寸大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public DataTable RateLogList(int pageSize, int currentPage, string postTableName, string condition)
        {
            int pagetop = (currentPage - 1) * pageSize;
            string commandText = "SELECT TOP {0} {1}  FROM [{2}ratelog] ";

            if (currentPage == 1)
                commandText += "WHERE {3}  ORDER BY [id] DESC";
            else
                commandText += "WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [{2}ratelog] WHERE {3} ORDER BY [id] DESC) AS tblTmp )  AND {3} ORDER BY [id] DESC";

            commandText = "SELECT {4} ,p.[title] AS title,p.[poster] AS poster , p.[posterid] AS posterid,  ug.[grouptitle] AS grouptitle FROM (" + commandText + ") r LEFT JOIN [" + postTableName + "] p ON r.[pid] = p.[pid] LEFT JOIN [{2}users] u ON u.[uid] = r.[uid] LEFT JOIN [{2}usergroups] ug ON ug.[groupid] = u.[groupid]";
            commandText = string.Format(commandText, pageSize, DbFields.RATE_LOG, BaseConfigs.GetTablePrefix, condition, DbFields.RATE_LOG_JOIN);

            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 得到评分日志记录数
        /// </summary>
        /// <returns></returns>
        public int GetRateLogCount()
        {
            string commandText = string.Format("SELECT COUNT([id]) FROM [{0}ratelog]", BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 得到指定查询条件下的评分日志数
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public int GetRateLogCount(string condition)
        {
            string commandText = string.Format("SELECT COUNT([id]) FROM [{0}ratelog] WHERE {1}",
                                                BaseConfigs.GetTablePrefix,
                                                condition);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows[0][0]);
        }

        public int GetPostsCount(string postTableId)
        {
            string commandText = string.Format("SELECT COUNT([pid]) AS [portscount] FROM [{0}posts{1}]",
                                                BaseConfigs.GetTablePrefix,
                                                postTableId);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        /// <summary>
        /// 获取指定主题下小于pid的有效帖子数
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        public int GetPostsCountBeforePid(int pid, int tid)
        {
            int postTableId = TypeConverter.StrToInt(PostTables.GetPostTableId(tid));
            DbParameter[] parms = {
                                       DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid),
                                       DbHelper.MakeInParam("@pid",(DbType)SqlDbType.Int,4,pid)
                                   };
            string commandText = string.Format("SELECT COUNT([pid]) FROM [{0}posts{1}] WHERE tid = @tid AND pid < @pid AND ([invisible]=0 OR [invisible]=-2)", BaseConfigs.GetTablePrefix, postTableId);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms));
        }

        public IDataReader GetMaxAndMinTid(int fid)
        {
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getmaxandmintid", BaseConfigs.GetTablePrefix),
                                             DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid));
        }

        public int GetPostCount(int fid, string postTableName)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int,4,fid),
									   DbHelper.MakeInParam("@posttablename", (DbType)SqlDbType.VarChar, 50, postTableName)
								   };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}getdesignatepostcount", BaseConfigs.GetTablePrefix),
                                             parms));
        }

        public int GetPostCountByTid(int tid, string postTableName)
        {
            string commandText = string.Format("SELECT COUNT([pid]) AS [postcount] FROM [{0}] WHERE [tid] = @tid AND [layer] <> 0", postTableName);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText,
                                             DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid)));
        }
        /// <summary>
        /// 根据分表名更新主题的最后回复等信息
        /// </summary>
        /// <param name="postTableName">分表名</param>
        public void ResetLastRepliesInfoOfTopics(int postTableID)
        {
            string postTableName = string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, postTableID);
            DbParameter parms = DbHelper.MakeInParam("@posttable", (DbType)SqlDbType.NVarChar, 20, postTableName);
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}resetLastRepliesInfoOftopics", BaseConfigs.GetTablePrefix), parms);
        }
        public int GetPostCount(string postTableId, int tid, int posterId)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.NChar,100,tid),
									   DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.VarChar, 20, posterId)
								   };
            string commandText = string.Format("{0}getpostcountbycondition{1}", BaseConfigs.GetTablePrefix, postTableId);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, commandText, parms));
        }

        public int GetTodayPostCount(int fid, string postTableName)
        {
            string commandText = string.Format("SELECT COUNT([pid]) AS [postcount] FROM [{0}] WHERE [fid] = {1} AND DATEDIFF(day, [postdatetime], GETDATE()) = 0",
                                                postTableName,
                                                fid);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText));
        }


        public int GetPostCountByUid(int uid, string postTableName)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.NChar,100,uid),
									   DbHelper.MakeInParam("@posttablename", (DbType)SqlDbType.VarChar, 20, postTableName)
								   };
            string commandText = string.Format("{0}getpostcountbyuid", BaseConfigs.GetTablePrefix);
            return Math.Abs(TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, commandText, parms)));
        }

        public int GetTodayPostCountByUid(int uid, string postTableName)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.NChar,100,uid),
									   DbHelper.MakeInParam("@posttablename", (DbType)SqlDbType.VarChar, 20, postTableName)
								   };
            string commandText = string.Format("{0}gettodaypostcountbyuid", BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, commandText, parms));
        }

        public int GetTopicCount()
        {
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}gettotaltopiccount", BaseConfigs.GetTablePrefix)));
        }

        public void ReSetStatistic(int userCount, int topicsCount, int postCount, string lastUserId, string lastUserName)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@totaltopic", (DbType)SqlDbType.Int, 4, topicsCount),
				DbHelper.MakeInParam("@totalpost", (DbType)SqlDbType.Int, 4, postCount),
				DbHelper.MakeInParam("@totalusers", (DbType)SqlDbType.Int, 4, userCount),
				DbHelper.MakeInParam("@lastusername", (DbType)SqlDbType.VarChar, 20, lastUserName),
				DbHelper.MakeInParam("@lastuserid", (DbType)SqlDbType.Int, 4, Utils.StrToInt(lastUserId, 0))
			};
            string commandText = string.Format("{0}resetstatistic", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, commandText, parms);
        }

        public IDataReader GetTopicTids(int statCount, int lastTid)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@lasttid", (DbType)SqlDbType.Int, 4, lastTid),
                DbHelper.MakeInParam("@statcount", (DbType)SqlDbType.Int, 4, statCount)
			};
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}gettopictids", BaseConfigs.GetTablePrefix), parms);
        }


        public void UpdateTopic(int tid, int postCount, int lastPostId, string lastPost, int lastPosterId, string poster)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid),
                                        DbHelper.MakeInParam("@postcount", (DbType)SqlDbType.Int, 4, postCount),
                                        DbHelper.MakeInParam("@lastpostid", (DbType)SqlDbType.Int, 4, lastPostId),
                                        DbHelper.MakeInParam("@lastpost", (DbType)SqlDbType.VarChar, 20, lastPost),
                                        DbHelper.MakeInParam("@lastposterid", (DbType)SqlDbType.Int, 4, lastPosterId),
                                        DbHelper.MakeInParam("@lastposter", (DbType)SqlDbType.VarChar, 20, poster)
                                    };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updatelastpostoftopic", BaseConfigs.GetTablePrefix), parms);
        }

        public void UpdateTopicLastPosterId(int tid)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid)
			};
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updatetopiclastposterid", BaseConfigs.GetTablePrefix), parms);
        }

        public IDataReader GetTopics(int startTid, int endTid)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@start_tid", (DbType)SqlDbType.Int, 4, startTid),
				DbHelper.MakeInParam("@end_tid", (DbType)SqlDbType.Int, 4, endTid)
			};
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}gettopics", BaseConfigs.GetTablePrefix), parms);
        }

        public IDataReader GetForumLastPost(int fid, string postTableName, int topicCount, int postCount, int lastTid, string lastTitle, string lastPost, int lastPosterId, string lastPoster, int todayPostCount)
        {
            DbParameter[] prams_posts = {
					DbHelper.MakeInParam("@lastfid", (DbType)SqlDbType.Int, 4, fid),
					DbHelper.MakeInParam("@topiccount", (DbType)SqlDbType.Int, 4, topicCount),
					DbHelper.MakeInParam("@postcount", (DbType)SqlDbType.Int, 4, postCount),
					DbHelper.MakeInParam("@lasttid", (DbType)SqlDbType.Int, 4, lastTid),
					DbHelper.MakeInParam("@lasttitle", (DbType)SqlDbType.NChar, 80, lastTitle),
					DbHelper.MakeInParam("@lastpost", (DbType)SqlDbType.VarChar, 20, lastPost),
					DbHelper.MakeInParam("@lastposterid", (DbType)SqlDbType.Int, 4, lastPosterId),
					DbHelper.MakeInParam("@lastposter", (DbType)SqlDbType.NChar, 20, lastPoster),
					DbHelper.MakeInParam("@todaypostcount", (DbType)SqlDbType.Int, 4, todayPostCount)
                                            };
            string commandText = string.Format("SELECT TOP 1 [tid], [title], [postdatetime], [posterid], [poster] FROM [{0}] WHERE [fid] = @lastfid OR [fid] IN (SELECT fid  FROM [{1}forums] WHERE CHARINDEX(',' + RTRIM(@lastfid) + ',', ',' + RTRIM(parentidlist) + ',') > 0) ORDER BY [pid] DESC",
                                                postTableName,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText, prams_posts);
        }

        public void UpdateForum(int fid, int topicCount, int postCount, int lastTid, string lastTitle, string lastPost, int lastPosterId, string lastPoster, int todayPostCount)
        {
            DbParameter[] prams_posts = {
					DbHelper.MakeInParam("@lastfid", (DbType)SqlDbType.Int, 4, fid),
					DbHelper.MakeInParam("@topiccount", (DbType)SqlDbType.Int, 4, topicCount),
					DbHelper.MakeInParam("@postcount", (DbType)SqlDbType.Int, 4, postCount),
					DbHelper.MakeInParam("@lasttid", (DbType)SqlDbType.Int, 4, lastTid),
					DbHelper.MakeInParam("@lasttitle", (DbType)SqlDbType.NChar, 80, lastTitle),
					DbHelper.MakeInParam("@lastpost", (DbType)SqlDbType.VarChar, 20, lastPost),
					DbHelper.MakeInParam("@lastposterid", (DbType)SqlDbType.Int, 4, lastPosterId),
					DbHelper.MakeInParam("@lastposter", (DbType)SqlDbType.NChar, 20, lastPoster),
					DbHelper.MakeInParam("@todaypostcount", (DbType)SqlDbType.Int, 4, todayPostCount)
                                            };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updateforum", BaseConfigs.GetTablePrefix), prams_posts);
        }

        public IDataReader GetForums(int startFid, int endFid)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@start_fid", (DbType)SqlDbType.Int, 4, startFid),
				DbHelper.MakeInParam("@end_fid", (DbType)SqlDbType.Int, 4, endFid)
			};
            string commandText = string.Format("SELECT  [fid] FROM [{0}forums] WHERE [fid] >= @start_fid AND [fid]<=@end_fid",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        /// <summary>
        ///  重置整个论坛所有版块的帖子数（topics and posts）
        /// </summary>
        public void ResetForumsPosts()
        {
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}resetforumsposts", BaseConfigs.GetTablePrefix));
        }

        /// <summary>
        /// 清除主题里面已经移走的主题
        /// </summary>
        public void ReSetClearMove()
        {
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}resetclearmove", BaseConfigs.GetTablePrefix));
        }

        public IDataReader GetLastPostByFid(int fid, string postTableName)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid),
				DbHelper.MakeInParam("@posttablename", (DbType)SqlDbType.NVarChar, 50, postTableName)
			};
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getlastpostbyfid", BaseConfigs.GetTablePrefix), parms);
        }

        public int CreatePoll(PollInfo pollInfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,pollInfo.Tid),
									   DbHelper.MakeInParam("@displayorder",(DbType)SqlDbType.Int,4,pollInfo.Displayorder),
									   DbHelper.MakeInParam("@multiple",(DbType)SqlDbType.Int,4,pollInfo.Multiple),
									   DbHelper.MakeInParam("@visible",(DbType)SqlDbType.Int,4,pollInfo.Visible),
                                       DbHelper.MakeInParam("@allowview",(DbType)SqlDbType.Int,4,pollInfo.Allowview),
									   DbHelper.MakeInParam("@maxchoices",(DbType)SqlDbType.Int,4,pollInfo.Maxchoices),
									   DbHelper.MakeInParam("@expiration",(DbType)SqlDbType.DateTime,8,pollInfo.Expiration),
                                       DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,pollInfo.Uid),
                                       DbHelper.MakeInParam("@voternames",(DbType)SqlDbType.NText,0,pollInfo.Voternames)
                                  };
            string commandText = string.Format("INSERT INTO [{0}polls] ( [tid] ,[displayorder] ,[multiple] ,[visible] , [allowview],[maxchoices] ,[expiration] ,[uid] ,[voternames] ) VALUES (@tid, @displayorder, @multiple, @visible, @allowview, @maxchoices, @expiration, @uid, @voternames);SELECT SCOPE_IDENTITY()  AS 'pollid'",
                                                BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms), -1);
        }

        public int CreatePollOption(PollOptionInfo pollOptionInfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,pollOptionInfo.Tid),
									   DbHelper.MakeInParam("@pollid",(DbType)SqlDbType.Int,4,pollOptionInfo.Pollid),
									   DbHelper.MakeInParam("@votes",(DbType)SqlDbType.Int,4,pollOptionInfo.Votes),
									   DbHelper.MakeInParam("@displayorder",(DbType)SqlDbType.Int,4,pollOptionInfo.Displayorder),
									   DbHelper.MakeInParam("@polloption",(DbType)SqlDbType.VarChar,80,pollOptionInfo.Polloption),
									   DbHelper.MakeInParam("@voternames",(DbType)SqlDbType.NText,0,pollOptionInfo.Voternames)
								   };
            string commandText = string.Format("INSERT INTO [{0}polloptions] ([tid] ,[pollid] ,[votes] ,[displayorder] ,[polloption] ,[voternames] ) VALUES (@tid, @pollid, @votes, @displayorder, @polloption, @voternames);SELECT SCOPE_IDENTITY()  AS 'polloptionid'",
                                                BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms), -1);
        }

        public bool UpdatePoll(PollInfo pollInfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,pollInfo.Tid),
									   DbHelper.MakeInParam("@displayorder",(DbType)SqlDbType.Int,4,pollInfo.Displayorder),
									   DbHelper.MakeInParam("@multiple",(DbType)SqlDbType.Int,4,pollInfo.Multiple),
									   DbHelper.MakeInParam("@visible",(DbType)SqlDbType.Int,4,pollInfo.Visible),
                                       DbHelper.MakeInParam("@allowview",(DbType)SqlDbType.Int,4,pollInfo.Allowview),
									   DbHelper.MakeInParam("@maxchoices",(DbType)SqlDbType.Int,4,pollInfo.Maxchoices),
									   DbHelper.MakeInParam("@expiration",(DbType)SqlDbType.DateTime,8,pollInfo.Expiration),
                                       DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,pollInfo.Uid),
                                       DbHelper.MakeInParam("@voternames",(DbType)SqlDbType.NText,0,pollInfo.Voternames),
                                       DbHelper.MakeInParam("@pollid",(DbType)SqlDbType.Int,4,pollInfo.Pollid),
                                  };
            string commandText = string.Format("UPDATE [{0}polls] set [tid] = @tid, [displayorder] = @displayorder, [multiple] = @multiple, [visible] = @visible, [allowview] = @allowview, [maxchoices] = @maxchoices, [expiration] = @expiration, [uid] = @uid, [voternames] = @voternames WHERE [pollid] = @pollid",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms) > 0;
        }

        public bool UpdatePollOption(PollOptionInfo pollOptionInfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,pollOptionInfo.Tid),
									   DbHelper.MakeInParam("@pollid",(DbType)SqlDbType.Int,4,pollOptionInfo.Pollid),
									   DbHelper.MakeInParam("@votes",(DbType)SqlDbType.Int,4,pollOptionInfo.Votes),
									   DbHelper.MakeInParam("@displayorder",(DbType)SqlDbType.Int,4,pollOptionInfo.Displayorder),
									   DbHelper.MakeInParam("@polloption",(DbType)SqlDbType.VarChar,80,pollOptionInfo.Polloption),
									   DbHelper.MakeInParam("@voternames",(DbType)SqlDbType.NText,0,pollOptionInfo.Voternames),
                                       DbHelper.MakeInParam("@polloptionid",(DbType)SqlDbType.Int,4,pollOptionInfo.Polloptionid)
								   };
            string commandText = string.Format("UPDATE [{0}polloptions] set [tid] = @tid, [pollid] = @pollid, [votes] = @votes, [displayorder] = @displayorder, [polloption] = @polloption, [voternames] = @voternames WHERE [polloptionid] = @polloptionid",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms) > 0;
        }

        public bool DeletePollOption(PollOptionInfo pollOptionInfo)
        {
            string commandText = string.Format("DELETE FROM [{0}polloptions] WHERE [polloptionid] = {1}",
                                                BaseConfigs.GetTablePrefix,
                                                pollOptionInfo.Polloptionid);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText) > 0;
        }


        public IDataReader GetPollList(int tid)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
                                  };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getpolllist", BaseConfigs.GetTablePrefix), parms);
        }

        public IDataReader GetPollOptionList(int tid)
        {
            string commandText = string.Format("SELECT {0} FROM [{1}polloptions] WHERE [tid]={2} Order By [displayorder] ASC, [polloptionid] ASC",
                                                DbFields.POLL_OPTIONS,
                                                BaseConfigs.GetTablePrefix,
                                                tid);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }


        /// <summary>
        /// 获得投票的用户名
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <returns></returns>
        public string GetPollUserNameList(int tid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int, 4, tid)
								   };
            string commandText = string.Format("SELECT TOP 1 [voternames] FROM [{0}polls] WHERE [tid]=@tid", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteScalarToStr(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 得到投票帖的投票类型
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns>投票类型</returns>
        public int GetPollType(int tid)
        {
            string commandText = string.Format("SELECT TOP 1 [multiple] FROM [{0}polls] WHERE [tid]={1}", BaseConfigs.GetTablePrefix, tid);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        /// <summary>
        /// 得到投票帖的结束时间
        /// </summary>
        /// <param name="tid">主题ＩＤ</param>
        /// <returns>结束时间</returns>
        public string GetPollEnddatetime(int tid)
        {
            string commandText = string.Format("SELECT TOP 1 [expiration] FROM [{0}polls] WHERE [tid]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                tid);
            return Utils.GetDate(DbHelper.ExecuteScalarToStr(CommandType.Text, commandText), Utils.GetDate());
        }

        /// <summary>
        /// 得到用户帖子分表信息
        /// </summary>
        /// <returns>分表记录集</returns>
        public DataSet GetAllPostTableName()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}tablelist] ORDER BY [id] DESC",
                                                DbFields.TABLE_LIST,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText);
        }

        /// <summary>
        /// 创建帖子
        /// </summary>
        /// <param name="postinfo">帖子信息类</param>
        /// <returns>返回帖子id</returns>
        public int CreatePost(PostInfo postInfo, string postTableId)
        {
            DbParameter[] parms = {

									   DbHelper.MakeInParam("@fid",(DbType)SqlDbType.SmallInt,2,postInfo.Fid),
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,postInfo.Tid),
									   DbHelper.MakeInParam("@parentid",(DbType)SqlDbType.Int,4,postInfo.Parentid),
									   DbHelper.MakeInParam("@layer",(DbType)SqlDbType.Int,4,postInfo.Layer),
									   DbHelper.MakeInParam("@poster",(DbType)SqlDbType.VarChar,15,postInfo.Poster),
									   DbHelper.MakeInParam("@posterid",(DbType)SqlDbType.Int,4,postInfo.Posterid),
									   DbHelper.MakeInParam("@title",(DbType)SqlDbType.NVarChar,60,postInfo.Title),
									   DbHelper.MakeInParam("@topictitle",(DbType)SqlDbType.NVarChar,60,postInfo.Topictitle),
									   DbHelper.MakeInParam("@postdatetime",(DbType)SqlDbType.DateTime,8, DateTime.Parse(postInfo.Postdatetime)),
									   DbHelper.MakeInParam("@message",(DbType)SqlDbType.NText,0,postInfo.Message),
									   DbHelper.MakeInParam("@ip",(DbType)SqlDbType.VarChar,15,postInfo.Ip),
									   DbHelper.MakeInParam("@lastedit",(DbType)SqlDbType.NVarChar,50,postInfo.Lastedit),
									   DbHelper.MakeInParam("@invisible",(DbType)SqlDbType.Int,4,postInfo.Invisible),
									   DbHelper.MakeInParam("@usesig",(DbType)SqlDbType.Int,4,postInfo.Usesig),
									   DbHelper.MakeInParam("@htmlon",(DbType)SqlDbType.Int,4,postInfo.Htmlon),
									   DbHelper.MakeInParam("@smileyoff",(DbType)SqlDbType.Int,4,postInfo.Smileyoff),
									   DbHelper.MakeInParam("@bbcodeoff",(DbType)SqlDbType.Int,4,postInfo.Bbcodeoff),
									   DbHelper.MakeInParam("@parseurloff",(DbType)SqlDbType.Int,4,postInfo.Parseurloff),
									   DbHelper.MakeInParam("@attachment",(DbType)SqlDbType.Int,4,postInfo.Attachment),
									   DbHelper.MakeInParam("@rate",(DbType)SqlDbType.SmallInt,2,postInfo.Rate),
									   DbHelper.MakeInParam("@ratetimes",(DbType)SqlDbType.Int,4,postInfo.Ratetimes)
								   };
            int pid = TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                    string.Format("{0}createpost{1}", BaseConfigs.GetTablePrefix, postTableId),
                                                                    parms), -1);
            if (pid != -1)
                UpdateTrendStat(TrendType.Post);
            return pid;
        }

        /// <summary>
        /// 更新指定帖子信息
        /// </summary>
        /// <param name="__postsInfo">帖子信息</param>
        /// <returns>更新数量</returns>
        public int UpdatePost(PostInfo postsInfo, string postTableId)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@pid",(DbType)SqlDbType.Int,4,postsInfo.Pid),
									   DbHelper.MakeInParam("@title",(DbType)SqlDbType.NVarChar,160,postsInfo.Title),
									   DbHelper.MakeInParam("@message",(DbType)SqlDbType.NText,0,postsInfo.Message),
									   DbHelper.MakeInParam("@lastedit",(DbType)SqlDbType.VarChar,50,postsInfo.Lastedit),
									   DbHelper.MakeInParam("@invisible",(DbType)SqlDbType.Int,4,postsInfo.Invisible),
									   DbHelper.MakeInParam("@usesig",(DbType)SqlDbType.Int,4,postsInfo.Usesig),
									   DbHelper.MakeInParam("@htmlon",(DbType)SqlDbType.Int,4,postsInfo.Htmlon),
									   DbHelper.MakeInParam("@smileyoff",(DbType)SqlDbType.Int,4,postsInfo.Smileyoff),
									   DbHelper.MakeInParam("@bbcodeoff",(DbType)SqlDbType.Int,4,postsInfo.Bbcodeoff),
									   DbHelper.MakeInParam("@parseurloff",(DbType)SqlDbType.Int,4,postsInfo.Parseurloff),
								   };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                            string.Format("{0}updatepost{1}", BaseConfigs.GetTablePrefix, postTableId),
                                            parms);
        }

        /// <summary>
        /// 删除指定ID的帖子
        /// </summary>
        /// <param name="posttableid">当前分表ID</param>
        /// <param name="pid">帖子ID</param>
        /// <returns>删除数量</returns>
        public int DeletePost(string postTableId, int pid, bool changePosts)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@pid",(DbType)SqlDbType.Int,4,pid),
                                       DbHelper.MakeInParam("@chanageposts",(DbType)SqlDbType.Bit,1,changePosts)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                            string.Format("{0}deletepost{1}bypid", BaseConfigs.GetTablePrefix, postTableId),
                                            parms);
        }

        /// <summary>
        /// 获得指定的帖子描述信息
        /// </summary>
        /// <param name="posttableid">当前分表ID</param>
        /// <param name="pid">帖子id</param>
        /// <returns>帖子描述信息</returns>
        public IDataReader GetPostInfo(string postTableId, int pid)
        {
            string commandText = string.Format("SELECT TOP 1 {0} FROM [{1}posts{2}] WHERE [pid]={3}",
                                                DbFields.POSTS,
                                                BaseConfigs.GetTablePrefix,
                                                postTableId,
                                                pid);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获得指定主题的帖子列表
        /// </summary>
        /// <param name="topiclist">主题ID列表</param>
        /// <param name="posttableid">分表Id列表</param>
        /// <returns></returns>
        public DataTable GetPostList(string topicList, string[] postTableId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < postTableId.Length; i++)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder.Append(" UNION ALL ");

                stringBuilder.AppendFormat("SELECT {0} FROM [{1}posts{2}] WHERE [tid] IN ({3})",
                                            DbFields.POSTS,
                                            BaseConfigs.GetTablePrefix,
                                            postTableId[i],
                                            topicList);
            }
            return DbHelper.ExecuteDataset(CommandType.Text, stringBuilder.ToString()).Tables[0];
        }

        /// <summary>
        /// 获取指定条件的帖子DataSet
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>指定条件的帖子DataSet</returns>
        public DataTable GetPostListTitle(int tid, string postTableName)
        {
            string commandText = string.Format("SELECT [pid], [title], [poster], [posterid],[message] FROM [{0}] WHERE [tid]={1}  ORDER BY [pid]",
                                                postTableName,
                                                tid);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 获取指定条件的帖子DataReader
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>指定条件的帖子DataReader</returns>
        public IDataReader GetPostListByCondition(PostpramsInfo postPramsInfo, string postTableId)
        {
            int posterid = Convert.ToInt32(postPramsInfo.Condition.Substring(postPramsInfo.Condition.IndexOf("=") + 1).Trim());
            DbParameter[] parms = {
										   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,postPramsInfo.Tid),
										   DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,postPramsInfo.Pagesize),
										   DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,postPramsInfo.Pageindex),
                                           DbHelper.MakeInParam("@posterid",(DbType)SqlDbType.Int,4,posterid)
									   };

            MemCachedConfigInfo mcci = MemCachedConfigs.GetConfig();
            RedisConfigInfo rci = RedisConfigs.GetConfig();

            //只有在应用memcached或redis的情况下才可以使用主题缓存，则此时为了确保数据同步，必须从主数据库中提取数据进行缓存
            if (((mcci != null && mcci.ApplyMemCached) || (rci != null && rci.ApplyRedis)) && postPramsInfo.Pageindex <= 5)
                return DbHelper.ExecuteReaderInMasterDB(CommandType.StoredProcedure,
                                          string.Format("{0}getpostlistbycondition{1}", BaseConfigs.GetTablePrefix, postTableId),
                                          parms);
            else
                return DbHelper.ExecuteReader(CommandType.StoredProcedure,
                                         string.Format("{0}getpostlistbycondition{1}", BaseConfigs.GetTablePrefix, postTableId),
                                         parms);
        }

        /// <summary>
        /// 获取指定条件的帖子DataReader
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>指定条件的帖子DataReader</returns>
        public IDataReader GetPostList(PostpramsInfo postParmsInfo, string postTableId)
        {
            DbParameter[] parms = {
										   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,postParmsInfo.Tid),
										   DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,postParmsInfo.Pagesize),
										   DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,postParmsInfo.Pageindex)
									   };

            MemCachedConfigInfo mcci = MemCachedConfigs.GetConfig();
            RedisConfigInfo rci = RedisConfigs.GetConfig();

            //只有在应用memcached的情况下才可以使用主题缓存，则此时为了确保数据同步，必须从主数据库中提取数据进行缓存
            if (((mcci != null && mcci.ApplyMemCached) || (rci != null && rci.ApplyRedis)) && postParmsInfo.Pageindex <= 5)
                return DbHelper.ExecuteReaderInMasterDB(CommandType.StoredProcedure,
                                          string.Format("{0}getpostlist{1}", BaseConfigs.GetTablePrefix, postTableId),
                                          parms);
            else
                return DbHelper.ExecuteReader(CommandType.StoredProcedure,
                                          string.Format("{0}getpostlist{1}", BaseConfigs.GetTablePrefix, postTableId),
                                          parms);


        }

        /// <summary>
        /// 返回指定主题的最后回复帖子
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public DataTable GetLastPostByTid(int tid, string postTableName)
        {
            DbParameter param = DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid);
            string commandText = string.Format("SELECT TOP 1 {0} FROM {1} WHERE [tid] = @tid ORDER BY [pid] DESC",
                                                DbFields.POSTS,
                                                postTableName);
            DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, commandText, param);
            return (ds != null && ds.Tables.Count > 0) ? ds.Tables[0] : new DataTable();
        }

        /// <summary>
        /// 获得最后回复的帖子列表
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>帖子列表</returns>
        public DataTable GetLastPostList(PostpramsInfo postParmsInfo, string postTableId)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,postParmsInfo.Tid),
                                       DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,postParmsInfo.Pageindex),
									   DbHelper.MakeInParam("@postnum",(DbType)SqlDbType.Int,4,postParmsInfo.Pagesize)
								   };
            return DbHelper.ExecuteDataset(CommandType.StoredProcedure,
                                           string.Format("{0}getlastpostlist{1}", BaseConfigs.GetTablePrefix, postTableId),
                                           parms).Tables[0];
        }

        /// <summary>
        /// 获得最后回复的帖子列表
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>帖子列表</returns>
        public DataTable GetPagedLastPostList(PostpramsInfo postParmsInfo, string postTableName)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,postParmsInfo.Tid),
								   };
            string where = "";
            if (postParmsInfo.Pageindex > 1)
                where = string.Format(" AND p.[pid] < (SELECT MIN([pid]) FROM (SELECT TOP {0} [{1}].[pid] FROM [{1}] WHERE [{1}].[tid]=@tid AND [{1}].[invisible]<=0 AND [{1}].layer<>0 ORDER BY [{1}].[pid] DESC) AS tblTmp)",
                                        (postParmsInfo.Pageindex - 1) * postParmsInfo.Pagesize,
                                        postTableName,
                                        BaseConfigs.GetTablePrefix);

            string commandText = string.Format("SELECT TOP {0} p.[pid], p.[fid], p.[layer], p.[posterid], p.[title], p.[message], p.[postdatetime], p.[attachment], p.[poster], p.[posterid], p.[invisible], p.[usesig], p.[htmlon], p.[smileyoff], p.[parseurloff], p.[bbcodeoff], p.[rate], p.[ratetimes], u.[username], u.[email], u.[showemail], uf.[avatar], uf.[avatarwidth], uf.[avatarheight], uf.[sightml] AS [signature], uf.[location], uf.[customstatus] FROM [{1}] p LEFT JOIN [{2}users] u ON u.[uid]=p.[posterid] LEFT JOIN [{2}userfields] uf ON uf.[uid]=u.[uid] WHERE p.[tid]=@tid AND p.[invisible]=0 AND p.layer<>0 {3} ORDER BY p.[pid] DESC",
                                                postParmsInfo.Pagesize,
                                                postTableName, BaseConfigs.GetTablePrefix,
                                                where);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }


        /// <summary>
        /// 获得单个帖子的信息, 包括发帖人的一般资料
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>帖子的信息</returns>
        public IDataReader GetSinglePost(out IDataReader attachments, PostpramsInfo postParmsInfo, string postTableId)
        {
            DbParameter[] parms = {
										DbHelper.MakeInParam("@pid",(DbType)SqlDbType.Int,4,postParmsInfo.Pid),
										DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,postParmsInfo.Tid)
									};
            attachments = null;
            return DbHelper.ExecuteReader(CommandType.StoredProcedure,
                                          string.Format("{0}getsinglepost{1}", BaseConfigs.GetTablePrefix, postTableId),
                                          parms);
        }

        /// <summary>
        /// 获得单个帖子的信息, 包括发帖人的一般资料
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>帖子的信息</returns>
        public IDataReader GetSinglePost(int tid, string postTableId)
        {
            string commandText = string.Format("SELECT {0} FROM [{1}posts{2}] WHERE [tid]=@tid AND [layer]=0",
                                                DbFields.POSTS,
                                                BaseConfigs.GetTablePrefix,
                                                postTableId);
            return DbHelper.ExecuteReader(CommandType.Text, commandText, DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid));
        }

        public DataTable GetPostTree(int tid, string postTableId)
        {
            return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getpost{1}tree", BaseConfigs.GetTablePrefix, postTableId),
                                                            DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid)).Tables[0];
        }

        /// <summary>
        /// 获得指定主题的第一个帖子的id
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns>帖子id</returns>
        public int GetFirstPostId(int tid, string postTableId)
        {
            return TypeConverter.ObjectToInt(
                                 DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                        string.Format("{0}getfirstpost{1}id", BaseConfigs.GetTablePrefix, postTableId),
                                                        DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid)), -1);
        }

        /// <summary>
        /// 判断指定用户是否是指定主题的回复者
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="uid">用户id</param>
        /// <returns>是否是指定主题的回复者</returns>
        public bool IsReplier(int tid, int uid, string postTableId)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
								   };
            return uid > 0 && TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                               string.Format("{0}getreplypid{1}", BaseConfigs.GetTablePrefix, postTableId),
                                                               parms)) > 0;
        }

        /// <summary>
        /// 更新帖子的评分值
        /// </summary>
        /// <param name="postIdList">帖子ID列表</param>
        /// <param name="postTableId"></param>
        /// <returns>更新的帖子数量</returns>
        public int UpdatePostRateTimes(string postIdList, string postTableId)
        {
            string commandText = "";
            foreach (string pid in postIdList.Split(','))
            {
                commandText = string.Format("SELECT [uid] FROM {0}ratelog WHERE [pid]={1} GROUP BY [uid]", BaseConfigs.GetTablePrefix, pid);
                int rateTimes = DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows.Count;
                commandText = string.Format("UPDATE [{0}posts{1}] SET [ratetimes] = {3} WHERE [pid] IN ({2})",
                    BaseConfigs.GetTablePrefix, postTableId, pid, rateTimes);

            }
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 更新帖子的评分值
        /// </summary>
        /// <param name="postidlist"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        public int UpdatePostRate(int pid, float rate, string postTableId)
        {
            string commandText = string.Format("UPDATE [{0}posts{1}] SET [rate] = [rate] + {2} WHERE [pid] IN ({3})",
                                                BaseConfigs.GetTablePrefix,
                                                postTableId,
                                                rate,
                                                pid);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 撤消帖子的评分值
        /// </summary>
        /// <param name="postidlist"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        public int CancelPostRate(string postIdList, string postTableId)
        {
            string commandText = string.Format("UPDATE [{0}posts{1}] SET [rate] = 0, [ratetimes]=0 WHERE [pid] IN ({2})",
                                                BaseConfigs.GetTablePrefix,
                                                postTableId,
                                                postIdList);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获取帖子评分列表
        /// </summary>
        /// <param name="pid">帖子列表</param>
        /// <returns></returns>
        public IDataReader GetPostRateLogs(int pid, int displayRateCount)
        {
            string commandText = string.Format("SELECT TOP {0} {1} FROM [{2}ratelog] WHERE [pid]=@pid ORDER BY [id] DESC",
                                                displayRateCount,
                                                DbFields.RATE_LOG,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText, DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 4, pid));
        }

        /// <summary>
        /// 获取分页帖子评分列表
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IDataReader GetPostRateLogList(int pid, int pageIndex, int pageSize)
        {
            DbParameter[] parms = {
                                      DbHelper.MakeInParam("@pid",(DbType)SqlDbType.Int,4,pid),
									   DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,pageIndex),
									   DbHelper.MakeInParam("@pagesize",(DbType)SqlDbType.Int,4,pageSize)
                                  };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getrateloglist", BaseConfigs.GetTablePrefix), parms);
        }

        public int GetPostRateLogCount(int pid)
        {
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}getratecount", BaseConfigs.GetTablePrefix), DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 4, pid)));
        }



        /// <summary>
        /// 获取新主题
        /// </summary>
        /// <param name="forumidlist">不允许游客访问的版块id列表</param>
        /// <returns></returns>
        public IDataReader GetNewTopics(string forumIdList, string postTableId)
        {
            DbParameter[] parm = { DbHelper.MakeInParam("@fidlist", (DbType)SqlDbType.VarChar, 500, forumIdList) };
            return DbHelper.ExecuteReaderInMasterDB(CommandType.StoredProcedure, string.Format("{0}getnewtopics{1}", BaseConfigs.GetTablePrefix, postTableId), parm);
        }

        public IDataReader GetSitemapNewTopics(string forumIdList)
        {
            DbParameter[] parm = { DbHelper.MakeInParam("@fidlist", (DbType)SqlDbType.VarChar, 500, forumIdList) };
            return DbHelper.ExecuteReaderInMasterDB(CommandType.StoredProcedure, string.Format("{0}getsitemapnewtopics", BaseConfigs.GetTablePrefix), parm);
        }

        /// <summary>
        /// 创建搜索缓存
        /// </summary>
        /// <param name="cacheinfo">搜索缓存信息</param>
        /// <returns>搜索缓存id</returns>
        public int CreateSearchCache(SearchCacheInfo cacheInfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@keywords",(DbType)SqlDbType.NVarChar,255,cacheInfo.Keywords),
									   DbHelper.MakeInParam("@searchstring",(DbType)SqlDbType.NVarChar,255,cacheInfo.Searchstring),
									   DbHelper.MakeInParam("@ip",(DbType)SqlDbType.VarChar,15,cacheInfo.Ip),
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,cacheInfo.Uid),
									   DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.Int,4,cacheInfo.Groupid),
									   DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(cacheInfo.Postdatetime)),
									   DbHelper.MakeInParam("@expiration",(DbType)SqlDbType.VarChar,19,cacheInfo.Expiration),
									   DbHelper.MakeInParam("@topics",(DbType)SqlDbType.Int,4,cacheInfo.Topics),
									   DbHelper.MakeInParam("@tids",(DbType)SqlDbType.Text,0,cacheInfo.Tids)
								   };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}createsearchcache", BaseConfigs.GetTablePrefix), parms), -1);
        }

        /// <summary>
        /// 删除超过30分钟的缓存记录
        /// </summary>
        public void DeleteExpriedSearchCache()
        {
            DbHelper.ExecuteNonQuery(CommandType.Text,
                                     string.Format(@"DELETE FROM [{0}searchcaches] WHERE [expiration]<@expiration", BaseConfigs.GetTablePrefix),
                                     DbHelper.MakeInParam("@expiration", (DbType)SqlDbType.DateTime, 8, DateTime.Now.AddMinutes(-30).ToString("yyyy-MM-dd HH:mm:ss")));
        }

        /// <summary>
        /// 获得搜索缓存
        /// </summary>
        /// <returns></returns>
        public DataTable GetSearchCache(int searchId)
        {
            return DbHelper.ExecuteDataset(CommandType.Text,
                                           string.Format("SELECT TOP 1 [tids] FROM [{0}searchcaches] WHERE [searchid]=@searchid", BaseConfigs.GetTablePrefix),
                                           DbHelper.MakeInParam("@searchid", (DbType)SqlDbType.Int, 4, searchId)).Tables[0];
        }

        /// <summary>
        /// 获得搜索的精华帖
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="strTids"></param>
        /// <returns></returns>
        public DataTable GetSearchDigestTopicsList(int pageSize, string strTids)
        {
            string commandText = string.Format("SELECT TOP {1} [{0}topics].[tid], [{0}topics].[title], [{0}topics].[poster], [{0}topics].[posterid], [{0}topics].[postdatetime], [{0}topics].[replies], [{0}topics].[views], [{0}topics].[lastpost],[{0}topics].[lastposter], [{0}topics].[lastposterid], [{0}forums].[fid],[{0}forums].[name] AS [forumname] FROM [{0}topics] LEFT JOIN [{0}forums] ON [{0}forums].[fid] = [{0}topics].[fid] WHERE [{0}topics].[tid] IN({2}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[{0}topics].[tid]),'{2}')",
                                                BaseConfigs.GetTablePrefix,
                                                pageSize,
                                                strTids);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 获得按帖子搜索的主题列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="strTids"></param>
        /// <returns></returns>
        public DataTable GetSearchPostsTopicsList(int pageSize, string strTids, string postTableName)
        {
            string commandText = string.Format("SELECT TOP {1} [{2}].[tid], [{2}].[title], [{2}].[poster], [{2}].[posterid],[lastposterid], [{2}].[postdatetime],[{2}].[lastedit], [{2}].[rate], [{2}].[ratetimes], [{0}forums].[fid],[{0}forums].[name] AS [forumname] FROM [{2}] LEFT JOIN [{0}forums] ON [{0}forums].[fid] = [{2}].[fid] WHERE [{2}].[pid] IN({3}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[{2}].[tid]),'{3}')",
                                                BaseConfigs.GetTablePrefix,
                                                pageSize,
                                                postTableName,
                                                strTids);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 按搜索获得主题列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="strTids"></param>
        /// <returns></returns>
        public DataTable GetSearchTopicsList(int pageSize, string strTids)
        {
            //string commandText = string.Format("SELECT TOP {1} [{0}topics].[tid], [{0}topics].[title], [{0}topics].[poster], [{0}topics].[posterid], [{0}topics].[postdatetime], [{0}topics].[replies], [{0}topics].[views], [{0}topics].[lastpost],[{0}topics].[lastposter],[{0}topics].[lastposterid], [{0}forums].[fid],[{0}forums].[name] AS [forumname] FROM [{0}topics] LEFT JOIN ([{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].[fid]=[{0}forumfields].[fid]) ON [{0}forums].[fid] = [{0}topics].[fid] WHERE [{0}topics].[tid] IN({2})  AND [{0}forumfields].[password]=''  ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[{0}topics].[tid]),'{2}')",
            //                                    BaseConfigs.GetTablePrefix,
            //                                    pageSize,
            //                                    strTids);
            //--
            //string commandText = string.Format("SELECT TOP {1} [{0}topics].[tid], [{0}topics].[title], [{0}topics].[poster], [{0}topics].[posterid], [{0}topics].[postdatetime], [{0}topics].[replies], [{0}topics].[views], [{0}topics].[lastpost],[{0}topics].[lastposter],[{0}topics].[lastposterid], [{0}forums].[fid],[{0}forums].[name] AS [forumname] FROM [{0}topics] LEFT JOIN ([{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].[fid]=[{0}forumfields].[fid]) ON [{0}forums].[fid] = [{0}topics].[fid] WHERE [{0}topics].[tid] IN({2}) AND [{0}forumfields].[password]='' AND CHARINDEX(CONVERT(VARCHAR(8),[{0}topics].[tid]),'{2}') > 0 ORDER BY [{0}topics].[lastpost] DESC",
            //                                    BaseConfigs.GetTablePrefix,
            //                                    pageSize,
            //                                    strTids);

            string commandText = string.Format("SELECT TOP {1} [{0}topics].[tid], [{0}topics].[title], [{0}topics].[poster], [{0}topics].[posterid], [{0}topics].[postdatetime], [{0}topics].[replies], [{0}topics].[views], [{0}topics].[lastpost],[{0}topics].[lastposter],[{0}topics].[lastposterid], [{0}forums].[fid],[{0}forums].[name] AS [forumname] FROM [{0}topics] LEFT JOIN ([{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].[fid]=[{0}forumfields].[fid]) ON [{0}forums].[fid] = [{0}topics].[fid] WHERE [{0}topics].[tid] IN({2}) AND [{0}forumfields].[password]='' AND CHARINDEX(CONVERT(VARCHAR(8),[{0}topics].[tid]),'{2}') > 0 ORDER BY CHARINDEX(LTRIM([{0}topics].[tid]),'{2}')",
                                    BaseConfigs.GetTablePrefix,
                                    pageSize,
                                    strTids);

            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 开启全文索引
        /// </summary>
        public void ConfirmFullTextEnable()
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "IF(SELECT DATABASEPROPERTY(DB_NAME(), 'IsFullTextEnabled'))=0 EXEC sp_fulltext_database 'enable'");
        }


        /// <summary>
        /// 设置主题指定字段的属性值(字符型)
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="field">要设置的字段</param>
        /// <param name="intValue">属性值</param>
        /// <returns>更新主题个数</returns>
        public int SetTopicStatus(string topicList, string field, string intValue)
        {
            string commandText = string.Format("UPDATE [{0}topics] SET [{1}] = @field WHERE [tid] IN ({2})",
                                                BaseConfigs.GetTablePrefix,
                                                field,
                                                topicList);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText,
                                            DbHelper.MakeInParam("@field", (DbType)SqlDbType.VarChar, 500, intValue));
        }

        public DataSet GetTopTopicList()
        {
            return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getalltoptopiclist", BaseConfigs.GetTablePrefix));
        }


        public DataTable GetShortForums()
        {
            string commandText = string.Format("SELECT [fid],[parentid],[parentidlist], [layer], CAST('' AS VARCHAR(1000)) AS [temptidlist],CAST('' AS VARCHAR(1000)) AS [tid2list], CAST('' AS VARCHAR(1000)) AS [tidlist],CAST(0 AS INT) AS [tidcount],CAST(0 AS INT) AS [tid2count],CAST(0 AS INT) AS [tid3count] FROM [{0}forums] ORDER BY [fid] DESC",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public IDataReader GetUserListWithTopicList(string topicList, int lossLessDel)
        {
            string commandText = string.Format("SELECT [posterid] FROM [{0}topics] WHERE DATEDIFF(day, [postdatetime], GETDATE())<@Losslessdel AND [tid] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                topicList);
            return DbHelper.ExecuteReader(CommandType.Text, commandText,
                                          DbHelper.MakeInParam("@Losslessdel", (DbType)SqlDbType.Int, 4, lossLessDel));
        }

        public IDataReader GetUserListWithTopicList(string topicList)
        {
            string commandText = string.Format("SELECT [posterid] FROM [{0}topics] WHERE [tid] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                topicList);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获取设置精华帖的用户list
        /// </summary>
        /// <param name="digestTopicList">主题id列表</param>
        /// <param name="digestType">操作类型，0：解除精华；1：升级精华</param>
        /// <returns></returns>
        public IDataReader GetUserListWithDigestTopicList(string digestTopicList, int digestType)
        {
            string commandText = string.Format("SELECT [posterid] FROM [{0}topics] WHERE [tid] IN ({1}) {2}",
                                    BaseConfigs.GetTablePrefix,
                                    digestTopicList, string.Format(" AND [digest]{0}", digestType == 0 ? ">0" /*执行解除操作时主题必须是精华主题*/ :
                                    "<=0" /*执行设置操作时必须是非精华主题*/));
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 将主题设置关闭/打开
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="intValue">关闭/打开标志( 0 为打开,1 为关闭)</param>
        /// <returns>更新主题个数</returns>
        public int SetTopicClose(string topicList, short intValue)
        {
            string commandText = string.Format("UPDATE [{0}topics] SET [closed] = @field WHERE [tid] IN ({1}) AND [closed] IN (0,1)",
                                                BaseConfigs.GetTablePrefix,
                                                topicList);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText,
                                            DbHelper.MakeInParam("@field", (DbType)SqlDbType.TinyInt, 1, intValue));
        }

        /// <summary>
        /// 获得主题指定字段的属性值
        /// </summary>
        /// <param name="topiclist">主题列表</param>
        /// <param name="field">要获得值的字段</param>
        /// <returns>主题指定字段的状态</returns>
        public int GetTopicStatus(string topicList, string field)
        {
            string commandText = string.Format("SELECT SUM(ISNULL([{0}],0)) AS [fieldcount] FROM [{1}topics] WHERE [tid] IN ({2})",
                                                field,
                                                BaseConfigs.GetTablePrefix,
                                                topicList);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 删除指定主题
        /// </summary>
        /// <param name="topiclist">要删除的主题ID列表</param>
        /// <param name="posttableid">所以分表的ID</param>
        /// <param name="chanageposts">删除帖时是否要减版块帖数</param>
        /// <returns></returns>
        public int DeleteTopicByTidList(string topicList, string postTableId, bool changePosts)
        {
            DbParameter[] parms = {
					DbHelper.MakeInParam("@tidlist", (DbType)SqlDbType.VarChar, 2000, topicList),
                    DbHelper.MakeInParam("@chanageposts",(DbType)SqlDbType.Bit,1,changePosts)
				};
            string commandText = string.Format("{0}deletetopicbytidlist{1}", BaseConfigs.GetTablePrefix, postTableId);
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, commandText, parms);
        }

        public int DeleteClosedTopics(int fid, string topicList)
        {
            string commandText = string.Format("DELETE FROM [{0}topics] WHERE [fid]={1} AND [closed] IN ({2})",
                                                BaseConfigs.GetTablePrefix,
                                                fid,
                                                topicList);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public int CopyTopicLink(int oldFid, string topicList)
        {
            ///用户设置转移后保留原连接执行以下三步操作
            ///1.向表中批量拷贝记录并将closed字段设置为原记录的tid*-1
            string commandText = string.Format(@"INSERT INTO [{0}topics] (
					[fid],
					[iconid],
					[typeid],
					[readperm],
					[price],
					[poster],
					[posterid],
					[title],
					[postdatetime],
					[lastpost],
					[lastposter],
					[lastposterid],
					[views],
					[replies],
					[displayorder],
					[highlight],
					[digest],
					[rate],					
					[attachment],
					[moderated],
					[hide],
					[lastpostid],
					[magic],
					[closed]
					) SELECT @fid,
					[iconid],
					[typeid],
					[readperm],
					[price],
					[poster],
					[posterid],
					[title],
					[postdatetime],
					[lastpost],
					[lastposter],
					[lastposterid],
					[views],
					[replies],
					[displayorder],
					[highlight],
					[digest],
					[rate],
					[attachment],
					[moderated],
					[hide],
					[lastpostid],
					[magic],
					[tid] AS [closed] FROM [{0}topics] WHERE [tid] IN ({1})", BaseConfigs.GetTablePrefix, topicList);

            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText,
                                            DbHelper.MakeInParam("@fid", (DbType)SqlDbType.SmallInt, 2, oldFid));
        }


        public void UpdatePost(string topicList, int fid, string postTable)
        {
            string commandText = string.Format("UPDATE [{0}] SET [fid]=@fid WHERE [tid] IN ({1})",
                                                postTable,
                                                topicList);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText,
                                     DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 1, fid));
        }
        /// <summary>
        /// 更新主题所属版块,会将主题分类至为0
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="fid"></param>
        /// <param name="topicType">要绑定的主题类型</param>
        /// <returns></returns>
        public int UpdateTopic(string topicList, int fid, int topicType)
        {
            DbParameter[] parms =
					{
						DbHelper.MakeInParam("@fid", (DbType)SqlDbType.SmallInt, 1, fid),
                        DbHelper.MakeInParam("@typeid", (DbType)SqlDbType.SmallInt, 1, topicType)
					};
            string commandText = string.Format("UPDATE [{0}topics] SET [fid]=@fid, [typeid]=@typeid WHERE [tid] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                topicList);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }


        public void UpdatePostTid(string postIdList, int tid, string postTableId)
        {
            DbParameter[] parms =
					{
						DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid)
					};
            string commandText = string.Format("UPDATE [{0}posts{1}] SET [tid]=@tid WHERE [pid] IN ({2})",
                                                BaseConfigs.GetTablePrefix,
                                                postTableId,
                                                postIdList);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void SetPrimaryPost(string subject, int tid, string[] postIdArray, string postTableId)
        {
            DbParameter[] prams =
					{
						DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 4, Utils.StrToInt(postIdArray[0], 0)),
						DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 80, subject)
					};
            string commandText = string.Format("UPDATE [{0}posts{1}] SET [title] = @title, [parentid] = [pid],[layer] = 0 WHERE [pid] = @pid",
                                                BaseConfigs.GetTablePrefix,
                                                postTableId);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, prams);
        }


        public int UpdatePostTidToAnotherTopic(int oldTid, int newTid, string postTableId)
        {
            DbParameter[] prams =
				{
					DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, newTid),
					DbHelper.MakeInParam("@oldtid", (DbType)SqlDbType.Int, 4, oldTid)
				};
            string commandText = string.Format("UPDATE [{0}posts{1}] SET [tid] = @tid, [layer] = CASE WHEN [layer] = 0 THEN 1 ELSE [layer] END WHERE [tid] = @oldtid",
                                                BaseConfigs.GetTablePrefix,
                                                postTableId);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, prams);
        }

        public int UpdateAttachmentTidToAnotherTopic(int oldTid, int newTid)
        {
            DbParameter[] prams =
				{
					DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, newTid),
					DbHelper.MakeInParam("@oldtid", (DbType)SqlDbType.Int, 4, oldTid)
				};
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                            string.Format("{0}updateattachmenttidtoanothertopic", BaseConfigs.GetTablePrefix),
                                            prams);
        }

        public int DeleteTopic(int tid)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text,
                                            string.Format("DELETE FROM [{0}topics] WHERE [tid] = {1}", BaseConfigs.GetTablePrefix, tid));
        }

        /// <summary>
        /// 修复主题操作
        /// </summary>
        /// <param name="topicId">主题ID(此处只能传入一个主题id)</param>
        /// <param name="postTable">帖子分表名称</param>
        /// <returns></returns>
        public int RepairTopics(string topicId, string postTable)
        {
            string commandText = string.Format("SELECT TOP 1 [postdatetime],[pid],[poster],[posterid]  FROM [{0}] WHERE  [tid]={1}  ORDER BY [{0}].[PID] DESC",
                                                postTable,
                                                topicId);
            IDataReader iDataReader = DbHelper.ExecuteReader(CommandType.Text, commandText);
            commandText = null;
            if (iDataReader.Read())
            {
                commandText = string.Format("UPDATE [{0}topics] SET [lastpost] = '{1}' ,[lastpostid] = {2}, [lastposter] = '{3}', [lastposterid] = {4}, [replies] = (SELECT COUNT([pid]) FROM [{5}] WHERE [{0}topics].[tid] = [{5}].[tid] AND ([{5}].[invisible] = 0 OR [{5}].[invisible] = -2) AND [{5}].[layer]>0)  WHERE [{0}topics].[tid] IN ({6})",
                                             BaseConfigs.GetTablePrefix,
                                             iDataReader["postdatetime"],
                                             iDataReader["pid"],
                                             iDataReader["poster"],
                                             iDataReader["posterid"],
                                             postTable,
                                             topicId);
            }
            iDataReader.Close();
            return Utils.StrIsNullOrEmpty(commandText) ? 0 : DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public IDataReader GetUserListWithPostList(string postList, string postTableId)
        {
            string commandText = string.Format("SELECT [posterid] FROM [{0}posts{1}] WHERE [pid] in ({2})",
                                                BaseConfigs.GetTablePrefix,
                                                postTableId,
                                                postList);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public string CheckRateState(int userId, string pid)
        {
            DbParameter[] parms =
					{
						DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 4, Utils.StrToFloat(pid, 0)),
						DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userId)
					};
            string commandText = string.Format("SELECT [pid] FROM [{0}ratelog] WHERE [pid] = @pid AND [uid] = @uid", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteScalarToStr(CommandType.Text, commandText, parms);
        }

        public IDataReader GetTopicListModeratorLog(int tid)
        {
            string commandText = string.Format("SELECT TOP 1 [grouptitle], [moderatorname],[postdatetime],[actions] FROM [{0}moderatormanagelog] WHERE [tid] = {1} ORDER BY [id] DESC",
                                                BaseConfigs.GetTablePrefix,
                                                tid);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 重设主题类型
        /// </summary>
        /// <param name="topictypeid">主题类型</param>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <returns></returns>
        public int ResetTopicTypes(int topicTypeId, string topicList)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@typeid", (DbType)SqlDbType.TinyInt, 1, topicTypeId),
									   DbHelper.MakeInParam("@topiclist", (DbType)SqlDbType.NVarChar, 250, topicList)
								   };
            string commandText = string.Format("UPDATE [{0}topics] SET [typeid] = @typeid WHERE [tid] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                topicList);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 按照用户Id获取其回复过的主题总数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetTopicReplyCountByUserId(int userId)
        {
            string commandText = string.Format("SELECT COUNT(DISTINCT [tid]) FROM [{0}myposts] WHERE [uid] = {1}",
                                                BaseConfigs.GetTablePrefix,
                                                userId);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText));
        }


        public IDataReader GetTopicsByReplyUserId(int userId, int pageIndex, int pageSize)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userId),
									   DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int, 4, pageIndex),
									   DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int, 4, pageSize)
								   };
            //TODO:修改存储过程getmyposts的名字，getmyrepliedtopics
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getmyposts", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 按照用户Id获取主题总数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetTopicCountByUserId(int userId)
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}mytopics] WHERE [uid] = {1}",
                                                BaseConfigs.GetTablePrefix,
                                                userId);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText));
        }


        public IDataReader GetTopicsByUserId(int userId, int pageIndex, int pageSize)
        {
            DbParameter[] parms = {
										DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userId),
										DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int, 4, pageIndex),
										DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int, 4, pageSize)
								   };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getmytopics", BaseConfigs.GetTablePrefix), parms);
        }

        public int CreateTopic(TopicInfo topicInfo)
        {
            DbParameter[] parms = {
										DbHelper.MakeInParam("@fid", (DbType)SqlDbType.SmallInt, 2, topicInfo.Fid), 
										DbHelper.MakeInParam("@iconid", (DbType)SqlDbType.SmallInt, 2, topicInfo.Iconid), 
										DbHelper.MakeInParam("@title", (DbType)SqlDbType.NChar, 60, topicInfo.Title), 
										DbHelper.MakeInParam("@typeid", (DbType)SqlDbType.SmallInt, 2, topicInfo.Typeid), 
										DbHelper.MakeInParam("@readperm", (DbType)SqlDbType.Int, 4, topicInfo.Readperm), 
										DbHelper.MakeInParam("@price", (DbType)SqlDbType.SmallInt, 2, topicInfo.Price), 
										DbHelper.MakeInParam("@poster", (DbType)SqlDbType.NChar, 15, topicInfo.Poster), 
										DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.Int, 4, topicInfo.Posterid), 
										DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime,8, DateTime.Parse(topicInfo.Postdatetime)), 
										DbHelper.MakeInParam("@lastpost", (DbType)SqlDbType.VarChar, 0, topicInfo.Lastpost), 
										DbHelper.MakeInParam("@lastpostid", (DbType)SqlDbType.Int, 4, topicInfo.Lastpostid),
										DbHelper.MakeInParam("@lastposter", (DbType)SqlDbType.NChar, 15, topicInfo.Lastposter), 
										DbHelper.MakeInParam("@views", (DbType)SqlDbType.Int, 4, topicInfo.Views), 
										DbHelper.MakeInParam("@replies", (DbType)SqlDbType.Int, 4, topicInfo.Replies), 
										DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, topicInfo.Displayorder), 
										DbHelper.MakeInParam("@highlight", (DbType)SqlDbType.VarChar, 500, topicInfo.Highlight), 
										DbHelper.MakeInParam("@digest", (DbType)SqlDbType.Int, 4, topicInfo.Digest), 
										DbHelper.MakeInParam("@rate", (DbType)SqlDbType.Int, 4, topicInfo.Rate), 
										DbHelper.MakeInParam("@hide", (DbType)SqlDbType.Int, 4, topicInfo.Hide), 
										DbHelper.MakeInParam("@attachment", (DbType)SqlDbType.Int, 4, topicInfo.Attachment), 
										DbHelper.MakeInParam("@moderated", (DbType)SqlDbType.Int, 4, topicInfo.Moderated), 
										DbHelper.MakeInParam("@closed", (DbType)SqlDbType.Int, 4, topicInfo.Closed),
										DbHelper.MakeInParam("@magic", (DbType)SqlDbType.Int, 4, topicInfo.Magic),
                                        DbHelper.MakeInParam("@special", (DbType)SqlDbType.TinyInt, 1, topicInfo.Special),
                                        DbHelper.MakeInParam("@attention", (DbType)SqlDbType.Int, 4, topicInfo.Attention)
								   };
            int tid = TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.StoredProcedure,
                                                                     string.Format("{0}createtopic", BaseConfigs.GetTablePrefix),
                                                                     parms).Tables[0].Rows[0][0], -1);
            if (tid != -1)
            {
                TrendType trendType = TrendType.Topic;
                switch (topicInfo.Special)
                {
                    case 0:
                        trendType = TrendType.Topic;
                        break;
                    case 1:
                        trendType = TrendType.Poll;
                        break;
                    case 2:
                        trendType = TrendType.Bonus;
                        break;
                    case 4:
                        trendType = TrendType.Debate;
                        break;
                }
                UpdateTrendStat(trendType);
            }
            return tid;
        }

        /// <summary>
        /// 增加父版块的主题数
        /// </summary>
        /// <param name="fpidlist">父板块id列表</param>
        /// <param name="topics">主题数</param>  
        public void AddParentForumTopics(string fpIdList, int topics)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@topics", (DbType)SqlDbType.Int, 4, topics),
									   DbHelper.MakeInParam("@fpidlist", (DbType)SqlDbType.NVarChar, 100, fpIdList)                                      
			};
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}addparentforumtopics", BaseConfigs.GetTablePrefix), parms);
        }


        public IDataReader GetTopicInfo(int tid, int fid, byte mode)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid),
									   DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid),
                                       DbHelper.MakeInParam("@mode", (DbType)SqlDbType.Int, 4, mode)
			};
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}gettopicinfo", BaseConfigs.GetTablePrefix), parms);
        }


        public IDataReader GetTopTopics(int fid, int pageSize, int pageIndex, string tids)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int,4,fid),
									   DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pageSize),
									   DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,pageIndex),
									   DbHelper.MakeInParam("@tids",(DbType)SqlDbType.VarChar,500,tids)									   
								   };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}gettoptopiclist", BaseConfigs.GetTablePrefix), parms);
        }


        public IDataReader GetTopics(int fid, int pageSize, int pageIndex, int startNum, string condition)
        {
            if (condition.Trim() == string.Empty)
            {
                DbParameter[] parms = {
									   DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int,4,fid),
									   DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pageSize),
									   DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int,4,pageIndex),
                                       DbHelper.MakeInParam("@startnum",(DbType)SqlDbType.Int,4,startNum)
								   };
                return DbHelper.ExecuteReader(CommandType.StoredProcedure,
                                              string.Format("{0}gettopiclist", BaseConfigs.GetTablePrefix),
                                              parms);
            }
            else
            {
                DbParameter[] parms = {
									   DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int,4,fid),
									   DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pageSize),
									   DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int,4,pageIndex),
									   DbHelper.MakeInParam("@condition", (DbType)SqlDbType.VarChar,80,condition),
								       DbHelper.MakeInParam("@startnum", (DbType)SqlDbType.VarChar,80,startNum)
								   };
                return DbHelper.ExecuteReader(CommandType.StoredProcedure,
                                              string.Format("{0}gettopiclistbycondition", BaseConfigs.GetTablePrefix),
                                              parms);
            }
        }


        public IDataReader GetTopicsByDate(int fid, int pageSize, int pageIndex, int startNumber, string condition, string orderFields, int sortType)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int,4,fid),
									   DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pageSize),
									   DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,pageIndex),
									   DbHelper.MakeInParam("@startnum",(DbType)SqlDbType.Int,4,startNumber),
									   DbHelper.MakeInParam("@condition",(DbType)SqlDbType.VarChar,80,condition),
									   DbHelper.MakeInParam("@orderby",(DbType)SqlDbType.VarChar,80,orderFields),
									   DbHelper.MakeInParam("@ascdesc",(DbType)SqlDbType.Int,4,sortType)				                    
								   };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}gettopiclistbydate", BaseConfigs.GetTablePrefix), parms);
        }

        //TODO:需要重命名
        public IDataReader GetTopicsByType(int pageSize, int pageIndex, int startNum, string condition, int ascDesc)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pageSize),
									   DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,pageIndex),
									   DbHelper.MakeInParam("@startnum",(DbType)SqlDbType.Int,4,startNum),
									   DbHelper.MakeInParam("@condition",(DbType)SqlDbType.VarChar,1000,condition),
			                           DbHelper.MakeInParam("@ascdesc", (DbType)SqlDbType.Int, 4, ascDesc)
								   };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}gettopiclistbytype", BaseConfigs.GetTablePrefix), parms);
        }

        //TODO:需要重命名
        public IDataReader GetTopicsByTypeDate(int pageSize, int pageIndex, int startNum, string condition, string orderBy, int ascDesc)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pageSize),
									   DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,pageIndex),
									   DbHelper.MakeInParam("@startnum",(DbType)SqlDbType.Int,4,startNum),
									   DbHelper.MakeInParam("@condition",(DbType)SqlDbType.VarChar,1000,condition),
									   DbHelper.MakeInParam("@orderby",(DbType)SqlDbType.VarChar,80,orderBy),
									   DbHelper.MakeInParam("@ascdesc",(DbType)SqlDbType.Int,4,ascDesc)				                    
								   };

            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}gettopiclistbytypedate", BaseConfigs.GetTablePrefix), parms);
        }


        public DataTable GetTopicList(string topicList, int displayOrder)
        {
            string commandText = string.Format("SELECT {0} FROM [{1}topics] WHERE [displayorder]>{2} AND [tid] IN ({3})",
                                                DbFields.TOPICS,
                                                BaseConfigs.GetTablePrefix,
                                                displayOrder,
                                                topicList);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public IDataReader GetTopicsByViewsOrReplies(int fid, int pageSize, int pageIndex, int startNumber, string condition, string orderFields, int sortType)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int,4,fid),
									   DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pageSize),
									   DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,pageIndex),
									   DbHelper.MakeInParam("@startnum",(DbType)SqlDbType.Int,4,startNumber),
									   DbHelper.MakeInParam("@condition",(DbType)SqlDbType.VarChar,80,condition),
									   DbHelper.MakeInParam("@orderby",(DbType)SqlDbType.VarChar,80,orderFields),
									   DbHelper.MakeInParam("@ascdesc",(DbType)SqlDbType.Int,4,sortType)				                    
								   };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}gettopiclistbyvieworreplies", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 列新主题的回复数
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postTableid">回复表ID</param>
        public int UpdateTopicReplyCount(int tid, string postTableid)
        {
            string commandText = string.Format("UPDATE [{0}topics] SET [replies]=(SELECT COUNT([pid]) FROM [{0}posts{1}] WHERE [tid]={2}) - 1 WHERE [displayorder]>=0 AND [tid]={2}",
                                                BaseConfigs.GetTablePrefix,
                                                postTableid,
                                                tid);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }


        /// <summary>
        /// 得到当前版块内正常(未关闭)主题总数
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <returns>主题总数</returns>
        public int GetTopicCount(int fid)
        {
            DbParameter param = DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid);
            return TypeConverter.ObjectToInt(
                                 DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                        string.Format("{0}gettopiccount", BaseConfigs.GetTablePrefix),
                                                        param));
        }

        /// <summary>
        /// 得到当前版块内(包括子版)正常(未关闭)主题总数
        /// </summary>
        /// <param name="subfidList">子版块列表</param>
        /// <returns>主题总数</returns>
        public int GetTopicCountOfForumWithSub(string subfidList)
        {
            DbParameter param = DbHelper.MakeInParam("@subfidList", (DbType)SqlDbType.NChar, 400, subfidList);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                        string.Format("{0}getalltopiccount", BaseConfigs.GetTablePrefix),
                                                        param));
        }

        /// <summary>
        /// 得到当前版块内主题总数
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <returns>主题总数</returns>
        public int GetTopicCount(int fid, int state, string condition)
        {
            if (condition.Trim() == string.Empty)
            {
                DbParameter[] parms = {
									   DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int,4,fid),
								   };
                return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                         string.Format("{0}gettopiccount", BaseConfigs.GetTablePrefix),
                                                                         parms), -1);
            }
            else
            {
                DbParameter[] parms = {
									   DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int,4,fid),
									   DbHelper.MakeInParam("@state",(DbType)SqlDbType.Int,4,state),
									   DbHelper.MakeInParam("@condition",(DbType)SqlDbType.VarChar,80,condition)
								   };
                return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                         string.Format("{0}gettopiccountbycondition", BaseConfigs.GetTablePrefix),
                                                                         parms), -1);
            }
        }

        /// <summary>
        /// 得到符合条件的主题总数
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>主题总数</returns>
        public int GetTopicCount(string condition)
        {
            DbParameter param = DbHelper.MakeInParam("@condition", (DbType)SqlDbType.VarChar, 4000, condition);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                     string.Format("{0}gettopiccountbytype", BaseConfigs.GetTablePrefix),
                                                                     param), -1);
        }


        /// <summary>
        /// 更新主题为已被管理
        /// </summary>
        /// <param name="topiclist">主题id列表</param>
        /// <param name="moderated">管理操作id</param>
        /// <returns>成功返回1，否则返回0</returns>
        public int UpdateTopicModerated(string topicList, int moderated)
        {
            string commandText = string.Format("UPDATE [{0}topics] SET [moderated] = {1} WHERE [tid] IN ({2})",
                                                BaseConfigs.GetTablePrefix,
                                                moderated,
                                                topicList);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="topicinfo">主题信息</param>
        /// <returns>成功返回1，否则返回0</returns>
        public int UpdateTopic(TopicInfo topicInfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4, topicInfo.Tid),
									   DbHelper.MakeInParam("@fid", (DbType)SqlDbType.SmallInt, 2, topicInfo.Fid), 
									   DbHelper.MakeInParam("@iconid", (DbType)SqlDbType.SmallInt, 2, topicInfo.Iconid), 
									   DbHelper.MakeInParam("@title", (DbType)SqlDbType.NChar, 60, topicInfo.Title), 
									   DbHelper.MakeInParam("@typeid", (DbType)SqlDbType.SmallInt, 2, topicInfo.Typeid), 
									   DbHelper.MakeInParam("@readperm", (DbType)SqlDbType.Int, 4, topicInfo.Readperm), 
									   DbHelper.MakeInParam("@price", (DbType)SqlDbType.SmallInt, 2, topicInfo.Price), 
									   DbHelper.MakeInParam("@poster", (DbType)SqlDbType.NChar, 15, topicInfo.Poster), 
									   DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.Int, 4, topicInfo.Posterid), 
									   DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.SmallDateTime, 4, DateTime.Parse(topicInfo.Postdatetime)), 
									   DbHelper.MakeInParam("@lastpost", (DbType)SqlDbType.SmallDateTime, 0, topicInfo.Lastpost), 
                                       DbHelper.MakeInParam("@lastpostid", (DbType)SqlDbType.Int, 4, topicInfo.Lastpostid), 
									   DbHelper.MakeInParam("@lastposter", (DbType)SqlDbType.NChar, 15, topicInfo.Lastposter), 
									   DbHelper.MakeInParam("@replies", (DbType)SqlDbType.Int, 4, topicInfo.Replies), 
									   DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, topicInfo.Displayorder), 
									   DbHelper.MakeInParam("@highlight", (DbType)SqlDbType.VarChar, 500, topicInfo.Highlight), 
									   DbHelper.MakeInParam("@digest", (DbType)SqlDbType.Int, 4, topicInfo.Digest), 
									   DbHelper.MakeInParam("@rate", (DbType)SqlDbType.Int, 4, topicInfo.Rate), 
									   DbHelper.MakeInParam("@hide", (DbType)SqlDbType.Int, 4, topicInfo.Hide), 
									   DbHelper.MakeInParam("@special", (DbType)SqlDbType.Int, 4, topicInfo.Special), 
									   DbHelper.MakeInParam("@attachment", (DbType)SqlDbType.Int, 4, topicInfo.Attachment), 
									   DbHelper.MakeInParam("@moderated", (DbType)SqlDbType.Int, 4, topicInfo.Moderated), 
									   DbHelper.MakeInParam("@closed", (DbType)SqlDbType.Int, 4, topicInfo.Closed),
									   DbHelper.MakeInParam("@magic", (DbType)SqlDbType.Int, 4, topicInfo.Magic),
								   };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updatetopic", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 判断帖子列表是否都在当前板块
        /// </summary>
        /// <param name="topicidlist"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        public int GetTopicCountInForumAndTopicIdList(string topicIdList, int fid)
        {
            string commandText = string.Format("SELECT COUNT([tid]) FROM [{0}topics] WHERE [fid]={1} AND [tid] IN ({2})",
                                                BaseConfigs.GetTablePrefix,
                                                fid,
                                                topicIdList);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        /// <summary>
        /// 将主题设置为隐藏主题
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public int UpdateTopicHide(int tid)
        {
            string commandText = string.Format(@"UPDATE [{0}topics] SET [hide] = 1 WHERE [tid] = {1}",
                                                 BaseConfigs.GetTablePrefix,
                                                 tid);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public DataTable GetTopicList(int forumId, int pageId, int tpp)
        {
            string commandText;
            if (pageId == 1)
            {
                commandText = string.Format("SELECT TOP {0} [tid],[title],[replies] FROM [{1}topics] WHERE [fid]={2} AND [displayorder]>=0 ORDER BY [lastpostid] DESC",
                                             tpp,
                                             BaseConfigs.GetTablePrefix,
                                             forumId);
            }
            else
            {
                commandText = string.Format("SELECT TOP {0} [tid],[title],[replies] FROM [{1}topics] WHERE [lastpostid] < (SELECT MIN([lastpostid])  FROM (SELECT TOP {2} [lastpostid] FROM [{1}topics] WHERE [fid]={3} AND [displayorder]>=0 ORDER BY [lastpostid] DESC) AS tblTmp ) AND [fid]={3} AND [displayorder]>=0 ORDER BY [lastpostid] DESC",
                                             tpp,
                                             BaseConfigs.GetTablePrefix,
                                             (pageId - 1) * tpp,
                                             forumId);
            }
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public DataTable GetTopicFidByTid(string tidList)
        {
            string commandText = string.Format("SELECT DISTINCT [fid] FROM [{0}topics] WHERE [tid] IN({1})",
                                                BaseConfigs.GetTablePrefix,
                                                tidList);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public DataTable GetTopicTidByFid(string tidList, int fid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int,4, fid)
            };
            string commandText = string.Format("SELECT [tid] FROM [{0}topics] WHERE [tid] IN({1}) AND [fid]=@fid",
                                                BaseConfigs.GetTablePrefix,
                                                tidList);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        /// <summary>
        /// 更新主题浏览量
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="viewcount">浏览量</param>
        /// <returns>成功返回1，否则返回0</returns>
        public int UpdateTopicViewCount(int tid, int viewCount)
        {
            DbParameter[] parms = {
										DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid),	
										DbHelper.MakeInParam("@viewcount",(DbType)SqlDbType.Int,4,viewCount)			   
									};
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updatetopicviewcount", BaseConfigs.GetTablePrefix), parms);
        }

        public string SearchTopics(int forumId, string keyWord, string displayOrder, string digest, string attachment, string poster, bool lowerUpper, string viewsMin, string viewsMax,
                                        string repliesMax, string repliesMin, string rate, string lastPost, DateTime postDateTimeStart, DateTime postDateTimeEnd)
        {
            StringBuilder commandText = new StringBuilder(" [tid]>0");

            if (forumId != 0)
                commandText.AppendFormat(" AND [fid]={0}", forumId);

            if (keyWord != "")
            {
                commandText.Append(" AND (");
                foreach (string word in keyWord.Split(','))
                {
                    if (word.Trim() != "")
                        commandText.AppendFormat(" [title] LIKE '%{0}%' OR ", RegEsc(word));
                }
                commandText.Remove(commandText.Length - 3, 3).Append(")");
            }

            switch (displayOrder)
            {
                case "0":
                    break;
                case "1":
                    commandText.Append(" AND [displayorder]>0");
                    break;
                case "2":
                    commandText.Append(" AND [displayorder]<=0");
                    break;
            }

            switch (digest)
            {
                case "0":
                    break;
                case "1":
                    commandText.Append(" AND [digest]>=1");
                    break;
                case "2":
                    commandText.Append(" AND [digest]<1");
                    break;
            }

            switch (attachment)
            {
                case "0":
                    break;
                case "1":
                    commandText.Append(" AND [attachment]>0");
                    break;
                case "2":
                    commandText.Append(" AND [attachment]<=0");
                    break;
            }

            if (poster != "")
            {
                commandText.AppendFormat(" AND (");
                foreach (string postername in poster.Split(','))
                {
                    if (postername.Trim() != "")
                    {
                        //不区别大小写
                        if (lowerUpper)
                            commandText.AppendFormat(" [poster]='{0}' OR", postername);
                        else
                            commandText.AppendFormat(" [poster] COLLATE Chinese_PRC_CS_AS_WS ='{0}' OR", postername);
                    }
                }
                commandText.Remove(commandText.Length - 3, 3).Append(")");
            }

            if (viewsMax != "")
                commandText.AppendFormat(" AND [views]>{0}", viewsMax);

            if (viewsMin != "")
                commandText.AppendFormat(" AND [views]<{0}", viewsMin);

            if (repliesMax != "")
                commandText.AppendFormat(" AND [replies]>{0}", repliesMax);

            if (repliesMin != "")
                commandText.AppendFormat(" AND [replies]<{0}", repliesMin);

            if (rate != "")
                commandText.AppendFormat(" AND [rate]>{0}", rate);

            if (lastPost != "")
                commandText.AppendFormat(" AND DATEDIFF(day,[lastpost],GETDATE())>={0}", lastPost);

            return GetSqlstringByPostDatetime(commandText.ToString(), postDateTimeStart, postDateTimeEnd);
        }

        public string SearchAttachment(int forumId, string postTableName, string fileSizeMin, string fileSizeMax, string downLoadsMin, string downLoadsMax, string postDateTime, string fileName, string description, string poster)
        {
            StringBuilder commandText = new StringBuilder(" WHERE [aid] > 0");

            if (forumId != 0)
                commandText.AppendFormat(" AND [pid] IN (SELECT PID FROM [{0}] WHERE [fid]={1})", postTableName, forumId);

            if (fileSizeMin != "")
                commandText.AppendFormat(" AND [filesize]<{0}", fileSizeMin);

            if (fileSizeMax != "")
                commandText.AppendFormat(" AND [filesize]>{0}", fileSizeMax);

            if (downLoadsMin != "")
                commandText.AppendFormat(" AND [downloads]<{0}", downLoadsMin);

            if (downLoadsMax != "")
                commandText.AppendFormat(" AND [downloads]>{0}", downLoadsMax);

            if (postDateTime != "")
                commandText.AppendFormat(" AND DATEDIFF(day,[postdatetime],GETDATE())>={0}", postDateTime);

            if (fileName != "")
                commandText.AppendFormat(" AND [filename] like '%{0}%'", RegEsc(fileName));

            if (description != "")
            {
                commandText.Append(" AND (");
                foreach (string word in description.Split(','))
                {
                    if (word.Trim() != "")
                        commandText.AppendFormat(" [description] LIKE '%{0}%' OR ", RegEsc(word));
                }
                commandText.Remove(commandText.Length - 3, 3).Append(")");
            }

            if (poster != "")
                commandText.AppendFormat(" AND [pid] IN (SELECT [pid] FROM [{0}] WHERE [poster]='{0}')", postTableName, poster);

            return commandText.ToString();
        }

        public string SearchPost(int forumId, string postTableId, DateTime postDateTimeStart, DateTime postDateTimeEnd, string poster, bool lowerUpper, string ip, string message)
        {
            StringBuilder commandText = new StringBuilder(" [pid]>0 ");

            if (forumId != 0)
                commandText.AppendFormat(" AND [fid]={0}", forumId);

            commandText = new StringBuilder(GetSqlstringByPostDatetime(commandText.ToString(), postDateTimeStart, postDateTimeEnd));

            if (poster != "")
            {
                commandText.Append(" AND (");
                foreach (string postername in poster.Split(','))
                {
                    if (postername.Trim() != "")
                    {
                        //不区别大小写
                        if (lowerUpper)
                            commandText.AppendFormat(" [poster]='{0}' OR", postername);
                        else
                            commandText.AppendFormat(" [poster] COLLATE Chinese_PRC_CS_AS_WS ='{0}' OR", postername);
                    }
                }
                commandText.Remove(commandText.Length - 3, 3).Append(")");
            }

            if (ip != "")
                commandText.AppendFormat(" AND [ip] LIKE '%{0}%'", RegEsc(ip.Replace(".*", "")));

            if (message != "")
            {
                commandText.Append(" AND (");
                foreach (string messageinf in message.Split(','))
                {
                    if (messageinf.Trim() != "")
                        commandText.AppendFormat(" [message] LIKE '%{0}%' OR", RegEsc(messageinf));
                }
                commandText.Remove(commandText.Length - 3, 3).Append(")");
            }

            return commandText.ToString();
        }

        public void IdentifyTopic(string topicList, int identify)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@identify", (DbType)SqlDbType.Int, 4, identify)
            };
            string commandText = string.Format("UPDATE [{0}topics] SET [identify]=@identify WHERE [tid] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                topicList);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }


        public string GetTopicCountCondition(out string type, string getType, int getNewTopic)
        {
            type = "";
            string condition = "";

            if (getType == "digest")
            {
                type = "digest";
                condition += " AND digest>0 ";
            }

            if (getType == "newtopic")
            {
                type = "newtopic";
                condition += string.Format(" AND lastpost>='{0}'", DateTime.Now.AddMinutes(-getNewTopic).ToString("yyyy-MM-dd HH:mm:ss"));
            }
            return condition;
        }


        public string GetRateLogCountCondition(int userId, string postIdList)
        {
            return string.Format("[uid]={0} AND [pid] = {1}", userId, TypeConverter.StrToInt(postIdList));
        }

        public DataTable GetLastPostNotInPidList(string postIdList, int topicId, int postId)
        {
            string commandText = string.Format("SELECT TOP 1 {0} FROM [{1}posts{2}] WHERE [pid] NOT IN ({3}) AND [tid]={4} ORDER BY [pid] DESC",
                                                DbFields.POSTS,
                                                BaseConfigs.GetTablePrefix,
                                                postId,
                                                postIdList,
                                                topicId);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public void CreateTopicTags(string tags, int topicId, int userId, string curDateTime)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@tags", (DbType)SqlDbType.NVarChar, 55, tags),
                DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, topicId),
                DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userId),
                DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, curDateTime)                
            };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}createtopictags", BaseConfigs.GetTablePrefix), parms);
        }

        public IDataReader GetTopicListByTag(int tagId, int pageIndex, int pageSize)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@tagid", (DbType)SqlDbType.Int, 4, tagId),
                DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int, 4, pageIndex),
                DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int, 4, pageSize)
            };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}gettopiclistbytag", BaseConfigs.GetTablePrefix), parms);
        }

        public IDataReader GetRelatedTopics(int topicId, int count)
        {
            DbParameter[] parms = 
            {
                DbHelper.MakeInParam("@count", (DbType)SqlDbType.Int, 4, count),
                DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, topicId)
            };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getrelatedtopics", BaseConfigs.GetTablePrefix), parms);
        }

        public int GetTopicsCountByTag(int tagId)
        {
            DbParameter parm = DbHelper.MakeInParam("@tagid", (DbType)SqlDbType.Int, 4, tagId);

            string commandText = string.Format(@"SELECT COUNT(1) FROM [{0}topictags] AS [tt], [{0}topics] AS [t] WHERE [t].[tid] = [tt].[tid] AND [t].[displayorder] >=0 AND [tt].[tagid] = @tagid",
                                                 BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText, parm).Tables[0].Rows[0][0]);
        }

        public void NeatenRelateTopics()
        {
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format(@"{0}neatenrelatetopic", BaseConfigs.GetTablePrefix));
        }

        public void DeleteTopicTags(int topicId)
        {
            DbParameter parm = DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, topicId);
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}deletetopictags", BaseConfigs.GetTablePrefix), parm);
        }

        public void DeleteRelatedTopics(int topicId)
        {
            DbParameter param = DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, topicId);

            string commandText = string.Format("DELETE FROM [{0}topictagcaches] WHERE [tid] = @tid OR [linktid] = @tid",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, param);
        }

        public void AddBonusLog(int tid, int authorId, int winerId, string winnerName, int postId, int bonus, int extId, int isBest)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid),
                                    DbHelper.MakeInParam("@authorid", (DbType)SqlDbType.Int, 4, authorId),
                                    DbHelper.MakeInParam("@winerid", (DbType)SqlDbType.Int, 4, winerId),
                                    DbHelper.MakeInParam("@winnername", (DbType)SqlDbType.NChar, 20, winnerName),
                                    DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4, postId),
                                    DbHelper.MakeInParam("@dateline", (DbType)SqlDbType.DateTime, 4, DateTime.Now),
                                    DbHelper.MakeInParam("@bonus", (DbType)SqlDbType.Int, 4, bonus),
                                    DbHelper.MakeInParam("@extid", (DbType)SqlDbType.TinyInt, 1, extId),
                                    DbHelper.MakeInParam("@isbest", (DbType)SqlDbType.Int, 4, isBest)
                                  };

            string commandText = string.Format("INSERT INTO [{0}bonuslog] VALUES(@tid, @authorid, @winerid, @winnername, @postid, @dateline, @bonus, @extid, @isbest)",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public IDataReader GetPostDebate(int tid)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid)
                                  };
            string commandText = string.Format("SELECT [pid],[opinion] FROM [{0}postdebatefields]",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        public void CreateDebateTopic(DebateInfo debateTopic)
        {
            DbParameter[] parms = {
                                      DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, debateTopic.Tid),
                                      DbHelper.MakeInParam("@positiveopinion", (DbType)SqlDbType.NVarChar, 200, debateTopic.Positiveopinion),
                                      DbHelper.MakeInParam("@negativeopinion", (DbType)SqlDbType.NVarChar, 200, debateTopic.Negativeopinion),
                                      DbHelper.MakeInParam("@terminaltime", (DbType)SqlDbType.DateTime, 8, debateTopic.Terminaltime)                                
                                  };

            string commandText = string.Format(@"INSERT INTO [{0}debates]([tid], [positiveopinion], [negativeopinion], [terminaltime]) VALUES(@tid, @positiveopinion, @negativeopinion, @terminaltime)",
                                                 BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }


        public IDataReader GetDebateTopic(int tid)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid)
                                        
                                  };
            string commandText = string.Format("SELECT {0} FROM [{1}debates] WHERE [tid]=@tid",
                                                DbFields.DEBATES,
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        public bool UpdateDebateTopic(DebateInfo debateInfo)
        {
            DbParameter[] parms = {
                                      DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,debateInfo.Tid),
                                      DbHelper.MakeInParam("@positiveopinion",(DbType)SqlDbType.NVarChar,200,debateInfo.Positiveopinion),
                                      DbHelper.MakeInParam("@positivediggs",(DbType)SqlDbType.Int,4,debateInfo.Positivediggs),
                                      DbHelper.MakeInParam("@negativeopinion",(DbType)SqlDbType.NVarChar,200,debateInfo.Negativeopinion),
                                      DbHelper.MakeInParam("@negativediggs",(DbType)SqlDbType.Int,4,debateInfo.Negativediggs),
                                      DbHelper.MakeInParam("@terminaltime",(DbType)SqlDbType.DateTime,8,debateInfo.Terminaltime)
                                  };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}updatedebate", BaseConfigs.GetTablePrefix), parms);
            return true;
        }

        public IDataReader GetHotDebatesList(string hotField, int defHotCount, int getCount)
        {
            string commandText = string.Format("SELECT TOP {0} {1}, {2} FROM [{3}topics] AS t LEFT JOIN  [{3}debates] AS d ON t.[tid]=d.[tid] WHERE t.[{4}]>=[{5}] AND t.[special]=4",
                                                getCount,
                                                DbFields.TOPICS_JOIN,
                                                DbFields.DEBATES_JOIN,
                                                BaseConfigs.GetTablePrefix,
                                                hotField,
                                                defHotCount);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public void CreateDebatePostExpand(DebatePostExpandInfo dpei)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, dpei.Tid),
                                        DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 4, dpei.Pid),
                                        DbHelper.MakeInParam("@opinion", (DbType)SqlDbType.Int, 4, dpei.Opinion), 
                                        DbHelper.MakeInParam("@diggs", (DbType)SqlDbType.Int, 4, dpei.Diggs)
                                  };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}createdebatepostexpand", BaseConfigs.GetTablePrefix), parms);
        }

        public IDataReader GetRecommendDebates(string tidList)
        {
            string commandText = string.Format("SELECT  t.[tid],t.[title],t.[lastpost],t.[lastposter],{2} FROM [{0}topics] AS t LEFT JOIN [{0}debates] AS d ON t.[tid]=d.[tid] WHERE t.tid IN ({1}) AND t.[special]=4",
                                                BaseConfigs.GetTablePrefix,
                                                tidList,
                                                DbFields.DEBATES_JOIN);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public void AddCommentDabetas(int tid, int tableId, string commentMsg)
        {
            string commandText = string.Format("SELECT [message] FROM [{0}posts{1}] WHERE [tid]={2} AND [layer]=0",
                                                BaseConfigs.GetTablePrefix,
                                                tableId,
                                                tid);
            commandText = DbHelper.ExecuteScalarToStr(CommandType.Text, commandText);

            DbParameter[] parms = {
                                       DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid),
									   DbHelper.MakeInParam("@message",(DbType)SqlDbType.NText, 0, commandText + commentMsg)                                        
                                  };
            commandText = string.Format("UPDATE [{0}posts{1}] SET [message]=@message WHERE [tid]=@tid AND [layer]=0",
                                                BaseConfigs.GetTablePrefix,
                                                tableId);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

        }

        public void AddDebateDigg(int tid, int pid, int field, string ip, UserInfo userInfo)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid),
                                        DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 4, pid),
                                        DbHelper.MakeInParam("@debates", (DbType)SqlDbType.Int,4, field),
                                        DbHelper.MakeInParam("@diggerip",(DbType)SqlDbType.VarChar,15,ip),
                	                    DbHelper.MakeInParam("@diggdatetime", (DbType)SqlDbType.DateTime, 8, DateTime.Now),
                                        DbHelper.MakeInParam("@diggerid", (DbType)SqlDbType.Int, 4,userInfo.Uid),
                                        DbHelper.MakeInParam("@digger", (DbType)SqlDbType.NChar, 20,userInfo.Username)                                        
                                  };
            string commandText = string.Format("INSERT INTO [{0}debatediggs]([tid],[pid],[digger],[diggerid],[diggerip],[diggdatetime]) VALUES(@tid,@pid,@digger,@diggerid,@diggerip,@diggdatetime)",
                                                BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            commandText = string.Format("UPDATE  [{0}postdebatefields] SET [opinion]=@debates,[diggs]=[diggs]+1 WHERE [tid]=@tid AND [pid]=@pid",
                                         BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            commandText = string.Format("UPDATE  [{0}debates] SET {1}={1}+1 WHERE [tid]=@tid",
                                         BaseConfigs.GetTablePrefix,
                                         Enum.GetName(typeof(DebateType), field));
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }


        public bool AllowDiggs(int pid, int userId)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 4, pid),
                                        DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userId)                                        
                                  };
            string commandText = string.Format("SELECT  COUNT(0) FROM [{0}debatediggs] WHERE [pid]=@pid AND [diggerid]=@userid",
                                                BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms)) < 1;
        }

        public IDataReader GetDebatePostList(int tid, int opinion, int pageSize, int pageIndex, string postTableId, PostOrderType postOrderType)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid),
                                        DbHelper.MakeInParam("@opinion", (DbType)SqlDbType.Int, 4, opinion),
                                        DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int, 4, pageSize),
                                        DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int, 4, pageIndex)
                                  };
            string commandText = string.Format("{0}getdebatepostlist{1}", BaseConfigs.GetTablePrefix, postTableId);
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, commandText, parms);
        }

        public DataTable GetLastPostList(int fid, int count, string postTableName, string visibleForum)
        {
            string fidParam = "";
            if (fid > 0)
                fidParam = string.Format(" AND ([p].[fid] = {0} OR CHARINDEX(',{0},' , ',' + RTRIM([f].[parentidlist]) + ',') > 0 ) ", fid);

            visibleForum = !Utils.StrIsNullOrEmpty(visibleForum) ? string.Format(" AND [p].[fid] IN ({0})", visibleForum) : visibleForum;

            string commandText = string.Format("SELECT TOP {0} [p].[pid], [p].[fid], [p].[tid], [p].[layer], [p].[posterid], [p].[title], [p].[postdatetime], [p].[attachment], [p].[poster], [p].[posterid]  FROM [{1}] AS [p] LEFT JOIN [{2}forums] AS [f] ON [p].[fid] = [f].[fid] LEFT JOIN [{2}topics] AS [t] ON [p].[tid]=[t].[tid] WHERE [p].[layer]>0 AND [t].[closed]<>1 AND  [t].[displayorder] >=0 AND [p].[invisible]<>1 {3} {4} ORDER BY [p].[pid] DESC",
                                                count,
                                                postTableName,
                                                BaseConfigs.GetTablePrefix,
                                                fidParam,
                                                visibleForum);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public IDataReader GetUserDiggs(int tid, int uid)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid),
                                        DbHelper.MakeInParam("@diggerid", (DbType)SqlDbType.Int, 4, uid)

                                  };
            string commandText = string.Format("SELECT [pid] FROM [{0}debatediggs] WHERE [tid]=@tid AND [diggerid]=@diggerid",
                                                 BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        public int ReviseDebateTopicDiggs(int tid, int debateOpinion)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid),
                                        DbHelper.MakeInParam("@opinion", (DbType)SqlDbType.Int, 4, debateOpinion),
                                        DbHelper.MakeOutParam("@count", (DbType)SqlDbType.Int, 4)
                                  };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format(@"{0}revisedebatetopicdiggs", BaseConfigs.GetTablePrefix), parms);
            return TypeConverter.ObjectToInt(parms[2].Value);
        }

        public IDataReader GetDebatePostDiggs(string pidList)
        {
            if (!Utils.IsNumericList(pidList))
                return null;

            string commandText = string.Format("SELECT [pid],[diggs] FROM [{0}postdebatefields] WHERE [pid] IN ({1})",
                                                BaseConfigs.GetTablePrefix,
                                                pidList);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public int GetLastPostTid(ForumInfo forumInfo, string visibleForums)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, forumInfo.Fid),
                                        DbHelper.MakeInParam("@visibleforums", (DbType)SqlDbType.VarChar, 4000, visibleForums)
                                  };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}getlastposttid", BaseConfigs.GetTablePrefix), parms), -1);
        }

        public void SetPostsBanned(string tableId, string postListId, int invisible)
        {
            string commandText = String.Format("UPDATE [{0}posts{1}] SET [invisible]={2} WHERE [PID] IN ({3})",
                                                BaseConfigs.GetTablePrefix,
                                                tableId,
                                                invisible,
                                                postListId);
            DbHelper.ExecuteNonQuery(commandText);
        }

        public void SetTopicsBump(string tidList, int lastPostId)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@lastpostid", (DbType)SqlDbType.Int, 4, lastPostId)
                                  };

            string commandText = string.Format("UPDATE [{0}topics] SET [lastpostid]={1} WHERE [tid] IN ({2})",
                                                BaseConfigs.GetTablePrefix,
                                                lastPostId != 0 ? "@lastpostid" : string.Format("(SELECT MIN([lastpostid])-1 FROM [{0}topics] WHERE [fid]=[{0}topics].[fid])", BaseConfigs.GetTablePrefix),
                                                tidList);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public int GetPostId()
        {
            string commandText = string.Format("INSERT INTO [{0}postid] ([postdatetime]) VALUES(GETDATE());SELECT SCOPE_IDENTITY()",
                                                            BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText), -1);
        }

        public DataTable GetPostList(string postList, string tableId)
        {
            string commandText = string.Format("SELECT {0}FROM [{1}posts{2}] WHERE [pid] IN ({3})",
                                                DbFields.POSTS,
                                                BaseConfigs.GetTablePrefix,
                                                tableId,
                                                postList);
            DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, commandText);
            return (ds != null && ds.Tables.Count > 0) ? ds.Tables[0] : null;
        }

        public string GetTopicFilterCondition(string filter)
        {
            switch (filter.ToLower().Trim())//如果此处要加入新分支，则showforum.aspx.cs  中  SetSearchCondition()  方法中也需要添加一个，具体的看代码就明白了
            {
                case "poll":
                    return " AND [special] = 1 ";
                case "reward":
                    return " AND ([special] = 2 OR [special] = 3) ";
                case "rewarded":
                    return " AND [special] = 3 ";
                case "rewarding":
                    return " AND [special] = 2 ";
                case "debate":
                    return " AND [special] = 4 ";
                case "digest":
                    return " AND [digest] > 0 ";
                default: return "";
            }
        }


        public int GetDebatesPostCount(int tid, int debateOpinion)
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}postdebatefields] WHERE [tid] = {1} AND [opinion] ={2}",
                                                BaseConfigs.GetTablePrefix,
                                                tid,
                                                debateOpinion);
            return TypeConverter.StrToInt(DbHelper.ExecuteScalarToStr(CommandType.Text, commandText));
        }



        public void DeleteDebatePost(int tid, string opinion, int pid)
        {
            string commandText = string.Format("DELETE FROM [{0}postdebatefields] WHERE [pid]={1}",
                                                BaseConfigs.GetTablePrefix,
                                                pid);
            int postdebatefieldscount = DbHelper.ExecuteNonQuery(commandText);

            commandText = string.Format("DELETE FROM [{0}debatediggs] WHERE [pid]={1}",
                                         BaseConfigs.GetTablePrefix,
                                         pid);
            int debatediggscount = DbHelper.ExecuteNonQuery(commandText);

            commandText = string.Format("UPDATE [{3}DEBATES] SET {0}={0}-{1} WHERE [TID]={2}",
                                         opinion,
                                         postdebatefieldscount + debatediggscount,
                                         tid,
                                         BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(commandText);
        }

        private string GetAttentionSql(string keyword)
        {
            return string.Format(" AND ([title] LIKE '%{0}%' OR  [poster] LIKE '%{0}%' OR [lastpost] LIKE '%{0}%' OR [postdatetime] LIKE '%{0}%')",
                                   RegEsc(keyword));
        }


        public IDataReader GetAttentionTopics(string fidList, int tpp, int pageIndex, string keyWord)
        {
            string condition = (!Utils.StrIsNullOrEmpty(keyWord)) ? GetAttentionSql(keyWord) : "";

            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@fid", (DbType)SqlDbType.VarChar, 255, fidList),
                                        DbHelper.MakeInParam("@tpp", (DbType)SqlDbType.Int, 4, tpp),
                                        DbHelper.MakeInParam("@pageid", (DbType)SqlDbType.Int, 4, pageIndex),
                                        DbHelper.MakeInParam("@condition", (DbType)SqlDbType.NVarChar, 255, condition)                                        
                                  };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getattentiontopics", BaseConfigs.GetTablePrefix), parms);
        }

        public int GetAttentionTopicCount(string fidList, string keyWord)
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}topics] WHERE [displayorder]>=0 AND [attention]=1 {1} {2}",
                                                BaseConfigs.GetTablePrefix,
                                                fidList != "0" ? " AND [fid] IN (" + fidList + ")" : "",
                                                !Utils.StrIsNullOrEmpty(keyWord) ? GetAttentionSql(keyWord) : "");

            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalarToStr(CommandType.Text, commandText));
        }

        public void UpdateTopicAttentionByTidList(string tidList, int attention)
        {
            string commandText = string.Format("UPDATE [{0}topics] SET [attention]={1} WHERE [tid] IN ({2})",
                                                BaseConfigs.GetTablePrefix,
                                                attention,
                                                tidList);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public void UpdateTopicAttentionByFidList(string fidList, int attention, string dateTime)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@attention", (DbType)SqlDbType.Int, 4, attention),
                                        DbHelper.MakeInParam("@datetime", (DbType)SqlDbType.DateTime, 8, dateTime)
                                  };
            string sql = string.Format("UPDATE [{0}topics] SET [attention]=@attention WHERE {1} [postdatetime]<@datetime",
                                            BaseConfigs.GetTablePrefix,
                                            fidList != "0" ? " ([fid] IN (" + fidList + ")) AND " : "");

            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }


        public IDataReader GetNoUsedAttachmentListByUid(int userId, string posttime, int isimage)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,userId),
                                    DbHelper.MakeInParam("@posttime",(DbType)SqlDbType.DateTime,8,DBNull.Value),
                                    DbHelper.MakeInParam("@isimage",(DbType)SqlDbType.TinyInt,1,isimage)
                                  };
            if (posttime != "")
                parms[1].Value = posttime;
            return DbHelper.ExecuteReader(CommandType.StoredProcedure,
                                          string.Format("{0}getnousedattachmentlistbyuid",
                                          BaseConfigs.GetTablePrefix), parms);
        }

        public IDataReader GetEditPostAttachList(int userid, string aidList)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,userid),
                                    DbHelper.MakeInParam("@aidlist",(DbType)SqlDbType.VarChar,2000,aidList),
                                  };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure,
                                          string.Format("{0}geteditpostattachlist",
                                          BaseConfigs.GetTablePrefix), parms);
        }

        public int DeleteNoUsedForumAttachment()
        {
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}deletenousedforumattachment", BaseConfigs.GetTablePrefix));
        }

        public IDataReader GetNoUsedForumAttachment()
        {
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getnousedforumattachment", BaseConfigs.GetTablePrefix));
        }


        public IDataReader GetAttachPaymentLogByAid(int aid)
        {
            DbParameter[] parms ={
                                     DbHelper.MakeInParam("@aid",(DbType)SqlDbType.Int,4,aid)
                                 };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getattachpaymentlogbyaid", BaseConfigs.GetTablePrefix), parms);
        }

        public IDataReader GetAttachPaymentLogByUid(string attachIdList, int uid)
        {
            DbParameter[] parms = {
                                     DbHelper.MakeInParam("@attachidlist",(DbType)SqlDbType.VarChar,2000,attachIdList),
                                     DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid)
                                  };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getattachpaymentlogbyuid", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 获取用户单位时间内的发帖数
        /// </summary>
        /// <param name="topNumber">Top条数</param>
        /// <param name="dateType">时间类型</param>
        /// <param name="dateNum">时间数</param>
        /// <param name="postTableName">当前帖子分表名</param>
        /// <returns></returns>
        public IDataReader GetUserPostCountList(int topNumber, DateType dateType, int dateNum, string postTableName)
        {
            string dateTypeStr = "day";
            switch (dateType)
            {
                case DateType.Minute: dateTypeStr = "minute"; break;
                case DateType.Hour: dateTypeStr = "hour"; break;
                case DateType.Day: dateTypeStr = "day"; break;
                case DateType.Week: dateTypeStr = "week"; break;
                case DateType.Month: dateTypeStr = "month"; break;
                case DateType.Year: dateTypeStr = "year"; break;
            }

            string commandText = string.Format("SELECT TOP {0} [posterid] As [uid], [poster] As [username], COUNT(pid) AS [postcount] FROM  [{1}] WHERE [posterid] > 0 AND [invisible] <=0 AND DATEDIFF({2}, [postdatetime], GETDATE()) <= {3} GROUP BY [posterid], [poster] ORDER BY [postcount] DESC",
                                        topNumber,
                                        postTableName,
                                        dateTypeStr,
                                        dateNum);

            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }
    }
}
