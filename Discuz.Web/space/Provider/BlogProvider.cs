using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Space.Entities;
using Discuz.Entity;
using Discuz.Data;
using Discuz.Config;
using Discuz.Cache;
using System.Collections;
using Discuz.Space.Utilities;
using Discuz.Common.Generic;
using Discuz.Space.Data;

namespace Discuz.Space.Provider
{
	/// <summary>
	/// SpaceTemplateProvider 的摘要说明。
	/// </summary>
	public class BlogProvider
	{
		public BlogProvider()
		{}

        /// <summary>
        /// 通过spaceid得到UID
        /// </summary>
        /// <returns>用户ID</returns>
        public static int GetUidBySpaceid(string spaceid)
        {
            return Utils.StrToInt(DbProvider.GetInstance().GetUidBySpaceid(spaceid), -1);
        }


		#region Space 个人操作
		/// <summary>
		/// 通过数据读取获得当前用户的个性化设置
		/// </summary>
		/// <param name="datareader">数据读取</param>
		/// <returns></returns>
		public static SpaceConfigInfo GetSpaceConfigInfo(int userid)
		{
            IDataReader idatareader = DbProvider.GetInstance().GetSpaceConfigDataByUserID(userid);
		
			if(idatareader.Read())
			{
				SpaceConfigInfo spaceconfiginfo = new SpaceConfigInfo();
				spaceconfiginfo.SpaceID = TypeConverter.ObjectToInt(idatareader["spaceid"]);
				spaceconfiginfo.UserID = TypeConverter.ObjectToInt(idatareader["userid"]);
				spaceconfiginfo.Spacetitle = idatareader["spacetitle"].ToString().Trim();
				spaceconfiginfo.Description = idatareader["description"].ToString().Trim();
				spaceconfiginfo.BlogDispMode = TypeConverter.ObjectToInt(idatareader["blogdispmode"]);
				spaceconfiginfo.Bpp = TypeConverter.ObjectToInt(idatareader["bpp"]);
				spaceconfiginfo.Commentpref = TypeConverter.ObjectToInt(idatareader["commentpref"]);
				spaceconfiginfo.MessagePref = TypeConverter.ObjectToInt(idatareader["messagepref"]);
				spaceconfiginfo.Rewritename = idatareader["rewritename"].ToString();
				spaceconfiginfo.ThemeID = TypeConverter.ObjectToInt(idatareader["themeid"]);
				spaceconfiginfo.ThemePath = idatareader["themepath"].ToString().Trim();
				spaceconfiginfo.PostCount = TypeConverter.ObjectToInt(idatareader["postcount"]);
				spaceconfiginfo.CommentCount = TypeConverter.ObjectToInt(idatareader["commentcount"]);
				spaceconfiginfo.VisitedTimes = TypeConverter.ObjectToInt(idatareader["visitedtimes"]);
				spaceconfiginfo.CreateDateTime = Convert.ToDateTime(idatareader["createdatetime"]);
				spaceconfiginfo.UpdateDateTime = Convert.ToDateTime(idatareader["updatedatetime"]);
				spaceconfiginfo.DefaultTab = TypeConverter.ObjectToInt(idatareader["defaulttab"]);
				spaceconfiginfo.Status = (SpaceStatusType) TypeConverter.ObjectToInt(idatareader["status"]);
				idatareader.Close();
				return spaceconfiginfo;
			}
			else
			{
                idatareader.Close();
				return null;
			}
		}


