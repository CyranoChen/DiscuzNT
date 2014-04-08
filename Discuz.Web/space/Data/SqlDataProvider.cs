using System;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;

using Discuz.Data;
using Discuz.Entity;
using Discuz.Config;
using System.Data.Common;
using Discuz.Common;
using Discuz.Common.Generic;

namespace Discuz.Space.Data
{
    public class DataProvider 
    {
        /// <summary>
        /// SQL SERVER SQL语句转义
        /// </summary>
        /// <param name="str">需要转义的关键字符串</param>
        /// <param name="pattern">需要转义的字符数组</param>
        /// <returns>转义后的字符串</returns>
        private string RegEsc(string str)
        {
            string[] pattern = { @"%", @"_", @"'" };
            foreach (string s in pattern)
            {
                switch (s)
                {
                    case "%":
                        str = str.Replace(s, "[%]");
                        break;
                    case "_":
                        str = str.Replace(s, "[_]");
                        break;
                    case "'":
                        str = str.Replace(s, "['']");
                        break;
                }
            }
            return str;
        }

        private DbParameter[] GetDateSpanParms(string startdate, string enddate)
        {
            DbParameter[] parms = new DbParameter[2];
            if (!Utils.StrIsNullOrEmpty(startdate))
                parms[0] = DbHelper.MakeInParam("@startdate", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(startdate));

            if (!Utils.StrIsNullOrEmpty(enddate))
                parms[1] = DbHelper.MakeInParam("@enddate", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(enddate).AddDays(1));

            return parms;
        }

        public int AddSpacePhoto(PhotoInfo photoinfo)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4,photoinfo.Userid),
                    DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar, 20, photoinfo.Username),
					DbHelper.MakeInParam("@title", (DbType)SqlDbType.NChar, 20,photoinfo.Title),
					DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4,photoinfo.Albumid),
					DbHelper.MakeInParam("@filename", (DbType)SqlDbType.NVarChar, 255,photoinfo.Filename),
					DbHelper.MakeInParam("@attachment", (DbType)SqlDbType.NVarChar, 255,photoinfo.Attachment),
					DbHelper.MakeInParam("@filesize", (DbType)SqlDbType.Int, 4,photoinfo.Filesize),
					DbHelper.MakeInParam("@description", (DbType)SqlDbType.NVarChar, 200,photoinfo.Description),
                    DbHelper.MakeInParam("@isattachment",(DbType)SqlDbType.Int,4,photoinfo.IsAttachment),
                    DbHelper.MakeInParam("@commentstatus", (DbType)SqlDbType.TinyInt, 1, (byte)photoinfo.Commentstatus),
                    DbHelper.MakeInParam("@tagstatus", (DbType)SqlDbType.TinyInt, 1, (byte)photoinfo.Tagstatus)
				};
            string commandText = String.Format("INSERT INTO [{0}photos] ([userid], [username], [title], [albumid], [filename], [attachment], [filesize], [description],[isattachment],[commentstatus], [tagstatus]) VALUES ( @userid, @username, @title, @albumid, @filename, @attachment, @filesize, @description,@isattachment, @commentstatus, @tagstatus);SELECT SCOPE_IDENTITY()", BaseConfigs.GetTablePrefix);
            //向关联表中插入相关数据
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms));
        }

        public int GetSpacePhotoCountByAlbumId(int albumid)
        {
            return TypeConverter.ObjectToInt(
                                 DbHelper.ExecuteScalar(CommandType.Text, 
                                                        string.Format("SELECT COUNT(1) FROM [{0}photos] WHERE [albumid]=@albumid", BaseConfigs.GetTablePrefix), 
                                                        DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4, albumid)));
        }

        public bool SaveSpaceAlbum(AlbumInfo spaceAlbum)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4, spaceAlbum.Albumid),
					DbHelper.MakeInParam("@albumcateid", (DbType)SqlDbType.Int, 4, spaceAlbum.Albumcateid),
					DbHelper.MakeInParam("@title", (DbType)SqlDbType.NChar, 50,spaceAlbum.Title),
					DbHelper.MakeInParam("@description", (DbType)SqlDbType.NChar, 200,spaceAlbum.Description),
					DbHelper.MakeInParam("@password", (DbType)SqlDbType.NChar, 50,spaceAlbum.Password),
					DbHelper.MakeInParam("@imgcount", (DbType)SqlDbType.Int, 4,spaceAlbum.Imgcount),
					DbHelper.MakeInParam("@logo", (DbType)SqlDbType.NChar, 255, spaceAlbum.Logo),
					DbHelper.MakeInParam("@type", (DbType)SqlDbType.Int, 8,spaceAlbum.Type)
				};
            string commandText = String.Format("UPDATE [{0}albums] SET [albumcateid] = @albumcateid, [title] = @title, [description] = @description, [password] = @password, [imgcount] = @imgcount, [logo] = @logo, [type] = @type WHERE [albumid] = @albumid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            return true;
        }

        public IDataReader GetSpaceConfigDataByUserID(int userid)
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}spaceconfigs] WHERE [userid] = {1}", BaseConfigs.GetTablePrefix, userid));
        }


        /// <summary>
        /// 保存用户space配置信息
        /// </summary>
        /// <param name="spaceconfiginfo"></param>
        /// <returns></returns>
        public bool SaveSpaceConfigData(SpaceConfigInfo spaceconfiginfo)
        {
            DbParameter[] parms = {
										   DbHelper.MakeInParam("@spacetitle", (DbType)SqlDbType.NVarChar, 100, spaceconfiginfo.Spacetitle),
										   DbHelper.MakeInParam("@description", (DbType)SqlDbType.NVarChar, 200, spaceconfiginfo.Description),
										   DbHelper.MakeInParam("@blogdispmode", (DbType)SqlDbType.TinyInt, 1, spaceconfiginfo.BlogDispMode),
										   DbHelper.MakeInParam("@bpp", (DbType)SqlDbType.Int, 4, spaceconfiginfo.Bpp),
										   DbHelper.MakeInParam("@commentpref", (DbType)SqlDbType.TinyInt, 1, spaceconfiginfo.Commentpref),
										   DbHelper.MakeInParam("@messagepref", (DbType)SqlDbType.TinyInt, 1, spaceconfiginfo.MessagePref),
										   DbHelper.MakeInParam("@themeid", (DbType)SqlDbType.Int, 4, spaceconfiginfo.ThemeID),
										   DbHelper.MakeInParam("@themepath", (DbType)SqlDbType.NChar, 50, spaceconfiginfo.ThemePath),
										   DbHelper.MakeInParam("@status", (DbType)SqlDbType.Int, 4, spaceconfiginfo.Status),
										   DbHelper.MakeInParam("@updatedatetime", (DbType)SqlDbType.SmallDateTime, 4, DateTime.Now),
										   DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, spaceconfiginfo.UserID)
									   };
            string commandText = string.Format("UPDATE [{0}spaceconfigs] SET [spacetitle] = @spacetitle ,[description] = @description,[blogdispmode] = @blogdispmode,[bpp] = @bpp, [commentpref] = @commentpref, [messagepref] = @messagepref, [themeid]=@themeid,[themepath] = @themepath, [updatedatetime] = @updatedatetime WHERE [userid] = @userid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            return true;
        }


        /// <summary>
        /// 建议用户space信息
        /// </summary>
        /// <param name="spaceconfiginfo"></param>
        /// 
        /// <returns></returns>
        public int AddSpaceConfigData(SpaceConfigInfo spaceconfiginfo)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4,spaceconfiginfo.UserID),
					DbHelper.MakeInParam("@spacetitle", (DbType)SqlDbType.NChar, 100,spaceconfiginfo.Spacetitle),
					DbHelper.MakeInParam("@description", (DbType)SqlDbType.NChar, 200,spaceconfiginfo.Description),
					DbHelper.MakeInParam("@blogdispmode", (DbType)SqlDbType.TinyInt, 1,spaceconfiginfo.BlogDispMode),
					DbHelper.MakeInParam("@bpp", (DbType)SqlDbType.Int, 4,spaceconfiginfo.Bpp),
					DbHelper.MakeInParam("@commentpref", (DbType)SqlDbType.TinyInt, 1,spaceconfiginfo.Commentpref),
					DbHelper.MakeInParam("@messagepref", (DbType)SqlDbType.TinyInt, 1,spaceconfiginfo.MessagePref),
					DbHelper.MakeInParam("@rewritename", (DbType)SqlDbType.Char, 100,spaceconfiginfo.Rewritename),
					DbHelper.MakeInParam("@themeid", (DbType)SqlDbType.Int, 4,spaceconfiginfo.ThemeID),
					DbHelper.MakeInParam("@themepath", (DbType)SqlDbType.NChar, 50,spaceconfiginfo.ThemePath),
					DbHelper.MakeInParam("@postcount", (DbType)SqlDbType.Int, 4,spaceconfiginfo.PostCount),
					DbHelper.MakeInParam("@commentcount", (DbType)SqlDbType.Int, 4,spaceconfiginfo.CommentCount),
					DbHelper.MakeInParam("@visitedtimes", (DbType)SqlDbType.Int, 4,spaceconfiginfo.VisitedTimes),
					DbHelper.MakeInParam("@createdatetime", (DbType)SqlDbType.SmallDateTime, 4,spaceconfiginfo.CreateDateTime),
					DbHelper.MakeInParam("@updatedatetime", (DbType)SqlDbType.SmallDateTime, 4,spaceconfiginfo.UpdateDateTime),
					DbHelper.MakeInParam("@status", (DbType)SqlDbType.Int, 4, spaceconfiginfo.Status),
				};
            string commandText = string.Format("INSERT INTO [{0}spaceconfigs] ([userid], [spacetitle], [description], [blogdispmode], [bpp], [commentpref], [messagepref], [rewritename], [themeid], [themepath], [postcount], [commentcount], [visitedtimes], [createdatetime], [updatedatetime]) VALUES (@userid, @spacetitle, @description, @blogdispmode, @bpp, @commentpref, @messagepref, @rewritename, @themeid, @themepath, @postcount, @commentcount, @visitedtimes, @createdatetime, @updatedatetime);SELECT SCOPE_IDENTITY()", BaseConfigs.GetTablePrefix);

            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms));
        }

        /// <summary>
        /// 为当前用户的SPACE访问量加1
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool CountUserSpaceVisitedTimesByUserID(int userid)
        {
            string commandText = string.Format("UPDATE [{0}spaceconfigs] SET [visitedtimes] = [visitedtimes] + 1 WHERE [userid] = @userid", BaseConfigs.GetTablePrefix);
            
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid));
           
            return true;
        }


        /// <summary>
        /// 更新当前用户的SPACE日志数
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public bool CountUserSpacePostCountByUserID(int userid, int postcount)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@postcount", (DbType)SqlDbType.Int, 4,postcount),
					DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4,userid)
				};

            string commandText = string.Format("UPDATE [{0}spaceconfigs] SET [postcount] = [postcount] + @postcount  WHERE [userid] = @userid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;
        }

        /// <summary>
        /// 更新当前用户的SPACE评论数
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public bool CountUserSpaceCommentCountByUserID(int userid, int commentcount)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@commentcount", (DbType)SqlDbType.Int, 4,commentcount),
					DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4,userid)
				};

            string commandText = string.Format("UPDATE [{0}spaceconfigs] SET [commentcount] = [commentcount] + @commentcount  WHERE [userid] = @userid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;         
        }

        public bool AddSpaceComment(SpaceCommentInfo spacecomments)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4,spacecomments.PostID),
					DbHelper.MakeInParam("@author", (DbType)SqlDbType.NVarChar, 50,spacecomments.Author),
					DbHelper.MakeInParam("@email", (DbType)SqlDbType.NVarChar, 100,spacecomments.Email),
					DbHelper.MakeInParam("@url", (DbType)SqlDbType.NVarChar, 255,spacecomments.Url),
					DbHelper.MakeInParam("@ip", (DbType)SqlDbType.VarChar, 100,spacecomments.Ip),
					DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.SmallDateTime, 4,spacecomments.PostDateTime),
					DbHelper.MakeInParam("@content", (DbType)SqlDbType.NText, 0,spacecomments.Content),
					DbHelper.MakeInParam("@parentid", (DbType)SqlDbType.Int, 4,spacecomments.ParentID),
					DbHelper.MakeInParam("@posttitle", (DbType)SqlDbType.NVarChar, 60,spacecomments.PostTitle),
					DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,spacecomments.Uid)
				};
            string commandText = string.Format("INSERT INTO [{0}spacecomments] ( [postid], [author], [email], [url], [ip], [postdatetime], [content], [parentid], [uid],[posttitle] ) VALUES ( @postid, @author, @email, @url, @ip, @postdatetime, @content, @parentid, @uid, @posttitle)", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="commentidList">删除评论的commentid列表</param>
        /// <returns></returns>
        public bool DeleteSpaceComments(string commentidList, int userid)
        {
            if (!Utils.IsNumericArray(commentidList.Split(',')))
                return false;

            try
            {
                string commandText = string.Format(@"DELETE FROM [{0}spacecomments] FROM [{0}spaceposts] WHERE [{0}spaceposts].[postid] = [{0}spacecomments].[postid] AND [{0}spaceposts].[uid]={1} AND [{0}spacecomments].[commentid] IN ({2})", 
                                                   BaseConfigs.GetTablePrefix, 
                                                   userid, 
                                                   commentidList);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteSpaceComments(int userid)
        {
            try
            {
                string commandText = string.Format("DELETE FROM [{0}spacecomments] WHERE [uid]={1}", BaseConfigs.GetTablePrefix, userid);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="commentid">删除评论的commentid</param>
        /// <returns></returns>
        public bool DeleteSpaceComment(int commentid)
        {
            try
            {
                string commandText = string.Format("DELETE FROM [{0}spacecomments] WHERE [commentid] = {1}", BaseConfigs.GetTablePrefix, commentid);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
                return true;
            }
            catch
            {
                return false;
            }
        }        

        /// <summary>
        /// 返回指定页数与条件的评论列表
        /// </summary>
        /// <param name="pageSize">每页的记录数</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="userid">用户ID</param>
        /// <param name="orderbyASC">排序方式，true为升序，false为降序</param>
        /// <returns></returns>
        public DataTable GetSpaceCommentList(int pageSize, int currentPage, int userid, bool orderbyASC)
        {
            try
            {
                DbParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
                string ordertype = orderbyASC ? "ASC" : "DESC";
                int pageTop = (currentPage - 1) * pageSize;

                string commandText = "";

                if (currentPage == 1)
                {
                    commandText = string.Format(@"SELECT TOP {0} [sc].* FROM  
                        [{1}spacecomments] AS [sc], [{1}spaceposts] AS [sp] WHERE [sc].[postid]=[sp].[postid] AND [sp].[uid]=@userid ORDER BY [sc].[commentid] {2}", pageSize, BaseConfigs.GetTablePrefix, ordertype);
                }
                else
                {
                    if (!orderbyASC)
                    {
                        commandText = string.Format(@"SELECT TOP {0} [sc].* FROM 
                            [{1}spacecomments] AS [sc], [{1}spaceposts] AS [sp] WHERE [commentid] < (SELECT min([commentid])  FROM 
                             (SELECT TOP {2} [sc1].[commentid] FROM [{1}spacecomments] AS [sc1], [{1}spaceposts] AS [sp1] WHERE 
                            [sc1].[postid]=[sp1].[postid] AND [sp1].[uid]=@userid ORDER BY [sc1].[commentid] {3}) AS tblTmp ) AND [sc].[postid]=[sp].[postid] AND [sp].[uid]=@userid ORDER BY [sc].[commentid] {3}", pageSize, BaseConfigs.GetTablePrefix, pageTop, ordertype);
                    }
                    else
                    {
                        commandText = string.Format(@"SELECT TOP {0} [sc].* FROM 
                            [{1}spacecomments] AS [sc], [{1}spaceposts] AS [sp] WHERE [commentid] > (SELECT MAX([commentid])  FROM 
                            (SELECT TOP {2} [commentid] FROM [{1}spacecomments] AS [sc1], [{1}spaceposts] AS [sp1] WHERE 
                            [sc1].[postid]=[sp1].[postid] AND [sp1].[uid]=@userid ORDER BY [commentid] {3}) AS tblTmp ) AND [sc].[postid]=[sp].[postid] AND [sp].[uid]=@userid ORDER BY [sc].[commentid] {3}", pageSize, BaseConfigs.GetTablePrefix, pageTop, ordertype);
                    }
                }
                return DbHelper.ExecuteDataset(CommandType.Text, commandText, parm).Tables[0];
            }
            catch
            {
                return new DataTable();
            }
        }

        public DataTable GetSpaceCommentListByPostid(int pageSize, int currentPage, int postid, bool orderbyASC)
        {
            try
            {
                DbParameter parm = DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4, postid);
                string ordertype = orderbyASC ? "ASC" : "DESC";
                int pageTop = (currentPage - 1) * pageSize;

                string commandText = "";

                if (currentPage == 1)
                    commandText = string.Format("SELECT TOP {0} * FROM [{1}spacecomments] WHERE [postid]=@postid ORDER BY [commentid] {2}", 
                                                 pageSize, BaseConfigs.GetTablePrefix, ordertype);
                else
                {
                    if (!orderbyASC)
                        commandText = string.Format("SELECT TOP {0} * FROM [{1}spacecomments] WHERE [commentid] < (SELECT min([commentid])  FROM "
                                             + "(SELECT TOP {2} [commentid] FROM [{1}spacecomments] WHERE [postid]=@postid ORDER BY [commentid] {3}) AS tblTmp ) AND [postid]=@postid ORDER BY [commentid] {3}",
                                             pageSize, BaseConfigs.GetTablePrefix, pageTop, ordertype);
                    else
                        commandText = string.Format("SELECT TOP {0} * FROM [{1}spacecomments] WHERE [commentid] > (SELECT MAX([commentid])  FROM "
                                             + "(SELECT TOP {2} [commentid] FROM [{1}spacecomments] WHERE [postid]=@postid ORDER BY [commentid] {3}) AS tblTmp ) AND [postid]=@postid ORDER BY [commentid] {3}",
                                             pageSize, BaseConfigs.GetTablePrefix, pageTop, ordertype);

                }
                return DbHelper.ExecuteDataset(CommandType.Text, commandText, parm).Tables[0];
            }
            catch
            {
                return new DataTable();
            }
        }


        /// <summary>
        /// 返回满足条件的评论数
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetSpaceCommentsCount(int userid)
        {
            try
            {
                return (int)DbHelper.ExecuteScalar(CommandType.Text, 
                                                   string.Format("SELECT COUNT([sc].[commentid]) FROM [{0}spacecomments] AS [sc], [{0}spaceposts] AS [sp] WHERE [sc].[postid]=[sp].[postid] AND [sp].[uid]=@userid", BaseConfigs.GetTablePrefix), 
                                                   DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid));
            }
            catch
            {
                return 0;
            }
        }

        public int GetSpaceCommentsCountByPostid(int postid)
        {
            try
            {
                return (int)DbHelper.ExecuteScalar(CommandType.Text, 
                                                  string.Format("SELECT COUNT([commentid]) FROM [{0}spacecomments] WHERE [postid]=@postid",BaseConfigs.GetTablePrefix),
                                                  DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4, postid));
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 返回全部评论数
        /// </summary>
        /// <returns></returns>
        public DataTable GetSpaceNewComments(int topcount, int userid)
        {
            try
            {
                string commandText = string.Format("SELECT TOP {0} * FROM [{1}spacecomments] WHERE [postid] IN (SELECT TOP 10 [postid] FROM [{1}spaceposts] WHERE [uid] = {2} AND [commentcount]>0 ORDER BY [postid] DESC) ORDER BY [commentid] DESC",
                                                   topcount, BaseConfigs.GetTablePrefix, userid);
                return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];

            }
            catch
            {
                return new DataTable();
            }
        }

        public int AddSpacePost(SpacePostInfo spaceposts)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4,spaceposts.Postid),
					DbHelper.MakeInParam("@author", (DbType)SqlDbType.NVarChar, 20,spaceposts.Author),
					DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,spaceposts.Uid),
					DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8,spaceposts.Postdatetime),
					DbHelper.MakeInParam("@content", (DbType)SqlDbType.NText, 0,spaceposts.Content),
					DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 150,spaceposts.Title),
					DbHelper.MakeInParam("@category", (DbType)SqlDbType.VarChar, 255,spaceposts.Category),
					DbHelper.MakeInParam("@poststatus", (DbType)SqlDbType.TinyInt, 1,spaceposts.PostStatus),
					DbHelper.MakeInParam("@commentstatus", (DbType)SqlDbType.TinyInt, 1,spaceposts.CommentStatus),
					DbHelper.MakeInParam("@postupdatetime", (DbType)SqlDbType.DateTime, 8,spaceposts.PostUpDateTime),
					DbHelper.MakeInParam("@commentcount", (DbType)SqlDbType.Int, 4,spaceposts.Commentcount)
				};
            string commandText = string.Format("INSERT INTO [{0}spaceposts] ([author], [uid], [postdatetime], [content], [title], [category], [poststatus], [commentstatus], [postupdatetime], [commentcount]) VALUES ( @author, @uid, @postdatetime, @content, @title, @category, @poststatus, @commentstatus, @postupdatetime, @commentcount);SELECT SCOPE_IDENTITY();", BaseConfigs.GetTablePrefix);

            //向关联表中插入相关数据
            int postid = TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms));

            commandText = string.Format("UPDATE [{0}spaceconfigs] SET [updatedatetime]=@postupdatetime WHERE [userid]=@uid", BaseConfigs.GetTablePrefix);
           
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            if (postid > 0)
            {
                foreach (string cateogryid in spaceposts.Category.Split(','))
                {
                    if (cateogryid != "")
                    {
                        SpacePostCategoryInfo spacepostCategoryInfo = new SpacePostCategoryInfo();
                        spacepostCategoryInfo.PostID = postid;
                        spacepostCategoryInfo.CategoryID = Convert.ToInt32(cateogryid);
                        AddSpacePostCategory(spacepostCategoryInfo);
                    }
                }
            }

            DbParameter[] prams1 = 
				{
					DbHelper.MakeInParam("@spacepostid", (DbType)SqlDbType.Int, 4, postid),
					DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,spaceposts.Uid)
				};
            //更新与当前日志关联的附件表中的数据
            DbHelper.ExecuteReader(CommandType.Text, 
                                   string.Format("UPDATE [{0}spaceattachments] SET [spacepostid] = @spacepostid  WHERE [spacepostid] = 0 AND [uid] = @uid ", BaseConfigs.GetTablePrefix), 
                                   prams1);
            //对当前用户日志加1
            CountUserSpacePostCountByUserID(spaceposts.Uid, 1);

            return postid;
        }

        public bool SaveSpacePost(SpacePostInfo spaceposts)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4, spaceposts.Postid),
					DbHelper.MakeInParam("@author", (DbType)SqlDbType.NVarChar, 20, spaceposts.Author),
					DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, spaceposts.Uid),
					DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, spaceposts.Postdatetime),
					DbHelper.MakeInParam("@content", (DbType)SqlDbType.NText, 0, spaceposts.Content),
					DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 150, spaceposts.Title),
					DbHelper.MakeInParam("@category", (DbType)SqlDbType.VarChar, 255, spaceposts.Category),
					DbHelper.MakeInParam("@poststatus", (DbType)SqlDbType.TinyInt, 1, spaceposts.PostStatus),
					DbHelper.MakeInParam("@commentstatus", (DbType)SqlDbType.TinyInt, 1, spaceposts.CommentStatus),
					DbHelper.MakeInParam("@postupdatetime", (DbType)SqlDbType.DateTime, 8, spaceposts.PostUpDateTime)
				};
            string commandText = string.Format("UPDATE [{0}spaceposts]  SET [author] = @author, [uid] = @uid, [postdatetime] = @postdatetime, [content] = @content, [title] = @title, [category] = @category, [poststatus] = @poststatus, [commentstatus] = @commentstatus, [postupdatetime] = @postupdatetime WHERE [postid] = @postid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            commandText = string.Format("UPDATE [{0}spaceconfigs] SET [updatedatetime]=@postupdatetime WHERE [userid]=@uid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            //先删除指定的日志关联数据再插入新数据
            DeleteSpacePostCategoryByPostID(spaceposts.Postid);

            foreach (string cateogryid in spaceposts.Category.Split(','))
            {
                if (cateogryid != "")
                {
                    SpacePostCategoryInfo spacepostCategoryInfo = new SpacePostCategoryInfo();
                    spacepostCategoryInfo.PostID = spaceposts.Postid;
                    spacepostCategoryInfo.CategoryID = Convert.ToInt32(cateogryid);
                    AddSpacePostCategory(spacepostCategoryInfo);
                }
            }
            return true;
        }

        public IDataReader GetSpacePost(int postid)
        {
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE  [postid]=" + postid);
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="postidList">删除日志的postid列表</param>
        /// <returns></returns>
        public bool DeleteSpacePosts(string postidList, int userid)
        {
            if (!Utils.IsNumericList(postidList))
                return false;

            string commandText = string.Format("DELETE FROM [{0}spaceposts] WHERE [postid] IN ({1}) AND [uid]={2}", BaseConfigs.GetTablePrefix, postidList, userid);
            int deletedCount = DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
            if (deletedCount > 0)
            {
                commandText = string.Format("UPDATE [{0}spaceconfigs] SET [postcount] = [postcount] - {1} WHERE [userid] = {2}", BaseConfigs.GetTablePrefix, deletedCount, userid);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
            }
            return true;
        }

        public bool DeleteSpacePosts(int userid)
        {
            string commandText = string.Format("DELETE FROM [{0}spaceposts] WHERE [uid] = {1}", BaseConfigs.GetTablePrefix, userid);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);

            commandText = string.Format("UPDATE [{0}spaceconfigs] SET [postcount] = 0 WHERE [userid] = {1}", BaseConfigs.GetTablePrefix, userid);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);

            return true;
        }

        /// <summary>
        /// 返回指定页数与条件的日志列表
        /// </summary>
        /// <param name="pageSize">每页的记录数</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public DataTable SpacePostsList(int pageSize, int currentPage, int userid)
        {
            DbParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
            int pageTop = (currentPage - 1) * pageSize;
            string commandText = "";
            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} * FROM [{1}spaceposts] WHERE [uid]=@userid ORDER BY [postid] DESC", pageSize, BaseConfigs.GetTablePrefix);
            else
                commandText = string.Format("SELECT TOP {0} * FROM [{1}spaceposts] WHERE [postid] < (SELECT min([postid])  FROM "
                                            + "(SELECT TOP {2} [postid] FROM [{1}spaceposts] WHERE [uid]=@userid ORDER BY [postid] DESC) AS tblTmp ) AND [uid]=@userid ORDER BY [postid] DESC",
                                            pageSize, BaseConfigs.GetTablePrefix, pageTop);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parm).Tables[0];
        }

        public DataTable SpacePostsList(int pageSize, int currentPage, int userid, int poststatus)
        {
            DbParameter[] parms = 
			{
			    DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
				DbHelper.MakeInParam("@poststatus", (DbType)SqlDbType.Int, 4, poststatus)
			};
            int pageTop = (currentPage - 1) * pageSize;
            string commandText = "";
            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} * FROM [{1}spaceposts] WHERE [uid]=@userid AND [poststatus]=@poststatus ORDER BY [postid] DESC",
                                            pageSize, BaseConfigs.GetTablePrefix);
            else
                commandText = string.Format("SELECT TOP {0} * FROM [{1}spaceposts] WHERE [postid] < (SELECT min([postid])  FROM "
                                            + "(SELECT TOP {2} [postid] FROM [{1}spaceposts] WHERE [uid]=@userid AND [poststatus]=@poststatus ORDER BY [postid] DESC) AS tblTmp ) AND [uid]=@userid AND [poststatus]=@poststatus ORDER BY [postid] DESC",
                                            pageSize, BaseConfigs.GetTablePrefix, pageTop);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        public DataTable SpacePostsList(int pageSize, int currentPage, int userid, DateTime postdatetime)
        {
            DbParameter[] parms = 
			{
			    DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
				DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, postdatetime)
			};

            int pageTop = (currentPage - 1) * pageSize;
            string commandText = "";
            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} * FROM [{1}spaceposts] WHERE [uid]=@userid AND [poststatus]=1 AND DATEDIFF(d, @postdatetime, postdatetime) = 0 ORDER BY [postid] DESC", 
                                            pageSize, BaseConfigs.GetTablePrefix);
            else
                commandText = string.Format("SELECT TOP {0} * FROM [{1}spaceposts] WHERE [postid] < (SELECT min([postid])  FROM "
                                            + "(SELECT TOP {2} [postid] FROM [{1}spaceposts] WHERE [uid]=@userid AND [poststatus]=1 AND DATEDIFF(d, @postdatetime, postdatetime) = 0 ORDER BY [postid] DESC) AS tblTmp ) "
                                            + "AND [uid]=@userid AND [poststatus]=1 AND DATEDIFF(d, @postdatetime, postdatetime) = 0 ORDER BY [postid] DESC",
                                            pageSize, BaseConfigs.GetTablePrefix, pageTop);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        /// <summary>
        /// 返回满足条件的日志数
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetSpacePostsCount(int userid)
        {
            try
            {
                return (int)DbHelper.ExecuteScalar(CommandType.Text, 
                                                   string.Format("SELECT COUNT([postid]) FROM [{0}spaceposts] WHERE uid=@userid", BaseConfigs.GetTablePrefix),
                                                   DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid));
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 返回满足条件的日志数
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="poststatus"></param>
        /// <returns></returns>
        public int GetSpacePostsCount(int userid, int poststatus)
        {
            try
            {
                DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
                    DbHelper.MakeInParam("@poststatus", (DbType)SqlDbType.Int, 4, poststatus)
				};
                return (int)DbHelper.ExecuteScalar(CommandType.Text, 
                                                   string.Format("SELECT COUNT([postid]) FROM [{0}spaceposts] WHERE [uid]=@userid AND [poststatus]=@poststatus", BaseConfigs.GetTablePrefix),
                                                   parms);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 返回满足条件的日志数
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="poststatus"></param>
        /// <returns></returns>
        public int GetSpacePostsCount(int userid, int poststatus, string postdatetime)
        {
            try
            {
                DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
                    DbHelper.MakeInParam("@poststatus", (DbType)SqlDbType.Int, 4, poststatus),
                    DbHelper.MakeInParam("@postdatetime",(DbType)SqlDbType.DateTime, 8, DateTime.Parse(postdatetime))
				};
                return (int)DbHelper.ExecuteScalar(CommandType.Text, 
                                                   string.Format("SELECT COUNT([postid]) FROM [{0}spaceposts] WHERE [uid]=@userid AND [poststatus]=@poststatus AND DATEDIFF(d, @postdatetime, postdatetime) = 0", BaseConfigs.GetTablePrefix),
                                                   parms);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 为当前用户的SPACE日志查看数加1
        /// </summary>
        /// <param name="postid"></param>
        /// <returns></returns>
        public bool CountUserSpacePostByUserID(int postid)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, 
                                     string.Format("UPDATE [{0}spaceposts] SET [views] = [views] + 1 WHERE [postid] = @postid", BaseConfigs.GetTablePrefix),
                                     DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4, postid));
            return true;
        }


        /// <summary>
        /// 更新当前日志数的评论数
        /// </summary>
        /// <param name="postid"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public bool CountSpaceCommentCountByPostID(int postid, int commentcount)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@commentcount", (DbType)SqlDbType.Int, 4,commentcount),
					DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4,postid)
				};

            if (commentcount >= 0)
                DbHelper.ExecuteNonQuery(CommandType.Text, 
                                         string.Format("UPDATE [{0}spaceposts] SET [commentcount] = [commentcount] + @commentcount  WHERE [postid] = @postid ", BaseConfigs.GetTablePrefix),
                                         parms);
            else
                DbHelper.ExecuteNonQuery(CommandType.Text, 
                                         string.Format("UPDATE [{0}spaceposts] SET [commentcount] = [commentcount] + @commentcount  WHERE [postid] = @postid AND [commentcount]>0", BaseConfigs.GetTablePrefix),
                                         parms);
            return true;
        }

        public IDataReader GetSpaceCategoryByCategoryID(int categoryid)
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}spacecategories] WHERE [categoryid] = {1}", BaseConfigs.GetTablePrefix, categoryid));
        }

        public bool AddSpaceCategory(SpaceCategoryInfo spacecategories)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4,spacecategories.CategoryID),
					DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 50,spacecategories.Title),
					DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,spacecategories.Uid),
					DbHelper.MakeInParam("@description", (DbType)SqlDbType.NVarChar, 1000,spacecategories.Description),
					DbHelper.MakeInParam("@typeid", (DbType)SqlDbType.Int, 4,spacecategories.TypeID),
					DbHelper.MakeInParam("@categorycount", (DbType)SqlDbType.Int, 4,spacecategories.CategoryCount),
					DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4,spacecategories.Displayorder)
				};
            string commandText = string.Format("INSERT INTO [{0}spacecategories] ( [title], [uid], [description], [typeid], [categorycount], [displayorder]) VALUES ( @title, @uid, @description, @typeid, @categorycount, @displayorder)", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;         
        }

        public bool SaveSpaceCategory(SpaceCategoryInfo spacecategories)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4,spacecategories.CategoryID),
					DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 50,spacecategories.Title),
					DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,spacecategories.Uid),
					DbHelper.MakeInParam("@description", (DbType)SqlDbType.NVarChar, 1000,spacecategories.Description),
					DbHelper.MakeInParam("@typeid", (DbType)SqlDbType.Int, 4,spacecategories.TypeID),
					DbHelper.MakeInParam("@categorycount", (DbType)SqlDbType.Int, 4,spacecategories.CategoryCount),
					DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4,spacecategories.Displayorder)
				};
            string commandText = string.Format("UPDATE [{0}spacecategories] SET  [title] = @title, [uid] = @uid, [description] = @description, [typeid] = @typeid, [categorycount] = @categorycount, [displayorder] = @displayorder WHERE [categoryid] = @categoryid ", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;    
        }

        /// <summary>
        ///	获取分类列表
        /// </summary>
        /// <param name="idList">分类的ID，以","分隔</param>
        /// <returns>返回分类名称列表</returns>
        public string GetCategoryNameByIdList(string idList)
        {
            if (idList.Trim() != "" && Utils.IsNumericList(idList))
            {

                string commandText = string.Format("SELECT [title] FROM [{0}spacecategories] WHERE [categoryid] IN ({1})", BaseConfigs.GetTablePrefix, idList);
                IDataReader categoryReader = DbHelper.ExecuteReader(CommandType.Text, commandText);
                string categoryNameList = "";

                while (categoryReader.Read())
                {
                    categoryNameList += categoryReader["title"].ToString() + ",";
                }
                categoryReader.Close();
                  
                return Utils.StrIsNullOrEmpty(categoryNameList) ? "" : 
                             categoryNameList.Substring(0, categoryNameList.Length - 1);           
            }
            else
                return "&nbsp;";
        }

        /// <summary>
        /// 根据用户id获取分类列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetSpaceCategoryListByUserId(int userid)
        {
            DbParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
            string commandText = string.Format("SELECT [categoryid], [title], [description] FROM [{0}spacecategories] WHERE [uid]=@userid ORDER BY [displayorder], [categoryid]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parm).Tables[0];
        }

        /// <summary>
        ///	获取分类列表
        /// </summary>
        /// <param name="idList">分类的ID, 以","分隔</param>
        /// <returns>返回分类名称列表</returns>
        public IDataReader GetCategoryIDAndName(string idList)
        {
            if (Utils.StrIsNullOrEmpty(idList))
                return null;
            if (!Utils.IsNumericList(idList))
                return null;

            return DbHelper.ExecuteReader(CommandType.Text, 
                                          string.Format("SELECT [categoryid],[title] FROM [{0}spacecategories] WHERE [categoryid] IN ({1})", BaseConfigs.GetTablePrefix, idList));
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="categoryidList">删除分类的categoryid列表</param>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        public bool DeleteSpaceCategory(string categoryidList, int userid)
        {
            if (!Utils.IsNumericList(categoryidList))
                return false; 

            try
            {
                //清除分类的categoryid列表相关信息
                string commandText = string.Format("DELETE FROM [{0}spacecategories] WHERE [categoryid] IN ({1}) AND [uid]={2}", BaseConfigs.GetTablePrefix, categoryidList, userid);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText);

                //清除分类的categoryid关联表
                commandText = string.Format("DELETE FROM [{0}spacepostcategories] WHERE [categoryid] IN ({1})", BaseConfigs.GetTablePrefix, categoryidList);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteSpaceCategory(int userid)
        {
            try
            {
                //清除分类的categoryid列表相关信息
                string commandText = string.Format("SELECT [categoryid] FROM [{0}spacecategories] WHERE [uid]={1}", BaseConfigs.GetTablePrefix, userid);
                string categoryidList = "";
                foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows)
                {
                    categoryidList += dr["categoryid"].ToString();
                }
                if (!Utils.StrIsNullOrEmpty(categoryidList))
                {
                    categoryidList = categoryidList.Substring(0, categoryidList.Length - 1);
                    //清除分类的categoryid关联表
                    commandText = string.Format("DELETE FROM [{0}spacepostcategories] WHERE [categoryid] IN ({1})", BaseConfigs.GetTablePrefix, categoryidList);
                    DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
                }
                commandText = string.Format("DELETE FROM [{0}spacecategories] WHERE [uid]={1}", BaseConfigs.GetTablePrefix, userid);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 返回指定页数与条件的分类列表
        /// </summary>
        /// <param name="pageSize">每页的记录数</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public DataTable GetSpaceCategoryList(int pageSize, int currentPage, int userid)
        {
            try
            {
                DbParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
                int pageTop = (currentPage - 1) * pageSize;
                string commandText = "";
                if (currentPage == 1)
                    commandText = string.Format("SELECT TOP {0} * FROM [{1}spacecategories] WHERE uid=@userid ORDER BY [categoryid] DESC", 
                                                pageSize, BaseConfigs.GetTablePrefix);
                else
                    commandText = string.Format("SELECT TOP {0} * FROM [{1}spacecategories] WHERE [categoryid] < (SELECT min([categoryid])  FROM "
                                                + "(SELECT TOP {2} [categoryid] FROM [{1}spacecategories] WHERE [uid]=@userid ORDER BY [categoryid] DESC) AS tblTmp ) AND [uid]=@userid ORDER BY [categoryid] DESC",
                                                pageSize, BaseConfigs.GetTablePrefix, pageTop);
                return DbHelper.ExecuteDataset(CommandType.Text, commandText, parm).Tables[0];
            }
            catch
            {
                return new DataTable();
            }
        }

        /// <summary>
        /// 返回满足条件的分类数
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public int GetSpaceCategoryCount(int userid)
        {
            try
            {
                return (int)DbHelper.ExecuteScalar(CommandType.Text, 
                                                   string.Format("SELECT COUNT([categoryid]) FROM [{0}spacecategories] WHERE [uid]=@userid", BaseConfigs.GetTablePrefix),
                                                   DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid));
            }
            catch
            {
                return 0;
            }
        }

        public bool AddSpacePostCategory(SpacePostCategoryInfo spacepostcategories)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4,spacepostcategories.ID),
					DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4,spacepostcategories.PostID),
					DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4,spacepostcategories.CategoryID)
				};
            string commandText = String.Format("INSERT INTO [{0}spacepostcategories] ([postid], [categoryid]) VALUES ( @postid, @categoryid)", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;
        }

        public bool DeleteSpacePostCategoryByPostID(int postid)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4,postid)
				};
            string commandText = string.Format("DELETE FROM [{0}spacepostcategories] WHERE [postid] = @postid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;
        }

        public bool AddSpaceAttachment(SpaceAttachmentInfo spaceattachments)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@aid", (DbType)SqlDbType.Int, 4,spaceattachments.AID),
					DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,spaceattachments.UID),
					DbHelper.MakeInParam("@spacepostid", (DbType)SqlDbType.Int, 4,spaceattachments.SpacePostID),
					DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8,spaceattachments.PostDateTime),
					DbHelper.MakeInParam("@filename", (DbType)SqlDbType.NChar, 100,spaceattachments.FileName),
					DbHelper.MakeInParam("@filetype", (DbType)SqlDbType.NChar, 50,spaceattachments.FileType),
					DbHelper.MakeInParam("@filesize", (DbType)SqlDbType.Float, 8,spaceattachments.FileSize),
					DbHelper.MakeInParam("@attachment", (DbType)SqlDbType.NChar, 100,spaceattachments.Attachment),
					DbHelper.MakeInParam("@downloads", (DbType)SqlDbType.Int, 4,spaceattachments.Downloads),
					
				};
            string commandText = string.Format("INSERT INTO [{0}spaceattachments] ( [uid], [spacepostid], [postdatetime], [filename], [filetype], [filesize], [attachment], [downloads]) VALUES ( @uid, @spacepostid, @postdatetime, @filename, @filetype, @filesize, @attachment, @downloads)", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;
        }

      
        /// <summary>
        /// 返回指定页数与条件的分类列表
        /// </summary>
        /// <param name="pageSize">每页的记录数</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public DataTable GetSpaceAttachmentList(int pageSize, int currentPage, int userid)
        {
            try
            {
                DbParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
                int pageTop = (currentPage - 1) * pageSize;
                string commandText = "";
                if (currentPage == 1)
                    commandText = string.Format("SELECT TOP {0} * FROM [{1}spaceattachments] WHERE [uid]=@userid ORDER BY [aid] DESC", 
                                                pageSize, BaseConfigs.GetTablePrefix);
                else
                {
                    commandText = string.Format("SELECT TOP {0} * FROM [{1}spaceattachments] WHERE [aid] < (SELECT min([aid])  FROM "
                                                + "(SELECT TOP {2} [aid] FROM [{1}spaceattachments] WHERE [uid]=@userid ORDER BY [aid] DESC) AS tblTmp ) AND [uid]=@userid ORDER BY [aid] DESC",
                                                pageSize, BaseConfigs.GetTablePrefix, pageTop);
                }
                return DbHelper.ExecuteDataset(CommandType.Text, commandText, parm).Tables[0];
            }
            catch
            {
                return new DataTable();
            }
        }

        /// <summary>
        /// 返回满足条件的分类数
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public int GetSpaceAttachmentCount(int userid)
        {
            try
            {
                return (int)DbHelper.ExecuteScalar(CommandType.Text, 
                                                   string.Format("SELECT COUNT([aid]) FROM [{0}spaceattachments] WHERE [uid]=@userid", BaseConfigs.GetTablePrefix),
                                                   DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid));
            }
            catch
            {
                return 0;
            }
        }


        /// <summary>
        /// 删除指定的附件记录和相关文件
        /// </summary>
        /// <param name="aidlist">附件ID串, 格式:1,3,5</param>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        public bool DeleteSpaceAttachmentByIDList(string aidlist, int userid)
        {
            if (!Utils.IsNumericList(aidlist))
                return false;

            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT [filename] FROM [" + BaseConfigs.GetTablePrefix + "spaceattachments] WHERE [aid] IN( " + aidlist + " ) AND [uid]=" + userid, null);
            string path = Utils.GetMapPath(BaseConfigs.GetForumPath);
            while (reader.Read())
            {
                try
                {
                    System.IO.File.Delete(path + reader[0].ToString());
                }
                catch { ;}
            }
            reader.Close();

            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE  FROM  [{0}spaceattachments] WHERE [aid] IN ({1})", BaseConfigs.GetTablePrefix, aidlist));

            return true;
        }


        /// <summary>
        /// 返回满足条件的友情链接数
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetSpaceLinkCount(int userid)
        {
            try
            {
                DbParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
                return (int)DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([linkid]) FROM [" + BaseConfigs.GetTablePrefix + "spacelinks] WHERE [userid]=@userid", parm);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 返回指定页数与条件的友情链接列表
        /// </summary>
        /// <param name="pageSize">每页的记录数</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public DataTable GetSpaceLinkList(int pageSize, int currentPage, int userid)
        {
            try
            {
                DbParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
                int pageTop = (currentPage - 1) * pageSize;
                string commandText = "";
                if (currentPage == 1)
                {
                    commandText = string.Format("SELECT TOP {0} * FROM [{1}spacelinks] WHERE [userid]=@userid ORDER BY [linkid] DESC", pageSize, BaseConfigs.GetTablePrefix);
                }
                else
                {
                    commandText = string.Format("SELECT TOP {0} * FROM [{1}spacelinks] WHERE [linkid] < (SELECT min([linkid])  FROM "
                                                + "(SELECT TOP {2} [linkid] FROM [{1}spacelinks] WHERE [userid]=@userid ORDER BY [linkid] DESC) AS tblTmp ) AND [userid]=@userid ORDER BY [linkid] DESC", 
                                                pageSize, BaseConfigs.GetTablePrefix, pageTop);
                }
                return DbHelper.ExecuteDataset(CommandType.Text, commandText, parm).Tables[0];
            }
            catch
            {
                return new DataTable();
            }
        }

        public IDataReader GetSpaceLinkByLinkID(int linkid)
        {
            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "spacelinks] WHERE [linkid] = " + linkid);
            return reader;
        }

        public bool SaveSpaceLink(SpaceLinkInfo spacelinks)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@linkid", (DbType)SqlDbType.Int, 4,spacelinks.LinkId),
					DbHelper.MakeInParam("@linktitle", (DbType)SqlDbType.NVarChar, 50,spacelinks.LinkTitle),
					DbHelper.MakeInParam("@linkurl", (DbType)SqlDbType.VarChar,255,spacelinks.LinkUrl),
					DbHelper.MakeInParam("@description", (DbType)SqlDbType.NVarChar, 200,spacelinks.Description),
				};
            string commandText = String.Format("UPDATE [{0}spacelinks] SET  [linktitle] = @linktitle, [linkurl] = @linkurl, [description] = @description WHERE [linkid] = @linkid ", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;
        }

        public bool AddSpaceLink(SpaceLinkInfo spacelinks)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@linkid", (DbType)SqlDbType.Int, 4,spacelinks.LinkId),
					DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4,spacelinks.UserId),
					DbHelper.MakeInParam("@linktitle", (DbType)SqlDbType.NVarChar, 50,spacelinks.LinkTitle),
					DbHelper.MakeInParam("@linkurl", (DbType)SqlDbType.VarChar,255,spacelinks.LinkUrl),
					DbHelper.MakeInParam("@description", (DbType)SqlDbType.NVarChar, 200,spacelinks.Description),
				};
            string commandText = String.Format("INSERT INTO [{0}spacelinks] ( [userid], [linktitle], [linkurl], [description]) VALUES ( @userid, @linktitle, @linkurl,  @description)", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;
        }

        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="linksList">删除友情链接的linkid列表</param>
        /// <returns></returns>
        public bool DeleteSpaceLink(string linksList, int userid)
        {
            if (!Utils.IsNumericList(linksList))
                return false;

            try
            {
                string commandText = string.Format("DELETE FROM [{0}spacelinks] WHERE [linkid] IN ({1}) AND userid={2}", BaseConfigs.GetTablePrefix, linksList, userid);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteSpaceLink(int userid)
        {
            try
            {
                string commandText = string.Format("DELETE FROM [{0}spacelinks] WHERE userid={1}", BaseConfigs.GetTablePrefix, userid);
                DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
                return true;
            }
            catch
            {
                return false;
            }
        }
    

        public IDataReader GetThemeInfos()
        {
            return RunSelectSql(string.Format(@"SELECT * FROM [{0}spacethemes] ORDER BY [type]", BaseConfigs.GetTablePrefix), null);
        }

        /// <summary>
        /// 修改指定的ModuleDef信息
        /// </summary>
        /// <param name="moduleDefInfo"></param>
        /// <returns></returns>
        public bool UpdateModuleDef(ModuleDefInfo moduleDefInfo)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@moduledefid", (DbType)SqlDbType.Int, 4, moduleDefInfo.ModuleDefID),
				DbHelper.MakeInParam("@modulename", (DbType)SqlDbType.NVarChar, 20, moduleDefInfo.ModuleName),
				DbHelper.MakeInParam("@cachetime", (DbType)SqlDbType.Int, 4, moduleDefInfo.CacheTime),
				DbHelper.MakeInParam("@configfile", (DbType)SqlDbType.VarChar, 100, moduleDefInfo.ConfigFile),
				DbHelper.MakeInParam("@controller", (DbType)SqlDbType.VarChar, 255, moduleDefInfo.BussinessController),
			};
            string commandText = string.Format(@"UPDATE [{0}spacemoduledefs] SET [modulename]=@modulename, [cachetime]=@cachetime, [configfile]=@configfile, [controller]=@controller WHERE [moduledefid]=@moduledefid", BaseConfigs.GetTablePrefix);
            return RunExecuteSql(commandText, parms);
        }

        public int GetModuleDefIdByUrl(string url)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@configfile", (DbType)SqlDbType.VarChar, 100, url),
			};

            string commandText = string.Format(@"SELECT [moduledefid] FROM [{0}spacemoduledefs] WHERE [configfile]=@configfile", BaseConfigs.GetTablePrefix);
            commandText = DbHelper.ExecuteScalarToStr(CommandType.Text, commandText, parms);
            return Utils.StrIsNullOrEmpty(commandText) ? 0 : Convert.ToInt32(commandText);
        }



        public int GetModulesCountByTabId(int tabId, int uid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@tabid", (DbType)SqlDbType.Int, 4, tabId),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
			};
            string commandText = string.Format(@"SELECT COUNT(1) FROM [{0}spacemodules] WHERE [tabid] = @tabid AND [uid]=@uid", BaseConfigs.GetTablePrefix);

            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms));
        }

        /// <summary>
        /// 根据TabId获得Modules集合
        /// </summary>
        /// <param name="tabId"></param>
        /// <returns></returns>
        public IDataReader GetModulesByTabId(int tabId, int uid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@tabid", (DbType)SqlDbType.Int, 4, tabId),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)	
			};
            string commandText = string.Format(@"SELECT * FROM [{0}spacemodules] WHERE [tabid] = @tabid AND [uid]=@uid ORDER BY [panename], [displayorder]", BaseConfigs.GetTablePrefix);

            return RunSelectSql(commandText, parms);
        }


        /// <summary>
        /// 根据ModuleId获得Module
        /// </summary>
        /// <param name="moduleInfoId"></param>
        /// <returns></returns>
        public IDataReader GetModuleInfoById(int moduleInfoId, int uid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@moduleid", (DbType)SqlDbType.Int, 4, moduleInfoId),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
			};
            string commandText = string.Format(@"SELECT * FROM [{0}spacemodules] WHERE [moduleid] = @moduleid AND [uid]=@uid", BaseConfigs.GetTablePrefix);

            return RunSelectSql(commandText, parms);
        }


        /// <summary>
        /// 添加Module至数据库
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public bool AddModule(ModuleInfo moduleInfo)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@moduleid", (DbType)SqlDbType.Int, 4, moduleInfo.ModuleID),
				DbHelper.MakeInParam("@tabid", (DbType)SqlDbType.Int, 4, moduleInfo.TabID),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, moduleInfo.Uid),
				DbHelper.MakeInParam("@moduledefid", (DbType)SqlDbType.Int, 4, moduleInfo.ModuleDefID),
				DbHelper.MakeInParam("@panename", (DbType)SqlDbType.VarChar, 10, moduleInfo.PaneName),
				DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, moduleInfo.DisplayOrder),
				DbHelper.MakeInParam("@userpref", (DbType)SqlDbType.NVarChar, 4000, moduleInfo.UserPref),
				DbHelper.MakeInParam("@val", (DbType)SqlDbType.TinyInt, 1, moduleInfo.Val),
				DbHelper.MakeInParam("@moduleurl", (DbType)SqlDbType.VarChar, 255, moduleInfo.ModuleUrl),
				DbHelper.MakeInParam("@moduletype", (DbType)SqlDbType.TinyInt, 2, moduleInfo.ModuleType)
			};
            string commandText = string.Format(@"INSERT INTO [{0}spacemodules]([moduleid], [tabid], [uid], [moduledefid], [panename], [displayorder], [userpref], [val], [moduleurl], [moduletype]) VALUES(@moduleid, @tabid, @uid, @moduledefid, @panename, @displayorder, @userpref, @val, @moduleurl, @moduletype)", BaseConfigs.GetTablePrefix);

            return RunExecuteSql(commandText, parms);
        }

        /// <summary>
        /// 更新指定的Module信息
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public bool UpdateModule(ModuleInfo moduleInfo)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@moduleid", (DbType)SqlDbType.Int, 4, moduleInfo.ModuleID),
				DbHelper.MakeInParam("@tabid", (DbType)SqlDbType.Int, 4, moduleInfo.TabID),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, moduleInfo.Uid),
				DbHelper.MakeInParam("@moduledefid", (DbType)SqlDbType.Int, 4, moduleInfo.ModuleDefID),
				DbHelper.MakeInParam("@panename", (DbType)SqlDbType.VarChar, 10, moduleInfo.PaneName),
				DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, moduleInfo.DisplayOrder),
				DbHelper.MakeInParam("@userpref", (DbType)SqlDbType.NVarChar, 4000, moduleInfo.UserPref),
				DbHelper.MakeInParam("@val", (DbType)SqlDbType.TinyInt, 1, moduleInfo.Val),
				DbHelper.MakeInParam("@moduleurl", (DbType)SqlDbType.VarChar, 255, moduleInfo.ModuleUrl),
				DbHelper.MakeInParam("@moduletype", (DbType)SqlDbType.TinyInt, 2, moduleInfo.ModuleType)
			};
            string commandText = string.Format(@"UPDATE [{0}spacemodules] SET [tabid]=@tabid, [moduledefid]=@moduledefid, [panename]=@panename, [displayorder]=@displayorder,[userpref]=@userpref,[val]=@val, moduleurl=@moduleurl, moduletype=@moduletype WHERE [moduleid]=@moduleid AND [uid]=@uid", BaseConfigs.GetTablePrefix);

            return RunExecuteSql(commandText, parms);
        }

        /// <summary>
        /// 删除指定的Module信息
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public bool DeleteModule(int moduleId, int uid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@moduleid", (DbType)SqlDbType.Int, 4, moduleId),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
			};

            string commandText = string.Format(@"DELETE FROM [{0}spacemodules] WHERE [moduleid]=@moduleid AND [uid]=@uid", BaseConfigs.GetTablePrefix);
            return RunExecuteSql(commandText, parms);
        }

        /// <summary>
        /// 为模块排序
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="panename"></param>
        /// <param name="displayorder"></param>
        public void UpdateModuleOrder(int mid, int uid, string panename, int displayorder)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@moduleid", (DbType)SqlDbType.Int, 4, mid),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
				DbHelper.MakeInParam("@panename", (DbType)SqlDbType.VarChar, 10, panename),
				DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder)
			};
            string commandText = string.Format(@"UPDATE [{0}spacemodules] SET [panename]=@panename, [displayorder]=@displayorder WHERE [moduleid]=@moduleid AND [uid]=@uid", BaseConfigs.GetTablePrefix);
            RunExecuteSql(commandText, parms);
        }

        public void UpdateModuleTab(int moduleid, int uid, int tabid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@moduleid", (DbType)SqlDbType.Int, 4, moduleid),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
				DbHelper.MakeInParam("@tabid", (DbType)SqlDbType.Int, 4, tabid)
			};
            string commandText = string.Format(@"UPDATE [{0}spacemodules] SET [displayorder]=0, [tabid]=@tabid WHERE [moduleid]=@moduleid AND [uid]=@uid", BaseConfigs.GetTablePrefix);
            RunExecuteSql(commandText, parms);
        }

        public int GetMaxModuleIdByUid(int userid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userid)
			};
            string commandText = string.Format(@"SELECT TOP 1 [moduleid] FROM [{0}spacemodules] WHERE [uid]=@uid ORDER BY [moduleid] DESC", BaseConfigs.GetTablePrefix);
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms));
        }

     
        /// <summary>
        /// 根据Uid获得Tab集合
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public IDataReader GetTabInfosByUid(int uid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
			};
            string commandText = string.Format(@"SELECT * FROM [{0}spacetabs] WHERE [uid]=@uid ORDER BY [tabid] ASC", BaseConfigs.GetTablePrefix);

            return RunSelectSql(commandText, parms);
        }

        /// <summary>
        /// 根据TabId获得Tab
        /// </summary>
        /// <param name="tabInfoId"></param>
        /// <returns></returns>
        public IDataReader GetTabInfoById(int tabInfoId, int uid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@tabid", (DbType)SqlDbType.Int, 4, tabInfoId),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)	
			};
            string commandText = string.Format(@"SELECT * FROM [{0}spacetabs] WHERE [tabid] = @tabid AND [uid]=@uid", BaseConfigs.GetTablePrefix);

            return RunSelectSql(commandText, parms);
        }

        /// <summary>
        /// 添加Tab信息至数据库
        /// </summary>
        /// <param name="tabInfo"></param>
        /// <returns></returns>
        public bool AddTab(TabInfo tabInfo)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@tabid", (DbType)SqlDbType.Int, 4, tabInfo.TabID),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, tabInfo.UserID),
				DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, tabInfo.DisplayOrder),
				DbHelper.MakeInParam("@tabname", (DbType)SqlDbType.NVarChar, 50, tabInfo.TabName),
				DbHelper.MakeInParam("@iconfile", (DbType)SqlDbType.VarChar, 50, tabInfo.IconFile),
				DbHelper.MakeInParam("@template", (DbType)SqlDbType.VarChar, 50, tabInfo.Template)
			};
            string commandText = string.Format(@"INSERT INTO [{0}spacetabs]([tabid], [uid], [displayorder], [tabname], [iconfile], [template]) VALUES(@tabid, @uid, @displayorder, @tabname, @iconfile, @template)", BaseConfigs.GetTablePrefix);

            return RunExecuteSql(commandText, parms);
        }

        /// <summary>
        /// 更新指定Tab信息
        /// </summary>
        /// <param name="tabInfo"></param>
        /// <returns></returns>
        public bool UpdateTab(TabInfo tabInfo)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@tabid", (DbType)SqlDbType.Int, 4, tabInfo.TabID),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, tabInfo.UserID),
				DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, tabInfo.DisplayOrder),
				DbHelper.MakeInParam("@tabname", (DbType)SqlDbType.NVarChar, 50, tabInfo.TabName),
				DbHelper.MakeInParam("@iconfile", (DbType)SqlDbType.VarChar, 50, tabInfo.IconFile),
				DbHelper.MakeInParam("@template", (DbType)SqlDbType.VarChar, 50, tabInfo.Template)
			};
            string commandText = string.Format(@"UPDATE [{0}spacetabs] SET [displayorder]=@displayorder, [tabname]=@tabname, [iconfile]=@iconfile, [template] = @template WHERE [tabid]=@tabid AND [uid]=@uid", BaseConfigs.GetTablePrefix);

            return RunExecuteSql(commandText, parms);
        }

        /// <summary>
        /// 删除Tab信息
        /// </summary>
        /// <param name="tabId"></param>
        /// <returns></returns>
        public bool DeleteTab(int tabId, int uid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@tabid", (DbType)SqlDbType.Int, 4, tabId),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
			};
            string commandText = string.Format(@"DELETE FROM [{0}spacetabs] WHERE [tabid]=@tabid AND [uid]=@uid", BaseConfigs.GetTablePrefix);

            return RunExecuteSql(commandText, parms);
        }

        public bool DeleteTab(int uid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
			};
            string commandText = string.Format(@"DELETE FROM [{0}spacetabs] WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);

            return RunExecuteSql(commandText, parms);
        }

        public int GetTabInfoCountByUserId(int userid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userid),
			};
            string commandText = string.Format(@"SELECT COUNT(1) FROM [{0}spacetabs] WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);

            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms), 0);
        }

        public int GetMaxTabIdByUid(int userid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userid)
			};
            string commandText = string.Format(@"SELECT TOP 1 [tabid] FROM [{0}spacetabs] WHERE [uid]=@uid ORDER BY [tabid] DESC", BaseConfigs.GetTablePrefix);
            
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms));
        }
    
        public void ClearDefaultTab(int userid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid)
			};
            string commandText = string.Format("UPDATE [{0}spaceconfigs] SET [defaulttab]=0 WHERE [userid]=@userid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void SetDefaultTab(int userid, int tabid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
				DbHelper.MakeInParam("@tabid", (DbType)SqlDbType.Int, 4, tabid)
			};
            string commandText = string.Format("UPDATE [{0}spaceconfigs] SET [defaulttab]=@tabid WHERE [userid]=@userid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void SetSpaceTheme(int userid, int themeid, string themepath)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
				DbHelper.MakeInParam("@themeid", (DbType)SqlDbType.Int, 4, themeid),
				DbHelper.MakeInParam("@themepath", (DbType)SqlDbType.VarChar, 50, themepath)
			};
            string commandText = string.Format("UPDATE [{0}spaceconfigs] SET [themeid]=@themeid, [themepath]=@themepath WHERE [userid]=@userid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }


        /// <summary>
        /// 运行非Select语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        private bool RunExecuteSql(string sql, DbParameter[] parms)
        {
            try
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 运行Select语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        private IDataReader RunSelectSql(string sql, DbParameter[] parms)
        {
            return DbHelper.ExecuteReader(CommandType.Text, sql, parms);
        }

        public DataRow GetThemes()
        {
            string commandText = string.Format("SELECT TOP 1 newid() AS row,[themeid],[directory] FROM [{0}spacethemes] WHERE type<>0 ORDER BY row", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0].Rows[0];
        }

        public DataTable GetUnActiveSpaceList()
        {
            string commandText = string.Format("SELECT [uid],s.[spaceid],[spacetitle],[username],[createdatetime] FROM [{0}spaceconfigs] s "
                                                + "LEFT JOIN [{0}users] u ON s.[userid]=u.[uid] WHERE s.[spaceid] IN (SELECT ABS([spaceid]) spaceid  FROM [{0}users] WHERE [spaceid] < 0) ORDER BY s.[spaceid] DESC",
                                                BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(commandText).Tables[0];
        }

        public void DeleteSpaces(string uidlist)
        {
            if (!Utils.IsNumericList(uidlist))
                return;
            DbHelper.ExecuteNonQuery(string.Format("DELETE FROM [{0}spaceconfigs] WHERE [userid] IN ({1})", BaseConfigs.GetTablePrefix, uidlist));
        }

        public void DeleteSpaceThemes(string themeidlist)
        {
            if (!Utils.IsNumericList(themeidlist))
                return;

            DbHelper.ExecuteNonQuery(string.Format("DELETE FROM [{0}spacethemes]  WHERE [themeid] IN({1})", BaseConfigs.GetTablePrefix, themeidlist));
        }

        public void UpdateSpaceThemeInfo(int themeid, string name, string author, string copyright)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar, 50, name),
                                        DbHelper.MakeInParam("@author", (DbType)SqlDbType.NVarChar, 100, author),
                                        DbHelper.MakeInParam("@copyright", (DbType)SqlDbType.NVarChar, 100, copyright),
                                        DbHelper.MakeInParam("@themeid", (DbType)SqlDbType.Int, 4, themeid)
                                    };
            string commandText = string.Format("UPDATE [{0}spacethemes] SET [name]=@name, [author]=@author, [copyright]=@copyright WHERE themeid=@themeid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public DataTable GetSpaceThemeDirectory()
        {
            return DbHelper.ExecuteDataset(string.Format("SELECT [directory] FROM [{0}spacethemes] WHERE [type]<>0", BaseConfigs.GetTablePrefix)).Tables[0];
        }

        public bool IsThemeExist(string name)
        {
            return TypeConverter.ObjectToInt(
                                 DbHelper.ExecuteScalar(CommandType.Text, 
                                                        string.Format("SELECT COUNT(*) FROM [{0}spacethemes] WHERE name=@name", BaseConfigs.GetTablePrefix),
                                                        DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar, 50, name))) > 0;
        }

        public void AddSpaceTheme(string directory, string name, int type, string author, string createdate, string copyright)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@directory", (DbType)SqlDbType.VarChar, 100, directory),
                                        DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar, 50, name),
                                        DbHelper.MakeInParam("@type", (DbType)SqlDbType.Int, 50, type),
                                        DbHelper.MakeInParam("@author", (DbType)SqlDbType.NVarChar, 100, author),
                                        DbHelper.MakeInParam("@createdate", (DbType)SqlDbType.NVarChar, 50, createdate),
                                        DbHelper.MakeInParam("@copyright", (DbType)SqlDbType.NVarChar, 100, copyright)
                                    };
            string commandText = string.Format("INSERT INTO [{0}spacethemes]([directory], [name], [type], [author], [createdate], [copyright]) VALUES(@directory,@name,@type,@author,@createdate,@copyright)", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void UpdateThemeName(int themeid, string name)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@themeid", (DbType)SqlDbType.Int, 4, themeid),
                                        DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar, 50, name)
                                    };
            DbHelper.ExecuteNonQuery(CommandType.Text, 
                                     string.Format("UPDATE [{0}spacethemes] SET name=@name WHERE themeid=@themeid", BaseConfigs.GetTablePrefix),
                                     parms);
        }

        public void DeleteTheme(int themeid)
        {
            DbParameter parm = DbHelper.MakeInParam("@themeid", (DbType)SqlDbType.Int, 4, themeid);
            string commandText = string.Format("DELETE FROM [{0}spacethemes] WHERE [themeid]=@themeid OR [type]=@themeid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parm);
        }


        public DataTable GetSpaceList(int pagesize, int currentpage, string username, string dateStart, string dateEnd)
        {
            int pagetop = (currentpage - 1) * pagesize;
            DbParameter[] parms = {
				DbHelper.MakeInParam("@dateStart", (DbType)SqlDbType.DateTime, 8, dateStart),
				DbHelper.MakeInParam("@dateEnd", (DbType)SqlDbType.DateTime, 8, dateEnd)
                                  };
            string condition = GetSpaceListCondition(username, dateStart, dateEnd);
            string commandText;
            if (currentpage == 1)
            {
                commandText = string.Format("SELECT TOP {0} s.[spaceid],[userid],[spacetitle],[username],[grouptitle],[postcount],[commentcount],[createdatetime],[status] "
                                          + "FROM [{1}spaceconfigs] s LEFT JOIN [{1}users] u ON s.userid=u.uid  LEFT JOIN [{1}usergroups] g ON u.[groupid]=g.[groupid] ", 
                                          pagesize, BaseConfigs.GetTablePrefix);
                if (condition != "")
                    commandText += "WHERE " + condition + " ";

                commandText += "ORDER BY s.spaceid DESC";
            }
            else
            {
                commandText = string.Format("SELECT TOP {0} s.[spaceid],[userid],[spacetitle],[username],[grouptitle],[postcount],[commentcount],[createdatetime],[status] "
                                            + "FROM [{1}spaceconfigs] s LEFT JOIN [{1}users] u ON s.[userid]=u.[uid] LEFT JOIN [{1}usergroups] g ON u.[groupid]=g.[groupid] "
                                            + "WHERE s.[spaceid]<(SELECT MIN([spaceid]) FROM (SELECT TOP {2} [spaceid] FROM [{1}spaceconfigs] ORDER BY [spaceid] DESC) AS tblTmp) ", 
                                            pagesize, BaseConfigs.GetTablePrefix, pagetop);
                if (condition != "")
                    commandText += "AND " + condition + " ";

                commandText += "ORDER BY s.[spaceid] DESC";
            }
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        private string GetSpaceListCondition(string username, string dateStart, string dateEnd)
        {
            string condition = " 1=1 ";
            if (username != "")
                condition += " AND u.[username] LIKE'%" + RegEsc(username) + "%'";
            if (dateStart != "")
                condition += " AND s.[createdatetime] >=@dateStart";
            if (dateEnd != "")
                condition += " AND s.[createdatetime] <=@dateEnd";
            return condition;
        }

        public void AdminOpenSpaceStatusBySpaceidlist(string spaceidlist)
        {
            if (!Utils.IsNumericList(spaceidlist))
                return;

            DbHelper.ExecuteNonQuery(string.Format("UPDATE [{0}spaceconfigs] SET [status]=[status]&~" + (int)SpaceStatusType.AdminClose + "  WHERE [spaceid] IN ({1})", BaseConfigs.GetTablePrefix, spaceidlist));
        }

        public void AdminCloseSpaceStatusBySpaceidlist(string spaceidlist)
        {
            if (!Utils.IsNumericList(spaceidlist))
                return;

            DbHelper.ExecuteNonQuery(string.Format("UPDATE [{0}spaceconfigs] SET [status]=[status]|" + (int)SpaceStatusType.AdminClose + "  WHERE [spaceid] IN ({1})", BaseConfigs.GetTablePrefix, spaceidlist));
        }

        public int GetSpaceRecordCount(string username, string dateStart, string dateEnd)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@dateStart", (DbType)SqlDbType.DateTime, 8, dateStart),
				DbHelper.MakeInParam("@dateEnd", (DbType)SqlDbType.DateTime, 8, dateEnd)
                                  };

            string condition = GetSpaceListCondition(username, dateStart, dateEnd);
            string commandText = string.Format("SELECT COUNT(s.[spaceid]) FROM [{0}spaceconfigs] s LEFT JOIN [{0}users] u ON s.[userid]=u.[uid] ", BaseConfigs.GetTablePrefix);
            if (condition != "")
                commandText += " WHERE " + condition;

            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0].Rows[0][0].ToString());
        }

        public bool IsRewritenameExist(string rewriteName)
        {
            DbParameter parm = DbHelper.MakeInParam("@rewritename", (DbType)SqlDbType.Char, 100, rewriteName);
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}spaceconfigs] WHERE [rewritename]=@rewritename", BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parm), 0) > 0;
        }

        public void UpdateUserSpaceRewriteName(int userid, string rewritename)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
                DbHelper.MakeInParam("@rewritename", (DbType)SqlDbType.Char, 100, rewritename)
            };
            string commandText = string.Format("UPDATE [{0}spaceconfigs] SET [rewritename]=@rewritename WHERE [userid]=@userid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public string GetUidBySpaceid(string spaceid)
        {
            DbParameter parm = DbHelper.MakeInParam("@spaceid", (DbType)SqlDbType.Int, 4, spaceid);
            string commandText = string.Format("SELECT [userid] FROM [{0}spaceconfigs] WHERE [spaceid]=@spaceid", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteScalar(CommandType.Text, commandText, parm).ToString();
        }

        public string GetSpaceattachmentsAidListByUid(int uid)
        {
            string aidlist = "";
            DbParameter parm = DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid);
            string commandText = string.Format("SELECT [aid] FROM [{0}spaceattachments] WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);
            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, commandText, parm).Tables[0];
            if (dt.Rows.Count == 0)
                return "";
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    aidlist += dr["aid"].ToString() + ",";
                }
                if (aidlist != "")
                    aidlist = aidlist.Substring(0, aidlist.Length - 1);
            }
            return aidlist;
        }

        public void DeleteSpaceByUid(int uid)
        {
            DbParameter parm = DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid);
            string commandText = string.Format("DELETE FROM [{0}spaceconfigs] WHERE [userid]=@uid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parm);
        }

        public void UpdateCustomizePanelContent(int moduleid, int userid, string modulecontent)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@moduleid", (DbType)SqlDbType.Int, 4, moduleid),                
                DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
                DbHelper.MakeInParam("@modulecontent", (DbType)SqlDbType.NText, 0, modulecontent)
            };
            string commandText = string.Format("UPDATE [{0}spacecustomizepanels] SET [panelcontent]=@modulecontent WHERE [moduleid]=@moduleid AND [userid]=@userid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public bool ExistCustomizePanelContent(int moduleid, int userid)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@moduleid", (DbType)SqlDbType.Int, 4, moduleid),
                DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid)
            };
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}spacecustomizepanels] WHERE [moduleid]=@moduleid AND [userid]=@userid", BaseConfigs.GetTablePrefix);

            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms), 0) > 0;
        }

        public void AddCustomizePanelContent(int moduleid, int userid, string modulecontent)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@moduleid", (DbType)SqlDbType.Int, 4, moduleid),
                DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
                DbHelper.MakeInParam("@modulecontent", (DbType)SqlDbType.NText, 0, modulecontent)
            };
            string commandText = string.Format("INSERT INTO [{0}spacecustomizepanels]([moduleid], [userid], [panelcontent]) VALUES(@moduleid, @userid, @modulecontent)", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public object GetCustomizePanelContent(int moduleid, int userid)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@moduleid", (DbType)SqlDbType.Int, 4, moduleid),
                DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid)
            };
            string commandText = string.Format("SELECT [panelcontent] FROM [{0}spacecustomizepanels] WHERE [moduleid]=@moduleid AND [userid]=@userid", BaseConfigs.GetTablePrefix);

            return DbHelper.ExecuteScalar(CommandType.Text, commandText, parms);
        }

        public void DeleteCustomizePanelContent(int moduleid, int userid)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@moduleid", (DbType)SqlDbType.Int, 4, moduleid),
                DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid)
            };
            string commandText = string.Format("DELETE FROM [{0}spacecustomizepanels] WHERE [moduleid]=@moduleid AND [userid]=@userid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public IDataReader GetModulesByUserId(int uid)
        {
            DbParameter parm = DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid);

            string commandText = string.Format("SELECT * FROM [{0}spacemodules] WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);

            return DbHelper.ExecuteReader(CommandType.Text, commandText, parm);
        }


        public string GetSapceThemeList(int themeid)
        {
            return string.Format("SELECT * FROM [{0}spacethemes] WHERE [type]={1}", BaseConfigs.GetTablePrefix, themeid);
        }

        public string DeleteSpaceThemeByThemeid(int themeid)
        {
            return string.Format("DELETE FROM [{0}spacethemes] WHERE [themeid]={1}", BaseConfigs.GetTablePrefix, themeid);
        }

        public IDataReader GetModuleDefList()
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}spacemoduledefs]", BaseConfigs.GetTablePrefix));
        }

        public void UpdateModuleDefInfo(string configfile, string controller)
        {
            configfile = RegEsc(configfile);
            controller = RegEsc(controller);
            string commandText = string.Format("UPDATE [{0}spacemoduledefs] SET [controller]='{1}' WHERE [configfile]='{2}'", BaseConfigs.GetTablePrefix, controller, configfile);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }


        public void AddModuleDefInfo(ModuleDefInfo mdi)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@modulename", (DbType)SqlDbType.NVarChar, 20, mdi.ModuleName),
                                        DbHelper.MakeInParam("@cachetime", (DbType)SqlDbType.Int, 4, mdi.CacheTime),
                                        DbHelper.MakeInParam("@configfile", (DbType)SqlDbType.VarChar, 100, mdi.ConfigFile),
                                        DbHelper.MakeInParam("@controller", (DbType)SqlDbType.VarChar, 255, mdi.BussinessController)
            };
            string commandText = string.Format("INSERT INTO [{0}spacemoduledefs]([modulename], [cachetime], [configfile], [controller]) VALUES(@modulename, @cachetime, @configfile, @controller)", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }


        public void DeleteModuleDefByUrl(string url)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@configfile", (DbType)SqlDbType.VarChar, 100, url)
            };

            string commandText = string.Format("DELETE FROM [{0}spacemoduledefs] WHERE [configfile] = @configfile", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public DataTable GetSearchSpacePostsList(int pagesize, string postids)
        {
            if (!Utils.IsNumericList(postids))
                return new DataTable();

            string commandText = string.Format("SELECT TOP {1} [{0}spaceposts].[postid], [{0}spaceposts].[title], [{0}spaceposts].[author], [{0}spaceposts].[uid], [{0}spaceposts].[postdatetime], [{0}spaceposts].[commentcount], [{0}spaceposts].[views] FROM [{0}spaceposts] WHERE [{0}spaceposts].[postid] IN({2}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[{0}spaceposts].[postid]),'{2}')", BaseConfigs.GetTablePrefix, pagesize, postids);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public int GetSpaceAttachmentSizeByUserid(int userid)
        {
            string sql = string.Format("SELECT ISNULL(SUM(filesize), 0) AS [filesize] FROM [{0}spaceattachments] WHERE uid={1}", BaseConfigs.GetTablePrefix, userid);
            return (int)DbHelper.ExecuteScalar(CommandType.Text, sql);
        }

        public string GetSpaceThemes()
        {
            return string.Format("SELECT * FROM [{0}spacethemes] WHERE [type]=0", BaseConfigs.GetTablePrefix);
        }

        public void CreateSpacePostTags(string tags, int postid, int userid, string postdatetime)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@tags", (DbType)SqlDbType.NVarChar, 55, tags),
                DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4, postid),
                DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
                DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, postdatetime)                
            };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}createspaceposttags", BaseConfigs.GetTablePrefix), parms);
        }

        public IDataReader GetTagsListBySpacePost(int postid)
        {
            DbParameter parm = DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4, postid);

            string commandText = string.Format("SELECT [{0}tags].* FROM [{0}tags], [{0}spaceposttags] WHERE [{0}spaceposttags].[tagid] = [{0}tags].[tagid] AND [{0}spaceposttags].[spacepostid] = @postid ORDER BY [orderid]", BaseConfigs.GetTablePrefix);

            return DbHelper.ExecuteReader(CommandType.Text, commandText, parm);
        }

        public int GetSpacePostCountWithSameTag(int tagid)
        {
            DbParameter parm = DbHelper.MakeInParam("@tagid", (DbType)SqlDbType.Int, 4, tagid);

            string commandText = string.Format("SELECT COUNT(1) FROM [{0}spaceposttags] AS [spt],[{0}spaceposts] AS [sp] WHERE [spt].[spacepostid] = [sp].[postid] AND [sp].[poststatus] = 1 AND [tagid] = @tagid", BaseConfigs.GetTablePrefix);

            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parm), 0);
        }

        public IDataReader GetSpacePostsWithSameTag(int tagid, int pageindex, int pagesize)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@tagid", (DbType)SqlDbType.Int, 4, tagid),
                DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int, 4, pageindex),
                DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int, 4, pagesize)
            };
            string commandText = string.Format("{0}getspacepostlistbytag", BaseConfigs.GetTablePrefix);

            return DbHelper.ExecuteReader(CommandType.StoredProcedure, commandText, parms);
        }

        public IDataReader GetHotTagsListForSpace(int count)
        {
            string commandText = string.Format("SELECT TOP {0} * FROM [{1}tags] WHERE [scount] > 0 ORDER BY [scount] DESC,[orderid]", count, BaseConfigs.GetTablePrefix);

            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public void DeleteSpacePostTags(int spacepostid)
        {
            DbParameter parm = DbHelper.MakeInParam("@spacepostid", (DbType)SqlDbType.Int, 4, spacepostid);

            string commandText = string.Format("{0}deletespaceposttags", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, commandText, parm);
        }

        public IDataReader GetSpacePostCategorys(string spacepostids)
        {
            if (!Utils.IsNumericList(spacepostids))
                return null;

            string commandText = string.Format("SELECT [a].[categoryid],[a].[title],[postid] FROM [{0}spacecategories] AS [a],[{0}spacepostcategories] AS [b] WHERE [a].[categoryid]=[b].[categoryid] AND [postid] IN ({1})", BaseConfigs.GetTablePrefix, spacepostids);

            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }        

        private string GetAlbumListCondition(string usernamelist, string titlelist, string descriptionlist, string startdate, string enddate)
        {
            string condition = "";
            if (usernamelist != "")
            {
                string[] username = usernamelist.Split(',');
                condition += " AND [username] in (";
                string tempusernamelist = "";
                foreach (string p in username)
                {
                    tempusernamelist += "'" + p + "',";
                }
                if (tempusernamelist != "")
                    tempusernamelist = tempusernamelist.Substring(0, tempusernamelist.Length - 1);
                condition += tempusernamelist + ")";
            }
            if (titlelist != "")
            {
                string[] title = titlelist.Split(',');
                condition += " AND [title] in (";
                string temptitlelist = "";
                foreach (string p in title)
                {
                    temptitlelist += "'" + p + "',";
                }
                if (temptitlelist != "")
                    temptitlelist = temptitlelist.Substring(0, temptitlelist.Length - 1);
                condition += temptitlelist + ")";
            }
            if (descriptionlist != "")
            {
                string tempdescriptionlist = "";
                foreach (string description in descriptionlist.Split(','))
                {
                    tempdescriptionlist += " [description] LIKE '%" + RegEsc(description) + "%' OR";
                }
                tempdescriptionlist = tempdescriptionlist.Substring(0, tempdescriptionlist.Length - 2);
                condition += " AND (" + tempdescriptionlist + ")";
            }
            if (startdate != "")
                condition += " AND [createdatetime]>=@startdate";
            if (enddate != "")
                condition += " AND [createdatetime]<=@enddate";

            return RegEsc(condition);
        }


        public int GetSpaceCountByCondition(string posterlist, string keylist, string startdate, string enddate)
        {
            if (!Utils.IsNumericList(posterlist))
                return 0;

            string commandText = string.Format("SELECT COUNT(1) FROM (SELECT s.*,u.[username] FROM [{0}spaceconfigs] s LEFT JOIN [{0}users] u ON s.[userid]=u.[uid]) AS tblTmp WHERE [status]=0", BaseConfigs.GetTablePrefix);
            string condition = GetSpaceCondition(posterlist, keylist, startdate, enddate);
            DbParameter[] parms = GetDateSpanParms(startdate, enddate);
            if (condition != "")
                commandText += condition;
            return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString());
        }

        public DataTable GetSpaceByCondition(string posterlist, string keylist, string startdate, string enddate, int pageSize, int currentPage)
        {
            string commandText = "";
            string condition = GetSpaceCondition(posterlist, keylist, startdate, enddate);
            DbParameter[] parms = GetDateSpanParms(startdate, enddate);
            int pageTop = (currentPage - 1) * pageSize;
            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} s.*,u.[username],f.[avatar],(SELECT COUNT(1) FROM [{1}albums] WHERE [userid]=s.[userid]) albumcount "
                                    + "FROM [{1}spaceconfigs] s LEFT JOIN [{1}userfields] f ON [userid]=f.[uid] LEFT JOIN [{1}users] u ON u.[uid]=[userid] WHERE [status]=0 {2} ORDER BY s.[spaceid] DESC",
                                    pageSize, BaseConfigs.GetTablePrefix, condition);
            else
                commandText = string.Format("SELECT TOP {0} s.*,u.[username],f.[avatar],(SELECT COUNT(1) FROM [{1}albums] WHERE [userid]=s.[userid]) albumcount "
                                    + "FROM [{1}spaceconfigs] s LEFT JOIN [{1}userfields] f ON [userid]=f.[uid] LEFT JOIN [{1}users] u ON u.[uid]=[userid] WHERE [status]=0 AND s.[spaceid]<(SELECT MIN([spaceid]) FROM (SELECT TOP {2}"
                                    + " [spaceid] FROM [{1}spaceconfigs] WHERE [status]=0 {3} ORDER BY [spaceid] DESC) AS tblTmp) {3} ORDER BY s.[spaceid] DESC",
                                    pageSize, BaseConfigs.GetTablePrefix, pageTop, condition);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        private string GetSpaceCondition(string posterlist, string keylist, string startdate, string enddate)
        {
            string condition = "";
            if (posterlist != "")
            {
                string[] poster = posterlist.Split(',');
                condition += " AND [username] in (";
                string tempposerlist = "";
                foreach (string p in poster)
                {
                    tempposerlist += "'" + p + "',";
                }
                if (tempposerlist != "")
                    tempposerlist = tempposerlist.Substring(0, tempposerlist.Length - 1);

                condition += tempposerlist + ")";
            }
            if (keylist != "")
            {
                string tempkeylist = "";
                foreach (string key in keylist.Split(','))
                {
                    tempkeylist += " [spacetitle] LIKE '%" + RegEsc(key) + "%' OR";
                }
                tempkeylist = tempkeylist.Substring(0, tempkeylist.Length - 2);
                condition += " AND (" + tempkeylist + ")";
            }
            if (startdate != "")
                condition += " AND [createdatetime]>=@startdate";
            if (enddate != "")
                condition += " AND [createdatetime]<=@enddate";

            return RegEsc(condition);
        }

        public DataTable GetSpaceLitByTidlist(string spaceidlist)
        {
            if (!Utils.IsNumericList(spaceidlist))
                return new DataTable();

            string commandText = string.Format("SELECT s.*,f.[avatar],(SELECT COUNT(1) FROM [{0}albums] WHERE userid=s.userid) albumcount "
                                        + "FROM [{0}spaceconfigs] s LEFT JOIN [{0}userfields] f ON [userid]=f.[uid] WHERE ([spaceid] IN ({1})) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[spaceid]),'{1}')",
                                        BaseConfigs.GetTablePrefix, spaceidlist);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }


        public DataTable GetWebSiteAggSpaceTopComments(int topnumber)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP {0} [postid],[content],[posttitle],[author],[uid] FROM [{1}spacecomments] ORDER BY [commentid] DESC", topnumber, BaseConfigs.GetTablePrefix)).Tables[0];
        }

        public string[] GetSpaceLastPostInfo(int userid)
        {
            DbParameter pram = DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userid);
            string commandText = string.Format("SELECT TOP 1 [postid],[title] FROM [{0}spaceposts] WHERE [uid]=@uid ORDER BY [postdatetime] DESC",
                                        BaseConfigs.GetTablePrefix);
            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, commandText, pram).Tables[0];
            string[] result = new string[2];
            if (dt != null && dt.Rows.Count != 0)
            {
                result[0] = dt.Rows[0]["postid"].ToString();
                result[1] = dt.Rows[0]["title"].ToString().Trim();
            }
            else
            {
                result[0] = "0";
                result[1] = "";
            }
            return result;
        }

        public int GetSpacePostCountByCondition(string posterlist, string keylist, string startdate, string enddate)
        {
            if (!Utils.IsNumericList(posterlist))
                return 0;

            string commandText = string.Format("SELECT COUNT(postid) FROM [{0}spaceposts]",  BaseConfigs.GetTablePrefix);
            string condition = GetSpacePostCondition(posterlist, keylist, startdate, enddate);

            DbParameter[] parms = GetDateSpanParms(startdate, enddate);

            if (condition != "")
                commandText += condition;
            return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString());
        }

        private string GetSpacePostCondition(string posterlist, string keylist, string startdate, string enddate)
        {
            string condition = "";
            if (posterlist != "")
            {
                string[] poster = posterlist.Split(',');
                condition += " AND [author] in (";
                string tempposerlist = "";
                foreach (string p in poster)
                {
                    tempposerlist += "'" + p + "',";
                }
                if (tempposerlist != "")
                    tempposerlist = tempposerlist.Substring(0, tempposerlist.Length - 1);

                condition += tempposerlist + ")";
            }
            if (keylist != "")
            {
                string tempkeylist = "";
                foreach (string key in keylist.Split(','))
                {
                    tempkeylist += " [title] LIKE '%" + RegEsc(key) + "%' OR";
                }
                tempkeylist = tempkeylist.Substring(0, tempkeylist.Length - 2);
                condition += " AND (" + tempkeylist + ")";
            }
            if (startdate != "")
                condition += " AND [postdatetime]>=@startdate";
            if (enddate != "")
                condition += " AND [postdatetime]<=@enddate";

            return RegEsc(condition);
        }

        public DataTable GetSpacePostByCondition(string posterlist, string keylist, string startdate, string enddate, int pageSize, int currentPage)
        {
            string commandText = "";
            DbParameter[] parms = GetDateSpanParms(startdate, enddate);

            string condition = GetSpacePostCondition(posterlist, keylist, startdate, enddate);
            int pageTop = (currentPage - 1) * pageSize;
            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} * FROM [{1}spaceposts] WHERE 1=1 {2} ORDER BY [postid] DESC", pageSize, BaseConfigs.GetTablePrefix, condition);
            else
                commandText = string.Format("SELECT TOP {0} * FROM [{1}spaceposts] WHERE [postid]<(SELECT MIN([postid]) FROM (SELECT TOP {2} [postid] FROM [{1}spaceposts] WHERE 1=1 {3} ORDER BY [postid] DESC) AS tblTmp) {3} ORDER BY [postid] DESC",
                                            pageSize, BaseConfigs.GetTablePrefix, pageTop, condition);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        public DataTable GetSpacepostLitByTidlist(string postidlist)
        {
            if (!Utils.IsNumericList(postidlist))
                return new DataTable();

            string commandText = string.Format("SELECT * FROM [{0}spaceposts] WHERE [postid] IN ({1}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[postid]),'{1}')", BaseConfigs.GetTablePrefix, postidlist);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

     
        public DataTable GetWebSiteAggSpacePostList(int topnumber)
        {
            return DbHelper.ExecuteDataset(string.Format("SELECT TOP {0} [postid], [author], [uid], [postdatetime], [title], [commentcount], [views] FROM [{1}spaceposts] WHERE [poststatus] = 1 ORDER BY [postdatetime] DESC", topnumber, BaseConfigs.GetTablePrefix)).Tables[0];
        }

        public DataTable GetWebSiteAggRecentUpdateSpaceList(int topnumber)
        {
            return DbHelper.ExecuteDataset(string.Format("SELECT TOP {0} [spaceid], [userid], [spacetitle], [postcount], [commentcount], [visitedtimes] FROM [{1}spaceconfigs] WHERE [status] = 0 AND [postcount]>0 ORDER BY [updatedatetime] DESC", topnumber, BaseConfigs.GetTablePrefix)).Tables[0];
        }

        public DataTable GetWebSiteAggTopSpaceList(string orderby,int topnumber)
        {
            orderby = RegEsc(orderby);
            return DbHelper.ExecuteDataset(string.Format("SELECT TOP {0} s.*,u.[avatar] FROM [{1}spaceconfigs] s LEFT JOIN [{1}userfields] u ON s.[userid] = u.[uid]  WHERE s.[status] = 0 ORDER BY s.[{2}] DESC", topnumber, BaseConfigs.GetTablePrefix, orderby)).Tables[0];
        }

        public DataTable GetWebSiteAggTopSpacePostList(string orderby, int topnumber)
        {
            orderby = RegEsc(orderby);
            return DbHelper.ExecuteDataset(string.Format("SELECT TOP {0} [postid],[title],[author],[uid],[postdatetime],[commentcount],[views] FROM [{1}spaceposts] WHERE [poststatus] = 1 ORDER BY [{2}] DESC", topnumber, BaseConfigs.GetTablePrefix, orderby)).Tables[0];
        }


        public DataTable GetWebSiteAggSpacePostsList(int pageSize, int currentPage)
        {
            DataTable dt = new DataTable();

            int pageTop = (currentPage - 1) * pageSize;
            string commandText = "";
            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} * FROM [{1}spaceposts] ORDER BY [postid] DESC", pageSize, BaseConfigs.GetTablePrefix);
            else
                commandText = string.Format("SELECT TOP {0} * FROM [{1}spaceposts] WHERE [postid] < (SELECT min([postid])  FROM "
                                            + "(SELECT TOP {2} [postid] FROM [{1}spaceposts] ORDER BY [postid] DESC) AS tblTmp ) ORDER BY [postid] DESC", 
                                            pageSize, BaseConfigs.GetTablePrefix, pageTop);
            return dt = DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0]; ;
        }

        public int GetWebSiteAggSpacePostsCount()
        {
            try
            {
                return (int)DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT([postid]) FROM [{0}spaceposts]", BaseConfigs.GetTablePrefix));
            }
            catch
            {
                return 0;
            }
        }
      
        private string GetPhotoCondition(string photousernamelist, string keylist, string startdate, string enddate)
        {
            string condition = "";
            if (photousernamelist != "")
            {
                string[] poster = photousernamelist.Split(',');
                condition += " AND p.[username] in (";
                string tempposerlist = "";
                foreach (string p in poster)
                {
                    tempposerlist += "'" + p + "',";
                }
                if (tempposerlist != "")
                    tempposerlist = tempposerlist.Substring(0, tempposerlist.Length - 1);

                condition += tempposerlist + ")";
            }
            if (keylist != "")
            {
                string tempkeylist = "";
                foreach (string key in keylist.Split(','))
                {
                    tempkeylist += " p.[title] LIKE '%" + RegEsc(key) + "%' OR";
                }
                tempkeylist = tempkeylist.Substring(0, tempkeylist.Length - 2);
                condition += " AND (" + tempkeylist + ")";
            }
            if (startdate != "")
                condition += " AND p.[postdate]>=@startdate";
            if (enddate != "")
                condition += " AND p.[postdate]<=@enddate";

            return RegEsc(condition);
        }

    }
}