		public static SpaceConfigInfo[] GetSpaceConfigInfoArray(DataTable dt)
		{
			if(dt == null || dt.Rows.Count == 0)
				return null;

			SpaceConfigInfo[] spaceconfiginfoarray = new SpaceConfigInfo[dt.Rows.Count];
			for(int i = 0 ; i < dt.Rows.Count ; i++)
			{
				spaceconfiginfoarray[i] = new SpaceConfigInfo();
				spaceconfiginfoarray[i].SpaceID = TypeConverter.ObjectToInt(dt.Rows[i]["spaceid"]);
				spaceconfiginfoarray[i].UserID = TypeConverter.ObjectToInt(dt.Rows[i]["userid"]);
				spaceconfiginfoarray[i].Spacetitle = dt.Rows[i]["spacetitle"].ToString();
				spaceconfiginfoarray[i].Description = dt.Rows[i]["description"].ToString();
				spaceconfiginfoarray[i].BlogDispMode = TypeConverter.ObjectToInt(dt.Rows[i]["blogdispmode"]);
				spaceconfiginfoarray[i].Bpp = TypeConverter.ObjectToInt(dt.Rows[i]["bpp"]);
				spaceconfiginfoarray[i].Commentpref = TypeConverter.ObjectToInt(dt.Rows[i]["commentpref"]);
				spaceconfiginfoarray[i].MessagePref = TypeConverter.ObjectToInt(dt.Rows[i]["messagepref"]);
				spaceconfiginfoarray[i].Rewritename = dt.Rows[i]["rewritename"].ToString();
				spaceconfiginfoarray[i].ThemeID = TypeConverter.ObjectToInt(dt.Rows[i]["themeid"]);
				spaceconfiginfoarray[i].ThemePath = dt.Rows[i]["themepath"].ToString();
				spaceconfiginfoarray[i].PostCount = TypeConverter.ObjectToInt(dt.Rows[i]["postcount"]);
				spaceconfiginfoarray[i].CommentCount = TypeConverter.ObjectToInt(dt.Rows[i]["commentcount"]);
				spaceconfiginfoarray[i].VisitedTimes = TypeConverter.ObjectToInt(dt.Rows[i]["visitedtimes"]);
				spaceconfiginfoarray[i].CreateDateTime = Convert.ToDateTime(dt.Rows[i]["createdatetime"]);
				spaceconfiginfoarray[i].UpdateDateTime = Convert.ToDateTime(dt.Rows[i]["updatedatetime"]);
				spaceconfiginfoarray[i].Status = (SpaceStatusType) TypeConverter.ObjectToInt(dt.Rows[i]["status"]);
			}

			dt.Dispose();
			return spaceconfiginfoarray;
		}
		#endregion

       
		#region Space 主题操作
		/// <summary>
		/// 通过数据读取获得主题内容
		/// </summary>
		/// <param name="datareader">数据读取</param>
		/// <returns></returns>
		public static ThemeInfo GetSpaceThemeInfo(IDataReader idatareader)
		{
			if (idatareader == null)
				return null;

			if (idatareader.Read())
			{
				ThemeInfo spacethemeinfo = new ThemeInfo();
				spacethemeinfo.ThemeId = TypeConverter.ObjectToInt(idatareader["themeid"]);
				spacethemeinfo.Directory = idatareader["directory"].ToString();
				spacethemeinfo.Name = idatareader["name"].ToString();
				spacethemeinfo.Type = TypeConverter.ObjectToInt(idatareader["type"]);
				spacethemeinfo.Author = idatareader["author"].ToString();
				spacethemeinfo.CreateDate = idatareader["createdate"].ToString();
				spacethemeinfo.CopyRight = idatareader["copyright"].ToString();

				idatareader.Close();
				return spacethemeinfo;
			}
			else
			{
                idatareader.Close();
				return null;
			}
		}

        /// <summary>
        /// 获取空间主题信息
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
		public static ThemeInfo[] GetSpaceThemeInfoArray(DataTable dt)
		{
			if(dt == null || dt.Rows.Count == 0)
				return null;
			ThemeInfo[] spacethemeinfoarray = new ThemeInfo[dt.Rows.Count];
			for(int i = 0 ; i < dt.Rows.Count ; i++)
			{
				spacethemeinfoarray[i] = new ThemeInfo();
				spacethemeinfoarray[i].ThemeId = TypeConverter.ObjectToInt(dt.Rows[i]["themeid"]);
				spacethemeinfoarray[i].Directory = dt.Rows[i]["datareader"].ToString();
				spacethemeinfoarray[i].Name = dt.Rows[i]["name"].ToString();
				spacethemeinfoarray[i].Type = TypeConverter.ObjectToInt(dt.Rows[i]["type"]);
				spacethemeinfoarray[i].Author = dt.Rows[i]["author"].ToString();
				spacethemeinfoarray[i].CreateDate = dt.Rows[i]["createdate"].ToString();
				spacethemeinfoarray[i].CopyRight = dt.Rows[i]["copyright"].ToString();
			}

			dt.Dispose();
			return spacethemeinfoarray;
		}
		#endregion

		#region Space 评论操作

        /// <summary>
        /// 获取单个评论对象
        /// </summary>
        /// <param name="__idatareader"></param>
        /// <returns></returns>
		public static SpaceCommentInfo GetSpaceCommentInfo (IDataReader idatareader)
		{
			if(idatareader == null)
				return null;

			if(idatareader.Read())
			{
				SpaceCommentInfo spacecommentsinfo = new SpaceCommentInfo();
				spacecommentsinfo.CommentID = TypeConverter.ObjectToInt(idatareader["commentid"]);
				spacecommentsinfo.PostID = TypeConverter.ObjectToInt(idatareader["postid"]);
				spacecommentsinfo.Author = idatareader["author"].ToString();
				spacecommentsinfo.Email = idatareader["email"].ToString();
				spacecommentsinfo.Url = idatareader["url"].ToString();
				spacecommentsinfo.Ip = idatareader["ip"].ToString();
				spacecommentsinfo.PostDateTime = Convert.ToDateTime(idatareader["postdatetime"]);
				spacecommentsinfo.Content = idatareader["content"].ToString();
				spacecommentsinfo.ParentID = TypeConverter.ObjectToInt(idatareader["parentid"]);
				spacecommentsinfo.Uid = TypeConverter.ObjectToInt(idatareader["uid"]);
				spacecommentsinfo.PostTitle = idatareader["posttitle"].ToString();

				idatareader.Close();
				return spacecommentsinfo;
			}
			else
			{
                idatareader.Close();
				return null;
			}
		}

        /// <summary>
        /// 获取评论对象数组
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
		public static SpaceCommentInfo[] GetSpaceCommentInfo (DataTable dt)
		{
			if(dt == null || dt.Rows.Count == 0)
				return null;

			SpaceCommentInfo[] spacecommentsinfoarray = new SpaceCommentInfo[dt.Rows.Count];
			for(int i = 0 ; i < dt.Rows.Count ; i++)
			{
				spacecommentsinfoarray[i] = new SpaceCommentInfo();
				spacecommentsinfoarray[i].CommentID = TypeConverter.ObjectToInt(dt.Rows[i]["commentid"]);
				spacecommentsinfoarray[i].PostID = TypeConverter.ObjectToInt(dt.Rows[i]["postid"]);
				spacecommentsinfoarray[i].Author = dt.Rows[i]["author"].ToString();
				spacecommentsinfoarray[i].Email = dt.Rows[i]["email"].ToString();
				spacecommentsinfoarray[i].Url = dt.Rows[i]["url"].ToString();
				spacecommentsinfoarray[i].Ip = dt.Rows[i]["ip"].ToString();
				spacecommentsinfoarray[i].PostDateTime = Convert.ToDateTime(dt.Rows[i]["postdatetime"]);
				spacecommentsinfoarray[i].Content = dt.Rows[i]["content"].ToString();
				spacecommentsinfoarray[i].ParentID = TypeConverter.ObjectToInt(dt.Rows[i]["parentid"]);
				spacecommentsinfoarray[i].Uid = TypeConverter.ObjectToInt(dt.Rows[i]["uid"]);
				spacecommentsinfoarray[i].PostTitle = dt.Rows[i]["posttitle"].ToString();
			}

			dt.Dispose();
			return spacecommentsinfoarray;
		}
		#endregion

		#region Space 日志操作

        /// <summary>
        /// 获取空间日志
        /// </summary>
        /// <param name="__idatareader"></param>
        /// <returns></returns>
		public static SpacePostInfo GetSpacepostsInfo(IDataReader idatareader)
		{
			if(idatareader == null)
				return null;

			if(idatareader.Read())
			{
				SpacePostInfo spacepostsinfo = new SpacePostInfo();
				spacepostsinfo.Postid = TypeConverter.ObjectToInt(idatareader["postid"]);
				spacepostsinfo.Author = idatareader["author"].ToString();
				spacepostsinfo.Uid = TypeConverter.ObjectToInt(idatareader["uid"]);
				spacepostsinfo.Postdatetime = Convert.ToDateTime(idatareader["postdatetime"]);
				spacepostsinfo.Content = idatareader["content"].ToString();
				spacepostsinfo.Title = idatareader["title"].ToString();
				spacepostsinfo.Category = idatareader["category"].ToString();
				spacepostsinfo.PostStatus = Convert.ToInt16(idatareader["poststatus"]);
				spacepostsinfo.CommentStatus = Convert.ToInt16(idatareader["commentstatus"]);
				spacepostsinfo.PostUpDateTime = Convert.ToDateTime(idatareader["postupdatetime"]);
				spacepostsinfo.Commentcount = TypeConverter.ObjectToInt(idatareader["commentcount"]);
				spacepostsinfo.Views = TypeConverter.ObjectToInt(idatareader["views"]);

				idatareader.Close();
				return spacepostsinfo;
			}
			else
			{
                idatareader.Close();
				return null;
			}
		}

        /// <summary>
        /// 获取空间日志
        /// </summary>
        /// <param name="spacepostid"></param>
        /// <returns></returns>
        public static SpacePostInfo GetSpacepostsInfo(int spacepostid)
        {
            return GetSpacepostsInfo(Space.Data.DbProvider.GetInstance().GetSpacePost(spacepostid));
        }

        /// <summary>
        /// 获取日志数组
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
		public static SpacePostInfo[] GetSpacepostsInfo (DataTable dt)
		{
			if(dt == null || dt.Rows.Count == 0)
				return null;

			SpacePostInfo[] spacepostsinfoarray = new SpacePostInfo[dt.Rows.Count];
			for(int i = 0 ; i < dt.Rows.Count ; i++)
			{
				spacepostsinfoarray[i] = new SpacePostInfo();
				spacepostsinfoarray[i].Postid = TypeConverter.ObjectToInt(dt.Rows[i]["postid"]);
				spacepostsinfoarray[i].Author = dt.Rows[i]["author"].ToString();
				spacepostsinfoarray[i].Uid = TypeConverter.ObjectToInt(dt.Rows[i]["uid"]);
				spacepostsinfoarray[i].Postdatetime = Convert.ToDateTime(dt.Rows[i]["postdatetime"]);
				spacepostsinfoarray[i].Content = dt.Rows[i]["content"].ToString();
				spacepostsinfoarray[i].Title = dt.Rows[i]["title"].ToString();
				spacepostsinfoarray[i].Category = dt.Rows[i]["category"].ToString();
				spacepostsinfoarray[i].PostStatus = TypeConverter.ObjectToInt(dt.Rows[i]["poststatus"]);
				spacepostsinfoarray[i].CommentStatus = TypeConverter.ObjectToInt(dt.Rows[i]["commentstatus"]);
				spacepostsinfoarray[i].PostUpDateTime = Convert.ToDateTime(dt.Rows[i]["postupdatetime"]);
				spacepostsinfoarray[i].Commentcount = TypeConverter.ObjectToInt(dt.Rows[i]["commentcount"]);
				spacepostsinfoarray[i].Views = TypeConverter.ObjectToInt(dt.Rows[i]["views"].ToString());
		

			}

			dt.Dispose();
			return spacepostsinfoarray;
		}
		#endregion

		#region Space 日志类型操作
        /// <summary>
        /// 获取日志分类
        /// </summary>
        /// <param name="__idatareader"></param>
        /// <returns></returns>
		public static SpaceCategoryInfo GetSpaceCategoryInfo (IDataReader idatareader)
		{
			if(idatareader == null)
				return null;

			if(idatareader.Read())
			{		
				SpaceCategoryInfo spacecategoriesinfo = new SpaceCategoryInfo();
				spacecategoriesinfo.CategoryID = TypeConverter.ObjectToInt(idatareader["categoryid"]);
				spacecategoriesinfo.Title = idatareader["title"].ToString();
				spacecategoriesinfo.Uid = TypeConverter.ObjectToInt(idatareader["uid"]);
				spacecategoriesinfo.Description = idatareader["description"].ToString();
				spacecategoriesinfo.TypeID = TypeConverter.ObjectToInt(idatareader["typeid"]);
				spacecategoriesinfo.CategoryCount = TypeConverter.ObjectToInt(idatareader["categorycount"]);
				spacecategoriesinfo.Displayorder = TypeConverter.ObjectToInt(idatareader["displayorder"]);

				idatareader.Close();
				return spacecategoriesinfo;
			}
			else
			{
                idatareader.Close();
				return null;
			}
		}
	
        /// <summary>
        /// 获取日志分类
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
		public static SpaceCategoryInfo[] GetSpaceCategories (DataTable dt)
		{

			if(dt == null || dt.Rows.Count == 0)
				return null;

			SpaceCategoryInfo[] spacecategoriesinfoarray = new SpaceCategoryInfo[dt.Rows.Count];
			for(int i = 0 ; i < dt.Rows.Count ; i++)
			{
				spacecategoriesinfoarray[i] = new SpaceCategoryInfo();
				spacecategoriesinfoarray[i].CategoryID = TypeConverter.ObjectToInt(dt.Rows[i]["categoryid"]);
				spacecategoriesinfoarray[i].Title = dt.Rows[i]["title"].ToString();
				spacecategoriesinfoarray[i].Uid = TypeConverter.ObjectToInt(dt.Rows[i]["uid"]);
				spacecategoriesinfoarray[i].Description = dt.Rows[i]["description"].ToString();
				spacecategoriesinfoarray[i].TypeID = TypeConverter.ObjectToInt(dt.Rows[i]["typeid"]);
				spacecategoriesinfoarray[i].CategoryCount = TypeConverter.ObjectToInt(dt.Rows[i]["categorycount"]);
				spacecategoriesinfoarray[i].Displayorder = TypeConverter.ObjectToInt(dt.Rows[i]["displayorder"]);
			}

			dt.Dispose();
			return spacecategoriesinfoarray;
		}
		#endregion
		
		#region Space 日志关联类型操作
		public static SpacePostCategoryInfo GetSpacePostCategoryInfo (IDataReader idatareader)
		{
			if(idatareader == null)
				return null;

			if(idatareader.Read())
			{
				SpacePostCategoryInfo spacepostcategoriesinfo = new SpacePostCategoryInfo();
				spacepostcategoriesinfo.ID = TypeConverter.ObjectToInt(idatareader["id"]);
				spacepostcategoriesinfo.PostID = TypeConverter.ObjectToInt(idatareader["postid"]);
				spacepostcategoriesinfo.CategoryID = TypeConverter.ObjectToInt(idatareader["categoryid"]);

                idatareader.Close();
				return spacepostcategoriesinfo;
			}
			else
			{
                idatareader.Close();
				return null;
			}
		}
		public static SpacePostCategoryInfo[] GetSpacePostCategories (DataTable dt)
		{
			if(dt == null || dt.Rows.Count == 0)
				return null;

			SpacePostCategoryInfo[] spacepostcategoriesinfoarray = new SpacePostCategoryInfo[dt.Rows.Count];
			for(int i = 0 ; i < dt.Rows.Count ; i++)
			{
				spacepostcategoriesinfoarray[i].ID = TypeConverter.ObjectToInt(dt.Rows[i]["id"]);
				spacepostcategoriesinfoarray[i].PostID = TypeConverter.ObjectToInt(dt.Rows[i]["postid"]);
				spacepostcategoriesinfoarray[i].CategoryID = TypeConverter.ObjectToInt(dt.Rows[i]["categoryid"]);
			}
			dt.Dispose();
			return spacepostcategoriesinfoarray;
		}
		#endregion
	
		#region Space 日志附件操作
		public static SpaceAttachmentInfo GetSpaceAttachmentInfo(IDataReader idatareader)
		{
			if(idatareader == null)
				return null;

			if(idatareader.Read())
			{
				SpaceAttachmentInfo spaceattachmentinfo = new SpaceAttachmentInfo();
				spaceattachmentinfo.AID = TypeConverter.ObjectToInt(idatareader["aid"]);
				spaceattachmentinfo.UID = TypeConverter.ObjectToInt(idatareader["uid"]);
				spaceattachmentinfo.SpacePostID = TypeConverter.ObjectToInt(idatareader["spacepostid"]);
				spaceattachmentinfo.PostDateTime = Convert.ToDateTime(idatareader["postdatetime"]);
				spaceattachmentinfo.FileName = idatareader["filename"].ToString();
				spaceattachmentinfo.FileType = idatareader["filetype"].ToString();
				spaceattachmentinfo.FileSize = TypeConverter.ObjectToInt(idatareader["filesize"]);
				spaceattachmentinfo.Attachment = idatareader["attachment"].ToString();
				spaceattachmentinfo.Downloads = TypeConverter.ObjectToInt(idatareader["downloads"]);
                idatareader.Close();
				return spaceattachmentinfo;
			}
			else
			{
                idatareader.Close();
				return null;
			}
		}


		public static SpaceAttachmentInfo[] GetSpaceAttachmentInfo (DataTable dt)
		{
			if(dt == null || dt.Rows.Count == 0)
				return null;

			SpaceAttachmentInfo[] spaceattachmentinfoarray = new SpaceAttachmentInfo[dt.Rows.Count];
			for(int i = 0 ; i < dt.Rows.Count ; i++)
			{
				spaceattachmentinfoarray[i] = new SpaceAttachmentInfo();
				spaceattachmentinfoarray[i].AID = TypeConverter.ObjectToInt(dt.Rows[i]["aid"]);
				spaceattachmentinfoarray[i].UID = TypeConverter.ObjectToInt(dt.Rows[i]["uid"]);
				spaceattachmentinfoarray[i].SpacePostID = TypeConverter.ObjectToInt(dt.Rows[i]["spacepostid"]);
				spaceattachmentinfoarray[i].PostDateTime = Convert.ToDateTime(dt.Rows[i]["postdatetime"]);
				spaceattachmentinfoarray[i].FileName = dt.Rows[i]["filename"].ToString();
				spaceattachmentinfoarray[i].FileType = dt.Rows[i]["filetype"].ToString();
				spaceattachmentinfoarray[i].FileSize = TypeConverter.ObjectToInt(dt.Rows[i]["filesize"]);
				spaceattachmentinfoarray[i].Attachment = dt.Rows[i]["attachment"].ToString();
				spaceattachmentinfoarray[i].Downloads = TypeConverter.ObjectToInt(dt.Rows[i]["downloads"]);
			}
			dt.Dispose();
			return spaceattachmentinfoarray;
		}
		#endregion

		#region 友情链接操作
		public static SpaceLinkInfo GetSpaceLinksInfo (IDataReader idatareader)
		{
			if(idatareader == null)
				return null;

			if(idatareader.Read())
			{		
				SpaceLinkInfo spacelinksinfo = new SpaceLinkInfo();
				spacelinksinfo.LinkId = TypeConverter.ObjectToInt(idatareader["linkid"]);
				spacelinksinfo.UserId = TypeConverter.ObjectToInt(idatareader["userid"]);
				spacelinksinfo.LinkTitle = idatareader["linktitle"].ToString();
				spacelinksinfo.Description = idatareader["description"].ToString();
				spacelinksinfo.LinkUrl = idatareader["linkurl"].ToString();

                idatareader.Close();
				return spacelinksinfo;
			}
			else
			{
                idatareader.Close();
				return null;
			}
		}

		public static SpaceLinkInfo[] GetSpaceLinksInfo (DataTable dt)
		{
			if(dt == null || dt.Rows.Count == 0)
				return null;

			SpaceLinkInfo[] spacelinksinfoarray = new SpaceLinkInfo[dt.Rows.Count];
			for(int i = 0 ; i < dt.Rows.Count ; i++)
			{
				spacelinksinfoarray[i] = new SpaceLinkInfo();
				spacelinksinfoarray[i].LinkId = TypeConverter.ObjectToInt(dt.Rows[i]["linkid"]);
				spacelinksinfoarray[i].UserId = TypeConverter.ObjectToInt(dt.Rows[i]["userid"]);
				spacelinksinfoarray[i].LinkTitle = dt.Rows[i]["linktitle"].ToString();
				spacelinksinfoarray[i].Description = dt.Rows[i]["description"].ToString();
				spacelinksinfoarray[i].LinkUrl =dt.Rows[i]["linkurl"].ToString();
			}
			dt.Dispose();
			return spacelinksinfoarray;
		}
		#endregion
    }
}
 