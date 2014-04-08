#if NET1

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using Discuz.Data;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;
using SQLDMO;

namespace Discuz.Data.SqlServer
{
	/// <summary>
	/// DataProvider 的摘要说明。
	/// </summary>
	public class DataProvider: IDataProvider
	{
		
		#region ForumManage

		/// <summary>
		/// SQL SERVER SQL语句转义
		/// </summary>
		/// <param name="str">需要转义的关键字符串</param>
		/// <param name="pattern">需要转义的字符数组</param>
		/// <returns>转义后的字符串</returns>
		private static string RegEsc(string str)
		{
			string[] pattern ={ @"%", @"_", @"'" };
			foreach (string s in pattern)
			{
				//Regex rgx = new Regex(s);
				//keyword = rgx.Replace(keyword, "\\" + s);
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
		/// <summary>
		/// 添加友情链接
		/// </summary>
		/// <param name="displayorder">显示顺序</param>
		/// <param name="name">名称</param>
		/// <param name="url">链接地址</param>
		/// <param name="note">备注</param>
		/// <param name="logo">Logo地址</param>
		/// <returns></returns>
		public int AddForumLink(int displayorder, string name, string url, string note, string logo)
		{
            
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder),
									  DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar, 100, name),
									  DbHelper.MakeInParam("@url", (DbType)SqlDbType.NVarChar, 100, url),
									  DbHelper.MakeInParam("@note", (DbType)SqlDbType.NVarChar, 200, note),
									  DbHelper.MakeInParam("@logo", (DbType)SqlDbType.NVarChar, 100, logo)
								  };
			string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "forumlinks] ([displayorder], [name],[url],[note],[logo]) VALUES (@displayorder,@name,@url,@note,@logo)";
			return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		/// <summary>
		/// 获得所有友情链接
		/// </summary>
		/// <returns></returns>
		public string GetForumLinks()
		{
			return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "forumlinks]";
		}

		/// <summary>
		/// 删除指定友情链接
		/// </summary>
		/// <param name="forumlinkid"></param>
		/// <returns></returns>
		public int DeleteForumLink(string forumlinkidlist)
		{
			string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "forumlinks] WHERE [id] IN (" + forumlinkidlist + ")";
			return DbHelper.ExecuteNonQuery(CommandType.Text, sql);
		}

		/// <summary>
		/// 更新指定友情链接
		/// </summary>
		/// <param name="id">友情链接Id</param>
		/// <param name="displayorder">显示顺序</param>
		/// <param name="name">名称</param>
		/// <param name="url">链接地址</param>
		/// <param name="note">备注</param>
		/// <param name="logo">Logo地址</param>
		/// <returns></returns>
		public int UpdateForumLink(int id, int displayorder, string name, string url, string note, string logo)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id),
									  DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder),
									  DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar, 100, name),
									  DbHelper.MakeInParam("@url", (DbType)SqlDbType.NVarChar, 100, url),
									  DbHelper.MakeInParam("@note", (DbType)SqlDbType.NVarChar, 200, note),
									  DbHelper.MakeInParam("@logo", (DbType)SqlDbType.NVarChar, 100, logo)
								  };
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forumlinks] SET [displayorder]=@displayorder,[name]=@name,[url]=@url,[note]=@note,[logo]=@logo WHERE [id]=@id";
			return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}


		/// <summary>
		/// 获得首页版块列表
		/// </summary>
		/// <returns></returns>
		public DataTable GetForumIndexListTable()
		{
			string commandText = string.Format("SELECT CASE WHEN DATEDIFF(n, [lastpost], GETDATE())<600 THEN 'new' ELSE 'old' END AS [havenew],[{0}forums].*, [{0}forumfields].* FROM [{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].[fid]=[{0}forumfields].[fid] WHERE [{0}forums].[parentid] NOT IN (SELECT fid FROM [{0}forums] WHERE [status] < 1 AND [layer] = 0) AND [{0}forums].[status] > 0 AND [layer] <= 1 ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
			return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
		}

		/// <summary>
		/// 获得首页版块列表
		/// </summary>
		/// <returns></returns>
		public IDataReader GetForumIndexList()
		{
			string commandText = string.Format("SELECT CASE WHEN DATEDIFF(n, [lastpost], GETDATE())<600 THEN 'new' ELSE 'old' END AS [havenew],[{0}forums].*, [{0}forumfields].* FROM [{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].[fid]=[{0}forumfields].[fid] WHERE [{0}forums].[parentid] NOT IN (SELECT fid FROM [{0}forums] WHERE [status] < 1 AND [layer] = 0) AND [{0}forums].[status] > 0 AND [layer] <= 1 ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
			return DbHelper.ExecuteReader(CommandType.Text, commandText);
		}

		/// <summary>
		/// 获得简介版论坛首页列表
		/// </summary>
		/// <returns></returns>
		public DataTable GetArchiverForumIndexList()
		{
			string commandText = string.Format("SELECT [{0}forums].[fid], [{0}forums].[name], [{0}forums].[layer], [{0}forumfields].[viewperm] FROM [{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].[fid]=[{0}forumfields].[fid] WHERE [{0}forums].[status] > 0  ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
			return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
		}

		/// <summary>
		/// 获得子版块列表
		/// </summary>
		/// <param name="fid">版块id</param>
		/// <returns></returns>
		public IDataReader GetSubForumReader(int fid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int,4, fid)
								  };

			string commandText = string.Format("SELECT CASE WHEN DATEDIFF(n, [lastpost], GETDATE())<600 THEN 'new' ELSE 'old' END AS [havenew],[{0}forums].*, [{0}forumfields].* FROM [{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].[fid]=[{0}forumfields].[fid] WHERE [parentid] = @fid AND [status] > 0 ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
			return DbHelper.ExecuteReader(CommandType.Text, commandText, prams);
		}

		/// <summary>
		/// 获得子版块列表
		/// </summary>
		/// <param name="fid">版块id</param>
		/// <returns></returns>
		public DataTable GetSubForumTable(int fid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int,4, fid)
								  };

			string commandText = string.Format("SELECT CASE WHEN DATEDIFF(n, [lastpost], GETDATE())<600 THEN 'new' ELSE 'old' END AS [havenew],[{0}forums].*, [{0}forumfields].* FROM [{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].[fid]=[{0}forumfields].[fid] WHERE [parentid] = @fid AND [status] > 0 ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
			return DbHelper.ExecuteDataset(CommandType.Text, commandText, prams).Tables[0];
		}

		/// <summary>
		/// 获得全部版块列表
		/// </summary>
		/// <returns></returns>
		public DataTable GetForumsTable()
		{
			string commandText = string.Format("SELECT [{0}forums].*, [{0}forumfields].* FROM [{0}forums] LEFT JOIN [{0}forumfields] ON [{0}forums].[fid]=[{0}forumfields].[fid] ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
			return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
		}

		/// <summary>
		/// 设置当前版块主题数(不含子版块)
		/// </summary>
		/// <param name="fid">版块id</param>
		/// <returns>主题数</returns>
		public int SetRealCurrentTopics(int fid)
		{
			string commandText =
				string.Format("UPDATE {0}forums SET [curtopics] = (SELECT COUNT(tid) FROM {0}topics WHERE [displayorder] >= 0 AND [fid]={1}) WHERE [fid]={1}", BaseConfigs.GetTablePrefix, fid);
			return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
		}

		public DataTable GetForumListTable()
		{
			DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT [name], [fid] FROM [{0}forums] WHERE [{0}forums].[parentid] NOT IN (SELECT fid FROM [{0}forums] WHERE [status] < 1 AND [layer] = 0) AND [status] > 0 AND [displayorder] >=0 ORDER BY [displayorder]", BaseConfigs.GetTablePrefix)).Tables[0];

			return dt;
		}

		public string GetTemplates()
		{
			return "SELECT [templateid],[name] FROM [" + BaseConfigs.GetTablePrefix + "templates] ";

		}

		public DataTable GetUserGroupsTitle()
		{
			string sql = "SELECT [groupid],[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups]  ORDER BY [groupid] ASC";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public DataTable GetUserGroupMaxspacephotosize()
		{
			string sql = "SELECT [groupid],[grouptitle],[maxspacephotosize] FROM [" + BaseConfigs.GetTablePrefix + "usergroups]  ORDER BY [groupid] ASC";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public DataTable GetUserGroupMaxspaceattachsize()
		{
			string sql = "SELECT [groupid],[grouptitle],[maxspaceattachsize] FROM [" + BaseConfigs.GetTablePrefix + "usergroups]  ORDER BY [groupid] ASC";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public DataTable GetAttachTypes()
		{
			string sql = "SELECT [id],[extension] FROM [" + BaseConfigs.GetTablePrefix + "attachtypes]  ORDER BY [id] ASC";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public DataTable GetForums()
		{
			string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "forums] ORDER BY [displayorder] ASC";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public string GetForumsTree()
		{
			return "SELECT [fid],[name],[parentid] FROM [" + BaseConfigs.GetTablePrefix + "forums]";
		}

		public int GetForumsMaxDisplayOrder()
		{
			return Utils.StrToInt(DbHelper.ExecuteDataset(CommandType.Text, "SELECT MAX(displayorder) FROM [" + BaseConfigs.GetTablePrefix + "forums]").Tables[0].Rows[0][0], 0) + 1;
		}

		public DataTable GetForumsMaxDisplayOrder(int parentid)
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT MAX([displayorder]) FROM [" + BaseConfigs.GetTablePrefix + "forums]  WHERE [parentid]=" + parentid).Tables[0];
		}
		public void UpdateForumsDisplayOrder(int minDisplayOrder)
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [displayorder]=[displayorder]+1  WHERE [displayorder]>" + minDisplayOrder.ToString());
		}

		public void UpdateSubForumCount(int fid)
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [subforumcount]=[subforumcount]+1  WHERE [fid]=" + fid.ToString());
		}

		public DataRow GetForum(int fid)
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=" + fid.ToString()).Tables[0].Rows[0];
		}

		public DataRowCollection GetModerators(int fid)
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [username] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid] IN(SELECT [uid] FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE [inherited]=1 AND [fid]=" + fid + ")").Tables[0].Rows;
		}

		public DataTable GetTopForum(int fid)
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [fid] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [parentid]=0 AND [layer]=0 AND [fid]=" + fid).Tables[0];
		}

		public int UpdateForum(int fid, string name, int subforumcount, int displayorder)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid),
									  DbHelper.MakeInParam("@name", (DbType)SqlDbType.NChar, 50, name),
									  DbHelper.MakeInParam("@subforumcount", (DbType)SqlDbType.Int, 4, subforumcount),
									  DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder) 
								  };
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [name]=@name,[subforumcount]=@subforumcount ,[displayorder]=@displayorder WHERE [fid]=@fid";
			return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public DataTable GetForumField(int fid, string fieldname)
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [" + fieldname + "] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] WHERE [fid]=" + fid).Tables[0];
		}

		public int UpdateForumField(int fid, string fieldname)
		{
			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET [" + fieldname + "]='' WHERE [fid]=" + fid);
		}

		public int UpdateForumField(int fid, string fieldname, string fieldvalue)
		{
			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET [" + fieldname + "]='" + fieldvalue + "' WHERE [fid]=" + fid);
		}

		public DataRowCollection GetDatechTableIds()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT id FROM [" + BaseConfigs.GetTablePrefix + "tablelist]").Tables[0].Rows;
		}

		public int UpdateMinMaxField(string posttablename, int posttableid)
		{
			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "tablelist] SET [mintid]=" + GetMinPostTableTid(posttablename) + ",[maxtid]=" + GetMaxPostTableTid(posttablename) + "  WHERE [id]=" + posttableid);
		}

		public DataRowCollection GetForumIdList()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [fid] FROM [" + BaseConfigs.GetTablePrefix + "forums]").Tables[0].Rows;
		}

		public int CreateFullTextIndex(string dbname)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("USE " + dbname + ";");
			sb.Append("execute sp_fulltext_database 'enable';");
			return DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
		}

		public int GetMaxForumId()
		{
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX(fid), 0) FROM " + BaseConfigs.GetTablePrefix + "forums"), 0);
		}

		public DataTable GetForumList()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [fid],[name] FROM [" + BaseConfigs.GetTablePrefix + "forums]").Tables[0];
		}

		public DataTable GetAllForumList()
		{
			string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "forums] ORDER BY [displayorder] ASC";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public DataTable GetForumInformation(int fid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int, 4,fid)
			};
			string sql = "SELECT [" + BaseConfigs.GetTablePrefix + "forums].*, [" + BaseConfigs.GetTablePrefix + "forumfields].* FROM [" + BaseConfigs.GetTablePrefix + "forums] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "forumfields] ON [" + BaseConfigs.GetTablePrefix + "forums].[fid]=[" + BaseConfigs.GetTablePrefix + "forumfields].[fid] WHERE [" + BaseConfigs.GetTablePrefix + "forums].[fid]=@fid";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		}

		public void SaveForumsInfo(ForumInfo foruminfo)
		{
			SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
			conn.Open();
			using (SqlTransaction trans = conn.BeginTransaction())
			{
				try
				{
					IDataParameter[] prams = {
											  DbHelper.MakeInParam("@name", (DbType)SqlDbType.NChar, 50, foruminfo.Name),
											  DbHelper.MakeInParam("@status", (DbType)SqlDbType.Int, 4, foruminfo.Status),
											  DbHelper.MakeInParam("@colcount", (DbType)SqlDbType.SmallInt, 4, foruminfo.Colcount),
											  DbHelper.MakeInParam("@templateid", (DbType)SqlDbType.SmallInt, 2, foruminfo.Templateid),
											  DbHelper.MakeInParam("@allowsmilies", (DbType)SqlDbType.Int, 4, foruminfo.Allowsmilies),
											  DbHelper.MakeInParam("@allowrss", (DbType)SqlDbType.Int, 6, foruminfo.Allowrss),
											  DbHelper.MakeInParam("@allowhtml", (DbType)SqlDbType.Int, 4, foruminfo.Allowhtml),
											  DbHelper.MakeInParam("@allowbbcode", (DbType)SqlDbType.Int, 4, foruminfo.Allowbbcode),
											  DbHelper.MakeInParam("@allowimgcode", (DbType)SqlDbType.Int, 4, foruminfo.Allowimgcode),
											  DbHelper.MakeInParam("@allowblog", (DbType)SqlDbType.Int, 4, foruminfo.Allowblog),
											  DbHelper.MakeInParam("@allowtrade", (DbType)SqlDbType.Int, 4, foruminfo.Allowtrade),
											  DbHelper.MakeInParam("@alloweditrules", (DbType)SqlDbType.Int, 4, foruminfo.Alloweditrules),
											  DbHelper.MakeInParam("@allowthumbnail", (DbType)SqlDbType.Int, 4, foruminfo.Allowthumbnail),
											  DbHelper.MakeInParam("@recyclebin", (DbType)SqlDbType.Int, 4, foruminfo.Recyclebin),
											  DbHelper.MakeInParam("@modnewposts", (DbType)SqlDbType.Int, 4, foruminfo.Modnewposts),
											  DbHelper.MakeInParam("@jammer", (DbType)SqlDbType.Int, 4, foruminfo.Jammer),
											  DbHelper.MakeInParam("@disablewatermark", (DbType)SqlDbType.Int, 4, foruminfo.Disablewatermark),
											  DbHelper.MakeInParam("@inheritedmod", (DbType)SqlDbType.Int, 4, foruminfo.Inheritedmod),
											  DbHelper.MakeInParam("@autoclose", (DbType)SqlDbType.SmallInt, 2, foruminfo.Autoclose),
											  DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, foruminfo.Displayorder),
											  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, foruminfo.Fid)
										  };
					string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [name]=@name, [status]=@status, [colcount]=@colcount, [templateid]=@templateid,[allowsmilies]=@allowsmilies ,[allowrss]=@allowrss, [allowhtml]=@allowhtml, [allowbbcode]=@allowbbcode, [allowimgcode]=@allowimgcode, [allowblog]=@allowblog,[allowtrade]=@allowtrade,[alloweditrules]=@alloweditrules ,[allowthumbnail]=@allowthumbnail ,[recyclebin]=@recyclebin, [modnewposts]=@modnewposts,[jammer]=@jammer,[disablewatermark]=@disablewatermark,[inheritedmod]=@inheritedmod,[autoclose]=@autoclose,[displayorder]=@displayorder WHERE [fid]=@fid";
					DbHelper.ExecuteNonQuery(trans, CommandType.Text, sql, prams);

					IDataParameter[] prams1 = {
											   DbHelper.MakeInParam("@description", (DbType)SqlDbType.NText, 0, foruminfo.Description),
											   DbHelper.MakeInParam("@password", (DbType)SqlDbType.NVarChar, 16, foruminfo.Password),
											   DbHelper.MakeInParam("@icon", (DbType)SqlDbType.VarChar, 255, foruminfo.Icon),
											   DbHelper.MakeInParam("@redirect", (DbType)SqlDbType.VarChar, 255, foruminfo.Redirect),
											   DbHelper.MakeInParam("@attachextensions", (DbType)SqlDbType.VarChar, 255, foruminfo.Attachextensions),
											   DbHelper.MakeInParam("@rules", (DbType)SqlDbType.Text, 0, foruminfo.Rules),
											   DbHelper.MakeInParam("@topictypes", (DbType)SqlDbType.Text, 0, foruminfo.Topictypes),
											   DbHelper.MakeInParam("@viewperm", (DbType)SqlDbType.Text, 0, foruminfo.Viewperm),
											   DbHelper.MakeInParam("@postperm", (DbType)SqlDbType.Text, 0, foruminfo.Postperm),
											   DbHelper.MakeInParam("@replyperm", (DbType)SqlDbType.Text, 0, foruminfo.Replyperm),
											   DbHelper.MakeInParam("@getattachperm", (DbType)SqlDbType.Text, 0, foruminfo.Getattachperm),
											   DbHelper.MakeInParam("@postattachperm", (DbType)SqlDbType.Text, 0, foruminfo.Postattachperm),
											   DbHelper.MakeInParam("@applytopictype", (DbType)SqlDbType.TinyInt, 1, foruminfo.Applytopictype),
											   DbHelper.MakeInParam("@postbytopictype", (DbType)SqlDbType.TinyInt, 1, foruminfo.Postbytopictype),
											   DbHelper.MakeInParam("@viewbytopictype", (DbType)SqlDbType.TinyInt, 1, foruminfo.Viewbytopictype),
											   DbHelper.MakeInParam("@topictypeprefix", (DbType)SqlDbType.TinyInt, 1, foruminfo.Topictypeprefix),
											   DbHelper.MakeInParam("@permuserlist", (DbType)SqlDbType.NText, 0, foruminfo.Permuserlist),
											   DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, foruminfo.Fid)
										   };
					sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET [description]=@description,[password]=@password,[icon]=@icon,[redirect]=@redirect,"
						+ "[attachextensions]=@attachextensions,[rules]=@rules,[topictypes]=@topictypes,[viewperm]=@viewperm,[postperm]=@postperm,[replyperm]=@replyperm,"
						+ "[getattachperm]=@getattachperm,[postattachperm]=@postattachperm,[applytopictype]=@applytopictype,[postbytopictype]=@postbytopictype,"
						+ "[viewbytopictype]=@viewbytopictype,[topictypeprefix]=@topictypeprefix,[permuserlist]=@permuserlist WHERE [fid]=@fid";
					
					DbHelper.ExecuteNonQuery(trans, CommandType.Text, sql, prams1);

					trans.Commit();
				}
				catch (Exception ex)
				{
					trans.Rollback();
					throw ex;
				}
			}
			conn.Close();
		}

		public int InsertForumsInf(ForumInfo foruminfo)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@parentid", (DbType)SqlDbType.SmallInt, 2, foruminfo.Parentid),
									  DbHelper.MakeInParam("@layer", (DbType)SqlDbType.Int, 4, foruminfo.Layer),
									  DbHelper.MakeInParam("@pathlist", (DbType)SqlDbType.NChar, 3000, foruminfo.Pathlist == null ? " " : foruminfo.Pathlist),
									  DbHelper.MakeInParam("@parentidlist", (DbType)SqlDbType.NChar, 300, foruminfo.Parentidlist== null ? " " : foruminfo.Parentidlist),
									  DbHelper.MakeInParam("@subforumcount", (DbType)SqlDbType.Int, 4, foruminfo.Subforumcount),
									  DbHelper.MakeInParam("@name", (DbType)SqlDbType.NChar, 50, foruminfo.Name),
									  DbHelper.MakeInParam("@status", (DbType)SqlDbType.Int, 4, foruminfo.Status),
									  DbHelper.MakeInParam("@colcount", (DbType)SqlDbType.SmallInt, 4, foruminfo.Colcount),
									  DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, foruminfo.Displayorder),
									  DbHelper.MakeInParam("@templateid", (DbType)SqlDbType.SmallInt, 2, foruminfo.Templateid),
									  DbHelper.MakeInParam("@allowsmilies", (DbType)SqlDbType.Int, 4, foruminfo.Allowsmilies),
									  DbHelper.MakeInParam("@allowrss", (DbType)SqlDbType.Int, 6, foruminfo.Allowrss),
									  DbHelper.MakeInParam("@allowhtml", (DbType)SqlDbType.Int, 4, foruminfo.Allowhtml),
									  DbHelper.MakeInParam("@allowbbcode", (DbType)SqlDbType.Int, 4, foruminfo.Allowbbcode),
									  DbHelper.MakeInParam("@allowimgcode", (DbType)SqlDbType.Int, 4, foruminfo.Allowimgcode),
									  DbHelper.MakeInParam("@allowblog", (DbType)SqlDbType.Int, 4, foruminfo.Allowblog),
									  DbHelper.MakeInParam("@allowtrade", (DbType)SqlDbType.Int, 4, foruminfo.Allowtrade),
									  DbHelper.MakeInParam("@alloweditrules", (DbType)SqlDbType.Int, 4, foruminfo.Alloweditrules),
									  DbHelper.MakeInParam("@allowthumbnail", (DbType)SqlDbType.Int, 4, foruminfo.Allowthumbnail),
									  DbHelper.MakeInParam("@recyclebin", (DbType)SqlDbType.Int, 4, foruminfo.Recyclebin),
									  DbHelper.MakeInParam("@modnewposts", (DbType)SqlDbType.Int, 4, foruminfo.Modnewposts),
									  DbHelper.MakeInParam("@jammer", (DbType)SqlDbType.Int, 4, foruminfo.Jammer),
									  DbHelper.MakeInParam("@disablewatermark", (DbType)SqlDbType.Int, 4, foruminfo.Disablewatermark),
									  DbHelper.MakeInParam("@inheritedmod", (DbType)SqlDbType.Int, 4, foruminfo.Inheritedmod),
									  DbHelper.MakeInParam("@autoclose", (DbType)SqlDbType.SmallInt, 2, foruminfo.Autoclose)
								  };
			string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "forums] ([parentid],[layer],[pathlist],[parentidlist],[subforumcount],[name],[status],[colcount],[displayorder],[templateid],[allowsmilies],[allowrss],[allowhtml],[allowbbcode],[allowimgcode],[allowblog],[allowtrade],[alloweditrules],[recyclebin],[modnewposts],[jammer],[disablewatermark],[inheritedmod],[autoclose],[allowthumbnail]) VALUES (@parentid,@layer,@pathlist,@parentidlist,@subforumcount,@name,@status, @colcount, @displayorder,@templateid,@allowsmilies,@allowrss,@allowhtml,@allowbbcode,@allowimgcode,@allowblog,@allowtrade,@alloweditrules,@recyclebin,@modnewposts,@jammer,@disablewatermark,@inheritedmod,@autoclose,@allowthumbnail)";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);

			int fid = GetMaxForumId();

			IDataParameter[] prams1 = {
									   DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid),
									   DbHelper.MakeInParam("@description", (DbType)SqlDbType.NText, 0, foruminfo.Description),
									   DbHelper.MakeInParam("@password", (DbType)SqlDbType.VarChar, 16, foruminfo.Password),
									   DbHelper.MakeInParam("@icon", (DbType)SqlDbType.VarChar, 255, foruminfo.Icon),
									   DbHelper.MakeInParam("@postcredits", (DbType)SqlDbType.VarChar, 255, foruminfo.Postcredits),
									   DbHelper.MakeInParam("@replycredits", (DbType)SqlDbType.VarChar, 255, foruminfo.Replycredits),
									   DbHelper.MakeInParam("@redirect", (DbType)SqlDbType.VarChar, 255, foruminfo.Redirect),
									   DbHelper.MakeInParam("@attachextensions", (DbType)SqlDbType.VarChar, 255, foruminfo.Attachextensions),
									   DbHelper.MakeInParam("@moderators", (DbType)SqlDbType.Text, 0, foruminfo.Moderators),
									   DbHelper.MakeInParam("@rules", (DbType)SqlDbType.Text, 0, foruminfo.Rules),
									   DbHelper.MakeInParam("@topictypes", (DbType)SqlDbType.Text, 0, foruminfo.Topictypes),
									   DbHelper.MakeInParam("@viewperm", (DbType)SqlDbType.Text, 0, foruminfo.Viewperm),
									   DbHelper.MakeInParam("@postperm", (DbType)SqlDbType.Text, 0, foruminfo.Postperm),
									   DbHelper.MakeInParam("@replyperm", (DbType)SqlDbType.Text, 0, foruminfo.Replyperm),
									   DbHelper.MakeInParam("@getattachperm", (DbType)SqlDbType.Text, 0, foruminfo.Getattachperm),
									   DbHelper.MakeInParam("@postattachperm", (DbType)SqlDbType.Text, 0, foruminfo.Postattachperm)
								   };
			sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "forumfields] ([fid],[description],[password],[icon],[postcredits],[replycredits],[redirect],[attachextensions],[moderators],[rules],[topictypes],[viewperm],[postperm],[replyperm],[getattachperm],[postattachperm]) VALUES (@fid,@description,@password,@icon,@postcredits,@replycredits,@redirect,@attachextensions,@moderators,@rules,@topictypes,@viewperm,@postperm,@replyperm,@getattachperm,@postattachperm)";
			DbHelper.ExecuteDataset(CommandType.Text, sql, prams1);
			return fid;
		}

		public void SetForumsPathList(string pathlist, int fid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@pathlist", (DbType)SqlDbType.VarChar, 3000, pathlist),
				DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET pathlist=@pathlist  WHERE [fid]=@fid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void SetForumslayer(int layer, string parentidlist, int fid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@layer", (DbType)SqlDbType.SmallInt, 2, layer),
				DbHelper.MakeInParam("@parentidlist", (DbType)SqlDbType.Char, 300, parentidlist),
				DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid)
			};
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [layer]=@layer WHERE [fid]=@fid", prams);
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [parentidlist]=@parentidlist WHERE [fid]=@fid", prams);
		}

		public int GetForumsParentidByFid(int fid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid)
			};
			string sql = "SELECT TOP 1 [parentid] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE fid=@fid";
			return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, sql, prams));
		}

		public void MovingForumsPos(string currentfid, string targetfid, bool isaschildnode, string extname)
		{
			SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
			conn.Open();

			using (SqlTransaction trans = conn.BeginTransaction())
			{
				try
				{
					//取得当前论坛版块的信息
					DataRow dr = DbHelper.ExecuteDataset(trans, CommandType.Text, "SELECT TOP 1 *  FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=" + currentfid).Tables[0].Rows[0];

					//取得目标论坛版块的信息
					DataRow targetdr = DbHelper.ExecuteDataset(trans, CommandType.Text, "SELECT TOP 1 *  FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=" + targetfid).Tables[0].Rows[0];

					//当前论坛版块带子版块时
					if (DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 FID FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [parentid]=" + currentfid).Tables[0].Rows.Count > 0)
					{
						#region

						string sqlstring = "";
						if (isaschildnode) //作为论坛子版块插入
						{
							//让位于当前论坛版块(分类)显示顺序之后的论坛版块全部加1(为新加入的论坛版块让位结果)
							sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [displayorder]=[displayorder]+1 WHERE [displayorder]>={0}",
								Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString()) + 1));
							DbHelper.ExecuteDataset(trans, CommandType.Text, sqlstring);

							//更新当前论坛版块的相关信息
							sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [parentid]='{1}',[displayorder]='{2}' WHERE [fid]={0}", currentfid, targetdr["fid"].ToString(), Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString().Trim()) + 1));
							DbHelper.ExecuteDataset(trans, CommandType.Text, sqlstring);
						}
						else //作为同级论坛版块,在目标论坛版块之前插入
						{
							//让位于包括当前论坛版块显示顺序之后的论坛版块全部加1(为新加入的论坛版块让位结果)
							sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [displayorder]=[displayorder]+1 WHERE [displayorder]>={0} OR [fid]={1}",
								Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString())),
								targetdr["fid"].ToString());
							DbHelper.ExecuteDataset(trans, CommandType.Text, sqlstring);

							//更新当前论坛版块的相关信息
							sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [parentid]='{1}',[displayorder]='{2}'  WHERE [fid]={0}", currentfid, targetdr["parentid"].ToString(), Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString().Trim())));
							DbHelper.ExecuteDataset(trans, CommandType.Text, sqlstring);
						}

						//更新由于上述操作所影响的版块数和帖子数
						if ((dr["topics"].ToString() != "0") && (Convert.ToInt32(dr["topics"].ToString()) > 0) && (dr["posts"].ToString() != "0") && (Convert.ToInt32(dr["posts"].ToString()) > 0))
						{
							if (dr["parentidlist"].ToString().Trim() != "")
							{
								DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics]=[topics]-" + dr["topics"].ToString() + ",[posts]=[posts]-" + dr["posts"].ToString() + "  WHERE [fid] IN(" + dr["parentidlist"].ToString().Trim() + ")");
							}
							if (targetdr["parentidlist"].ToString().Trim() != "")
							{
								DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics]=[topics]+" + dr["topics"].ToString() + ",[posts]=[posts]+" + dr["posts"].ToString() + "  WHERE [fid] IN(" + targetdr["parentidlist"].ToString().Trim() + ")");
							}
						}

						#endregion
					}
					else //当前论坛版块不带子版
					{
						#region

						//设置旧的父一级的子论坛数
						DbHelper.ExecuteDataset(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [subforumcount]=[subforumcount]-1 WHERE [fid]=" + dr["parentid"].ToString());

						//让位于当前节点显示顺序之后的节点全部减1 [起到删除节点的效果]
						if (isaschildnode) //作为子论坛版块插入
						{
							//更新相应的被影响的版块数和帖子数
							if ((dr["topics"].ToString() != "0") && (Convert.ToInt32(dr["topics"].ToString()) > 0) && (dr["posts"].ToString() != "0") && (Convert.ToInt32(dr["posts"].ToString()) > 0))
							{
								DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics]=[topics]-" + dr["topics"].ToString() + ",[posts]=[posts]-" + dr["posts"].ToString() + " WHERE [fid] IN(" + dr["parentidlist"].ToString() + ")");
								if (targetdr["parentidlist"].ToString().Trim() != "")
								{
									DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics]=[topics]+" + dr["topics"].ToString() + ",[posts]=[posts]+" + dr["posts"].ToString() + " WHERE [fid] IN(" + targetdr["parentidlist"].ToString() + "," + targetfid + ")");
								}
							}

							//让位于当前论坛版块显示顺序之后的论坛版块全部加1(为新加入的论坛版块让位结果)
							string sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [displayorder]=[displayorder]+1 WHERE [displayorder]>={0}",
								Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString()) + 1));
							DbHelper.ExecuteDataset(trans, CommandType.Text, sqlstring);

							//设置新的父一级的子论坛数
							DbHelper.ExecuteDataset(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [subforumcount]=[subforumcount]+1 WHERE [fid]=" + targetfid);

							string parentidlist = null;
							if (targetdr["parentidlist"].ToString().Trim() == "0")
							{
								parentidlist = targetfid;
							}
							else
							{
								parentidlist = targetdr["parentidlist"].ToString().Trim() + "," + targetfid;
							}

							//更新当前论坛版块的相关信息
							sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [parentid]='{1}',[layer]='{2}',[pathlist]='{3}', [parentidlist]='{4}',[displayorder]='{5}' WHERE [fid]={0}",
								currentfid,
								targetdr["fid"].ToString(),
								Convert.ToString(Convert.ToInt32(targetdr["layer"].ToString()) + 1),
                                                      targetdr["pathlist"].ToString().Trim() + "<a href=\"showforum-" + currentfid + extname + "\">" + dr["name"].ToString().Trim().Replace("'","''") + "</a>",
								parentidlist,
								Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString().Trim()) + 1)
								);
							DbHelper.ExecuteDataset(trans, CommandType.Text, sqlstring);

						}
						else //作为同级论坛版块,在目标论坛版块之前插入
						{
							//更新相应的被影响的版块数和帖子数
							if ((dr["topics"].ToString() != "0") && (Convert.ToInt32(dr["topics"].ToString()) > 0) && (dr["posts"].ToString() != "0") && (Convert.ToInt32(dr["posts"].ToString()) > 0))
							{
								DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE " + BaseConfigs.GetTablePrefix + "forums SET [topics]=[topics]-" + dr["topics"].ToString() + ",[posts]=[posts]-" + dr["posts"].ToString() + "  WHERE [fid] IN(" + dr["parentidlist"].ToString() + ")");
								DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE " + BaseConfigs.GetTablePrefix + "forums SET [topics]=[topics]+" + dr["topics"].ToString() + ",[posts]=[posts]+" + dr["posts"].ToString() + "  WHERE [fid] IN(" + targetdr["parentidlist"].ToString() + ")");
							}

							//让位于包括当前论坛版块显示顺序之后的论坛版块全部加1(为新加入的论坛版块让位结果)
							string sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [displayorder]=[displayorder]+1 WHERE [displayorder]>={0} OR [fid]={1}",
								Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString()) + 1),
								targetdr["fid"].ToString());
							DbHelper.ExecuteDataset(trans, CommandType.Text, sqlstring);

							//设置新的父一级的子论坛数
							DbHelper.ExecuteDataset(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums]  SET [subforumcount]=[subforumcount]+1 WHERE [fid]=" + targetdr["parentid"].ToString());
							string parentpathlist = "";
							DataTable dt = DbHelper.ExecuteDataset(trans, CommandType.Text, "SELECT TOP 1 [pathlist] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=" + targetdr["parentid"].ToString()).Tables[0];
							if (dt.Rows.Count > 0)
							{
								parentpathlist = DbHelper.ExecuteDataset(trans, CommandType.Text, "SELECT TOP 1 [pathlist] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=" + targetdr["parentid"].ToString()).Tables[0].Rows[0][0].ToString().Trim();
							}

							//更新当前论坛版块的相关信息
							sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "forums]  SET [parentid]='{1}',[layer]='{2}',[pathlist]='{3}', [parentidlist]='{4}',[displayorder]='{5}' WHERE [fid]={0}",
								currentfid,
								targetdr["parentid"].ToString(),
								Convert.ToInt32(targetdr["layer"].ToString()),
								parentpathlist + "<a href=\"showforum-" + currentfid + extname + "\">" + dr["name"].ToString().Trim() + "</a>",
								targetdr["parentidlist"].ToString().Trim(),
								Convert.ToString(Convert.ToInt32(targetdr["displayorder"].ToString().Trim()))
								);
							DbHelper.ExecuteDataset(trans, CommandType.Text, sqlstring);
						}

						#endregion
					}
					trans.Commit();
				}

				catch (Exception ex)
				{
					trans.Rollback();
					throw ex;
				}
				conn.Close();
			}
		}

		public bool IsExistSubForum(int fid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid)
			};
			string sql = "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [parentid]=@fid";
			if (DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0].Rows.Count > 0)
				return true;
			else
				return false;
		}

		public void DeleteForumsByFid(string postname, string fid)
		{
			SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
			conn.Open();
			using (SqlTransaction trans = conn.BeginTransaction())
			{
				try
				{
					//先取出当前节点的信息
					DataRow dr = DbHelper.ExecuteDataset(trans, CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=" + fid).Tables[0].Rows[0];

					//调整在当前节点排序位置之后的节点,做减1操作
					DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [displayorder]=[displayorder]-1 WHERE [displayorder]>" + dr["displayorder"].ToString());

					//修改父结点中的子论坛个数
					DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [subforumcount]=[subforumcount]-1 WHERE  [fid]=" + dr["parentid"].ToString());

					//删除当前节点的高级属性部分
					DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "forumfields] WHERE [fid]=" + fid);

					//删除相关投票的信息
					DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "polls] WHERE [tid] IN(SELECT [tid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid]=" + fid + ")");

					//删除帖子附件表中的信息
					DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [tid] IN(SELECT [tid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid]=" + fid + ") OR [pid] IN(SELECT [pid] FROM [" + postname + "] WHERE [fid]=" + fid + ")");

					//删除相关帖子
					DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM [" + postname + "] WHERE [fid]=" + fid);

					//删除相关主题
					DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid]=" + fid);


					//删除当前节点
					DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE  [fid]=" + fid);

					//删除版主列表中的相关信息
					DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE  [fid]=" + fid);

					trans.Commit();
				}
				catch
				{
					trans.Rollback();
				}

			}
			conn.Close();
		}

		public DataTable GetParentIdByFid(int fid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid)
			};
			string sql = "SELECT [parentid] From [" + BaseConfigs.GetTablePrefix + "forums] WHERE [inheritedmod]=1 AND [fid]=@fid";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		}

		public void InsertForumsModerators(string fid, string moderators, int displayorder, int inherited)
		{
			SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
			conn.Open();
			using (SqlTransaction trans = conn.BeginTransaction())
			{
				try
				{
					int count = displayorder;


					//数据库中存在的用户
					string usernamelist = "";
					//清除已有论坛的版主设置
					foreach (string username in moderators.Split(','))
					{
						if (username.Trim() != "")
						{
							IDataParameter[] prams =
								{
									DbHelper.MakeInParam("@username", (DbType)SqlDbType.VarChar, 20, username.Trim())
								};
							//先取出当前节点的信息
							DataTable dt = DbHelper.ExecuteDataset(trans, CommandType.Text, "SELECT TOP 1 [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid]<>7 AND [groupid]<>8 AND [username]=@username", prams).Tables[0];
							if (dt.Rows.Count > 0)
							{
								DbHelper.ExecuteNonQuery(trans, CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "moderators] ([uid],[fid],[displayorder],[inherited]) VALUES(" + dt.Rows[0][0].ToString() + "," + fid + "," + count.ToString() + "," + inherited.ToString() + ")");
								usernamelist = usernamelist + username.Trim() + ",";
								count++;
							}
						}
					}

					if (usernamelist != "")
					{
						IDataParameter[] prams1 =
							{
								DbHelper.MakeInParam("@moderators", (DbType)SqlDbType.VarChar, 255, usernamelist.Substring(0, usernamelist.Length - 1))

							};
						DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET [moderators]=@moderators WHERE [fid] =" + fid, prams1);
					}
					else
					{
						DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET [moderators]='' WHERE [fid] =" + fid);
					}

					trans.Commit();
				}
				catch (Exception ex)
				{
					trans.Rollback();
					throw ex;
				}
			}
			conn.Close();
		}

		public DataTable GetFidInForumsByParentid(int parentid)
		{
			IDataParameter[] prams =
			{
				DbHelper.MakeInParam("@parentid", (DbType)SqlDbType.Int, 4, parentid)
			};
			string sql = "SELECT [fid] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [parentid]=@parentid ORDER BY [displayorder] ASC";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		}

		public void CombinationForums(string sourcefid, string targetfid, string fidlist)
		{
			SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
			conn.Open();
			using (SqlTransaction trans = conn.BeginTransaction())
			{
				try
				{
					//ChildNode = "0";
					//string fidlist = ("," + FindChildNode(targetfid)).Replace(",0,", "");
					//更新帖子与主题的信息
					DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [fid]=" + targetfid + "  WHERE [fid]=" + sourcefid);
					//要更新目标论坛的主题数
					int totaltopics = Convert.ToInt32(DbHelper.ExecuteDataset(trans, CommandType.Text, "SELECT COUNT(tid)  FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid] IN(" + fidlist + ")").Tables[0].Rows[0][0].ToString());

					int totalposts = 0;
					foreach (DataRow postdr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT [id] From [" + BaseConfigs.GetTablePrefix + "tablelist]").Tables[0].Rows)
					{
						DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "posts" + postdr["id"].ToString() + "] SET [fid]=" + targetfid + "  WHERE [fid]=" + sourcefid);

						//要更新目标论坛的帖子数
						totalposts = totalposts + Convert.ToInt32(DbHelper.ExecuteDataset(trans, CommandType.Text, "SELECT COUNT(pid)  FROM [" + BaseConfigs.GetTablePrefix + "posts" + postdr["id"].ToString() + "] WHERE [fid] IN(" + fidlist + ")").Tables[0].Rows[0][0].ToString());
					}

					DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics]=" + totaltopics + " ,[posts]=" + totalposts + " WHERE [fid]=" + targetfid);

					//获取源论坛信息
					DataRow dr = DbHelper.ExecuteDataset(trans, CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=" + sourcefid).Tables[0].Rows[0];

					//调整在当前节点排序位置之后的节点,做减1操作
					DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [displayorder]=[displayorder]-1 WHERE [displayorder]>" + dr["displayorder"].ToString());

					//修改父结点中的子论坛个数
					DbHelper.ExecuteNonQuery(trans, CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [subforumcount]=[subforumcount]-1 WHERE [fid]=" + dr["parentid"].ToString());

					//删除当前节点的高级属性部分
					DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "forumfields] WHERE [fid]=" + sourcefid);

					//删除源论坛版块
					DbHelper.ExecuteNonQuery(trans, CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=" + sourcefid);
					trans.Commit();

				}
				catch (Exception ex)
				{
					trans.Rollback();
					throw ex;
				}
			}
			conn.Close();
		}

		public void UpdateSubForumCount(int subforumcount, int fid)
		{
			IDataParameter[] prams =
			{
				DbHelper.MakeInParam("@subforumcount", (DbType)SqlDbType.Int, 4, subforumcount),
				DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [subforumcount]=@subforumcount WHERE [fid]=@fid";
			DbHelper.ExecuteDataset(CommandType.Text, sql, prams);
		}

		public void UpdateDisplayorderInForumByFid(int displayorder, int fid)
		{
			IDataParameter[] prams =
			{
				DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder),
				DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [displayorder]=@displayorder WHERE [fid]=@fid";
			DbHelper.ExecuteDataset(CommandType.Text, sql, prams);
		}

		public DataTable GetMainForum()
		{
			string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [layer]=0 Order By [displayorder] ASC";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public void SetStatusInForum(int status, int fid)
		{
			IDataParameter[] prams =
			{
				DbHelper.MakeInParam("@status", (DbType)SqlDbType.Int, 4, status),
				DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [status]=@status WHERE [fid]=@fid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public DataTable GetForumByParentid(int parentid)
		{
			IDataParameter[] prams =
			{
				DbHelper.MakeInParam("@parentid", (DbType)SqlDbType.Int, 4, parentid)
			};
			string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [parentid]=@parentid ORDER BY [DisplayOrder]";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		}

		public void UpdateStatusByFidlist(string fidlist)
		{
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [status]=0 WHERE [fid] IN(" + fidlist + ")";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql);
		}

		public void UpdateStatusByFidlistOther(string fidlist)
		{
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [status]=1 WHERE [status]>1 AND [fid] IN(" + fidlist + ")";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql);
		}

		public bool BatchSetForumInf(ForumInfo foruminfo, BatchSetParams bsp, string fidlist)
		{
			StringBuilder forums = new StringBuilder();
			StringBuilder forumfields = new StringBuilder();

			forums.Append("UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET ");
			if (bsp.SetSetting)
			{
				forums.Append("[Allowsmilies]='" + foruminfo.Allowsmilies + "' ,");
				forums.Append("[Allowrss]='" + foruminfo.Allowrss + "' ,");
				forums.Append("[Allowhtml]='" + foruminfo.Allowhtml + "' ,");
				forums.Append("[Allowbbcode]='" + foruminfo.Allowbbcode + "' ,");
				forums.Append("[Allowimgcode]='" + foruminfo.Allowimgcode + "' ,");
				forums.Append("[Allowblog]='" + foruminfo.Allowblog + "' ,");
				forums.Append("[Allowtrade]='" + foruminfo.Allowtrade + "' ,");
				forums.Append("[Alloweditrules]='" + foruminfo.Alloweditrules + "' ,");
				forums.Append("[allowthumbnail]='" + foruminfo.Allowthumbnail + "' ,");
				forums.Append("[Recyclebin]='" + foruminfo.Recyclebin + "' ,");
				forums.Append("[Modnewposts]='" + foruminfo.Modnewposts + "' ,");
				forums.Append("[Jammer]='" + foruminfo.Jammer + "' ,");
				forums.Append("[Disablewatermark]='" + foruminfo.Disablewatermark + "' ,");
				forums.Append("[Inheritedmod]='" + foruminfo.Inheritedmod + "' ,");
			}
			if (forums.ToString().EndsWith(","))
			{
				forums.Remove(forums.Length - 1, 1);
			}
			forums.Append("WHERE [fid] IN(" + fidlist + ")");


			forumfields.Append("UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET ");

			if (bsp.SetPassWord)
			{
				forumfields.Append("[password]='" + foruminfo.Password + "' ,");
			}

			if (bsp.SetAttachExtensions)
			{
				forumfields.Append("[attachextensions]='" + foruminfo.Attachextensions + "' ,");
			}

			if (bsp.SetPostCredits)
			{
				forumfields.Append("[postcredits]='" + foruminfo.Postcredits + "' ,");
			}

			if (bsp.SetReplyCredits)
			{
				forumfields.Append("[replycredits]='" + foruminfo.Replycredits + "' ,");
			}


			if (bsp.SetViewperm)
			{
				forumfields.Append("[Viewperm]='" + foruminfo.Viewperm + "' ,");
			}

			if (bsp.SetPostperm)
			{
				forumfields.Append("[Postperm]='" + foruminfo.Postperm + "' ,");
			}

			if (bsp.SetReplyperm)
			{
				forumfields.Append("[Replyperm]='" + foruminfo.Replyperm + "' ,");
			}

			if (bsp.SetGetattachperm)
			{
				forumfields.Append("[Getattachperm]='" + foruminfo.Getattachperm + "' ,");
			}

			if (bsp.SetPostattachperm)
			{
				forumfields.Append("[Postattachperm]='" + foruminfo.Postattachperm + "' ,");
			}

			if (forumfields.ToString().EndsWith(","))
			{
				forumfields.Remove(forumfields.Length - 1, 1);
			}

			forumfields.Append("WHERE [fid] IN(" + fidlist + ")");


			SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
			conn.Open();
			using (SqlTransaction trans = conn.BeginTransaction())
			{
				try
				{
					if (forums.ToString().IndexOf("SET WHERE") < 0)
					{
						DbHelper.ExecuteNonQuery(trans, CommandType.Text, forums.ToString());
					}

					if (forumfields.ToString().IndexOf("SET WHERE") < 0)
					{
						DbHelper.ExecuteNonQuery(trans, CommandType.Text, forumfields.ToString());
					}
					trans.Commit();
				}
				catch
				{
					trans.Rollback();
					return false;
				}
			}
			return true;
		}

		public IDataReader GetTopForumFids(int lastfid, int statcount)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@lastfid", (DbType)SqlDbType.Int, 4, lastfid),
			};

			return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP " + statcount + " [fid] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid] > @lastfid", prams);
		}

		public DataSet GetOnlineList()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [groupid],(SELECT TOP 1 [grouptitle]  FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [" + BaseConfigs.GetTablePrefix + "usergroups].[groupid]=[" + BaseConfigs.GetTablePrefix + "onlinelist].[groupid]) AS GroupName ,[displayorder],[title],[img] FROM [" + BaseConfigs.GetTablePrefix + "onlinelist] ORDER BY [groupid] ASC");
		}

		public int UpdateOnlineList(int groupid, int displayorder, string img, string title)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupid),
									  DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder),
									  DbHelper.MakeInParam("@img", (DbType)SqlDbType.VarChar, 50, img),
									  DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 50, title)
								  };
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "onlinelist] SET [displayorder]=@displayorder,[title]=@title,[img]=@img  WHERE [groupid]=@groupid";
			return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public string GetWords()
		{
			return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "words]";
		}

		public int DeleteWord(int id)
		{
			IDataParameter parm = DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id);

			string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "words] WHERE [id]=@id";
			return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
		}

		public int UpdateWord(int id, string find, string replacement)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id),
									  DbHelper.MakeInParam("@find", (DbType)SqlDbType.VarChar, 255, find),
									  DbHelper.MakeInParam("@replacement", (DbType)SqlDbType.VarChar, 255, replacement)
								  };

			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "words] SET [find]=@find, [replacement]=@replacement WHERE [id]=@id";

			return DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public int DeleteWords(string idlist)
		{
			return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "words]  WHERE [ID] IN(" + idlist + ")");
		}

		public bool ExistWord(string find)
		{
			IDataParameter parm = DbHelper.MakeInParam("@find", (DbType)SqlDbType.NVarChar, 255, find);
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1  * FROM [" + BaseConfigs.GetTablePrefix + "words] WHERE [find]=@find", parm).Tables[0].Rows.Count > 0;
		}

		public int AddWord(string username, string find, string replacement)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@username", (DbType)SqlDbType.NVarChar, 20, username),
									  DbHelper.MakeInParam("@find", (DbType)SqlDbType.NVarChar, 255, find),
									  DbHelper.MakeInParam("@replacement", (DbType)SqlDbType.NVarChar, 255, replacement)
								  };
			string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "words] ([admin], [find], [replacement]) VALUES (@username,@find,@replacement)";

			return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public bool IsExistTopicType(string typename,int currenttypeid)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@typename", (DbType)SqlDbType.NVarChar, 30, typename),
									  DbHelper.MakeInParam("@currenttypeid", (DbType)SqlDbType.Int, 4, currenttypeid)
								  };
			string sql = "SELECT [typeid] FROM [" + BaseConfigs.GetTablePrefix + "topictypes] WHERE [name]=@typename AND [typeid]<>@currenttypeid";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0].Rows.Count != 0;
		}

		public bool IsExistTopicType(string typename)
		{
			IDataParameter parms = DbHelper.MakeInParam("@typename", (DbType)SqlDbType.NVarChar, 30, typename);
			string sql = "SELECT TOP 1  * FROM [" + BaseConfigs.GetTablePrefix + "topictypes] WHERE [name]=@typename";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0].Rows.Count != 0;
		}

		public string GetTopicTypes()
		{
			return "SELECT [typeid] AS id,[name],[displayorder],[description] FROM [" + BaseConfigs.GetTablePrefix + "topictypes] ORDER BY [displayorder] ASC";
		}

		public DataTable GetExistTopicTypeOfForum()
		{
			string sql = "SELECT [fid],[topictypes] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] WHERE [topictypes] NOT LIKE ''";
			return DbHelper.ExecuteDataset(CommandType.Text,sql).Tables[0];
		}

		public void UpdateTopicTypeForForum(string topictypes,int fid)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@topictypes", (DbType)SqlDbType.Text, 0, topictypes),
									  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid)
								  };
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET [topictypes]=@topictypes WHERE [fid]=@fid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public void UpdateTopicTypes(string name, int displayorder,string description,int typeid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar,100, name),
									  DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int,4,displayorder),
									  DbHelper.MakeInParam("@description", (DbType)SqlDbType.VarChar,500,description),
									  DbHelper.MakeInParam("@typeid", (DbType)SqlDbType.Int,4,typeid)
								  };
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "topictypes] SET [name]=@name ,[displayorder]=@displayorder, [description]=@description WHERE [typeid]=@typeid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void AddTopicTypes(string typename, int displayorder, string description)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@name",(DbType)SqlDbType.NVarChar,100, typename),
									  DbHelper.MakeInParam("@displayorder",(DbType)SqlDbType.Int,4,displayorder),
									  DbHelper.MakeInParam("@description",(DbType)SqlDbType.VarChar,500,description)
								  };
			string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "topictypes] ([name],[displayorder],[description]) VALUES(@name,@displayorder,@description)";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public int GetMaxTopicTypesId()
		{
			return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, "SELECT MAX([typeid]) FROM [" + BaseConfigs.GetTablePrefix + "topictypes]").ToString());
		}

		public void DeleteTopicTypesByTypeidlist(string typeidlist)
		{
			string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "topictypes]  WHERE [typeid] IN(" + typeidlist + ")";
			DbHelper.ExecuteNonQuery(CommandType.Text,sql);
		}

		public DataTable GetForumNameIncludeTopicType()
		{
			string sql = "SELECT f1.[fid],[name],[topictypes] FROM [" + BaseConfigs.GetTablePrefix + "forums] AS f1 LEFT JOIN [" + BaseConfigs.GetTablePrefix + "forumfields] AS f2 ON f1.fid=f2.fid";
			return DbHelper.ExecuteDataset(CommandType.Text,sql).Tables[0];
		}

		public DataTable GetForumTopicType()
		{
			string sql = "SELECT [fid],[topictypes] FROM [" + BaseConfigs.GetTablePrefix + "forumfields]";
			return DbHelper.ExecuteDataset(CommandType.Text,sql).Tables[0];
		}

		public void ClearTopicTopicType(int typeid)
		{
			IDataParameter pram = DbHelper.MakeInParam("@typeid", (DbType)SqlDbType.Int, 4, typeid);
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [typeid]=0 Where [typeid]=@typeid";
			DbHelper.ExecuteNonQuery(CommandType.Text,sql,pram);
		}

		public string GetTopicTypeInfo()
		{
			return "SELECT [typeid] AS id,[name],[description] FROM [" + BaseConfigs.GetTablePrefix + "topictypes] ORDER BY [displayorder] ASC";
		}

		public string GetTemplateName()
		{
			return "SELECT [templateid],[name] FROM [" + BaseConfigs.GetTablePrefix + "templates]";
		}

		public DataTable GetAttachType()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [id],[extension]  FROM [" + BaseConfigs.GetTablePrefix + "attachtypes]  ORDER BY [id] ASC").Tables[0];
		}

		public void UpdatePermUserListByFid(string permuserlist, int fid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@permuserlist", (DbType)SqlDbType.NText,0,permuserlist),
				DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int,4, fid)
			};
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET [Permuserlist]=@permuserlist WHERE [fid]=@fid", prams);
		}

		public IDataReader GetTopicsIdentifyItem()
		{
			return DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topicidentify]");
		}

		public string ResetTopTopicListSql(int layer, string fid, string parentidlist)
		{

			string filterexpress = "";

			switch (layer)
			{
				case 0:
					filterexpress = string.Format("[fid]<>{0} AND (',' + TRIM([parentidlist]) + ',' LIKE '%,{1},%')", fid.ToString(), RegEsc(fid.ToString()));
					break;
				case 1:
					filterexpress = parentidlist.ToString().Trim();
					if (filterexpress != string.Empty)
					{
						filterexpress =
							string.Format(
							"[fid]<>{0} AND ([fid]={1} OR (',' + TRIM([parentidlist]) + ',' LIKE '%,{2},%'))",
							fid.ToString().Trim(), filterexpress, RegEsc(filterexpress));
					}
					else
					{
						filterexpress =
							string.Format(
							"[fid]<>{0} AND (',' + TRIM([parentidlist]) + ',' LIKE '%,{1},%')",
							fid.ToString().Trim(), RegEsc(filterexpress));
					}
					break;
				default:
					filterexpress = parentidlist.ToString().Trim();
					if (filterexpress != string.Empty)
					{
						filterexpress = Utils.CutString(filterexpress, 0, filterexpress.IndexOf(","));
						filterexpress =
							string.Format(
							"[fid]<>{0} AND ([fid]={1} OR (',' + TRIM([parentidlist]) + ',' LIKE '%,{2},%'))",
							fid.ToString().Trim(), filterexpress, RegEsc(filterexpress));
					}
					else
					{
						filterexpress =
							string.Format(
							"[fid]<>{0} AND (',' + TRIM([parentidlist]) + ',' LIKE '%,{1},%')",
							fid.ToString().Trim(), RegEsc(filterexpress));
					}
					break;
			}

			return filterexpress;
		}

		public string showforumcondition(int sqlid, int cond)
		{

			string sql = null;
			switch (sqlid)
			{
				case 1:
					sql = " AND [typeid]=";
					break;
				case 2:
					sql = " AND [postdatetime]>='" + DateTime.Now.AddDays(-1 * cond).ToString("yyyy-MM-dd HH:mm:ss") + "'";
					break;

				case 3:
					sql = "tid";
					break;

			}
			return sql;

		}

		public string DelVisitLogCondition(string deletemod, string visitid, string deletenum, string deletefrom)
		{
			string condition = null;
			switch (deletemod)
			{
				case "chkall":
					if (visitid != "")
						condition = " [visitid] IN(" + visitid + ")";
					break;
				case "deleteNum":
					if (deletenum != "" && Utils.IsNumeric(deletenum))
						condition = " [visitid] NOT IN (SELECT TOP " + deletenum + " [visitid] FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] ORDER BY [visitid] DESC)";
					break;
				case "deleteFrom":
					if (deletefrom != "")
						condition = " [postdatetime]<'" + deletefrom + "'";
					break;
			}
			return condition;
		}


		public string AttachDataBind(string condition, string postname)
		{


			return "SELECT [aid], [attachment], [filename], (SELECT TOP 1 [poster] FROM [" + postname + "] WHERE [" + postname + "].[pid]=[" + BaseConfigs.GetTablePrefix + "attachments].[pid]) AS [poster],(Select TOP 1 [title] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [" + BaseConfigs.GetTablePrefix + "topics].[tid]=[" + BaseConfigs.GetTablePrefix + "attachments].[tid]) AS [topictitle], [filesize],[downloads]  FROM [" + BaseConfigs.GetTablePrefix + "attachments] " + condition;
		}

		public DataTable GetAttachDataTable(string condition, string postname)
		{
			string sqlstring = "SELECT [aid], [attachment], [filename], (SELECT TOP 1 [poster] FROM [" + postname + "] WHERE [" + postname + "].[pid]=[" + BaseConfigs.GetTablePrefix + "attachments].[pid]) AS [poster],(Select TOP 1 [title] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [" + BaseConfigs.GetTablePrefix + "topics].[tid]=[" + BaseConfigs.GetTablePrefix + "attachments].[tid]) AS [topictitle], [filesize],[downloads]  FROM [" + BaseConfigs.GetTablePrefix + "attachments] " + condition;
			return DbHelper.ExecuteDataset(sqlstring).Tables[0];
		}


		public bool AuditTopicCount(string condition)
		{

			if (DbHelper.ExecuteDataset("SELECT COUNT(tid) FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE " + condition).Tables[0].Rows[0][0].ToString() == "0")
			{
				return true;
			}
			else
			{
				return false;
			}


		}

		public string AuditTopicBindStr(string condition)
		{


			return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE " + condition;
		}

		public DataTable AuditTopicBind(string condition)
		{
			//IDataParameter param =
			//    DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, postdatetime);

			DataTable dt = DbHelper.ExecuteDataset("SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE " + condition).Tables[0];
			return dt;
		}

		public string AuditNewUserClear(string regbefore, string regip)
		{
			string sqlstring = "";
			sqlstring += " [groupid]=8";
			if (regbefore != "")
			{
				sqlstring += " AND [joindate]<='" + DateTime.Now.AddDays(-Convert.ToInt32(regbefore)).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
				//sqlstring += " AND [joindate]<=@joindate ";
			}

			if (regip != "")
			{
				sqlstring += " AND [regip] LIKE '" + RegEsc(regip) + "%'";
			}

			return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE " + sqlstring;
		}

		public string DelMedalLogCondition(string deletemode, string id, string deleteNum, string deleteFrom)
		{
			string condition = "";
			switch (deletemode)
			{
				case "chkall":
					if (id != "")
						condition = " [id] IN(" + id + ")";
					break;
				case "deleteNum":
					if (deleteNum != "" && Utils.IsNumeric(deleteNum))
						condition = " [id] NOT IN (SELECT TOP " + deleteNum + " [id] FROM [" + BaseConfigs.GetTablePrefix + "medalslog] ORDER BY [id] DESC)";
					break;
				case "deleteFrom":
					if (deleteFrom != "")
						condition = " [postdatetime]<'" + DateTime.Parse(deleteFrom).ToString("yyyy-MM-dd HH:mm:ss") + "'";
					break;
			}
			return condition;

		}

		public DataTable MedalsTable(string medalid)
		{

			DataTable dt = DbHelper.ExecuteDataset("SELECT * FROM [" + BaseConfigs.GetTablePrefix + "medals] WHERE [medalid]=" + medalid).Tables[0];
			return dt;
		}

		public string DelModeratorManageCondition(string deletemode, string id, string deleteNum, string deleteFrom)
		{
			string condition = "";
			switch (deletemode)
			{
				case "chkall":
					if (id!= "")
						condition = " [id] IN(" + id + ")";
					break;
				case "deleteNum":
					if (deleteNum != "" && Utils.IsNumeric(deleteNum))
						condition = " [id] NOT IN (SELECT TOP " + deleteNum + " [id] FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog] ORDER BY [id] DESC)";
					break;
				case "deleteFrom":
					if (deleteFrom != "")
						condition = " [postdatetime]<'" + DateTime.Parse(deleteFrom).ToString("yyyy-MM-dd HH:mm:ss") + "'";
					break;
			}
			return condition;
		}

		public DataTable GroupNameTable(string groupid)
		{
			DataTable dt = DbHelper.ExecuteDataset("SELECT TOP 1 [grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=" + groupid).Tables[0];
			return dt;
		}

		public string PaymentLogCondition(string deletemode, string id, string deleteNum, string deleteFrom)
		{
			string condition = "";
			switch (deletemode)
			{
				case "chkall":
					if (id != "")
						condition = " [id] IN(" + id + ")";
					break;
				case "deleteNum":
					if (deleteNum != "" && Utils.IsNumeric(deleteNum))
						condition = " [id] NOT IN (SELECT TOP " + deleteNum + " [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] ORDER BY [id] DESC)";
					break;
				case "deleteFrom":
					if (deleteFrom != "")
						condition = " [buydate]<'" + DateTime.Parse(deleteFrom).ToString("yyyy-MM-dd HH:mm:ss") + "'";
					break;
			}
			return condition;
		}

		public string PostGridBind(string posttablename, string condition)
		{


			return "SELECT * FROM [" + posttablename + "] WHERE " + condition.ToString();
		}

		public string DelRateScoreLogCondition(string deletemode, string id, string deleteNum, string deleteFrom)
		{
			string condition = "";
			switch (deletemode)
			{
				case "chkall":
					if (id != "")
						condition = " [id] IN(" + id + ")";
					break;
				case "deleteNum":
					if (deleteNum != "" && Utils.IsNumeric(deleteNum))
						condition = " [id] NOT IN (SELECT TOP " + deleteNum + " [id] FROM [" + BaseConfigs.GetTablePrefix + "ratelog] ORDER BY [id] DESC)";
					break;
				case "deleteFrom":
					if (deleteFrom != "")
						condition = " [postdatetime]<'" + DateTime.Parse(deleteFrom).ToString("yyyy-MM-dd HH:mm:ss") + "'";
					break;
			}
			return condition;
		}

		public void UpdatePostSP()
		{
			#region 更新分表的存储过程
			foreach (DataRow dr in DatabaseProvider.GetInstance().GetDatechTableIds())
			{
				CreateStoreProc(Convert.ToInt16(dr["id"].ToString()));
			}
			#endregion
		}

		public void CreateStoreProc(int tablelistmaxid)
		{
			#region 创建分表存储过程
			StringBuilder sb = new StringBuilder(@"

                    if exists (select * from sysobjects where id = object_id(N'[dnt_createpost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
                    drop procedure [dnt_createpost]

                    ~

                    if exists (select * from sysobjects where id = object_id(N'[dnt_getfirstpostid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
                    drop procedure [dnt_getfirstpostid]

                    ~

                    if exists (select * from sysobjects where id = object_id(N'[dnt_getpostcount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
                    drop procedure [dnt_getpostcount]

                    ~

                    if exists (select * from sysobjects where id = object_id(N'[dnt_deletepostbypid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
                    drop procedure [dnt_deletepostbypid]

                    ~

                    if exists (select * from sysobjects where id = object_id(N'[dnt_getposttree]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
                    drop procedure [dnt_getposttree]

                    ~

                    if exists (select * from sysobjects where id = object_id(N'[dnt_getsinglepost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
                    drop procedure [dnt_getsinglepost]

                    ~

                    if exists (select * from sysobjects where id = object_id(N'[dnt_updatepost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
                    drop procedure [dnt_updatepost]

                    ~

                    if exists (select * from sysobjects where id = object_id(N'[dnt_getnewtopics]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
                    drop procedure [dnt_getnewtopics]

                    ~

                    CREATE PROCEDURE dnt_createpost
                    @fid int,
                    @tid int,
                    @parentid int,
                    @layer int,
                    @poster varchar(20),
                    @posterid int,
                    @title nvarchar(60),
                    @postdatetime char(20),
                    @message ntext,
                    @ip varchar(15),
                    @lastedit varchar(50),
                    @invisible int,
                    @usesig int,
                    @htmlon int,
                    @smileyoff int,
                    @bbcodeoff int,
                    @parseurloff int,
                    @attachment int,
                    @rate int,
                    @ratetimes int

                    AS


                    DEClARE @postid int

                    DELETE FROM [dnt_postid] WHERE DATEDIFF(n, postdatetime, GETDATE()) >5

                    INSERT INTO [dnt_postid] ([postdatetime]) VALUES(GETDATE())

                    SELECT @postid=SCOPE_IDENTITY()

                    INSERT INTO [dnt_posts1]([pid], [fid], [tid], [parentid], [layer], [poster], [posterid], [title], [postdatetime], [message], [ip], [lastedit], [invisible], [usesig], [htmlon], [smileyoff], [bbcodeoff], [parseurloff], [attachment], [rate], [ratetimes]) VALUES(@postid, @fid, @tid, @parentid, @layer, @poster, @posterid, @title, @postdatetime, @message, @ip, @lastedit, @invisible, @usesig, @htmlon, @smileyoff, @bbcodeoff, @parseurloff, @attachment, @rate, @ratetimes)

                    IF @parentid=0
                        BEGIN
                    		
                            UPDATE [dnt_posts1] SET [parentid]=@postid WHERE [pid]=@postid
                        END

                    IF @@ERROR=0
                        BEGIN
                            IF  @invisible = 0
                            BEGIN
                    		
                                UPDATE [dnt_statistics] SET [totalpost]=[totalpost] + 1
                    		
                    		
                    		
                                DECLARE @fidlist AS VARCHAR(1000)
                                DECLARE @strsql AS VARCHAR(4000)
                    			
                                SET @fidlist = '';
                    			
                                SELECT @fidlist = ISNULL([parentidlist],'') FROM [dnt_forums] WHERE [fid] = @fid
                                IF RTRIM(@fidlist)<>''
	                                BEGIN
		                                SET @fidlist = RTRIM(@fidlist) + ',' + CAST(@fid AS VARCHAR(10))
	                                END
                                ELSE
	                                BEGIN
		                                SET @fidlist = CAST(@fid AS VARCHAR(10))
	                                END
                            
                    			
                                UPDATE [dnt_forums] SET 
						                                [posts]=[posts] + 1, 
						                                [todayposts]=CASE 
										                                WHEN DATEDIFF(day, [lastpost], GETDATE())=0 THEN [todayposts]*1 + 1 
									                                 ELSE 1 
									                                 END,
						                                [lasttid]=@tid,	
						                                [lasttitle]=@title,
						                                [lastpost]=@postdatetime,
						                                [lastposter]=@poster,
						                                [lastposterid]=@posterid 
                    							
				                                WHERE (CHARINDEX(',' + RTRIM([fid]) + ',', ',' + (SELECT @fidlist AS [fid]) + ',') > 0)
                    			
                    			
                                UPDATE [dnt_users] SET
	                                [lastpost] = @postdatetime,
	                                [lastpostid] = @postid,
	                                [lastposttitle] = @title,
	                                [posts] = [posts] + 1,
	                                [lastactivity] = GETDATE()
                                WHERE [uid] = @posterid
                            
                            
                                IF @layer<=0
	                                BEGIN
		                                UPDATE [dnt_topics] SET [replies]=0,[lastposter]=@poster,[lastpost]=@postdatetime,[lastposterid]=@posterid WHERE [tid]=@tid
	                                END
                                ELSE
	                                BEGIN
		                                UPDATE [dnt_topics] SET [replies]=[replies] + 1,[lastposter]=@poster,[lastpost]=@postdatetime,[lastposterid]=@posterid WHERE [tid]=@tid
	                                END
                            END

                            UPDATE [dnt_topics] SET [lastpostid]=@postid WHERE [tid]=@tid
                            
                        IF @posterid <> -1
                            BEGIN
                                INSERT [dnt_myposts]([uid], [tid], [pid], [dateline]) VALUES(@posterid, @tid, @postid, @postdatetime)
                            END

                        END
                    	
                    SELECT @postid AS postid

                    ~


                    CREATE PROCEDURE dnt_getfirstpostid
                    @tid int
                    AS
                    SELECT TOP 1 [pid] FROM [dnt_posts1] WHERE [tid]=@tid ORDER BY [pid]

                    ~


                    CREATE PROCEDURE dnt_getpostcount
                    @tid int
                    AS
                    SELECT COUNT(pid) FROM [dnt_posts1] WHERE [tid]=@tid AND [invisible]=0 AND layer>0

                    ~


                    CREATE  PROCEDURE dnt_deletepostbypid
                        @pid int
                    AS

                        DECLARE @fid int
                        DECLARE @tid int
                        DECLARE @posterid int
                        DECLARE @lastforumposterid int
                        DECLARE @layer int
                        DECLARE @postdatetime smalldatetime
                        DECLARE @poster varchar(50)
                        DECLARE @postcount int
                        DECLARE @title nchar(60)
                        DECLARE @lasttid int
                        DECLARE @postid int
                        DECLARE @todaycount int
                    	
                    	
                        SELECT @fid = [fid],@tid = [tid],@posterid = [posterid],@layer = [layer], @postdatetime = [postdatetime] FROM [dnt_posts1] WHERE pid = @pid

                        DECLARE @fidlist AS VARCHAR(1000)
                    	
                        SET @fidlist = '';
                    	
                        SELECT @fidlist = ISNULL([parentidlist],'') FROM [dnt_forums] WHERE [fid] = @fid
                        IF RTRIM(@fidlist)<>''
                            BEGIN
                                SET @fidlist = RTRIM(@fidlist) + ',' + CAST(@fid AS VARCHAR(10))
                            END
                        ELSE
                            BEGIN
                                SET @fidlist = CAST(@fid AS VARCHAR(10))
                            END


                        IF @layer<>0

                            BEGIN
                    			
                                UPDATE [dnt_statistics] SET [totalpost]=[totalpost] - 1

                                UPDATE [dnt_forums] SET 
	                                [posts]=[posts] - 1, 
	                                [todayposts]=CASE 
						                                WHEN DATEPART(yyyy, @postdatetime)=DATEPART(yyyy,GETDATE()) AND DATEPART(mm, @postdatetime)=DATEPART(mm,GETDATE()) AND DATEPART(dd, @postdatetime)=DATEPART(dd,GETDATE()) THEN [todayposts] - 1
						                                ELSE [todayposts]
				                                END						
                                WHERE (CHARINDEX(',' + RTRIM([fid]) + ',', ',' +
					                                (SELECT @fidlist AS [fid]) + ',') > 0)
                    			
                                UPDATE [dnt_users] SET			
	                                [posts] = [posts] - 1
                                WHERE [uid] = @posterid

                                UPDATE [dnt_topics] SET [replies]=[replies] - 1 WHERE [tid]=@tid
                    			
                                DELETE FROM [dnt_posts1] WHERE [pid]=@pid
                    			
                            END
                        ELSE
                            BEGIN
                    		
                                SELECT @postcount = COUNT([pid]) FROM [dnt_posts1] WHERE [tid] = @tid
                                SELECT @todaycount = COUNT([pid]) FROM [dnt_posts1] WHERE [tid] = @tid AND DATEDIFF(d, [postdatetime], GETDATE()) = 0
                    			

                                UPDATE [dnt_statistics] SET [totaltopic]=[totaltopic] - 1, [totalpost]=[totalpost] - @postcount
                    			
                                UPDATE [dnt_forums] SET [posts]=[posts] - @postcount, [topics]=[topics] - 1,[todayposts]=[todayposts] - @todaycount WHERE (CHARINDEX(',' + RTRIM([fid]) + ',', ',' +(SELECT @fidlist AS [fid]) + ',') > 0)
                    			
                                UPDATE [dnt_users] SET
	                                [posts] = [posts] - @postcount					
                                WHERE [uid] = @posterid
                    			

                                DELETE FROM [dnt_posts1] WHERE [tid] = @tid
                    			
                                DELETE FROM [dnt_topics] WHERE [tid] = @tid
                    			
                            END	
                    		

                        IF @layer<>0
                            BEGIN
                                SELECT TOP 1 @pid = [pid], @posterid = [posterid], @postdatetime = [postdatetime], @title = [title], @poster = [poster] FROM [dnt_posts1] WHERE [tid]=@tid ORDER BY [pid] DESC
                                UPDATE [dnt_topics] SET [lastposter]=@poster,[lastpost]=@postdatetime,[lastpostid]=@pid,[lastposterid]=@posterid WHERE [tid]=@tid
                            END



                        SELECT @lasttid = [lasttid] FROM [dnt_forums] WHERE [fid] = @fid

                    	
                        IF @lasttid = @tid
                            BEGIN

                    			
                    			

                                SELECT TOP 1 @pid = [pid], @tid = [tid],@lastforumposterid = [posterid], @title = [title], @postdatetime = [postdatetime], @poster = [poster] FROM [dnt_posts1] WHERE [fid] = @fid ORDER BY [pid] DESC
                    			
                            
                            
                                UPDATE [dnt_forums] SET 
                    			
	                                [lasttid]=@tid,
	                                [lasttitle]=ISNULL(@title,''),
	                                [lastpost]=@postdatetime,
	                                [lastposter]=ISNULL(@poster,''),
	                                [lastposterid]=ISNULL(@lastforumposterid,'0')

                                WHERE (CHARINDEX(',' + RTRIM([fid]) + ',', ',' +
					                                (SELECT @fidlist AS [fid]) + ',') > 0)


                    			
                                SELECT TOP 1 @pid = [pid], @tid = [tid],@posterid = [posterid], @postdatetime = [postdatetime], @title = [title], @poster = [poster] FROM [dnt_posts1] WHERE [posterid]=@posterid ORDER BY [pid] DESC
                    			
                                UPDATE [dnt_users] SET
                    			
	                                [lastpost] = @postdatetime,
	                                [lastpostid] = @pid,
	                                [lastposttitle] = ISNULL(@title,'')
                    				
                                WHERE [uid] = @posterid
                    			
                            END


                    ~


                    CREATE PROCEDURE dnt_getposttree
                    @tid int
                    AS
                    SELECT [pid], [layer], [title], [poster], [posterid],[postdatetime],[message] FROM [dnt_posts1] WHERE [tid]=@tid AND [invisible]=0 ORDER BY [parentid];


                    ~

                    CREATE PROCEDURE dnt_getsinglepost
                    @tid int,
                    @pid int
                    AS
                    SELECT [aid], [tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize], [attachment], [downloads] FROM [dnt_attachments] WHERE [tid]=@tid

                    SELECT TOP 1 
	                                [dnt_posts1].[pid], 
	                                [dnt_posts1].[fid], 
	                                [dnt_posts1].[title], 
	                                [dnt_posts1].[layer],
	                                [dnt_posts1].[message], 
	                                [dnt_posts1].[ip], 
	                                [dnt_posts1].[lastedit], 
	                                [dnt_posts1].[postdatetime], 
	                                [dnt_posts1].[attachment], 
	                                [dnt_posts1].[poster], 
	                                [dnt_posts1].[invisible], 
	                                [dnt_posts1].[usesig], 
	                                [dnt_posts1].[htmlon], 
	                                [dnt_posts1].[smileyoff], 
	                                [dnt_posts1].[parseurloff], 
	                                [dnt_posts1].[bbcodeoff], 
	                                [dnt_posts1].[rate], 
	                                [dnt_posts1].[ratetimes], 
	                                [dnt_posts1].[posterid], 
	                                [dnt_users].[nickname],  
	                                [dnt_users].[username], 
	                                [dnt_users].[groupid],
                                    [dnt_users].[spaceid],
                                    [dnt_users].[gender],
									[dnt_users].[bday], 
	                                [dnt_users].[email], 
	                                [dnt_users].[showemail], 
	                                [dnt_users].[digestposts], 
	                                [dnt_users].[credits], 
	                                [dnt_users].[extcredits1], 
	                                [dnt_users].[extcredits2], 
	                                [dnt_users].[extcredits3], 
	                                [dnt_users].[extcredits4], 
	                                [dnt_users].[extcredits5], 
	                                [dnt_users].[extcredits6], 
	                                [dnt_users].[extcredits7], 
	                                [dnt_users].[extcredits8], 
	                                [dnt_users].[posts], 
	                                [dnt_users].[joindate], 
	                                [dnt_users].[onlinestate], 
	                                [dnt_users].[lastactivity], 
	                                [dnt_users].[invisible], 
	                                [dnt_userfields].[avatar], 
	                                [dnt_userfields].[avatarwidth], 
	                                [dnt_userfields].[avatarheight], 
	                                [dnt_userfields].[medals], 
	                                [dnt_userfields].[sightml] AS signature, 
	                                [dnt_userfields].[location], 
	                                [dnt_userfields].[customstatus], 
	                                [dnt_userfields].[website], 
	                                [dnt_userfields].[icq], 
	                                [dnt_userfields].[qq], 
	                                [dnt_userfields].[msn], 
	                                [dnt_userfields].[yahoo], 
	                                [dnt_userfields].[skype] 
                    FROM [dnt_posts1] LEFT JOIN [dnt_users] ON [dnt_users].[uid]=[dnt_posts1].[posterid] LEFT JOIN [dnt_userfields] ON [dnt_userfields].[uid]=[dnt_users].[uid] WHERE [dnt_posts1].[pid]=@pid


                    ~

                    CREATE PROCEDURE dnt_updatepost
                        @pid int,
                        @title nvarchar(160),
                        @message ntext,
                        @lastedit nvarchar(50),
                        @invisible int,
                        @usesig int,
                        @htmlon int,
                        @smileyoff int,
                        @bbcodeoff int,
                        @parseurloff int
                    AS
                    UPDATE dnt_posts1 SET 
                        [title]=@title,
                        [message]=@message,
                        [lastedit]=@lastedit,
                        [invisible]=@invisible,
                        [usesig]=@usesig,
                        [htmlon]=@htmlon,
                        [smileyoff]=@smileyoff,
                        [bbcodeoff]=@bbcodeoff,
                        [parseurloff]=@parseurloff WHERE [pid]=@pid


                    ~

                    CREATE PROCEDURE dnt_getnewtopics 
                    @fidlist VARCHAR(500)
                    AS
                    IF @fidlist<>''
                    BEGIN
                    DECLARE @strSQL VARCHAR(5000)
                    SET @strSQL = 'SELECT TOP 20   [dnt_posts1].[tid], [dnt_posts1].[title], [dnt_posts1].[poster], [dnt_posts1].[postdatetime], [dnt_posts1].[message],[dnt_forums].[name] FROM [dnt_posts1]  LEFT JOIN [dnt_forums] ON [dnt_posts1].[fid]=[dnt_forums].[fid] WHERE  [dnt_forums].[fid] NOT IN ('+@fidlist +')  AND [dnt_posts1].[layer]=0 ORDER BY [dnt_posts1].[pid] DESC' 
                    END
                    ELSE
                    BEGIN
                    SET @strSQL = 'SELECT TOP 20   [dnt_posts1].[tid], [dnt_posts1].[title], [dnt_posts1].[poster], [dnt_posts1].[postdatetime], [dnt_posts1].[message],[dnt_forums].[name] FROM [dnt_posts1]  LEFT JOIN [dnt_forums] ON [dnt_posts1].[fid]=[dnt_forums].[fid] WHERE [dnt_posts1].[layer]=0 ORDER BY [dnt_posts1].[pid] DESC'
                    END
                    EXEC(@strSQL)

               ");

			sb.Replace("\"", "'").Replace("dnt_posts1", BaseConfigs.GetTablePrefix + "posts" + tablelistmaxid);
			sb.Replace("maxtablelistid", tablelistmaxid.ToString());
			sb.Replace("dnt_createpost", BaseConfigs.GetTablePrefix + "createpost" + tablelistmaxid);
			sb.Replace("dnt_getfirstpostid", BaseConfigs.GetTablePrefix + "getfirstpost" + tablelistmaxid + "id");
			sb.Replace("dnt_getpostcount", BaseConfigs.GetTablePrefix + "getpost" + tablelistmaxid + "count");
			sb.Replace("dnt_deletepostbypid", BaseConfigs.GetTablePrefix + "deletepost" + tablelistmaxid + "bypid");
			sb.Replace("dnt_getposttree", BaseConfigs.GetTablePrefix + "getpost" + tablelistmaxid + "tree");
			sb.Replace("dnt_getsinglepost", BaseConfigs.GetTablePrefix + "getsinglepost" + tablelistmaxid);
			sb.Replace("dnt_updatepost", BaseConfigs.GetTablePrefix + "updatepost" + tablelistmaxid);
			sb.Replace("dnt_getnewtopics", BaseConfigs.GetTablePrefix + "getnewtopics");
			sb.Replace("dnt_", BaseConfigs.GetTablePrefix);
            
			DatabaseProvider.GetInstance().CreatePostProcedure(sb.ToString());

			#endregion
		}

		public void UpdateMyTopic()
		{
			string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "mytopics]";//清空我的主题表
			DbHelper.ExecuteNonQuery(CommandType.Text, sql);
			//重建我的主题表
			sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "mytopics]([uid], [tid], [dateline]) SELECT [posterid],[tid],[postdatetime] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [posterid]<>-1";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql);
		}

		public void UpdateMyPost()
		{
			string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "myposts]";//清空我的帖子表
			DbHelper.ExecuteNonQuery(CommandType.Text, sql);
			//重建我的帖子表
            StringBuilder sqlstring = new StringBuilder();
            sqlstring.Append("DECLARE @tableid int\r\n");
            sqlstring.Append("DECLARE tables_cursor CURSOR FOR SELECT [id] FROM [" + BaseConfigs.GetTablePrefix + "tablelist]\r\n");
            sqlstring.Append("OPEN tables_cursor\r\n");
            sqlstring.Append("FETCH NEXT FROM tables_cursor INTO @tableid\r\n");
            sqlstring.Append("WHILE @@FETCH_STATUS = 0\r\n");
            sqlstring.Append("BEGIN\r\n");
            sqlstring.Append("DECLARE @sql varchar(500)\r\n");
            sqlstring.Append("SET @sql ='INSERT INTO [" + BaseConfigs.GetTablePrefix + "myposts]([uid], [tid], [pid], [dateline])\r\n");
            sqlstring.Append("SELECT [posterid],[tid],[pid],[postdatetime] FROM [" + BaseConfigs.GetTablePrefix + "posts' + LTRIM(STR(@tableid)) + '] WHERE [posterid]<>-1'\r\n");
            sqlstring.Append("EXEC(@sql)\r\n");
            sqlstring.Append("FETCH NEXT FROM tables_cursor INTO @tableid\r\n");
            sqlstring.Append("END\r\n");
            sqlstring.Append("CLOSE tables_cursor\r\n");
            sqlstring.Append("DEALLOCATE tables_cursor\r\n");

            
            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring.ToString());
		}

		public string GetAllIdentifySql()
		{
			return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topicidentify]";
		}

		public DataTable GetAllIdentify()
		{
			string sql = GetAllIdentifySql();
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public bool UpdateIdentifyById(int id, string name)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@identifyid", (DbType)SqlDbType.Int,4,id),
				DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar,50, name)
			};
			string sql = "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "topicidentify] WHERE [name]=@name AND [identifyid]<>@identifyid";
			if (int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql,prams).ToString()) != 0)  //有相同的名称存在，更新失败
			{
				return false;
			}
			sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "topicidentify] SET [name]=@name WHERE [identifyid]=@identifyid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
			return true;
		}

		public bool AddIdentify(string name ,string filename)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar,50, name),
				DbHelper.MakeInParam("@filename", (DbType)SqlDbType.VarChar,50,filename),
			};
			string sql = "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "topicidentify] WHERE [name]=@name";
			if (int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql,prams).ToString()) != 0)  //有相同的名称存在，插入失败
			{
				return false;
			}
			sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "topicidentify] ([name],[filename]) VALUES(@name,@filename)";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
			return true;
		}

		public void DeleteIdentify(string idlist)
		{
			string sql = "DELETE [" + BaseConfigs.GetTablePrefix + "topicidentify] WHERE [identifyid] IN (" + idlist + ")";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql);
		}

		/// <summary>
        /// 获取非默认模板数
        /// </summary>
        /// <returns></returns>
        public int GetSpecifyForumTemplateCount()
        {
            string sql = "SELECT COUNT(*) FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [templateid] <> 0 AND [templateid]<>" + GeneralConfigs.GetDefaultTemplateID();
            return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql).ToString());
        }

		#endregion

		#region GlobalManage

		private string GetSqlstringByPostDatetime(string sqlstring, DateTime postdatetimeStart, DateTime postdatetimeEnd)
		{
			//日期需要改成参数，以后需要重构！需要先修改后台传递参数方式
			if (postdatetimeStart.ToString() != "")
			{
				sqlstring += " AND [postdatetime]>='" + postdatetimeStart.ToString("yyyy-MM-dd HH:mm:ss") + "'";
			}

			if (postdatetimeEnd.ToString() != "")
			{
				sqlstring += " AND [postdatetime]<='" + postdatetimeEnd.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "'";
			}
			return sqlstring;
		}


		public DataTable GetAdsTable()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [advid], [type], [displayorder], [targets], [parameters], [code] FROM [" + BaseConfigs.GetTablePrefix + "advertisements] WHERE [available]=1 AND [starttime] <='" + DateTime.Now.ToShortDateString() + "' AND [endtime] >='" + DateTime.Now.ToShortDateString() + "' ORDER BY [displayorder] DESC, [advid] DESC").Tables[0];
		}

		/// <summary>
		/// 获得全部指定时间段内的公告列表
		/// </summary>
		/// <param name="starttime">开始时间</param>
		/// <param name="endtime">结束时间</param>
		/// <returns>公告列表</returns>
		public DataTable GetAnnouncementList(string starttime, string endtime)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@starttime",(DbType)SqlDbType.DateTime, 8, DateTime.Parse(starttime)),
									  DbHelper.MakeInParam("@endtime",(DbType)SqlDbType.DateTime, 8, DateTime.Parse(endtime))
								  };
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "announcements] WHERE [starttime] <=@starttime AND [endtime] >=@starttime ORDER BY [displayorder], [id] DESC", prams).Tables[0];
		}

		/// <summary>
		/// 获得全部指定时间段内的前n条公告列表
		/// </summary>
		/// <param name="starttime">开始时间</param>
		/// <param name="endtime">结束时间</param>
		/// <param name="maxcount">最大记录数,小于0返回全部</param>
		/// <returns>公告列表</returns>
		public DataTable GetSimplifiedAnnouncementList(string starttime, string endtime, int maxcount)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@starttime",(DbType)SqlDbType.DateTime, 8, DateTime.Parse(starttime)),
									  DbHelper.MakeInParam("@endtime",(DbType)SqlDbType.DateTime, 8, DateTime.Parse(endtime))
								  };
			string topstr = " TOP " + maxcount;
			if (maxcount < 0)
				topstr = "";
			string sqlstr = "SELECT" + topstr + " [id], [title], [poster], [posterid],[starttime] FROM [" + BaseConfigs.GetTablePrefix + "announcements] WHERE [starttime] <=@starttime AND [endtime] >=@starttime ORDER BY [displayorder], [id] DESC";

			return DbHelper.ExecuteDataset(CommandType.Text, sqlstr, prams).Tables[0];
		}


		public int AddAnnouncement(string poster, int posterid, string title, int displayorder, string starttime, string endtime, string message)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@poster", (DbType)SqlDbType.NVarChar, 20, poster),
									  DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.Int, 4, posterid),
									  DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 250, title),
									  DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder),
									  DbHelper.MakeInParam("@starttime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(starttime)),
									  DbHelper.MakeInParam("@endtime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(endtime)),
									  DbHelper.MakeInParam("@message", (DbType)SqlDbType.NText, 0, message)
								  };
			string sqlstring = "INSERT INTO  [" + BaseConfigs.GetTablePrefix + "announcements] ([poster],[posterid],[title],[displayorder],[starttime],[endtime],[message]) VALUES(@poster, @posterid, @title, @displayorder, @starttime, @endtime, @message)";
			//this.username,
			//this.userid,
			//title.Text,
			//displayorder.Text,
			//starttime.Text,
			//endtime.Text,
			//message.Text);
			return DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
		}

		public string GetAnnouncements()
		{
			return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "announcements] ORDER BY [id] ASC";
		}

		public void DeleteAnnouncements(string idlist)
		{
			string sqlstring = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "announcements]  WHERE [id] IN(" + idlist + ")";

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
		}

		public DataTable GetAnnouncement(int id)
		{
			IDataParameter parm = DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id);
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "announcements] WHERE [id]=@id", parm).Tables[0];
		}

		public void UpdateAnnouncement(int id, string poster, string title, int displayorder, string starttime, string endtime, string message)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id),
									  DbHelper.MakeInParam("@poster", (DbType)SqlDbType.NVarChar, 20, poster),
									  DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 250, title),
									  DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder),
									  DbHelper.MakeInParam("@starttime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(starttime)),
									  DbHelper.MakeInParam("@endtime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(endtime)),
									  DbHelper.MakeInParam("@message", (DbType)SqlDbType.NText, 0, message)
								  };
			string sqlstring = "UPDATE [" + BaseConfigs.GetTablePrefix + "announcements] SET [displayorder]=@displayorder,[title]=@title, [poster]=@poster,[starttime]=@starttime,[endtime]=@endtime,[message]=@message WHERE [id]=@id";

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
		}

		public void DeleteAnnouncement(int id)
		{
			IDataParameter parm = DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id);

			DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "announcements] WHERE [id]=@id", parm);
		}


		/// <summary>
		/// 获得公共可见板块列表
		/// </summary>
		/// <returns></returns>
		public IDataReader GetVisibleForumList()
		{
			return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT [name], [fid], [layer] FROM [{0}forums] WHERE [parentid] NOT IN (SELECT fid FROM [{0}forums] WHERE [status] < 1 AND [layer] = 0) AND [status] > 0 AND [displayorder] >=0 ORDER BY [displayorder]", BaseConfigs.GetTablePrefix));
		}

		public IDataReader GetOnlineGroupIconList()
		{
			return DbHelper.ExecuteReader(CommandType.Text, "SELECT [title], [img] FROM [" + BaseConfigs.GetTablePrefix + "onlinelist] WHERE [img]<> '' ORDER BY [displayorder]");
		}

		/// <summary>
		/// 获得友情链接列表
		/// </summary>
		/// <returns>友情链接列表</returns>
		public DataTable GetForumLinkList()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, @"SELECT [name],[url],[note],[displayorder]+10000 AS [displayorder],[logo] FROM [" + BaseConfigs.GetTablePrefix + @"forumlinks] WHERE [displayorder] > 0 AND [logo] = '' 
																	UNION SELECT [name],[url],[note],[displayorder],[logo] FROM [" + BaseConfigs.GetTablePrefix + @"forumlinks] WHERE [displayorder] > 0 AND [logo] <> '' ORDER BY [displayorder]").Tables[0];
		}

		/// <summary>
		/// 获得脏字过滤列表
		/// </summary>
		/// <returns></returns>
		public DataTable GetBanWordList()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [find], [replacement] FROM [" + BaseConfigs.GetTablePrefix + "words]").Tables[0];
		}

		/// <summary>
		/// 获得勋章列表
		/// </summary>
		/// <returns>获得勋章列表</returns>
		public DataTable GetMedalsList()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [medalid], [name], [image],[available]  FROM [" + BaseConfigs.GetTablePrefix + "medals] ORDER BY [medalid]").Tables[0];
		}

		/// <summary>
		/// 获得魔法表情列表
		/// </summary>
		/// <returns>魔法表情列表</returns>
		public DataTable GetMagicList()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "magic] ORDER BY [id]").Tables[0];
		}

		/// <summary>
		/// 获得主题类型列表
		/// </summary>
		/// <returns></returns>
		public DataTable GetTopicTypeList()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [typeid],[name] FROM [" + BaseConfigs.GetTablePrefix + "topictypes] ORDER BY [displayorder]").Tables[0];
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
		public int AddCreditsLog(int uid, int fromto, int sendcredits, int receivecredits, float send, float receive, string paydate, int operation)
		{

			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									  DbHelper.MakeInParam("@fromto",(DbType)SqlDbType.Int,4,fromto),
									  DbHelper.MakeInParam("@sendcredits",(DbType)SqlDbType.Int,4,sendcredits),
									  DbHelper.MakeInParam("@receivecredits",(DbType)SqlDbType.Int,4,receivecredits),
									  DbHelper.MakeInParam("@send",(DbType)SqlDbType.Float,4,send),
									  DbHelper.MakeInParam("@receive",(DbType)SqlDbType.Float,4,receive),
									  DbHelper.MakeInParam("@paydate",(DbType)SqlDbType.VarChar,0,paydate),
									  DbHelper.MakeInParam("@operation",(DbType)SqlDbType.Int,4,operation)
								  };

			return DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "creditslog] ([uid],[fromto],[sendcredits],[receivecredits],[send],[receive],[paydate],[operation]) VALUES(@uid,@fromto,@sendcredits,@receivecredits,@send,@receive,@paydate,@operation)", prams);

		}

		/// <summary>
		/// 返回指定范围的积分日志
		/// </summary>
		/// <param name="pagesize">页大小</param>
		/// <param name="currentpage">当前页数</param>
		/// <param name="uid">用户id</param>
		/// <returns>积分日志</returns>
		public DataTable GetCreditsLogList(int pagesize, int currentpage, int uid)
		{
			int pagetop = (currentpage - 1) * pagesize;
			string sqlstring;
			if (currentpage == 1)
			{
				//select c.*,ufrom.username as fromuser ,uto.username as touser from dnt_creditslog c,dnt_users ufrom, dnt_users uto where c.uid=ufrom.uid AND c.fromto=uto.uid
				// AND (c.uid=1 or c.fromto =1)
				sqlstring = string.Format("SELECT TOP {0} [c].*, [ufrom].[username] AS [fromuser], [uto].[username] AS [touser] FROM [{1}creditslog] AS [c], [{1}users] AS [ufrom], [{1}users] AS [uto] WHERE [c].[uid]=[ufrom].[uid] AND [c].[fromto]=[uto].[uid] AND ([c].[uid]={2} OR [c].[fromto] = {2})  ORDER BY [id] DESC", pagesize, BaseConfigs.GetTablePrefix, uid);
			}
			else
			{
				sqlstring = string.Format("SELECT TOP {0} [c].*, [ufrom].[username] AS [fromuser], [uto].[username] AS [touser] FROM [{1}creditslog] AS [c], [{1}users] AS [ufrom], [{1}users] AS [uto] WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP {2} [id] FROM [{1}creditslog] WHERE [{1}creditslog].[uid]={3}  OR [{1}creditslog].[fromto]={3} ORDER BY [id] DESC) AS tblTmp ) AND [c].[uid]=[ufrom].[uid] AND [c].[fromto]=[uto].[uid] AND ([c].[uid]={3} OR [c].[fromto] = {3}) ORDER BY [c].[id] DESC", pagesize, BaseConfigs.GetTablePrefix, pagetop, uid);
			}

			return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
		}

		/// <summary>
		/// 获得指定用户的积分交易历史记录总条数
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <returns>历史记录总条数</returns>
		public int GetCreditsLogRecordCount(int uid)
		{
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM [{0}creditslog] WHERE [uid]={1} OR [fromto]={1}", BaseConfigs.GetTablePrefix, uid)), 0);
		}


		public string GetTableStruct()
		{
			#region 数据表查询语句

			string SqlString = null;
			SqlString = "SELECT 表名=case when a.colorder=1 then d.name else '' end,";
			SqlString += "表说明=case when a.colorder=1 then isnull(f.value,'') else '' end,";
			SqlString += " 字段序号=a.colorder,";
			SqlString += " 字段名=a.name,";
			SqlString += " 标识=case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '√'else '' end,";
			SqlString += " 主键=case when exists(SELECT 1 FROM sysobjects where xtype='PK' and name in (";
			SqlString += " SELECT name FROM sysindexes WHERE indid in(";
			SqlString += "   SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid";
			SqlString += "  ))) then '√' else '' end,";
			SqlString += " 类型=b.name,";
			SqlString += " 占用字节数=a.length,";
			SqlString += " 长度=COLUMNPROPERTY(a.id,a.name,'PRECISION'),";
			SqlString += " 小数位数=isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),";
			SqlString += " 允许空=case when a.isnullable=1 then '√'else '' end,";
			SqlString += " 默认值=isnull(e.text,''),";
			SqlString += " 字段说明=isnull(g.[value],'')";
			SqlString += "FROM syscolumns a";
			SqlString += " left join systypes b on a.xtype=b.xusertype";
			SqlString += " inner join sysobjects d on a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'";
			SqlString += " left join syscomments e on a.cdefault=e.id";
			SqlString += " left join sysproperties g on a.id=g.id and a.colid=g.smallid  ";
			SqlString += " left join sysproperties f on d.id=f.id and f.smallid=0";
			//SqlString+="--where d.name='要查询的表'    --如果只查询指定表,加上此条件";
			SqlString += " order by a.id,a.colorder";
			return SqlString;

			#endregion
		}

		public void ShrinkDataBase(string shrinksize, string dbname)
		{
			string SqlString = null;

			SqlString += "SET NOCOUNT ON ";

			SqlString += "DECLARE @LogicalFileName sysname, @MaxMinutes INT, @NewSize INT ";
			SqlString += "USE [" + dbname + "] -- 要操作的数据库名 ";
			SqlString += "SELECT @LogicalFileName = '" + dbname + "_log', -- 日志文件名 ";
			SqlString += "@MaxMinutes = 10, -- Limit on time allowed to wrap log. ";
			SqlString += "@NewSize = 1 -- 你想设定的日志文件的大小(M) ";
			SqlString += "-- Setup / initialize ";
			SqlString += "DECLARE @OriginalSize int ";
			SqlString += "SELECT @OriginalSize = " + shrinksize;
			SqlString += "FROM sysfiles ";
			SqlString += "WHERE name = @LogicalFileName ";
			SqlString += "SELECT 'Original Size of ' + db_name() + ' LOG is ' + ";
			SqlString += "CONVERT(VARCHAR(30),@OriginalSize) + ' 8K pages or ' + ";
			SqlString += "CONVERT(VARCHAR(30),(@OriginalSize*8/1024)) + 'MB' ";
			SqlString += "FROM sysfiles ";
			SqlString += "WHERE name = @LogicalFileName ";
			SqlString += "CREATE TABLE DummyTrans ";
			SqlString += "(DummyColumn char (8000) not null) ";
			SqlString += "DECLARE @Counter INT, ";
			SqlString += "@StartTime DATETIME, ";
			SqlString += "@TruncLog VARCHAR(255) ";
			SqlString += "SELECT @StartTime = GETDATE(), ";
			SqlString += "@TruncLog = 'BACKUP LOG ' + db_name() + ' WITH TRUNCATE_ONLY' ";
			SqlString += "DBCC SHRINKFILE (@LogicalFileName, @NewSize) ";
			SqlString += "EXEC (@TruncLog) ";
			SqlString += "-- Wrap the log if necessary. ";
			SqlString += "WHILE @MaxMinutes > DATEDIFF (mi, @StartTime, GETDATE()) -- time has not expired ";
			SqlString += "AND @OriginalSize = (SELECT size FROM sysfiles WHERE name = @LogicalFileName) ";
			SqlString += "AND (@OriginalSize * 8 /1024) > @NewSize ";
			SqlString += "BEGIN -- Outer loop. ";
			SqlString += "SELECT @Counter = 0 ";
			SqlString += "WHILE ((@Counter < @OriginalSize / 16) AND (@Counter < 50000)) ";
			SqlString += "BEGIN -- update ";
			SqlString += "INSERT DummyTrans VALUES ('Fill Log') ";
			SqlString += "DELETE DummyTrans ";
			SqlString += "SELECT @Counter = @Counter + 1 ";
			SqlString += "END ";
			SqlString += "EXEC (@TruncLog) ";
			SqlString += "END ";
			SqlString += "SELECT 'Final Size of ' + db_name() + ' LOG is ' + ";
			SqlString += "CONVERT(VARCHAR(30),size) + ' 8K pages or ' + ";
			SqlString += "CONVERT(VARCHAR(30),(size*8/1024)) + 'MB' ";
			SqlString += "FROM sysfiles ";
			SqlString += "WHERE name = @LogicalFileName ";
			SqlString += "DROP TABLE DummyTrans ";
			SqlString += "SET NOCOUNT OFF ";

			DbHelper.ExecuteDataset(CommandType.Text, SqlString);
		}

		public void ClearDBLog(string dbname)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@DBName", (DbType)SqlDbType.VarChar, 50, dbname),
			};
			DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "" + BaseConfigs.GetTablePrefix + "shrinklog", prams);
		}

		public string RunSql(string sql)
		{
			string errorInfo = string.Empty;
			if (sql != "")
			{
				SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
				conn.Open();
				foreach (string sqlStr in Utils.SplitString(sql, "--/* Discuz!NT SQL Separator */--"))
				{
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
							errorInfo += message + "<br>";
						}
					}
				}
				conn.Close();
			}
			return errorInfo;
		}

		//得到数据库的名称
		public string GetDbName()
		{
			string connectionString = BaseConfigs.GetDBConnectString;
			foreach (string info in connectionString.Split(';'))
			{
				if (info.ToLower().IndexOf("initial catalog") >= 0 || info.ToLower().IndexOf("database") >= 0)
				{
					return info.Split('=')[1].Trim();
				}
			}
			return "dnt";
		}


		/// <summary>
		/// 创建并填充指定帖子分表id全文索引
		/// </summary>
		/// <param name="DbName">数据库名称</param>
		/// <param name="postsid">当前帖子表的id</param>
		/// <returns></returns>
		public bool CreateORFillIndex(string DbName, string postsid)
		{
			StringBuilder sb = new StringBuilder();

			string currenttablename = BaseConfigs.GetTablePrefix + "posts" + postsid;

			try
			{
				//如果有全文索引则进行填充,如果没有就抛出异常
				sb.Remove(0, sb.Length);
				DbHelper.ExecuteNonQuery("SELECT TOP 1 [pid] FROM [" + currenttablename + "] WHERE CONTAINS([message],'asd') ORDER BY [pid] ASC");

				sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + currenttablename + "_msg','start_full'; \r\n");
				sb.Append("WHILE fulltextcatalogproperty('pk_" + currenttablename + "_msg','populateStatus')<>0 \r\n");
				sb.Append("BEGIN \r\n");
				sb.Append("WAITFOR DELAY '0:5:30' \r\n");
				sb.Append("END \r\n");
				DbHelper.ExecuteNonQuery(sb.ToString());

				return true;
			}
			catch
			{
				try
				{
					#region 构建全文索引

					sb.Remove(0, sb.Length);
					sb.Append("IF(SELECT DATABASEPROPERTY('[" + DbName + "]','isfulltextenabled'))=0  EXECUTE sp_fulltext_database 'enable';");

					try
					{ //此处删除以确保全文索引目录和系统表中的数据同步
						sb.Append("EXECUTE sp_fulltext_table '[" + currenttablename + "]', 'drop' ;");
						sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + currenttablename + "_msg','drop';");
						DbHelper.ExecuteNonQuery(sb.ToString());
					}
					catch
					{
						;
					}
					finally
					{
						//执行全文填充语句
						sb.Remove(0, sb.Length);
						sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + currenttablename + "_msg','create';");
						sb.Append("EXECUTE sp_fulltext_table '[" + currenttablename + "]','create','pk_" + currenttablename + "_msg','pk_" + currenttablename + "';");
						sb.Append("EXECUTE sp_fulltext_column '[" + currenttablename + "]','message','add';");
						sb.Append("EXECUTE sp_fulltext_table '[" + currenttablename + "]','activate';");
						sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + currenttablename + "_msg','start_full';");
						DbHelper.ExecuteNonQuery(sb.ToString());
					}
					return true;

					#endregion
				}
				catch (SqlException ex)
				{
					string message = ex.Message.Replace("'", " ");
					message = message.Replace("\\", "/");
					message = message.Replace("\r\n", "\\r\\n");
					message = message.Replace("\r", "\\r");
					message = message.Replace("\n", "\\n");
					return true;
				}
			}
		}


		/// <summary>
		/// 得到指定帖子分表的全文索引建立(填充)语句
		/// </summary>
		/// <param name="tablename"></param>
		/// <returns></returns>
		public string GetSpecialTableFullIndexSQL(string tablename)
		{
			#region 建表

			StringBuilder sb = new StringBuilder();
			sb.Append("IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[" + tablename + "]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)  DROP TABLE [" + tablename + "];");
			sb.Append("CREATE TABLE [" + tablename + "] ([pid] [int] NOT NULL ,[fid] [int] NOT NULL ," +
				"[tid] [int] NOT NULL ,[parentid] [int] NOT NULL ,[layer] [int] NOT NULL ,[poster] [nvarchar] (20) NOT NULL ," +
				"[posterid] [int] NOT NULL ,[title] [nvarchar] (80) NOT NULL ,[postdatetime] [smalldatetime] NOT NULL ," +
				"[message] [ntext] NOT NULL ,[ip] [nvarchar] (15) NOT NULL ," +
				"[lastedit] [nvarchar] (50) NOT NULL ,[invisible] [int] NOT NULL ,[usesig] [int] NOT NULL ,[htmlon] [int] NOT NULL ," +
				"[smileyoff] [int] NOT NULL ,[parseurloff] [int] NOT NULL ,[bbcodeoff] [int] NOT NULL ,[attachment] [int] NOT NULL ,[rate] [int] NOT NULL ," +
				"[ratetimes] [int] NOT NULL ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");
			sb.Append(";");
			sb.Append("ALTER TABLE [" + tablename + "] WITH NOCHECK ADD CONSTRAINT [PK_" + tablename + "] PRIMARY KEY  CLUSTERED ([pid])  ON [PRIMARY]");
			sb.Append(";");

			sb.Append("ALTER TABLE [" + tablename + "] ADD ");
			sb.Append("CONSTRAINT [DF_" + tablename + "_pid] DEFAULT (0) FOR [pid],");
			sb.Append("CONSTRAINT [DF_" + tablename + "_parentid] DEFAULT (0) FOR [parentid],CONSTRAINT [DF_" + tablename + "_layer] DEFAULT (0) FOR [layer],");
			sb.Append("CONSTRAINT [DF_" + tablename + "_poster] DEFAULT ('') FOR [poster],CONSTRAINT [DF_" + tablename + "_posterid] DEFAULT (0) FOR [posterid],");
			sb.Append("CONSTRAINT [DF_" + tablename + "_postdatetime] DEFAULT (getdate()) FOR [postdatetime],CONSTRAINT [DF_" + tablename + "_message] DEFAULT ('') FOR [message],");
			sb.Append("CONSTRAINT [DF_" + tablename + "_ip] DEFAULT ('') FOR [ip],CONSTRAINT [DF_" + tablename + "_lastedit] DEFAULT ('') FOR [lastedit],");
			sb.Append("CONSTRAINT [DF_" + tablename + "_invisible] DEFAULT (0) FOR [invisible],CONSTRAINT [DF_" + tablename + "_usesig] DEFAULT (0) FOR [usesig],");
			sb.Append("CONSTRAINT [DF_" + tablename + "_htmlon] DEFAULT (0) FOR [htmlon],CONSTRAINT [DF_" + tablename + "_smileyoff] DEFAULT (0) FOR [smileyoff],");
			sb.Append("CONSTRAINT [DF_" + tablename + "_parseurloff] DEFAULT (0) FOR [parseurloff],CONSTRAINT [DF_" + tablename + "_bbcodeoff] DEFAULT (0) FOR [bbcodeoff],");
			sb.Append("CONSTRAINT [DF_" + tablename + "_attachment] DEFAULT (0) FOR [attachment],CONSTRAINT [DF_" + tablename + "_rate] DEFAULT (0) FOR [rate],");
			sb.Append("CONSTRAINT [DF_" + tablename + "_ratetimes] DEFAULT (0) FOR [ratetimes]");

			sb.Append(";");
			sb.Append("CREATE  INDEX [parentid] ON [" + tablename + "]([parentid]) ON [PRIMARY]");
			sb.Append(";");

			sb.Append("CREATE  UNIQUE  INDEX [showtopic] ON [" + tablename + "]([tid], [invisible], [pid]) ON [PRIMARY]");
			sb.Append(";");


			sb.Append("CREATE  INDEX [treelist] ON [" + tablename + "]([tid], [invisible], [parentid]) ON [PRIMARY]");
			sb.Append(";");

			#endregion

			#region 建全文索引

			sb.Append("USE " + GetDbName() + " \r\n");
			sb.Append("EXECUTE sp_fulltext_database 'enable'; \r\n");
			sb.Append("IF(SELECT DATABASEPROPERTY('[" + GetDbName() + "]','isfulltextenabled'))=0  EXECUTE sp_fulltext_database 'enable';");
			sb.Append("IF EXISTS (SELECT * FROM sysfulltextcatalogs WHERE name ='pk_" + tablename + "_msg')  EXECUTE sp_fulltext_catalog 'pk_" + tablename + "_msg','drop';");
			sb.Append("IF EXISTS (SELECT * FROM sysfulltextcatalogs WHERE name ='pk_" + tablename + "_msg')  EXECUTE sp_fulltext_table '[" + tablename + "]', 'drop' ;");
			sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + tablename + "_msg','create';");
			sb.Append("EXECUTE sp_fulltext_table '[" + tablename + "]','create','pk_" + tablename + "_msg','pk_" + tablename + "';");
			sb.Append("EXECUTE sp_fulltext_column '[" + tablename + "]','message','add';");
			sb.Append("EXECUTE sp_fulltext_table '[" + tablename + "]','activate';");
			sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + tablename + "_msg','start_full';");

			#endregion

			return sb.ToString();
		}


		/// <summary>
		/// 以DataReader返回自定义编辑器按钮列表
		/// </summary>
		/// <returns></returns>
		public IDataReader GetCustomEditButtonList()
		{
			return DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "bbcodes] WHERE [available] = 1");
		}

		/// <summary>
		/// 以DataTable返回自定义按钮列表
		/// </summary>
		/// <returns></returns>
		public DataTable GetCustomEditButtonListWithTable()
		{
			DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "bbcodes] WHERE [available] = 1");
			if (ds != null)
			{
				if (ds.Tables.Count > 0)
				{
					return ds.Tables[0];
				}
			}
			ds.Dispose();
			return null;
		}

		public DataRowCollection GetTableListIds()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [id] FROM [" + BaseConfigs.GetTablePrefix + "tablelist]").Tables[0].Rows;
		}



		public void UpdateAnnouncementPoster(int posterid, string poster)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.Int, 4, posterid),
									  DbHelper.MakeInParam("@poster", (DbType)SqlDbType.VarChar, 20, poster)
								  };

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "announcements] SET [poster]=@poster WHERE [posterid]=@posterid", parms);
		}

		public bool HasStatisticsByLastUserId(int lastuserid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@lastuserid", (DbType)SqlDbType.Int, 4, lastuserid);
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [lastuserid] FROM  [" + BaseConfigs.GetTablePrefix + "statistics]  WHERE [lastuserid]=@lastuserid",parm).Tables[0].Rows.Count > 0;
		}

		public void UpdateStatisticsLastUserName(int lastuserid, string lastusername)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@lastuserid", (DbType)SqlDbType.Int, 4, lastuserid),
									  DbHelper.MakeInParam("@lastusername", (DbType)SqlDbType.VarChar, 20, lastusername)
								  };

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [lastusername]=@lastusername WHERE [lastuserid]=@lastuserid", parms);
		}

		public void AddVisitLog(int uid, string username, int groupid, string grouptitle, string ip, string actions, string others)
		{
			string sqlstring = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "adminvisitlog] ([uid],[username],[groupid],[grouptitle],[ip],[actions],[others]) VALUES (@uid,@username,@groupid,@grouptitle,@ip,@actions,@others)";

			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
									  DbHelper.MakeInParam("@username", (DbType)SqlDbType.VarChar, 50, username),
									  DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupid),
									  DbHelper.MakeInParam("@grouptitle", (DbType)SqlDbType.VarChar, 50, grouptitle),
									  DbHelper.MakeInParam("@ip", (DbType)SqlDbType.VarChar, 15, ip),
									  DbHelper.MakeInParam("@actions", (DbType)SqlDbType.VarChar, 100, actions),
									  DbHelper.MakeInParam("@others", (DbType)SqlDbType.VarChar, 200, others)
								  };

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);
		}

		public void DeleteVisitLogs()
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] ");
		}

		public void DeleteVisitLogs(string condition)
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] WHERE " + condition);
		}

		/// <summary>
		/// 得到当前指定页数的后台访问日志记录(表)
		/// </summary>
		/// <param name="pagesize">当前分页的尺寸大小</param>
		/// <param name="currentpage">当前页码</param>
		/// <returns></returns>
		public DataTable GetVisitLogList(int pagesize, int currentpage)
		{
			int pagetop = (currentpage - 1) * pagesize;

			if (currentpage == 1)
			{
				return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] ORDER BY [visitid] DESC").Tables[0];
			}
			else
			{
				string sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog]  WHERE [visitid] < (SELECT MIN([visitid]) FROM (SELECT TOP " + pagetop + " [visitid] FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] ORDER BY [visitid] DESC) AS tblTmp )  ORDER BY [visitid] DESC";
				return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
			}
		}

		/// <summary>
		/// 得到当前指定条件和页数的后台访问日志记录(表)
		/// </summary>
		/// <param name="pagesize">当前分页的尺寸大小</param>
		/// <param name="currentpage">当前页码</param>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public DataTable GetVisitLogList(int pagesize, int currentpage, string condition)
		{
			int pagetop = (currentpage - 1) * pagesize;

			if (currentpage == 1)
			{
				return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] WHERE " + condition + " ORDER BY [visitid] DESC").Tables[0];
			}
			else
			{
				string sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog]  WHERE [visitid] < (SELECT MIN([visitid])  FROM (SELECT TOP " + pagetop + " [visitid] FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] WHERE " + condition + " ORDER BY [visitid] DESC) AS tblTmp ) AND " + condition + " ORDER BY [visitid] DESC";
				return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
			}
		}

		public int GetVisitLogCount()
		{
			return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT(visitid) FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog]").Tables[0].Rows[0][0].ToString());
		}

		public int GetVisitLogCount(string condition)
		{
			return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT(visitid) FROM [" + BaseConfigs.GetTablePrefix + "adminvisitlog] WHERE " + condition).Tables[0].Rows[0][0].ToString());
		}

		public void UpdateForumAndUserTemplateId(string templateidlist)
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [templateid]=1 WHERE [templateid] IN(" + templateidlist + ")");
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [templateid]=1 WHERE [templateid] IN(" + templateidlist + ")");
		}

		public void AddTemplate(string name, string directory, string copyright, string author, string createdate, string ver, string fordntver)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar, 50, name),
									  DbHelper.MakeInParam("@directory", (DbType)SqlDbType.NVarChar, 100, directory),
									  DbHelper.MakeInParam("@copyright", (DbType)SqlDbType.NVarChar, 100, copyright),
									  DbHelper.MakeInParam("@author", (DbType)SqlDbType.NVarChar, 100, author),
									  DbHelper.MakeInParam("@createdate", (DbType)SqlDbType.NVarChar, 50, createdate),
									  DbHelper.MakeInParam("@ver", (DbType)SqlDbType.NVarChar, 100, ver),
									  DbHelper.MakeInParam("@fordntver", (DbType)SqlDbType.NVarChar, 100, fordntver)
								  };
			string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "templates] ([name],[directory],[copyright],[author],[createdate],[ver],[fordntver]) VALUES(@name,@directory,@copyright,@author,@createdate,@ver,@fordntver)";

			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		/// <summary>
		/// 添加新的模板项
		/// </summary>
		/// <param name="templateName">模板名称</param>
		/// <param name="directory">模板文件所在目录</param>
		/// <param name="copyright">模板版权文字</param>
		/// <returns>模板id</returns>
		public int AddTemplate(string templateName, string directory, string copyright)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@templatename", (DbType)SqlDbType.VarChar, 0, templateName),
									  DbHelper.MakeInParam("@directory", (DbType)SqlDbType.VarChar, 0, directory),
									  DbHelper.MakeInParam("@copyright", (DbType)SqlDbType.VarChar, 0, copyright),

			};
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "templates]([templatename],[directory],[copyright]) VALUES(@templatename, @directory, @copyright);SELECT SCOPE_IDENTITY()", prams), -1);
		}

		/// <summary>
		/// 删除指定的模板项
		/// </summary>
		/// <param name="templateid">模板id</param>
		public void DeleteTemplateItem(int templateid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@templateid", (DbType)SqlDbType.Int, 4, templateid)
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "templates] WHERE [templateid]=@templateid");
		}

		/// <summary>
		/// 删除指定的模板项列表,
		/// </summary>
		/// <param name="templateidlist">格式为： 1,2,3</param>
		public void DeleteTemplateItem(string templateidlist)
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "templates] WHERE [templateid] IN (" + templateidlist + ")");
		}

		/// <summary>
		/// 获得所有在模板目录下的模板列表(即:子目录名称)
		/// </summary>
		/// <param name="templatePath">模板所在路径</param>
		/// <example>GetAllTemplateList(Utils.GetMapPath(@"..\..\templates\"))</example>
		/// <returns>模板列表</returns>
		public DataTable GetAllTemplateList(string templatePath)
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "templates] ORDER BY [templateid]").Tables[0];
		}


		public int GetMaxTemplateId()
		{
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX(templateid), 0) FROM " + BaseConfigs.GetTablePrefix + "templates"), 0);
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
		public bool InsertModeratorLog(string moderatoruid, string moderatorname, string groupid, string grouptitle, string ip, string postdatetime, string fid, string fname, string tid, string title, string actions, string reason)
		{
			try
			{
				IDataParameter[] parms = { 
										  DbHelper.MakeInParam("@moderatoruid", (DbType)SqlDbType.Int, 4, moderatoruid),
										  DbHelper.MakeInParam("@moderatorname", (DbType)SqlDbType.NVarChar, 50, moderatorname),
										  DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupid),
										  DbHelper.MakeInParam("@grouptitle", (DbType)SqlDbType.NVarChar, 50, grouptitle),
										  DbHelper.MakeInParam("@ip", (DbType)SqlDbType.VarChar, 15, ip),
										  DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(postdatetime)),
										  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid),
										  DbHelper.MakeInParam("@fname", (DbType)SqlDbType.NVarChar, 100, fname),
										  DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 8, tid),
										  DbHelper.MakeInParam("@title", (DbType)SqlDbType.VarChar, 200, title),
										  DbHelper.MakeInParam("@actions", (DbType)SqlDbType.VarChar, 50, actions),
										  DbHelper.MakeInParam("@reason", (DbType)SqlDbType.NVarChar, 200, reason)
									  };

				string sqlstring = "INSERT INTO [" + BaseConfigs.GetTablePrefix +
					"moderatormanagelog] ([moderatoruid],[moderatorname],[groupid],[grouptitle],[ip],[postdatetime],[fid],[fname],[tid],[title],[actions],[reason]) VALUES (@moderatoruid, @moderatorname, @groupid, @grouptitle,@ip,@postdatetime,@fid,@fname,@tid,@title,@actions,@reason)";
				DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
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
				DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog] WHERE " + condition);
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
		public DataTable GetModeratorLogList(int pagesize, int currentpage, string condition)
		{
			int pagetop = (currentpage - 1) * pagesize;
			string sqlstring;

			if (condition != "") condition = " WHERE " + condition;

			if (currentpage == 1)
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog]  " + condition + "  ORDER BY [id] DESC";
			}
			else
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog]  WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog]  " + condition + " ORDER BY [id] DESC) AS tblTmp ) " + (condition.Replace("WHERE", "") == "" ? "" : "AND " + condition.Replace("WHERE", "")) + " ORDER BY [id] DESC";
			}

			return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
		}

		/// <summary>
		/// 得到前台管理日志记录数
		/// </summary>
		/// <returns></returns>
		public int GetModeratorLogListCount()
		{
			return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog]").Tables[0].Rows[0][0].ToString());
		}

		/// <summary>
		/// 得到指定查询条件下的前台管理日志数
		/// </summary>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public int GetModeratorLogListCount(string condition)
		{
			return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog] WHERE " + condition).Tables[0].Rows[0][0].ToString());
		}

		/// <summary>
		/// 删除日志
		/// </summary>
		/// <returns></returns>
		public bool DeleteMedalLog()
		{
			try
			{
				DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "medalslog] ");
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
				DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "medalslog] WHERE " + condition);
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
		public DataTable GetMedalLogList(int pagesize, int currentpage)
		{
			int pagetop = (currentpage - 1) * pagesize;
			string sqlstring;
			if (currentpage == 1)
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "medalslog] ORDER BY [id] DESC";
			}
			else
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "medalslog]  WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "medalslog] ORDER BY [id] DESC) AS tblTmp )  ORDER BY [id] DESC";
			}

			return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
		}

		/// <summary>
		/// 得到当前指定条件和页数的勋章日志记录(表)
		/// </summary>
		/// <param name="pagesize">当前分页的尺寸大小</param>
		/// <param name="currentpage">当前页码</param>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public DataTable GetMedalLogList(int pagesize, int currentpage, string condition)
		{
			int pagetop = (currentpage - 1) * pagesize;
			string sqlstring;
			if (currentpage == 1)
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "medalslog] WHERE " + condition + " ORDER BY [id] DESC";
			}
			else
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "medalslog]  WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "medalslog] WHERE " + condition + " ORDER BY [id] DESC) AS tblTmp ) AND " + condition + " ORDER BY [id] DESC";
			}

			return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
		}

		/// <summary>
		/// 得到缓存日志记录数
		/// </summary>
		/// <returns></returns>
		public int GetMedalLogListCount()
		{
			return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "medalslog]").Tables[0].Rows[0][0].ToString());
		}

		/// <summary>
		/// 得到指定查询条件下的勋章日志数
		/// </summary>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public int GetMedalLogListCount(string condition)
		{
			return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "medalslog] WHERE " + condition).Tables[0].Rows[0][0].ToString());
		}


		/// <summary>
		/// 根据IP获取错误登录记录
		/// </summary>
		/// <param name="ip"></param>
		/// <returns></returns>
		public DataTable GetErrLoginRecordByIP(string ip)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@ip",(DbType)SqlDbType.Char,15, ip),
			};
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [errcount], [lastupdate] FROM [" + BaseConfigs.GetTablePrefix + "failedlogins] WHERE [ip]=@ip", prams).Tables[0];
		}

		/// <summary>
		/// 增加指定IP的错误记录数
		/// </summary>
		/// <param name="ip"></param>
		/// <returns></returns>
		public int AddErrLoginCount(string ip)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@ip",(DbType)SqlDbType.Char,15, ip),
			};
			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "failedlogins] SET [errcount]=[errcount]+1, [lastupdate]=GETDATE() WHERE [ip]=@ip", prams);
		}

		/// <summary>
		/// 增加指定IP的错误记录
		/// </summary>
		/// <param name="ip"></param>
		/// <returns></returns>
		public int AddErrLoginRecord(string ip)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@ip",(DbType)SqlDbType.Char,15, ip),
			};
			return DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "failedlogins] ([ip], [errcount], [lastupdate]) VALUES(@ip, 1, GETDATE())", prams);
		}

		/// <summary>
		/// 将指定IP的错误登录次数重置为1
		/// </summary>
		/// <param name="ip"></param>
		/// <returns></returns>
		public int ResetErrLoginCount(string ip)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@ip",(DbType)SqlDbType.Char,15, ip),
			};
			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "failedlogins] SET [errcount]=1, [lastupdate]=GETDATE() WHERE [ip]=@ip", prams);
		}

		/// <summary>
		/// 删除指定IP或者超过15天的记录
		/// </summary>
		/// <param name="ip"></param>
		/// <returns></returns>
		public int DeleteErrLoginRecord(string ip)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@ip",(DbType)SqlDbType.Char,15, ip),
			};
			return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "failedlogins] WHERE [ip]=@ip OR DATEDIFF(n,[lastupdate], GETDATE()) > 15", prams);
		}

		public int GetPostCount(string posttablename)
		{
			return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([pid]) FROM [" + posttablename + "]").Tables[0].Rows[0][0].ToString());
		}

		public DataTable GetPostTableList()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "tablelist]").Tables[0];
		}

		public int UpdateDetachTable(int fid, string description)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid),
									  DbHelper.MakeInParam("@description", (DbType)SqlDbType.NVarChar, 50, description)
								  };
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "tablelist] SET [description]=@description  Where [id]=@fid";
			//fid, description);
			return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public int StartFullIndex(string dbname)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("USE " + dbname + ";");
			sb.Append("EXECUTE sp_fulltext_database 'enable';");
			return DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
		}
		public void CreatePostTableAndIndex(string tablename)
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, GetSpecialTableFullIndexSQL(tablename, GetDbName()));
		}


		/// <summary>
		/// 得到指定帖子分表的全文索引建立(填充)语句
		/// </summary>
		/// <param name="tablename"></param>
		/// <returns></returns>
		public string GetSpecialTableFullIndexSQL(string tablename, string dbname)
		{
			#region 建表

			StringBuilder sb = new StringBuilder();
			sb.Append("IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[" + tablename + "]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)  DROP TABLE [" + tablename + "];");
			sb.Append("CREATE TABLE [" + tablename + "] ([pid] [int] NOT NULL ,[fid] [int] NOT NULL ," +
				"[tid] [int] NOT NULL ,[parentid] [int] NOT NULL ,[layer] [int] NOT NULL ,[poster] [nvarchar] (20) NOT NULL ," +
				"[posterid] [int] NOT NULL ,[title] [nvarchar] (80) NOT NULL ,[postdatetime] [smalldatetime] NOT NULL ," +
				"[message] [ntext] NOT NULL ,[ip] [nvarchar] (15) NOT NULL ," +
				"[lastedit] [nvarchar] (50) NOT NULL ,[invisible] [int] NOT NULL ,[usesig] [int] NOT NULL ,[htmlon] [int] NOT NULL ," +
				"[smileyoff] [int] NOT NULL ,[parseurloff] [int] NOT NULL ,[bbcodeoff] [int] NOT NULL ,[attachment] [int] NOT NULL ,[rate] [int] NOT NULL ," +
				"[ratetimes] [int] NOT NULL ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");
			sb.Append(";");
			sb.Append("ALTER TABLE [" + tablename + "] WITH NOCHECK ADD CONSTRAINT [PK_" + tablename + "] PRIMARY KEY  CLUSTERED ([pid])  ON [PRIMARY]");
			sb.Append(";");

			sb.Append("ALTER TABLE [" + tablename + "] ADD ");
			sb.Append("CONSTRAINT [DF_" + tablename + "_pid] DEFAULT (0) FOR [pid],");
			sb.Append("CONSTRAINT [DF_" + tablename + "_parentid] DEFAULT (0) FOR [parentid],CONSTRAINT [DF_" + tablename + "_layer] DEFAULT (0) FOR [layer],");
			sb.Append("CONSTRAINT [DF_" + tablename + "_poster] DEFAULT ('') FOR [poster],CONSTRAINT [DF_" + tablename + "_posterid] DEFAULT (0) FOR [posterid],");
			sb.Append("CONSTRAINT [DF_" + tablename + "_postdatetime] DEFAULT (getdate()) FOR [postdatetime],CONSTRAINT [DF_" + tablename + "_message] DEFAULT ('') FOR [message],");
			sb.Append("CONSTRAINT [DF_" + tablename + "_ip] DEFAULT ('') FOR [ip],CONSTRAINT [DF_" + tablename + "_lastedit] DEFAULT ('') FOR [lastedit],");
			sb.Append("CONSTRAINT [DF_" + tablename + "_invisible] DEFAULT (0) FOR [invisible],CONSTRAINT [DF_" + tablename + "_usesig] DEFAULT (0) FOR [usesig],");
			sb.Append("CONSTRAINT [DF_" + tablename + "_htmlon] DEFAULT (0) FOR [htmlon],CONSTRAINT [DF_" + tablename + "_smileyoff] DEFAULT (0) FOR [smileyoff],");
			sb.Append("CONSTRAINT [DF_" + tablename + "_parseurloff] DEFAULT (0) FOR [parseurloff],CONSTRAINT [DF_" + tablename + "_bbcodeoff] DEFAULT (0) FOR [bbcodeoff],");
			sb.Append("CONSTRAINT [DF_" + tablename + "_attachment] DEFAULT (0) FOR [attachment],CONSTRAINT [DF_" + tablename + "_rate] DEFAULT (0) FOR [rate],");
			sb.Append("CONSTRAINT [DF_" + tablename + "_ratetimes] DEFAULT (0) FOR [ratetimes]");

			sb.Append(";");
			sb.Append("CREATE  INDEX [parentid] ON [" + tablename + "]([parentid]) ON [PRIMARY]");
			sb.Append(";");

			sb.Append("CREATE  UNIQUE  INDEX [showtopic] ON [" + tablename + "]([tid], [invisible], [pid]) ON [PRIMARY]");
			sb.Append(";");


			sb.Append("CREATE  INDEX [treelist] ON [" + tablename + "]([tid], [invisible], [parentid]) ON [PRIMARY]");
			sb.Append(";");

			#endregion

			#region 建全文索引

			sb.Append("USE " + dbname + " \r\n");
			sb.Append("EXECUTE sp_fulltext_database 'enable'; \r\n");
			sb.Append("IF(SELECT DATABASEPROPERTY('[" + dbname + "]','isfulltextenabled'))=0  EXECUTE sp_fulltext_database 'enable';");
			sb.Append("IF EXISTS (SELECT * FROM sysfulltextcatalogs WHERE name ='pk_" + tablename + "_msg')  EXECUTE sp_fulltext_catalog 'pk_" + tablename + "_msg','drop';");
			sb.Append("IF EXISTS (SELECT * FROM sysfulltextcatalogs WHERE name ='pk_" + tablename + "_msg')  EXECUTE sp_fulltext_table '[" + tablename + "]', 'drop' ;");
			sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + tablename + "_msg','create';");
			sb.Append("EXECUTE sp_fulltext_table '[" + tablename + "]','create','pk_" + tablename + "_msg','pk_" + tablename + "';");
			sb.Append("EXECUTE sp_fulltext_column '[" + tablename + "]','message','add';");
			sb.Append("EXECUTE sp_fulltext_table '[" + tablename + "]','activate';");
			sb.Append("EXECUTE sp_fulltext_catalog 'pk_" + tablename + "_msg','start_full';");

			#endregion

			return sb.ToString();
		}

		public void AddPostTableToTableList(string description, int mintid, int maxtid)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@description", (DbType)SqlDbType.NVarChar, 50, description),
									  DbHelper.MakeInParam("@mintid", (DbType)SqlDbType.Int, 4, mintid),
									  DbHelper.MakeInParam("@maxtid", (DbType)SqlDbType.Int, 4, maxtid)
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "tablelist] ([description],[mintid],[maxtid]) VALUES(@description, @mintid, @maxtid)",parms);
		}

		public void CreatePostProcedure(string sqltemplate)
		{
			foreach (string sql in sqltemplate.Split('~'))
			{
				DbHelper.ExecuteNonQuery(CommandType.Text, sql);
			}
		}

		public DataTable GetMaxTid()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT MAX([tid]) FROM [" + BaseConfigs.GetTablePrefix + "topics]").Tables[0];
		}

		public DataTable GetPostCountFromIndex(string postsid)
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [rows] FROM [sysindexes] WHERE [name]='PK_" + BaseConfigs.GetTablePrefix + "posts" + postsid + "'").Tables[0];
		}

		public DataTable GetPostCountTable(string postsid)
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT(pid) FROM [" + BaseConfigs.GetTablePrefix + "posts" + postsid + "]").Tables[0];
		}

		public void TestFullTextIndex(int posttableid)
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, "SELECT TOP 1 [pid] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE CONTAINS([message],'asd') ORDER BY [pid] ASC");
		}

		public DataRowCollection GetRateRange(int scoreid)
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [groupid], [raterange] FROM [" +
				BaseConfigs.GetTablePrefix +
				"usergroups] WHERE [raterange] LIKE '%" + scoreid + ",True,%'").
				Tables[0].Rows;
		}

		public void UpdateRateRange(string raterange, int groupid)
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix +
				"usergroups] SET [raterange]='" +
				raterange +
				"' WHERE [groupid]=" + groupid);
		}

		public int GetMaxTableListId()
		{
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX([id]), 0) FROM " + BaseConfigs.GetTablePrefix + "tablelist"), 0);
		}

		public int GetMaxPostTableTid(string posttablename)
		{
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX([tid]), 0) FROM " + posttablename), 0) + 1;
		}

		public int GetMinPostTableTid(string posttablename)
		{
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MIN([tid]), 0) FROM " + posttablename), 0) + 1;
		}

		public void AddAdInfo(int available, string type, int displayorder, string title, string targets, string parameters, string code, string starttime, string endtime)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@available", (DbType)SqlDbType.Int, 4, available),
									  DbHelper.MakeInParam("@type", (DbType)SqlDbType.NVarChar, 50, type),
									  DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder),
									  DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 50, title),
									  DbHelper.MakeInParam("@targets", (DbType)SqlDbType.NVarChar, 255, targets),
									  DbHelper.MakeInParam("@parameters", (DbType)SqlDbType.NText, 0, parameters),
									  DbHelper.MakeInParam("@code", (DbType)SqlDbType.NText, 0, code),
									  DbHelper.MakeInParam("@starttime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(starttime)),
									  DbHelper.MakeInParam("@endtime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(endtime))                                        
								  };
			string sql = "INSERT INTO  [" + BaseConfigs.GetTablePrefix + "advertisements] ([available],[type],[displayorder],[title],[targets],[parameters],[code],[starttime],[endtime]) VALUES(@available,@type,@displayorder,@title,@targets,@parameters,@code,@starttime,@endtime)";

			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public string GetAdvertisements()
		{
			return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "advertisements] ORDER BY [advid] ASC";
		}

		public DataRowCollection GetTargetsForumName(string targets)
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [name] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid] IN(0" + targets + "0)").Tables[0].Rows;
		}

		public int UpdateAdvertisementAvailable(string aidlist, int available)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@available", (DbType)SqlDbType.Int, 4, available)
								  };
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "advertisements] SET [available]=@available  WHERE [advid] IN(" + aidlist + ")";

			return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public int UpdateAdvertisement(int aid, int available, string type, int displayorder, string title, string targets, string parameters, string code, string starttime, string endtime)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@aid", (DbType)SqlDbType.Int, 4, aid),
									  DbHelper.MakeInParam("@available", (DbType)SqlDbType.Int, 4, available),
									  DbHelper.MakeInParam("@type", (DbType)SqlDbType.NVarChar, 50, type),
									  DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder),
									  DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 50, title),
									  DbHelper.MakeInParam("@targets", (DbType)SqlDbType.NVarChar, 255, targets),
									  DbHelper.MakeInParam("@parameters", (DbType)SqlDbType.NText, 0, parameters),
									  DbHelper.MakeInParam("@code", (DbType)SqlDbType.NText, 0, code),
									  DbHelper.MakeInParam("@starttime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(starttime)),
									  DbHelper.MakeInParam("@endtime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(endtime))
								  };
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "advertisements] SET [available]=@available,[type]=@type, [displayorder]=@displayorder,[title]=@title,[targets]=@targets,[parameters]=@parameters,[code]=@code,[starttime]=@starttime,[endtime]=@endtime WHERE [advid]=@aid";

			return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public void DeleteAdvertisement(string aidlist)
		{
			string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "advertisements] WHERE [advid] IN (" + aidlist + ")";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql);
		}

		public void BuyTopic(int uid, int tid, int posterid, int price, float netamount, int creditsTrans)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid),
									  DbHelper.MakeInParam("@authorid",(DbType)SqlDbType.Int,4,posterid),
									  DbHelper.MakeInParam("@buydate",(DbType)SqlDbType.DateTime,4,DateTime.Now),
									  DbHelper.MakeInParam("@amount",(DbType)SqlDbType.Int,4,price),
									  DbHelper.MakeInParam("@netamount",(DbType)SqlDbType.Float,8,netamount)
								  };

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [extcredits" + creditsTrans + "] = [extcredits" + creditsTrans + "] - " + price.ToString() + " WHERE [uid] = @uid", prams);
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [extcredits" + creditsTrans + "] = [extcredits" + creditsTrans + "] + @netamount WHERE [uid] = @authorid", prams);
		}

		public int AddPaymentLog(int uid, int tid, int posterid, int price, float netamount)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid),
									  DbHelper.MakeInParam("@authorid",(DbType)SqlDbType.Int,4,posterid),
									  DbHelper.MakeInParam("@buydate",(DbType)SqlDbType.DateTime,4,DateTime.Now),
									  DbHelper.MakeInParam("@amount",(DbType)SqlDbType.Int,4,price),
									  DbHelper.MakeInParam("@netamount",(DbType)SqlDbType.Float,8,netamount)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO [" + BaseConfigs.GetTablePrefix + "paymentlog] ([uid],[tid],[authorid],[buydate],[amount],[netamount]) VALUES(@uid,@tid,@authorid,@buydate,@amount,@netamount)", prams);
		}

		/// <summary>
		/// 判断用户是否已购买主题
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <param name="uid">用户id</param>
		/// <returns></returns>
		public bool IsBuyer(int tid, int uid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [tid] = @tid AND [uid]=@uid", prams), 0) > 0;
		}

		public DataTable GetPayLogInList(int pagesize, int currentpage, int uid)
		{
			int pagetop = (currentpage - 1) * pagesize;
			string sqlstring;
			if (currentpage == 1)
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "paymentlog].*, [" + BaseConfigs.GetTablePrefix + "topics].[fid] AS fid ,[" + BaseConfigs.GetTablePrefix + "topics].[postdatetime] AS postdatetime ,[" + BaseConfigs.GetTablePrefix + "topics].[poster] AS authorname, [" + BaseConfigs.GetTablePrefix + "topics].[title] AS title,[" + BaseConfigs.GetTablePrefix + "users].[username] AS UserName FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid] = [" + BaseConfigs.GetTablePrefix + "topics].[tid] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid] = [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] WHERE [" + BaseConfigs.GetTablePrefix + "paymentlog].[authorid]=" + uid + "  ORDER BY [id] DESC";
			}
			else
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + "[ " + BaseConfigs.GetTablePrefix + "paymentlog].*, [" + BaseConfigs.GetTablePrefix + "topics].[fid] AS fid ,[" + BaseConfigs.GetTablePrefix + "topics].[postdatetime] AS postdatetime ,[" + BaseConfigs.GetTablePrefix + "topics].[poster] AS authorname, [" + BaseConfigs.GetTablePrefix + "topics].[title] AS title,[" + BaseConfigs.GetTablePrefix + "users].[username] AS UserName FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid] = [" + BaseConfigs.GetTablePrefix + "topics].[tid] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid] = [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] WHERE [id] < (SELECT MIN([id]) FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [" + BaseConfigs.GetTablePrefix + "paymentlog].[authorid]=" + uid + " ORDER BY [id] DESC) AS tblTmp ) AND [" + BaseConfigs.GetTablePrefix + "paymentlog].[authorid]=" + uid + " ORDER BY [" + BaseConfigs.GetTablePrefix + "paymentlog].[id] DESC";
			}

			DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
			return dt;
		}

		/// <summary>
		/// 获取指定用户的收入日志记录数
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <returns></returns>
		public int GetPaymentLogInRecordCount(int uid)
		{
			return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [authorid]=" + uid).Tables[0].Rows[0][0].ToString());
		}

		/// <summary>
		/// 返回指定用户的支出日志记录数
		/// </summary>
		/// <param name="pagesize">每页记录数</param>
		/// <param name="currentpage">当前页</param>
		/// <param name="uid">用户id</param>
		/// <returns></returns>
		public DataTable GetPayLogOutList(int pagesize, int currentpage, int uid)
		{
			int pagetop = (currentpage - 1) * pagesize;
			string sqlstring;
			if (currentpage == 1)
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "paymentlog].*, [" + BaseConfigs.GetTablePrefix + "topics].[fid] AS fid ,[" + BaseConfigs.GetTablePrefix + "topics].[postdatetime] AS postdatetime ,[" + BaseConfigs.GetTablePrefix + "topics].[poster] AS authorname, [" + BaseConfigs.GetTablePrefix + "topics].[title] AS title,[" + BaseConfigs.GetTablePrefix + "users].[username] AS UserName FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid] = [" + BaseConfigs.GetTablePrefix + "topics].[tid] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid] = [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] WHERE [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid]=" + uid + "  ORDER BY [id] DESC";
			}
			else
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "paymentlog].*, [" + BaseConfigs.GetTablePrefix + "topics].[fid] AS fid ,[" + BaseConfigs.GetTablePrefix + "topics].[postdatetime] AS postdatetime ,[" + BaseConfigs.GetTablePrefix + "topics].[poster] AS authorname, [" + BaseConfigs.GetTablePrefix + "topics].[title] AS title,[" + BaseConfigs.GetTablePrefix + "users].[username] AS UserName FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid] = [" + BaseConfigs.GetTablePrefix + "topics].[tid] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid] = [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid]=" + uid + " ORDER BY [id] DESC) AS tblTmp ) AND [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid]=" + uid + " ORDER BY [" + BaseConfigs.GetTablePrefix + "paymentlog].[id] DESC";
			}

			DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
			return dt;
		}

		/// <summary>
		/// 返回指定用户支出日志总数
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <returns></returns>
		public int GetPaymentLogOutRecordCount(int uid)
		{
			return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [uid]=" + uid).Tables[0].Rows[0][0].ToString());
		}

		/// <summary>
		/// 获取指定主题的购买记录
		/// </summary>
		/// <param name="pagesize">每页记录数</param>
		/// <param name="currentpage">当前页数</param>
		/// <param name="tid">主题id</param>
		/// <returns></returns>
		public DataTable GetPaymentLogByTid(int pagesize, int currentpage, int tid)
		{
			int pagetop = (currentpage - 1) * pagesize;
			string sqlstring;
			if (currentpage == 1)
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "paymentlog].*, [" + BaseConfigs.GetTablePrefix + "users].[username] AS username FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid] = [" + BaseConfigs.GetTablePrefix + "topics].[tid] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid] = [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] WHERE [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid]=" + tid + "  ORDER BY [id] DESC";
			}
			else
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "paymentlog].*, [" + BaseConfigs.GetTablePrefix + "users].[username] AS username FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "topics] ON [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid] = [" + BaseConfigs.GetTablePrefix + "topics].[tid] LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "users] ON [" + BaseConfigs.GetTablePrefix + "users].[uid] = [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid]=" + tid + " ORDER BY [id] DESC) AS tblTmp ) AND [" + BaseConfigs.GetTablePrefix + "paymentlog].[tid]=" + tid + " ORDER BY [" + BaseConfigs.GetTablePrefix + "paymentlog].[id] DESC";
			}

			DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
			return dt;
		}

		/// <summary>
		/// 主题购买总次数
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <returns></returns>
		public int GetPaymentLogByTidCount(int tid)
		{
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE [tid]=" + tid), 0);
		}


		public void AddSmiles(int id, int displayorder, int type, string code, string url)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id),
									  DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder),
									  DbHelper.MakeInParam("@type", (DbType)SqlDbType.Int, 4, type),
									  DbHelper.MakeInParam("@code", (DbType)SqlDbType.NVarChar, 30, code),
									  DbHelper.MakeInParam("@url", (DbType)SqlDbType.VarChar, 60, url)
								  };


			string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "smilies] ([id],[displayorder],[type],[code],[url]) Values (@id,@displayorder,@type,@code,@url)";

			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public string GetIcons()
		{
			return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=1";
		}

		public string DeleteSmily(int id)
		{
			return "DELETE FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [id]=" + id;
		}

		public void DeleteSmilyByType(int type)
		{
			string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=" + type;
			DbHelper.ExecuteNonQuery(CommandType.Text,sql);
		}

		public int UpdateSmilies(int id, int displayorder, int type, string code, string url)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id),
									  DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder),
									  DbHelper.MakeInParam("@type", (DbType)SqlDbType.Int, 4, type),
									  DbHelper.MakeInParam("@code", (DbType)SqlDbType.NVarChar, 30, code),
									  DbHelper.MakeInParam("@url", (DbType)SqlDbType.VarChar, 60, url)
								  };
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "smilies] SET [displayorder]=@displayorder,[type]=@type,[code]=@code,[url]=@url Where [id]=@id";

			return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public int UpdateSmiliesPart(string code, int displayorder, int id)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id),
									  DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayorder),
									  DbHelper.MakeInParam("@code", (DbType)SqlDbType.NVarChar, 30, code)
								  };
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "smilies] SET [code]=@code,[displayorder]=@displayorder WHERE [id]=@id";
			return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public int DeleteSmilies(string idlist)
		{
			return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "smilies]  WHERE [ID] IN(" + idlist + ")");
		}

		public string GetSmilies()
		{
			return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=0";
		}

		public int GetMaxSmiliesId()
		{
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX(id), 0) FROM " + BaseConfigs.GetTablePrefix + "smilies"), 0) + 1;
		}

		public DataTable GetSmiliesInfoByType(int type)
		{
			IDataParameter parm = DbHelper.MakeInParam("@type", (DbType)SqlDbType.Int, 4, type);
			string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=@type";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
		}


		/// <summary>
		/// 得到表情符数据
		/// </summary>
		/// <returns>表情符数据</returns>
		public IDataReader GetSmiliesList()
		{
			IDataReader dr = DbHelper.ExecuteReader(System.Data.CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=0 ORDER BY [displayorder] DESC,[id] ASC");
			return dr;
		}

		/// <summary>
		/// 得到表情符数据
		/// </summary>
		/// <returns>表情符表</returns>
		public DataTable GetSmiliesListDataTable()
		{
			DataSet ds = DbHelper.ExecuteDataset(System.Data.CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] ORDER BY [type],[displayorder],[id]");
			if (ds != null)
			{
				if (ds.Tables.Count > 0)
				{
					return ds.Tables[0];
				}
			}
			return new DataTable();
		}

		/// <summary>
		/// 得到不带分类的表情符数据
		/// </summary>
		/// <returns>表情符表</returns>
		public DataTable GetSmiliesListWithoutType()
		{
			DataSet ds = DbHelper.ExecuteDataset(System.Data.CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]<>0 ORDER BY [type],[displayorder],[id]");
			if (ds != null)
			{
				if (ds.Tables.Count > 0)
				{
					return ds.Tables[0];
				}
			}
			return new DataTable();
		}

		/// <summary>
		/// 获得表情分类列表
		/// </summary>
		/// <returns></returns>
		public DataTable GetSmilieTypes()
		{
			DataSet ds = DbHelper.ExecuteDataset(System.Data.CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=0 ORDER BY [displayorder],[id]");
			if (ds != null && ds.Tables.Count > 0)
			{
				return ds.Tables[0];
			}
			return new DataTable();
		}

		public DataRow GetSmilieTypeById(string id)
		{
			DataSet ds = DbHelper.ExecuteDataset(System.Data.CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [id]=" + id);
			if (ds != null && ds.Tables[0].Rows.Count == 1)
				return ds.Tables[0].Rows[0];
			else
				return null;
		}
		/// <summary>
		/// 获得统计列
		/// </summary>
		/// <returns>统计列</returns>
		public DataRow GetStatisticsRow()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "statistics]").Tables[0].Rows[0];
		}

		/// <summary>
		/// 将缓存中的统计列保存到数据库
		/// </summary>
		public void SaveStatisticsRow(DataRow dr)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@totaltopic", (DbType)SqlDbType.Int,4,Int32.Parse(dr["totaltopic"].ToString())),
									  DbHelper.MakeInParam("@totalpost",(DbType)SqlDbType.Int,4,Int32.Parse(dr["totalpost"].ToString())),
									  DbHelper.MakeInParam("@totalusers",(DbType)SqlDbType.Int,4,Int32.Parse(dr["totalusers"].ToString())),
									  DbHelper.MakeInParam("@lastusername",(DbType)SqlDbType.NChar,20,dr["totalusers"].ToString()),
									  DbHelper.MakeInParam("@lastuserid",(DbType)SqlDbType.Int,4,Int32.Parse(dr["highestonlineusercount"].ToString())),
									  DbHelper.MakeInParam("@highestonlineusercount",(DbType)SqlDbType.Int,4,Int32.Parse(dr["highestonlineusercount"].ToString())),
									  DbHelper.MakeInParam("@highestonlineusertime",(DbType)SqlDbType.SmallDateTime,4,DateTime.Parse(dr["highestonlineusertime"].ToString()))
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totaltopic]=@totaltopic,[totalpost]=@totalpost, [totalusers]=@totalusers, [lastusername]=@lastusername, [lastuserid]=@lastuserid, [highestonlineusercount]=@@highestonlineusercount, [highestonlineusertime]=@highestonlineusertime", prams);
		}

		public IDataReader GetAllForumStatistics()
		{
			return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT SUM([topics]) AS [topiccount],SUM([posts]) AS [postcount],SUM([todayposts])-(SELECT SUM([todayposts]) FROM [{0}forums] WHERE [lastpost] < CONVERT(CHAR(12),GETDATE(),101) AND [layer]=1) AS [todaypostcount] FROM [{0}forums] WHERE [layer]=1", BaseConfigs.GetTablePrefix));
		}

		public IDataReader GetForumStatistics(int fid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int,4, fid)
								  };
			return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT SUM([topics]) AS [topiccount],SUM([posts]) AS [postcount],SUM([todayposts])-(SELECT SUM([todayposts]) FROM [{0}forums] WHERE [lastpost] < CONVERT(CHAR(12),GETDATE(),101) AND [layer]=1 AND [fid] = @fid) AS [todaypostcount] FROM [{0}forums] WHERE [fid] = @fid AND [layer]=1", BaseConfigs.GetTablePrefix), prams);
		}

		/// <summary>
		/// 更新指定名称的统计项
		/// </summary>
		/// <param name="param">项目名称</param>
		/// <param name="Value">指定项的值</param>
		/// <returns>更新数</returns>
		public int UpdateStatistics(string param, int intValue)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			if (!param.Equals(""))
			{
				sb.Append("UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET ");
				sb.Append(param);
				sb.Append(" = ");
				sb.Append(intValue);
			}
			return DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
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
			if (!param.Equals(""))
			{
				sb.Append("UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET ");
				sb.Append(param);
				sb.Append(" = '");
				sb.Append(strValue);
				sb.Append("'");
			}
			return DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
		}

		/// <summary>
		/// 获得前台有效的模板列表
		/// </summary>
		/// <returns></returns>
		public DataTable GetValidTemplateList()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "templates] ORDER BY [templateid]").Tables[0];
		}

		/// <summary>
		/// 获得前台有效的模板ID列表
		/// </summary>
		/// <returns>模板ID列表</returns>
		public DataTable GetValidTemplateIDList()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [templateid] FROM [" + BaseConfigs.GetTablePrefix + "templates] ORDER BY [templateid]").Tables[0];
		}

		public DataTable GetPost(string posttablename, int pid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 4, pid);
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 *  FROM [" + posttablename + "] WHERE [pid]=@pid", parm).Tables[0];
		}

		public DataTable GetMainPostByTid(string posttablename, int tid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid);
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 * FROM [" + posttablename + "] WHERE [layer]=0  AND [tid]=@tid", parm).Tables[0];
		}

		public DataTable GetAttachmentsByPid(int pid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 4, pid);
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [aid], [tid], [pid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize], [attachment], [downloads] FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [pid]=@pid", parm).Tables[0];
		}

		public DataTable GetAdvertisement(int aid)
		{
			//此函数放在Advs.cs文件中较好
			IDataParameter parm = DbHelper.MakeInParam("@aid", (DbType)SqlDbType.Int, 4, aid);
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "advertisements] WHERE [advid]=@aid", parm).Tables[0];
		}

		private string GetSearchTopicTitleSQL(int posterid, string searchforumid, int resultorder, int resultordertype, int digest, string keyword)
		{
			keyword = Regex.Replace(keyword, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

			StringBuilder strKeyWord = new StringBuilder(keyword);

			// 替换转义字符
			strKeyWord.Replace("'", "''");
			strKeyWord.Replace("%", "[%]");
			strKeyWord.Replace("_", "[_]");
			strKeyWord.Replace("[", "[[]");

			StringBuilder strSQL = new StringBuilder();
			strSQL.AppendFormat("SELECT [tid] FROM [{0}topics] WHERE [displayorder]>=0", BaseConfigs.GetTablePrefix);

			if (posterid > 0)
			{
				strSQL.Append(" AND [posterid]=");
				strSQL.Append(posterid);
			}

			if (digest > 0)
			{
				strSQL.Append(" AND [digest]>0 ");
			}

			if (searchforumid != string.Empty)
			{
				strSQL.Append(" AND [fid] IN (");
				strSQL.Append(searchforumid);
				strSQL.Append(")");
			}

			string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
			strKeyWord = new StringBuilder();

			if (keyword.Length > 0)
			{
				strKeyWord.Append(" AND (1=0 ");
				for (int i = 0; i < keywordlist.Length; i++)
				{
					strKeyWord.Append(" OR [title] ");

					strKeyWord.Append("LIKE '%");
					strKeyWord.Append(RegEsc(keywordlist[i]));
					strKeyWord.Append("%' ");
				}
				strKeyWord.Append(")");
			}

			strSQL.Append(strKeyWord.ToString());

			strSQL.Append(" ORDER BY ");
			switch (resultorder)
			{
				case 1:
					strSQL.Append("[tid]");
					break;
				case 2:
					strSQL.Append("[replies]");
					break;
				case 3:
					strSQL.Append("[views]");
					break;
				default:
					strSQL.Append("[postdatetime]");
					break;
			}

			if (resultordertype == 1)
			{
				strSQL.Append(" ASC");
			}
			else
			{
				strSQL.Append(" DESC");
			}

			return strSQL.ToString();
		}

		private string GetSearchPostContentSQL(int posterid, string searchforumid, int resultorder, int resultordertype, int searchtime, int searchtimetype, int posttableid, StringBuilder strKeyWord)
		{
			StringBuilder strSQL = new StringBuilder();

			string orderfield = "lastpost";
			switch (resultorder)
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
					orderfield = "lastpost";
					break;
			}

			strSQL.AppendFormat("SELECT DISTINCT [{0}posts{1}].[tid],[{0}topics].[{2}] FROM [{0}posts{1}] LEFT JOIN [{0}topics] ON [{0}topics].[tid]=[{0}posts{1}].[tid] WHERE [{0}topics].[displayorder]>=0 AND ", BaseConfigs.GetTablePrefix, posttableid, orderfield);

			if (searchforumid != string.Empty)
			{
				strSQL.AppendFormat("[{0}posts{1}].[fid] IN (", BaseConfigs.GetTablePrefix, posttableid);
				strSQL.Append(searchforumid);
				strSQL.Append(") AND ");
			}

			if (posterid != -1)
			{
				strSQL.AppendFormat("[{0}posts{1}].[posterid]=", BaseConfigs.GetTablePrefix, posttableid);
				strSQL.Append(posterid);
				strSQL.Append(" AND ");
			}

			if (searchtime != 0)
			{
				strSQL.AppendFormat("[{0}posts{1}].[postdatetime]", BaseConfigs.GetTablePrefix, posttableid);
				if (searchtimetype == 1)
				{
					strSQL.Append("<'");
				}
				else
				{
					strSQL.Append(">'");
				}
				strSQL.Append(DateTime.Now.AddDays(searchtime).ToString("yyyy-MM-dd 00:00:00"));
				strSQL.Append("'AND ");
			}

			string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
			strKeyWord = new StringBuilder();
			for (int i = 0; i < keywordlist.Length; i++)
			{
				strKeyWord.Append(" OR ");
				if (GeneralConfigs.GetConfig().Fulltextsearch == 1)
				{
					strKeyWord.AppendFormat("CONTAINS(message, '\"*", BaseConfigs.GetTablePrefix, posttableid);
					strKeyWord.Append(keywordlist[i]);
					strKeyWord.Append("*\"') ");
				}
				else
				{
					strKeyWord.AppendFormat("[{0}posts{1}].[message] LIKE '%", BaseConfigs.GetTablePrefix, posttableid);
					strKeyWord.Append(RegEsc(keywordlist[i]));
					strKeyWord.Append("%' ");
				}
			}

			strSQL.Append(strKeyWord.ToString().Substring(3));
			strSQL.AppendFormat("ORDER BY [{0}topics].", BaseConfigs.GetTablePrefix);

			switch (resultorder)
			{
				case 1:
					strSQL.Append("[tid]");
					break;
				case 2:
					strSQL.Append("[replies]");
					break;
				case 3:
					strSQL.Append("[views]");
					break;
				default:
					strSQL.Append("[lastpost]");
					break;
			}
			if (resultordertype == 1)
			{
				strSQL.Append(" ASC");
			}
			else
			{
				strSQL.Append(" DESC");
			}

			return strSQL.ToString();
		}

		private string GetSearchSpacePostTitleSQL(int posterid, int resultorder, int resultordertype, int searchtime, int searchtimetype, string keyword)
		{
			keyword = Regex.Replace(keyword, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

			StringBuilder strKeyWord = new StringBuilder(keyword);

			// 替换转义字符
			strKeyWord.Replace("'", "''");
			strKeyWord.Replace("%", "[%]");
			strKeyWord.Replace("_", "[_]");
			strKeyWord.Replace("[", "[[]");

			StringBuilder strSQL = new StringBuilder();
			strSQL.AppendFormat("SELECT [postid] FROM [{0}spaceposts] WHERE [{0}spaceposts].[poststatus]=1 ", BaseConfigs.GetTablePrefix);
            
			if (posterid > 0)
			{
				strSQL.Append(" AND [uid]=");
				strSQL.Append(posterid);
			}

			if (searchtime != 0)
			{
				strSQL.AppendFormat(" AND [{0}spaceposts].[postdatetime]", BaseConfigs.GetTablePrefix);
				if (searchtimetype == 1)
				{
					strSQL.Append("<'");
				}
				else
				{
					strSQL.Append(">'");
				}
				strSQL.Append(DateTime.Now.AddDays(searchtime).ToString("yyyy-MM-dd 00:00:00"));
				strSQL.Append("' ");
			}

			string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
			strKeyWord = new StringBuilder();

			if (keyword.Length > 0)
			{
				strKeyWord.Append(" AND (1=0 ");
				for (int i = 0; i < keywordlist.Length; i++)
				{
					strKeyWord.Append(" OR [title] ");
					strKeyWord.Append("LIKE '%");
					strKeyWord.Append(RegEsc(keywordlist[i]));
					strKeyWord.Append("%' ");
				}
				strKeyWord.Append(")");
			}

			strSQL.Append(strKeyWord.ToString());

			strSQL.Append(" ORDER BY ");
			switch (resultorder)
			{
				case 1:
					strSQL.Append("[commentcount]");
					break;
				case 2:
					strSQL.Append("[views]");
					break;
				default:
					strSQL.Append("[postdatetime]");
					break;
			}

			if (resultordertype == 1)
			{
				strSQL.Append(" ASC");
			}
			else
			{
				strSQL.Append(" DESC");
			}

			return strSQL.ToString();
		}

		private string GetSearchAlbumTitleSQL(int posterid, int resultorder, int resultordertype, int searchtime, int searchtimetype, string keyword)
		{
			keyword = Regex.Replace(keyword, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

			StringBuilder strKeyWord = new StringBuilder(keyword);

			// 替换转义字符
			strKeyWord.Replace("'", "''");
			strKeyWord.Replace("%", "[%]");
			strKeyWord.Replace("_", "[_]");
			strKeyWord.Replace("[", "[[]");

			StringBuilder strSQL = new StringBuilder();
			strSQL.AppendFormat("SELECT [albumid] FROM [{0}albums] WHERE [{0}albums].[type]=0 ", BaseConfigs.GetTablePrefix);

			if (posterid > 0)
			{
				strSQL.Append(" AND [userid]=");
				strSQL.Append(posterid);
			}

			if (searchtime != 0)
			{
				strSQL.AppendFormat(" AND [{0}albums].[createdatetime]", BaseConfigs.GetTablePrefix);
				if (searchtimetype == 1)
				{
					strSQL.Append("<'");
				}
				else
				{
					strSQL.Append(">'");
				}
				strSQL.Append(DateTime.Now.AddDays(searchtime).ToString("yyyy-MM-dd 00:00:00"));
				strSQL.Append("' ");
			}

			string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
			strKeyWord = new StringBuilder();

			if (keyword.Length > 0)
			{
				strKeyWord.Append(" AND (1=0 ");
				for (int i = 0; i < keywordlist.Length; i++)
				{
					strKeyWord.Append(" OR [title] ");
					strKeyWord.Append("LIKE '%");
					strKeyWord.Append(RegEsc(keywordlist[i]));
					strKeyWord.Append("%' ");
				}
				strKeyWord.Append(")");
			}

			strSQL.Append(strKeyWord.ToString());

			strSQL.Append(" ORDER BY ");
			switch (resultorder)
			{
				case 1:
					strSQL.Append("[albumid]");
					break;
				default:
					strSQL.Append("[createdatetime]");
					break;
			}

			if (resultordertype == 1)
			{
				strSQL.Append(" ASC");
			}
			else
			{
				strSQL.Append(" DESC");
			}

			return strSQL.ToString();
		}

		private string GetSearchByPosterSQL(int posterid, int posttableid)
		{
			if (posterid > 0)
			{
				string sql = string.Format(@"SELECT DISTINCT [tid], 'forum' AS [datafrom] FROM [{0}posts{1}] WHERE [posterid]={2} AND [tid] NOT IN (SELECT [tid] FROM [{0}topics] WHERE [posterid]={2} AND [displayorder]<0) UNION ALL SELECT [albumid],'album' AS [datafrom] FROM [{0}albums] WHERE   [userid]={2} UNION ALL SELECT [postid],'spacepost' AS [datafrom] FROM [{0}spaceposts] WHERE [uid]={2}", BaseConfigs.GetTablePrefix, posttableid, posterid);
				return sql;
			}
			return string.Empty;
		}

		private StringBuilder GetSearchByPosterResult(IDataReader reader)
		{
			StringBuilder strTids = new StringBuilder("<ForumTopics>");
			StringBuilder strAlbumids = new StringBuilder("<Albums>");
			StringBuilder strSpacePostids = new StringBuilder("<SpacePosts>");
			StringBuilder result = new StringBuilder();

			if (reader != null)
			{
				while (reader.Read())
				{
					switch (reader[1].ToString())
					{
						case "forum":
							strTids.AppendFormat("{0},", reader[0].ToString());
							break;
						case "album":
							strAlbumids.AppendFormat("{0},", reader[0].ToString());
							break;
						case "spacepost":
							strSpacePostids.AppendFormat("{0},", reader[0].ToString());
							break;
					}
				}
				reader.Close();
			}
			if (strTids.ToString().EndsWith(","))
			{
				strTids.Length--;
			}
			if (strAlbumids.ToString().EndsWith(","))
			{
				strAlbumids.Length--;
			}
			if (strSpacePostids.ToString().EndsWith(","))
			{
				strSpacePostids.Length--;
			}
			strTids.Append("</ForumTopics>");
			strAlbumids.Append("</Albums>");
			strSpacePostids.Append("</SpacePosts>");

			result.Append(strTids.ToString());
			result.Append(strAlbumids.ToString());
			result.Append(strSpacePostids.ToString());

			return result;
		}
		/// <summary>
		/// 根据指定条件进行搜索
		/// </summary>
		/// <param name="posttableid">帖子表id</param>
		/// <param name="userid">用户id</param>
		/// <param name="usergroupid">用户组id</param>
		/// <param name="keyword">关键字</param>
		/// <param name="posterid">发帖者id</param>
		/// <param name="type">搜索类型</param>
		/// <param name="searchforumid">搜索版块id</param>
		/// <param name="keywordtype">关键字类型</param>
		/// <param name="searchtime">搜索时间</param>
		/// <param name="searchtimetype">搜索时间类型</param>
		/// <param name="resultorder">结果排序方式</param>
		/// <param name="resultordertype">结果类型类型</param>
		/// <returns>如果成功则返回searchid, 否则返回-1</returns>
		public int Search(int posttableid, int userid, int usergroupid, string keyword, int posterid, string type, string searchforumid, int keywordtype, int searchtime, int searchtimetype, int resultorder, int resultordertype)
		{

			// 超过30分钟的缓存纪录将被删除
			DatabaseProvider.GetInstance().DeleteExpriedSearchCache();
			string sql = string.Empty;
			StringBuilder strTids = new StringBuilder();
			SearchType searchType = SearchType.TopicTitle;

			switch (keywordtype)
			{ 
				case 0:
					searchType = SearchType.PostTitle;
					if (type == "digest")
					{
						searchType = SearchType.DigestTopic;
					}
					break;
				case 1:
					searchType = SearchType.PostContent;
					break;
				case 2:
					searchType = SearchType.SpacePostTitle;
					break;
				case 3:
					searchType = SearchType.AlbumTitle;
					break;
				case 8:
					searchType = SearchType.ByPoster;
					break;
			}
			switch (searchType)
			{ 
				case SearchType.All:
					break;
				case SearchType.DigestTopic:
					sql = GetSearchTopicTitleSQL(posterid, searchforumid, resultorder, resultordertype, 1, keyword);
					break;
				case SearchType.TopicTitle:
					sql = GetSearchTopicTitleSQL(posterid, searchforumid, resultorder, resultordertype, 0, keyword);
					break;
				case SearchType.PostTitle:
					sql = GetSearchTopicTitleSQL(posterid, searchforumid, resultorder, resultordertype, 0, keyword);
					break;
				case SearchType.PostContent:
					sql = GetSearchPostContentSQL(posterid, searchforumid, resultorder, resultordertype, searchtime, searchtimetype, posttableid, new StringBuilder(keyword));
					break;
				case SearchType.SpacePostTitle:
					sql = GetSearchSpacePostTitleSQL(posterid, resultorder, resultordertype, searchtime, searchtimetype, keyword);
					break;
				case SearchType.AlbumTitle:
					sql = GetSearchAlbumTitleSQL(posterid, resultorder, resultordertype, searchtime, searchtimetype, keyword);
					break;
				case SearchType.ByPoster:
					sql = GetSearchByPosterSQL(posterid, posttableid);
					break;
				default:
					sql = GetSearchTopicTitleSQL(posterid, searchforumid, resultorder, resultordertype, 0, keyword);
					break;
			}
            
			#region
			/*
			// 关键词与作者至少有一个条件不为空
			if (keyword.Equals(""))//按作者搜索
			{

				if (type == "digest")
				{
					strSQL.AppendFormat("SELECT [tid] FROM [{0}topics] WHERE [digest]>0 AND [posterid]=", BaseConfigs.GetTablePrefix);
					strSQL.Append(posterid);
				}

				else if (type == "post")
				{
					strSQL.AppendFormat("SELECT [pid] FROM [{0}posts{1}] WHERE [posterid]=", BaseConfigs.GetTablePrefix, posttableid);
					strSQL.Append(posterid);
				}
				else
				{
					strSQL.AppendFormat("SELECT [tid] FROM [{0}topics] WHERE [posterid]=", BaseConfigs.GetTablePrefix);
					strSQL.Append(posterid);
				}

				//所属板块判断
				if (!searchforumid.Equals(""))
				{
					strSQL.Append(" AND [fid] IN (");
					strSQL.Append(searchforumid);
					strSQL.Append(")");
				}


				strSQL.Append(" ORDER BY ");

				switch (resultorder)
				{
					case 1:
						strSQL.Append("[tid]");
						break;
					case 2:
						strSQL.Append("[replies]");
						break;
					case 3:
						strSQL.Append("[views]");
						break;
					default:
						strSQL.Append("[postdatetime]");
						break;
				}
				if (resultordertype == 1)
				{
					strSQL.Append(" ASC");
				}
				else
				{
					strSQL.Append(" DESC");
				}

			}
			else
			{
				// 过滤危险字符
				keyword = Regex.Replace(keyword, "--|;|'|\"", "", RegexOptions.Compiled | RegexOptions.Multiline);

				StringBuilder strKeyWord = new StringBuilder(keyword);

				// 替换转义字符
				strKeyWord.Replace("'", "''");
				strKeyWord.Replace("%", "[%]");
				strKeyWord.Replace("_", "[_]");
				strKeyWord.Replace("[", "[[]");


				// 将SQL查询条件循序指定为"forumid, posterid, 搜索时间范围, 关键词"
				if (keywordtype == 0)
				{
					strSQL.AppendFormat("SELECT [tid] FROM [{0}topics] WHERE", BaseConfigs.GetTablePrefix);
					if (!searchforumid.Equals(""))
					{
						strSQL.Append(" [fid] IN (");
						strSQL.Append(searchforumid);
						strSQL.Append(") AND ");
					}
					if (posterid != -1)
					{
						strSQL.Append("[posterid]=");
						strSQL.Append(posterid);
						strSQL.Append(" AND ");
					}

					if (searchtime != 0)
					{
						strSQL.Append("[postdatetime]");
						if (searchtimetype == 1)
						{
							strSQL.Append("<");
						}
						else
						{
							strSQL.Append(">'");
						}
						strSQL.Append(DateTime.Now.AddDays(searchtime * -1).ToString("yyyy-MM-dd 00:00:00"));
						strSQL.Append("'AND ");
					}

					string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
					strKeyWord = new StringBuilder();
					for (int i = 0; i < keywordlist.Length; i++)
					{
						strKeyWord.Append(" OR [title] ");

						strKeyWord.Append("LIKE '%");
						strKeyWord.Append(keywordlist[i]);
						strKeyWord.Append("%' ");
					}

					strSQL.Append(strKeyWord.ToString().Substring(3));
					strSQL.Append("ORDER BY ");

					switch (resultorder)
					{
						case 1:
							strSQL.Append("[tid]");
							break;
						case 2:
							strSQL.Append("[replies]");
							break;
						case 3:
							strSQL.Append("[views]");
							break;
						default:
							strSQL.Append("[lastpost]");
							break;
					}
					if (resultordertype == 1)
					{
						strSQL.Append(" ASC");
					}
					else
					{
						strSQL.Append(" DESC");
					}
				}
				else
				{
					string orderfield = "lastpost";
					switch (resultorder)
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
							orderfield = "lastpost";
							break;
					}


					strSQL.AppendFormat("SELECT DISTINCT [{0}posts{1}].[tid],[{0}topics].[{2}] FROM [{0}posts{1}] LEFT JOIN [{0}topics] ON [{0}topics].[tid]=[{0}posts{1}].[tid] WHERE ", BaseConfigs.GetTablePrefix, posttableid, orderfield);
					if (!searchforumid.Equals(""))
					{
						strSQL.AppendFormat("[{0}posts{1}].[fid] IN (", BaseConfigs.GetTablePrefix, posttableid);
						strSQL.Append(searchforumid);
						strSQL.Append(") AND ");
					}
					if (posterid != -1)
					{
						strSQL.AppendFormat("[{0}posts{1}].[posterid]=", BaseConfigs.GetTablePrefix, posttableid);
						strSQL.Append(posterid);
						strSQL.Append(" AND ");
					}

					if (searchtime != 0)
					{
						strSQL.AppendFormat("[{0}posts{1}].[postdatetime]", BaseConfigs.GetTablePrefix, posttableid);
						if (searchtimetype == 1)
						{
							strSQL.Append("<");
						}
						else
						{
							strSQL.Append(">'");
						}
						strSQL.Append(DateTime.Now.AddDays(searchtime).ToString("yyyy-MM-dd 00:00:00"));
						strSQL.Append("'AND ");
					}

					string[] keywordlist = Utils.SplitString(strKeyWord.ToString(), " ");
					strKeyWord = new StringBuilder();
					for (int i = 0; i < keywordlist.Length; i++)
					{
						strKeyWord.Append(" OR ");
						if (GeneralConfigs.GetConfig().Fulltextsearch == 1)
						{
							strKeyWord.AppendFormat("CONTAINS(message, '\"*", BaseConfigs.GetTablePrefix, posttableid);
							strKeyWord.Append(keywordlist[i]);
							strKeyWord.Append("*\"') ");
						}
						else
						{
							strKeyWord.AppendFormat("[{0}posts{1}].[message] LIKE '%", BaseConfigs.GetTablePrefix, posttableid);
							strKeyWord.Append(keywordlist[i]);
							strKeyWord.Append("%' ");
						}
					}

					strSQL.Append(strKeyWord.ToString().Substring(3));
					strSQL.AppendFormat("ORDER BY [{0}topics].", BaseConfigs.GetTablePrefix);

					switch (resultorder)
					{
						case 1:
							strSQL.Append("[tid]");
							break;
						case 2:
							strSQL.Append("[replies]");
							break;
						case 3:
							strSQL.Append("[views]");
							break;
						default:
							strSQL.Append("[lastpost]");
							break;
					}
					if (resultordertype == 1)
					{
						strSQL.Append(" ASC");
					}
					else
					{
						strSQL.Append(" DESC");
					}
				}
			}
			*/
			#endregion

			if (sql == string.Empty)
			{
				return -1;
			}

			IDataParameter[] prams2 = {
									   DbHelper.MakeInParam("@searchstring",(DbType)SqlDbType.VarChar,255, sql),
									   DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,userid),
									   DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.Int,4,usergroupid)
								   };
			int searchid = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format(@"SELECT TOP 1 [searchid] FROM [{0}searchcaches] WHERE [searchstring]=@searchstring AND [groupid]=@groupid", BaseConfigs.GetTablePrefix), prams2), -1);

			if (searchid > -1)
			{
				return searchid;
			}

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
					case SearchType.All:
					case SearchType.DigestTopic:
					case SearchType.TopicTitle:
					case SearchType.PostTitle:
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
						cacheinfo.Keywords = keyword;
						cacheinfo.Searchstring = sql;
						cacheinfo.Postdatetime = Utils.GetDateTime();
						cacheinfo.Topics = rowcount;
						cacheinfo.Tids = strTids.ToString();
						cacheinfo.Uid = userid;
						cacheinfo.Groupid = usergroupid;
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
						case SearchType.All:
						case SearchType.DigestTopic:
						case SearchType.TopicTitle:
						case SearchType.PostTitle:
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
					cacheinfo.Keywords = keyword;
					cacheinfo.Searchstring = sql;
					cacheinfo.Postdatetime = Utils.GetDateTime();
					cacheinfo.Topics = rowcount;
					cacheinfo.Tids = strTids.ToString();
					cacheinfo.Uid = userid;
					cacheinfo.Groupid = usergroupid;
					cacheinfo.Ip = DNTRequest.GetIP();
					cacheinfo.Expiration = Utils.GetDateTime();

					reader.Close();

					return CreateSearchCache(cacheinfo);
				}
				reader.Close();
			}
			return -1;
		}     

		public string BackUpDatabase(string backuppath, string ServerName, string UserName, string Password, string strDbName, string strFileName)
		{
			SQLServer svr = new SQLServerClass();
			try
			{
				svr.Connect(ServerName, UserName, Password);
				Backup bak = new BackupClass();
				bak.Action = 0;
				bak.Initialize = true;
				bak.Files = backuppath + strFileName + ".config";
				bak.Database = strDbName;
				bak.SQLBackup(svr);
				return string.Empty;
			}
			catch(Exception ex)
			{
				string message = ex.Message.Replace("'", " ");
				message = message.Replace("\n", " ");
				message = message.Replace("\\", "/");
				return message;
			}
			finally
			{
				svr.DisConnect();
			}
		}

		public string RestoreDatabase(string backuppath, string ServerName, string UserName, string Password, string strDbName, string strFileName)
		{
			#region 数据库的恢复的代码

			SQLServer svr = new SQLServerClass();
			try
			{
				svr.Connect(ServerName, UserName, Password);
				QueryResults qr = svr.EnumProcesses(-1);
				int iColPIDNum = -1;
				int iColDbName = -1;
				for (int i = 1; i <= qr.Columns; i++)
				{
					string strName = qr.get_ColumnName(i);
					if (strName.ToUpper().Trim() == "SPID")
					{
						iColPIDNum = i;
					}
					else if (strName.ToUpper().Trim() == "DBNAME")
					{
						iColDbName = i;
					}
					if (iColPIDNum != -1 && iColDbName != -1)
						break;
				}

				for (int i = 1; i <= qr.Rows; i++)
				{
					int lPID = qr.GetColumnLong(i, iColPIDNum);
					string strDBName = qr.GetColumnString(i, iColDbName);
					if (strDBName.ToUpper() == strDbName.ToUpper())
						svr.KillProcess(lPID);
				}


				Restore res = new RestoreClass();
				res.Action = 0;
				string path = backuppath + strFileName + ".config";
				res.Files = path;

				res.Database = strDbName;
				res.ReplaceDatabase = true;
				res.SQLRestore(svr);

				return string.Empty;
			}
			catch (Exception err)
			{
				string message = err.Message.Replace("'", " ");
				message = message.Replace("\n", " ");
				message = message.Replace("\\", "/");
                
				return message;
			}
			finally
			{
				svr.DisConnect();
			}

			#endregion
		}

		public string SearchVisitLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string others)
		{
			string sqlstring = null;
			sqlstring += " [visitid]>0";

			sqlstring = GetSqlstringByPostDatetime(sqlstring, postdatetimeStart, postdatetimeEnd);

			if (others != "")
			{
				sqlstring += " AND [others] LIKE '%" + RegEsc(others) + "%'";
			}

			if (Username != "")
			{
				sqlstring += " AND (";
				foreach (string word in Username.Split(','))
				{
					if (word.Trim() != "")
						sqlstring += " [username] LIKE '%" + RegEsc(word) + "%' OR ";
				}
				sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
			}

			return sqlstring;
        
		}


		public string SearchMedalLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string reason)
		{
			string sqlstring = null;
			sqlstring += " [id]>0";

			sqlstring = GetSqlstringByPostDatetime(sqlstring, postdatetimeStart, postdatetimeEnd);

			if (reason != "")
			{
				sqlstring += " AND [reason] LIKE '%" + RegEsc(reason) + "%'";
			}

			if (Username != "")
			{
				sqlstring += " AND (";
				foreach (string word in Username.Split(','))
				{
					if (word.Trim() != "")
						sqlstring += " [username] LIKE '%" + RegEsc(word) + "%' OR ";
				}
				sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
			}
			return sqlstring;
		}

		public string SearchModeratorManageLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string others)
		{
			string sqlstring = null;
			sqlstring += " [id]>0";

			sqlstring = GetSqlstringByPostDatetime(sqlstring, postdatetimeStart, postdatetimeEnd);

			if (others != "")
			{
				sqlstring += " AND [reason] LIKE '%" + RegEsc(others) + "%'";
			}

			if (Username != "")
			{
				sqlstring += " AND (";
				foreach (string word in Username.Split(','))
				{
					if (word.Trim() != "")
						sqlstring += " [moderatorname] LIKE '%" + RegEsc(word) + "%' OR ";
				}
				sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
			}

			return sqlstring;
		}

		public string SearchPaymentLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username)
		{

			string sqlstring = null;
			sqlstring += " [" + BaseConfigs.GetTablePrefix + "paymentlog].[id]>0";

			if (postdatetimeStart.ToString() != "")
			{
				sqlstring += " AND [" + BaseConfigs.GetTablePrefix + "paymentlog].[buydate]>='" + postdatetimeStart.ToString("yyyy-MM-dd HH:mm:ss") + "'";
			}

			if (postdatetimeEnd.ToString() != "")
			{
				sqlstring += " AND [" + BaseConfigs.GetTablePrefix + "paymentlog].[buydate]<='" + postdatetimeEnd.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "'";
			}

			if (Username != "")
			{
				string usernamesearch = " WHERE (";
				foreach (string word in Username.Split(','))
				{
					if (word.Trim() != "")
						usernamesearch += " [username] LIKE '%" + RegEsc(word) + "%' OR ";
				}
				usernamesearch = usernamesearch.Substring(0, usernamesearch.Length - 3) + ")";

				//找出当前用户名所属的UID
				DataTable dt = DbHelper.ExecuteDataset("SELECT [uid] From [" + BaseConfigs.GetTablePrefix + "users] " + usernamesearch).Tables[0];
				string uid = "-1";
				if (dt.Rows.Count > 0)
				{
					foreach (DataRow dr in dt.Rows)
					{
						uid += "," + dr["uid"].ToString();
					}
				}
				sqlstring += " AND [" + BaseConfigs.GetTablePrefix + "paymentlog].[uid] IN(" + uid + ")";

			}

			return sqlstring;
		}

		public string SearchRateLog(DateTime postdatetimeStart, DateTime postdatetimeEnd, string Username, string others)
		{
			string sqlstring = null;
			sqlstring += " [id]>0";

			sqlstring = GetSqlstringByPostDatetime(sqlstring, postdatetimeStart, postdatetimeEnd);

			if (others != "")
			{
				sqlstring += " AND [reason] LIKE '%" + RegEsc(others) + "%'";
			}

			if (Username != "")
			{
				sqlstring += " AND (";
				foreach (string word in Username.Split(','))
				{
					if (word.Trim() != "")
						sqlstring += " [username] LIKE '%" + RegEsc(word) + "%' OR ";
				}
				sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
			}

			return sqlstring;
		}

		public string DeletePrivateMessages(bool isnew, string postdatetime, string msgfromlist, bool lowerupper, string subject, string message, bool isupdateusernewpm)
		{
			string sqlstring = null;
			sqlstring += "WHERE [pmid]>0";

			if (isnew)
			{
				sqlstring += " AND [new]=0";
			}

			if (postdatetime != "")
			{
				sqlstring += " AND DATEDIFF(day,postdatetime,getdate())>=" + postdatetime + "";
			}

			if (msgfromlist != "")
			{
				sqlstring += " AND (";
				foreach (string msgfrom in msgfromlist.Split(','))
				{
					if (msgfrom.Trim() != "")
					{
						if (lowerupper)
						{
							sqlstring += " [msgfrom]='" + msgfrom + "' OR";
						}
						else
						{
							sqlstring += " [msgfrom] COLLATE Chinese_PRC_CS_AS_WS ='" + msgfrom + "' OR";

						}
					}
				}
				sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
			}

			if (subject != "")
			{
				sqlstring += " AND (";
				foreach (string sub in subject.Split(','))
				{
					if (sub.Trim() != "")
						sqlstring += " [subject] LIKE '%" + RegEsc(sub) + "%' OR ";
				}
				sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
			}

			if (message != "")
			{
				sqlstring += " AND (";
				foreach (string mess in message.Split(','))
				{
					if (mess.Trim() != "")
						sqlstring += " [message] LIKE '%" + RegEsc(mess) + "%' OR ";
				}
				sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
			}

			if (isupdateusernewpm)
			{
				DbHelper.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [newpm]=0 WHERE [uid] IN (SELECT [msgtoid] FROM [" + BaseConfigs.GetTablePrefix + "pms] " + sqlstring + ")");
			}

			DbHelper.ExecuteNonQuery("DELETE FROM [" + BaseConfigs.GetTablePrefix + "pms] " + sqlstring);

			return sqlstring;
		}

		public bool IsExistSmilieCode(string code, int currentid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@code",(DbType)SqlDbType.NVarChar, 30, code),
									  DbHelper.MakeInParam("@currentid",(DbType)SqlDbType.Int, 4, currentid)
								  };
			string sql = "SELECT [id] FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [code]=@code AND [id]<>@currentid";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0].Rows.Count != 0;
		}

		public string  GetSmilieByType(int id)
		{
			return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "smilies] WHERE [type]=" + id;
		}


		public string AddTableData()
		{

			return "SELECT [groupid], [grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]<=3 ORDER BY [groupid]";

		}

		public string Global_UserGrid_GetCondition(string getstring)
		{

			return "[" + BaseConfigs.GetTablePrefix + "users].[username]='" + getstring + "'";

		}

		public int Global_UserGrid_RecordCount()
		{

			return Convert.ToInt32(DbHelper.ExecuteDataset("SELECT COUNT(uid) FROM [" + BaseConfigs.GetTablePrefix + "users]").Tables[0].Rows[0][0].ToString());

		}


		public int Global_UserGrid_RecordCount(string condition)
		{

			return Convert.ToInt32(DbHelper.ExecuteDataset("SELECT COUNT(uid) FROM [" + BaseConfigs.GetTablePrefix + "users]  WHERE " + condition).Tables[0].Rows[0][0].ToString());

		}

		public string Global_UserGrid_SearchCondition(bool islike, bool ispostdatetime, string username, string nickname, string UserGroup, string email, string credits_start, string credits_end, string lastip, string posts, string digestposts, string uid, string joindateStart, string joindateEnd)
		{

			string searchcondition = " [" + BaseConfigs.GetTablePrefix + "users].[uid]>0 ";
			if (islike)
			{
				if (username != "") searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[username] like'%" + RegEsc(username) + "%'";
				if (nickname != "") searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[nickname] like'%" + RegEsc(nickname) + "%'";
			}
			else
			{
				if (username != "") searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[username] ='" + username + "'";
				if (nickname != "") searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[nickname] ='" + nickname + "'";
			}

			if (UserGroup != "0")
			{
				searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[groupid]=" + UserGroup;
			}

			if (email != "")
			{
				searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[email] LIKE '%" + RegEsc(email) + "%'";
			}

			if (credits_start != "")
			{
				searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[credits] >=" + credits_start;
			}

			if (credits_end != "")
			{
				searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[credits] <=" + credits_end;
			}

			if (lastip != "")
			{
				searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[lastip] LIKE '%" + RegEsc(lastip) + "%'";
			}

			if (posts != "")
			{
				searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[posts] >=" + posts;
			}


			if (digestposts != "")
			{
				searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[digestposts] >=" + digestposts;
			}

			if (uid != "")
			{
				uid = uid.Replace(", ", ",");

				if (uid.IndexOf(",") == 0)
				{
					uid = uid.Substring(1, uid.Length - 1);
				}
				if (uid.LastIndexOf(",") == (uid.Length - 1))
				{
					uid = uid.Substring(0, uid.Length - 1);
				}

				if (uid != "")
				{
					searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[uid] IN(" + uid + ")";
				}

			}

			if (ispostdatetime)
			{
				searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[joindate] >='" + DateTime.Parse(joindateStart).ToString("yyyy-MM-dd HH:mm:ss") + "'";
				searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[joindate] <='" + DateTime.Parse(joindateEnd).ToString("yyyy-MM-dd HH:mm:ss") + "'";
			}


			return searchcondition;

		}


		public DataTable Global_UserGrid_Top2(string searchcondition)
		{

			return DbHelper.ExecuteDataset("SELECT TOP 2 [" + BaseConfigs.GetTablePrefix + "users].[uid]  FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE " + searchcondition).Tables[0];

		}


		public System.Collections.ArrayList CheckDbFree()
		{
          
           
			return null;
		}

		public void DbOptimize(string tablelist)
		{
           
		}

		public void UpdateAdminUsergroup(string targetadminusergroup, string sourceadminusergroup)
		{

			DbHelper.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [groupid]=" + targetadminusergroup + " WHERE [groupid]=" + sourceadminusergroup);

		}

		public void UpdateUserCredits(string formula)
		{
			DbHelper.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [credits]=" + formula);

		}

		public DataTable MailListTable(string usernamelist)
		{

			string strwhere = " WHERE [Email] Is Not null AND (";
			foreach (string username in usernamelist.Split(','))
			{
				if (username.Trim() != "")
					strwhere += " [username] LIKE '%" + RegEsc(username.Trim()) + "%' OR ";
			}
			strwhere = strwhere.Substring(0, strwhere.Length - 3) + ")";

			DataTable dt = DbHelper.ExecuteDataset("SELECT [username],[Email]  FROM [" + BaseConfigs.GetTablePrefix + "users] " + strwhere).Tables[0];
			return dt;
		}

		#endregion

		#region HelpManage

		//取得分类
		public IDataReader GetHelpList(int id)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id),
                                        
			};
			string sql = "SELECT [id],[title],[message],[pid],[orderby] FROM [" + BaseConfigs.GetTablePrefix + "help] WHERE [pid]=@id OR [id]=@id ORDER BY [pid] ASC, [orderby] ASC";

			return DbHelper.ExecuteReader(CommandType.Text, sql,parms);
    
        
		}


		public IDataReader ShowHelp(int id)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id),
                                        
			};
			string sql = "SELECT [title],[message],[pid],[orderby] FROM [" + BaseConfigs.GetTablePrefix + "help] WHERE [id]=@id";
			return DbHelper.ExecuteReader(CommandType.Text, sql,parms);

		}


		public IDataReader GetHelpClass()
		{

			string sql = "SELECT [id] FROM [" + BaseConfigs.GetTablePrefix + "help] WHERE [pid]=0 ORDER BY [orderby] ASC";
			return DbHelper.ExecuteReader(CommandType.Text, sql);
		}
        


		public void AddHelp(string title,string message,int pid,int orderby)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@title", (DbType)SqlDbType.Char, 100, title),
									  DbHelper.MakeInParam("@message", (DbType)SqlDbType.NText,0,message),
									  DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int,4, pid),
									  DbHelper.MakeInParam("@orderby", (DbType)SqlDbType.Int, 4, orderby)
                                        
								  };
			string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "help]([title],[message],[pid],[orderby]) VALUES(@title,@message,@pid,@orderby)";
			DbHelper.ExecuteNonQuery(CommandType.Text,sql,parms);
		}

		public void DelHelp(string idlist)
		{
			//IDataParameter[] parms = {
			//                            DbHelper.MakeInParam("@idlist", (DbType)SqlDbType.Int, 100, idlist)
                                       
                                        
			//                        };

			string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "help] WHERE [id] IN ("+idlist+") OR [pid] IN ("+idlist+")";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql);
		}

		public void ModHelp(int id,string title,string message,int pid,int orderby)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@title", (DbType)SqlDbType.Char, 100, title),
									  DbHelper.MakeInParam("@message", (DbType)SqlDbType.NText,0,message),
									  DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int,4, pid),
									  DbHelper.MakeInParam("@orderby", (DbType)SqlDbType.Int, 4, orderby),
									  DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id)
                                        
								  };

			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "help] SET [title]=@title,[message]=@message,[pid]=@pid,[orderby]=@orderby WHERE [id]=@id";
			DbHelper.ExecuteNonQuery(CommandType.Text,sql,parms);
        
		}

		//public string  admingrid()
		//{
		//    string sql = "select *,case pid when 0 then id else pid end as o from dnt_help order by o";

		//    return sql;
        
        
		//}

		public int HelpCount()
		{
			string sql = "SELECT COUNT(*) FROM [" + BaseConfigs.GetTablePrefix + "help]";
			return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql).ToString());
        
		}

		public string BindHelpType()

		{
			string sql = "SELECT [id],[title] FROM [" + BaseConfigs.GetTablePrefix + "help] WHERE [pid]=0 ORDER BY [orderby] ASC";
			return sql;
        
		}

		public void UpOrder(string orderby, string id)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@orderby", (DbType)SqlDbType.Char, 100, orderby),
									  DbHelper.MakeInParam("@id", (DbType)SqlDbType.VarChar, 100,id),
                                        
                                        
			};

			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "help] SET [ORDERBY]=@orderby  Where id=@id";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);

		}

		#endregion

		#region PostManage

		public string SearchTopicAudit(int fid, string poster, string title, string moderatorname, DateTime postdatetimeStart, DateTime postdatetimeEnd, DateTime deldatetimeStart, DateTime deldatetimeEnd)
		{
			string sqlstring = null;
			sqlstring += " [displayorder]<0";

			if (fid != 0)
			{
				sqlstring += " AND [fid]=" + fid;
			}

			if (poster != "")
			{
				sqlstring += " AND (";
				foreach (string postername in poster.Split(','))
				{
					if (postername.Trim() != "")
					{
						sqlstring += " [poster]='" + postername + "'  OR";
					}
				}
				sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
			}


			if (title != "")
			{
				sqlstring += " AND (";
				foreach (string titlename in title.Split(','))
				{
					if (titlename.Trim() != "")
					{
						sqlstring += " [title] LIKE '%" + RegEsc(titlename) + "%' OR";
					}
				}
				sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
			}


			if (moderatorname != "")
			{
				string logtidlist = "";
				//DataTable dt = DbHelper.ExecuteDataset("SELECT [title]	FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog] WHERE (moderatorname = '" + moderatorname + "') AND (actions = 'DELETE')").Tables[0];
				DataTable dt = DatabaseProvider.GetInstance().GetTitleForModeratormanagelogByModeratorname(moderatorname);
				if (dt.Rows.Count > 0)
				{
					foreach (DataRow dr in dt.Rows)
					{
						logtidlist += dr["title"].ToString() + ",";
					}
					sqlstring = sqlstring + " AND tid IN (" + logtidlist.Substring(0, logtidlist.Length - 1) + ") ";
				}
			}

			if (postdatetimeStart.ToString().IndexOf("1900") < 0)
			{
				sqlstring += " AND [postdatetime]>='" + postdatetimeStart.ToString("yyyy-MM-dd HH:mm:ss") + "'";
			}

			if (postdatetimeEnd.ToString().IndexOf("1900") < 0)
			{
				sqlstring += " AND [postdatetime]<='" + postdatetimeEnd.ToString("yyyy-MM-dd HH:mm:ss") + "'";
			}

			if ((deldatetimeStart.ToString().IndexOf("1900") < 0) && (deldatetimeStart.ToString().IndexOf("1900") < 0))
			{
				string logtidlist2 = "";
				//DataTable dt = DbHelper.ExecuteDataset("SELECT [title]	FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog] WHERE (postdatetime >= '" + deldatetimeStart.SelectedDate.ToString() + "') AND (postdatetime<='" + deldatetimeEnd.SelectedDate.ToString() + "')AND (actions = 'DELETE')").Tables[0];
				DataTable dt = DatabaseProvider.GetInstance().GetTitleForModeratormanagelogByPostdatetime(deldatetimeStart, deldatetimeStart);
				if (dt.Rows.Count > 0)
				{
					foreach (DataRow dr in dt.Rows)
					{
						logtidlist2 += dr["title"].ToString() + ",";
					}
					sqlstring = sqlstring + " AND tid IN (" + logtidlist2.Substring(0, logtidlist2.Length - 1) + ") ";
				}
			}
			return sqlstring;
		}

		public void AddBBCCode(int available, string tag, string icon, string replacement, string example,
			string explanation, string param, string nest, string paramsdescript, string paramsdefvalue)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@available", (DbType)SqlDbType.Int, 4, available),
				DbHelper.MakeInParam("@tag", (DbType)SqlDbType.VarChar, 100, tag),
				DbHelper.MakeInParam("@icon", (DbType)SqlDbType.VarChar,50, icon),
				DbHelper.MakeInParam("@replacement", (DbType)SqlDbType.NText,0, replacement),
				DbHelper.MakeInParam("@example", (DbType)SqlDbType.NVarChar, 255, example),
				DbHelper.MakeInParam("@explanation", (DbType)SqlDbType.NText, 0, explanation),
				DbHelper.MakeInParam("@params", (DbType)SqlDbType.Int, 4, param),
				DbHelper.MakeInParam("@nest", (DbType)SqlDbType.Int, 4, nest),
				DbHelper.MakeInParam("@paramsdescript", (DbType)SqlDbType.NText, 0, paramsdescript),
				DbHelper.MakeInParam("@paramsdefvalue", (DbType)SqlDbType.NText, 0, paramsdefvalue)
			};
			string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "bbcodes] ([available],[tag],[icon],[replacement],[example]," +
				"[explanation],[params],[nest],[paramsdescript],[paramsdefvalue]) VALUES(@available,@tag,@icon,@replacement,@example,@explanation,@params," +
				"@nest,@paramsdescript,@paramsdefvalue)";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}


		/// <summary>
		/// 产生附件
		/// </summary>
		/// <param name="attachmentinfo">附件描述类实体</param>
		/// <returns>附件id</returns>
		public int CreateAttachment(AttachmentInfo attachmentinfo)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,attachmentinfo.Uid),
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,attachmentinfo.Tid),
									  DbHelper.MakeInParam("@pid",(DbType)SqlDbType.Int,4,attachmentinfo.Pid),
									  DbHelper.MakeInParam("@postdatetime",(DbType)SqlDbType.DateTime, 8, DateTime.Parse(attachmentinfo.Postdatetime)),
									  DbHelper.MakeInParam("@readperm",(DbType)SqlDbType.Int,4,attachmentinfo.Readperm),
									  DbHelper.MakeInParam("@filename",(DbType)SqlDbType.VarChar,100,attachmentinfo.Filename),
									  DbHelper.MakeInParam("@description",(DbType)SqlDbType.VarChar,100,attachmentinfo.Description),
									  DbHelper.MakeInParam("@filetype",(DbType)SqlDbType.VarChar,50,attachmentinfo.Filetype),
									  DbHelper.MakeInParam("@filesize",(DbType)SqlDbType.Int,4,attachmentinfo.Filesize),
									  DbHelper.MakeInParam("@attachment",(DbType)SqlDbType.VarChar,100,attachmentinfo.Attachment),
									  DbHelper.MakeInParam("@downloads",(DbType)SqlDbType.Int,4,attachmentinfo.Downloads)
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "createattachment", prams), -1);
		}

		/// <summary>
		/// 更新主题附件类型
		/// </summary>
		/// <param name="tid">主题Id</param>
		/// <param name="attType">附件类型,1普通附件,2为图片附件</param>
		/// <returns></returns>
		public int UpdateTopicAttachmentType(int tid, int attType)
		{
			IDataParameter[] parm = {
									 DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
								 };
			return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}topics] SET [attachment]={1} WHERE [tid]=@tid", BaseConfigs.GetTablePrefix, attType), parm);
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
			IDataParameter[] parm = {
									 DbHelper.MakeInParam("@pid",(DbType)SqlDbType.Int,4,pid)
								 };
			return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}posts{1}] SET [attachment]={2} WHERE [pid]=@pid", BaseConfigs.GetTablePrefix, postTableId, attType), parm);
		}

		/// <summary>
		/// 获取指定附件信息
		/// </summary>
		/// <param name="aid">附件Id</param>
		/// <returns></returns>
		public IDataReader GetAttachmentInfo(int aid)
		{
			IDataParameter[] parm = {
									 DbHelper.MakeInParam("@aid", (DbType)SqlDbType.Int,4, aid),
			};

			return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT TOP 1 * FROM [{0}attachments] WHERE [aid]=@aid", BaseConfigs.GetTablePrefix), parm);
		}

		/// <summary>
		/// 获得指定帖子的附件个数
		/// </summary>
		/// <param name="pid">帖子ID</param>
		/// <returns>附件个数</returns>
		public int GetAttachmentCountByPid(int pid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@pid",(DbType)SqlDbType.Int,4,pid)
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([aid]) AS [acount] FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [pid]=@pid", prams), 0);
		}

		/// <summary>
		/// 获得指定主题的附件个数
		/// </summary>
		/// <param name="tid">主题ID</param>
		/// <returns>附件个数</returns>
		public int GetAttachmentCountByTid(int tid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([aid]) AS [acount] FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [tid]=@tid", prams), 0);
		}

		/// <summary>
		/// 获得指定帖子的附件
		/// </summary>
		/// <param name="pid">帖子ID</param>
		/// <returns>帖子信息</returns>
		public DataTable GetAttachmentListByPid(int pid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@pid",(DbType)SqlDbType.Int,4,pid)
								  };
			DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [pid]=@pid", prams);
			if (ds != null)
			{
				if (ds.Tables.Count > 0)
				{
					return ds.Tables[0];
				}
			}
			return new DataTable();
		}

		/// <summary>
		/// 获得系统设置的附件类型
		/// </summary>
		/// <returns>系统设置的附件类型</returns>
		public DataTable GetAttachmentType()
		{
			DataTable dt = new DataTable();
			DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT [id], [extension], [maxsize] FROM [{0}attachtypes]", BaseConfigs.GetTablePrefix));
			if (ds != null)
			{
				dt = ds.Tables[0];
			}

			return dt;
		}

		/// <summary>
		/// 更新附件下载次数
		/// </summary>
		/// <param name="aid">附件id</param>
		public void UpdateAttachmentDownloads(int aid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@aid",(DbType)SqlDbType.Int,4,aid)
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}attachments] SET [downloads]=[downloads]+1 WHERE [aid]=@aid", BaseConfigs.GetTablePrefix), prams);
		}

		/// <summary>
		/// 更新主题是否包含附件
		/// </summary>
		/// <param name="tid">主题Id</param>
		/// <param name="hasAttachment">是否包含附件,0不包含,1包含</param>
		/// <returns></returns>
		public int UpdateTopicAttachment(int tid, int hasAttachment)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}topics] SET [attachment]={1} WHERE [tid]=@tid", BaseConfigs.GetTablePrefix, hasAttachment), prams);
		}

		/// <summary>
		/// 获得指定主题的所有附件
		/// </summary>
		/// <param name="tid">主题Id</param>
		/// <returns></returns>
		public IDataReader GetAttachmentListByTid(int tid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
								  };
			return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT [aid],[filename] FROM [{0}attachments] WHERE [tid]=@tid ", BaseConfigs.GetTablePrefix), prams);

		}

		/// <summary>
		/// 获得指定主题的所有附件
		/// </summary>
		/// <param name="tidlist">主题Id列表，以英文逗号分割</param>
		/// <returns></returns>
		public IDataReader GetAttachmentListByTid(string tidlist)
		{
			return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT [aid],[filename] FROM [{0}attachments] WHERE [tid] IN ({1})", BaseConfigs.GetTablePrefix, tidlist));
		}

		/// <summary>
		/// 删除指定主题的所有附件
		/// </summary>
		/// <param name="tid">版块tid</param>
		/// <returns>删除个数</returns>
		public int DeleteAttachmentByTid(int tid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}attachments] WHERE [tid]=@tid ", BaseConfigs.GetTablePrefix), prams);
		}

		/// <summary>
		/// 删除指定主题的所有附件
		/// </summary>
		/// <param name="tid">主题Id列表，以英文逗号分割</param>
		/// <returns>删除个数</returns>
		public int DeleteAttachmentByTid(string tidlist)
		{
			return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}attachments] WHERE [tid] IN ({1})", BaseConfigs.GetTablePrefix, tidlist));
		}

		/// <summary>
		/// 删除指定附件
		/// </summary>
		/// <param name="aid"></param>
		/// <returns></returns>
		public int DeleteAttachment(int aid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@aid",(DbType)SqlDbType.Int,4,aid)
								  };

			return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}attachments] WHERE [aid]=@aid", BaseConfigs.GetTablePrefix), prams);
		}

		/// <summary>
		/// 批量删除附件
		/// </summary>
		/// <param name="aidList">附件Id列表，以英文逗号分割</param>
		/// <returns></returns>
		public int DeleteAttachment(string aidList)
		{
			return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}attachments] WHERE [aid] IN ({1})", BaseConfigs.GetTablePrefix, aidList));
		}

		public int UpdatePostAttachment(int pid, string postTableId, int hasAttachment)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@pid",(DbType)SqlDbType.Int,4,pid)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}posts{1}] SET [attachment]={2} WHERE [pid]=@pid", BaseConfigs.GetTablePrefix, postTableId, hasAttachment), prams);
		}

        /// <summary>
        /// 根据帖子Id删除附件
        /// </summary>
        /// <param name="pid">帖子Id</param>
        /// <returns></returns>
        public int DeleteAttachmentByPid(int pid)
        {
            IDataParameter[] parms = {
                                        DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 4, pid)
                                    };
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}attachments] WHERE [pid]=@pid", BaseConfigs.GetTablePrefix), parms);
        }


		/// <summary>
		/// 更新附件信息
		/// </summary>
		/// <param name="attachmentInfo">附件对象</param>
		/// <returns>返回被更新的数量</returns>
		public int UpdateAttachment(AttachmentInfo attachmentInfo)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@aid",(DbType)SqlDbType.Int, 4,attachmentInfo.Aid),
									  DbHelper.MakeInParam("@postdatetime",(DbType)SqlDbType.DateTime, 8, DateTime.Parse(attachmentInfo.Postdatetime)),
									  DbHelper.MakeInParam("@readperm", (DbType)SqlDbType.Int, 4, attachmentInfo.Readperm),
									  DbHelper.MakeInParam("@filename", (DbType)SqlDbType.NChar, 100, attachmentInfo.Filename),
									  DbHelper.MakeInParam("@description", (DbType)SqlDbType.NChar, 100, attachmentInfo.Description),
									  DbHelper.MakeInParam("@filetype", (DbType)SqlDbType.NChar, 50, attachmentInfo.Filetype),
									  DbHelper.MakeInParam("@filesize", (DbType)SqlDbType.Int, 4, attachmentInfo.Filesize),
									  DbHelper.MakeInParam("@attachment", (DbType)SqlDbType.NChar, 100, attachmentInfo.Attachment),
									  DbHelper.MakeInParam("@downloads", (DbType)SqlDbType.Int, 4, attachmentInfo.Downloads)
								  };

			string sql = string.Format(@"UPDATE [{0}attachments] SET [postdatetime] = @postdatetime, [readperm] = @readperm, [filename] = @filename, [description] = @description, [filetype] = @filetype, [filesize] = @filesize, [attachment] = @attachment, [downloads] = @downloads 
											WHERE [aid]=@aid", BaseConfigs.GetTablePrefix);

			return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		/// <summary>
		/// 更新附件信息
		/// </summary>
		/// <param name="aid">附件Id</param>
		/// <param name="readperm">阅读权限</param>
		/// <param name="description">描述</param>
		/// <returns>返回被更新的数量</returns>
		public int UpdateAttachment(int aid, int readperm, string description)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@aid", (DbType)SqlDbType.Int, 4, aid),
									  DbHelper.MakeInParam("@readperm", (DbType)SqlDbType.Int, 4, readperm),
									  DbHelper.MakeInParam("@description", (DbType)SqlDbType.NChar, 100, description)
								  };

			string sql = string.Format(@"UPDATE [{0}attachments] SET [readperm] = @readperm, [description] = @description WHERE [aid] = @aid", BaseConfigs.GetTablePrefix);

			return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public IDataReader GetAttachmentList(string aidList)
		{
			return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT [aid],[filename],[tid],[pid] FROM [{0}attachments] WHERE [aid] IN ({1})", BaseConfigs.GetTablePrefix, aidList));
		}

		public IDataReader GetAttachmentListByPid(string pidList)
		{
			return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}attachments] WHERE [pid] IN ({1})", BaseConfigs.GetTablePrefix, pidList));
		}

		/// <summary>
		/// 获得上传附件文件的大小
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <returns></returns>
		public int GetUploadFileSizeByUserId(int uid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid)
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT SUM([filesize]) AS [todaysize] FROM [{0}attachments] WHERE [uid]=@uid AND DATEDIFF(d,[postdatetime],GETDATE())=0", BaseConfigs.GetTablePrefix), prams), 0);
		}

		/// <summary>
		/// 取得主题贴的第一个图片附件
		/// </summary>
		/// <param name="tid">主题id</param>
		public DataSet GetFirstImageAttachByTid(int tid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid)
								  };
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [tid]=@tid AND LEFT([filetype], 5)='image' ORDER BY [aid]", prams);
		}

		public DataSet GetAttchType()
		{
			string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "attachtypes] ORDER BY [id] ASC";
			return DbHelper.ExecuteDataset(CommandType.Text, sql);
		}

		public string GetAttchTypeSql()
		{
			return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "attachtypes] ORDER BY [id] ASC";
		}

		public void AddAttchType(string extension, string maxsize)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@extension", (DbType)SqlDbType.VarChar,256, extension),
				DbHelper.MakeInParam("@maxsize", (DbType)SqlDbType.Int, 4, maxsize)
			};
			string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "attachtypes] ([extension], [maxsize]) VALUES (@extension,@maxsize)";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void UpdateAttchType(string extension, string maxsize, int id)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@extension", (DbType)SqlDbType.VarChar,256, extension),
				DbHelper.MakeInParam("@maxsize", (DbType)SqlDbType.Int, 4, maxsize),
				DbHelper.MakeInParam("@id",(DbType)SqlDbType.Int, 4,id)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "attachtypes] SET [extension]=@extension ,[maxsize]=@maxsize WHERE [id]=@id";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void DeleteAttchType(string attchtypeidlist)
		{
			string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "attachtypes] WHERE [id] IN (" + attchtypeidlist + ")";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql);
		}

		public bool IsExistExtensionInAttachtypes(string extensionname)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@extension", (DbType)SqlDbType.VarChar,256, extensionname)
			};
			string sql = "SELECT TOP 1  * FROM [" + BaseConfigs.GetTablePrefix + "attachtypes] WHERE [extension]=@extension";
			if (DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0].Rows.Count > 0)
				return true;
			else
				return false;
		}

		public DataTable GetTitleForModeratormanagelogByModeratorname(string moderatorname)
		{
			string sql = "SELECT [title] FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog] WHERE ([moderatorname] = '" + moderatorname + "') AND ([actions] = 'DELETE')";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public DataTable GetTitleForModeratormanagelogByPostdatetime(DateTime startDateTime, DateTime endDateTime)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@startDateTime", (DbType)SqlDbType.DateTime, 8, startDateTime),
									  DbHelper.MakeInParam("@endDateTime", (DbType)SqlDbType.DateTime, 8, endDateTime)
								  };
			string sql = "SELECT [title] FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog] WHERE (postdatetime >= @startDateTime) AND ([postdatetime]<= @endDateTime) AND ([actions] = 'DELETE')";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
		}

		public DataTable GetTidForModeratormanagelogByPostdatetime(DateTime postDateTime)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, postDateTime)
								  };
			string sql = "SELECT [tid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [displayorder]=-1 AND [postdatetime]<=@postdatetime";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
		}


		public string GetUnauditNewTopicSQL()
		{
			return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [displayorder]=-2";
		}

		public void PassAuditNewTopic(string postTableName, string tidlist)
		{
			string sqlstring = "UPDATE  [" + postTableName + "]  SET [invisible]=0 WHERE [layer]=0  AND [tid] IN(" + tidlist + ")";
			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

			sqlstring = "UPDATE  [" + BaseConfigs.GetTablePrefix + "topics]  SET [displayorder]=0 WHERE [tid] IN(" + tidlist + ")";
			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totaltopic]=[totaltopic] + " + tidlist.Split(',').Length);

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totalpost]=[totalpost] + " + tidlist.Split(',').Length);

			//更新相关的版块统计信息
			foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] IN(" + tidlist + ") ORDER BY [tid] ASC").Tables[0].Rows)
			{
				DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics] = [topics] + 1, [curtopics] = [curtopics] + 1, [posts]=[posts] + 1, [todayposts]=CASE WHEN DATEPART(yyyy, [lastpost])=DATEPART(yyyy,GETDATE()) AND DATEPART(mm,[lastpost])=DATEPART(mm,GETDATE()) AND DATEPART(dd, [lastpost])=DATEPART(dd,GETDATE()) THEN [todayposts]*1 + 1	ELSE 1 END,[lasttid]=" + dr["tid"].ToString() + " ,	[lasttitle]='" + dr["title"].ToString().Replace("'", "''") + "',[lastpost]='" + dr["postdatetime"].ToString() + "',[lastposter]='" + dr["poster"].ToString().Replace("'", "''") + "',[lastposterid]=" + dr["posterid"].ToString() + " WHERE [fid]=" + dr["fid"].ToString());
				DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [lastpost] = '" + dr["postdatetime"].ToString() + "', [lastpostid] =" + dr["posterid"].ToString() + ", [lastposttitle] ='" + dr["title"].ToString().Replace("'", "''") + "', [posts] = [posts] + 1	WHERE [uid] = " + dr["posterid"].ToString());
			}
		}

		public DataTable GetDetachTableId()
		{
			string sql = "SELECT ID FROM [" + BaseConfigs.GetTablePrefix + "tablelist] ORDER BY ID ASC";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public int GetCurrentPostTableRecordCount(int currentPostTableId)
		{
			string sql = "SELECT count(pid) FROM [" + BaseConfigs.GetTablePrefix + "posts" + currentPostTableId + "] WHERE [invisible]=1";
			return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, sql));
		}

		public string GetUnauditPostSQL(int currentPostTableId)
		{
			return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "posts" + currentPostTableId + "] WHERE [invisible]=1 AND [layer]>0";
		}

		public void PassPost(int currentPostTableId, string pidlist)
		{
			string sqlstring = "UPDATE  [" + BaseConfigs.GetTablePrefix + "posts" + currentPostTableId + "]  SET [invisible]=0 WHERE [pid] IN(" + pidlist + ")";
			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totalpost]=[totalpost] + " + pidlist.Split(',').Length);

			//更新相关的版块统计信息
			foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "posts" + currentPostTableId + "] WHERE [pid] IN(" + pidlist + ") ORDER BY [pid] ASC").Tables[0].Rows)
			{
				DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [posts]=[posts] + 1, [todayposts]=CASE WHEN DATEPART(yyyy, [lastpost])=DATEPART(yyyy,GETDATE()) AND DATEPART(mm, [lastpost])=DATEPART(mm,GETDATE()) AND DATEPART(dd, [lastpost])=DATEPART(dd,GETDATE()) THEN [todayposts]*1 + 1	ELSE 1 END,[lastpost]='" + dr["postdatetime"].ToString() + "',[lastposter]='" + dr["poster"].ToString().Replace("'", "''") + "',[lastposterid]=" + dr["posterid"].ToString() + " WHERE [fid]=" + dr["fid"].ToString());
				DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [lastpost] = '" + dr["postdatetime"].ToString() + "', [lastpostid] =" + dr["posterid"].ToString() + ", [lastposttitle] ='" + dr["title"].ToString().Replace("'", "''") + "', [posts] = [posts] + 1	WHERE [uid] = " + dr["posterid"].ToString());
				DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics]  SET [replies]=[replies]+1,[lastposter]='" + dr["poster"].ToString().Replace("'", "''") + "',[lastposterid]=" + dr["posterid"].ToString() + ",[lastpost]='" + dr["postdatetime"].ToString() + "' WHERE [tid]=" + dr["tid"].ToString());
			}
		}

		public DataTable GetPostLayer(int currentPostTableId, int postid)
		{
			string sql = "SELECT TOP 1 [layer],[tid]  FROM [" + BaseConfigs.GetTablePrefix + "posts" + currentPostTableId + "] WHERE [pid]=" + postid;
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public void UpdateBBCCode(int available, string tag, string icon, string replacement, string example,
			string explanation, string param, string nest, string paramsdescript, string paramsdefvalue, int id)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@available", (DbType)SqlDbType.Int, 4, available),
				DbHelper.MakeInParam("@tag", (DbType)SqlDbType.VarChar, 100, tag),
				DbHelper.MakeInParam("@icon", (DbType)SqlDbType.VarChar,50, icon),
				DbHelper.MakeInParam("@replacement", (DbType)SqlDbType.NText,0, replacement),
				DbHelper.MakeInParam("@example", (DbType)SqlDbType.NVarChar, 255, example),
				DbHelper.MakeInParam("@explanation", (DbType)SqlDbType.NText, 0, explanation),
				DbHelper.MakeInParam("@params", (DbType)SqlDbType.Int, 4, param),
				DbHelper.MakeInParam("@nest", (DbType)SqlDbType.Int, 4, nest),
				DbHelper.MakeInParam("@paramsdescript", (DbType)SqlDbType.NText, 0, paramsdescript),
				DbHelper.MakeInParam("@paramsdefvalue", (DbType)SqlDbType.NText, 0, paramsdefvalue),
				DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "bbcodes] SET [available]=@available,tag=@tag, icon=@icon,replacement=@replacement," +
				"example=@example,explanation=@explanation,params=@params,nest=@nest,paramsdescript=@paramsdescript,paramsdefvalue=@paramsdefvalue " +
				"WHERE [id]=@id";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public DataTable GetBBCode()
		{
			string sql = "Select * From [" + BaseConfigs.GetTablePrefix + "bbcodes] Order BY [id] ASC";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public DataTable GetBBCode(int id)
		{
			IDataParameter parm = DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id);
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "bbcodes] WHERE [id]=@id", parm).Tables[0];
		}

		public void DeleteBBCode(string idlist)
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "bbcodes]  WHERE [id] IN(" + idlist + ")");
		}

		public void SetBBCodeAvailableStatus(string idlist, int status)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@status",(DbType)SqlDbType.Int,4,status)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "bbcodes] SET [available]=@status  WHERE [id] IN(" + idlist + ")";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public DataSet GetBBCCodeById(int id)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id)
			};
			string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "bbcodes] WHERE [id]=@id";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, prams);
		}

		/// <summary>
		/// 获得帖子列表
		/// </summary>
		/// <param name="count">数量</param>
		/// <param name="views">最小浏览量</param>
		/// <param name="fid">板块ID</param>
		/// <param name="timetype">期限类型,一天、一周、一月、不限制</param>
		/// <param name="ordertype">排序类型,时间倒序、浏览量倒序、最后回复倒序</param>
		/// <param name="isdigest">是否精华</param>
		/// <param name="onlyimg">缓存的有效期(单位:分钟)</param>
		/// <returns></returns>
		public DataTable GetFocusTopicList(int count, int views, int fid, string starttime, string orderfieldname, string visibleForum, bool isdigest, bool onlyimg)
		{
			IDataParameter param =
				DbHelper.MakeInParam("@starttime", (DbType) SqlDbType.DateTime, 8, DateTime.Parse(starttime));
			string digestParam = "";
			if (isdigest)
				digestParam = " AND [digest] > 0";

			string fidParam = "";
			if (fid > 0)
				fidParam = " AND [fid]=" + fid;

			if (count < 0)
				count = 0;

			string attParam = "";
			if (onlyimg)
				attParam = "AND [attachment]=2";

			if (visibleForum != string.Empty)
				visibleForum = " AND [fid] IN (" + visibleForum + ")";

			string sqlstr = @"SELECT TOP {0} * FROM [{1}topics] WHERE [displayorder] >=0 AND [views] > {2} AND [postdatetime] > @starttime{3}{4} ORDER BY [{5}] DESC";

			sqlstr = string.Format(sqlstr,
				count,
				BaseConfigs.GetTablePrefix,
				views,
				fidParam + digestParam + visibleForum,
				attParam,
				orderfieldname
				);

			return DbHelper.ExecuteDataset(CommandType.Text, sqlstr, param).Tables[0];
		}

		public void UpdateTopicLastPoster(int lastposterid, string lastposter)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@lastposter", (DbType)SqlDbType.VarChar, 20, lastposter),
									  DbHelper.MakeInParam("@lastposterid", (DbType)SqlDbType.Int, 4, lastposterid)
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [lastposter]=@lastposter  WHERE [lastposterid]=@lastposterid", parms);
		}

		public void UpdateTopicPoster(int posterid, string poster)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.Int, 4, posterid),
									  DbHelper.MakeInParam("@poster", (DbType)SqlDbType.VarChar, 20, poster)
								  };

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [poster]=@poster WHERE [posterid]=@posterid", parms);
		}

		public void UpdatePostPoster(int posterid, string poster, string posttableid)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.Int, 4, posterid),
									  DbHelper.MakeInParam("@poster", (DbType)SqlDbType.VarChar, 20, poster)
								  };

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] SET [poster]=@poster WHERE [posterid]=@posterid", parms);
		}

		/// <summary>
		/// 更新主题信息
		/// </summary>
		/// <param name="topicinfo"></param>
		/// <returns></returns>
		public bool UpdateTopicAllInfo(TopicInfo topicinfo)
		{
			string sqlstring = string.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET fid='{1}',iconid='{2}',typeid='{3}',readperm='{4}',price='{5}',poster='{6}'," +
				"title='{7}',postdatetime='{8}',lastpost='{9}',lastpostid='{10}',lastposter='{11}'," +
				"views='{12}',replies='{13}',displayorder='{14}',highlight='{15}',digest='{16}',rate='{17}',blog='{18}'," +
				"poll='{19}',attachment='{20}',moderated='{21}',closed='{22}' WHERE [tid]={0}",
				topicinfo.Tid.ToString(),
				topicinfo.Fid.ToString(),
				topicinfo.Iconid.ToString(),
				topicinfo.Typeid.ToString(),
				topicinfo.Readperm.ToString(),
				topicinfo.Price,
				topicinfo.Poster,
				topicinfo.Title,
				topicinfo.Postdatetime,
				topicinfo.Lastpost,
				topicinfo.Lastpostid.ToString(),
				topicinfo.Lastposter,
				topicinfo.Views.ToString(),
				topicinfo.Replies.ToString(),
				topicinfo.Displayorder.ToString(),
				topicinfo.Highlight,
				topicinfo.Digest.ToString(),
				topicinfo.Rate.ToString(),
				topicinfo.Hide.ToString(),
				topicinfo.Poll.ToString(),
				topicinfo.Attachment.ToString(),
				topicinfo.Moderated.ToString(),
				topicinfo.Closed.ToString());

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
			return true;
		}

		/// <summary>
		/// 根据主题ID删除相应的主题信息
		/// </summary>
		/// <param name="tid"></param>
		/// <returns></returns>
		public bool DeleteTopicByTid(int tid, string posttablename)
		{
			SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
			conn.Open();
			using (SqlTransaction trans = conn.BeginTransaction())
			{
				try
				{
					DbHelper.ExecuteNonQuery(trans, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [tid]=" + tid.ToString());
					DbHelper.ExecuteNonQuery(trans, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "favorites] WHERE [tid]=" + tid.ToString());
					DbHelper.ExecuteNonQuery(trans, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "polls] WHERE [tid]=" + tid.ToString());
					DbHelper.ExecuteNonQuery(trans, "DELETE FROM [" + posttablename + "] WHERE [tid]=" + tid.ToString());
					DbHelper.ExecuteNonQuery(trans, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid]=" + tid.ToString());
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

		public bool SetTypeid(string topiclist, int value)
		{
			SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
			conn.Open();
			using (SqlTransaction trans = conn.BeginTransaction())
			{
				try
				{
					DbHelper.ExecuteNonQuery(trans, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [typeid]=" + value.ToString() + " WHERE [tid] IN(" + topiclist + ")");
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

		public DataSet GetPosts(int tid, int pagesize, int pageindex, string posttablename)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid),
									  DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int, 4, pagesize),
									  DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int, 4, pageindex),
									  DbHelper.MakeInParam("@posttablename", (DbType)SqlDbType.VarChar, 30, posttablename)
								  };
			DataSet ds = DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getpostlist", prams);
			return ds;
		}


		public int GetAttachCount(int pid)
		{
			IDataParameter[] prams2 = {
									   DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 0, pid)
								   };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([aid]) AS [aidcount] FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [pid] = @pid", prams2), 0);

		}

		public bool SetDisplayorder(string topiclist, int value)
		{
			//SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
			//conn.Open();
			//using (SqlTransaction trans = conn.BeginTransaction())
			//{
			//    try
			//    {
			DbHelper.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [displayorder]=" + value.ToString() + " WHERE [tid] IN(" + topiclist + ")");
			//        trans.Commit();
			//    }
			//    catch (Exception ex)
			//    {
			//        trans.Rollback();
			//        throw ex;
			//    }
			//}
			//conn.Close();
			return true;

		}

		/// <summary>
		/// 添加评分记录
		/// </summary>
		/// <param name="postidlist">被评分帖子pid</param>
		/// <param name="userid">评分者uid</param>
		/// <param name="username">评分者用户名</param>
		/// <param name="extid">分的积分类型</param>
		/// <param name="score">积分数值</param>
		/// <param name="reason">评分理由</param>
		/// <returns>更新数据行数</returns>
		public int InsertRateLog(int pid, int userid, string username, int extid, float score, string reason)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 4, pid),
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userid),
									  DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar, 20, username),
									  DbHelper.MakeInParam("@extcredits", (DbType)SqlDbType.TinyInt, 1, extid),
									  DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, DateTime.Now),
									  DbHelper.MakeInParam("@score", (DbType)SqlDbType.SmallInt, 2, score),
									  DbHelper.MakeInParam("@reason", (DbType)SqlDbType.NVarChar, 50, reason)
								  };

			string CommandText = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "ratelog] ([pid],[uid],[username],[extcredits],[postdatetime],[score],[reason]) VALUES (@pid,@uid,@username,@extcredits,@postdatetime,@score,@reason)";

			return DbHelper.ExecuteNonQuery(CommandType.Text, CommandText, prams);
		}

		/// <summary>
		/// 删除日志
		/// </summary>
		/// <returns></returns>
		public bool DeleteRateLog()
		{
			try
			{
				if (DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "ratelog] ") > 1)
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
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
				DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "ratelog] WHERE " + condition);
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
		public DataTable RateLogList(int pagesize, int currentpage, string posttablename)
		{
			int pagetop = (currentpage - 1) * pagesize;
			string sqlstring = "";

			if (currentpage == 1)
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "ratelog] ORDER BY [id] DESC";
			}
			else
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "ratelog]  WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "ratelog] ORDER BY [id] DESC) AS tblTmp )  ORDER BY [id] DESC";
			}

			sqlstring = "SELECT r.*,p.[title] AS title,p.[poster] AS poster , p.[posterid] AS posterid,  ug.[grouptitle] AS grouptitle FROM (" + sqlstring + ") r LEFT JOIN [" + posttablename + "] p ON r.[pid] = p.[pid] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] u ON u.[uid] = r.[uid] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "usergroups] ug ON ug.[groupid] = u.[groupid]";


			return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];

		}

		/// <summary>
		/// 得到当前指定条件和页数的评分日志记录(表)
		/// </summary>
		/// <param name="pagesize">当前分页的尺寸大小</param>
		/// <param name="currentpage">当前页码</param>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public DataTable RateLogList(int pagesize, int currentpage, string posttablename, string condition)
		{
			int pagetop = (currentpage - 1) * pagesize;
			string sqlstring = "";

			if (currentpage == 1)
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "ratelog] WHERE " + condition + "  ORDER BY [id] DESC";
			}
			else
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " * FROM [" + BaseConfigs.GetTablePrefix + "ratelog]  WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "ratelog] WHERE " + condition + " ORDER BY [id] DESC) AS tblTmp )  AND " + condition + " ORDER BY [id] DESC";
			}

			sqlstring = "SELECT r.*,p.[title] AS title,p.[poster] AS poster , p.[posterid] AS posterid,  ug.[grouptitle] AS grouptitle FROM (" + sqlstring + ") r LEFT JOIN [" + posttablename + "] p ON r.[pid] = p.[pid] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] u ON u.[uid] = r.[uid] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "usergroups] ug ON ug.[groupid] = u.[groupid]";


			return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];

		}

		/// <summary>
		/// 得到评分日志记录数
		/// </summary>
		/// <returns></returns>
		public int GetRateLogCount()
		{
			return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "ratelog]").Tables[0].Rows[0][0].ToString());
		}

		/// <summary>
		/// 得到指定查询条件下的评分日志数
		/// </summary>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public int GetRateLogCount(string condition)
		{
			return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT([id]) FROM [" + BaseConfigs.GetTablePrefix + "ratelog] WHERE " + condition).Tables[0].Rows[0][0].ToString());
		}

		public int GetPostsCount(string posttableid)
		{
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pid]) AS [portscount] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "]"), 0);
		}

		public IDataReader GetMaxAndMinTid(int fid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid)
								  };

			return DbHelper.ExecuteReader(CommandType.Text, "SELECT MAX([tid]) AS [maxtid],MIN([tid]) AS [mintid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid] IN (SELECT [fid] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=@fid OR (CHARINDEX(',' + RTRIM(@fid) + ',', ',' + RTRIM(parentidlist) + ',') > 0))", prams);
		}

		public int GetPostCount(int fid, string posttablename)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid)
								  };

			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pid]) AS [postcount] FROM [" + posttablename + "] WHERE [fid] = @fid", prams), 0);
		}

		public int GetPostCountByTid(int tid, string posttablename)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid)
								  };

			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pid]) AS [postcount] FROM [" + posttablename + "] WHERE [tid] = @tid AND [layer] <> 0", prams), 0);
		}

		public int GetPostCount(string posttableid, int tid, int posterid)
		{
			string posttablename = string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, posttableid);
			string sqlstr = string.Format("[{0}].[tid]={1} AND [{0}].[posterid]={2}", posttablename, tid, posterid);
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@condition",(DbType)SqlDbType.NChar,100,sqlstr),
									  DbHelper.MakeInParam("@posttablename", (DbType)SqlDbType.VarChar, 20, string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, posttableid))
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}getpostcountbycondition", BaseConfigs.GetTablePrefix), prams), 0);
		}

		public int GetTodayPostCount(int fid, string posttablename)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid)
								  };

			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pid]) AS [postcount] FROM [" + posttablename + "] WHERE [fid] = @fid AND DATEDIFF(day, [postdatetime], GETDATE()) = 0 ", prams), 0);
		}

		public int GetPostCount(int fid, int posttableid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid)
								  };

			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pid]) AS [postcount] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE  [fid] IN (SELECT [fid] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=@fid OR (CHARINDEX(',' + RTRIM(@fid) + ',', ',' + RTRIM(parentidlist) + ',') > 0))", prams), 0);
		}

		public int GetTodayPostCount(int fid, int posttableid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid)
								  };

			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pid]) AS [postcount] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE  [fid] IN (SELECT [fid] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid]=@fid OR (CHARINDEX(',' + RTRIM(@fid) + ',', ',' + RTRIM(parentidlist) + ',') > 0)) AND DATEDIFF(day, [postdatetime], GETDATE()) = 0 ", prams), 0);
		}

		public IDataReader GetMaxAndMinTidByUid(int uid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
								  };

			return DbHelper.ExecuteReader(CommandType.Text, "SELECT MAX([tid]) AS [maxtid],MIN([tid]) AS [mintid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [posterid] = @uid", prams);
		}

		public int GetPostCountByUid(int uid, string posttablename)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
								  };

			return Math.Abs(Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pid]) AS [postcount] FROM [" + posttablename + "] WHERE [posterid] = @uid", prams), 0));
		}

		public int GetTodayPostCountByUid(int uid, string posttablename)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
								  };

			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pid]) AS [postcount] FROM [" + posttablename + "] WHERE [posterid] = @uid AND DATEDIFF(day, [postdatetime], GETDATE()) = 0 ", prams), 0);
		}

		public int GetTopicsCount()
		{
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([tid]) AS [topicscount] FROM [" + BaseConfigs.GetTablePrefix + "topics]"), 0);
		}

		public void ReSetStatistic(int UserCount, int TopicsCount, int PostCount, string lastuserid, string lastusername)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@totaltopic", (DbType)SqlDbType.Int, 4, TopicsCount),
									  DbHelper.MakeInParam("@totalpost", (DbType)SqlDbType.Int, 4, PostCount),
									  DbHelper.MakeInParam("@totalusers", (DbType)SqlDbType.Int, 4, UserCount),
									  DbHelper.MakeInParam("@lastusername", (DbType)SqlDbType.VarChar, 20, lastusername),
									  DbHelper.MakeInParam("@lastuserid", (DbType)SqlDbType.Int, 4, Utils.StrToInt(lastuserid, 0))

								  };

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totaltopic]=@totaltopic,[totalpost]=@totalpost,[totalusers]=@totalusers,[lastusername]=@lastusername,[lastuserid]=@lastuserid", prams);
		}

		public IDataReader GetTopicTids(int statcount, int lasttid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@lasttid", (DbType)SqlDbType.Int, 4, lasttid),
			};

			return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP " + statcount + " [tid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] > @lasttid ORDER BY [tid]", prams);

		}

		public IDataReader GetLastPost(int tid, int posttableid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid);
			return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [pid], [postdatetime], [posterid], [poster] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE [tid] = @tid ORDER BY [pid] DESC", parm);
		}

		public void UpdateTopic(int tid, int postcount, int lastpostid, string lastpost, int lastposterid, string poster)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid),
									  DbHelper.MakeInParam("@postcount", (DbType)SqlDbType.Int, 4, postcount),
									  DbHelper.MakeInParam("@lastpostid", (DbType)SqlDbType.Int, 4, lastpostid),
									  DbHelper.MakeInParam("@lastpost", (DbType)SqlDbType.VarChar, 20, lastpost),
									  DbHelper.MakeInParam("@lastposterid", (DbType)SqlDbType.Int, 4, lastposterid),
									  DbHelper.MakeInParam("@lastposter", (DbType)SqlDbType.VarChar, 20, poster)
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [lastpost]=@lastpost, [lastposterid]=@lastposterid, [lastposter]=@lastposter, [replies]=@postcount WHERE [" + BaseConfigs.GetTablePrefix + "topics].[tid] = @tid", parms);
		}

		public void UpdateTopicLastPosterId(int tid)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid)
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [lastposterid]=(SELECT ISNULL(MIN(lastpostid), -1)-1 FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [" + BaseConfigs.GetTablePrefix + "topics].[tid] = @tid", parms);
		}

		public IDataReader GetTopics(int start_tid, int end_tid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@start_tid", (DbType)SqlDbType.Int, 4, start_tid),
									  DbHelper.MakeInParam("@end_tid", (DbType)SqlDbType.Int, 4, end_tid)
								  };

			return DbHelper.ExecuteReader(CommandType.Text, "SELECT [tid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] >= @start_tid AND [tid]<=@end_tid  ORDER BY [tid]", prams);
		}

		public IDataReader GetForumLastPost(int fid, string posttablename, int topiccount, int postcount, int lasttid, string lasttitle, string lastpost, int lastposterid, string lastposter, int todaypostcount)
		{
			IDataParameter[] prams_posts = {
											DbHelper.MakeInParam("@lastfid", (DbType)SqlDbType.Int, 4, fid),
											DbHelper.MakeInParam("@topiccount", (DbType)SqlDbType.Int, 4, topiccount),
											DbHelper.MakeInParam("@postcount", (DbType)SqlDbType.Int, 4, postcount),
											DbHelper.MakeInParam("@lasttid", (DbType)SqlDbType.Int, 4, lasttid),
											DbHelper.MakeInParam("@lasttitle", (DbType)SqlDbType.NChar, 80, lasttitle),
											DbHelper.MakeInParam("@lastpost", (DbType)SqlDbType.VarChar, 20, lastpost),
											DbHelper.MakeInParam("@lastposterid", (DbType)SqlDbType.Int, 4, lastposterid),
											DbHelper.MakeInParam("@lastposter", (DbType)SqlDbType.NChar, 20, lastposter),
											DbHelper.MakeInParam("@todaypostcount", (DbType)SqlDbType.Int, 4, todaypostcount)
										};
			return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [tid], [title], [postdatetime], [posterid], [poster] FROM [" + posttablename + "] WHERE [fid] = @lastfid ORDER BY [pid] DESC", prams_posts);
		}

		public void UpdateForum(int fid, int topiccount, int postcount, int lasttid, string lasttitle, string lastpost, int lastposterid, string lastposter, int todaypostcount)
		{
			IDataParameter[] prams_posts = {
											DbHelper.MakeInParam("@lastfid", (DbType)SqlDbType.Int, 4, fid),
											DbHelper.MakeInParam("@topiccount", (DbType)SqlDbType.Int, 4, topiccount),
											DbHelper.MakeInParam("@postcount", (DbType)SqlDbType.Int, 4, postcount),
											DbHelper.MakeInParam("@lasttid", (DbType)SqlDbType.Int, 4, lasttid),
											DbHelper.MakeInParam("@lasttitle", (DbType)SqlDbType.NChar, 80, lasttitle),
											DbHelper.MakeInParam("@lastpost", (DbType)SqlDbType.VarChar, 20, lastpost),
											DbHelper.MakeInParam("@lastposterid", (DbType)SqlDbType.Int, 4, lastposterid),
											DbHelper.MakeInParam("@lastposter", (DbType)SqlDbType.NChar, 20, lastposter),
											DbHelper.MakeInParam("@todaypostcount", (DbType)SqlDbType.Int, 4, todaypostcount)
										};

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forums] SET [topics] = @topiccount, [posts]=@postcount, [todayposts] = @todaypostcount, [lasttid] = @lasttid, [lasttitle] = @lasttitle, [lastpost]=@lastpost, [lastposterid] = @lastposterid, [lastposter]=@lastposter WHERE [" + BaseConfigs.GetTablePrefix + "forums].[fid] = @lastfid", prams_posts);
		}

		public IDataReader GetForums(int start_fid, int end_fid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@start_fid", (DbType)SqlDbType.Int, 4, start_fid),
									  DbHelper.MakeInParam("@end_fid", (DbType)SqlDbType.Int, 4, end_fid)
								  };

			return DbHelper.ExecuteReader(CommandType.Text, "SELECT  [fid] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [fid] >= @start_fid AND [fid]<=@end_fid", prams);
		}

		/// <summary>
		/// 清除主题里面已经移走的主题
		/// </summary>
		public void ReSetClearMove()
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [closed] > 1");
		}

		public IDataReader GetLastPostByFid(int fid, string posttablename)
		{
			IDataParameter parm = DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid);
			return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [tid], [title], [postdatetime], [posterid], [poster] FROM [" + posttablename + "] WHERE [fid] = @fid ORDER BY [pid] DESC", parm);
		}

		/// <summary>
		/// 创建一个投票
		/// </summary>
		/// <param name="tid">关联的主题id</param>
		/// <param name="polltype">投票类型, 0为单选, 1为多选</param>
		/// <param name="itemcount">投票项总数</param>
		/// <param name="itemnamelist">投票项目列表</param>
		/// <param name="itemvaluelist">投票项目结果列表</param>
		/// <param name="enddatetime">截止日期</param>
		/// <returns>成功则返回true, 否则返回false</returns>
		public bool CreatePoll(int tid, int polltype, int itemcount, string itemnamelist, string itemvaluelist, string enddatetime, int userid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid),
									  DbHelper.MakeInParam("@polltype",(DbType)SqlDbType.Int,4,polltype),
									  DbHelper.MakeInParam("@itemcount",(DbType)SqlDbType.Int,4,itemcount),
									  DbHelper.MakeInParam("@itemnamelist",(DbType)SqlDbType.NText,0,itemnamelist),
									  DbHelper.MakeInParam("@itemvaluelist",(DbType)SqlDbType.Text,0,itemvaluelist),
									  DbHelper.MakeInParam("@usernamelist",(DbType)SqlDbType.NText,0,""),
									  DbHelper.MakeInParam("@enddatetime",(DbType)SqlDbType.VarChar,19,enddatetime),
									  DbHelper.MakeInParam("@userid",(DbType)SqlDbType.Int,4,userid)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "createpoll", prams) > 0;
		}

		public bool UpdatePoll(int tid, int polltype, int itemcount, string itemnamelist, string itemvaluelist, string enddatetime)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid),
									  DbHelper.MakeInParam("@polltype",(DbType)SqlDbType.Int,4,polltype),
									  DbHelper.MakeInParam("@itemcount",(DbType)SqlDbType.Int,4,itemcount),
									  DbHelper.MakeInParam("@itemnamelist",(DbType)SqlDbType.Text,0,itemnamelist),
									  DbHelper.MakeInParam("@itemvaluelist",(DbType)SqlDbType.Text,0,itemvaluelist),
									  DbHelper.MakeInParam("@enddatetime",(DbType)SqlDbType.VarChar,19,enddatetime),
			};
			return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updatepoll", prams) > 0;
		}

		/// <summary>
		/// 获得投票信息
		/// </summary>
		/// <param name="tid"></param>
		/// <returns></returns>
		public IDataReader GetPoll(int tid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
								  };
			IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [itemvaluelist], [usernamelist] FROM [" + BaseConfigs.GetTablePrefix + "polls] WHERE [tid]=@tid", prams);
			return reader;
		}

		/// <summary>
		/// 根据投票信息更新数据库中的记录
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <param name="selitemidlist">选择的投票项id列表</param>
		/// <param name="username">用户名</param>
		/// <returns>如果执行成功则返回0, 非法提交返回负值</returns>
		public int UpdatePoll(int tid, string usernamelist, StringBuilder newselitemidlist)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@itemvaluelist",(DbType)SqlDbType.Text,0,newselitemidlist.ToString().Trim()),
									  DbHelper.MakeInParam("@usernamelist",(DbType)SqlDbType.Text,0,usernamelist.ToString().Trim()),
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
								  };
			if (DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "polls] SET [itemvaluelist]=@itemvaluelist, [usernamelist]=@usernamelist WHERE [tid]=@tid", prams) > 0)
			{
				return 0;
			}
			else
			{
				return -4;
			}
		}

		/// <summary>
		/// 获得与指定主题id关联的投票数据
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <returns>投票数据</returns>
		public DataTable GetPollList(int tid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
								  };
			DataTable dtTemp = DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getpoll", prams).Tables[0];
			return dtTemp;
		}

		/// <summary>
		/// 获得投票的用户名
		/// </summary>
		/// <param name="tid">主题Id</param>
		/// <returns></returns>
		public string GetPollUserNameList(int tid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int, 4, tid)
								  };

			string strUsernamelist = DbHelper.ExecuteScalarToStr(CommandType.Text, "SELECT TOP 1 [usernamelist] FROM [" + BaseConfigs.GetTablePrefix + "polls] WHERE [tid]=@tid", prams);
			return strUsernamelist;
		}

		/// <summary>
		/// 得到投票帖的投票类型
		/// </summary>
		/// <param name="tid">主题ＩＤ</param>
		/// <returns>投票类型</returns>
		public int GetPollType(int tid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT TOP 1 [polltype] FROM [" + BaseConfigs.GetTablePrefix + "polls] WHERE [tid]=@tid", prams), 0);
		}

		/// <summary>
		/// 得到投票帖的结束时间
		/// </summary>
		/// <param name="tid">主题ＩＤ</param>
		/// <returns>结束时间</returns>
		public string GetPollEnddatetime(int tid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
								  };
			return Utils.GetDate(DbHelper.ExecuteScalarToStr(CommandType.Text, "SELECT TOP 1 [enddatetime] FROM [" + BaseConfigs.GetTablePrefix + "polls] WHERE [tid]=@tid", prams), Utils.GetDate());
		}



		/// <summary>
		/// 得到用户帖子分表信息
		/// </summary>
		/// <returns>分表记录集</returns>
		public DataSet GetAllPostTableName()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM [{0}tablelist] ORDER BY [id] DESC", BaseConfigs.GetTablePrefix));
		}

		/// <summary>
		/// 创建帖子
		/// </summary>
		/// <param name="postinfo">帖子信息类</param>
		/// <returns>返回帖子id</returns>
		public int CreatePost(PostInfo postinfo, string posttableid)
		{
			IDataParameter[] prams = {

									  DbHelper.MakeInParam("@fid",(DbType)SqlDbType.SmallInt,2,postinfo.Fid),
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,postinfo.Tid),
									  DbHelper.MakeInParam("@parentid",(DbType)SqlDbType.Int,4,postinfo.Parentid),
									  DbHelper.MakeInParam("@layer",(DbType)SqlDbType.Int,4,postinfo.Layer),
									  DbHelper.MakeInParam("@poster",(DbType)SqlDbType.VarChar,15,postinfo.Poster),
									  DbHelper.MakeInParam("@posterid",(DbType)SqlDbType.Int,4,postinfo.Posterid),
									  DbHelper.MakeInParam("@title",(DbType)SqlDbType.NVarChar,60,postinfo.Title),
									  DbHelper.MakeInParam("@postdatetime",(DbType)SqlDbType.SmallDateTime,4, DateTime.Parse(postinfo.Postdatetime)),
									  DbHelper.MakeInParam("@message",(DbType)SqlDbType.NText,0,postinfo.Message),
									  DbHelper.MakeInParam("@ip",(DbType)SqlDbType.VarChar,15,postinfo.Ip),
									  DbHelper.MakeInParam("@lastedit",(DbType)SqlDbType.NVarChar,50,postinfo.Lastedit),
									  DbHelper.MakeInParam("@invisible",(DbType)SqlDbType.Int,4,postinfo.Invisible),
									  DbHelper.MakeInParam("@usesig",(DbType)SqlDbType.Int,4,postinfo.Usesig),
									  DbHelper.MakeInParam("@htmlon",(DbType)SqlDbType.Int,4,postinfo.Htmlon),
									  DbHelper.MakeInParam("@smileyoff",(DbType)SqlDbType.Int,4,postinfo.Smileyoff),
									  DbHelper.MakeInParam("@bbcodeoff",(DbType)SqlDbType.Int,4,postinfo.Bbcodeoff),
									  DbHelper.MakeInParam("@parseurloff",(DbType)SqlDbType.Int,4,postinfo.Parseurloff),
									  DbHelper.MakeInParam("@attachment",(DbType)SqlDbType.Int,4,postinfo.Attachment),
									  DbHelper.MakeInParam("@rate",(DbType)SqlDbType.SmallInt,2,postinfo.Rate),
									  DbHelper.MakeInParam("@ratetimes",(DbType)SqlDbType.Int,4,postinfo.Ratetimes)
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}createpost{1}", BaseConfigs.GetTablePrefix, posttableid), prams).ToString(), -1);

		}

		/// <summary>
		/// 更新指定帖子信息
		/// </summary>
		/// <param name="__postsInfo">帖子信息</param>
		/// <returns>更新数量</returns>
		public int UpdatePost(PostInfo __postsInfo, string posttableid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@pid",(DbType)SqlDbType.Int,4,__postsInfo.Pid),
									  DbHelper.MakeInParam("@title",(DbType)SqlDbType.NVarChar,160,__postsInfo.Title),
									  DbHelper.MakeInParam("@message",(DbType)SqlDbType.NText,0,__postsInfo.Message),
									  DbHelper.MakeInParam("@lastedit",(DbType)SqlDbType.VarChar,50,__postsInfo.Lastedit),
									  DbHelper.MakeInParam("@invisible",(DbType)SqlDbType.Int,4,__postsInfo.Invisible),
									  DbHelper.MakeInParam("@usesig",(DbType)SqlDbType.Int,4,__postsInfo.Usesig),
									  DbHelper.MakeInParam("@htmlon",(DbType)SqlDbType.Int,4,__postsInfo.Htmlon),
									  DbHelper.MakeInParam("@smileyoff",(DbType)SqlDbType.Int,4,__postsInfo.Smileyoff),
									  DbHelper.MakeInParam("@bbcodeoff",(DbType)SqlDbType.Int,4,__postsInfo.Bbcodeoff),
									  DbHelper.MakeInParam("@parseurloff",(DbType)SqlDbType.Int,4,__postsInfo.Parseurloff),
			};
			return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updatepost" + posttableid, prams);
		}

		/// <summary>
		/// 删除指定ID的帖子
		/// </summary>
		/// <param name="pid">帖子ID</param>
		/// <returns>删除数量</returns>
		public int DeletePost(string posttableid, int pid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@pid",(DbType)SqlDbType.Int,4,pid)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}deletepost{1}bypid", BaseConfigs.GetTablePrefix, posttableid), prams);
		}

		/// <summary>
		/// 获得指定的帖子描述信息
		/// </summary>
		/// <param name="pid">帖子id</param>
		/// <returns>帖子描述信息</returns>
		public IDataReader GetPostInfo(string posttableid, int pid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int,4, pid),
			};
			return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT TOP 1 * FROM [{0}posts{1}] WHERE [pid]=@pid", BaseConfigs.GetTablePrefix, posttableid), prams);
		}

		/// <summary>
		/// 获得指定主题的帖子列表
		/// </summary>
		/// <param name="topiclist">主题ID列表</param>
		/// <returns></returns>
		public DataSet GetPostList(string topiclist, string[] posttableid)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < posttableid.Length; i++)
			{
				if (sb.Length > 0)
				{
					sb.Append(" UNION ALL ");
				}
				sb.Append("SELECT * FROM [");
				sb.Append(BaseConfigs.GetTablePrefix);
				sb.Append("posts");
				sb.Append(posttableid[i]);
				sb.Append("] WHERE [tid] IN (");
				sb.Append(topiclist);
				sb.Append(")");
			}

			DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, sb.ToString());
			return ds;
		}

		/// <summary>
		/// 获取指定条件的帖子DataSet
		/// </summary>
		/// <param name="_postpramsinfo">参数列表</param>
		/// <returns>指定条件的帖子DataSet</returns>
		public DataTable GetPostListTitle(int Tid, string posttablename)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,Tid)	
								  };
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [pid], [title], [poster], [posterid],[message] FROM [" + posttablename + "] WHERE [tid]=@tid ORDER BY [pid]", prams).Tables[0];

		}

		/// <summary>
		/// 获取指定条件的帖子DataReader
		/// </summary>
		/// <param name="_postpramsinfo">参数列表</param>
		/// <returns>指定条件的帖子DataReader</returns>
		public IDataReader GetPostListByCondition(PostpramsInfo _postpramsinfo, string posttablename)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,_postpramsinfo.Tid),
									  DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,_postpramsinfo.Pagesize),
									  DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,_postpramsinfo.Pageindex),
									  DbHelper.MakeInParam("@condition",(DbType)SqlDbType.NVarChar,100,_postpramsinfo.Condition),
									  DbHelper.MakeInParam("@posttablename",(DbType)SqlDbType.VarChar,20,posttablename)
								  };
			return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getpostlistbycondition", prams);
		}

		/// <summary>
		/// 获取指定条件的帖子DataReader
		/// </summary>
		/// <param name="_postpramsinfo">参数列表</param>
		/// <returns>指定条件的帖子DataReader</returns>
		public IDataReader GetPostList(PostpramsInfo _postpramsinfo, string posttablename)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,_postpramsinfo.Tid),
									  DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,_postpramsinfo.Pagesize),
									  DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,_postpramsinfo.Pageindex),
									  DbHelper.MakeInParam("@posttablename",(DbType)SqlDbType.VarChar,20,posttablename)
								  };
			return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getpostlist", BaseConfigs.GetTablePrefix), prams);
		}

		/// <summary>
		/// 返回指定主题的最后回复帖子
		/// </summary>
		/// <param name="tid"></param>
		/// <returns></returns>
		public DataTable GetLastPostByTid(int tid, string posttablename)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid),
			};
			DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP 1 * FROM {0} WHERE [tid] = @tid ORDER BY [pid] DESC", posttablename), prams);

			if (ds != null)
			{
				if (ds.Tables.Count > 0)
				{
					return ds.Tables[0];
				}
			}
			return new DataTable();
		}

		/// <summary>
		/// 获得最后回复的帖子列表
		/// </summary>
		/// <param name="_postpramsinfo">参数列表</param>
		/// <returns>帖子列表</returns>
		public DataTable GetLastPostList(PostpramsInfo _postpramsinfo, string posttablename)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,_postpramsinfo.Tid),
									  DbHelper.MakeInParam("@postnum",(DbType)SqlDbType.Int,4,_postpramsinfo.Pagesize),
									  DbHelper.MakeInParam("@posttablename",(DbType)SqlDbType.VarChar,20,posttablename)
								  };
			DataTable dt = DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getlastpostlist", BaseConfigs.GetTablePrefix), prams).Tables[0];

			return dt;
		}

		/// <summary>
		/// 获得单个帖子的信息, 包括发帖人的一般资料
		/// </summary>
		/// <param name="_postpramsinfo">参数列表</param>
		/// <returns>帖子的信息</returns>
		public IDataReader GetSinglePost(out IDataReader _Attachments,PostpramsInfo _postpramsinfo, string posttableid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@pid",(DbType)SqlDbType.Int,4,_postpramsinfo.Pid),
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,_postpramsinfo.Tid)
								  };
			_Attachments = null;
			IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getsinglepost{1}", BaseConfigs.GetTablePrefix, posttableid), prams);

			return reader;
		}

		public DataTable GetPostTree(int tid, string posttableid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
								  };
			return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getpost{1}tree", BaseConfigs.GetTablePrefix, posttableid), prams).Tables[0];

		}

		/// <summary>
		/// 按条件获取指定tid的帖子总数
		/// </summary>
		/// <param name="tid">帖子的tid</param>
		/// <returns>指定tid的帖子总数</returns>
		public int GetPostCount(int tid, string condition, string posttableid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@condition",(DbType)SqlDbType.NChar,100,condition),
									  DbHelper.MakeInParam("@posttablename", (DbType)SqlDbType.VarChar, 20, string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, posttableid))
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}getpostcountbycondition", BaseConfigs.GetTablePrefix), prams), 0);

		}

		/// <summary>
		/// 获得指定主题的第一个帖子的id
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <returns>帖子id</returns>
		public int GetFirstPostId(int tid, string posttableid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}getfirstpost{1}id", BaseConfigs.GetTablePrefix, posttableid), prams).ToString(), -1);

		}

		/// <summary>
		/// 判断指定用户是否是指定主题的回复者
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <param name="uid">用户id</param>
		/// <returns>是否是指定主题的回复者</returns>
		public bool IsReplier(int tid, int uid, string posttableid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT([pid]) AS [pidcount] FROM [{0}posts{1}] WHERE [tid] = @tid AND [posterid]=@uid AND @uid>0", BaseConfigs.GetTablePrefix, posttableid), prams), 0) > 0;
		}

		/// <summary>
		/// 更新帖子的评分值
		/// </summary>
		/// <param name="tid">主题ID</param>
		/// <param name="postidlist">帖子ID列表</param>
		/// <returns>更新的帖子数量</returns>
		public int UpdatePostRate(int tid, string postidlist, string posttableid)
		{
			return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}posts{1}] SET [ratetimes] = [ratetimes] + 1 WHERE [pid] IN ({2})", BaseConfigs.GetTablePrefix, posttableid, postidlist));
		}

		/// <summary>
		/// 更新帖子的评分值
		/// </summary>
		/// <param name="postidlist"></param>
		/// <param name="posttableid"></param>
		/// <returns></returns>
		public int UpdatePostRate(int pid, float rate, string posttableid)
		{
			return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}posts{1}] SET [rate] = [rate] + {2} WHERE [pid] IN ({3})", BaseConfigs.GetTablePrefix, posttableid, rate, pid));
		}

		/// <summary>
		/// 撤消帖子的评分值
		/// </summary>
		/// <param name="postidlist"></param>
		/// <param name="posttableid"></param>
		/// <returns></returns>
		public int CancelPostRate(string postidlist, string posttableid)
		{
			return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}posts{1}] SET [rate] = 0, [ratetimes]=0 WHERE [pid] IN ({2})", BaseConfigs.GetTablePrefix, posttableid, postidlist));
		}

		/// <summary>
		/// 获取帖子评分列表
		/// </summary>
		/// <param name="pid">帖子列表</param>
		/// <returns></returns>
		public DataTable GetPostRateList(int pid, int displayRateCount)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@pid",(DbType)SqlDbType.Int,4,pid)
								  };
			string commandText = string.Format("SELECT TOP {0} * FROM [{1}ratelog] WHERE [pid]=@pid ORDER BY [id] DESC", displayRateCount, BaseConfigs.GetTablePrefix);
			return DbHelper.ExecuteDataset(CommandType.Text, commandText, prams).Tables[0];
		}

		/// <summary>
		/// 获取新主题
		/// </summary>
		/// <param name="forumidlist">不允许游客访问的版块id列表</param>
		/// <returns></returns>
		public IDataReader GetNewTopics(string forumidlist)
		{
			IDataParameter parm = DbHelper.MakeInParam("@fidlist", (DbType)SqlDbType.VarChar, 500, forumidlist);

			IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getnewtopics", parm);
			return reader;
		}
        
		public IDataReader GetSitemapNewTopics(string forumidlist)
		{
			IDataParameter parm = DbHelper.MakeInParam("@fidlist", (DbType)SqlDbType.VarChar, 500, forumidlist);

			IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getsitemapnewtopics", parm);
			return reader;
		}

		/// <summary>
		/// 获取版块新主题
		/// </summary>
		/// <param name="fid">版块id</param>
		/// <returns></returns>
		public IDataReader GetForumNewTopics(int fid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int,4,fid)
									   
								  };
			return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getforumnewtopics", prams);
		}


		/// <summary>
		/// 创建搜索缓存
		/// </summary>
		/// <param name="cacheinfo">搜索缓存信息</param>
		/// <returns>搜索缓存id</returns>
		public int CreateSearchCache(SearchCacheInfo cacheinfo)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@keywords",(DbType)SqlDbType.NVarChar,255,cacheinfo.Keywords),
									  DbHelper.MakeInParam("@searchstring",(DbType)SqlDbType.NVarChar,255,cacheinfo.Searchstring),
									  DbHelper.MakeInParam("@ip",(DbType)SqlDbType.VarChar,15,cacheinfo.Ip),
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,cacheinfo.Uid),
									  DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.Int,4,cacheinfo.Groupid),
									  DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(cacheinfo.Postdatetime)),
									  DbHelper.MakeInParam("@expiration",(DbType)SqlDbType.VarChar,19,cacheinfo.Expiration),
									  DbHelper.MakeInParam("@topics",(DbType)SqlDbType.Int,4,cacheinfo.Topics),
									  DbHelper.MakeInParam("@tids",(DbType)SqlDbType.Text,0,cacheinfo.Tids)
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}createsearchcache", BaseConfigs.GetTablePrefix), prams).ToString(), -1);
		}

		/// <summary>
		/// 删除超过３０分钟的缓存记录
		/// </summary>
		public void DeleteExpriedSearchCache()
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@expiration",(DbType)SqlDbType.DateTime,8,DateTime.Now.AddMinutes(-30).ToString("yyyy-MM-dd HH:mm:ss"))
								  };

			DbHelper.ExecuteNonQuery(CommandType.Text, string.Format(@"DELETE FROM [{0}searchcaches] WHERE [expiration]<@expiration", BaseConfigs.GetTablePrefix), prams);
		}

		/// <summary>
		/// 获得搜索缓存
		/// </summary>
		/// <returns></returns>
		public DataTable GetSearchCache(int searchid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@searchid",(DbType)SqlDbType.Int,4,searchid)
								  };
			return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP 1 [tids] FROM [{0}searchcaches] WHERE [searchid]=@searchid", BaseConfigs.GetTablePrefix), prams).Tables[0];
		}

		/// <summary>
		/// 获得搜索的精华贴
		/// </summary>
		/// <param name="pagesize"></param>
		/// <param name="strTids"></param>
		/// <returns></returns>
		public DataTable GetSearchDigestTopicsList(int pagesize, string strTids)
		{
			string commandText = string.Format("SELECT TOP {1} [{0}topics].[tid], [{0}topics].[title], [{0}topics].[poster], [{0}topics].[posterid], [{0}topics].[postdatetime], [{0}topics].[replies], [{0}topics].[views], [{0}topics].[lastpost],[{0}topics].[lastposter], [{0}forums].[fid],[{0}forums].[name] AS [forumname] FROM [{0}topics] LEFT JOIN [{0}forums] ON [{0}forums].[fid] = [{0}topics].[fid] WHERE [{0}topics].[tid] IN({2}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[{0}topics].[tid]),'{2}')", BaseConfigs.GetTablePrefix, pagesize, strTids);
			return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
		}

		/// <summary>
		/// 获得按帖子搜索的主题列表
		/// </summary>
		/// <param name="pagesize"></param>
		/// <param name="strTids"></param>
		/// <returns></returns>
		public DataTable GetSearchPostsTopicsList(int pagesize, string strTids, string postablename)
		{
			string commandText = string.Format("SELECT TOP {1} [{2}].[tid], [{2}].[title], [{2}].[poster], [{2}].[posterid], [{2}].[postdatetime],[{2}].[lastedit], [{2}].[rate], [{2}].[ratetimes], [{0}forums].[fid],[{0}forums].[name] AS [forumname] FROM [{2}] LEFT JOIN [{0}forums] ON [{0}forums].[fid] = [{2}].[fid] WHERE [{2}].[pid] IN({3}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[{2}].[tid]),'{3}')", BaseConfigs.GetTablePrefix, pagesize, postablename, strTids);
			return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
		}

		/// <summary>
		/// 按搜索获得主题列表
		/// </summary>
		/// <param name="pagesize"></param>
		/// <param name="strTids"></param>
		/// <returns></returns>
		public DataTable GetSearchTopicsList(int pagesize, string strTids)
		{
			string commandText = string.Format("SELECT TOP {1} [{0}topics].[tid], [{0}topics].[title], [{0}topics].[poster], [{0}topics].[posterid], [{0}topics].[postdatetime], [{0}topics].[replies], [{0}topics].[views], [{0}topics].[lastpost],[{0}topics].[lastposter], [{0}forums].[fid],[{0}forums].[name] AS [forumname] FROM [{0}topics] LEFT JOIN [{0}forums] ON [{0}forums].[fid] = [{0}topics].[fid] WHERE [{0}topics].[tid] IN({2}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[{0}topics].[tid]),'{2}')", BaseConfigs.GetTablePrefix, pagesize, strTids);
			return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
		}

		/// <summary>
		/// 开启全文索引
		/// </summary>
		public void ConfirmFullTextEnable()
		{
			string commandText = "IF(SELECT DATABASEPROPERTY(DB_NAME(), 'IsFullTextEnabled'))=0 EXEC sp_fulltext_database 'enable'";
			DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
		}

		/// <summary>
		/// 设置主题指定字段的属性值
		/// </summary>
		/// <param name="topiclist">要设置的主题列表</param>
		/// <param name="field">要设置的字段</param>
		/// <param name="intValue">属性值</param>
		/// <returns>更新主题个数</returns>
		public int SetTopicStatus(string topiclist, string field, int intValue)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@field", (DbType)SqlDbType.Int, 1, intValue)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}topics] SET [{1}] = @field WHERE [tid] IN ({2})", BaseConfigs.GetTablePrefix, field, topiclist), prams);
		}

		/// <summary>
		/// 设置主题指定字段的属性值
		/// </summary>
		/// <param name="topiclist">要设置的主题列表</param>
		/// <param name="field">要设置的字段</param>
		/// <param name="intValue">属性值</param>
		/// <returns>更新主题个数</returns>
		public int SetTopicStatus(string topiclist, string field, byte intValue)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@field", (DbType)SqlDbType.TinyInt, 1, intValue)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}topics] SET [{1}] = @field WHERE [tid] IN ({2})", BaseConfigs.GetTablePrefix, field, topiclist), prams);
		}

		/// <summary>
		/// 设置主题指定字段的属性值(字符型)
		/// </summary>
		/// <param name="topiclist">要设置的主题列表</param>
		/// <param name="field">要设置的字段</param>
		/// <param name="intValue">属性值</param>
		/// <returns>更新主题个数</returns>
		public int SetTopicStatus(string topiclist, string field, string intValue)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@field", (DbType)SqlDbType.VarChar, 500, intValue)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}topics] SET [{1}] = @field WHERE [tid] IN ({2})", BaseConfigs.GetTablePrefix, field, topiclist), prams);
		}

		public DataSet GetTopTopicList(int fid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid)
								  };
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [tid],[displayorder],[fid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [displayorder] > 0 ORDER BY [fid]", prams);

		}


		public DataTable GetShortForums()
		{
			DataTable topTable = DbHelper.ExecuteDataset(CommandType.Text, "SELECT [fid],[parentid],[parentidlist], [layer], CAST('' AS VARCHAR(1000)) AS [temptidlist],CAST('' AS VARCHAR(1000)) AS [tid2list], CAST('' AS VARCHAR(1000)) AS [tidlist],CAST(0 AS INT) AS [tidcount],CAST(0 AS INT) AS [tid2count],CAST(0 AS INT) AS [tid3count] FROM [" + BaseConfigs.GetTablePrefix + "forums] ORDER BY [fid] DESC").Tables[0];

			return topTable;
		}

		public IDataReader GetUserListWithTopicList(string topiclist, int losslessdel)
		{
			IDataParameter[] prams =
						{
							DbHelper.MakeInParam("@Losslessdel", (DbType)SqlDbType.Int, 4, losslessdel)
						};
			return DbHelper.ExecuteReader(CommandType.Text, "SELECT [posterid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE DATEDIFF(day, [postdatetime], GETDATE())<@Losslessdel AND [tid] IN (" + topiclist + ")", prams);
		}

		public IDataReader GetUserListWithTopicList(string topiclist)
		{
			return DbHelper.ExecuteReader(CommandType.Text, "SELECT [posterid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] IN (" + topiclist + ")");
		}

		/// <summary>
		/// 将主题设置关闭/打开
		/// </summary>
		/// <param name="topiclist">要设置的主题列表</param>
		/// <param name="intValue">关闭/打开标志( 0 为打开,1 为关闭)</param>
		/// <returns>更新主题个数</returns>
		public int SetTopicClose(string topiclist, short intValue)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@field", (DbType)SqlDbType.TinyInt, 1, intValue)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [closed] = @field WHERE [tid] IN (" + topiclist + ") AND [closed] IN (0,1)", prams);

		}

		/// <summary>
		/// 获得主题指定字段的属性值
		/// </summary>
		/// <param name="topiclist">主题列表</param>
		/// <param name="field">要获得值的字段</param>
		/// <returns>主题指定字段的状态</returns>
		public int GetTopicStatus(string topiclist, string field)
		{
			return Utils.StrToInt(DbHelper.ExecuteDataset(CommandType.Text, "SELECT SUM(ISNULL([" + field + "],0)) AS [fieldcount] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] IN (" + topiclist + ")").Tables[0].Rows[0][0], 0);

		}

		/// <summary>
		/// 删除指定主题
		/// </summary>
		/// <param name="topiclist"></param>
		/// <param name="reval"></param>
		/// <param name="posttableid"></param>
		/// <returns></returns>
		public int DeleteTopicByTidList(string topiclist, int reval, string posttableid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tidlist", (DbType)SqlDbType.VarChar, 2000, topiclist),
									  DbHelper.MakeInParam("@posttablename", (DbType)SqlDbType.VarChar, 2000, BaseConfigs.GetTablePrefix + "posts" + posttableid)
								  };

			reval = DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "deletetopicbytidlist", prams);
			return reval;
		}

		public int DeleteClosedTopics(int fid, string topiclist)
		{
			return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid]=" + fid + " AND [closed] IN (" + topiclist + ")");
		}

		public int CopyTopicLink(int oldfid, string topiclist)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.SmallInt, 2, oldfid)
								  };

			///用户设置转移后保留原连接执行以下三步操作

			///1.向表中批量拷贝记录并将closed字段设置为原记录的tid*-1
			string sql = string.Format(@"INSERT INTO [{0}topics] (
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
					[poll],
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
					[poll],
					[attachment],
					[moderated],
					[hide],
					[lastpostid],
					[magic],
					[tid] AS [closed] FROM [{0}topics] WHERE [tid] IN ({1})", BaseConfigs.GetTablePrefix, topiclist);

			return DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}


		public void UpdatePost(string topiclist, int fid, string posttable)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 1, fid)
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + posttable + "] SET [fid]=@fid WHERE [tid] IN (" + topiclist + ")", prams);
		}
		/// <summary>
		/// 更新主题所属版块,会将主题分类至为0
		/// </summary>
		/// <param name="topiclist"></param>
		/// <param name="fid"></param>
		/// <returns></returns>
		public int UpdateTopic(string topiclist, int fid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.SmallInt, 1, fid)
								  };
			//更新主题
			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [fid]=@fid, [typeid]=0 WHERE [tid] IN (" + topiclist + ")", prams);
		}


		public void UpdatePostTid(string postidlist, int tid, string posttableid)
		{
			IDataParameter[] prams =
					{
						DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid)
					};

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] SET [tid]=@tid WHERE [pid] IN (" + postidlist + ")", prams);
		}


		public void SetPrimaryPost(string subject, int tid, string[] postid, string posttableid)
		{
			IDataParameter[] prams1 =
					{
						DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 4, Utils.StrToInt(postid[0], 0)),
						DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 80, subject)
					};

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] SET [title] = @title, [parentid] = [pid],[layer] = 0 WHERE [pid] = @pid", prams1);
		}

		public void SetNewTopicProperty(int topicid, int Replies, int lastpostid, int lastposterid, string lastposter, DateTime lastpost)
		{
			IDataParameter[] prams2 =
					{
						DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, topicid),
						DbHelper.MakeInParam("@lastpostid", (DbType)SqlDbType.Int, 4, lastpostid),
						DbHelper.MakeInParam("@lastposterid", (DbType)SqlDbType.Int, 4, lastposterid),
						DbHelper.MakeInParam("@lastpost", (DbType)SqlDbType.DateTime, 8, lastpost),
						DbHelper.MakeInParam("@lastposter", (DbType)SqlDbType.NChar, 20, lastposter),
						DbHelper.MakeInParam("@replies", (DbType)SqlDbType.Int, 4, Replies)

					};
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [lastpostid]=@lastpostid,[lastposterid] = @lastposterid, [lastpost] = @lastpost, [lastposter] = @lastposter, [replies] = @replies WHERE [tid] = @tid", prams2);
		}

		public int UpdatePostTidToAnotherTopic(int oldtid, int newtid, string posttableid)
		{
			IDataParameter[] prams0 =
				{
					DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, newtid),
					DbHelper.MakeInParam("@oldtid", (DbType)SqlDbType.Int, 4, oldtid)
				};

			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] SET [tid] = @tid, [layer] = CASE WHEN [layer] = 0 THEN 1 ELSE [layer] END WHERE [tid] = @oldtid", prams0);
		}

		public int UpdateAttachmentTidToAnotherTopic(int oldtid, int newtid)
		{
			IDataParameter[] prams0 =
				{
					DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, newtid),
					DbHelper.MakeInParam("@oldtid", (DbType)SqlDbType.Int, 4, oldtid)
				};

			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "attachments] SET [tid]=@tid WHERE [tid]=@oldtid", prams0);
		}

		public int DeleteTopic(int tid)
		{
			IDataParameter[] prams1 =
				{
					DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid)
				};
			return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] = @tid", prams1);

		}

		public int UpdateTopic(int tid, TopicInfo __topicinfo)
		{
			IDataParameter[] prams =
						{
							DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid),
							DbHelper.MakeInParam("@lastpostid", (DbType)SqlDbType.Int, 4, __topicinfo.Lastpostid),
							DbHelper.MakeInParam("@lastposterid", (DbType)SqlDbType.Int, 4, __topicinfo.Lastposterid),
							DbHelper.MakeInParam("@lastpost", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(__topicinfo.Lastpost)),
							DbHelper.MakeInParam("@lastposter", (DbType)SqlDbType.NChar, 20, __topicinfo.Lastposter),
							DbHelper.MakeInParam("@replies", (DbType)SqlDbType.Int, 4, __topicinfo.Replies)
						};
			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [lastpostid] = @lastpostid,[lastposterid] = @lastposterid, [lastpost] = @lastpost, [lastposter] = @lastposter, [replies] = [replies] + @replies WHERE [tid] = @tid", prams);
		}

		public int UpdateTopicReplies(int tid, int topicreplies)
		{
			IDataParameter[] prams =
						{
							DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid),
							DbHelper.MakeInParam("@replies", (DbType)SqlDbType.Int, 4, topicreplies)
						};
			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [replies] = [replies] + @replies WHERE [tid] = @tid", prams);
		}

		public int RepairTopics(string topiclist, string posttable)
		{
			string commandtext = "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [lastpost] = (SELECT TOP 1 [postdatetime] FROM [" + posttable + "] WHERE [" + BaseConfigs.GetTablePrefix + "topics].[tid] = [" + posttable + "].[tid]),"
				+ "[lastpostid] = (SELECT TOP 1 [pid] FROM [" + posttable + "] WHERE [" + BaseConfigs.GetTablePrefix + "topics].[tid] = [" + posttable + "].[tid]),"
				+ " [lastposter] = (SELECT TOP 1 [poster] FROM [" + posttable + "] WHERE [" + BaseConfigs.GetTablePrefix + "topics].[tid] = [" + posttable + "].[tid]),"
				+ "[lastposterid] = (SELECT TOP 1 [posterid] FROM [" + posttable + "] WHERE [" + BaseConfigs.GetTablePrefix + "topics].[tid] = [" + posttable + "].[tid]),"
				+ " [replies] = (SELECT COUNT([pid]) FROM [" + posttable + "] WHERE [" + BaseConfigs.GetTablePrefix + "topics].[tid] = [" + posttable + "].[tid]) - 1 "
				+ " WHERE [" + BaseConfigs.GetTablePrefix + "topics].[tid] IN (" + topiclist + ")";

			return DbHelper.ExecuteNonQuery(CommandType.Text, commandtext);
		}

		public IDataReader GetUserListWithPostList(string postlist, string posttableid)
		{
			return DbHelper.ExecuteReader(CommandType.Text, "SELECT [posterid] FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE [pid] in (" + postlist + ")");
		}

		public string CheckRateState(int userid, string pid)
		{
			IDataParameter[] prams =
					{
						DbHelper.MakeInParam("@pid", (DbType)SqlDbType.Int, 4, Utils.StrToFloat(pid, 0)),
						DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userid)
					};
			return DbHelper.ExecuteScalarToStr(CommandType.Text, "SELECT [pid] FROM [" + BaseConfigs.GetTablePrefix + "ratelog] WHERE [pid] = @pid AND [uid] = @uid", prams);
		}

		public IDataReader GetTopicListModeratorLog(int tid)
		{
			IDataParameter[] prams =
					{
						DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int, 4, tid),
			};

			return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [grouptitle], [moderatorname],[postdatetime],[actions] FROM [" + BaseConfigs.GetTablePrefix + "moderatormanagelog] WHERE [tid] = @tid ORDER BY [id] DESC", prams);
		}

		/// <summary>
		/// 重设主题类型
		/// </summary>
		/// <param name="topictypeid">主题类型</param>
		/// <param name="topiclist">要设置的主题列表</param>
		/// <returns></returns>
		public int ResetTopicTypes(int topictypeid, string topiclist)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@typeid", (DbType)SqlDbType.TinyInt, 1, topictypeid),
									  DbHelper.MakeInParam("@topiclist", (DbType)SqlDbType.NVarChar, 250, topiclist)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [typeid] = @typeid WHERE [tid] IN (" + topiclist + ")", prams);
		}

		/// <summary>
		/// 按照用户Id获取其回复过的主题总数
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public int GetTopicsCountbyReplyUserId(int userId)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userId)
								  };

			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(DISTINCT [tid]) FROM [{0}myposts] WHERE [uid] = @uid", BaseConfigs.GetTablePrefix), parms), 0);
		}


		public IDataReader GetTopicsByReplyUserId(int userId, int pageIndex, int pageSize)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userId),
									  DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int, 4, pageIndex),
									  DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int, 4, pageSize)
								  };

			IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getmyposts", parms);
			return reader;
		}
		/// <summary>
		/// 按照用户Id获取主题总数
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public int GetTopicsCountbyUserId(int userId)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userId)
								  };

			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM [{0}mytopics] WHERE [uid] = @uid", BaseConfigs.GetTablePrefix), parms), 0);
		}


		public IDataReader GetTopicsByUserId(int userId, int pageIndex, int pageSize)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userId),
									  DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int, 4, pageIndex),
									  DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int, 4, pageSize)
								  };

			IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getmytopics", parms);
			return reader;
		}

		public int CreateTopic(TopicInfo topicinfo)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.SmallInt, 2, topicinfo.Fid), 
									  DbHelper.MakeInParam("@iconid", (DbType)SqlDbType.SmallInt, 2, topicinfo.Iconid), 
									  DbHelper.MakeInParam("@title", (DbType)SqlDbType.NChar, 60, topicinfo.Title), 
									  DbHelper.MakeInParam("@typeid", (DbType)SqlDbType.SmallInt, 2, topicinfo.Typeid), 
									  DbHelper.MakeInParam("@readperm", (DbType)SqlDbType.Int, 4, topicinfo.Readperm), 
									  DbHelper.MakeInParam("@price", (DbType)SqlDbType.SmallInt, 2, topicinfo.Price), 
									  DbHelper.MakeInParam("@poster", (DbType)SqlDbType.NChar, 15, topicinfo.Poster), 
									  DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.Int, 4, topicinfo.Posterid), 
									  DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.SmallDateTime,4, DateTime.Parse(topicinfo.Postdatetime)), 
									  DbHelper.MakeInParam("@lastpost", (DbType)SqlDbType.VarChar, 0, topicinfo.Lastpost), 
									  DbHelper.MakeInParam("@lastpostid", (DbType)SqlDbType.Int, 4, topicinfo.Lastpostid),
									  DbHelper.MakeInParam("@lastposter", (DbType)SqlDbType.NChar, 15, topicinfo.Lastposter), 
									  DbHelper.MakeInParam("@views", (DbType)SqlDbType.Int, 4, topicinfo.Views), 
									  DbHelper.MakeInParam("@replies", (DbType)SqlDbType.Int, 4, topicinfo.Replies), 
									  DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, topicinfo.Displayorder), 
									  DbHelper.MakeInParam("@highlight", (DbType)SqlDbType.VarChar, 500, topicinfo.Highlight), 
									  DbHelper.MakeInParam("@digest", (DbType)SqlDbType.Int, 4, topicinfo.Digest), 
									  DbHelper.MakeInParam("@rate", (DbType)SqlDbType.Int, 4, topicinfo.Rate), 
									  DbHelper.MakeInParam("@hide", (DbType)SqlDbType.Int, 4, topicinfo.Hide), 
									  DbHelper.MakeInParam("@poll", (DbType)SqlDbType.Int, 4, topicinfo.Poll), 
									  DbHelper.MakeInParam("@attachment", (DbType)SqlDbType.Int, 4, topicinfo.Attachment), 
									  DbHelper.MakeInParam("@moderated", (DbType)SqlDbType.Int, 4, topicinfo.Moderated), 
									  DbHelper.MakeInParam("@closed", (DbType)SqlDbType.Int, 4, topicinfo.Closed),
									  DbHelper.MakeInParam("@magic", (DbType)SqlDbType.Int, 4, topicinfo.Magic)
								  };
			return Utils.StrToInt(DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "createtopic", prams).Tables[0].Rows[0][0].ToString(), -1);

		}

		/// <summary>
		/// 增加父版块的主题数
		/// </summary>
		/// <param name="fpidlist">父板块id列表</param>
		/// <param name="topics">主题数</param>
		/// <param name="posts">贴子数</param>
		public void AddParentForumTopics(string fpidlist, int topics, int posts)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@topics", (DbType)SqlDbType.Int, 4, topics)
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}forums] SET [topics] = [topics] + @topics WHERE [fid] IN ({1})", BaseConfigs.GetTablePrefix, fpidlist), prams);
		}


		public IDataReader GetTopicInfo(int tid, int fid, byte mode)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int,4, fid),
									  DbHelper.MakeInParam("@tid", (DbType)SqlDbType.Int,4, tid),
			};
			IDataReader reader;
			switch (mode)
			{
				case 1:
					reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid]=@fid AND [tid]<@tid AND [displayorder]>=0 ORDER BY [tid] DESC", prams);
					break;
				case 2:
					reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [fid]=@fid AND [tid]>@tid AND [displayorder]>=0 ORDER BY [tid] ASC", prams);
					break;
				default:
					reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid]=@tid", prams);
					break;
			}
			return reader;
		}


		public IDataReader GetTopTopics(int fid, int pagesize, int pageindex, string tids)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int,4,fid),
									  DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pagesize),
									  DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,pageindex),
									  DbHelper.MakeInParam("@tids",(DbType)SqlDbType.VarChar,500,tids)
									   
								  };
			IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "gettoptopiclist", prams);
			return reader;
		}


		public IDataReader GetTopics(int fid, int pagesize, int pageindex, int startnum, string condition)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int,4,fid),
									  DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pagesize),
									  DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int,4,pageindex),
									  DbHelper.MakeInParam("@startnum", (DbType)SqlDbType.Int,4,startnum),
									  DbHelper.MakeInParam("@condition", (DbType)SqlDbType.VarChar,80,condition)									   
								  };
			IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "gettopiclist", prams);

			return reader;
		}


		public IDataReader GetTopicsByDate(int fid, int pagesize, int pageindex, int startnum, string condition, string orderby, int ascdesc)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int,4,fid),
									  DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pagesize),
									  DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,pageindex),
									  DbHelper.MakeInParam("@startnum",(DbType)SqlDbType.Int,4,startnum),
									  DbHelper.MakeInParam("@condition",(DbType)SqlDbType.VarChar,80,condition),
									  DbHelper.MakeInParam("@orderby",(DbType)SqlDbType.VarChar,80,orderby),
									  DbHelper.MakeInParam("@ascdesc",(DbType)SqlDbType.Int,4,ascdesc)
				                    
								  };

			IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "gettopiclistbydate", prams);

			return reader;
		}


		public IDataReader GetTopicsByType(int pagesize, int pageindex, int startnum, string condition, int ascdesc)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pagesize),
									  DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,pageindex),
									  DbHelper.MakeInParam("@startnum",(DbType)SqlDbType.Int,4,startnum),
									  DbHelper.MakeInParam("@condition",(DbType)SqlDbType.VarChar,1000,condition),
									  DbHelper.MakeInParam("@ascdesc", (DbType)SqlDbType.Int, 4, ascdesc)
								  };
			IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "gettopiclistbytype", prams);

			return reader;
		}


		public IDataReader GetTopicsByTypeDate(int pagesize, int pageindex, int startnum, string condition, string orderby, int ascdesc)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pagesize),
									  DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,pageindex),
									  DbHelper.MakeInParam("@startnum",(DbType)SqlDbType.Int,4,startnum),
									  DbHelper.MakeInParam("@condition",(DbType)SqlDbType.VarChar,1000,condition),
									  DbHelper.MakeInParam("@orderby",(DbType)SqlDbType.VarChar,80,orderby),
									  DbHelper.MakeInParam("@ascdesc",(DbType)SqlDbType.Int,4,ascdesc)
				                    
								  };

			IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "gettopiclistbytypedate", prams);

			return reader;
		}


		public DataTable GetTopicList(string topiclist, int displayorder)
		{
			string commandText = string.Format("SELECT * FROM [{0}topics] WHERE [displayorder]>{1} AND [tid] IN ({2})", BaseConfigs.GetTablePrefix, displayorder, topiclist);
			DataSet ds = DbHelper.ExecuteDataset(CommandType.Text, commandText);
			if (ds != null)
			{
				if (ds.Tables.Count > 0)
				{
					return ds.Tables[0];
				}
			}

			return null;
		}

		/// <summary>
		/// 列新主题的回复数
		/// </summary>
		/// <param name="tid">主题ID</param>
		public void UpdateTopicReplies(int tid, string posttableid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid)								   
								  };
			DbHelper.ExecuteDataset(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [replies]=(SELECT COUNT([pid]) FROM [" + BaseConfigs.GetTablePrefix + "posts" + posttableid + "] WHERE [tid]=@tid AND [invisible]=0) - 1 WHERE [displayorder]>=0 AND [tid]=@tid", prams);

		}

		/// <summary>
		/// 得到当前版块内正常(未关闭)主题总数
		/// </summary>
		/// <param name="fid">版块ID</param>
		/// <returns>主题总数</returns>
		public int GetTopicCount(int fid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int,4,fid)
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "gettopiccount", prams), 0);
		}

		/// <summary>
		/// 得到当前版块内(包括子版)正常(未关闭)主题总数
		/// </summary>
		/// <param name="fid">版块ID</param>
		/// <returns>主题总数</returns>
		public int GetAllTopicCount(int fid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int,4,fid)
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getalltopiccount", prams), 0);
		}

		/// <summary>
		/// 得到当前版块内主题总数
		/// </summary>
		/// <param name="fid">版块ID</param>
		/// <returns>主题总数</returns>
		public int GetTopicCount(int fid, int state, string condition)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@fid",(DbType)SqlDbType.Int,4,fid),
									  DbHelper.MakeInParam("@state",(DbType)SqlDbType.Int,4,state),
									  DbHelper.MakeInParam("@condition",(DbType)SqlDbType.VarChar,80,condition)
								  };
			return Utils.StrToInt(DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "gettopiccountbycondition", prams).Tables[0].Rows[0][0].ToString(), -1);
		}

		/// <summary>
		/// 得到符合条件的主题总数
		/// </summary>
		/// <param name="condition">条件</param>
		/// <returns>主题总数</returns>
		public int GetTopicCount(string condition)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@condition",(DbType)SqlDbType.VarChar,1000,condition)
								  };
			return Utils.StrToInt(DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "gettopiccountbytype", prams).Tables[0].Rows[0][0].ToString(), -1);
		}

		/// <summary>
		/// 更新主题标题
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <param name="topictitle">新标题</param>
		/// <returns>成功返回1，否则返回0</returns>
		public int UpdateTopicTitle(int tid, string topictitle)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4, tid),
									  DbHelper.MakeInParam("@topictitle", (DbType)SqlDbType.NChar, 60, topictitle)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [title] = @topictitle WHERE [tid] = @tid", prams);
		}

		/// <summary>
		/// 更新主题图标id
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <param name="iconid">主题图标id</param>
		/// <returns>成功返回1，否则返回0</returns>
		public int UpdateTopicIconID(int tid, int iconid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4, tid),
									  DbHelper.MakeInParam("@iconid", (DbType)SqlDbType.SmallInt, 2, iconid)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [iconid] = @iconid WHERE [tid] = @tid", prams);
		}

		/// <summary>
		/// 更新主题价格
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <returns>成功返回1，否则返回0</returns>
		public int UpdateTopicPrice(int tid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4, tid),
			};
			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [price] = 0 WHERE [tid] = @tid", prams);
		}

		/// <summary>
		/// 更新主题价格
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <param name="price">价格</param>
		/// <returns>成功返回1，否则返回0</returns>
		public int UpdateTopicPrice(int tid, int price)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4, tid),
									  DbHelper.MakeInParam("@price",(DbType)SqlDbType.Int,4, price),
			};
			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [price] = @price WHERE [tid] = @tid", prams);
		}

		/// <summary>
		/// 更新主题阅读权限
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <param name="readperm">阅读权限</param>
		/// <returns>成功返回1，否则返回0</returns>
		public int UpdateTopicReadperm(int tid, int readperm)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4, tid),
									  DbHelper.MakeInParam("@readperm",(DbType)SqlDbType.Int,4, readperm),
			};
			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [readperm] = @readperm WHERE [tid] = @tid", prams);
		}

		/// <summary>
		/// 更新主题为已被管理
		/// </summary>
		/// <param name="topiclist">主题id列表</param>
		/// <param name="moderated">管理操作id</param>
		/// <returns>成功返回1，否则返回0</returns>
		public int UpdateTopicModerated(string topiclist, int moderated)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@moderated",(DbType)SqlDbType.Int,4, moderated),
			};
			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [moderated] = @moderated WHERE [tid] IN (" + topiclist + ")", prams);

		}

		/// <summary>
		/// 更新主题
		/// </summary>
		/// <param name="topicinfo">主题信息</param>
		/// <returns>成功返回1，否则返回0</returns>
		public int UpdateTopic(TopicInfo topicinfo)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4, topicinfo.Tid),
									  DbHelper.MakeInParam("@fid", (DbType)SqlDbType.SmallInt, 2, topicinfo.Fid), 
									  DbHelper.MakeInParam("@iconid", (DbType)SqlDbType.SmallInt, 2, topicinfo.Iconid), 
									  DbHelper.MakeInParam("@title", (DbType)SqlDbType.NChar, 60, topicinfo.Title), 
									  DbHelper.MakeInParam("@typeid", (DbType)SqlDbType.SmallInt, 2, topicinfo.Typeid), 
									  DbHelper.MakeInParam("@readperm", (DbType)SqlDbType.Int, 4, topicinfo.Readperm), 
									  DbHelper.MakeInParam("@price", (DbType)SqlDbType.SmallInt, 2, topicinfo.Price), 
									  DbHelper.MakeInParam("@poster", (DbType)SqlDbType.NChar, 15, topicinfo.Poster), 
									  DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.Int, 4, topicinfo.Posterid), 
									  DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.SmallDateTime, 4, DateTime.Parse(topicinfo.Postdatetime)), 
									  DbHelper.MakeInParam("@lastpost", (DbType)SqlDbType.VarChar, 0, topicinfo.Lastpost), 
									  DbHelper.MakeInParam("@lastposter", (DbType)SqlDbType.NChar, 15, topicinfo.Lastposter), 
									  //DbHelper.MakeInParam("@views", (DbType)SqlDbType.Int, 4, topicinfo.Views), 
									  DbHelper.MakeInParam("@replies", (DbType)SqlDbType.Int, 4, topicinfo.Replies), 
									  DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, topicinfo.Displayorder), 
									  DbHelper.MakeInParam("@highlight", (DbType)SqlDbType.VarChar, 500, topicinfo.Highlight), 
									  DbHelper.MakeInParam("@digest", (DbType)SqlDbType.Int, 4, topicinfo.Digest), 
									  DbHelper.MakeInParam("@rate", (DbType)SqlDbType.Int, 4, topicinfo.Rate), 
									  DbHelper.MakeInParam("@hide", (DbType)SqlDbType.Int, 4, topicinfo.Hide), 
									  DbHelper.MakeInParam("@poll", (DbType)SqlDbType.Int, 4, topicinfo.Poll), 
									  DbHelper.MakeInParam("@attachment", (DbType)SqlDbType.Int, 4, topicinfo.Attachment), 
									  DbHelper.MakeInParam("@moderated", (DbType)SqlDbType.Int, 4, topicinfo.Moderated), 
									  DbHelper.MakeInParam("@closed", (DbType)SqlDbType.Int, 4, topicinfo.Closed),
									  DbHelper.MakeInParam("@magic", (DbType)SqlDbType.Int, 4, topicinfo.Magic)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updatetopic", prams);
		}

		/// <summary>
		/// 判断帖子列表是否都在当前板块
		/// </summary>
		/// <param name="topicidlist"></param>
		/// <param name="fid"></param>
		/// <returns></returns>
		public bool InSameForum(string topicidlist, int fid)
		{
			string commandText = string.Format("SELECT COUNT([tid]) FROM [{0}topics] WHERE [fid]={1} AND [tid] IN ({2})", BaseConfigs.GetTablePrefix, fid, topicidlist);
			return Utils.SplitString(topicidlist, ",").Length == (int)DbHelper.ExecuteScalar(CommandType.Text, commandText);
		}

		/// <summary>
		/// 将主题设置为隐藏主题
		/// </summary>
		/// <param name="tid"></param>
		/// <returns></returns>
		public int UpdateTopicHide(int tid)
		{
			string sql = string.Format(@"UPDATE [{0}topics] SET [hide] = 1 WHERE [tid] = {1}", BaseConfigs.GetTablePrefix, tid);

			return DbHelper.ExecuteNonQuery(CommandType.Text, sql);
		}

		public DataTable GetTopicList(int forumid, int pageid, int tpp)
		{
			DataTable dt;
			if (pageid == 1)
			{
				dt = DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP {0} [tid],[title],[replies] FROM [{1}topics] WHERE [fid]={2} AND [displayorder]>=0 ORDER BY [lastpostid] DESC", tpp.ToString(), BaseConfigs.GetTablePrefix, forumid.ToString())).Tables[0];
			}
			else
			{
				dt = DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP {0} [tid],[title],[replies] FROM [{1}topics] WHERE [lastpostid] < (SELECT MIN([lastpostid])  FROM (SELECT TOP {2} [lastpostid] FROM [{1}topics] WHERE [fid]={3} AND [displayorder]>=0 ORDER BY [lastpostid] DESC) AS tblTmp ) AND [fid]={3} AND [displayorder]>=0 ORDER BY [lastpostid] DESC", tpp.ToString(), BaseConfigs.GetTablePrefix, ((pageid - 1) * tpp).ToString(), forumid.ToString())).Tables[0];
			}
			return dt;
		}

		public DataTable GetTopicFidByTid(string tidlist)
		{
			string sql = "SELECT DISTINCT [fid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] IN(" + tidlist + ")";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public DataTable GetTopicTidByFid(string tidlist, int fid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int,4, fid)
			};
			string sql = "SELECT [tid] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid] IN(" + tidlist + ") AND [fid]=@fid";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		}

		/// <summary>
		/// 更新主题浏览量
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <param name="viewcount">浏览量</param>
		/// <returns>成功返回1，否则返回0</returns>
		public int UpdateTopicViewCount(int tid, int viewcount)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid),	
									  DbHelper.MakeInParam("@viewcount",(DbType)SqlDbType.Int,4,viewcount)			   
								  };
			return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updatetopicviewcount",
				prams);
		}

		public string SearchTopics(int forumid, string keyword, string displayorder, string digest, string attachment, string poster, bool lowerupper, string viewsmin, string viewsmax,
			string repliesmax, string repliesmin, string rate, string lastpost, DateTime postdatetimeStart, DateTime postdatetimeEnd)
		{
			string sqlstring = null;
			sqlstring += " [tid]>0";

			if (forumid != 0)
			{
				sqlstring += " AND [fid]=" + forumid;
			}

			if (keyword != "")
			{
				sqlstring += " AND (";
				foreach (string word in keyword.Split(','))
				{
					if (word.Trim() != "")
						sqlstring += " [title] LIKE '%" + RegEsc(word) + "%' OR ";
				}
				sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
			}

			switch (displayorder)
			{
				case "0":
					break;
				case "1":
					sqlstring += " AND [displayorder]>0";
					break;
				case "2":
					sqlstring += " AND [displayorder]<=0";
					break;
			}

			switch (digest)
			{
				case "0":
					break;
				case "1":
					sqlstring += " AND [digest]>=1";
					break;
				case "2":
					sqlstring += " AND [digest]<1";
					break;
			}

			switch (attachment)
			{
				case "0":
					break;
				case "1":
					sqlstring += " AND [attachment]>0";
					break;
				case "2":
					sqlstring += " AND [attachment]<=0";
					break;
			}

			if (poster != "")
			{
				sqlstring += " AND (";
				foreach (string postername in poster.Split(','))
				{
					if (postername.Trim() != "")
					{
						//不区别大小写
						if (lowerupper)
						{
							sqlstring += " [poster]='" + postername + "' OR";
						}
						else
						{
							sqlstring += " [poster] COLLATE Chinese_PRC_CS_AS_WS ='" + postername + "' OR";
						}
					}
				}
				sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
			}

			if (viewsmax != "")
			{
				sqlstring += " AND [views]>" + viewsmax;
			}

			if (viewsmin != "")
			{
				sqlstring += " AND [views]<" + viewsmin;
			}

			if (repliesmax != "")
			{
				sqlstring += " AND [replies]>" + repliesmax;
			}

			if (repliesmin != "")
			{
				sqlstring += " AND [replies]<" + repliesmin;
			}

			if (rate != "")
			{
				sqlstring += " AND [rate]>" + rate;
			}

			if (lastpost != "")
			{
				sqlstring += " AND DATEDIFF(day,[lastpost],GETDATE())>=" + lastpost + "";
			}

			sqlstring = GetSqlstringByPostDatetime(sqlstring, postdatetimeStart, postdatetimeEnd);

			return sqlstring;
		}

		public string SearchAttachment(int forumid, string posttablename, string filesizemin, string filesizemax, string downloadsmin, string downloadsmax, string postdatetime, string filename, string description, string poster)
		{
			string sqlstring = null;
			sqlstring += " WHERE [aid] > 0";


			if (forumid != 0)
			{
				sqlstring += " AND [pid] IN (SELECT PID FROM [" + posttablename + "] WHERE [fid]=" + forumid + ")";
			}

			if (filesizemin != "")
			{
				sqlstring += " AND [filesize]<" + filesizemin;
			}

			if (filesizemax != "")
			{
				sqlstring += " AND [filesize]>" + filesizemax;
			}

			if (downloadsmin != "")
			{
				sqlstring += " AND [downloads]<" + downloadsmin;
			}

			if (downloadsmax != "")
			{
				sqlstring += " AND [downloads]>" + downloadsmax;
			}

			if (postdatetime != "")
			{
				sqlstring += " AND DATEDIFF(day,[postdatetime],GETDATE())>=" + postdatetime + "";
			}

			if (filename != "")
			{
				sqlstring += " AND [filename] like '%" + RegEsc(filename) + "%'";
			}


			if (description != "")
			{
				sqlstring += " AND (";
				foreach (string word in description.Split(','))
				{
					if (word.Trim() != "")
						sqlstring += " [description] LIKE '%" + RegEsc(word) + "%' OR ";
				}
				sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
			}

			if (poster != "")
			{
				sqlstring += " AND [pid] IN (SELECT [pid] FROM [" + posttablename + "] WHERE [poster]='" + poster + "')";
			}

			return sqlstring;
		}

		public string SearchPost(int forumid, string posttableid, DateTime postdatetimeStart, DateTime postdatetimeEnd, string poster, bool lowerupper, string ip, string message)
		{
			string sqlstring = null;
			sqlstring += " [pid]>0 ";

			if (forumid != 0)
			{
				sqlstring += " AND [fid]=" + forumid;                
			}

			sqlstring = GetSqlstringByPostDatetime(sqlstring, postdatetimeStart, postdatetimeEnd);

			if (poster != "")
			{
				sqlstring += " AND (";
				foreach (string postername in poster.Split(','))
				{
					if (postername.Trim() != "")
					{
						//不区别大小写
						if (lowerupper)
						{
							sqlstring += " [poster]='" + postername + "' OR";
						}
						else
						{
							sqlstring += " [poster] COLLATE Chinese_PRC_CS_AS_WS ='" + postername + "' OR";
						}
					}
				}
				sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
			}

			if (ip != "")
			{
				sqlstring += " AND [ip] LIKE'%" + RegEsc(ip.Replace(".*", "")) + "%'";
			}

			if (message != "")
			{
				sqlstring += " AND (";
				foreach (string messageinf in message.Split(','))
				{
					if (messageinf.Trim() != "")
					{
						sqlstring += " [message] LIKE '%" + RegEsc(messageinf) + "%' OR";
					}

				}
				sqlstring = sqlstring.Substring(0, sqlstring.Length - 3) + ")";
			}

			return sqlstring;
		}

		public void IdentifyTopic(string topiclist, int identify)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@identify", (DbType)SqlDbType.Int, 4, identify)
								  };

			string sql = string.Format("UPDATE [{0}topics] SET [identify]=@identify WHERE [tid] IN ({1})", BaseConfigs.GetTablePrefix, topiclist);
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public void UpdateTopic(int tid, string title, int posterid, string poster)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@tid", (DbType) SqlDbType.Int, 4, tid),
									  DbHelper.MakeInParam("@title", (DbType) SqlDbType.NChar, 60, title),
									  DbHelper.MakeInParam("@posterid", (DbType) SqlDbType.Int, 4, posterid),
									  DbHelper.MakeInParam("@poster", (DbType) SqlDbType.NChar, 20, poster)
								  };

			string sql = string.Format("UPDATE [{0}topics] SET [title]=@title, [posterid]=@posterid, [poster]=@poster WHERE [tid]=@tid", BaseConfigs.GetTablePrefix);
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public string GetTopicCountCondition(out string type, string gettype, int getnewtopic)
		{
			string condition = "";
			type = string.Empty;
			if (gettype == "digest")
			{
				type = "digest";
				condition += " AND digest>0 ";
			}

			if (gettype == "newtopic")
			{
				type = "newtopic";
				condition += " AND postdatetime>='" + DateTime.Now.AddMinutes(-getnewtopic).ToString("yyyy-MM-dd HH:mm:ss") + "'";
			}
			return condition;
		}


		public string GetRateLogCountCondition(int userid, string postidlist)
		{

			return "[uid]=" + userid + " AND [pid] = " + Utils.StrToInt(postidlist, 0).ToString();
		}

		public DataTable GetOtherPostId(string postidlist, int topicid, int postid)
		{

			DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "select * from " + BaseConfigs.GetTablePrefix + "posts" + postid + " where pid not in (" + postidlist + ") and tid=" + topicid + " order by pid desc").Tables[0];
			return dt;
		}

		#endregion
		
		#region SpaceManage

		#region Space 个人数据操作

		public void AddAlbumCategory(AlbumCategoryInfo aci)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@title", (DbType)SqlDbType.NChar, 50, aci.Title),
									  DbHelper.MakeInParam("@description", (DbType)SqlDbType.NChar, 300, aci.Description),
									  DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, aci.Displayorder)
								  };

			string sql = string.Format(@"INSERT INTO [{0}albumcategories]([title], [description], [albumcount], [displayorder])
                            VALUES(@title, @description, 0, @displayorder)", BaseConfigs.GetTablePrefix);

			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public void UpdateAlbumCategory(AlbumCategoryInfo aci)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@albumcateid", (DbType)SqlDbType.Int, 4, aci.Albumcateid),
									  DbHelper.MakeInParam("@title", (DbType)SqlDbType.NChar, 50, aci.Title),
									  DbHelper.MakeInParam("@description", (DbType)SqlDbType.NChar, 300, aci.Description),
									  DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, aci.Displayorder)
								  };

			string sql = string.Format(@"UPDATE [{0}albumcategories] 
                                         SET [title]=@title, [description]=@description, [displayorder]=@displayorder 
                                         WHERE [albumcateid]=@albumcateid", BaseConfigs.GetTablePrefix);

			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}


		public void DeleteAlbumCategory(int albumcateid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@albumcateid", (DbType)SqlDbType.Int, 4, albumcateid);

			string sql = string.Format(@"DELETE FROM [{0}albumcategories] 
                                         WHERE [albumcateid]=@albumcateid", BaseConfigs.GetTablePrefix);

			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
		}

		public IDataReader GetSpaceConfigDataByUserID(int userid)
		{
			IDataReader IDataReader = DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}spaceconfigs] WHERE [userid] = {1}", BaseConfigs.GetTablePrefix, userid));
			return IDataReader;
		}


		public IDataReader GetSpaceConfigDataBySpaceID(int spaceid)
		{
			IDataReader IDataReader = DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}spaceconfigs] WHERE [spaceid] = {1}", BaseConfigs.GetTablePrefix, spaceid));
			return IDataReader;
		}


		/// <summary>
		/// 保存用户space配置信息
		/// </summary>
		/// <param name="spaceconfiginfo"></param>
		/// 
		/// <returns></returns>
		public bool SaveSpaceConfigData(SpaceConfigInfo spaceconfiginfo)
		{
			//try
			//{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@spacetitle", (DbType)SqlDbType.NVarChar, 100, spaceconfiginfo.Spacetitle),
									  DbHelper.MakeInParam("@description", (DbType)SqlDbType.NVarChar, 200, spaceconfiginfo.Description),
									  DbHelper.MakeInParam("@blogdispmode", (DbType)SqlDbType.TinyInt, 1, spaceconfiginfo.BlogDispMode),
									  DbHelper.MakeInParam("@bpp", (DbType)SqlDbType.Int, 4, spaceconfiginfo.Bpp),
									  DbHelper.MakeInParam("@commentpref", (DbType)SqlDbType.TinyInt, 1, spaceconfiginfo.Commentpref),
									  DbHelper.MakeInParam("@messagepref", (DbType)SqlDbType.TinyInt, 1, spaceconfiginfo.MessagePref),
									  //DbHelper.MakeInParam("@rewritename", (DbType)SqlDbType.VarChar, 100, spaceconfiginfo.Rewritename),
									  DbHelper.MakeInParam("@themeid", (DbType)SqlDbType.Int, 4, spaceconfiginfo.ThemeID),
									  DbHelper.MakeInParam("@themepath", (DbType)SqlDbType.NChar, 50, spaceconfiginfo.ThemePath),
									  DbHelper.MakeInParam("@status", (DbType)SqlDbType.Int, 4, spaceconfiginfo.Status),
									  DbHelper.MakeInParam("@updatedatetime", (DbType)SqlDbType.SmallDateTime, 4, DateTime.Now),
										   
									  //DbHelper.MakeInParam("@defaulttab", (DbType)SqlDbType.Int, 4, DateTime.Now),
										   
									  DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, spaceconfiginfo.UserID)
								  };
			string sqlstring = "UPDATE [" + BaseConfigs.GetTablePrefix + "spaceconfigs] SET [spacetitle] = @spacetitle ,[description] = @description,[blogdispmode] = @blogdispmode,[bpp] = @bpp, [commentpref] = @commentpref, [messagepref] = @messagepref, [themeid]=@themeid,[themepath] = @themepath, [updatedatetime] = @updatedatetime WHERE [userid] = @userid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);
			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}
		}


		/// <summary>
		/// 建议用户space信息
		/// </summary>
		/// <param name="spaceconfiginfo"></param>
		/// 
		/// <returns></returns>
		public int AddSpaceConfigData(SpaceConfigInfo spaceconfiginfo)
		{
			//try
			//{
			IDataParameter[] prams = 
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
			string sqlstring = String.Format("INSERT INTO [" + BaseConfigs.GetTablePrefix + "spaceconfigs] ([userid], [spacetitle], [description], [blogdispmode], [bpp], [commentpref], [messagepref], [rewritename], [themeid], [themepath], [postcount], [commentcount], [visitedtimes], [createdatetime], [updatedatetime]) VALUES (@userid, @spacetitle, @description, @blogdispmode, @bpp, @commentpref, @messagepref, @rewritename, @themeid, @themepath, @postcount, @commentcount, @visitedtimes, @createdatetime, @updatedatetime);SELECT SCOPE_IDENTITY()");

			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sqlstring, prams), 0);
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return 0;
			//}
		}

		/// <summary>
		/// 为当前用户的SPACE访问量加1
		/// </summary>
		/// <param name="userid"></param>
		/// 
		/// <returns></returns>
		public bool CountUserSpaceVisitedTimesByUserID(int userid)
		{
			//try
			//{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4,userid)
				};

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "spaceconfigs] SET [visitedtimes] = [visitedtimes] + 1 WHERE [userid] = @userid", prams);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}
		}


		/// <summary>
		/// 更新当前用户的SPACE日志数
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="errormsg"></param>
		/// <returns></returns>
		public bool CountUserSpacePostCountByUserID(int userid, int postcount)
		{
			//try
			//{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@postcount", (DbType)SqlDbType.Int, 4,postcount),
					DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4,userid)
				};

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "spaceconfigs] SET [postcount] = [postcount] + @postcount  WHERE [userid] = @userid", prams);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}
		}


		/// <summary>
		/// 更新当前用户的SPACE评论数
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="errormsg"></param>
		/// <returns></returns>
		public bool CountUserSpaceCommentCountByUserID(int userid, int commentcount)
		{
			//try
			//{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@commentcount", (DbType)SqlDbType.Int, 4,commentcount),
					DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4,userid)
				};

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "spaceconfigs] SET [commentcount] = [commentcount] + @commentcount  WHERE [userid] = @userid", prams);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}
		}
		#endregion


		#region Space 主题数据操作
		public IDataReader GetSpaceThemeDataByThemeID(int themeid)
		{
			IDataReader IDataReader = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "spacethemes] WHERE [themeid] = " + themeid);
			return IDataReader;
		}
		#endregion


		#region Space 评论数据操作
		public bool AddSpaceComment(SpaceCommentInfo spacecomments)
		{
			//try
			//{
			IDataParameter[] prams = 
				{
					//DbHelper.MakeInParam("@commentid", (DbType)SqlDbType.Int, 4,spacecomments.CommentID),
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
			string sqlstring = String.Format("INSERT INTO [" + BaseConfigs.GetTablePrefix + "spacecomments] ( [postid], [author], [email], [url], [ip], [postdatetime], [content], [parentid], [uid],[posttitle] ) VALUES ( @postid, @author, @email, @url, @ip, @postdatetime, @content, @parentid, @uid, @posttitle)");

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}
		}

		public bool SaveSpaceComment(SpaceCommentInfo spacecomments)
		{
			//try
			//{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@commentid", (DbType)SqlDbType.Int, 4,spacecomments.CommentID),
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
			string sqlstring = String.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "spacecomments]  Set [postid] = @postid, [author] = @author, [email] = @email, [url] = @url, [ip] = @ip, [postdatetime] = @postdatetime, [content] = @content, [parentid] = @parentid, [uid] = @uid, [posttitle]=@posttile  WHERE [commentid] = @commentid");

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}
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
                string sqlstring = string.Format(@"DELETE FROM [{0}spacecomments] 
                                                     FROM [{0}spaceposts] 
                                                     WHERE [{0}spaceposts].[postid] = [{0}spacecomments].[postid] AND [{0}spaceposts].[uid]={1} AND
                                                     [{0}spacecomments].[commentid] IN ({2})", BaseConfigs.GetTablePrefix, userid, commentidList);//"DELETE FROM [" + BaseConfigs.GetTablePrefix + "spacecomments] WHERE [commentid] IN (" + commentidList + ") AND [uid]=" + userid;
                DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
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
				string sqlstring = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "spacecomments] WHERE [uid]=" + userid;
				DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
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
				string sqlstring = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "spacecomments] WHERE [commentid] = " + commentid;
				DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
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
				IDataParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
				string ordertype = orderbyASC ? "ASC" : "DESC";
				int pageTop = (currentPage - 1) * pageSize;

				string sql = "";

				if (currentPage == 1)
				{
					sql = string.Format(@"SELECT TOP {0} [sc].* FROM  
                        [{1}spacecomments] AS [sc], [{1}spaceposts] AS [sp] WHERE [sc].[postid]=[sp].[postid] AND [sp].[uid]=@userid ORDER BY [sc].[commentid] {2}", pageSize, BaseConfigs.GetTablePrefix, ordertype);
				}
				else
				{
					if (!orderbyASC)
					{
						sql = string.Format(@"SELECT TOP {0} [sc].* FROM 
                            [{1}spacecomments] AS [sc], [{1}spaceposts] AS [sp] WHERE [commentid] < (SELECT min([commentid])  FROM 
                             (SELECT TOP {2} [sc1].[commentid] FROM [{1}spacecomments] AS [sc1], [{1}spaceposts] AS [sp1] WHERE 
                            [sc1].[postid]=[sp1].[postid] AND [sp1].[uid]=@userid ORDER BY [sc1].[commentid] {3}) AS tblTmp ) AND [sc].[postid]=[sp].[postid] AND [sp].[uid]=@userid ORDER BY [sc].[commentid] {3}", pageSize, BaseConfigs.GetTablePrefix, pageTop, ordertype);
					}
					else
					{
						sql = string.Format(@"SELECT TOP {0} [sc].* FROM 
                            [{1}spacecomments] AS [sc], [{1}spaceposts] AS [sp] WHERE [commentid] > (SELECT MAX([commentid])  FROM 
                            (SELECT TOP {2} [commentid] FROM [{1}spacecomments] AS [sc1], [{1}spaceposts] AS [sp1] WHERE 
                            [sc1].[postid]=[sp1].[postid] AND [sp1].[uid]=@userid ORDER BY [commentid] {3}) AS tblTmp ) AND [sc].[postid]=[sp].[postid] AND [sp].[uid]=@userid ORDER BY [sc].[commentid] {3}", pageSize, BaseConfigs.GetTablePrefix, pageTop, ordertype);
					}
				}
				return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
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
				IDataParameter parm = DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4, postid);
				string ordertype = orderbyASC ? "ASC" : "DESC";
				int pageTop = (currentPage - 1) * pageSize;

				string sql = "";

				if (currentPage == 1)
				{
					sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
						+ "[" + BaseConfigs.GetTablePrefix + "spacecomments] WHERE [postid]=@postid ORDER BY [commentid] " + ordertype;
				}
				else
				{
					if (!orderbyASC)
					{
						sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
							+ "[" + BaseConfigs.GetTablePrefix + "spacecomments] WHERE [commentid] < (SELECT min([commentid])  FROM "
							+ "(SELECT TOP " + pageTop + " [commentid] FROM [" + BaseConfigs.GetTablePrefix + "spacecomments] WHERE "
							+ "[postid]=@postid ORDER BY [commentid] " + ordertype + ") AS tblTmp ) AND [postid]=@postid ORDER BY [commentid] " + ordertype;
					}
					else
					{
						sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
							+ "[" + BaseConfigs.GetTablePrefix + "spacecomments] WHERE [commentid] > (SELECT MAX([commentid])  FROM "
							+ "(SELECT TOP " + pageTop + " [commentid] FROM [" + BaseConfigs.GetTablePrefix + "spacecomments] WHERE "
							+ "[postid]=@postid ORDER BY [commentid] " + ordertype + ") AS tblTmp ) AND [postid]=@postid ORDER BY [commentid] " + ordertype;

					}
				}
				return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
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
				IDataParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
				return (int)DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT([sc].[commentid]) FROM [{0}spacecomments] AS [sc], [{0}spaceposts] AS [sp] WHERE [sc].[postid]=[sp].[postid] AND [sp].[uid]=@userid", BaseConfigs.GetTablePrefix), parm);
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
				IDataParameter parm = DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4, postid);
				return (int)DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([commentid]) FROM [" + BaseConfigs.GetTablePrefix + "spacecomments] WHERE [postid]=@postid", parm);
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
				string sql = "SELECT TOP " + topcount.ToString() + " * FROM "
					+ "[" + BaseConfigs.GetTablePrefix + "spacecomments] WHERE [postid] IN (SELECT TOP 10 [postid] FROM [" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE [uid] = " + userid + " AND [commentcount]>0 ORDER BY [postid] DESC) ORDER BY [commentid] DESC";
				return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];

			}
			catch
			{
				return new DataTable();
			}
		}


		#endregion


		#region Space 日志数据操作
		public bool AddSpacePost(SpacePostInfo spaceposts)
		{
			//try
			//{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4,spaceposts.PostID),
					DbHelper.MakeInParam("@author", (DbType)SqlDbType.NVarChar, 20,spaceposts.Author),
					DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,spaceposts.Uid),
					DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8,spaceposts.PostDateTime),
					DbHelper.MakeInParam("@content", (DbType)SqlDbType.NText, 0,spaceposts.Content),
					DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 150,spaceposts.Title),
					DbHelper.MakeInParam("@category", (DbType)SqlDbType.VarChar, 255,spaceposts.Category),
					DbHelper.MakeInParam("@poststatus", (DbType)SqlDbType.TinyInt, 1,spaceposts.PostStatus),
					DbHelper.MakeInParam("@commentstatus", (DbType)SqlDbType.TinyInt, 1,spaceposts.CommentStatus),
					DbHelper.MakeInParam("@postupdatetime", (DbType)SqlDbType.DateTime, 8,spaceposts.PostUpDateTime),
					DbHelper.MakeInParam("@commentcount", (DbType)SqlDbType.Int, 4,spaceposts.CommentCount)
				};
			string sqlstring = String.Format("INSERT INTO [" + BaseConfigs.GetTablePrefix + "spaceposts] ([author], [uid], [postdatetime], [content], [title], [category], [poststatus], [commentstatus], [postupdatetime], [commentcount]) VALUES ( @author, @uid, @postdatetime, @content, @title, @category, @poststatus, @commentstatus, @postupdatetime, @commentcount);SELECT SCOPE_IDENTITY();");

			//向关联表中插入相关数据
			IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sqlstring, prams);
			sqlstring = "UPDATE [" + BaseConfigs.GetTablePrefix + "spaceconfigs] SET [updatedatetime]=@postupdatetime WHERE [userid]=@uid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

			if (reader != null)
			{
				reader.Read();

				foreach (string cateogryid in spaceposts.Category.Split(','))
				{
					if (cateogryid != "")
					{
						SpacePostCategoryInfo spacepostCategoryInfo = new SpacePostCategoryInfo();
						spacepostCategoryInfo.PostID = Convert.ToInt32(reader[0].ToString());
						spacepostCategoryInfo.CategoryID = Convert.ToInt32(cateogryid);
						AddSpacePostCategory(spacepostCategoryInfo);
					}
				}
			}

			IDataParameter[] prams1 = 
				{
					DbHelper.MakeInParam("@spacepostid", (DbType)SqlDbType.Int, 4,reader[0].ToString()),
					DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,spaceposts.Uid)
				};

			reader.Close();

			//更新与当前日志关联的附件表中的数据
			DbHelper.ExecuteReader(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "spaceattachments] SET [spacepostid] = @spacepostid  WHERE [spacepostid] = 0 AND [uid] = @uid ", prams1);

			//对当前用户日志加1
			CountUserSpacePostCountByUserID(spaceposts.Uid, 1);

            

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}
		}

		public bool SaveSpacePost(SpacePostInfo spaceposts)
		{
			//try
			//{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4, spaceposts.PostID),
					DbHelper.MakeInParam("@author", (DbType)SqlDbType.NVarChar, 20, spaceposts.Author),
					DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, spaceposts.Uid),
					DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, spaceposts.PostDateTime),
					DbHelper.MakeInParam("@content", (DbType)SqlDbType.NText, 0, spaceposts.Content),
					DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 150, spaceposts.Title),
					DbHelper.MakeInParam("@category", (DbType)SqlDbType.VarChar, 255, spaceposts.Category),
					DbHelper.MakeInParam("@poststatus", (DbType)SqlDbType.TinyInt, 1, spaceposts.PostStatus),
					DbHelper.MakeInParam("@commentstatus", (DbType)SqlDbType.TinyInt, 1, spaceposts.CommentStatus),
					DbHelper.MakeInParam("@postupdatetime", (DbType)SqlDbType.DateTime, 8, spaceposts.PostUpDateTime)
				};
			string sqlstring = String.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "spaceposts]  SET [author] = @author, [uid] = @uid, [postdatetime] = @postdatetime, [content] = @content, [title] = @title, [category] = @category, [poststatus] = @poststatus, [commentstatus] = @commentstatus, [postupdatetime] = @postupdatetime WHERE [postid] = @postid");

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);
			sqlstring = "UPDATE [" + BaseConfigs.GetTablePrefix + "spaceconfigs] SET [updatedatetime]=@postupdatetime WHERE [userid]=@uid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);
			//先删除指定的日志关联数据再插入新数据
			DeleteSpacePostCategoryByPostID(spaceposts.PostID);

			foreach (string cateogryid in spaceposts.Category.Split(','))
			{
				if (cateogryid != "")
				{
					SpacePostCategoryInfo spacepostCategoryInfo = new SpacePostCategoryInfo();
					spacepostCategoryInfo.PostID = spaceposts.PostID;
					spacepostCategoryInfo.CategoryID = Convert.ToInt32(cateogryid);
					AddSpacePostCategory(spacepostCategoryInfo);
				}
			}


			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}
		}

		public IDataReader GetSpacePost(int postid)
		{
			IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE  [postid]=" + postid);
			return dr;
		}

		/// <summary>
		/// 删除日志
		/// </summary>
		/// <param name="postidList">删除日志的postid列表</param>
		/// <returns></returns>
		public bool DeleteSpacePosts(string postidList, int userid)
		{
			if (!Utils.IsNumericArray(postidList.Split(',')))
			{
				return false;
			}

			string sqlstring = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE [postid] IN (" + postidList + ") AND [uid]=" + userid;
			int deletedCount = DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

			if (deletedCount > 0)
			{
				sqlstring = string.Format("UPDATE [{0}spaceconfigs] SET [postcount] = [postcount] - {1} WHERE [userid] = {2}", BaseConfigs.GetTablePrefix, deletedCount, userid);
				DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
			}
			return true;
		}

		public bool DeleteSpacePosts(int userid)
		{
			string sqlstring = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE [uid]=" + userid;
			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
			sqlstring = string.Format("UPDATE [{0}spaceconfigs] SET [postcount] = 0 WHERE [userid] = {1}", BaseConfigs.GetTablePrefix, userid);
			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

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
			IDataParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
			int pageTop = (currentPage - 1) * pageSize;
			string sql = "";
			if (currentPage == 1)
			{
				sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
					+ "[" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE [uid]=@userid ORDER BY [postid] DESC";
			}
			else
			{
				sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
					+ "[" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE [postid] < (SELECT min([postid])  FROM "
					+ "(SELECT TOP " + pageTop + " [postid] FROM [" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE "
					+ "[uid]=@userid ORDER BY [postid] DESC) AS tblTmp ) AND [uid]=@userid ORDER BY [postid] DESC";
			}
			return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
		}

		public DataTable SpacePostsList(int pageSize, int currentPage, int userid, int poststatus)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
				DbHelper.MakeInParam("@poststatus", (DbType)SqlDbType.Int, 4, poststatus)
			};
			int pageTop = (currentPage - 1) * pageSize;
			string sql = "";
			if (currentPage == 1)
			{
				sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
					+ "[" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE [uid]=@userid AND [poststatus]=@poststatus ORDER BY [postid] DESC";
			}
			else
			{
				sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
					+ "[" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE [postid] < (SELECT min([postid])  FROM "
					+ "(SELECT TOP " + pageTop + " [postid] FROM [" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE "
					+ "[uid]=@userid AND [poststatus]=@poststatus ORDER BY [postid] DESC) AS tblTmp ) AND [uid]=@userid AND [poststatus]=@poststatus ORDER BY [postid] DESC";
			}
			return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		}

		public DataTable SpacePostsList(int pageSize, int currentPage, int userid, DateTime postdatetime)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
				DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, postdatetime)
			};

			int pageTop = (currentPage - 1) * pageSize;
			string sql = "";
			if (currentPage == 1)
			{
				sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
					+ "[" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE [uid]=@userid AND [poststatus]=1 AND "
					+ "DATEDIFF(d, @postdatetime, postdatetime) = 0 ORDER BY [postid] DESC";
			}
			else
			{
				sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
					+ "[" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE [postid] < (SELECT min([postid])  FROM "
					+ "(SELECT TOP " + pageTop + " [postid] FROM [" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE "
					+ "[uid]=@userid AND [poststatus]=1 AND DATEDIFF(d, @postdatetime, postdatetime) = 0 ORDER BY [postid] DESC) AS tblTmp ) "
					+ "AND [uid]=@userid AND [poststatus]=1 AND DATEDIFF(d, @postdatetime, postdatetime) = 0 ORDER BY [postid] DESC";
			}
			return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
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
				IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid)
				};
				return (int)DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([postid]) FROM [" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE uid=@userid", prams);
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
				IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
					DbHelper.MakeInParam("@poststatus", (DbType)SqlDbType.Int, 4, poststatus)
				};
				return (int)DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([postid]) FROM [" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE [uid]=@userid AND [poststatus]=@poststatus", prams);
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
				IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
					DbHelper.MakeInParam("@poststatus", (DbType)SqlDbType.Int, 4, poststatus),
					DbHelper.MakeInParam("@postdatetime",(DbType)SqlDbType.DateTime, 8, DateTime.Parse(postdatetime))
				};
				return (int)DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([postid]) FROM [" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE [uid]=@userid AND [poststatus]=@poststatus AND DATEDIFF(d, @postdatetime, postdatetime) = 0", prams);
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
		/// 
		/// <returns></returns>
		public bool CountUserSpacePostByUserID(int postid)
		{
			//try
			//{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4,postid)
				};

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "spaceposts] SET [views] = [views] + 1 WHERE [postid] = @postid", prams);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}
		}


		/// <summary>
		/// 更新当前日志数的评论数
		/// </summary>
		/// <param name="postid"></param>
		/// <param name="errormsg"></param>
		/// <returns></returns>
		public bool CountSpaceCommentCountByPostID(int postid, int commentcount)
		{
			//try
			//{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@commentcount", (DbType)SqlDbType.Int, 4,commentcount),
					DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4,postid)
				};

			if (commentcount >= 0)
			{
				DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "spaceposts] SET [commentcount] = [commentcount] + @commentcount  WHERE [postid] = @postid ", prams);
			}
			else
			{
				DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "spaceposts] SET [commentcount] = [commentcount] + @commentcount  WHERE [postid] = @postid AND [commentcount]>0", prams);
			}
			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}
		}


		/// <summary>
		/// 更新当前日志数的浏览量
		/// </summary>
		/// <param name="postid"></param>
		/// <param name="errormsg"></param>
		/// <returns></returns>
		public bool CountUserSpaceViewsByUserID(int postid, int views)
		{
			//try
			//{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@views", (DbType)SqlDbType.Int, 4,views),
					DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4,postid)
				};

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "spaceposts] SET [views] = [views] + @views  WHERE [postid] = @postid", prams);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}
		}
		#endregion


		#region 日志类型 操作类

		public IDataReader GetSpaceCategoryByCategoryID(int categoryid)
		{
			IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "spacecategories] WHERE [categoryid] = " + categoryid);
			return reader;
		}


		public bool AddSpaceCategory(SpaceCategoryInfo spacecategories)
		{
			//try
			//{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4,spacecategories.CategoryID),
					DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 50,spacecategories.Title),
					DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,spacecategories.Uid),
					DbHelper.MakeInParam("@description", (DbType)SqlDbType.NVarChar, 1000,spacecategories.Description),
					DbHelper.MakeInParam("@typeid", (DbType)SqlDbType.Int, 4,spacecategories.TypeID),
					DbHelper.MakeInParam("@categorycount", (DbType)SqlDbType.Int, 4,spacecategories.CategoryCount),
					DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4,spacecategories.Displayorder)
				};
			string sqlstring = String.Format("INSERT INTO [" + BaseConfigs.GetTablePrefix + "spacecategories] ( [title], [uid], [description], [typeid], [categorycount], [displayorder]) VALUES ( @title, @uid, @description, @typeid, @categorycount, @displayorder)");

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}

		}

		public bool SaveSpaceCategory(SpaceCategoryInfo spacecategories)
		{
			//try
			//{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4,spacecategories.CategoryID),
					DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 50,spacecategories.Title),
					DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,spacecategories.Uid),
					DbHelper.MakeInParam("@description", (DbType)SqlDbType.NVarChar, 1000,spacecategories.Description),
					DbHelper.MakeInParam("@typeid", (DbType)SqlDbType.Int, 4,spacecategories.TypeID),
					DbHelper.MakeInParam("@categorycount", (DbType)SqlDbType.Int, 4,spacecategories.CategoryCount),
					DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4,spacecategories.Displayorder)
				};
			string sqlstring = String.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "spacecategories] SET  [title] = @title, [uid] = @uid, [description] = @description, [typeid] = @typeid, [categorycount] = @categorycount, [displayorder] = @displayorder WHERE [categoryid] = @categoryid ");

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}

		}

		/// <summary>
		///	获取分类列表
		/// </summary>
		/// <param name="idList">分类的ID，以","分隔</param>
		/// <returns>返回分类名称列表</returns>
		public string GetCategoryNameByIdList(string idList)
		{
			if (idList.ToString() != "")
			{
				string sql = "SELECT [title] FROM [" + BaseConfigs.GetTablePrefix + "spacecategories] WHERE [categoryid] IN (" + idList + ")";
				IDataReader categoryReader = DbHelper.ExecuteReader(CommandType.Text, sql);
				string categoryNameList = "";
				if (categoryReader != null)
				{
					while (categoryReader.Read())
					{
						categoryNameList += categoryReader["title"].ToString() + ",";
					}
					categoryReader.Close();
				}
				if (categoryNameList == "")
				{
					return "";
				}
				else
				{
					return categoryNameList.Substring(0, categoryNameList.Length - 1);
				}
			}
			else
			{
				return "&nbsp;";
			}
		}


		/// <summary>
		///	获取分类列表
		/// </summary>
		/// <param name="userid">用户的id</param>
		/// <returns>返回分类名称列表</returns>
		public IDataReader GetCategoryNameByUserID(int userid)
		{
			if (userid > 0)
			{
				string sql = "SELECT [categoryid], [title]  FROM [" + BaseConfigs.GetTablePrefix + "spacecategories] WHERE [uid] = " + userid;
				IDataReader categoryReader = DbHelper.ExecuteReader(CommandType.Text, sql);
				return categoryReader;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 根据用户id获取分类列表
		/// </summary>
		/// <param name="userid"></param>
		/// <returns></returns>
		public DataTable GetSpaceCategoryListByUserId(int userid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
			string sql = "SELECT [categoryid], [title] FROM [" + BaseConfigs.GetTablePrefix + "spacecategories] WHERE [uid]=@userid ORDER BY [displayorder], [categoryid]";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
		}

		/// <summary>
		///	获取分类列表
		/// </summary>
		/// <param name="idList">分类的ID, 以","分隔</param>
		/// <returns>返回分类名称列表</returns>
		public IDataReader GetCategoryIDAndName(string idList)
		{
			if (idList.Trim() == "")
			{
				return null;
			}

			IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT [categoryid],[title] FROM [" + BaseConfigs.GetTablePrefix + "spacecategories] WHERE [categoryid] IN (" + idList + ")");
			return reader;
		}

		/// <summary>
		/// 删除分类
		/// </summary>
		/// <param name="categoryidList">删除分类的categoryid列表</param>
		/// <returns></returns>
		public bool DeleteSpaceCategory(string categoryidList, int userid)
		{
			try
			{
				//清除分类的categoryid列表相关信息
				string sqlstring = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "spacecategories] WHERE [categoryid] IN (" + categoryidList + ") AND [uid]=" + userid;
				DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

				//清除分类的categoryid关联表
				sqlstring = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "spacepostcategories] WHERE [categoryid] IN (" + categoryidList + ")";
				DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

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
				string sqlstring = "SELECT [categoryid] FROM [" + BaseConfigs.GetTablePrefix + "spacecategories] WHERE [uid]=" + userid;
				DataTable dt = DbHelper.ExecuteDataset(CommandType.Text,sqlstring).Tables[0];
				string categoryidList = "";
				foreach (DataRow dr in dt.Rows)
				{
					categoryidList += dr["categoryid"].ToString();
				}
				if (categoryidList != "")
				{
					categoryidList = categoryidList.Substring(0, categoryidList.Length - 1);
					//清除分类的categoryid关联表
					sqlstring = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "spacepostcategories] WHERE [categoryid] IN (" + categoryidList + ")";
					DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
				}

				sqlstring = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "spacecategories] WHERE [uid]=" + userid;
				DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

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
				IDataParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
				int pageTop = (currentPage - 1) * pageSize;
				string sql = "";
				if (currentPage == 1)
				{
					sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
						+ "[" + BaseConfigs.GetTablePrefix + "spacecategories] WHERE uid=@userid ORDER BY [categoryid] DESC";
				}
				else
				{
					sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
						+ "[" + BaseConfigs.GetTablePrefix + "spacecategories] WHERE [categoryid] < (SELECT min([categoryid])  FROM "
						+ "(SELECT TOP " + pageTop + " [categoryid] FROM [" + BaseConfigs.GetTablePrefix + "spacecategories] WHERE "
						+ "[uid]=@userid ORDER BY [categoryid] DESC) AS tblTmp ) AND [uid]=@userid ORDER BY [categoryid] DESC";
				}
				return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
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
				IDataParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
				return (int)DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([categoryid]) FROM [" + BaseConfigs.GetTablePrefix + "spacecategories] WHERE [uid]=@userid", parm);
			}
			catch
			{
				return 0;
			}
		}


		#endregion


		#region 日志关联类型 操作类
		public bool AddSpacePostCategory(SpacePostCategoryInfo spacepostcategories)
		{
			//try
			//{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4,spacepostcategories.ID),
					DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4,spacepostcategories.PostID),
					DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4,spacepostcategories.CategoryID)
				};
			string sqlstring = String.Format("INSERT INTO [" + BaseConfigs.GetTablePrefix + "spacepostcategories] ([postid], [categoryid]) VALUES ( @postid, @categoryid)");

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}

		}

		public bool SaveSpacePostCategory(SpacePostCategoryInfo spacepostcategories)
		{
			//try
			//{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4,spacepostcategories.ID),
					DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4,spacepostcategories.PostID),
					DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4,spacepostcategories.CategoryID)
				};
			string sqlstring = String.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "spacepostcategories] SET [postid] = @postid, [categoryid] = @categoryid WHERE  [id] = @id");

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}

		}

		public bool DeleteSpacePostCategoryByPostID(int postid)
		{
			//try
			//{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@postid", (DbType)SqlDbType.Int, 4,postid)
				};
			string sqlstring = String.Format("DELETE FROM [" + BaseConfigs.GetTablePrefix + "spacepostcategories] WHERE [postid] = @postid");

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

			return true;
			//}
			//catch
			//{
			//    return false;
			//}

		}

		#endregion


		#region 日志附件 操作类
		public bool AddSpaceAttachment(SpaceAttachmentInfo spaceattachments)
		{
			//try
			//{
			IDataParameter[] prams = 
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
			string sqlstring = String.Format("INSERT INTO [" + BaseConfigs.GetTablePrefix + "spaceattachments] ( [uid], [spacepostid], [postdatetime], [filename], [filetype], [filesize], [attachment], [downloads]) VALUES ( @uid, @spacepostid, @postdatetime, @filename, @filetype, @filesize, @attachment, @downloads)");

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}
		}

		public bool SaveSpaceAttachment(SpaceAttachmentInfo spaceattachments)
		{
			//try
			//{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@aid", (DbType)SqlDbType.Int, 4,spaceattachments.AID),
					DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,spaceattachments.UID),
					DbHelper.MakeInParam("@spacepostid", (DbType)SqlDbType.Int, 4,spaceattachments.SpacePostID),
					DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8,spaceattachments.PostDateTime),
					DbHelper.MakeInParam("@filename", (DbType)SqlDbType.NChar, 100,spaceattachments.FileName),
					DbHelper.MakeInParam("@filetype", (DbType)SqlDbType.NChar, 50,spaceattachments.FileType),
					DbHelper.MakeInParam("@filesize", (DbType)SqlDbType.Float, 8,spaceattachments.FileSize),
					DbHelper.MakeInParam("@attachment", (DbType)SqlDbType.NChar, 100,spaceattachments.Attachment),
					DbHelper.MakeInParam("@attachment", (DbType)SqlDbType.Int, 4,spaceattachments.Downloads)
				};
			string sqlstring = String.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "spaceattachments]  SET [uid] = @uid, [spacepostid] = @spacepostid, [postdatetime] = @postdatetime, [filename] = @filename, [filetype] = @filetype, [filesize] = @filesize, [attachment] = @attachment, [downloads] = @downloads  WHERE [aid] = @aid");

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}
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
				IDataParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
				int pageTop = (currentPage - 1) * pageSize;
				string sql = "";
				if (currentPage == 1)
				{
					sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
						+ "[" + BaseConfigs.GetTablePrefix + "spaceattachments] WHERE [uid]=@userid ORDER BY [aid] DESC";
				}
				else
				{
					sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
						+ "[" + BaseConfigs.GetTablePrefix + "spaceattachments] WHERE [aid] < (SELECT min([aid])  FROM "
						+ "(SELECT TOP " + pageTop + " [aid] FROM [" + BaseConfigs.GetTablePrefix + "spaceattachments] WHERE "
						+ "[uid]=@userid ORDER BY [aid] DESC) AS tblTmp ) AND [uid]=@userid ORDER BY [aid] DESC";
				}
				return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
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
				IDataParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
				return (int)DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([aid]) FROM [" + BaseConfigs.GetTablePrefix + "spaceattachments] WHERE [uid]=@userid", parm);
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
		/// <returns></returns>
		public bool DeleteSpaceAttachmentByIDList(string aidlist, int userid)
		{
			//			IDataParameter[] prams = 
			//			{
			//						DbHelper.MakeInParam("@aidlist", (DbType)SqlDbType.VarChar, 1000, aidlist)
			//			};


			IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT [filename] FROM [" + BaseConfigs.GetTablePrefix + "spaceattachments] WHERE [aid] IN( " + aidlist + " ) AND [uid]=" + userid, null);

			if (reader != null)
			{
				string path = Utils.GetMapPath(BaseConfigs.GetForumPath);
				while (reader.Read())
				{
					try
					{
						System.IO.File.Delete(path + reader[0].ToString());
					}
					catch
					{ ;}
				}
				reader.Close();
			}

			DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE  FROM  [" + BaseConfigs.GetTablePrefix + "spaceattachments] WHERE [aid] IN( " + aidlist + " )", null);

			return true;
		}

		#endregion


		#region 友情链接 操作类

		/// <summary>
		/// 返回满足条件的友情链接数
		/// </summary>
		/// <param name="userid"></param>
		/// <returns></returns>
		public int GetSpaceLinkCount(int userid)
		{
			try
			{
				IDataParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
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
				IDataParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
				int pageTop = (currentPage - 1) * pageSize;
				string sql = "";
				if (currentPage == 1)
				{
					sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
						+ "[" + BaseConfigs.GetTablePrefix + "spacelinks] WHERE [userid]=@userid ORDER BY [linkid] DESC";
				}
				else
				{
					sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
						+ "[" + BaseConfigs.GetTablePrefix + "spacelinks] WHERE [linkid] < (SELECT min([linkid])  FROM "
						+ "(SELECT TOP " + pageTop + " [linkid] FROM [" + BaseConfigs.GetTablePrefix + "spacelinks] WHERE "
						+ "[userid]=@userid ORDER BY [linkid] DESC) AS tblTmp ) AND [userid]=@userid ORDER BY [linkid] DESC";
				}
				return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
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
			//try
			//{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@linkid", (DbType)SqlDbType.Int, 4,spacelinks.LinkId),
					DbHelper.MakeInParam("@linktitle", (DbType)SqlDbType.NVarChar, 50,spacelinks.LinkTitle),
					DbHelper.MakeInParam("@linkurl", (DbType)SqlDbType.VarChar,255,spacelinks.LinkUrl),
					DbHelper.MakeInParam("@description", (DbType)SqlDbType.NVarChar, 200,spacelinks.Description),
			};
			string sqlstring = String.Format("UPDATE [" + BaseConfigs.GetTablePrefix + "spacelinks] SET  [linktitle] = @linktitle, [linkurl] = @linkurl, [description] = @description WHERE [linkid] = @linkid ");

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}

		}

		public bool AddSpaceLink(SpaceLinkInfo spacelinks)
		{
			//try
			//{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@linkid", (DbType)SqlDbType.Int, 4,spacelinks.LinkId),
					DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4,spacelinks.UserId),
					DbHelper.MakeInParam("@linktitle", (DbType)SqlDbType.NVarChar, 50,spacelinks.LinkTitle),
					DbHelper.MakeInParam("@linkurl", (DbType)SqlDbType.VarChar,255,spacelinks.LinkUrl),
					DbHelper.MakeInParam("@description", (DbType)SqlDbType.NVarChar, 200,spacelinks.Description),
			};
			string sqlstring = String.Format("INSERT INTO [" + BaseConfigs.GetTablePrefix + "spacelinks] ( [userid], [linktitle], [linkurl], [description]) VALUES ( @userid, @linktitle, @linkurl,  @description)");

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}

		}

		/// <summary>
		/// 删除友情链接
		/// </summary>
		/// <param name="linksList">删除友情链接的linkid列表</param>
		/// <returns></returns>
		public bool DeleteSpaceLink(string linksList, int userid)
		{
			try
			{
				string sqlstring = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "spacelinks] WHERE [linkid] IN (" + linksList + ") AND userid=" + userid;
				DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

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
				string sqlstring = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "spacelinks] WHERE userid=" + userid;
				DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

				return true;
			}
			catch
			{
				return false;
			}
		}
		#endregion

		#region	相册 操作类

		public int GetSpaceAlbumsCount(int userid)
		{
			try
			{
				IDataParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
				return (int)DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([albumid]) FROM [" + BaseConfigs.GetTablePrefix + "albums] WHERE [userid]=@userid", parm);
			}
			catch
			{
				return 0;
			}
		}

		public bool CountAlbumByAlbumID(int albumid)
		{
			//try
			//{
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "albums] SET [views] = [views] + 1 WHERE [albumid] = " + albumid);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}
		}

		public DataTable SpaceAlbumsList(int pageSize, int currentPage, int userid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
			int pageTop = (currentPage - 1) * pageSize;
			string sql = "";
			if (currentPage == 1)
			{
				sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
					+ "[" + BaseConfigs.GetTablePrefix + "albums] WHERE [userid]=@userid ORDER BY [albumid] DESC";
			}
			else
			{
				sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
					+ "[" + BaseConfigs.GetTablePrefix + "albums] WHERE [albumid] < (SELECT min([albumid])  FROM "
					+ "(SELECT TOP " + pageTop + " [albumid] FROM [" + BaseConfigs.GetTablePrefix + "albums] WHERE "
					+ "[userid]=@userid ORDER BY [albumid] DESC) AS tblTmp ) AND [userid]=@userid ORDER BY [albumid] DESC";
			}
			return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
		}

		public IDataReader SpaceAlbumsList(int userid, int albumcategoryid, int pageSize, int currentPage)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userid),
									  DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int, 4, pageSize),
									  DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int, 4, currentPage),
									  DbHelper.MakeInParam("@albumcateid", (DbType)SqlDbType.Int, 4, albumcategoryid)
								  };

			return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getalbumlist", parms);
		}

		public int SpaceAlbumsListCount(int userid,int albumcategoryid)
		{
			string sql = string.Format("SELECT COUNT(1) FROM [{0}albums] WHERE [imgcount]>0 ", BaseConfigs.GetTablePrefix);
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int,  4, userid),
									  DbHelper.MakeInParam("@albumcateid", (DbType)SqlDbType.Int, 4, albumcategoryid)
								  };
			if (userid > 0)
			{
				sql += " AND [userid]=@userid";
			}

			if (albumcategoryid != 0)
			{
				sql += " AND [albumcateid]=@albumcateid";
			}

			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parms), 0);
		}

		public IDataReader GetSpaceAlbumById(int albumId)
		{
			IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "albums] WHERE [albumid]=" + albumId);
			return reader;
		}

		public DataTable GetSpaceAlbumByUserId(int userid)
		{
			DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "albums] WHERE [userid]=" + userid).Tables[0];
			return dt;
		}


		public IDataReader GetRecommendAlbumList(string idlist)
		{
			if (!Utils.IsNumericArray(idlist.Split(',')))
			{
				return null;
			}

			string sql = string.Format("SELECT * FROM [{0}albums] WHERE [albumid] IN ({1})", BaseConfigs.GetTablePrefix, idlist);

			return DbHelper.ExecuteReader(CommandType.Text, sql);
		}

		public bool AddSpaceAlbum(AlbumInfo spaceAlbum)
		{
			//try
			//{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4,spaceAlbum.Userid),
					DbHelper.MakeInParam("@albumcateid", (DbType)SqlDbType.Int, 4,spaceAlbum.Albumcateid),
					DbHelper.MakeInParam("@title", (DbType)SqlDbType.NChar, 50,spaceAlbum.Title),
					DbHelper.MakeInParam("@description", (DbType)SqlDbType.NChar, 200,spaceAlbum.Description),
					DbHelper.MakeInParam("@password", (DbType)SqlDbType.NChar, 50,spaceAlbum.Password),
					DbHelper.MakeInParam("@type", (DbType)SqlDbType.Int, 8,spaceAlbum.Type),
					DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar, 20, spaceAlbum.Username)
					//DbHelper.MakeInParam("@creatdatetime", (DbType)SqlDbType.DateTime, 8,spaceAlbum.Createdatetime)
				};
			string sqlstring = String.Format("INSERT INTO [{0}albums] ([userid], [username], [albumcateid], [title], [description], [password], [type]) VALUES ( @userid, @username, @albumcateid, @title, @description, @password, @type)", BaseConfigs.GetTablePrefix);

			//向关联表中插入相关数据
			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}
		}

		public bool SaveSpaceAlbum(AlbumInfo spaceAlbum)
		{
			//try
			//{
			IDataParameter[] prams = 
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
			string sqlstring = String.Format("UPDATE [{0}albums] SET [albumcateid] = @albumcateid, [title] = @title, [description] = @description, [password] = @password, [imgcount] = @imgcount, [logo] = @logo, [type] = @type WHERE [albumid] = @albumid", BaseConfigs.GetTablePrefix);

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}
		}

		public void UpdateAlbumViews(int albumid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4, albumid);
			string sql = string.Format("UPDATE [{0}albums] SET [views]=[views]+1 WHERE [albumid]=@albumid", BaseConfigs.GetTablePrefix);
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
		}

		public bool DeleteSpaceAlbum(int albumId, int userid)
		{
			//try
			//{
			//删除照片及文件
			string sqlstring = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "albums] WHERE [albumid]=" + albumId + " AND [userid]=" + userid;
			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}
		}

        

#if NET1

		public AlbumCategoryInfoCollection GetAlbumCategory()
		{
			string sql = string.Format("SELECT * FROM [{0}albumcategories] ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);

			IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql);

			AlbumCategoryInfoCollection acic = new AlbumCategoryInfoCollection();

			if (reader != null)
			{
				while (reader.Read())
				{
					AlbumCategoryInfo aci = new AlbumCategoryInfo();
					aci.Albumcateid = Utils.StrToInt(reader["albumcateid"], 0);
					aci.Albumcount = Utils.StrToInt(reader["albumcount"], 0);
					aci.Description = reader["description"].ToString();
					aci.Displayorder = Utils.StrToInt(reader["displayorder"], 0);
					aci.Title = reader["title"].ToString();
					acic.Add(aci);
				}
				reader.Close();
			}
			return acic;
		}

#else

        public Discuz.Common.Generic.List<AlbumCategoryInfo> GetAlbumCategory()
        {
            string sql = string.Format("SELECT * FROM [{0}albumcategories] ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);

            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql);

            Discuz.Common.Generic.List<AlbumCategoryInfo> acic = new Discuz.Common.Generic.List<AlbumCategoryInfo>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    AlbumCategoryInfo aci = new AlbumCategoryInfo();
                    aci.Albumcateid = Utils.StrToInt(reader["albumcateid"], 0);
                    aci.Albumcount = Utils.StrToInt(reader["albumcount"], 0);
                    aci.Description = reader["description"].ToString();
                    aci.Displayorder = Utils.StrToInt(reader["displayorder"], 0);
                    aci.Title = reader["title"].ToString();
                    acic.Add(aci);
                }
                reader.Close();
            }
            return acic;
        }
#endif
		#endregion

		#region 照片 操作类

		/// <summary>
		/// 获取图片集合
		/// </summary>
		/// <param name="userid">用户Id,必须指定一个用户,不能为0</param>
		/// <param name="albumid">相册Id，当为0时表示此用户所有相册</param>
		/// <param name="count">取出的数量</param>
		/// <returns></returns>
		public IDataReader GetPhotoListByUserId(int userid, int albumid, int count)
		{
			string sql = string.Format("SELECT TOP {0} [p].* FROM [{1}photos] [p],[{1}albums] [a] WHERE [a].[albumid] = [p].[albumid] AND [a].[type]=0 AND [p].[userid]=@userid", count, BaseConfigs.GetTablePrefix);

			if (albumid > 0)
			{
				sql += " AND [p].[albumid]=@albumid";
			}

			sql += " ORDER BY [p].[postdate] DESC";

			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
									  DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4, albumid)
								  };

			return DbHelper.ExecuteReader(CommandType.Text, sql, parms);
		}

		/// <summary>
		/// 获得图片排行图集合
		/// </summary>
		/// <param name="type">排行方式，0浏览量，1评论数,2上传时间，3收藏数</param>
		/// <returns></returns>
		public IDataReader GetPhotoRankList(int type, int photocount)
		{
			string sql = string.Format("SELECT TOP {0} [p].* FROM [{1}photos] [p],[{1}albums] [a] WHERE [a].[albumid] = [p].[albumid] AND [a].[type]=0",
				photocount, BaseConfigs.GetTablePrefix);

			switch (type)
			{
				case 0:
					sql += " ORDER BY [p].[views] DESC";
					break;
				case 1:
					sql += " ORDER BY [p].[comments] DESC";
					break;
				case 2:
					sql += " ORDER BY [p].[postdate] DESC";
					break;
				case 3:
					sql = string.Format(@"SELECT * FROM [{0}albums] WHERE albumid IN (SELECT TOP {1} [tid] 
		                                                                FROM [{0}favorites]
		                                                                WHERE  [typeid]=1 AND [tid] in (SELECT [albumid] 
                                                                                                        FROM [{0}albums] 
                                                                                                        WHERE [type]=0) 
		                                                                GROUP BY [tid] 
		                                                                ORDER BY COUNT([tid]) DESC)", BaseConfigs.GetTablePrefix, photocount);
					break;
				default:
					sql += " ORDER BY [p].[views] DESC";
					break;
			}

			return DbHelper.ExecuteReader(CommandType.Text, sql);
		}

		public IDataReader GetFocusPhotoList(int type, int focusphotocount, int validDays)
		{
			//IDataParameter parm = DbHelper.MakeInParam("@validDays", (DbType)SqlDbType.Int, 4, validDays);
			//string sql = string.Format("SELECT TOP {0} [p].* FROM [{1}photos] [p],[{1}albums] [a] WHERE DATEDIFF(d, getdate(), [postdate]) < @validDays AND [a].[albumid] = [p].[albumid] AND [a].[type]=0",
			//                            focusphotocount, BaseConfigs.GetTablePrefix);
			//switch (type)
			//{
			//    case 0:
			//        sql += " ORDER BY [p].[views] DESC";
			//        break;
			//    case 1:
			//        sql += " ORDER BY [p].[comments] DESC";
			//        break;
			//    case 2:
			//        sql += " ORDER BY [p].[postdate] DESC";
			//        break;
			//    default:
			//        sql += " ORDER BY [p].[views] DESC";
			//        break;
			//}
			//return DbHelper.ExecuteReader(CommandType.Text, sql, parm);
			return GetFocusPhotoList(type, focusphotocount, validDays, -1);
		}


		public IDataReader GetFocusPhotoList(int type, int focusphotocount, int validDays, int uid)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@validDays", (DbType)SqlDbType.Int, 4, validDays),
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
								  };
			string sql = string.Format("SELECT TOP {0} [p].* FROM [{1}photos] [p],[{1}albums] [a] WHERE DATEDIFF(d,  [postdate], getdate()) < @validDays AND [a].[albumid] = [p].[albumid] AND [a].[type]=0{2}",
				focusphotocount, BaseConfigs.GetTablePrefix, uid > 1 ? " AND [p].[userid] =@uid" : string.Empty);
			switch (type)
			{
				case 0:
					sql += " ORDER BY [p].[views] DESC";
					break;
				case 1:
					sql += " ORDER BY [p].[comments] DESC";
					break;
				case 2:
					sql += " ORDER BY [p].[postdate] DESC";
					break;
				default:
					sql += " ORDER BY [p].[views] DESC";
					break;
			}
			return DbHelper.ExecuteReader(CommandType.Text, sql, parms);
		}


		public IDataReader GetRecommendPhotoList(string idlist)
		{
			if (!Utils.IsNumericArray(idlist.Split(',')))
			{
				return null;
			}

			string sql = string.Format("SELECT [p].* FROM [{0}photos] [p],[{0}albums] [a] WHERE [p].[albumid] = [a].[albumid] AND [a].[type]=0 AND [p].[photoid] IN ({1}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[p].[photoid]),'{1}')", BaseConfigs.GetTablePrefix, idlist);

			return DbHelper.ExecuteReader(CommandType.Text, sql);
		}

		public bool AddSpacePhoto(PhotoInfo photoinfo)
		{
			//try
			//{
			IDataParameter[] prams = 
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
					//DbHelper.MakeInParam("@creatdatetime", (DbType)SqlDbType.DateTime, 8,spaceAlbum.Createdatetime)
				};
			string sqlstring = String.Format("INSERT INTO [{0}photos] ([userid], [username], [title], [albumid], [filename], [attachment], [filesize], [description],[isattachment],[commentstatus], [tagstatus]) VALUES ( @userid, @username, @title, @albumid, @filename, @attachment, @filesize, @description,@isattachment, @commentstatus, @tagstatus)", BaseConfigs.GetTablePrefix);

			//向关联表中插入相关数据
			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);

			return true;
			//}
			//catch (Exception ex)
			//{
			//    errormsg = Globals.TransferSqlErrorInfo(ex.Message);
			//    return false;
			//}
		}

		/// <summary>
		/// 更新图片信息(仅更新 标题、描述、评论设置和标签设置4项)
		/// </summary>
		/// <param name="photo"></param>
		public void UpdatePhotoInfo(PhotoInfo photo)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@photoid", (DbType)SqlDbType.Int, 4, photo.Photoid),
									  DbHelper.MakeInParam("@title", (DbType)SqlDbType.NChar, 20, photo.Title),
									  DbHelper.MakeInParam("@description", (DbType)SqlDbType.NChar, 200, photo.Description),
									  DbHelper.MakeInParam("@commentstatus", (DbType)SqlDbType.TinyInt, 1, (byte)photo.Commentstatus),
									  DbHelper.MakeInParam("@tagstatus", (DbType)SqlDbType.TinyInt, 1, (byte)photo.Tagstatus)
								  };

			string sql = string.Format("UPDATE [{0}photos] SET [title]=@title, [description]=@description, [commentstatus]=@commentstatus, [tagstatus]=@tagstatus WHERE [photoid]=@photoid", BaseConfigs.GetTablePrefix);

			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		/// <summary>
		/// 通过相册ID得到相册中所有图片的信息
		/// </summary>
		/// <param name="albumid">相册ID</param>
		/// <param name="errormsg"></param>
		/// <returns></returns>
		public DataTable GetSpacePhotoByAlbumID(int albumid)
		{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4,albumid)
				};
			string sqlstring = String.Format("SELECT * FROM [" + BaseConfigs.GetTablePrefix + "photos] WHERE [albumid] = @albumid");

			//向关联表中插入相关数据
			DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring, prams).Tables[0];

			return dt;
		}

		/// <summary>
		/// 获得照片信息
		/// </summary>
		/// <param name="photoid">图片Id</param>
		/// <param name="albumid">相册Id</param>
		/// <param name="mode">模式,0=当前图片,1上一张,2下一张</param>
		/// <returns></returns>
		public IDataReader GetPhotoByID(int photoid, int albumid, byte mode)
		{
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@photoid", (DbType)SqlDbType.Int, 4,photoid),
					DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4, albumid)
				};
			string sqlstring;

			switch (mode)
			{
				case 1:
					sqlstring = String.Format("SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "photos] WHERE [albumid] = @albumid AND [photoid]<@photoid ORDER BY [photoid] DESC");
					break;
				case 2:
					sqlstring = String.Format("SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "photos] WHERE [albumid] = @albumid AND [photoid]>@photoid ORDER BY [photoid] ASC");
					break;
				default:
					sqlstring = String.Format("SELECT * FROM [" + BaseConfigs.GetTablePrefix + "photos] WHERE [photoid] = @photoid");
					break;
			}
			//向关联表中插入相关数据
			IDataReader idr = DbHelper.ExecuteReader(CommandType.Text, sqlstring, prams);

			return idr;
		}

		public void UpdatePhotoViews(int photoid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@photoid", (DbType)SqlDbType.Int, 4, photoid);
			string sql = string.Format("UPDATE [{0}photos] SET [views]=[views]+1 WHERE [photoid]=@photoid", BaseConfigs.GetTablePrefix);

			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
		}

		public void UpdatePhotoComments(int photoid, int count)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@photoid", (DbType)SqlDbType.Int, 4, photoid),
									  DbHelper.MakeInParam("@count", (DbType)SqlDbType.Int, 4, count),
			};
			string commandText = string.Format("UPDATE [{0}photos] SET [comments]=[comments]+@count WHERE [photoid]=@photoid", BaseConfigs.GetTablePrefix);

			DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
		}

		public int GetSpacePhotosCount(int albumid)
		{
			try
			{
				IDataParameter parm = DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4, albumid);
				return (int)DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([photoid]) FROM [" + BaseConfigs.GetTablePrefix + "photos] WHERE [albumid]=@albumid", parm);
			}
			catch
			{
				return 0;
			}
		}

		public DataTable SpacePhotosList(int pageSize, int currentPage, int userid, int albumid)
		{
			//try
			//{
			//"userid=" + userid + " AND albumid=" + albumid
			IDataParameter[] prams = 
				{
					DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4,userid),
					DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4,albumid)
				};
			int pageTop = (currentPage - 1) * pageSize;
			string sql = "";
			if (currentPage == 1)
			{
				sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
					+ "[" + BaseConfigs.GetTablePrefix + "photos] WHERE [userid]=@userid AND [albumid]=@albumid ORDER BY [photoid] ASC";
			}
			else
			{
				sql = "SELECT TOP " + pageSize.ToString() + " * FROM "
					+ "[" + BaseConfigs.GetTablePrefix + "photos] WHERE [photoid] > (SELECT MAX([photoid])  FROM "
					+ "(SELECT TOP " + pageTop + " [photoid] FROM [" + BaseConfigs.GetTablePrefix + "photos] WHERE "
					+ "[userid]=@userid AND [albumid]=@albumid ORDER BY [photoid] ASC) AS tblTmp ) AND [userid]=@userid "
					+ "AND [albumid]=@albumid ORDER BY [photoid] ASC";
			}
			return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
			//}
			//catch
			//{
			//    return new DataTable();
			//}
		}

		public DataTable SpacePhotosList(int albumid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4, albumid);
			string sql = sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "photos] WHERE [albumid]=@albumid ORDER BY [photoid] ASC";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
		}

		public bool DeleteSpacePhotoByIDList(string photoidlist, int albumid, int userid)
		{
			if (photoidlist == "")
				return false;
			IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT [filename],[isattachment] FROM [" + BaseConfigs.GetTablePrefix + "photos] WHERE [photoid] IN( " + photoidlist + " ) AND [userid]=" + userid, null);

			if (reader != null)
			{
				while (reader.Read())
				{
					try
					{
						string file = Utils.GetMapPath(BaseConfigs.GetForumPath + reader["filename"].ToString());
						if (reader["isattachment"].ToString() == "0")    //如果是附件图片，则不删除原图，但缩略图、方图将被删除
						{
							System.IO.File.Delete(file);
						}
						string thumbnailimg = file.Replace(Path.GetExtension(file), "_thumbnail" + Path.GetExtension(file));
						if (File.Exists(thumbnailimg))
							File.Delete(thumbnailimg);
						string squareimg = file.Replace(Path.GetExtension(file), "_square" + Path.GetExtension(file));
						if (File.Exists(squareimg))
							File.Delete(squareimg);
					}
					catch
					{ }
				}
				reader.Close();
			}

			DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "photos] WHERE [photoid] IN( " + photoidlist + " ) AND [userid]=" + userid, null);

			return true;
		}

		public int ChangeAlbum(int targetAlbumId, string photoIdList, int userid)
		{
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "photos] SET albumid=" + targetAlbumId + " WHERE photoid IN (" + photoIdList + ") AND [userid]=" + userid;
			return DbHelper.ExecuteNonQuery(CommandType.Text, sql);
		}

		public int GetPhotoSizeByUserid(int userid)
		{
			string sql = "SELECT ISNULL(SUM(filesize), 0) AS [filesize] FROM [" + BaseConfigs.GetTablePrefix + "photos] WHERE userid=" + userid;
			//object o = DbHelper.ExecuteScalar(CommandType.Text,sql);
			return (int)DbHelper.ExecuteScalar(CommandType.Text, sql);
		}

		public int GetSpacePhotoCountByAlbumId(int albumid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4, albumid);
			string sql = string.Format("SELECT COUNT(1) FROM [{0}photos] WHERE [albumid]=@albumid", BaseConfigs.GetTablePrefix);
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parm), 0);
		}

		public DataTable GetPhotosByAlbumid(int albumid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4, albumid);
			string commandText = string.Format("SELECT [photoid], [userid], [username], [title], [filename] FROM [{0}photos] WHERE [albumid]=@albumid", BaseConfigs.GetTablePrefix);
			return DbHelper.ExecuteDataset(CommandType.Text, commandText, parm).Tables[0];
		}
		#endregion

		public string GetThemeDropDownTreeSql()
		{
			return "SELECT [themeid], [name], [type] AS [parentid] FROM [" + BaseConfigs.GetTablePrefix + "spacethemes] ORDER BY [themeid]";
		}

		public string GetTemplateDropDownSql()
		{
			return "SELECT [templateid], [name]  FROM [" + BaseConfigs.GetTablePrefix + "spacetemplates] ORDER BY [templateid]";
		}

		public string GetCategoryCheckListSql(int userid)
		{
			return "SELECT [categoryid], [title] FROM [" + BaseConfigs.GetTablePrefix + "spacecategories] WHERE [uid]=" + userid + " ORDER BY [displayorder], [categoryid]";
		}

		#region 对ThemeInfo的操作
		public IDataReader GetThemeInfos()
		{
			string sql = string.Format(@"SELECT * FROM [{0}spacethemes] ORDER BY [type]", BaseConfigs.GetTablePrefix);

			return RunSelectSql(sql, null);
		}

		public IDataReader GetThemeInfoById(int themeId)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@themeid", (DbType)SqlDbType.Int, 4, themeId)
			};

			string sql = string.Format(@"SELECT * FROM [{0}spacethemes] WHERE [themeid] = @themeid", BaseConfigs.GetTablePrefix);

			return RunSelectSql(sql, parms);
		}

		#endregion

		#region 对TemplateInfo的操作

		public IDataReader GetTemplateInfos()
		{
			string sql = string.Format(@"SELECT * FROM [{0}spacetemplates]", BaseConfigs.GetTablePrefix);

			return RunSelectSql(sql, null);
		}
		public IDataReader GetTemplateInfoById(int templateInfoId)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@templateid", (DbType)SqlDbType.Int, 4, templateInfoId)	
			};

			string sql = string.Format(@"SELECT * FROM [{0}spacetemplates] WHERE [templateid] = @templateid", BaseConfigs.GetTablePrefix);

			return RunSelectSql(sql, parms);
		}

		#endregion

		#region 对spacemoduledefs表的操作
		public IDataReader GetModuleDefInfoById(int moduleDefInfoId)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@moduledefid", (DbType)SqlDbType.Int, 4, moduleDefInfoId)	
			};

			string sql = string.Format(@"SELECT * FROM [{0}spacemoduledefs] WHERE [moduledefid] = @moduledefid", BaseConfigs.GetTablePrefix);

			return RunSelectSql(sql, parms);
		}


		/// <summary>
		/// 添加ModuleDef信息至数据库
		/// </summary>
		/// <param name="moduleDefInfo"></param>
		/// <returns></returns>
		public bool AddModuleDef(ModuleDefInfo moduleDefInfo)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@modulename", (DbType)SqlDbType.NVarChar, 20, moduleDefInfo.ModuleName),
				DbHelper.MakeInParam("@cachetime", (DbType)SqlDbType.Int, 4, moduleDefInfo.CacheTime),
				DbHelper.MakeInParam("@configfile", (DbType)SqlDbType.VarChar, 100, moduleDefInfo.ConfigFile),
				DbHelper.MakeInParam("@controller", (DbType)SqlDbType.VarChar, 255, moduleDefInfo.BussinessController),
			};

			string sql = string.Format(@"INSERT INTO [{0}spacemoduledefs]([modulename], [cachetime], [configfile], [controller]) VALUES(@moduledefid, @modulename, @cachetime, @configfile, @controller)", BaseConfigs.GetTablePrefix);
			return RunExecuteSql(sql, parms);
		}

		/// <summary>
		/// 修改指定的ModuleDef信息
		/// </summary>
		/// <param name="moduleDefInfo"></param>
		/// <returns></returns>
		public bool UpdateModuleDef(ModuleDefInfo moduleDefInfo)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@moduledefid", (DbType)SqlDbType.Int, 4, moduleDefInfo.ModuleDefID),
				DbHelper.MakeInParam("@modulename", (DbType)SqlDbType.NVarChar, 20, moduleDefInfo.ModuleName),
				DbHelper.MakeInParam("@cachetime", (DbType)SqlDbType.Int, 4, moduleDefInfo.CacheTime),
				DbHelper.MakeInParam("@configfile", (DbType)SqlDbType.VarChar, 100, moduleDefInfo.ConfigFile),
				DbHelper.MakeInParam("@controller", (DbType)SqlDbType.VarChar, 255, moduleDefInfo.BussinessController),
			};

			string sql = string.Format(@"UPDATE [{0}spacemoduledefs] SET [modulename]=@modulename, [cachetime]=@cachetime, [configfile]=@configfile, [controller]=@controller WHERE [moduledefid]=@moduledefid", BaseConfigs.GetTablePrefix);

			return RunExecuteSql(sql, parms);
		}

		/// <summary>
		/// 删除指定的ModuleDef信息
		/// </summary>
		/// <param name="moduleDefId"></param>
		/// <returns></returns>
		public bool DeleteModuleDef(int moduleDefId)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@moduledefid", (DbType)SqlDbType.Int, 4, moduleDefId),
			};

			string sql = string.Format(@"DELETE FROM [{0}spacemoduledefs] WHERE [moduledefid]=@moduledefid", BaseConfigs.GetTablePrefix);

			return RunExecuteSql(sql, parms);
		}


		public int GetModuleDefIdByUrl(string url)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@configfile", (DbType)SqlDbType.VarChar, 100, url),
			};

			string commandText = string.Format(@"SELECT [moduledefid] FROM [{0}spacemoduledefs] WHERE [configfile]=@configfile", BaseConfigs.GetTablePrefix);

			string str = DbHelper.ExecuteScalarToStr(CommandType.Text, commandText, parms);
			return str == string.Empty ? 0 : Convert.ToInt32(str);
		}

		#endregion

		#region 对spacemodules表的操作

		public int GetModulesCountByTabId(int tabId, int uid)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@tabid", (DbType)SqlDbType.Int, 4, tabId),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
			};

			string sql = string.Format(@"SELECT COUNT(1) FROM [{0}spacemodules] WHERE [tabid] = @tabid AND [uid]=@uid", BaseConfigs.GetTablePrefix);

			int reval = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parms), 0);

			return reval;
		}

		/// <summary>
		/// 根据TabId获得Modules集合
		/// </summary>
		/// <param name="tabId"></param>
		/// <returns></returns>
		public IDataReader GetModulesByTabId(int tabId, int uid)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@tabid", (DbType)SqlDbType.Int, 4, tabId),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)	
			};

			string sql = string.Format(@"SELECT * FROM [{0}spacemodules] WHERE [tabid] = @tabid AND [uid]=@uid ORDER BY [panename], [displayorder]", BaseConfigs.GetTablePrefix);

			return RunSelectSql(sql, parms);
		}


		/// <summary>
		/// 根据ModuleId获得Module
		/// </summary>
		/// <param name="moduleInfoId"></param>
		/// <returns></returns>
		public IDataReader GetModuleInfoById(int moduleInfoId, int uid)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@moduleid", (DbType)SqlDbType.Int, 4, moduleInfoId),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
			};

			string sql = string.Format(@"SELECT * FROM [{0}spacemodules] WHERE [moduleid] = @moduleid AND [uid]=@uid", BaseConfigs.GetTablePrefix);

			return RunSelectSql(sql, parms);
		}


		/// <summary>
		/// 添加Module至数据库
		/// </summary>
		/// <param name="moduleInfo"></param>
		/// <returns></returns>
		public bool AddModule(ModuleInfo moduleInfo)
		{
			IDataParameter[] parms =
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

			string sql = string.Format(@"INSERT INTO [{0}spacemodules]([moduleid], [tabid], [uid], [moduledefid], [panename], [displayorder], [userpref], [val], [moduleurl], [moduletype]) VALUES(@moduleid, @tabid, @uid, @moduledefid, @panename, @displayorder, @userpref, @val, @moduleurl, @moduletype)", BaseConfigs.GetTablePrefix);

			return RunExecuteSql(sql, parms);
		}

		/// <summary>
		/// 更新指定的Module信息
		/// </summary>
		/// <param name="moduleInfo"></param>
		/// <returns></returns>
		public bool UpdateModule(ModuleInfo moduleInfo)
		{
			IDataParameter[] parms =
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

			string sql = string.Format(@"UPDATE [{0}spacemodules] SET [tabid]=@tabid, [moduledefid]=@moduledefid, [panename]=@panename, [displayorder]=@displayorder,[userpref]=@userpref,[val]=@val, moduleurl=@moduleurl, moduletype=@moduletype WHERE [moduleid]=@moduleid AND [uid]=@uid", BaseConfigs.GetTablePrefix);

			return RunExecuteSql(sql, parms);
		}

		/// <summary>
		/// 删除指定的Module信息
		/// </summary>
		/// <param name="moduleId"></param>
		/// <returns></returns>
		public bool DeleteModule(int moduleId, int uid)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@moduleid", (DbType)SqlDbType.Int, 4, moduleId),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
			};

			string sql = string.Format(@"DELETE FROM [{0}spacemodules] WHERE [moduleid]=@moduleid AND [uid]=@uid", BaseConfigs.GetTablePrefix);
			return RunExecuteSql(sql, parms);
		}

		public bool DeleteModule(int uid)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
			};

			string sql = string.Format(@"DELETE FROM [{0}spacemodules] WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);
			return RunExecuteSql(sql, parms);
		}

		/// <summary>
		/// 为模块排序
		/// </summary>
		/// <param name="mid"></param>
		/// <param name="panename"></param>
		/// <param name="displayorder"></param>
		public void UpdateModuleOrder(int mid, int uid, string panename, int displayorder)
		{
			IDataParameter[] parms =
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
			IDataParameter[] parms =
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
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userid)
			};
			string commandText = string.Format(@"SELECT TOP 1 [moduleid] FROM [{0}spacemodules] WHERE [uid]=@uid ORDER BY [moduleid] DESC", BaseConfigs.GetTablePrefix);
			return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms));
		}

		#endregion

		#region 对spacetabs表的操作

		/// <summary>
		/// 根据Uid获得Tab集合
		/// </summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		public IDataReader GetTabInfosByUid(int uid)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
			};

			string sql = string.Format(@"SELECT * FROM [{0}spacetabs] WHERE [uid]=@uid ORDER BY [tabid] ASC", BaseConfigs.GetTablePrefix);

			return RunSelectSql(sql, parms);
		}

		/// <summary>
		/// 根据TabId获得Tab
		/// </summary>
		/// <param name="tabInfoId"></param>
		/// <returns></returns>
		public IDataReader GetTabInfoById(int tabInfoId, int uid)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@tabid", (DbType)SqlDbType.Int, 4, tabInfoId),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)	
			};

			string sql = string.Format(@"SELECT * FROM [{0}spacetabs] WHERE [tabid] = @tabid AND [uid]=@uid", BaseConfigs.GetTablePrefix);

			return RunSelectSql(sql, parms);
		}

		/// <summary>
		/// 添加Tab信息至数据库
		/// </summary>
		/// <param name="tabInfo"></param>
		/// <returns></returns>
		public bool AddTab(TabInfo tabInfo)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@tabid", (DbType)SqlDbType.Int, 4, tabInfo.TabID),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, tabInfo.UserID),
				DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, tabInfo.DisplayOrder),
				DbHelper.MakeInParam("@tabname", (DbType)SqlDbType.NVarChar, 50, tabInfo.TabName),
				DbHelper.MakeInParam("@iconfile", (DbType)SqlDbType.VarChar, 50, tabInfo.IconFile),
				DbHelper.MakeInParam("@template", (DbType)SqlDbType.VarChar, 50, tabInfo.Template)
			};

			string sql = string.Format(@"INSERT INTO [{0}spacetabs]([tabid], [uid], [displayorder], [tabname], [iconfile], [template]) VALUES(@tabid, @uid, @displayorder, @tabname, @iconfile, @template)", BaseConfigs.GetTablePrefix);

			return RunExecuteSql(sql, parms);
		}

		/// <summary>
		/// 更新指定Tab信息
		/// </summary>
		/// <param name="tabInfo"></param>
		/// <returns></returns>
		public bool UpdateTab(TabInfo tabInfo)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@tabid", (DbType)SqlDbType.Int, 4, tabInfo.TabID),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, tabInfo.UserID),
				DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, tabInfo.DisplayOrder),
				DbHelper.MakeInParam("@tabname", (DbType)SqlDbType.NVarChar, 50, tabInfo.TabName),
				DbHelper.MakeInParam("@iconfile", (DbType)SqlDbType.VarChar, 50, tabInfo.IconFile),
				DbHelper.MakeInParam("@template", (DbType)SqlDbType.VarChar, 50, tabInfo.Template)
			};

			string sql = string.Format(@"UPDATE [{0}spacetabs] SET [displayorder]=@displayorder, [tabname]=@tabname, [iconfile]=@iconfile, [template] = @template WHERE [tabid]=@tabid AND [uid]=@uid", BaseConfigs.GetTablePrefix);

			return RunExecuteSql(sql, parms);
		}

		/// <summary>
		/// 删除Tab信息
		/// </summary>
		/// <param name="tabId"></param>
		/// <returns></returns>
		public bool DeleteTab(int tabId, int uid)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@tabid", (DbType)SqlDbType.Int, 4, tabId),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
			};

			string sql = string.Format(@"DELETE FROM [{0}spacetabs] WHERE [tabid]=@tabid AND [uid]=@uid", BaseConfigs.GetTablePrefix);

			return RunExecuteSql(sql, parms);
		}

		public bool DeleteTab(int uid)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
			};

			string sql = string.Format(@"DELETE FROM [{0}spacetabs] WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);

			return RunExecuteSql(sql, parms);
		}

		public int GetTabInfoCountByUserId(int userid)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userid),
			};
			string commandText = string.Format(@"SELECT COUNT(1) FROM [{0}spacetabs] WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms), 0);
		}

		public bool SetTabTemplate(int tabid, int uid, string template)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@tabid", (DbType)SqlDbType.Int, 4, tabid),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
				DbHelper.MakeInParam("@template", (DbType)SqlDbType.VarChar, 50, template)
			};

			string sql = string.Format(@"UPDATE [{0}spacetabs] SET [template] = @template WHERE [tabid]=@tabid AND [uid]=@uid", BaseConfigs.GetTablePrefix);

			return RunExecuteSql(sql, parms);
		}

		public int GetMaxTabIdByUid(int userid)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userid)
			};
			string commandText = string.Format(@"SELECT TOP 1 [tabid] FROM [{0}spacetabs] WHERE [uid]=@uid ORDER BY [tabid] DESC", BaseConfigs.GetTablePrefix);
			return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms));
		}

		#endregion

		#region config

		public void ClearDefaultTab(int userid)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid)
			};
			string commandText = string.Format("UPDATE [{0}spaceconfigs] SET [defaulttab]=0 WHERE [userid]=@userid", BaseConfigs.GetTablePrefix);
			DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

		}
		public void SetDefaultTab(int userid, int tabid)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
				DbHelper.MakeInParam("@tabid", (DbType)SqlDbType.Int, 4, tabid)
			};
			string commandText = string.Format("UPDATE [{0}spaceconfigs] SET [defaulttab]=@tabid WHERE [userid]=@userid", BaseConfigs.GetTablePrefix);
			DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
		}

		public void SetSpaceTheme(int userid, int themeid, string themepath)
		{
			IDataParameter[] parms =
			{
				DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
				DbHelper.MakeInParam("@themeid", (DbType)SqlDbType.Int, 4, themeid),
				DbHelper.MakeInParam("@themepath", (DbType)SqlDbType.VarChar, 50, themepath)
			};
			string commandText = string.Format("UPDATE [{0}spaceconfigs] SET [themeid]=@themeid, [themepath]=@themepath WHERE [userid]=@userid", BaseConfigs.GetTablePrefix);
			DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
		}

		#endregion

		/// <summary>
		/// 运行非Select语句
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="parms"></param>
		/// <returns></returns>
		private bool RunExecuteSql(string sql, IDataParameter[] parms)
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
		private IDataReader RunSelectSql(string sql, IDataParameter[] parms)
		{
			return DbHelper.ExecuteReader(CommandType.Text, sql, parms);
		}

		public DataRow GetThemes()
		{
			string sql = "SELECT TOP 1 newid() AS row,[themeid],[directory] FROM [" + BaseConfigs.GetTablePrefix + "spacethemes] WHERE type<>0 ORDER BY row";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0].Rows[0];
		}

		public DataTable GetUnActiveSpaceList()
		{
			string sql = "SELECT [uid],s.[spaceid],[spacetitle],[username],[createdatetime] FROM [" + BaseConfigs.GetTablePrefix + "spaceconfigs] s ";
			sql += "LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] u ON s.[userid]=u.[uid] ";
			sql += "WHERE s.[spaceid] IN (SELECT ABS([spaceid]) spaceid  FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [spaceid] < 0) ORDER BY s.[spaceid] DESC";
			return DbHelper.ExecuteDataset(sql).Tables[0];
		}

		public void DeleteSpaces(string uidlist)
		{
			DbHelper.ExecuteNonQuery("DELETE FROM [" + BaseConfigs.GetTablePrefix + "spaceconfigs] WHERE [userid] IN (" + uidlist + ")");
		}

		public void DeleteSpaceThemes(string themeidlist)
		{
			DbHelper.ExecuteNonQuery("DELETE FROM [" + BaseConfigs.GetTablePrefix + "spacethemes]  WHERE [themeid] IN(" + themeidlist + ")");
		}

		public void UpdateSpaceThemeInfo(int themeid, string name, string author, string copyright)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar, 50, name),
									  DbHelper.MakeInParam("@author", (DbType)SqlDbType.NVarChar, 100, author),
									  DbHelper.MakeInParam("@copyright", (DbType)SqlDbType.NVarChar, 100, copyright),
									  DbHelper.MakeInParam("@themeid", (DbType)SqlDbType.Int, 4, themeid)
								  };
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "spacethemes] SET [name]=@name, [author]=@author, [copyright]=@copyright WHERE themeid=@themeid";

			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public DataTable GetSpaceThemeDirectory()
		{
			return DbHelper.ExecuteDataset("SELECT [directory] FROM [" + BaseConfigs.GetTablePrefix + "spacethemes] WHERE [type]<>0").Tables[0];
		}

		public bool IsThemeExist(string name)
		{
			IDataParameter parm = DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar, 50, name);
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(*) FROM [" + BaseConfigs.GetTablePrefix + "spacethemes] WHERE name=@name", parm), 0) > 0;
		}

		public bool IsThemeExist(string name, int themeid)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar, 50, name),
									  DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, themeid)
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(*) FROM [" + BaseConfigs.GetTablePrefix + "spacethemes] WHERE [name]=@name AND themeid<>@id", parms), 0) > 0;
		}

		public void AddSpaceTheme(string directory, string name, int type, string author, string createdate, string copyright)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@directory", (DbType)SqlDbType.VarChar, 100, directory),
									  DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar, 50, name),
									  DbHelper.MakeInParam("@type", (DbType)SqlDbType.Int, 50, type),
									  DbHelper.MakeInParam("@author", (DbType)SqlDbType.NVarChar, 100, author),
									  DbHelper.MakeInParam("@createdate", (DbType)SqlDbType.NVarChar, 50, createdate),
									  DbHelper.MakeInParam("@copyright", (DbType)SqlDbType.NVarChar, 100, copyright)
								  };
			string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "spacethemes]([directory], [name], [type], [author], [createdate], [copyright]) VALUES(@directory,@name,@type,@author,@createdate,@copyright)";

			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public void UpdateThemeName(int themeid, string name)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@themeid", (DbType)SqlDbType.Int, 4, themeid),
									  DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar, 50, name)
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "spacethemes] SET name=@name WHERE themeid=@themeid", parms);
		}

		public void DeleteTheme(int themeid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@themeid", (DbType)SqlDbType.Int, 4, themeid);
			string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "spacethemes] WHERE [themeid]=@themeid OR [type]=@themeid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
		}

		#region PhotoComment

		public IDataReader GetPhotoCommentCollection(int photoid)
		{
			string commandText = "SELECT * FROM[" + BaseConfigs.GetTablePrefix + "photocomments] WHERE [photoid]=" + photoid + " ORDER BY [commentid] ASC";
			return DbHelper.ExecuteReader(CommandType.Text, commandText);
		}

		public void CreatePhotoComment(PhotoCommentInfo pcomment)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, pcomment.Userid),
									  DbHelper.MakeInParam("@username", (DbType)SqlDbType.NVarChar, 20, pcomment.Username),
									  DbHelper.MakeInParam("@photoid", (DbType)SqlDbType.Int, 4, pcomment.Photoid),
									  DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.SmallDateTime, 4, pcomment.Postdatetime),
									  DbHelper.MakeInParam("@ip", (DbType)SqlDbType.VarChar, 100, pcomment.Ip),
									  DbHelper.MakeInParam("@content", (DbType)SqlDbType.NVarChar, 2000, pcomment.Content)
								  };
			string commandText = string.Format("INSERT INTO [{0}photocomments]([userid], [username], [photoid], [postdatetime], [ip], [content]) VALUES(@userid, @username, @photoid, @postdatetime, @ip, @content)", BaseConfigs.GetTablePrefix);
			DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
		}

		/// <summary>
		/// 删除图片评论
		/// </summary>
		/// <param name="commentid">评论Id</param>
		public void DeletePhotoComment(int commentid)
		{
			string commandText = string.Format("DELETE FROM [{0}photocomments] WHERE [commentid]={1}", BaseConfigs.GetTablePrefix, commentid);
			DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
		}
		#endregion

		public DataTable GetSpaceList(int pagesize, int currentpage, string username, string dateStart, string dateEnd)
		{
			int pagetop = (currentpage - 1) * pagesize;
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@dateStart", (DbType)SqlDbType.DateTime, 8, dateStart),
									  DbHelper.MakeInParam("@dateEnd", (DbType)SqlDbType.DateTime, 8, dateEnd)
								  };
			string condition = GetSpaceListCondition(username, dateStart, dateEnd);
			string sqlstring;
			if (currentpage == 1)
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " s.[spaceid],[userid],[spacetitle],[username],[grouptitle],[postcount],[commentcount],[createdatetime],[status] ";
				sqlstring += "FROM [" + BaseConfigs.GetTablePrefix + "spaceconfigs] s ";
				sqlstring += "LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] u ON s.userid=u.uid  ";
				sqlstring += "LEFT JOIN [" + BaseConfigs.GetTablePrefix + "usergroups] g ON u.[groupid]=g.[groupid] ";
				if (condition != "")
					sqlstring += "WHERE " + condition + " ";
				sqlstring += "ORDER BY s.spaceid DESC";
			}
			else
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " s.[spaceid],[userid],[spacetitle],[username],[grouptitle],[postcount],[commentcount],[createdatetime],[status] ";
				sqlstring += "FROM [" + BaseConfigs.GetTablePrefix + "spaceconfigs] s ";
				sqlstring += "LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] u ON s.[userid]=u.[uid] ";
				sqlstring += "LEFT JOIN [" + BaseConfigs.GetTablePrefix + "usergroups] g ON u.[groupid]=g.[groupid] ";
				sqlstring += "WHERE s.[spaceid]<(SELECT MIN([spaceid]) FROM (SELECT TOP " + pagetop + " [spaceid] FROM [" + BaseConfigs.GetTablePrefix + "spaceconfigs] ";
				sqlstring += "ORDER BY [spaceid] DESC) AS tblTmp) ";
				if (condition != "")
					sqlstring += "AND " + condition + " ";
				sqlstring += "ORDER BY s.[spaceid] DESC";

			}
			return DbHelper.ExecuteDataset(CommandType.Text, sqlstring, parms).Tables[0];

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
			DbHelper.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "spaceconfigs] SET [status]=[status]&~" + (int)SpaceStatusType.AdminClose + "  WHERE [spaceid] IN (" + spaceidlist + ")");
		}

		public void AdminCloseSpaceStatusBySpaceidlist(string spaceidlist)
		{
			DbHelper.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "spaceconfigs] SET [status]=[status]|" + (int)SpaceStatusType.AdminClose + "  WHERE [spaceid] IN (" + spaceidlist + ")");
		}

		public int GetSpaceRecordCount(string username, string dateStart, string dateEnd)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@dateStart", (DbType)SqlDbType.DateTime, 8, dateStart),
									  DbHelper.MakeInParam("@dateEnd", (DbType)SqlDbType.DateTime, 8, dateEnd)
								  };

			string condition = GetSpaceListCondition(username, dateStart, dateEnd);
			string sqlstring = "SELECT COUNT(s.[spaceid]) FROM [" + BaseConfigs.GetTablePrefix + "spaceconfigs] s LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] u ON s.[userid]=u.[uid] ";
			if (condition != "")
				sqlstring += " WHERE " + condition;
			return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, sqlstring, parms).Tables[0].Rows[0][0].ToString());
		}

		public bool IsRewritenameExist(string rewriteName)
		{
			IDataParameter parm = DbHelper.MakeInParam("@rewritename", (DbType)SqlDbType.Char, 100, rewriteName);
			string sql = string.Format("SELECT COUNT(1) FROM [{0}spaceconfigs] WHERE [rewritename]=@rewritename", BaseConfigs.GetTablePrefix);
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parm), 0) > 0;
		}

		public void UpdateUserSpaceRewriteName(int userid, string rewritename)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
									  DbHelper.MakeInParam("@rewritename", (DbType)SqlDbType.Char, 100, rewritename)
								  };

			string sql = string.Format("UPDATE [{0}spaceconfigs] SET [rewritename]=@rewritename WHERE [userid]=@userid", BaseConfigs.GetTablePrefix);

			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public string GetUidBySpaceid(string spaceid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@spaceid", (DbType)SqlDbType.Int, 4, spaceid);
			string sql = string.Format("SELECT [userid] FROM [{0}spaceconfigs] WHERE [spaceid]=@spaceid", BaseConfigs.GetTablePrefix);
			return DbHelper.ExecuteScalar(CommandType.Text, sql, parm).ToString();
		}

		public string GetSpaceattachmentsAidListByUid(int uid)
		{
			string aidlist = "";
			IDataParameter parm = DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid);
			string sql = string.Format("SELECT [aid] FROM [{0}spaceattachments] WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);
			DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
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
			IDataParameter parm = DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid);
			string sql = string.Format("DELETE FROM [{0}spaceconfigs] WHERE [userid]=@uid", BaseConfigs.GetTablePrefix);
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
		}

		public void UpdateCustomizePanelContent(int moduleid, int userid, string modulecontent)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@moduleid", (DbType)SqlDbType.Int, 4, moduleid),                
									  DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
									  DbHelper.MakeInParam("@modulecontent", (DbType)SqlDbType.NText, 0, modulecontent)
								  };

			string sql = string.Format("UPDATE [{0}spacecustomizepanels] SET [panelcontent]=@modulecontent WHERE [moduleid]=@moduleid AND [userid]=@userid", BaseConfigs.GetTablePrefix);

			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public bool ExistCustomizePanelContent(int moduleid, int userid)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@moduleid", (DbType)SqlDbType.Int, 4, moduleid),
									  DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid)
								  };

			string sql = string.Format("SELECT COUNT(1) FROM [{0}spacecustomizepanels] WHERE [moduleid]=@moduleid AND [userid]=@userid", BaseConfigs.GetTablePrefix);

			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parms), 0) > 0;
		}

		public void AddCustomizePanelContent(int moduleid, int userid, string modulecontent)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@moduleid", (DbType)SqlDbType.Int, 4, moduleid),
									  DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
									  DbHelper.MakeInParam("@modulecontent", (DbType)SqlDbType.NText, 0, modulecontent)
								  };

			string sql = string.Format("INSERT INTO [{0}spacecustomizepanels]([moduleid], [userid], [panelcontent]) VALUES(@moduleid, @userid, @modulecontent)", BaseConfigs.GetTablePrefix);

			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public object GetCustomizePanelContent(int moduleid, int userid)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@moduleid", (DbType)SqlDbType.Int, 4, moduleid),
									  DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid)
								  };

			string sql = string.Format("SELECT [panelcontent] FROM [{0}spacecustomizepanels] WHERE [moduleid]=@moduleid AND [userid]=@userid", BaseConfigs.GetTablePrefix);

			return DbHelper.ExecuteScalar(CommandType.Text, sql, parms);
		}

		public void DeleteCustomizePanelContent(int moduleid, int userid)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@moduleid", (DbType)SqlDbType.Int, 4, moduleid),
									  DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid)
								  };

			string sql = string.Format("DELETE FROM [{0}spacecustomizepanels] WHERE [moduleid]=@moduleid AND [userid]=@userid", BaseConfigs.GetTablePrefix);

			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public IDataReader GetModulesByUserId(int uid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid);

			string sql = string.Format("SELECT * FROM [{0}spacemodules] WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);

			return DbHelper.ExecuteReader(CommandType.Text, sql, parm);
		}


		public string GetSapceThemeList(int themeid)
		{
			return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "spacethemes] WHERE [type]=" + themeid;
		}

		public string DeleteSpaceThemeByThemeid(int themeid)
		{
			return "DELETE FROM [" + BaseConfigs.GetTablePrefix + "spacethemes] WHERE [themeid]=" + themeid;
		}

		public IDataReader GetModuleDefList()
		{
			string sql = string.Format("SELECT * FROM [{0}spacemoduledefs]", BaseConfigs.GetTablePrefix);

			return DbHelper.ExecuteReader(CommandType.Text, sql);
		}


		public void AddModuleDefInfo(ModuleDefInfo mdi)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@modulename", (DbType)SqlDbType.NVarChar, 20, mdi.ModuleName),
									  DbHelper.MakeInParam("@cachetime", (DbType)SqlDbType.Int, 4, mdi.CacheTime),
									  DbHelper.MakeInParam("@configfile", (DbType)SqlDbType.VarChar, 100, mdi.ConfigFile),
									  DbHelper.MakeInParam("@controller", (DbType)SqlDbType.VarChar, 255, mdi.BussinessController)
								  };

			string sql = string.Format("INSERT INTO [{0}spacemoduledefs]([modulename], [cachetime], [configfile], [controller]) VALUES(@modulename, @cachetime, @configfile, @controller)", BaseConfigs.GetTablePrefix);

			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        
		}


		public void DeleteModuleDefByUrl(string url)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@configfile", (DbType)SqlDbType.VarChar, 100, url)
								  };

			string sql = string.Format("DELETE FROM [{0}spacemoduledefs] WHERE [configfile] = @configfile", BaseConfigs.GetTablePrefix);

			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public DataTable GetSearchSpacePostsList(int pagesize, string postids)
		{
			string commandText = string.Format("SELECT TOP {1} [{0}spaceposts].[postid], [{0}spaceposts].[title], [{0}spaceposts].[author], [{0}spaceposts].[uid], [{0}spaceposts].[postdatetime], [{0}spaceposts].[commentcount], [{0}spaceposts].[views] FROM [{0}spaceposts] WHERE [{0}spaceposts].[postid] IN({2}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[{0}spaceposts].[postid]),'{2}')", BaseConfigs.GetTablePrefix, pagesize, postids);
			return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
		}

		public DataTable GetSearchAlbumList(int pagesize, string albumids)
		{
			string commandText = string.Format("SELECT TOP {1} [{0}albums].[albumid], [{0}albums].[title], [{0}albums].[username], [{0}albums].[userid], [{0}albums].[createdatetime], [{0}albums].[imgcount], [{0}albums].[views], [{0}albumcategories].[albumcateid],[{0}albumcategories].[title] AS [categorytitle] FROM [{0}albums] LEFT JOIN [{0}albumcategories] ON [{0}albumcategories].[albumcateid] = [{0}albums].[albumcateid] WHERE [{0}albums].[albumid] IN({2}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[{0}albums].[albumid]),'{2}')", BaseConfigs.GetTablePrefix, pagesize, albumids);
			return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
		}

		public int GetSpaceAttachmentSizeByUserid(int userid)
		{
			string sql = "SELECT ISNULL(SUM(filesize), 0) AS [filesize] FROM [" + BaseConfigs.GetTablePrefix + "spaceattachments] WHERE uid=" + userid;
			//object o = DbHelper.ExecuteScalar(CommandType.Text,sql);
			return (int)DbHelper.ExecuteScalar(CommandType.Text, sql);
		}

		public string GetSpaceThemes()
		{
			return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "spacethemes] WHERE [type]=0";
		}

		#endregion

		#region UserManage

		public DataTable GetUsers(string idlist)
		{
			if (!Utils.IsNumericArray(idlist.Split(',')))
				return new DataTable();

			string sql = string.Format("SELECT [uid],[username] FROM [{0}users] WHERE [groupid] IN ({1})", BaseConfigs.GetTablePrefix, idlist);
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public  DataTable GetUserGroupInfoByGroupid(int groupid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.Int, 4,groupid)
			};
			string sql = "SELECT TOP 1 * FROM  [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=@groupid";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		}

		public  DataTable GetAdmingroupByAdmingid(int admingid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@admingid",(DbType)SqlDbType.Int, 4,admingid)
			};
			string sql = "SELECT TOP 1 * FROM  [" + BaseConfigs.GetTablePrefix + "admingroups] WHERE [admingid]=@admingid";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		}

		public DataTable GetMedal()
		{
			string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "medals]";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public string GetMedalSql()
		{
			return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "medals]";
		}

		public DataTable GetExistMedalList()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [medalid],[image] FROM [" + BaseConfigs.GetTablePrefix + "medals] WHERE [image]<>''").Tables[0];
		}

		public void AddMedal(int medalid, string name, int available, string image)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@medalid", (DbType)SqlDbType.SmallInt,2, medalid),
				DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar,50, name),
				DbHelper.MakeInParam("@available", (DbType)SqlDbType.Int, 4, available),
				DbHelper.MakeInParam("@image",(DbType)SqlDbType.VarChar,30,image)
			};
			string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "medals] (medalid,name,available,image) Values (@medalid,@name,@available,@image)";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void UpdateMedal(int medalid, string name, string image)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@medalid", (DbType)SqlDbType.SmallInt,2, medalid),
				DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar,50, name),
				DbHelper.MakeInParam("@image",(DbType)SqlDbType.VarChar,30,image)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "medals] SET [name]=@name,[image]=@image  Where [medalid]=@medalid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void SetAvailableForMedal(int available, string medailidlist)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@available", (DbType)SqlDbType.Int, 4, available)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "medals] SET [available]=@available WHERE [medalid] IN(" + medailidlist + ")";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void DeleteMedalById(string medailidlist)
		{
			string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "medals] WHERE [medalid] IN(" + medailidlist + ")";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql);
		}

		public int GetMaxMedalId()
		{
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX(medalid), 0) FROM " + BaseConfigs.GetTablePrefix + "medals"), 0) + 1;
		}

		public string GetGroupInfo()
		{
			string sql = "SELECT [groupid], [grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] ORDER BY [groupid]";
			return sql;
		}

		/// <summary>
		/// 获得到指定管理组信息
		/// </summary>
		/// <returns>管理组信息</returns>
		public DataTable GetAdminGroupList()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "admingroups]").Tables[0];
		}

		/// <summary>
		/// 设置管理组信息
		/// </summary>
		/// <param name="__admingroupsInfo">管理组信息</param>
		/// <returns>更改记录数</returns>
		public int SetAdminGroupInfo(AdminGroupInfo __admingroupsInfo)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@admingid",(DbType)SqlDbType.SmallInt,2,__admingroupsInfo.Admingid),
									  DbHelper.MakeInParam("@alloweditpost",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Alloweditpost),
									  DbHelper.MakeInParam("@alloweditpoll",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Alloweditpoll),
									  DbHelper.MakeInParam("@allowstickthread",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowstickthread),
									  DbHelper.MakeInParam("@allowmodpost",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowmodpost),
									  DbHelper.MakeInParam("@allowdelpost",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowdelpost),
									  DbHelper.MakeInParam("@allowmassprune",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowmassprune),
									  DbHelper.MakeInParam("@allowrefund",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowrefund),
									  DbHelper.MakeInParam("@allowcensorword",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowcensorword),
									  DbHelper.MakeInParam("@allowviewip",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowviewip),
									  DbHelper.MakeInParam("@allowbanip",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowbanip),
									  DbHelper.MakeInParam("@allowedituser",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowedituser),
									  DbHelper.MakeInParam("@allowmoduser",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowmoduser),
									  DbHelper.MakeInParam("@allowbanuser",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowbanuser),
									  DbHelper.MakeInParam("@allowpostannounce",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowpostannounce),
									  DbHelper.MakeInParam("@allowviewlog",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowviewlog),
									  DbHelper.MakeInParam("@disablepostctrl",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Disablepostctrl),
									  DbHelper.MakeInParam("@allowviewrealname",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowviewrealname)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateadmingroup", prams);
		}

		/// <summary>
		/// 创建一个新的管理组信息
		/// </summary>
		/// <param name="__admingroupsInfo">要添加的管理组信息</param>
		/// <returns>更改记录数</returns>
		public int CreateAdminGroupInfo(AdminGroupInfo __admingroupsInfo)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@admingid",(DbType)SqlDbType.SmallInt,2,__admingroupsInfo.Admingid),
									  DbHelper.MakeInParam("@alloweditpost",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Alloweditpost),
									  DbHelper.MakeInParam("@alloweditpoll",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Alloweditpoll),
									  DbHelper.MakeInParam("@allowstickthread",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowstickthread),
									  DbHelper.MakeInParam("@allowmodpost",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowmodpost),
									  DbHelper.MakeInParam("@allowdelpost",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowdelpost),
									  DbHelper.MakeInParam("@allowmassprune",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowmassprune),
									  DbHelper.MakeInParam("@allowrefund",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowrefund),
									  DbHelper.MakeInParam("@allowcensorword",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowcensorword),
									  DbHelper.MakeInParam("@allowviewip",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowviewip),
									  DbHelper.MakeInParam("@allowbanip",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowbanip),
									  DbHelper.MakeInParam("@allowedituser",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowedituser),
									  DbHelper.MakeInParam("@allowmoduser",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowmoduser),
									  DbHelper.MakeInParam("@allowbanuser",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowbanuser),
									  DbHelper.MakeInParam("@allowpostannounce",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowpostannounce),
									  DbHelper.MakeInParam("@allowviewlog",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowviewlog),
									  DbHelper.MakeInParam("@disablepostctrl",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Disablepostctrl),
									  DbHelper.MakeInParam("@allowviewrealname",(DbType)SqlDbType.TinyInt,1,__admingroupsInfo.Allowviewrealname)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "createadmingroup", prams);
		}

		/// <summary>
		/// 删除指定的管理组信息
		/// </summary>
		/// <param name="admingid">管理组ID</param>
		/// <returns>更改记录数</returns>
		public int DeleteAdminGroupInfo(short admingid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@admingid",(DbType)SqlDbType.SmallInt,2,admingid),
			};
			return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "admingroups] WHERE [admingid] = @admingid", prams);
		}

		public string GetAdminGroupInfoSql()
		{
			return "Select * From [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]>0 AND [radminid]<=3  Order By [groupid]";
		}

		public DataTable GetRaterangeByGroupid(int groupid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.Int, 4,groupid)
			};
			string sql = "SELECT TOP 1 [raterange] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=@groupid";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		}

		public void UpdateRaterangeByGroupid(string raterange, int groupid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@raterange",(DbType)SqlDbType.NChar, 500,raterange),
				DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.Int, 4,groupid)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [raterange]=@raterange WHERE [groupid]=@groupid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public string GetAudituserSql()
		{
			return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "users] Where [groupid]=8";
		}

		public DataSet GetAudituserUid()
		{
			string sql = "SELECT [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid]=8";
			return DbHelper.ExecuteDataset(CommandType.Text, sql);
		}

		public void ClearAuthstrByUidlist(string uidlist)
		{
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "userfields] SET [authstr]='' WHERE [uid] IN (" + uidlist + ")";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql);
		}

		public void ClearAllUserAuthstr()
		{
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "userfields] SET [authstr]='' WHERE [uid] IN (SELECT [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid]=8 )";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql);
		}

		public void DeleteUserByUidlist(string uidlist)
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "userfields] WHERE [uid] IN(" + uidlist + ")");
			DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid] IN(" + uidlist + ")");
		}

		public void DeleteAuditUser()
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "userfields] WHERE [uid] IN (SELECT [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid]=8 )");
			DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid]=8 ");
		}

		public DataTable GetAuditUserEmail()
		{
			string sql = "SELECT [username],[password],[email] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid]=8";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}
		public DataTable GetUserEmailByUidlist(string uidlist)
		{
			string sql = "SELECT [username],[password],[email] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid] IN(" + uidlist + ")";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public  string GetUserGroup()
		{
			string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]= 0 And [groupid]>8 ORDER BY [groupid]";
			return sql;
		}

		public  string GetUserGroupTitle()
		{
			return "SELECT [groupid],[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]= 0 And [groupid]>8 ORDER BY [groupid]";
		}

		public  DataTable GetUserGroupWithOutGuestTitle()
		{
			return DbHelper.ExecuteDataset("SELECT [groupid],[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]<>7  ORDER BY [groupid] ASC").Tables[0];
		}

		public  string GetAdminUserGroupTitle()
		{
			string sql = "SELECT [groupid],[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]> 0 AND [radminid]<=3  ORDER BY [groupid]";
			return sql;
		}

		public void CombinationUsergroupScore(int sourceusergroupid, int targetusergroupid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@sourceusergroupid",(DbType)SqlDbType.Int, 4,sourceusergroupid),
				DbHelper.MakeInParam("@targetusergroupid",(DbType)SqlDbType.Int, 4,targetusergroupid)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [creditshigher]=(SELECT [creditshigher] FROM "
				+ "[" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=@sourceusergroupid) WHERE [groupid]=@targetusergroupid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void DeleteUserGroupInfo(int groupid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.Int, 4,groupid)
			};
			string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "usergroups] Where [groupid]=@groupid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void DeleteAdminGroupInfo(int admingid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@admingid",(DbType)SqlDbType.Int, 4,admingid)
			};
			string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "admingroups] Where [admingid]=@admingid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void ChangeUsergroup(int soureceusergroupid, int targetusergroupid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@soureceusergroupid",(DbType)SqlDbType.Int, 4,soureceusergroupid),
				DbHelper.MakeInParam("@targetusergroupid",(DbType)SqlDbType.Int, 4,targetusergroupid)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [groupid]=@targetusergroupid WHERE [groupid]=@soureceusergroupid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}
		public DataTable GetAdmingid(int admingid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@admingid",(DbType)SqlDbType.Int, 4,admingid)
			};
			string sql = "SELECT [admingid]  FROM [" + BaseConfigs.GetTablePrefix + "admingroups] WHERE [admingid]=@admingid";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		}

		public void ChangeUserAdminidByGroupid(int adminid, int groupid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@adminid",(DbType)SqlDbType.Int, 4,adminid),
				DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.Int, 4,groupid)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [adminid]=@adminid WHERE [groupid]=@groupid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public DataTable GetAvailableMedal()
		{
			string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "medals] WHERE [available]=1";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public bool IsExistMedalAwardRecord(int medalid, int userid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@medalid", (DbType)SqlDbType.Int,4, medalid),
				DbHelper.MakeInParam("@userid",(DbType)SqlDbType.Int,4,userid)
			};
			string sql = "SELECT TOP 1 ID FROM [" + BaseConfigs.GetTablePrefix + "medalslog] WHERE [medals]=@medalid AND [uid]=@userid";
			if (DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0].Rows.Count != 0)
				return true;
			else
				return false;
		}

		public void AddMedalslog(int adminid, string adminname, string ip, string username, int uid, string actions, int medals, string reason)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@adminid", (DbType)SqlDbType.Int,4, adminid),
				DbHelper.MakeInParam("@adminname",(DbType)SqlDbType.NVarChar,50,adminname),
				DbHelper.MakeInParam("@ip", (DbType)SqlDbType.NVarChar,15, ip),
				DbHelper.MakeInParam("@username",(DbType)SqlDbType.NVarChar,50,username),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int,4, uid),
				DbHelper.MakeInParam("@actions",(DbType)SqlDbType.NVarChar,100,actions),
				DbHelper.MakeInParam("@medals", (DbType)SqlDbType.Int,4, medals),
				DbHelper.MakeInParam("@reason",(DbType)SqlDbType.NVarChar,100,reason)
			};
			string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "medalslog] (adminid,adminname,ip,username,uid,actions,medals,reason) VALUES (@adminid,@adminname,@ip,@username,@uid,@actions,@medals,@reason)";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void UpdateMedalslog(string newactions, DateTime postdatetime, string reason, string oldactions, int medals, int uid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@newactions",(DbType)SqlDbType.NVarChar,100,newactions),
				DbHelper.MakeInParam("@postdatetime",(DbType)SqlDbType.DateTime,8,postdatetime),
				DbHelper.MakeInParam("@reason",(DbType)SqlDbType.NVarChar,100,reason),
				DbHelper.MakeInParam("@oldactions",(DbType)SqlDbType.NVarChar,100,oldactions),
				DbHelper.MakeInParam("@medals", (DbType)SqlDbType.Int,4, medals),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int,4, uid)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "medalslog] SET [actions]=@newactions ,[postdatetime]=@postdatetime, reason=@reason  WHERE [actions]=@oldactions AND [medals]=@medals  AND [uid]=@uid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void UpdateMedalslog(string actions, DateTime postdatetime, string reason, int uid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@actions",(DbType)SqlDbType.NVarChar,100,actions),
				DbHelper.MakeInParam("@postdatetime",(DbType)SqlDbType.DateTime,8,postdatetime),
				DbHelper.MakeInParam("@reason",(DbType)SqlDbType.NVarChar,100,reason),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int,4, uid)
			};
			string sql = "Update [" + BaseConfigs.GetTablePrefix + "medalslog] SET [actions]=@actions ,[postdatetime]=@postdatetime,[reason]=@reason  WHERE [uid]=@uid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void UpdateMedalslog(string newactions, DateTime postdatetime, string reason, string oldactions, string medalidlist, int uid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@newactions",(DbType)SqlDbType.NVarChar,100,newactions),
				DbHelper.MakeInParam("@postdatetime",(DbType)SqlDbType.DateTime,8,postdatetime),
				DbHelper.MakeInParam("@reason",(DbType)SqlDbType.NVarChar,100,reason),
				DbHelper.MakeInParam("@oldactions",(DbType)SqlDbType.NVarChar,100,oldactions),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int,4, uid)
			};
			string sql = "Update [" + BaseConfigs.GetTablePrefix + "medalslog] SET [actions]=@newactions ,[postdatetime]=@postdatetime, reason=@reason  WHERE [actions]=@oldactions AND [medals] NOT IN (" + medalidlist + ") AND [uid]=@uid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void SetStopTalkUser(string uidlist)
		{
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [groupid]=4, [adminid]=0  WHERE [uid] IN (" + uidlist + ")";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql);
		}

		public void ChangeUserGroupByUid(int groupid, string uidlist)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.Int,4,groupid)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [groupid]=@groupid  WHERE [uid] IN (" + uidlist + ")";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public DataTable GetTableListInfo()
		{
			string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "tablelist]";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public void DeletePostByPosterid(int tabid, int posterid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.Int,4, posterid)
			};
			string sql = "DELETE FROM  [" + BaseConfigs.GetTablePrefix + "posts" + tabid + "]   WHERE [posterid]=@posterid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void DeleteTopicByPosterid(int posterid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@posterid", (DbType)SqlDbType.Int,4, posterid)
			};
			string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [posterid]=@posterid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void ClearPosts(int uid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int,4, uid)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [digestposts]=0 , [posts]=0  WHERE [uid]=@uid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void UpdateEmailValidateInfo(string authstr, DateTime authtime, int uid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@authstr",(DbType)SqlDbType.VarChar,20,authstr),
				DbHelper.MakeInParam("@authtime",(DbType)SqlDbType.DateTime,8,authtime),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int,4, uid)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "userfields] SET [Authstr]=@authstr,[Authtime]=@authtime ,[Authflag]=1  WHERE [uid]=@uid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public int GetRadminidByGroupid(int groupid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int,4, groupid)
			};
			string sql = "SELECT TOP 1 [radminid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=@groupid";
			return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, sql, prams));
		}

		public string GetTemplateInfo()
		{
			string sql = "SELECT [templateid], [name] FROM [" + BaseConfigs.GetTablePrefix + "templates]";
			return sql;
		}

		public DataTable GetUserEmailByGroupid(string groupidlist)
		{
			string sql = "SELECT [username],[Email]  From [" + BaseConfigs.GetTablePrefix + "users] WHERE [Email] Is Not null AND [Email]<>'' AND [groupid] IN(" + groupidlist + ")";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public DataTable GetUserGroupExceptGroupid(int groupid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.Int, 4,groupid)
			};
			string sql = "SELECT [groupid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]=0 And [groupid]>8 AND [groupid]<>@groupid";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		}

		/// <summary>
		/// 创建收藏信息
		/// </summary>
		/// <param name="uid">用户ID</param>
		/// <param name="tid">主题ID</param>
		/// <returns>创建成功返回 1 否则返回 0</returns>	
		public int CreateFavorites(int uid, int tid)
		{
			return CreateFavorites(uid, tid, 0);
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
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid),
									  DbHelper.MakeInParam("@type", (DbType)SqlDbType.TinyInt, 4, type)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "createfavorite", prams);
		}



		/// <summary>
		/// 删除指定用户的收藏信息
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="fitemid">要删除的收藏信息id列表,以英文逗号分割</param>
		/// <returns>删除的条数．出错时返回 -1</returns>
		public int DeleteFavorites(int uid, string fidlist, byte type)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int,4, uid),
									  DbHelper.MakeInParam("@typeid", (DbType)SqlDbType.TinyInt, 1, type)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "favorites] WHERE [tid] IN (" + fidlist + ") AND [uid] = @uid AND [typeid]=@typeid", prams);
		}

		/// <summary>
		/// 得到用户收藏信息列表
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="pagesize">分页时每页的记录数</param>
		/// <param name="pageindex">当前页码</param>
		/// <returns>用户信息列表</returns>
		public DataTable GetFavoritesList(int uid, int pagesize, int pageindex)
		{
			return GetFavoritesList(uid, pagesize, pageindex, 0);
		}

		/// <summary>
		/// 得到用户收藏信息列表
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="pagesize">分页时每页的记录数</param>
		/// <param name="pageindex">当前页码</param>
		/// <param name="typeid">收藏类型id</param>
		/// <returns>用户信息列表</returns>
		public DataTable GetFavoritesList(int uid, int pagesize, int pageindex, int typeid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									  DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pagesize),
									  DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,pageindex)
								   
								  };

			switch (typeid)
			{
				case 1:
					return DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getfavoriteslistbyalbum", prams).Tables[0];

				case 2:
					return DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getfavoriteslistbyspacepost", prams).Tables[0];

					//case FavoriteType.ForumTopic:
				default:
					return DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getfavoriteslist", prams).Tables[0];

			}
		}

		/// <summary>
		/// 得到用户收藏的总数
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <returns>收藏总数</returns>
		public int GetFavoritesCount(int uid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
			};
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getfavoritescount", prams).ToString(), 0);
		}

		public int GetFavoritesCount(int uid, int typeid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									  DbHelper.MakeInParam("@typeid",(DbType)SqlDbType.TinyInt,1,typeid)
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getfavoritescountbytype", prams).ToString(), 0);
		}

		public int CheckFavoritesIsIN(int uid, int tid)
		{
			return CreateFavorites(uid, tid, 0);
		}

		/// <summary>
		/// 收藏夹里是否包含了指定的主题
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="tid">主题id</param>
		/// <returns></returns>
		public int CheckFavoritesIsIN(int uid, int tid, byte type)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									  DbHelper.MakeInParam("@tid",(DbType)SqlDbType.Int,4,tid),
									  DbHelper.MakeInParam("@type", (DbType)SqlDbType.TinyInt, 1, type)
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([tid]) AS [tidcount] FROM [" + BaseConfigs.GetTablePrefix + "favorites] WHERE [tid]=@tid AND [uid]=@uid AND [typeid]=@type", prams), 0);
		}


		public void UpdateUserAllInfo(UserInfo userinfo)
		{
			string sqlstring = "Update [" + BaseConfigs.GetTablePrefix + "users] Set username=@username ,nickname=@nickname,secques=@secques,gender=@gender,adminid=@adminid,groupid=@groupid,groupexpiry=@groupexpiry,extgroupids=@extgroupids, regip=@regip," +
				"joindate=@joindate , lastip=@lastip, lastvisit=@lastvisit,  lastactivity=@lastactivity, lastpost=@lastpost, lastposttitle=@lastposttitle,posts=@posts, digestposts=@digestposts,oltime=@oltime,pageviews=@pageviews,credits=@credits," +
				"avatarshowid=@avatarshowid, email=@email,bday=@bday,sigstatus=@sigstatus,tpp=@tpp,ppp=@ppp,templateid=@templateid,pmsound=@pmsound," +
				"showemail=@showemail,newsletter=@newsletter,invisible=@invisible,newpm=@newpm,accessmasks=@accessmasks,extcredits1=@extcredits1,extcredits2=@extcredits2,extcredits3=@extcredits3,extcredits4=@extcredits4,extcredits5=@extcredits5,extcredits6=@extcredits6,extcredits7=@extcredits7,extcredits8=@extcredits8   Where uid=@uid";

			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@username", (DbType)SqlDbType.VarChar, 20, userinfo.Username),
									  DbHelper.MakeInParam("@nickname", (DbType)SqlDbType.VarChar, 10, userinfo.Nickname),
									  DbHelper.MakeInParam("@secques", (DbType)SqlDbType.VarChar, 8, userinfo.Secques),
									  DbHelper.MakeInParam("@gender", (DbType)SqlDbType.Int, 4, userinfo.Gender),
									  DbHelper.MakeInParam("@adminid", (DbType)SqlDbType.Int, 4, userinfo.Uid == 1 ? 1 : userinfo.Adminid),
									  DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.SmallInt, 2, userinfo.Groupid),
									  DbHelper.MakeInParam("@groupexpiry", (DbType)SqlDbType.Int, 4, userinfo.Groupexpiry),
									  DbHelper.MakeInParam("@extgroupids", (DbType)SqlDbType.VarChar, 60, userinfo.Extgroupids),
									  DbHelper.MakeInParam("@regip", (DbType)SqlDbType.NChar, 15, userinfo.Regip),
									  DbHelper.MakeInParam("@joindate", (DbType)SqlDbType.SmallDateTime, 4, userinfo.Joindate),
									  DbHelper.MakeInParam("@lastip", (DbType)SqlDbType.NChar, 15, userinfo.Lastip),
									  DbHelper.MakeInParam("@lastvisit", (DbType)SqlDbType.DateTime, 8, userinfo.Lastvisit),
									  DbHelper.MakeInParam("@lastactivity", (DbType)SqlDbType.DateTime, 8, userinfo.Lastactivity),
									  DbHelper.MakeInParam("@lastpost", (DbType)SqlDbType.DateTime, 8, userinfo.Lastpost),
									  DbHelper.MakeInParam("@lastposttitle", (DbType)SqlDbType.NChar, 80, userinfo.Lastposttitle),
									  DbHelper.MakeInParam("@posts", (DbType)SqlDbType.Int, 4, userinfo.Posts),
									  DbHelper.MakeInParam("@digestposts", (DbType)SqlDbType.SmallInt, 2, userinfo.Digestposts),
									  DbHelper.MakeInParam("@oltime", (DbType)SqlDbType.Int, 4, userinfo.Oltime),
									  DbHelper.MakeInParam("@pageviews", (DbType)SqlDbType.Int, 4, userinfo.Pageviews),
									  DbHelper.MakeInParam("@credits", (DbType)SqlDbType.Decimal, 10, userinfo.Credits),
									  DbHelper.MakeInParam("@avatarshowid", (DbType)SqlDbType.Int, 4, userinfo.Avatarshowid),
									  DbHelper.MakeInParam("@email", (DbType)SqlDbType.NChar, 50, userinfo.Email.ToString()),
									  DbHelper.MakeInParam("@bday", (DbType)SqlDbType.NChar, 10, userinfo.Bday.ToString()),
									  DbHelper.MakeInParam("@sigstatus", (DbType)SqlDbType.Int, 4, userinfo.Sigstatus.ToString()),
									  DbHelper.MakeInParam("@tpp", (DbType)SqlDbType.Int, 4, userinfo.Tpp),
									  DbHelper.MakeInParam("@ppp", (DbType)SqlDbType.Int, 4, userinfo.Ppp),
									  DbHelper.MakeInParam("@templateid", (DbType)SqlDbType.Int, 4, userinfo.Templateid),
									  DbHelper.MakeInParam("@pmsound", (DbType)SqlDbType.Int, 4, userinfo.Pmsound),
									  DbHelper.MakeInParam("@showemail", (DbType)SqlDbType.Int, 4, userinfo.Showemail),
									  DbHelper.MakeInParam("@newsletter", (DbType)SqlDbType.Int, 4, userinfo.Newsletter),
									  DbHelper.MakeInParam("@invisible", (DbType)SqlDbType.Int, 4, userinfo.Invisible),
									  DbHelper.MakeInParam("@newpm", (DbType)SqlDbType.Int, 4, userinfo.Newpm),
									  DbHelper.MakeInParam("@accessmasks", (DbType)SqlDbType.Int, 4, userinfo.Accessmasks),
									  DbHelper.MakeInParam("@extcredits1", (DbType)SqlDbType.Decimal, 10, userinfo.Extcredits1),
									  DbHelper.MakeInParam("@extcredits2", (DbType)SqlDbType.Decimal, 10, userinfo.Extcredits2),
									  DbHelper.MakeInParam("@extcredits3", (DbType)SqlDbType.Decimal, 10, userinfo.Extcredits3),
									  DbHelper.MakeInParam("@extcredits4", (DbType)SqlDbType.Decimal, 10, userinfo.Extcredits4),
									  DbHelper.MakeInParam("@extcredits5", (DbType)SqlDbType.Decimal, 10, userinfo.Extcredits5),
									  DbHelper.MakeInParam("@extcredits6", (DbType)SqlDbType.Decimal, 10, userinfo.Extcredits6),
									  DbHelper.MakeInParam("@extcredits7", (DbType)SqlDbType.Decimal, 10, userinfo.Extcredits7),
									  DbHelper.MakeInParam("@extcredits8", (DbType)SqlDbType.Decimal, 10, userinfo.Extcredits8),
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userinfo.Uid)
								  };

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);
		}

		public void DeleteModerator(int uid)
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE [uid]=" + uid);
		}

		public void UpdateUserField(UserInfo __userinfo, string signature, string authstr, string sightml)
		{
			string sqlstring = "Update [" + BaseConfigs.GetTablePrefix + "userfields] Set website=@website,icq=@icq,qq=@qq,yahoo=@yahoo,msn=@msn,skype=@skype,location=@location,customstatus=@customstatus, avatar=@avatar," +
				"avatarwidth=@avatarwidth , avatarheight=@avatarheight, medals=@medals,  authstr=@authstr, authtime=@authtime, authflag=@authflag,bio=@bio, signature=@signature,sightml=@sightml,realname=@Realname,idcard=@Idcard,mobile=@Mobile,phone=@Phone Where uid=@uid";

			IDataParameter[] prams1 = {
									   DbHelper.MakeInParam("@website", (DbType)SqlDbType.NVarChar, 80, __userinfo.Website),
									   DbHelper.MakeInParam("@icq", (DbType)SqlDbType.VarChar, 12, __userinfo.Icq),
									   DbHelper.MakeInParam("@qq", (DbType)SqlDbType.VarChar, 12, __userinfo.Qq),
									   DbHelper.MakeInParam("@yahoo", (DbType)SqlDbType.VarChar, 40, __userinfo.Yahoo),
									   DbHelper.MakeInParam("@msn", (DbType)SqlDbType.VarChar, 40, __userinfo.Msn),
									   DbHelper.MakeInParam("@skype", (DbType)SqlDbType.VarChar, 40, __userinfo.Skype),
									   DbHelper.MakeInParam("@location", (DbType)SqlDbType.NVarChar, 50, __userinfo.Location),
									   DbHelper.MakeInParam("@customstatus", (DbType)SqlDbType.NVarChar, 50, __userinfo.Customstatus),
									   DbHelper.MakeInParam("@avatar", (DbType)SqlDbType.NVarChar, 255, __userinfo.Avatar),
									   DbHelper.MakeInParam("@avatarwidth", (DbType)SqlDbType.Int, 4, __userinfo.Avatarwidth),
									   DbHelper.MakeInParam("@avatarheight", (DbType)SqlDbType.Int, 4, __userinfo.Avatarheight),
									   DbHelper.MakeInParam("@medals", (DbType)SqlDbType.VarChar, 300, __userinfo.Medals),
									   DbHelper.MakeInParam("@authstr", (DbType)SqlDbType.VarChar, 20, authstr),
									   DbHelper.MakeInParam("@authtime", (DbType)SqlDbType.SmallDateTime, 4, __userinfo.Authtime),
									   DbHelper.MakeInParam("@authflag", (DbType)SqlDbType.TinyInt, 1, 1),
									   DbHelper.MakeInParam("@bio", (DbType)SqlDbType.NVarChar, 500, __userinfo.Bio.ToString()),
									   DbHelper.MakeInParam("@signature", (DbType)SqlDbType.NVarChar, 500, signature),
									   DbHelper.MakeInParam("@sightml", (DbType)SqlDbType.NVarChar, 1000, sightml),
									   DbHelper.MakeInParam("@Realname", (DbType)SqlDbType.NVarChar, 1000, __userinfo.Realname),
									   DbHelper.MakeInParam("@Idcard", (DbType)SqlDbType.NVarChar, 1000, __userinfo.Idcard),
									   DbHelper.MakeInParam("@Mobile", (DbType)SqlDbType.NVarChar, 1000, __userinfo.Mobile),
									   DbHelper.MakeInParam("@Phone", (DbType)SqlDbType.NVarChar, 1000, __userinfo.Phone),
									   DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, __userinfo.Uid)
								   };

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams1);
		}



		public void UpdatePMSender(int msgfromid, string msgfrom)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@msgfromid", (DbType)SqlDbType.Int, 4, msgfromid),
									  DbHelper.MakeInParam("@msgfrom", (DbType)SqlDbType.VarChar, 20, msgfrom)
								  };

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "pms] SET [msgfrom]=@msgfrom WHERE [msgfromid]=@msgfromid", parms);
		}

		public void UpdatePMReceiver(int msgtoid, string msgto)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@msgtoid", (DbType)SqlDbType.Int, 4, msgtoid),
									  DbHelper.MakeInParam("@msgto", (DbType)SqlDbType.VarChar, 20, msgto)
								  };

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "pms] SET [msgto]=@msgto  WHERE [msgtoid]=@msgtoid", parms);
		}



		public DataRowCollection GetModerators(string oldusername)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@oldusername", (DbType)SqlDbType.VarChar, 20, RegEsc(oldusername))
								  };

			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [fid],[moderators] FROM  [" + BaseConfigs.GetTablePrefix + "forumfields] WHERE [moderators] LIKE '% @oldusername %'", prams).Tables[0].Rows;
		}

		public DataTable GetModeratorsTable(string oldusername)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@oldusername", (DbType)SqlDbType.VarChar, 20, RegEsc(oldusername))
								  };

			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [fid],[moderators] FROM  [" + BaseConfigs.GetTablePrefix + "forumfields] WHERE [moderators] LIKE '% @oldusername %'", prams).Tables[0];
		}

		public void UpdateModerators(int fid, string moderators)
		{
			IDataParameter[] parm = { 
									 DbHelper.MakeInParam("@moderators", (DbType)SqlDbType.VarChar, 20, moderators),
									 DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid)
								 };

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET [moderators]=@moderators  WHERE [fid]=@fid", parm);
		}

		public void UpdateUserCredits(int userid, float credits, float extcredits1, float extcredits2, float extcredits3, float extcredits4, float extcredits5, float extcredits6, float extcredits7, float extcredits8)
		{
			IDataParameter[] prams1 = {
									   DbHelper.MakeInParam("@targetuid",(DbType)SqlDbType.Int,4,userid.ToString()),
									   DbHelper.MakeInParam("@Credits",(DbType)SqlDbType.Decimal,9, credits),
									   DbHelper.MakeInParam("@Extcredits1", (DbType)SqlDbType.Decimal, 20,extcredits1),
									   DbHelper.MakeInParam("@Extcredits2", (DbType)SqlDbType.Decimal, 20,extcredits2),
									   DbHelper.MakeInParam("@Extcredits3", (DbType)SqlDbType.Decimal, 20,extcredits3),
									   DbHelper.MakeInParam("@Extcredits4", (DbType)SqlDbType.Decimal, 20,extcredits4),
									   DbHelper.MakeInParam("@Extcredits5", (DbType)SqlDbType.Decimal, 20,extcredits5),
									   DbHelper.MakeInParam("@Extcredits6", (DbType)SqlDbType.Decimal, 20,extcredits6),
									   DbHelper.MakeInParam("@Extcredits7", (DbType)SqlDbType.Decimal, 20,extcredits7),
									   DbHelper.MakeInParam("@Extcredits8", (DbType)SqlDbType.Decimal, 20,extcredits8)
								   };

			string sqlstring = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET credits=@Credits,extcredits1=@Extcredits1, extcredits2=@Extcredits2, extcredits3=@Extcredits3, extcredits4=@Extcredits4, extcredits5=@Extcredits5, extcredits6=@Extcredits6, extcredits7=@Extcredits7, extcredits8=@Extcredits8 WHERE [uid]=@targetuid";

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams1);
		}

		public void UpdateUserCredits(int userid, int extcreditsid, float score)
		{
			IDataParameter[] prams1 = {
									   DbHelper.MakeInParam("@targetuid",(DbType)SqlDbType.Int,4,userid.ToString()),
									   DbHelper.MakeInParam("@Extcredits", (DbType)SqlDbType.Float, 8, score)
								   };

			string sqlstring = string.Format("UPDATE [{0}users] SET extcredits{1}=extcredits{1} + @Extcredits WHERE [uid]=@targetuid", BaseConfigs.GetTablePrefix, extcreditsid);

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams1);
		}

		public void CombinationUser(string posttablename, UserInfo __targetuserinfo, UserInfo __srcuserinfo)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@target_uid", (DbType)SqlDbType.Int, 4, __targetuserinfo.Uid),
									  DbHelper.MakeInParam("@target_username", (DbType)SqlDbType.NChar, 20, __targetuserinfo.Username.Trim()),
									  DbHelper.MakeInParam("@src_uid", (DbType)SqlDbType.Int, 4, __srcuserinfo.Uid)
								  };

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE  [" + BaseConfigs.GetTablePrefix + "topics] SET [posterid]=@target_uid,[poster]=@target_username  WHERE [posterid]=@src_uid", prams);

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE  [" + posttablename + "] SET [posterid]=@target_uid,[poster]=@target_username  WHERE [posterid]=@src_uid", prams);

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE  [" + BaseConfigs.GetTablePrefix + "pms] SET [msgtoid]=@target_uid,[msgto]=@target_username  WHERE [msgtoid]=@src_uid", prams);
		}

		/// <summary>
		/// 通过用户名得到UID
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public int GetuidByusername(string username)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar, 20, username)
								  };

			DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [username]=@username", prams).Tables[0];
			if (dt.Rows.Count > 0)
			{
				return Convert.ToInt32(dt.Rows[0][0].ToString());
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// 删除指定用户的所有信息
		/// </summary>
		/// <param name="uid">指定的用户uid</param>
		/// <param name="delposts">是否删除帖子</param>
		/// <param name="delpms">是否删除短消息</param>
		/// <returns></returns>
		public bool DelUserAllInf(int uid, bool delposts, bool delpms)
		{
			SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
			conn.Open();
			using (SqlTransaction trans = conn.BeginTransaction())
			{
				try
				{
					DbHelper.ExecuteNonQuery(CommandType.Text, "Delete From [" + BaseConfigs.GetTablePrefix + "users] Where [uid]=" + uid.ToString());
					DbHelper.ExecuteNonQuery(CommandType.Text, "Delete From [" + BaseConfigs.GetTablePrefix + "userfields] Where [uid]=" + uid.ToString());
					DbHelper.ExecuteNonQuery(CommandType.Text, "Delete From [" + BaseConfigs.GetTablePrefix + "polls] Where [userid]=" + uid.ToString());
					DbHelper.ExecuteNonQuery(CommandType.Text, "Delete From [" + BaseConfigs.GetTablePrefix + "favorites] Where [uid]=" + uid.ToString());

					if (delposts)
					{
						DbHelper.ExecuteNonQuery(CommandType.Text, "Delete From [" + BaseConfigs.GetTablePrefix + "topics] Where [posterid]=" + uid.ToString());

						//清除用户所发的帖子
						foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "tablelist]").Tables[0].Rows)
						{
							if (dr["id"].ToString() != "")
							{
								DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM  [" + BaseConfigs.GetTablePrefix + "posts" + dr["id"].ToString() + "]   WHERE [posterid]=" + uid);
							}
						}
					}
					else
					{
						DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [poster]='该用户已被删除'  Where [posterid]=" + uid.ToString());

						DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [lastposter]='该用户已被删除'  Where [lastpostid]=" + uid.ToString());

						//清除用户所发的帖子
						foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "tablelist]").Tables[0].Rows)
						{
							if (dr["id"].ToString() != "")
							{
								DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE  [" + BaseConfigs.GetTablePrefix + "posts" + dr["id"].ToString() + "] SET  [poster]='该用户已被删除'  WHERE [posterid]=" + uid);
							}
						}
					}

					if (delpms)
					{
						DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "pms] Where [msgfromid]=" + uid.ToString());
					}
					else
					{
						DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "pms] SET [msgfrom]='该用户已被删除'  Where [msgfromid]=" + uid.ToString());
						DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "pms] SET [msgto]='该用户已被删除'  Where [msgtoid]=" + uid.ToString());
					}

					//删除版主表的相关用户信息
					DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE [uid]=" + uid.ToString());

					//更新当前论坛总人数
					DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "Statistics] SET [totalusers]=[totalusers]-1");

					DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [uid],[username] FROM [" + BaseConfigs.GetTablePrefix + "users] ORDER BY [uid] DESC").Tables[0];
					if (dt.Rows.Count > 0)
					{
						//更新当前论坛最新注册会员信息
						DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "Statistics] SET [lastuserid]=" + dt.Rows[0][0] + ", [lastusername]='" + dt.Rows[0][1] + "'");
					}



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

		public DataTable GetUserGroup(int groupid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupid);

			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=@groupid", parm).Tables[0];
		}

		public DataTable GetAdminGroup(int groupid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupid);

			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT Top 1 * FROM [" + BaseConfigs.GetTablePrefix + "admingroups] WHERE [admingid]=@groupid", parm).Tables[0];
		}

		public void AddUserGroup(UserGroupInfo __usergroupinfo, int Creditshigher, int Creditslower)
		{
			IDataParameter[] prams = 
					{
						DbHelper.MakeInParam("@Radminid",(DbType)SqlDbType.Int,4,__usergroupinfo.Radminid),
						DbHelper.MakeInParam("@Grouptitle",(DbType)SqlDbType.NVarChar,50, Utils.RemoveFontTag(__usergroupinfo.Grouptitle)),
						DbHelper.MakeInParam("@Creditshigher",(DbType)SqlDbType.Int,4,Creditshigher),
						DbHelper.MakeInParam("@Creditslower",(DbType)SqlDbType.Int,4,Creditslower),
						DbHelper.MakeInParam("@Stars",(DbType)SqlDbType.Int,4,__usergroupinfo.Stars),
						DbHelper.MakeInParam("@Color",(DbType)SqlDbType.Char,7,__usergroupinfo.Color),
						DbHelper.MakeInParam("@Groupavatar",(DbType)SqlDbType.NVarChar,60,__usergroupinfo.Groupavatar),
						DbHelper.MakeInParam("@Readaccess",(DbType)SqlDbType.Int,4,__usergroupinfo.Readaccess),
						DbHelper.MakeInParam("@Allowvisit",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowvisit),
						DbHelper.MakeInParam("@Allowpost",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowpost),
						DbHelper.MakeInParam("@Allowreply",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowreply),
						DbHelper.MakeInParam("@Allowpostpoll",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowpostpoll),
						DbHelper.MakeInParam("@Allowdirectpost",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowdirectpost),
						DbHelper.MakeInParam("@Allowgetattach",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowgetattach),
						DbHelper.MakeInParam("@Allowpostattach",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowpostattach),
						DbHelper.MakeInParam("@Allowvote",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowvote),
						DbHelper.MakeInParam("@Allowmultigroups",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowmultigroups),
						DbHelper.MakeInParam("@Allowsearch",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowsearch),
						DbHelper.MakeInParam("@Allowavatar",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowavatar),
						DbHelper.MakeInParam("@Allowcstatus",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowcstatus),
						DbHelper.MakeInParam("@Allowuseblog",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowuseblog),
						DbHelper.MakeInParam("@Allowinvisible",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowinvisible),
						DbHelper.MakeInParam("@Allowtransfer",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowtransfer),
						DbHelper.MakeInParam("@Allowsetreadperm",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowsetreadperm),
						DbHelper.MakeInParam("@Allowsetattachperm",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowsetattachperm),
						DbHelper.MakeInParam("@Allowhidecode",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowhidecode),
						DbHelper.MakeInParam("@Allowhtml",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowhtml),
						DbHelper.MakeInParam("@Allowcusbbcode",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowcusbbcode),
						DbHelper.MakeInParam("@Allownickname",(DbType)SqlDbType.Int,4,__usergroupinfo.Allownickname),
						DbHelper.MakeInParam("@Allowsigbbcode",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowsigbbcode),
						DbHelper.MakeInParam("@Allowsigimgcode",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowsigimgcode),
						DbHelper.MakeInParam("@Allowviewpro",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowviewpro),
						DbHelper.MakeInParam("@Allowviewstats",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowviewstats),
						DbHelper.MakeInParam("@Disableperiodctrl",(DbType)SqlDbType.Int,4,__usergroupinfo.Disableperiodctrl),
						DbHelper.MakeInParam("@Reasonpm",(DbType)SqlDbType.Int,4,__usergroupinfo.Reasonpm),
						DbHelper.MakeInParam("@Maxprice",(DbType)SqlDbType.SmallInt,2,__usergroupinfo.Maxprice),
						DbHelper.MakeInParam("@Maxpmnum",(DbType)SqlDbType.SmallInt,2,__usergroupinfo.Maxpmnum),
						DbHelper.MakeInParam("@Maxsigsize",(DbType)SqlDbType.SmallInt,2,__usergroupinfo.Maxsigsize),
						DbHelper.MakeInParam("@Maxattachsize",(DbType)SqlDbType.Int,4,__usergroupinfo.Maxattachsize),
						DbHelper.MakeInParam("@Maxsizeperday",(DbType)SqlDbType.Int,4,__usergroupinfo.Maxsizeperday),
						DbHelper.MakeInParam("@Attachextensions",(DbType)SqlDbType.Char,100,__usergroupinfo.Attachextensions),
						DbHelper.MakeInParam("@Maxspaceattachsize",(DbType)SqlDbType.Int,4,__usergroupinfo.Maxspaceattachsize),
						DbHelper.MakeInParam("@Maxspacephotosize",(DbType)SqlDbType.Int,4,__usergroupinfo.Maxspacephotosize),
						DbHelper.MakeInParam("@Raterange",(DbType)SqlDbType.Char,100,__usergroupinfo.Raterange)
					};

			string sqlstring = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "usergroups]  ([radminid],[grouptitle],[creditshigher],[creditslower]," +
				"[stars] ,[color], [groupavatar],[readaccess], [allowvisit],[allowpost],[allowreply]," +
				"[allowpostpoll], [allowdirectpost],[allowgetattach],[allowpostattach],[allowvote],[allowmultigroups]," +
				"[allowsearch],[allowavatar],[allowcstatus],[allowuseblog],[allowinvisible],[allowtransfer]," +
				"[allowsetreadperm],[allowsetattachperm],[allowhidecode],[allowhtml],[allowcusbbcode],[allownickname]," +
				"[allowsigbbcode],[allowsigimgcode],[allowviewpro],[allowviewstats],[disableperiodctrl],[reasonpm]," +
				"[maxprice],[maxpmnum],[maxsigsize],[maxattachsize],[maxsizeperday],[attachextensions],[raterange],[maxspaceattachsize],[maxspacephotosize]) VALUES(" +
				"@Radminid,@Grouptitle,@Creditshigher,@Creditslower,@Stars,@Color,@Groupavatar,@Readaccess,@Allowvisit,@Allowpost,@Allowreply," +
				"@Allowpostpoll,@Allowdirectpost,@Allowgetattach,@Allowpostattach,@Allowvote,@Allowmultigroups,@Allowsearch,@Allowavatar,@Allowcstatus," +
				"@Allowuseblog,@Allowinvisible,@Allowtransfer,@Allowsetreadperm,@Allowsetattachperm,@Allowhidecode,@Allowhtml,@Allowcusbbcode,@Allownickname," +
				"@Allowsigbbcode,@Allowsigimgcode,@Allowviewpro,@Allowviewstats,@Disableperiodctrl,@Reasonpm,@Maxprice,@Maxpmnum,@Maxsigsize,@Maxattachsize," +
				"@Maxsizeperday,@Attachextensions,@Raterange,@Maxspaceattachsize,@Maxspacephotosize)";

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);
		}

		public void AddOnlineList(string grouptitle)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, GetMaxUserGroupId()),
									  DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 50, grouptitle)
								  };
			string sqlstring = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "onlinelist] ([groupid], [title], [img]) VALUES(@groupid,@title, '')";

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
		}

		public DataTable GetMinCreditHigher()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT MIN(Creditshigher) FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]>8 AND [radminid]=0 ").Tables[0];
		}

		public DataTable GetMaxCreditLower()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT MAX(Creditslower) FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]>8 AND [radminid]=0 ").Tables[0];
		}

		public DataTable GetUserGroupByCreditshigher(int Creditshigher)
		{
			IDataParameter parm = DbHelper.MakeInParam("@Creditshigher", (DbType)SqlDbType.Int, 4, Creditshigher);
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [groupid],[creditshigher],[creditslower] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]>8 AND [radminid]=0  AND [Creditshigher]<=@Creditshigher AND @Creditshigher<[Creditslower]", parm).Tables[0];
		}

		public void UpdateUserGroupCreditsHigher(int currentGroupID, int Creditslower)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, currentGroupID),
									  DbHelper.MakeInParam("@creditshigher", (DbType)SqlDbType.Int, 4, Creditslower)
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET creditshigher=@creditshigher WHERE [groupid]=@groupid", parms);
		}

		public void UpdateUserGroupCreidtsLower(int currentCreditsHigher, int Creditshigher)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@creditslower", (DbType)SqlDbType.Int, 4, Creditshigher),
									  DbHelper.MakeInParam("@creditshigher", (DbType)SqlDbType.Int, 4, currentCreditsHigher)
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [creditslower]=@creditslower WHERE [groupid]>8 AND [radminid]=0 AND [creditshigher]=@creditshigher", parms);
		}

		public DataTable GetUserGroupByCreditsHigherAndLower(int Creditshigher, int Creditslower)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@Creditshigher", (DbType)SqlDbType.Int, 4, Creditshigher),
									  DbHelper.MakeInParam("@Creditslower", (DbType)SqlDbType.Int, 4, Creditslower)
								  };
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [groupid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]>8 AND [radminid]=0 AND [Creditshigher]=@Creditshigher AND [Creditslower]=@Creditslower", parms).Tables[0];
		}
		public int GetGroupCountByCreditsLower(int Creditshigher)
		{
			IDataParameter parm = DbHelper.MakeInParam("@creditslower", (DbType)SqlDbType.Int, 4, Creditshigher);
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [groupid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]>8 AND [radminid]=0 AND [creditslower]=@creditslower", parm).Tables[0].Rows.Count;
		}

		public void UpdateUserGroupsCreditsLowerByCreditsLower(int Creditslower, int Creditshigher)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@Creditshigher", (DbType)SqlDbType.Int, 4, Creditshigher),
									  DbHelper.MakeInParam("@Creditslower", (DbType)SqlDbType.Int, 4, Creditslower)
								  };
			DbHelper.ExecuteDataset(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [creditslower]=@Creditslower WHERE [groupid]>8 AND [radminid]=0 AND [creditslower]=@Creditshigher", parms);
		}

		public void UpdateUserGroupTitleAndCreditsByGroupid(int groupid, string grouptitle, int creditslower, int creditshigher)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.Int,4,groupid),
									  DbHelper.MakeInParam("@grouptitle",(DbType)SqlDbType.NVarChar,50,grouptitle),
									  DbHelper.MakeInParam("@creditslower",(DbType)SqlDbType.Int,4,creditslower),
									  DbHelper.MakeInParam("@creditshigher",(DbType)SqlDbType.Int,4,creditshigher)
								  };
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [grouptitle]=@grouptitle,[creditshigher]=@creditshigher,[creditslower]=@creditslower WHERE [groupid]=@groupid";
			DbHelper.ExecuteDataset(CommandType.Text, sql, parms);
		}

		public void UpdateUserGroupsCreditsHigherByCreditsHigher(int Creditshigher, int Creditslower)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@Creditshigher", (DbType)SqlDbType.Int, 4, Creditshigher),
									  DbHelper.MakeInParam("@Creditslower", (DbType)SqlDbType.Int, 4, Creditslower)
								  };

			DbHelper.ExecuteDataset(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [Creditshigher]=@Creditshigher WHERE [groupid]>8 AND [radminid]=0 AND [Creditshigher]=@Creditslower", parms);
		}

		public DataTable GetUserGroupCreditsLowerAndHigher(int groupid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupid);

			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [groupid],[creditshigher],[creditslower] FROM [" + BaseConfigs.GetTablePrefix + "usergroups]  WHERE [groupid]=@groupid", parm).Tables[0];
		}

		public void UpdateUserGroup(UserGroupInfo __usergroupinfo, int Creditshigher, int Creditslower)
		{
			IDataParameter[] prams = 
					{
						DbHelper.MakeInParam("@Radminid",(DbType)SqlDbType.Int,4,(__usergroupinfo.Groupid == 1) ? 1 : __usergroupinfo.Radminid),
						DbHelper.MakeInParam("@Grouptitle",(DbType)SqlDbType.NVarChar,50, Utils.RemoveFontTag(__usergroupinfo.Grouptitle)),
						DbHelper.MakeInParam("@Creditshigher",(DbType)SqlDbType.Int,4,Creditshigher),
						DbHelper.MakeInParam("@Creditslower",(DbType)SqlDbType.Int,4,Creditslower),
						DbHelper.MakeInParam("@Stars",(DbType)SqlDbType.Int,4,__usergroupinfo.Stars),
						DbHelper.MakeInParam("@Color",(DbType)SqlDbType.Char,7,__usergroupinfo.Color),
						DbHelper.MakeInParam("@Groupavatar",(DbType)SqlDbType.NVarChar,60,__usergroupinfo.Groupavatar),
						DbHelper.MakeInParam("@Readaccess",(DbType)SqlDbType.Int,4,__usergroupinfo.Readaccess),
						DbHelper.MakeInParam("@Allowvisit",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowvisit),
						DbHelper.MakeInParam("@Allowpost",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowpost),
						DbHelper.MakeInParam("@Allowreply",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowreply),
						DbHelper.MakeInParam("@Allowpostpoll",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowpostpoll),
						DbHelper.MakeInParam("@Allowdirectpost",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowdirectpost),
						DbHelper.MakeInParam("@Allowgetattach",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowgetattach),
						DbHelper.MakeInParam("@Allowpostattach",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowpostattach),
						DbHelper.MakeInParam("@Allowvote",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowvote),
						DbHelper.MakeInParam("@Allowmultigroups",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowmultigroups),
						DbHelper.MakeInParam("@Allowsearch",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowsearch),
						DbHelper.MakeInParam("@Allowavatar",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowavatar),
						DbHelper.MakeInParam("@Allowcstatus",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowcstatus),
						DbHelper.MakeInParam("@Allowuseblog",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowuseblog),
						DbHelper.MakeInParam("@Allowinvisible",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowinvisible),
						DbHelper.MakeInParam("@Allowtransfer",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowtransfer),
						DbHelper.MakeInParam("@Allowsetreadperm",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowsetreadperm),
						DbHelper.MakeInParam("@Allowsetattachperm",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowsetattachperm),
						DbHelper.MakeInParam("@Allowhidecode",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowhidecode),
						DbHelper.MakeInParam("@Allowhtml",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowhtml),
						DbHelper.MakeInParam("@Allowcusbbcode",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowcusbbcode),
						DbHelper.MakeInParam("@Allownickname",(DbType)SqlDbType.Int,4,__usergroupinfo.Allownickname),
						DbHelper.MakeInParam("@Allowsigbbcode",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowsigbbcode),
						DbHelper.MakeInParam("@Allowsigimgcode",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowsigimgcode),
						DbHelper.MakeInParam("@Allowviewpro",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowviewpro),
						DbHelper.MakeInParam("@Allowviewstats",(DbType)SqlDbType.Int,4,__usergroupinfo.Allowviewstats),
						DbHelper.MakeInParam("@Disableperiodctrl",(DbType)SqlDbType.Int,4,__usergroupinfo.Disableperiodctrl),
						DbHelper.MakeInParam("@Reasonpm",(DbType)SqlDbType.Int,4,__usergroupinfo.Reasonpm),
						DbHelper.MakeInParam("@Maxprice",(DbType)SqlDbType.SmallInt,2,__usergroupinfo.Maxprice),
						DbHelper.MakeInParam("@Maxpmnum",(DbType)SqlDbType.SmallInt,2,__usergroupinfo.Maxpmnum),
						DbHelper.MakeInParam("@Maxsigsize",(DbType)SqlDbType.SmallInt,2,__usergroupinfo.Maxsigsize),
						DbHelper.MakeInParam("@Maxattachsize",(DbType)SqlDbType.Int,4,__usergroupinfo.Maxattachsize),
						DbHelper.MakeInParam("@Maxsizeperday",(DbType)SqlDbType.Int,4,__usergroupinfo.Maxsizeperday),
						DbHelper.MakeInParam("@Attachextensions",(DbType)SqlDbType.Char,100,__usergroupinfo.Attachextensions),
						DbHelper.MakeInParam("@Maxspaceattachsize",(DbType)SqlDbType.Int,4,__usergroupinfo.Maxspaceattachsize),
						DbHelper.MakeInParam("@Maxspacephotosize",(DbType)SqlDbType.Int,4,__usergroupinfo.Maxspacephotosize),
						DbHelper.MakeInParam("@Groupid",(DbType)SqlDbType.Int,4,__usergroupinfo.Groupid)

					};

			string sqlstring = "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups]  SET [radminid]=@Radminid,[grouptitle]=@Grouptitle,[creditshigher]=@Creditshigher," +
				"[creditslower]=@Creditslower,[stars]=@Stars,[color]=@Color,[groupavatar]=@Groupavatar,[readaccess]=@Readaccess, [allowvisit]=@Allowvisit,[allowpost]=@Allowpost," +
				"[allowreply]=@Allowreply,[allowpostpoll]=@Allowpostpoll, [allowdirectpost]=@Allowdirectpost,[allowgetattach]=@Allowgetattach,[allowpostattach]=@Allowpostattach," +
				"[allowvote]=@Allowvote,[allowmultigroups]=@Allowmultigroups,[allowsearch]=@Allowsearch,[allowavatar]=@Allowavatar,[allowcstatus]=@Allowcstatus," +
				"[allowuseblog]=@Allowuseblog,[allowinvisible]=@Allowinvisible,[allowtransfer]=@Allowtransfer,[allowsetreadperm]=@Allowsetreadperm," +
				"[allowsetattachperm]=@Allowsetattachperm,[allowhidecode]=@Allowhidecode,[allowhtml]=@Allowhtml,[allowcusbbcode]=@Allowcusbbcode,[allownickname]=@Allownickname," +
				"[allowsigbbcode]=@Allowsigbbcode,[allowsigimgcode]=@Allowsigimgcode,[allowviewpro]=@Allowviewpro,[allowviewstats]=@Allowviewstats," +
				"[disableperiodctrl]=@Disableperiodctrl,[reasonpm]=@Reasonpm,[maxprice]=@Maxprice,[maxpmnum]=@Maxpmnum,[maxsigsize]=@Maxsigsize,[maxattachsize]=@Maxattachsize," +
				"[maxsizeperday]=@Maxsizeperday,[attachextensions]=@Attachextensions,[maxspaceattachsize]=@Maxspaceattachsize,[maxspacephotosize]=@Maxspacephotosize  WHERE [groupid]=@Groupid";


			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams);
		}


		public void UpdateOnlineList(UserGroupInfo usergroupinfo)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, usergroupinfo.Groupid),
									  DbHelper.MakeInParam("@title", (DbType)SqlDbType.NVarChar, 50, Utils.RemoveFontTag(usergroupinfo.Grouptitle))
								  };
			string sqlstring = "UPDATE [" + BaseConfigs.GetTablePrefix + "onlinelist] SET [title]=@title WHERE [groupid]=@groupid";

			DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
		}

		public bool IsSystemOrTemplateUserGroup(int groupid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupid);
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 *  FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE ([system]=1 OR [type]=1) AND [groupid]=@groupid", parm).Tables[0].Rows.Count > 0;
		}

		public DataTable GetOthersCommonUserGroup(int exceptgroupid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, exceptgroupid);
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [groupid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]=0 And [groupid]>8 AND [groupid]<>@groupid", parm).Tables[0];
		}

		public string GetUserGroupRAdminId(int groupid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupid);
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [radminid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE  [groupid]=@groupid", parm).Tables[0].Rows[0][0].ToString();
		}

		public void UpdateUserGroupLowerAndHigherToLimit(int groupid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupid);
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [creditshigher]=-9999999 ,creditslower=9999999  WHERE [groupid]=@groupid", parm);
		}

		public void DeleteUserGroup(int groupid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupid);
			DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=@groupid", parm);
		}

		public void DeleteAdminGroup(int groupid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupid);
			DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "admingroups] WHERE [admingid]=@groupid", parm);
		}

		public void DeleteOnlineList(int groupid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupid);
			DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "onlinelist] WHERE [groupid]=@groupid", parm);
		}

		public int GetMaxUserGroupId()
		{
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX(groupid), 0) FROM " + BaseConfigs.GetTablePrefix + "usergroups"), 0);
		}



		public bool DeletePaymentLog()
		{
			try
			{
				DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] ");
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
				DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE " + condition);
				return true;
			}
			catch
			{
				return false;
			}

		}

		public DataTable GetPaymentLogList(int pagesize, int currentpage)
		{
			int pagetop = (currentpage - 1) * pagesize;
			string sqlstring;
			if (currentpage == 1)
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " " + BaseConfigs.GetTablePrefix + "paymentlog.*, " + BaseConfigs.GetTablePrefix + "topics.fid AS fid ," + BaseConfigs.GetTablePrefix + "topics.postdatetime AS postdatetime ," + BaseConfigs.GetTablePrefix + "topics.poster AS authorname, " + BaseConfigs.GetTablePrefix + "topics.title AS title," + BaseConfigs.GetTablePrefix + "users.username As UserName  FROM " + BaseConfigs.GetTablePrefix + "paymentlog LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "topics ON " + BaseConfigs.GetTablePrefix + "paymentlog.tid = " + BaseConfigs.GetTablePrefix + "topics.tid LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "users.uid = " + BaseConfigs.GetTablePrefix + "paymentlog.uid ORDER BY " + BaseConfigs.GetTablePrefix + "paymentlog.id DESC";
			}
			else
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " " + BaseConfigs.GetTablePrefix + "paymentlog.*, " + BaseConfigs.GetTablePrefix + "topics.fid AS fid ," + BaseConfigs.GetTablePrefix + "topics.postdatetime AS postdatetime ," + BaseConfigs.GetTablePrefix + "topics.poster AS authorname, " + BaseConfigs.GetTablePrefix + "topics.title AS title," + BaseConfigs.GetTablePrefix + "users.username As UserName  FROM " + BaseConfigs.GetTablePrefix + "paymentlog LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "topics ON " + BaseConfigs.GetTablePrefix + "paymentlog.tid = " + BaseConfigs.GetTablePrefix + "topics.tid LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "users.uid = " + BaseConfigs.GetTablePrefix + "paymentlog.uid WHERE [id] < (SELECT min([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] ORDER BY [id] DESC) AS tblTmp )  ORDER BY " + BaseConfigs.GetTablePrefix + "paymentlog.id DESC";
			}

			return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
		}

		public DataTable GetPaymentLogList(int pagesize, int currentpage, string condition)
		{
			int pagetop = (currentpage - 1) * pagesize;
			string sqlstring;
			if (currentpage == 1)
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " " + BaseConfigs.GetTablePrefix + "paymentlog.*, " + BaseConfigs.GetTablePrefix + "topics.fid AS fid ," + BaseConfigs.GetTablePrefix + "topics.postdatetime AS postdatetime ," + BaseConfigs.GetTablePrefix + "topics.poster AS authorname, " + BaseConfigs.GetTablePrefix + "topics.title AS title," + BaseConfigs.GetTablePrefix + "users.username As UserName  FROM " + BaseConfigs.GetTablePrefix + "paymentlog LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "topics ON " + BaseConfigs.GetTablePrefix + "paymentlog.tid = " + BaseConfigs.GetTablePrefix + "topics.tid LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "users.uid = " + BaseConfigs.GetTablePrefix + "paymentlog.uid WHERE " + condition + "  Order by [id] DESC";
			}
			else
			{
				sqlstring = "SELECT TOP " + pagesize.ToString() + " " + BaseConfigs.GetTablePrefix + "paymentlog.*, " + BaseConfigs.GetTablePrefix + "topics.fid AS fid ," + BaseConfigs.GetTablePrefix + "topics.postdatetime AS postdatetime ," + BaseConfigs.GetTablePrefix + "topics.poster AS authorname, " + BaseConfigs.GetTablePrefix + "topics.title AS title," + BaseConfigs.GetTablePrefix + "users.username As UserName  FROM " + BaseConfigs.GetTablePrefix + "paymentlog LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "topics ON " + BaseConfigs.GetTablePrefix + "paymentlog.tid = " + BaseConfigs.GetTablePrefix + "topics.tid LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "users.uid = " + BaseConfigs.GetTablePrefix + "paymentlog.uid  WHERE [id] < (SELECT min([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE " + condition + " ORDER BY [id] DESC) AS tblTmp ) AND " + condition + " ORDER BY [" + BaseConfigs.GetTablePrefix + "paymentlog].[id] DESC";
			}

			return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
		}

		/// <summary>
		/// 得到积分交易日志记录数
		/// </summary>
		/// <returns></returns>
		public int GetPaymentLogListCount()
		{
			return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT count(id) FROM [" + BaseConfigs.GetTablePrefix + "paymentlog]").Tables[0].Rows[0][0].ToString());
		}

		/// <summary>
		/// 得到指定查询条件下的积分交易日志数
		/// </summary>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public int GetPaymentLogListCount(string condition)
		{
			return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT count(id) FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE " + condition).Tables[0].Rows[0][0].ToString());
		}

		public void DeleteModeratorByFid(int fid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid)
			};
			string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE [fid]=@fid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}



		public DataTable GetUidModeratorByFid(string fidlist)
		{
			string sql = "SELECT distinct [uid] FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE [fid] IN(" + fidlist + ")";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public void AddModerator(int uid, int fid, int displayorder, int inherited)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
				DbHelper.MakeInParam("@fid", (DbType)SqlDbType.SmallInt, 2, fid),
				DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.SmallInt, 2, displayorder),
				DbHelper.MakeInParam("@inherited", (DbType)SqlDbType.SmallInt, 2, inherited)
			};
			string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "moderators] (uid,fid,displayorder,inherited) VALUES(@uid,@fid,@displayorder,@inherited)";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public DataTable GetModeratorInfo(string moderator)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@username", (DbType)SqlDbType.VarChar, 20, moderator.Trim())
			};

			return DbHelper.ExecuteDataset(CommandType.Text, "Select Top 1 [uid],[groupid]  From [" + BaseConfigs.GetTablePrefix + "users] Where [groupid]<>7 AND [groupid]<>8 AND [username]=@username", prams).Tables[0];
		}

		public void SetModerator(string moderator)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@username", (DbType)SqlDbType.VarChar, 20, moderator.Trim())
			};
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [adminid]=3,[groupid]=3  WHERE [username]=@username", prams);
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "online] SET [adminid]=3,[groupid]=3  WHERE [username]=@username", prams);
		}



		public DataTable GetUidAdminIdByUsername(string username)
		{
			IDataParameter[] prams =
			{
				DbHelper.MakeInParam("@username", (DbType)SqlDbType.VarChar, 20, username)
			};
			string sql = "Select Top 1 [uid],[adminid] From [" + BaseConfigs.GetTablePrefix + "users] Where [username] = @username";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		}

		public DataTable GetUidInModeratorsByUid(int currentfid, int uid)
		{
			IDataParameter[] prams =
			{
				DbHelper.MakeInParam("@currentfid", (DbType)SqlDbType.Int, 4, currentfid),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
			};
			string sql = "Select TOP 1 [uid]  FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE [fid]<>@currentfid AND [uid]=@uid";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		}

		public void UpdateUserOnlineInfo(int groupid, int userid)
		{
			IDataParameter[] prams =
			{
				DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupid),
				DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "online] SET [groupid]=@groupid  WHERE [userid]=@userid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void UpdateUserOtherInfo(int groupid, int userid)
		{
			IDataParameter[] prams =
			{
				DbHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, groupid),
				DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [groupid]=@groupid ,[adminid]=0  WHERE [uid]=@userid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		/// <summary>
		/// 得到论坛中最后注册的用户ID和用户名
		/// </summary>
		/// <param name="lastuserid">输出参数：最后注册的用户ID</param>
		/// <param name="lastusername">输出参数：最后注册的用户名</param>
		/// <returns>存在返回true,不存在返回false</returns>
		public bool GetLastUserInfo(out string lastuserid, out string lastusername)
		{
			lastuserid = "";
			lastusername = "";

			IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [uid],[username] FROM [" + BaseConfigs.GetTablePrefix + "users] ORDER BY [uid] DESC");
			if (reader != null)
			{
				if (reader.Read())
				{
					lastuserid = reader["uid"].ToString();
					lastusername = reader["username"].ToString().Trim();
					reader.Close();
					return true;
				}
				reader.Close();
			}
			return false;

		}

		public IDataReader GetTopUsers(int statcount, int lastuid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@lastuid", (DbType)SqlDbType.Int, 4, lastuid),
			};

			return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP " + statcount + " [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid] > @lastuid", prams);
		}

		public void ResetUserDigestPosts(int userid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userid);
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [digestposts]=(SELECT COUNT(tid) AS [digestposts] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [" + BaseConfigs.GetTablePrefix + "topics].[posterid] = [" + BaseConfigs.GetTablePrefix + "users].[uid] AND [digest] > 0) WHERE [" + BaseConfigs.GetTablePrefix + "users].[uid] = @uid", parm);
		}

		public IDataReader GetUsers(int start_uid, int end_uid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@start_uid", (DbType)SqlDbType.Int, 4, start_uid),
									  DbHelper.MakeInParam("@end_uid", (DbType)SqlDbType.Int, 4, end_uid)
								  };

			return DbHelper.ExecuteReader(CommandType.Text, "SELECT [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid] >= @start_uid AND [uid]<=@end_uid", prams);
		}

		public void UpdateUserPostCount(int postcount, int userid)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@postcount", (DbType)SqlDbType.Int, 4, postcount),
									  DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid)
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [posts]=@postcount WHERE [" + BaseConfigs.GetTablePrefix + "users].[uid] = @userid", parms);
		}


		/// <summary>
		/// 获得所有版主列表
		/// </summary>
		/// <returns></returns>
		public DataTable GetModeratorList()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM [{0}moderators]", BaseConfigs.GetTablePrefix)).Tables[0];
		}


		/// <summary>
		/// 获得全部在线用户数
		/// </summary>
		/// <returns></returns>
		public int GetOnlineAllUserCount()
		{
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(olid) FROM [" + BaseConfigs.GetTablePrefix + "online]"), 1);
		}

		/// <summary>
		/// 创建在线表
		/// </summary>
		/// <returns></returns>
		public int CreateOnlineTable()
		{
			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("IF EXISTS (SELECT * FROM SYSOBJECTS WHERE id = object_id(N'[dnt_online]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) DROP TABLE [dnt_online];");
				sb.Append("CREATE TABLE [dnt_online] ([olid] [int] IDENTITY (1, 1) NOT NULL,[userid] [int] NOT NULL,[ip] [varchar] (15) NOT NULL,[username] [nvarchar] (20) NOT NULL,[nickname] [nvarchar] (20) NOT NULL,[password] [char] (32) NOT NULL,[groupid] [smallint] NOT NULL,[olimg] [varchar] (80) NOT NULL,[adminid] [smallint] NOT NULL,[invisible] [smallint] NOT NULL,[action] [smallint] NOT NULL,[lastactivity] [smallint] NOT NULL,[lastposttime] [datetime] NOT NULL,[lastpostpmtime] [datetime] NOT NULL,[lastsearchtime] [datetime] NOT NULL,[lastupdatetime] [datetime] NOT NULL,[forumid] [int] NOT NULL,[forumname] [nvarchar] (50) NOT NULL,[titleid] [int] NOT NULL,[title] [nvarchar] (80) NOT NULL,[verifycode] [varchar] (10) NOT NULL ) ON [PRIMARY];");
				sb.Append("ALTER TABLE [dnt_online] WITH NOCHECK ADD CONSTRAINT [PK_dnt_online] PRIMARY KEY CLUSTERED ([olid]) ON [PRIMARY]; ");
				sb.Append("ALTER TABLE [dnt_online] ADD CONSTRAINT [DF_dnt_online_userid] DEFAULT ((-1)) FOR [userid],CONSTRAINT [DF_dnt_online_ip] DEFAULT ('0.0.0.0') FOR [ip],CONSTRAINT [DF_dnt_online_username] DEFAULT ('') FOR [username],CONSTRAINT [DF_dnt_online_nickname] DEFAULT ('') FOR [nickname],CONSTRAINT [DF_dnt_online_password] DEFAULT ('') FOR [password],CONSTRAINT [DF_dnt_online_groupid] DEFAULT (0) FOR [groupid],CONSTRAINT [DF_dnt_online_olimg] DEFAULT ('') FOR [olimg],CONSTRAINT [DF_dnt_online_adminid] DEFAULT (0) FOR [adminid],CONSTRAINT [DF_dnt_online_invisible] DEFAULT (0) FOR [invisible],CONSTRAINT [DF_dnt_online_action] DEFAULT (0) FOR [action],CONSTRAINT [DF_dnt_online_lastactivity] DEFAULT (0) FOR [lastactivity],CONSTRAINT [DF_dnt_online_lastposttime] DEFAULT ('1900-1-1 00:00:00') FOR [lastposttime],CONSTRAINT [DF_dnt_online_lastpostpmtime] DEFAULT ('1900-1-1 00:00:00') FOR [lastpostpmtime],CONSTRAINT [DF_dnt_online_lastsearchtime] DEFAULT ('1900-1-1 00:00:00') FOR [lastsearchtime],CONSTRAINT [DF_dnt_online_lastupdatetime] DEFAULT (getdate()) FOR [lastupdatetime],CONSTRAINT [DF_dnt_online_forumid] DEFAULT (0) FOR [forumid],CONSTRAINT [DF_dnt_online_forumname] DEFAULT ('') FOR [forumname],CONSTRAINT [DF_dnt_online_titleid] DEFAULT (0) FOR [titleid],CONSTRAINT [DF_dnt_online_title] DEFAULT ('') FOR [title],CONSTRAINT [DF_dnt_online_verifycode] DEFAULT ('') FOR [verifycode];");
				sb.Append("CREATE INDEX [forum] ON [dnt_online]([userid], [forumid], [invisible]) ON [PRIMARY];");
				sb.Append("CREATE INDEX [invisible] ON [dnt_online]([userid], [invisible]) ON [PRIMARY];");
				sb.Append("CREATE INDEX [forumid] ON [dnt_online]([forumid]) ON [PRIMARY];");
				sb.Append("CREATE INDEX [password] ON [dnt_online]([userid], [password]) ON [PRIMARY];");
				sb.Append("CREATE INDEX [ip] ON [dnt_online]([userid], [ip]) ON [PRIMARY];");

				return DbHelper.ExecuteNonQuery(CommandType.Text, sb.Replace("dnt_", BaseConfigs.GetBaseConfig().Tableprefix).ToString());
			}
			catch
			{
				return -1;
			}
		}

		///// <summary>
		///// 取得在线表最后一条记录的tickcount字段
		///// </summary>
		///// <returns></returns>
		//public DateTime GetLastUpdateTime()
		//{
		//    object val =
		//        DbHelper.ExecuteScalar(CommandType.Text,
		//                               "SELECT TOP 1 [lastupdatetime] FROM [" + BaseConfigs.GetTablePrefix +
		//                               "online] ORDER BY [olid] DESC");
		//    return val == null ? DateTime.Now : Convert.ToDateTime(val);
		//}

		/// <summary>
		/// 获得在线注册用户总数量
		/// </summary>
		/// <returns>用户数量</returns>
		public int GetOnlineUserCount()
		{

			return (int)DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT(olid) FROM [" + BaseConfigs.GetTablePrefix + "online] WHERE [userid]>0").Tables[0].Rows[0][0];

		}

		/// <summary>
		/// 获得版块在线用户列表
		/// </summary>
		/// <param name="forumid">版块Id</param>
		/// <returns></returns>
		public DataTable GetForumOnlineUserListTable(int forumid)
		{
			return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM [{0}online] WHERE [forumid]={1}", BaseConfigs.GetTablePrefix, forumid)).Tables[0];
		}

		/// <summary>
		/// 获得全部在线用户列表
		/// </summary>
		/// <returns></returns>
		public DataTable GetOnlineUserListTable()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "online]").Tables[0];
		}

		/// <summary>
		/// 获得版块在线用户列表
		/// </summary>
		/// <param name="forumid">版块Id</param>
		/// <returns></returns>
		public IDataReader GetForumOnlineUserList(int forumid)
		{
			return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}online] WHERE [forumid]={1}", BaseConfigs.GetTablePrefix, forumid.ToString()));
		}

		/// <summary>
		/// 获得全部在线用户列表
		/// </summary>
		/// <returns></returns>
		public IDataReader GetOnlineUserList()
		{
			return DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "online]");
		}

		/// <summary>
		/// 返回在线用户图例
		/// </summary>
		/// <returns></returns>
		public DataTable GetOnlineGroupIconTable()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [groupid], [displayorder], [title], [img] FROM [" + BaseConfigs.GetTablePrefix + "onlinelist] WHERE [img] <> '' ORDER BY [displayorder]").Tables[0];
		}

        /// <summary>
        /// 获得指定在线用户
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <returns>在线用户的详细信息</returns>
        public IDataReader GetOnlineUser(int olid)
        {
            DbParameter[] parms = { DbHelper.MakeInParam("@olid", (DbType)SqlDbType.Int, 4, olid) };
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM [{0}online] WHERE [olid]=@olid", BaseConfigs.GetTablePrefix), parms).Tables[0];
        }

		/// <summary>
		/// 获得指定用户的详细信息
		/// </summary>
		/// <param name="userid">在线用户ID</param>
		/// <param name="password">用户密码</param>
		/// <returns>用户的详细信息</returns>
		public DataTable GetOnlineUser(int userid, string password)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
									  DbHelper.MakeInParam("@password", (DbType)SqlDbType.Char, 32, password)
								  };
			return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP 1 * FROM [{0}online] WHERE [userid]=@userid AND [password]=@password", BaseConfigs.GetTablePrefix), parms).Tables[0];
		}

		/// <summary>
		/// 获得指定用户的详细信息
		/// </summary>
		/// <param name="userid">在线用户ID</param>
		/// <param name="ip">IP</param>
		/// <returns></returns>
		public DataTable GetOnlineUserByIP(int userid, string ip)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
									  DbHelper.MakeInParam("@ip", (DbType)SqlDbType.VarChar, 15, ip)
								  };
			return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP 1 * FROM [{0}online] WHERE [userid]=@userid AND [ip]=@ip", BaseConfigs.GetTablePrefix), parms).Tables[0];
		}

		/// <summary>
		/// 检查在线用户验证码是否有效
		/// </summary>
		/// <param name="olid">在组用户ID</param>
		/// <param name="verifycode">验证码</param>
		/// <returns>在组用户ID</returns>
		public bool CheckUserVerifyCode(int olid, string verifycode, string newverifycode)
		{
			IDataParameter[] parms = { 
									  DbHelper.MakeInParam("@olid", (DbType)SqlDbType.Int, 4, olid),
									  DbHelper.MakeInParam("@verifycode", (DbType)SqlDbType.VarChar, 10, verifycode)
								  };
			DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP 1 [olid] FROM [{0}online] WHERE [olid]=@olid and [verifycode]=@verifycode", BaseConfigs.GetTablePrefix), parms).Tables[0];
			parms[1].Value = newverifycode;
			DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [verifycode]=@verifycode WHERE [olid]=@olid", BaseConfigs.GetTablePrefix), parms);
			return dt.Rows.Count > 0;
		}

		/// <summary>
		/// 设置用户在线状态
		/// </summary>
		/// <param name="uid">用户Id</param>
		/// <param name="onlinestate">在线状态，１在线</param>
		/// <returns></returns>
		public int SetUserOnlineState(int uid, int onlinestate)
		{
			return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [onlinestate]={1},[lastactivity]=GETDATE(),[lastvisit]=GETDATE() WHERE [uid]={2}", BaseConfigs.GetTablePrefix, onlinestate, uid));
		}

		/// <summary>
		/// 删除符合条件的一个或多个用户信息
		/// </summary>
		/// <returns>删除行数</returns>
		public int DeleteRowsByIP(string ip)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@ip",(DbType)SqlDbType.VarChar,15,ip)
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [onlinestate]=0,[lastactivity]=GETDATE() WHERE [uid] IN (SELECT [userid] FROM [" + BaseConfigs.GetTablePrefix + "online] WHERE [userid]>0 AND [ip]=@ip)", prams);
			if (ip != "0.0.0.0")
			{
				return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "online] WHERE [userid]=-1 AND [ip]=@ip", prams);
			}
			return 0;
		}

		/// <summary>
		/// 删除在线表中指定在线id的行
		/// </summary>
		/// <param name="olid">在线id</param>
		/// <returns></returns>
		public int DeleteRows(int olid)
		{
			return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "online] WHERE [olid]=" + olid.ToString());
		}

		/// <summary>
		/// 更新用户的当前动作及相关信息
		/// </summary>
		/// <param name="olid">在线列表id</param>
		/// <param name="action">动作</param>
		/// <param name="inid">所在位置代码</param>
		public void UpdateAction(int olid, int action, int inid)
		{
			IDataParameter[] prams = {
									  //DbHelper.MakeInParam("@tickcount",(DbType)SqlDbType.Int,4,System.Environment.TickCount),
									  DbHelper.MakeInParam("@action",(DbType)SqlDbType.SmallInt,2,action),
									  DbHelper.MakeInParam("@lastupdatetime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))),
									  DbHelper.MakeInParam("@forumid",(DbType)SqlDbType.Int,4,inid),
									  DbHelper.MakeInParam("@forumname",(DbType)SqlDbType.NVarChar,100,""),
									  DbHelper.MakeInParam("@titleid",(DbType)SqlDbType.Int,4,inid),
									  DbHelper.MakeInParam("@title",(DbType)SqlDbType.NVarChar,160,""),
									  DbHelper.MakeInParam("@olid",(DbType)SqlDbType.Int,4,olid)

								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "online] SET [lastactivity]=[action],[action]=@action,[lastupdatetime]=@lastupdatetime,[forumid]=@forumid,[forumname]=@forumname,[titleid]=@titleid,[title]=@title WHERE [olid]=@olid", prams);
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
		public void UpdateAction(int olid, int action, int fid, string forumname, int tid, string topictitle)
		{
			IDataParameter[] prams = {
									  //DbHelper.MakeInParam("@tickcount",(DbType)SqlDbType.Int,4,System.Environment.TickCount),
									  DbHelper.MakeInParam("@action",(DbType)SqlDbType.SmallInt,2,action),
									  DbHelper.MakeInParam("@lastupdatetime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))),
									  DbHelper.MakeInParam("@forumid",(DbType)SqlDbType.Int,4,fid),
									  DbHelper.MakeInParam("@forumname",(DbType)SqlDbType.NVarChar,100,forumname),
									  DbHelper.MakeInParam("@titleid",(DbType)SqlDbType.Int,4,tid),
									  DbHelper.MakeInParam("@title",(DbType)SqlDbType.NVarChar,160,topictitle),
									  DbHelper.MakeInParam("@olid",(DbType)SqlDbType.Int,4,olid)

								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "online] SET [lastactivity]=[action],[action]=@action,[lastupdatetime]=@lastupdatetime,[forumid]=@forumid,[forumname]=@forumname,[titleid]=@titleid,[title]=@title WHERE [olid]=@olid", prams);
		}

		/// <summary>
		/// 更新用户最后活动时间
		/// </summary>
		/// <param name="olid">在线id</param>
		public void UpdateLastTime(int olid)
		{
			IDataParameter[] prams = {
									  //DbHelper.MakeInParam("@tickcount",(DbType)SqlDbType.Int,4,System.Environment.TickCount),
									  DbHelper.MakeInParam("@lastupdatetime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))),
									  DbHelper.MakeInParam("@olid",(DbType)SqlDbType.Int,4,olid)

								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "online] SET [lastupdatetime]=@lastupdatetime WHERE [olid]=@olid", prams);
		}

		/// <summary>
		/// 更新用户最后发帖时间
		/// </summary>
		/// <param name="olid">在线id</param>
		public void UpdatePostTime(int olid)
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [lastposttime]='{1}' WHERE [olid]={2}", BaseConfigs.GetTablePrefix, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), olid.ToString()));
		}

		/// <summary>
		/// 更新用户最后发短消息时间
		/// </summary>
		/// <param name="olid">在线id</param>
		public void UpdatePostPMTime(int olid)
		{

			DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [lastpostpmtime]='{1}' WHERE [olid]={2}", BaseConfigs.GetTablePrefix, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), olid.ToString()));

		}

		/// <summary>
		/// 更新在线表中指定用户是否隐身
		/// </summary>
		/// <param name="olid">在线id</param>
		/// <param name="invisible">是否隐身</param>
		public void UpdateInvisible(int olid, int invisible)
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [invisible]={1} WHERE [olid]={2}", BaseConfigs.GetTablePrefix, invisible.ToString(), olid.ToString()));
		}

		/// <summary>
		/// 更新在线表中指定用户的用户密码
		/// </summary>
		/// <param name="olid">在线id</param>
		/// <param name="password">用户密码</param>
		public void UpdatePassword(int olid, string password)
		{

			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@password",(DbType)SqlDbType.Char,32,password),
									  DbHelper.MakeInParam("@olid",(DbType)SqlDbType.Int,4,olid)

								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [password]=@password WHERE [olid]=@olid", BaseConfigs.GetTablePrefix), prams);
		}

		/// <summary>
		/// 更新用户IP地址
		/// </summary>
		/// <param name="olid">在线id</param>
		/// <param name="ip">ip地址</param>
		public void UpdateIP(int olid, string ip)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@ip",(DbType)SqlDbType.VarChar,15,ip),
									  DbHelper.MakeInParam("@olid",(DbType)SqlDbType.Int,4,olid)

								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [ip]=@ip WHERE [olid]=@olid", BaseConfigs.GetTablePrefix), prams);

		}

		/// <summary>
		/// 更新用户最后搜索时间
		/// </summary>
		/// <param name="olid">在线id</param>
		public void UpdateSearchTime(int olid)
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [lastsearchtime]={1} WHERE [olid]={2}", BaseConfigs.GetTablePrefix, olid.ToString()));
		}

		/// <summary>
		/// 更新用户的用户组
		/// </summary>
		/// <param name="userid">用户ID</param>
		/// <param name="groupid">组名</param>
		public void UpdateGroupid(int userid, int groupid)
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [groupid]={1} WHERE [userid]={2}", BaseConfigs.GetTablePrefix, groupid.ToString(), userid.ToString()));
		}


		/// <summary>
		/// 获得指定ID的短消息的内容
		/// </summary>
		/// <param name="pmid">短消息pmid</param>
		/// <returns>短消息内容</returns>
		public IDataReader GetPrivateMessageInfo(int pmid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@pmid", (DbType)SqlDbType.Int,4, pmid),
			};
			return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "pms] WHERE [pmid]=@pmid", prams);
		}

		/// <summary>
		/// 获得指定用户的短信息列表
		/// </summary>
		/// <param name="userid">用户ID</param>
		/// <param name="folder">短信息类型(0:收件箱,1:发件箱,2:草稿箱)</param>
		/// <param name="pagesize">每页显示短信息数</param>
		/// <param name="pageindex">当前要显示的页数</param>
		/// <param name="inttypewhere">筛选条件</param>
		/// <returns>短信息列表</returns>
		public IDataReader GetPrivateMessageList(int userid, int folder, int pagesize, int pageindex, int inttype)
		{
			string strwhere = "";
			if (inttype == 1)
			{
				strwhere = "[new]=1";
			}
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@userid",(DbType)SqlDbType.Int,4,userid),
									  DbHelper.MakeInParam("@folder",(DbType)SqlDbType.Int,4,folder),
									  DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pagesize),
									  DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,pageindex),
									  DbHelper.MakeInParam("@strwhere",(DbType)SqlDbType.VarChar,500,strwhere)
									   
								  };
			IDataReader reader = DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getpmlist", prams);
			return reader;
		}

		/// <summary>
		/// 得到当用户的短消息数量
		/// </summary>
		/// <param name="userid">用户ID</param>
		/// <param name="folder">所属文件夹(0:收件箱,1:发件箱,2:草稿箱)</param>
		/// <param name="state">短消息状态(0:已读短消息、1:未读短消息、-1:全部短消息)</param>
		/// <returns>短消息数量</returns>
		public int GetPrivateMessageCount(int userid, int folder, int state)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@userid",(DbType)SqlDbType.Int,4,userid),
									  DbHelper.MakeInParam("@folder",(DbType)SqlDbType.Int,4,folder),								   
									  DbHelper.MakeInParam("@state",(DbType)SqlDbType.Int,4,state)
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getpmcount", prams).ToString(), 0);
		}

		/// <summary>
		/// 创建短消息
		/// </summary>
		/// <param name="__privatemessageinfo">短消息内容</param>
		/// <param name="savetosentbox">设置短消息是否在发件箱保留(0为不保留, 1为保留)</param>
		/// <returns>短消息在数据库中的pmid</returns>
		public int CreatePrivateMessage(PrivateMessageInfo __privatemessageinfo, int savetosentbox)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@pmid",(DbType)SqlDbType.Int,4,__privatemessageinfo.Pmid),
									  DbHelper.MakeInParam("@msgfrom",(DbType)SqlDbType.NVarChar,20,__privatemessageinfo.Msgfrom),
									  DbHelper.MakeInParam("@msgfromid",(DbType)SqlDbType.Int,4,__privatemessageinfo.Msgfromid),
									  DbHelper.MakeInParam("@msgto",(DbType)SqlDbType.NVarChar,20,__privatemessageinfo.Msgto),
									  DbHelper.MakeInParam("@msgtoid",(DbType)SqlDbType.Int,4,__privatemessageinfo.Msgtoid),
									  DbHelper.MakeInParam("@folder",(DbType)SqlDbType.SmallInt,2,__privatemessageinfo.Folder),
									  DbHelper.MakeInParam("@new",(DbType)SqlDbType.Int,4,__privatemessageinfo.New),
									  DbHelper.MakeInParam("@subject",(DbType)SqlDbType.NVarChar,80,__privatemessageinfo.Subject),
									  DbHelper.MakeInParam("@postdatetime",(DbType)SqlDbType.DateTime,8,DateTime.Parse(__privatemessageinfo.Postdatetime)),
									  DbHelper.MakeInParam("@message",(DbType)SqlDbType.NText,0,__privatemessageinfo.Message),
									  DbHelper.MakeInParam("@savetosentbox",(DbType)SqlDbType.Int,4,savetosentbox)
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "createpm", prams).ToString(), -1);
		}

		/// <summary>
		/// 删除指定用户的短信息
		/// </summary>
		/// <param name="userid">用户ID</param>
		/// <param name="pmitemid">要删除的短信息列表(数组)</param>
		/// <returns>删除记录数</returns>
		public int DeletePrivateMessages(int userid, string pmidlist)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int,4, userid)
								  };

			return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "pms] WHERE [pmid] IN (" + pmidlist + ") AND ([msgtoid] = @userid OR [msgfromid] = @userid)", prams);

		}

		/// <summary>
		/// 获得新短消息数
		/// </summary>
		/// <returns></returns>
		public int GetNewPMCount(int userid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int,4, userid)
								  };
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pmid]) AS [pmcount] FROM [" + BaseConfigs.GetTablePrefix + "pms] WHERE [new] = 1 AND [folder] = 0 AND [msgtoid] = @userid", prams), 0);
		}

		/// <summary>
		/// 删除指定用户的一条短消息
		/// </summary>
		/// <param name="userid">用户Ｉｄ</param>
		/// <param name="pmid">ＰＭＩＤ</param>
		/// <returns></returns>
		public int DeletePrivateMessage(int userid, int pmid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int,4, userid),
									  DbHelper.MakeInParam("@pmid", (DbType)SqlDbType.Int,4, pmid)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "pms] WHERE [pmid]=@pmid AND ([msgtoid] = @userid OR [msgfromid] = @userid)", prams);

		}

		/// <summary>
		/// 设置短信息状态
		/// </summary>
		/// <param name="pmid">短信息ID</param>
		/// <param name="state">状态值</param>
		/// <returns>更新记录数</returns>
		public int SetPrivateMessageState(int pmid, byte state)
		{

			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@pmid", (DbType)SqlDbType.Int,1,pmid),
									  DbHelper.MakeInParam("@state",(DbType)SqlDbType.TinyInt,1,state)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "pms] SET [new]=@state WHERE [pmid]=@pmid", prams);

		}

		public int GetRAdminIdByGroup(int groupid)
		{
			return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [radminid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=" + groupid).Tables[0].Rows[0][0].ToString());
		}

		public string GetUserGroupsStr()
		{
			return "SELECT [groupid], [grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] ORDER BY [groupid]";
		}


		public DataTable GetUserNameListByGroupid(string groupidlist)
		{
			string sql = "SELECT [uid] ,[username]  From [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid] IN(" + groupidlist + ")";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public DataTable GetUserNameByUid(int uid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
			};
			string sql = "SELECT TOP 1 [username] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=@uid";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, prams).Tables[0];
		}

		public void ResetPasswordUid(string password, int uid)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@password", (DbType)SqlDbType.Char, 32, password),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
			};
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [password]=@password WHERE [uid]=@uid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public void SendPMToUser(string msgfrom, int msgfromid, string msgto, int msgtoid, int folder, string subject, DateTime postdatetime, string message)
		{
			IDataParameter[] prams = 
			{
				DbHelper.MakeInParam("@msgfrom", (DbType)SqlDbType.NVarChar,50, msgfrom),
				DbHelper.MakeInParam("@msgfromid", (DbType)SqlDbType.Int, 4, msgfromid),
				DbHelper.MakeInParam("@msgto", (DbType)SqlDbType.NVarChar,50, msgto),
				DbHelper.MakeInParam("@msgtoid", (DbType)SqlDbType.Int, 4, msgtoid),
				DbHelper.MakeInParam("@folder", (DbType)SqlDbType.SmallInt, 2, folder),
				DbHelper.MakeInParam("@subject", (DbType)SqlDbType.NVarChar,60, subject),
				DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime,8, postdatetime),
				DbHelper.MakeInParam("@message",(DbType)SqlDbType.NText, 0,message)
			};
			string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "pms] (msgfrom,msgfromid,msgto,msgtoid,folder,new,subject,postdatetime,message) " +
				"VALUES (@msgfrom,@msgfromid,@msgto,@msgtoid,@folder,1,@subject,@postdatetime,@message)";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
			sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [newpmcount]=[newpmcount]+1  WHERE [uid] =@msgtoid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public string GetSystemGroupInfoSql()
		{
			return "Select * From [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]<=8 Order By [groupid]";
		}

		public void UpdateUserCredits(int uid, string credits)
		{
			IDataParameter[] prams_credits = {
											  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
										  };

			DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [credits] = {1} WHERE [uid]=@uid", BaseConfigs.GetTablePrefix, credits), prams_credits);
		}

		public void UpdateUserGroup(int uid, int groupid)
		{
			IDataParameter[] prams_credits = {
											  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
										  };

			DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [groupid] = {1} WHERE [uid]=@uid", BaseConfigs.GetTablePrefix, groupid), prams_credits);

		}

		public bool CheckUserCreditsIsEnough(int uid, float[] values)
		{
			IDataParameter[] prams = {
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
			string CommandText = "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=@uid AND"
				+ "	[extcredits1]>= (case when @extcredits1<0 then abs(@extcredits1) else [extcredits1] end) AND "
				+ "	[extcredits2]>= (case when @extcredits2<0 then abs(@extcredits2) else [extcredits2] end) AND "
				+ "	[extcredits3]>= (case when @extcredits3<0 then abs(@extcredits3) else [extcredits3] end) AND "
				+ "	[extcredits4]>= (case when @extcredits4<0 then abs(@extcredits4) else [extcredits4] end) AND "
				+ "	[extcredits5]>= (case when @extcredits5<0 then abs(@extcredits5) else [extcredits5] end) AND "
				+ "	[extcredits6]>= (case when @extcredits6<0 then abs(@extcredits6) else [extcredits6] end) AND "
				+ "	[extcredits7]>= (case when @extcredits7<0 then abs(@extcredits7) else [extcredits7] end) AND "
				+ "	[extcredits8]>= (case when @extcredits8<0 then abs(@extcredits8) else [extcredits8] end) ";

			if (Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, CommandText, prams)) == 0)
			{
				return false;
			}
			return true;
		}

		public void UpdateUserCredits(int uid, float[] values)
		{
			IDataParameter[] prams = {
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

			string CommandText = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET "
				+ "		[extcredits1]=[extcredits1] + @extcredits1, "
				+ "		[extcredits2]=[extcredits2] + @extcredits2, "
				+ "		[extcredits3]=[extcredits3] + @extcredits3, "
				+ "		[extcredits4]=[extcredits4] + @extcredits4, "
				+ "		[extcredits5]=[extcredits5] + @extcredits5, "
				+ "		[extcredits6]=[extcredits6] + @extcredits6, "
				+ "		[extcredits7]=[extcredits7] + @extcredits7, "
				+ "		[extcredits8]=[extcredits8] + @extcredits8 "
				+ "WHERE [uid]=@uid";

			DbHelper.ExecuteNonQuery(CommandType.Text, CommandText, prams);
		}

		public bool CheckUserCreditsIsEnough(int uid, DataRow values, int pos, int mount)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
									  DbHelper.MakeInParam("@extcredits1", (DbType)SqlDbType.Float, 8, Utils.StrToFloat(values["extcredits1"],0) * pos * mount),
									  DbHelper.MakeInParam("@extcredits2", (DbType)SqlDbType.Float, 8, Utils.StrToFloat(values["extcredits2"],0) * pos * mount),
									  DbHelper.MakeInParam("@extcredits3", (DbType)SqlDbType.Float, 8, Utils.StrToFloat(values["extcredits3"],0) * pos * mount),
									  DbHelper.MakeInParam("@extcredits4", (DbType)SqlDbType.Float, 8, Utils.StrToFloat(values["extcredits4"],0) * pos * mount),
									  DbHelper.MakeInParam("@extcredits5", (DbType)SqlDbType.Float, 8, Utils.StrToFloat(values["extcredits5"],0) * pos * mount),
									  DbHelper.MakeInParam("@extcredits6", (DbType)SqlDbType.Float, 8, Utils.StrToFloat(values["extcredits6"],0) * pos * mount),
									  DbHelper.MakeInParam("@extcredits7", (DbType)SqlDbType.Float, 8, Utils.StrToFloat(values["extcredits7"],0) * pos * mount),
									  DbHelper.MakeInParam("@extcredits8", (DbType)SqlDbType.Float, 8, Utils.StrToFloat(values["extcredits8"],0) * pos * mount)
								  };
			string CommandText = "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=@uid AND"
				+ "	[extcredits1]>= (case when @extcredits1 >= 0 then abs(@extcredits1) else 0 end) AND "
				+ "	[extcredits2]>= (case when @extcredits2 >= 0 then abs(@extcredits2) else 0 end) AND "
				+ "	[extcredits3]>= (case when @extcredits3 >= 0 then abs(@extcredits3) else 0 end) AND "
				+ "	[extcredits4]>= (case when @extcredits4 >= 0 then abs(@extcredits4) else 0 end) AND "
				+ "	[extcredits5]>= (case when @extcredits5 >= 0 then abs(@extcredits5) else 0 end) AND "
				+ "	[extcredits6]>= (case when @extcredits6 >= 0 then abs(@extcredits6) else 0 end) AND "
				+ "	[extcredits7]>= (case when @extcredits7 >= 0 then abs(@extcredits7) else 0 end) AND "
				+ "	[extcredits8]>= (case when @extcredits8 >= 0 then abs(@extcredits8) else 0 end) ";

			if (Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, CommandText, prams)) == 0)
			{
				return false;
			}
			return true;
		}

		public void UpdateUserCredits(int uid, DataRow values, int pos, int mount)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
									  DbHelper.MakeInParam("@extcredits1", (DbType)SqlDbType.Float, 8, Utils.StrToFloat(values["extcredits1"],0) * pos * mount),
									  DbHelper.MakeInParam("@extcredits2", (DbType)SqlDbType.Float, 8, Utils.StrToFloat(values["extcredits2"],0) * pos * mount),
									  DbHelper.MakeInParam("@extcredits3", (DbType)SqlDbType.Float, 8, Utils.StrToFloat(values["extcredits3"],0) * pos * mount),
									  DbHelper.MakeInParam("@extcredits4", (DbType)SqlDbType.Float, 8, Utils.StrToFloat(values["extcredits4"],0) * pos * mount),
									  DbHelper.MakeInParam("@extcredits5", (DbType)SqlDbType.Float, 8, Utils.StrToFloat(values["extcredits5"],0) * pos * mount),
									  DbHelper.MakeInParam("@extcredits6", (DbType)SqlDbType.Float, 8, Utils.StrToFloat(values["extcredits6"],0) * pos * mount),
									  DbHelper.MakeInParam("@extcredits7", (DbType)SqlDbType.Float, 8, Utils.StrToFloat(values["extcredits7"],0) * pos * mount),
									  DbHelper.MakeInParam("@extcredits8", (DbType)SqlDbType.Float, 8, Utils.StrToFloat(values["extcredits8"],0) * pos * mount)
								  };
			if (pos < 0 && mount < 0)
			{
				for (int i = 1; i < prams.Length; i++)
				{
					prams[i].Value = -Convert.ToInt32(prams[i].Value);
				}
			}

			string CommandText = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET "
				+ "	[extcredits1]=[extcredits1] + @extcredits1, "
				+ "	[extcredits2]=[extcredits2] + @extcredits2, "
				+ "	[extcredits3]=[extcredits3] + @extcredits3, "
				+ "	[extcredits4]=[extcredits4] + @extcredits4, "
				+ "	[extcredits5]=[extcredits5] + @extcredits5, "
				+ "	[extcredits6]=[extcredits6] + @extcredits6, "
				+ "	[extcredits7]=[extcredits7] + @extcredits7, "
				+ "	[extcredits8]=[extcredits8] + @extcredits8 "
				+ "WHERE [uid]=@uid";

			DbHelper.ExecuteNonQuery(CommandType.Text, CommandText, prams);
		}


		public DataTable GetUserGroups()
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "usergroups] ORDER BY [groupid]").Tables[0];
		}

		public DataTable GetUserGroupRateRange(int groupid)
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [raterange] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=" + groupid.ToString()).Tables[0];
		}

		public IDataReader GetUserTodayRate(int uid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
								  };
			return DbHelper.ExecuteReader(CommandType.Text, "SELECT [extcredits], SUM(ABS([score])) AS [todayrate] FROM [" + BaseConfigs.GetTablePrefix + "ratelog] WHERE DATEDIFF(d,[postdatetime],getdate()) = 0 AND [uid] = @uid GROUP BY [extcredits]", prams);
		}


		public string GetSpecialGroupInfoSql()
		{
			return "Select * From [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]=-1 And [groupid]>8 Order By [groupid]";
		}


		/// <summary>
		/// 更新在线时间
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <returns></returns>
		public int UpdateOnlineTime(int uid)
		{
			return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [oltime] = [oltime] + DATEDIFF(n,[lastvisit],GETDATE()) WHERE [uid]={1}", BaseConfigs.GetTablePrefix, uid));
		}

		/// <summary>
		/// 返回指定用户的信息
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <returns>用户信息</returns>
		public IDataReader GetUserInfoToReader(int uid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int,4, uid),
			};

			return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getuserinfo", prams);
		}

		/// <summary>
		/// 获取简短用户信息
		/// </summary>
		/// <param name="uid">用id</param>
		/// <returns>用户简短信息</returns>
		public IDataReader GetShortUserInfoToReader(int uid)
		{

			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int,4, uid),
			};

			return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getshortuserinfo", prams);

		}

		/// <summary>
		/// 根据IP查找用户
		/// </summary>
		/// <param name="ip">ip地址</param>
		/// <returns>用户信息</returns>
		public IDataReader GetUserInfoByIP(string ip)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@regip", (DbType)SqlDbType.Char,15, ip),
			};

			return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [" + BaseConfigs.GetTablePrefix + "users].*, [" + BaseConfigs.GetTablePrefix + "userfields].* FROM [" + BaseConfigs.GetTablePrefix + "users] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "users].[uid]=[" + BaseConfigs.GetTablePrefix + "userfields].[uid] WHERE [" + BaseConfigs.GetTablePrefix + "users].[regip]=@regip ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] DESC", prams);

		}

		public IDataReader GetUserName(int uid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
			};
			return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [username] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [" + BaseConfigs.GetTablePrefix + "users].[uid]=@uid", prams);

		}

		public IDataReader GetUserJoinDate(int uid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
			};
			return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [joindate] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [" + BaseConfigs.GetTablePrefix + "users].[uid]=@uid", prams);
		}

		public IDataReader GetUserID(string username)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@username",(DbType)SqlDbType.VarChar,20,username),
			};
			return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [" + BaseConfigs.GetTablePrefix + "users].[username]=@username", prams);
		}

		public DataTable GetUserList(int pagesize, int currentpage)
		{
			#region 获得用户列表
			int pagetop = (currentpage - 1) * pagesize;

			if (currentpage == 1)
			{
				return DbHelper.ExecuteDataset("SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "users].[uid], [" + BaseConfigs.GetTablePrefix + "users].[username],[" + BaseConfigs.GetTablePrefix + "users].[nickname], [" + BaseConfigs.GetTablePrefix + "users].[joindate], [" + BaseConfigs.GetTablePrefix + "users].[credits], [" + BaseConfigs.GetTablePrefix + "users].[posts], [" + BaseConfigs.GetTablePrefix + "users].[lastactivity], [" + BaseConfigs.GetTablePrefix + "users].[email],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[accessmasks], [" + BaseConfigs.GetTablePrefix + "userfields].[location],[" + BaseConfigs.GetTablePrefix + "usergroups].[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "users] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "userfields].[uid] = [" + BaseConfigs.GetTablePrefix + "users].[uid] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "usergroups] ON [" + BaseConfigs.GetTablePrefix + "usergroups].[groupid]=[" + BaseConfigs.GetTablePrefix + "users].[groupid] ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] DESC").Tables[0];
			}
			else
			{
				string sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "users].[uid], [" + BaseConfigs.GetTablePrefix + "users].[username],[" + BaseConfigs.GetTablePrefix + "users].[nickname], [" + BaseConfigs.GetTablePrefix + "users].[joindate], [" + BaseConfigs.GetTablePrefix + "users].[credits], [" + BaseConfigs.GetTablePrefix + "users].[posts], [" + BaseConfigs.GetTablePrefix + "users].[lastactivity], [" + BaseConfigs.GetTablePrefix + "users].[email],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[accessmasks], [" + BaseConfigs.GetTablePrefix + "userfields].[location],[" + BaseConfigs.GetTablePrefix + "usergroups].[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "users],[" + BaseConfigs.GetTablePrefix + "userfields],[" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [" + BaseConfigs.GetTablePrefix + "userfields].[uid] = [" + BaseConfigs.GetTablePrefix + "users].[uid] AND  [" + BaseConfigs.GetTablePrefix + "usergroups].[groupid]=[" + BaseConfigs.GetTablePrefix + "users].[groupid] AND [" + BaseConfigs.GetTablePrefix + "users].[uid] < (SELECT min([uid])  FROM (SELECT TOP " + pagetop + " [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] ORDER BY [uid] DESC) AS tblTmp )  ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] DESC";
				return DbHelper.ExecuteDataset(sqlstring).Tables[0];
			}

			#endregion
		}

		/// <summary>
		/// 获得用户列表DataTable
		/// </summary>
		/// <param name="pagesize">每页记录数</param>
		/// <param name="pageindex">当前页数</param>
		/// <returns>用户列表DataTable</returns>
		public DataTable GetUserList(int pagesize, int pageindex, string orderby, string ordertype)
		{
			string[] arrayorderby = new string[] { "username", "credits", "posts", "admin", "lastactivity" };
			int i = Array.IndexOf(arrayorderby, orderby);

			switch (i)
			{
					//case 0:
					//    orderby = "ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] " + ordertype;
					//    break;
				case 0:
					orderby = string.Format("ORDER BY [{0}users].[username] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
					break;
				case 1:
					orderby = string.Format("ORDER BY [{0}users].[credits] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
					break;
				case 2:
					orderby = string.Format("ORDER BY [{0}users].[posts] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
					break;
				case 3:
					orderby = string.Format("WHERE [{0}users].[adminid] > 0 ORDER BY [{0}users].[adminid] {1}, [{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
					break;
					//case "joindate":
					//    orderby = "ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[joindate] " + ordertype + ",[" + BaseConfigs.GetTablePrefix + "users].[uid] " + ordertype;
					//    break;
				case 4:
					orderby = string.Format("ORDER BY [{0}users].[lastactivity] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
					break;
				default:
					orderby = string.Format("ORDER BY [{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
					break;
			}

			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pagesize),
									  DbHelper.MakeInParam("@pageindex",(DbType)SqlDbType.Int,4,pageindex),
									  DbHelper.MakeInParam("@orderby",(DbType)SqlDbType.VarChar,1000,orderby)
								  };
			return DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getuserlist", prams).Tables[0];
		}

		/// <summary>
		/// 判断指定用户名是否已存在
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <returns>如果已存在该用户id则返回true, 否则返回false</returns>
		public bool Exists(int uid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
			};
			return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=@uid", prams)) >= 1;
		}

		/// <summary>
		/// 判断指定用户名是否已存在.
		/// </summary>
		/// <param name="username">用户名</param>
		/// <returns>如果已存在该用户名则返回true, 否则返回false</returns>
		public bool Exists(string username)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@username",(DbType)SqlDbType.Char,20,username),
			};
			return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM [{0}users] WHERE [username]=@username", BaseConfigs.GetTablePrefix), prams)) >= 1;
		}

		/// <summary>
		/// 是否有指定ip地址的用户注册
		/// </summary>
		/// <param name="ip">ip地址</param>
		/// <returns>存在返回true,否则返回false</returns>
		public bool ExistsByIP(string ip)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@regip",(DbType)SqlDbType.Char, 15,ip),
			};
			return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM [{0}users] WHERE [regip]=@regip", BaseConfigs.GetTablePrefix), prams)) >= 1;
		}

		/// <summary>
		/// 检测Email和安全项
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="email">email</param>
		/// <param name="questionid">问题id</param>
		/// <param name="answer">答案</param>
		/// <returns>如果正确则返回用户id, 否则返回-1</returns>
		public IDataReader CheckEmailAndSecques(string username, string email, string secques)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@username",(DbType)SqlDbType.Char,20,username),
									  DbHelper.MakeInParam("@email",(DbType)SqlDbType.Char,50, email),
									  DbHelper.MakeInParam("@secques",(DbType)SqlDbType.Char,8, secques)
								  };
			return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "checkemailandsecques", prams);
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
		public IDataReader CheckPasswordAndSecques(string username, string password, bool originalpassword, string secques)
		{

			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@username",(DbType)SqlDbType.Char,20,username),
									  DbHelper.MakeInParam("@password",(DbType)SqlDbType.Char,32, originalpassword ? Utils.MD5(password) : password),
									  DbHelper.MakeInParam("@secques",(DbType)SqlDbType.Char,8, secques)
								  };
			return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "checkpasswordandsecques", prams);
		}

		/// <summary>
		/// 检查密码
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="password">密码</param>
		/// <param name="originalpassword">是否非MD5密码</param>
		/// <returns>如果正确则返回用户id, 否则返回-1</returns>
		public IDataReader CheckPassword(string username, string password, bool originalpassword)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@username",(DbType)SqlDbType.Char,20, username),
									  DbHelper.MakeInParam("@password",(DbType)SqlDbType.Char,32, originalpassword ? Utils.MD5(password) : password)
								  };
			return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "checkpasswordbyusername", prams);
		}

		/// <summary>
		/// 检测DVBBS兼容模式的密码
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="password">密码</param>
		/// <param name="questionid">问题id</param>
		/// <param name="answer">答案</param>
		/// <returns>如果正确则返回用户id, 否则返回-1</returns>
		public IDataReader CheckDvBbsPasswordAndSecques(string username, string password)
		{

			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@username",(DbType)SqlDbType.Char,20,username),
									  DbHelper.MakeInParam("@password",(DbType)SqlDbType.Char,32, Utils.MD5(password).Substring(8, 16))
								  };
			return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [uid], [password], [secques] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [username]=@username", prams);
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
		public IDataReader CheckPassword(int uid, string password, bool originalpassword)
		{

			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									  DbHelper.MakeInParam("@password",(DbType)SqlDbType.Char,32, originalpassword ? Utils.MD5(password) : password)
								  };
			return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "checkpasswordbyuid", prams);
		}

		/// <summary>
		/// 根据指定的email查找用户并返回用户uid
		/// </summary>
		/// <param name="email">email地址</param>
		/// <returns>用户uid</returns>
		public IDataReader FindUserEmail(string email)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@email",(DbType)SqlDbType.Char,50, email),
			};
			return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [email]=@email", prams);
		}

		/// <summary>
		/// 得到论坛中用户总数
		/// </summary>
		/// <returns>用户总数</returns>
		public int GetUserCount()
		{
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(uid) FROM [" + BaseConfigs.GetTablePrefix + "users]"), 0);
		}

		/// <summary>
		/// 得到论坛中用户总数
		/// </summary>
		/// <returns>用户总数</returns>
		public int GetUserCountByAdmin()
		{
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(uid) FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [" + BaseConfigs.GetTablePrefix + "users].[adminid] > 0"), 0);
		}

		/// <summary>
		/// 创建新用户.
		/// </summary>
		/// <param name="__userinfo">用户信息</param>
		/// <returns>返回用户ID, 如果已存在该用户名则返回-1</returns>
		public int CreateUser(UserInfo __userinfo)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@username",(DbType)SqlDbType.Char,20,__userinfo.Username),
									  DbHelper.MakeInParam("@nickname",(DbType)SqlDbType.Char,20,__userinfo.Nickname),
									  DbHelper.MakeInParam("@password",(DbType)SqlDbType.Char,32,__userinfo.Password),
									  DbHelper.MakeInParam("@secques",(DbType)SqlDbType.Char,8,__userinfo.Secques),
									  DbHelper.MakeInParam("@gender",(DbType)SqlDbType.Int,4,__userinfo.Gender),
									  DbHelper.MakeInParam("@adminid",(DbType)SqlDbType.Int,4,__userinfo.Adminid),
									  DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.SmallInt,2,__userinfo.Groupid),
									  DbHelper.MakeInParam("@groupexpiry",(DbType)SqlDbType.Int,4,__userinfo.Groupexpiry),
									  DbHelper.MakeInParam("@extgroupids",(DbType)SqlDbType.Char,60,__userinfo.Extgroupids),
									  DbHelper.MakeInParam("@regip",(DbType)SqlDbType.VarChar,0,__userinfo.Regip),
									  DbHelper.MakeInParam("@joindate",(DbType)SqlDbType.VarChar,0,__userinfo.Joindate),
									  DbHelper.MakeInParam("@lastip",(DbType)SqlDbType.Char,15,__userinfo.Lastip),
									  DbHelper.MakeInParam("@lastvisit",(DbType)SqlDbType.VarChar,0,__userinfo.Lastvisit),
									  DbHelper.MakeInParam("@lastactivity",(DbType)SqlDbType.VarChar,0,__userinfo.Lastactivity),
									  DbHelper.MakeInParam("@lastpost",(DbType)SqlDbType.VarChar,0,__userinfo.Lastpost),
									  DbHelper.MakeInParam("@lastpostid",(DbType)SqlDbType.Int,4,__userinfo.Lastpostid),
									  DbHelper.MakeInParam("@lastposttitle",(DbType)SqlDbType.VarChar,0,__userinfo.Lastposttitle),
									  DbHelper.MakeInParam("@posts",(DbType)SqlDbType.Int,4,__userinfo.Posts),
									  DbHelper.MakeInParam("@digestposts",(DbType)SqlDbType.SmallInt,2,__userinfo.Digestposts),
									  DbHelper.MakeInParam("@oltime",(DbType)SqlDbType.Int,2,__userinfo.Oltime),
									  DbHelper.MakeInParam("@pageviews",(DbType)SqlDbType.Int,4,__userinfo.Pageviews),
									  DbHelper.MakeInParam("@credits",(DbType)SqlDbType.Int,4,__userinfo.Credits),
									  DbHelper.MakeInParam("@extcredits1",(DbType)SqlDbType.Float,8,__userinfo.Extcredits1),
									  DbHelper.MakeInParam("@extcredits2",(DbType)SqlDbType.Float,8,__userinfo.Extcredits2),
									  DbHelper.MakeInParam("@extcredits3",(DbType)SqlDbType.Float,8,__userinfo.Extcredits3),
									  DbHelper.MakeInParam("@extcredits4",(DbType)SqlDbType.Float,8,__userinfo.Extcredits4),
									  DbHelper.MakeInParam("@extcredits5",(DbType)SqlDbType.Float,8,__userinfo.Extcredits5),
									  DbHelper.MakeInParam("@extcredits6",(DbType)SqlDbType.Float,8,__userinfo.Extcredits6),
									  DbHelper.MakeInParam("@extcredits7",(DbType)SqlDbType.Float,8,__userinfo.Extcredits7),
									  DbHelper.MakeInParam("@extcredits8",(DbType)SqlDbType.Float,8,__userinfo.Extcredits8),
									  DbHelper.MakeInParam("@avatarshowid",(DbType)SqlDbType.Int,4,__userinfo.Avatarshowid),
									  DbHelper.MakeInParam("@email",(DbType)SqlDbType.Char,50,__userinfo.Email),
									  DbHelper.MakeInParam("@bday",(DbType)SqlDbType.VarChar,0,__userinfo.Bday),
									  DbHelper.MakeInParam("@sigstatus",(DbType)SqlDbType.Int,4,__userinfo.Sigstatus),
									  DbHelper.MakeInParam("@tpp",(DbType)SqlDbType.Int,4,__userinfo.Tpp),
									  DbHelper.MakeInParam("@ppp",(DbType)SqlDbType.Int,4,__userinfo.Ppp),
									  DbHelper.MakeInParam("@templateid",(DbType)SqlDbType.SmallInt,2,__userinfo.Templateid),
									  DbHelper.MakeInParam("@pmsound",(DbType)SqlDbType.Int,4,__userinfo.Pmsound),
									  DbHelper.MakeInParam("@showemail",(DbType)SqlDbType.Int,4,__userinfo.Showemail),
									  DbHelper.MakeInParam("@newsletter",(DbType)SqlDbType.Int,4,__userinfo.Newsletter),
									  DbHelper.MakeInParam("@invisible",(DbType)SqlDbType.Int,4,__userinfo.Invisible),
									  //DbHelper.MakeInParam("@timeoffset",(DbType)SqlDbType.Char,4,__userinfo.Timeoffset),
									  DbHelper.MakeInParam("@newpm",(DbType)SqlDbType.Int,4,__userinfo.Newpm),
									  DbHelper.MakeInParam("@accessmasks",(DbType)SqlDbType.Int,4,__userinfo.Accessmasks),
									  //
									  DbHelper.MakeInParam("@website",(DbType)SqlDbType.VarChar,80,__userinfo.Website),
									  DbHelper.MakeInParam("@icq",(DbType)SqlDbType.VarChar,12,__userinfo.Icq),
									  DbHelper.MakeInParam("@qq",(DbType)SqlDbType.VarChar,12,__userinfo.Qq),
									  DbHelper.MakeInParam("@yahoo",(DbType)SqlDbType.VarChar,40,__userinfo.Yahoo),
									  DbHelper.MakeInParam("@msn",(DbType)SqlDbType.VarChar,40,__userinfo.Msn),
									  DbHelper.MakeInParam("@skype",(DbType)SqlDbType.VarChar,40,__userinfo.Skype),
									  DbHelper.MakeInParam("@location",(DbType)SqlDbType.VarChar,30,__userinfo.Location),
									  DbHelper.MakeInParam("@customstatus",(DbType)SqlDbType.VarChar,30,__userinfo.Customstatus),
									  DbHelper.MakeInParam("@avatar",(DbType)SqlDbType.VarChar,255,__userinfo.Avatar),
									  DbHelper.MakeInParam("@avatarwidth",(DbType)SqlDbType.Int,4,__userinfo.Avatarwidth),
									  DbHelper.MakeInParam("@avatarheight",(DbType)SqlDbType.Int,4,__userinfo.Avatarheight),
									  DbHelper.MakeInParam("@medals",(DbType)SqlDbType.VarChar,40, __userinfo.Medals),
									  DbHelper.MakeInParam("@bio",(DbType)SqlDbType.NVarChar,500,__userinfo.Bio),
									  DbHelper.MakeInParam("@signature",(DbType)SqlDbType.NVarChar,500,__userinfo.Signature),
									  DbHelper.MakeInParam("@sightml",(DbType)SqlDbType.NVarChar,1000,__userinfo.Sightml),
									  DbHelper.MakeInParam("@authstr",(DbType)SqlDbType.VarChar,20,__userinfo.Authstr),
									  DbHelper.MakeInParam("@realname",(DbType)SqlDbType.NVarChar,10,__userinfo.Realname),
									  DbHelper.MakeInParam("@idcard",(DbType)SqlDbType.VarChar,20,__userinfo.Idcard),
									  DbHelper.MakeInParam("@mobile",(DbType)SqlDbType.VarChar,20,__userinfo.Mobile),
									  DbHelper.MakeInParam("@phone",(DbType)SqlDbType.VarChar,20,__userinfo.Phone)
								  };

			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "createuser", prams), -1);
		}

		/// <summary>
		/// 更新权限验证字符串
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="authstr">验证串</param>
		/// <param name="authflag">验证标志</param>
		public void UpdateAuthStr(int uid, string authstr, int authflag)
		{

			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
									  DbHelper.MakeInParam("@authstr", (DbType)SqlDbType.Char, 20, authstr),
									  DbHelper.MakeInParam("@authflag", (DbType)SqlDbType.Int, 4, authflag) 
								  };
			DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateuserauthstr", prams);
		}

		/// <summary>
		/// 更新指定用户的个人资料
		/// </summary>
		/// <param name="__userinfo">用户信息</param>
		/// <returns>如果用户不存在则为false, 否则为true</returns>
		public void UpdateUserProfile(UserInfo __userinfo)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, __userinfo.Uid), 
									  DbHelper.MakeInParam("@nickname",(DbType)SqlDbType.Char,20,__userinfo.Nickname),
									  DbHelper.MakeInParam("@gender", (DbType)SqlDbType.Int, 4, __userinfo.Gender), 
									  DbHelper.MakeInParam("@email", (DbType)SqlDbType.Char, 50, __userinfo.Email), 
									  DbHelper.MakeInParam("@bday", (DbType)SqlDbType.Char, 10, __userinfo.Bday), 
									  DbHelper.MakeInParam("@showemail", (DbType)SqlDbType.Int, 4, __userinfo.Showemail),
									  DbHelper.MakeInParam("@website", (DbType)SqlDbType.VarChar, 80, __userinfo.Website), 
									  DbHelper.MakeInParam("@icq", (DbType)SqlDbType.VarChar, 12, __userinfo.Icq), 
									  DbHelper.MakeInParam("@qq", (DbType)SqlDbType.VarChar, 12, __userinfo.Qq), 
									  DbHelper.MakeInParam("@yahoo", (DbType)SqlDbType.VarChar, 40, __userinfo.Yahoo), 
									  DbHelper.MakeInParam("@msn", (DbType)SqlDbType.VarChar, 40, __userinfo.Msn), 
									  DbHelper.MakeInParam("@skype", (DbType)SqlDbType.VarChar, 40, __userinfo.Skype), 
									  DbHelper.MakeInParam("@location", (DbType)SqlDbType.NVarChar, 30, __userinfo.Location), 
									  DbHelper.MakeInParam("@bio", (DbType)SqlDbType.NVarChar, 500, __userinfo.Bio),
									  DbHelper.MakeInParam("@realname",(DbType)SqlDbType.NVarChar,10,__userinfo.Realname),
									  DbHelper.MakeInParam("@idcard",(DbType)SqlDbType.VarChar,20,__userinfo.Idcard),
									  DbHelper.MakeInParam("@mobile",(DbType)SqlDbType.VarChar,20,__userinfo.Mobile),
									  DbHelper.MakeInParam("@phone",(DbType)SqlDbType.VarChar,20,__userinfo.Phone)
								  };

			DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateuserprofile", prams);
		}

		/// <summary>
		/// 更新用户论坛设置
		/// </summary>
		/// <param name="__userinfo">用户信息</param>
		/// <returns>如果用户不存在则返回false, 否则返回true</returns>
		public void UpdateUserForumSetting(UserInfo __userinfo)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,__userinfo.Uid),
									  DbHelper.MakeInParam("@tpp",(DbType)SqlDbType.Int,4,__userinfo.Tpp),
									  DbHelper.MakeInParam("@ppp",(DbType)SqlDbType.Int,4,__userinfo.Ppp),
									  DbHelper.MakeInParam("@invisible",(DbType)SqlDbType.Int,4,__userinfo.Invisible),
									  DbHelper.MakeInParam("@customstatus",(DbType)SqlDbType.VarChar,30,__userinfo.Customstatus),
									  DbHelper.MakeInParam("@sigstatus", (DbType)SqlDbType.Int, 4, __userinfo.Sigstatus),
									  DbHelper.MakeInParam("@signature", (DbType)SqlDbType.NVarChar, 500, __userinfo.Signature),
									  DbHelper.MakeInParam("@sightml", (DbType)SqlDbType.NVarChar, 1000, __userinfo.Sightml)
								  };

			DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateuserforumsetting", prams);
		}

		/// <summary>
		/// 修改用户自定义积分字段的值
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="extid">扩展字段序号(1-8)</param>
		/// <param name="pos">增加的数值(可以是负数)</param>
		/// <returns>执行是否成功</returns>
		public void UpdateUserExtCredits(int uid, int extid, float pos)
		{
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [extcredits" + extid.ToString() + "]=[extcredits" + extid.ToString() + "] + (" + pos.ToString() + ") WHERE [uid]=" + uid.ToString());
		}

		/// <summary>
		/// 获得指定用户的指定积分扩展字段的值
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="extid">扩展字段序号(1-8)</param>
		/// <returns>值</returns>
		public float GetUserExtCredits(int uid, int extid)
		{
			return Utils.StrToFloat(DbHelper.ExecuteDataset(CommandType.Text, "SELECT [extcredits" + extid.ToString() + "] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=" + uid.ToString()).Tables[0].Rows[0][0], 0);
		}

		///// <summary>
		///// 更新用户签名
		///// </summary>
		///// <param name="uid">用户id</param>
		///// <param name="signature">签名</param>
		///// <returns>如果用户不存在则返回false, 否则返回true</returns>
		//public void UpdateUserSignature(int uid, int sigstatus, string signature, string sightml)
		//{
		//    IDataParameter[] prams = {
		//                               DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
		//                               DbHelper.MakeInParam("@sigstatus",(DbType)SqlDbType.Int,4,sigstatus),
		//                               DbHelper.MakeInParam("@signature",(DbType)SqlDbType.NVarChar,500,signature),
		//                               DbHelper.MakeInParam("@sightml",(DbType)SqlDbType.NVarChar,1000,sightml)
		//                           };
		//    DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateusersignature", prams);
		//}

		/// <summary>
		/// 更新用户头像
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="avatar">头像</param>
		/// <param name="avatarwidth">头像宽度</param>
		/// <param name="avatarheight">头像高度</param>
		/// <returns>如果用户不存在则返回false, 否则返回true</returns>
		public void UpdateUserPreference(int uid, string avatar, int avatarwidth, int avatarheight, int templateid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,uid),
									  DbHelper.MakeInParam("@avatar",(DbType)SqlDbType.VarChar,255,avatar),
									  DbHelper.MakeInParam("@avatarwidth",(DbType)SqlDbType.Int,4,avatarwidth),
									  DbHelper.MakeInParam("@avatarheight",(DbType)SqlDbType.Int,4,avatarheight),
									  DbHelper.MakeInParam("@templateid", (DbType)SqlDbType.SmallInt, 4, templateid)
								  };

			DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateuserpreference", prams);
		}

		/// <summary>
		/// 更新用户密码
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="password">密码</param>
		/// <param name="originalpassword">是否非MD5密码</param>
		/// <returns>成功返回true否则false</returns>
		public void UpdateUserPassword(int uid, string password, bool originalpassword)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
									  DbHelper.MakeInParam("@password", (DbType)SqlDbType.Char, 32, originalpassword ? Utils.MD5(password) : password)
								  };

			DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateuserpassword", prams);
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
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
									  DbHelper.MakeInParam("@secques", (DbType)SqlDbType.Char, 8, secques)
								  };

			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [secques]=@secques WHERE [uid]=@uid", prams);
		}

		/// <summary>
		/// 更新用户最后登录时间
		/// </summary>
		/// <param name="uid">用户id</param>
		public void UpdateUserLastvisit(int uid, string ip)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
									  DbHelper.MakeInParam("@ip", (DbType)SqlDbType.Char,15, ip)
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [lastvisit]=GETDATE(), [lastip]=@ip WHERE [uid] =@uid", prams);
		}

		public void UpdateUserOnlineStateAndLastActivity(string uidlist, int onlinestate, string activitytime)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@onlinestate", (DbType)SqlDbType.Int, 4, onlinestate),
									  DbHelper.MakeInParam("@activitytime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(activitytime))
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [onlinestate]=@onlinestate,[lastactivity] = @activitytime WHERE [uid] IN (" + uidlist + ")", prams);
		}

		public void UpdateUserOnlineStateAndLastActivity(int uid, int onlinestate, string activitytime)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
									  DbHelper.MakeInParam("@onlinestate", (DbType)SqlDbType.Int, 4, onlinestate),
									  DbHelper.MakeInParam("@activitytime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(activitytime))
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [onlinestate]=@onlinestate,[lastactivity] = @activitytime WHERE [uid]=@uid", prams);
		}

		public void UpdateUserOnlineStateAndLastVisit(string uidlist, int onlinestate, string activitytime)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@onlinestate", (DbType)SqlDbType.Int, 4, onlinestate),
									  DbHelper.MakeInParam("@activitytime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(activitytime))
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [onlinestate]=@onlinestate,[lastvisit] = @activitytime WHERE [uid] IN (" + uidlist + ")", prams);
		}

		public void UpdateUserOnlineStateAndLastVisit(int uid, int onlinestate, string activitytime)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
									  DbHelper.MakeInParam("@onlinestate", (DbType)SqlDbType.Int, 4, onlinestate),
									  DbHelper.MakeInParam("@activitytime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(activitytime))
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [onlinestate]=@onlinestate,[lastvisit] = @activitytime WHERE [uid]=@uid", prams);
		}

		/// <summary>
		/// 更新用户当前的在线时间和最后活动时间
		/// </summary>
		/// <param name="uid">用户uid</param>
		public void UpdateUserOnlineTime(int uid, string activitytime)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid),
									  DbHelper.MakeInParam("@activitytime", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(activitytime))
								  };


			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [lastactivity] = @activitytime  WHERE [uid] = @uid", prams);

		}

		/// <summary>
		/// 设置用户信息表中未读短消息的数量
		/// </summary>
		/// <param name="uid">用户ID</param>
		/// <param name="pmnum">短消息数量</param>
		/// <returns>更新记录个数</returns>
		public int SetUserNewPMCount(int uid, int pmnum)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
									  DbHelper.MakeInParam("@value", (DbType)SqlDbType.Int, 4, pmnum)
								  };
			return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [newpmcount]=@value WHERE [uid]=@uid", prams);
		}

		/// <summary>
		/// 更新指定用户的勋章信息
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="medals">勋章信息</param>
		public void UpdateMedals(int uid, string medals)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
									  DbHelper.MakeInParam("@medals", (DbType)SqlDbType.VarChar, 300, medals)
								  };
			DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "userfields] SET [medals]=@medals WHERE [uid]=@uid", prams);

		}

		public int DecreaseNewPMCount(int uid, int subval)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
									  DbHelper.MakeInParam("@subval", (DbType)SqlDbType.Int, 4, subval)
								  };

			try
			{
				return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [newpmcount]=CASE WHEN [newpmcount] >= 0 THEN [newpmcount]-@subval ELSE 0 END WHERE [uid]=@uid", prams);
			}
			catch
			{
				return -1;
			}
		}

		/// <summary>
		/// 得到用户新短消息数量
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <returns>新短消息数</returns>
		public int GetUserNewPMCount(int uid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid), 
			};
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT [newpmcount] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=@uid", prams), 0);
		}

		/// <summary>
		/// 更新用户精华数
		/// </summary>
		/// <param name="useridlist">uid列表</param>
		/// <returns></returns>
		public int UpdateUserDigest(string useridlist)
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [digestposts] = (");
			sql.Append("SELECT COUNT([tid]) AS [digest] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [" + BaseConfigs.GetTablePrefix + "topics].[posterid] = [" + BaseConfigs.GetTablePrefix + "users].[uid] AND [digest]>0");
			sql.Append(") WHERE [uid] IN (");
			sql.Append(useridlist);
			sql.Append(")");

			return DbHelper.ExecuteNonQuery(CommandType.Text, sql.ToString());
		}

		/// <summary>
		/// 更新用户SpaceID
		/// </summary>
		/// <param name="spaceid">要更新的SpaceId</param>
		/// <param name="userid">要更新的UserId</param>
		/// <returns></returns>
		public void UpdateUserSpaceId(int spaceid, int userid)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@spaceid",(DbType)SqlDbType.Int,4,spaceid),
									  DbHelper.MakeInParam("@uid",(DbType)SqlDbType.Int,4,userid)
								  };
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [spaceid]=@spaceid WHERE [uid]=@uid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, prams);
		}

		public DataTable GetUserIdByAuthStr(string authstr)
		{
			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@authstr",(DbType)SqlDbType.VarChar,20,authstr)
								  };

			DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT [uid] FROM [" + BaseConfigs.GetTablePrefix + "userfields] WHERE DateDiff(d,[authtime],getdate())<=3  AND [authstr]=@authstr", prams).Tables[0];

			return dt;
		}

		/// <summary>
		/// 执行在线用户向表及缓存中添加的操作。
		/// </summary>
		/// <param name="__onlineuserinfo">在组用户信息内容</param>
		/// <returns>添加成功则返回刚刚添加的olid,失败则返回0</returns>
		public int AddOnlineUser(OnlineUserInfo __onlineuserinfo, int timeout)
		{

			string strDelTimeOutSql = "";
			// 如果timeout为负数则代表不需要精确更新用户是否在线的状态
			if (timeout > 0)
			{
				if (__onlineuserinfo.Userid > 0)
				{
					strDelTimeOutSql = string.Format("{0}UPDATE [{1}users] SET [onlinestate]=1 WHERE [uid]={2};", strDelTimeOutSql, BaseConfigs.GetTablePrefix, __onlineuserinfo.Userid.ToString());
				}
			}
			else
			{
				timeout = timeout * -1;
			}

			if (timeout > 9999)
			{
				timeout = 9999;
			}

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			System.Text.StringBuilder sb2 = new System.Text.StringBuilder();

			IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT [userid] FROM [{0}online] WHERE [lastupdatetime]<'{1}'", BaseConfigs.GetTablePrefix, DateTime.Parse(DateTime.Now.AddMinutes(timeout * -1).ToString("yyyy-MM-dd HH:mm:ss"))));
			try
			{
				while (dr.Read())
				{
					sb.Append(",");
					sb.Append(dr[0].ToString());
					if (dr[0].ToString() != "-1")
					{
						sb2.Append(",");
						sb2.Append(dr[0].ToString());
					}
				}
			}
			finally
			{
				dr.Close();
			}

			if (sb.Length > 0)
			{
				sb.Remove(0, 1);
				strDelTimeOutSql = string.Format("{0}DELETE FROM [{1}online] WHERE [userid] IN ({2});", strDelTimeOutSql, BaseConfigs.GetTablePrefix, sb.ToString());
			}
			if (sb2.Length > 0)
			{
				sb2.Remove(0, 1);
				strDelTimeOutSql = string.Format("{0}UPDATE [{1}users] SET [onlinestate]=0,[lastactivity]=GETDATE() WHERE [uid] IN ({2});", strDelTimeOutSql, BaseConfigs.GetTablePrefix, sb2.ToString());
			}




			IDataParameter[] prams = {
									  DbHelper.MakeInParam("@userid",(DbType)SqlDbType.Int,4,__onlineuserinfo.Userid),
									  DbHelper.MakeInParam("@ip",(DbType)SqlDbType.VarChar,15,__onlineuserinfo.Ip),
									  DbHelper.MakeInParam("@username",(DbType)SqlDbType.NVarChar,40,__onlineuserinfo.Username),
									  //DbHelper.MakeInParam("@tickcount",(DbType)SqlDbType.Int,4,System.Environment.TickCount),
									  DbHelper.MakeInParam("@nickname",(DbType)SqlDbType.NVarChar,40,__onlineuserinfo.Nickname),
									  DbHelper.MakeInParam("@password",(DbType)SqlDbType.Char,32,__onlineuserinfo.Password),
									  DbHelper.MakeInParam("@groupid",(DbType)SqlDbType.SmallInt,2,__onlineuserinfo.Groupid),
									  DbHelper.MakeInParam("@olimg",(DbType)SqlDbType.VarChar,80,__onlineuserinfo.Olimg),
									  DbHelper.MakeInParam("@adminid",(DbType)SqlDbType.SmallInt,2,__onlineuserinfo.Adminid),
									  DbHelper.MakeInParam("@invisible",(DbType)SqlDbType.SmallInt,2,__onlineuserinfo.Invisible),
									  DbHelper.MakeInParam("@action",(DbType)SqlDbType.SmallInt,2,__onlineuserinfo.Action),
									  DbHelper.MakeInParam("@lastactivity",(DbType)SqlDbType.SmallInt,2,__onlineuserinfo.Lastactivity),
									  DbHelper.MakeInParam("@lastposttime",(DbType)SqlDbType.DateTime,8,DateTime.Parse(__onlineuserinfo.Lastposttime)),
									  DbHelper.MakeInParam("@lastpostpmtime",(DbType)SqlDbType.DateTime,8,DateTime.Parse(__onlineuserinfo.Lastpostpmtime)),
									  DbHelper.MakeInParam("@lastsearchtime",(DbType)SqlDbType.DateTime,8,DateTime.Parse(__onlineuserinfo.Lastsearchtime)),
									  DbHelper.MakeInParam("@lastupdatetime",(DbType)SqlDbType.DateTime,8,DateTime.Parse(__onlineuserinfo.Lastupdatetime)),
									  DbHelper.MakeInParam("@forumid",(DbType)SqlDbType.Int,4,__onlineuserinfo.Forumid),
									  DbHelper.MakeInParam("@forumname",(DbType)SqlDbType.NVarChar,50,""),
									  DbHelper.MakeInParam("@titleid",(DbType)SqlDbType.Int,4,__onlineuserinfo.Titleid),
									  DbHelper.MakeInParam("@title",(DbType)SqlDbType.NVarChar,80,""),
									  DbHelper.MakeInParam("@verifycode",(DbType)SqlDbType.VarChar,10,__onlineuserinfo.Verifycode)
								  };
			int olid = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, strDelTimeOutSql + "INSERT INTO [" + BaseConfigs.GetTablePrefix + "online] ([userid],[ip],[username],[nickname],[password],[groupid],[olimg],[adminid],[invisible],[action],[lastactivity],[lastposttime],[lastpostpmtime],[lastsearchtime],[lastupdatetime],[forumid],[forumname],[titleid],[title], [verifycode])VALUES(@userid,@ip,@username,@nickname,@password,@groupid,@olimg,@adminid,@invisible,@action,@lastactivity,@lastposttime,@lastpostpmtime,@lastsearchtime,@lastupdatetime,@forumid,@forumname,@titleid,@title,@verifycode);SELECT SCOPE_IDENTITY()", prams).ToString(), 0);

			// 如果id值太大则重建在线表
			if (olid > 2147483000)
			{
				CreateOnlineTable();
				DbHelper.ExecuteNonQuery(CommandType.Text, strDelTimeOutSql + "INSERT INTO [" + BaseConfigs.GetTablePrefix + "online] ([userid],[ip],[username],[nickname],[password],[groupid],[olimg],[adminid],[invisible],[action],[lastactivity],[lastposttime],[lastpostpmtime],[lastsearchtime],[lastupdatetime],[forumid],[titleid],[verifycode])VALUES(@userid,@ip,@username,@nickname,@password,@groupid,@olimg,@adminid,@invisible,@action,@lastactivity,@lastposttime,@lastpostpmtime,@lastsearchtime,@lastupdatetime,@forumid,@forumname,@titleid,@title,@verifycode);SELECT SCOPE_IDENTITY()", prams);
				return 1;
			}


			return 0;
			//return (int)DbHelper.ExecuteDataset(CommandType.Text, "SELECT [olid] FROM ["+BaseConfigFactory.GetTablePrefix+"online] WHERE [userid]=" + __onlineuserinfo.Userid.ToString()).Tables[0].Rows[0][0];

		}

		public DataTable GetUserInfo(int userid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userid);
			string sql = "select Top 1 * from [" + BaseConfigs.GetTablePrefix + "users]  where [uid]=@uid";
			return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
		}

		public DataTable GetUserInfo(string username, string password)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar, 20, username),
									  DbHelper.MakeInParam("@password", (DbType)SqlDbType.Char, 32, password)
								  };
			string sql = "select Top 1 * from [" + BaseConfigs.GetTablePrefix + "users]  where [username]=@username And [password]=@password";

			return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
		}

		public void UpdateUserSpaceId(int userid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
			string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [spaceid]=ABS([spaceid]) WHERE [uid]=@userid";
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
		}

		public int GetUserIdByRewriteName(string rewritename)
		{
			IDataParameter parm = DbHelper.MakeInParam("@rewritename", (DbType)SqlDbType.Char, 100, rewritename);
			string sql = string.Format("SELECT [userid] FROM [{0}spaceconfigs] WHERE [rewritename]=@rewritename", BaseConfigs.GetTablePrefix);
			return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parm), -1);
		}

		public void UpdateUserPMSetting(UserInfo user)
		{
			IDataParameter[] parms = {
									  DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, user.Uid),
									  DbHelper.MakeInParam("@pmsound", (DbType)SqlDbType.Int, 4, user.Pmsound),
									  DbHelper.MakeInParam("@newsletter", (DbType)SqlDbType.Int, 4, (int)user.Newsletter)
								  };
			string sql = string.Format(@"UPDATE [{0}users] SET [pmsound]=@pmsound, [newsletter]=@newsletter WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);

			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
		}

		public void ClearUserSpace(int uid)
		{
			IDataParameter parm = DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid);
			string sql = string.Format("UPDATE [{0}users] SET [spaceid]=0 WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);
			DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
		}


		public IDataReader GetUserInfoByName(string username)
		{
			//IDataParameter parm =DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar, 20, username);
			return DbHelper.ExecuteReader(CommandType.Text, "SELECT [uid], [username] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [username] like '%" + RegEsc(username) + "%'");
		}


		public DataTable UserList(int pagesize, int currentpage, string condition)
		{
			#region 获得用户列表

			int pagetop = (currentpage - 1) * pagesize;

			if (currentpage == 1)
			{
				return DbHelper.ExecuteDataset("SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "users].[uid], [" + BaseConfigs.GetTablePrefix + "users].[username],[" + BaseConfigs.GetTablePrefix + "users].[nickname], [" + BaseConfigs.GetTablePrefix + "users].[joindate], [" + BaseConfigs.GetTablePrefix + "users].[credits], [" + BaseConfigs.GetTablePrefix + "users].[posts], [" + BaseConfigs.GetTablePrefix + "users].[lastactivity], [" + BaseConfigs.GetTablePrefix + "users].[email],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[accessmasks], [" + BaseConfigs.GetTablePrefix + "userfields].[location],[" + BaseConfigs.GetTablePrefix + "usergroups].[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "users] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "userfields].[uid] = [" + BaseConfigs.GetTablePrefix + "users].[uid]  LEFT JOIN [" + BaseConfigs.GetTablePrefix + "usergroups] ON [" + BaseConfigs.GetTablePrefix + "usergroups].[groupid]=[" + BaseConfigs.GetTablePrefix + "users].[groupid] WHERE " + condition + " ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] DESC").Tables[0];
			}
			else
			{
				string sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "users].[uid], [" + BaseConfigs.GetTablePrefix + "users].[username],[" + BaseConfigs.GetTablePrefix + "users].[nickname], [" + BaseConfigs.GetTablePrefix + "users].[joindate], [" + BaseConfigs.GetTablePrefix + "users].[credits], [" + BaseConfigs.GetTablePrefix + "users].[posts], [" + BaseConfigs.GetTablePrefix + "users].[lastactivity], [" + BaseConfigs.GetTablePrefix + "users].[email],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[accessmasks], [" + BaseConfigs.GetTablePrefix + "userfields].[location],[" + BaseConfigs.GetTablePrefix + "usergroups].[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "users],[" + BaseConfigs.GetTablePrefix + "userfields],[" + BaseConfigs.GetTablePrefix + "usergroups]  WHERE [" + BaseConfigs.GetTablePrefix + "userfields].[uid] = [" + BaseConfigs.GetTablePrefix + "users].[uid] AND  [" + BaseConfigs.GetTablePrefix + "usergroups].[groupid]=[" + BaseConfigs.GetTablePrefix + "users].[groupid] AND [" + BaseConfigs.GetTablePrefix + "users].[uid] < (SELECT min([uid])  FROM (SELECT TOP " + pagetop + " [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE " + condition + " ORDER BY [uid] DESC) AS tblTmp ) AND " + condition + " ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] DESC";
				return DbHelper.ExecuteDataset(sqlstring).Tables[0];
			}

			#endregion
		}

		#endregion

		#region WebSiteManage

		private IDataParameter[] GetParms(string startdate, string enddate)
		{
			IDataParameter[] parms = new IDataParameter[2];
			if (startdate != "")
			{
				parms[0] = DbHelper.MakeInParam("@startdate", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(startdate));
			}
			if (enddate != "")
			{
				parms[1] = DbHelper.MakeInParam("@enddate", (DbType)SqlDbType.DateTime, 8, DateTime.Parse(enddate).AddDays(1));
			}
			return parms;
		}

		public DataTable GetTopicListByCondition(int forumid, string posterlist, string keylist, string startdate, string enddate, int pageSize, int currentPage)
		{
			string sql = "";
			string condition = GetCondition(forumid, posterlist, keylist, startdate, enddate);

			IDataParameter[] parms = GetParms(startdate, enddate);

			int pageTop = (currentPage - 1)*pageSize;
			if (currentPage == 1)
			{
				sql =
					string.Format(
					"SELECT TOP {0} t.*,f.[name] FROM [{1}topics] t LEFT JOIN [{1}forums] f ON t.fid=f.fid LEFT JOIN [{1}forumfields] ff ON f.[fid]=ff.[fid] AND (ff.[viewperm] IS NULL OR CONVERT(NVARCHAR(1000),ff.[viewperm])='' OR CHARINDEX(',7,',','+CONVERT(NVARCHAR(1000),ff.[viewperm])+',')<>0) WHERE [closed]<>1 AND [status]=1 AND [password]='' {2} ORDER BY [tid] DESC",
					pageSize, BaseConfigs.GetTablePrefix, condition);
			}
			else
			{
				sql =
					string.Format(
					"SELECT TOP {0} t.*,f.[name] FROM [{1}topics] t LEFT JOIN [{1}forums] f ON t.fid=f.fid LEFT JOIN [{1}forumfields] ff ON f.[fid]=ff.[fid] AND (ff.[viewperm] IS NULL OR CONVERT(NVARCHAR(1000),ff.[viewperm])='' OR CHARINDEX(',7,',','+CONVERT(NVARCHAR(1000),ff.[viewperm])+',')<>0) WHERE [closed]<>1 AND [status]=1 AND [password]='' "
					+ "AND [tid]<(SELECT MIN([tid]) FROM (SELECT TOP {2} [tid] FROM [{1}topics] t LEFT JOIN [{1}forums] f ON t.fid=f.fid LEFT JOIN [{1}forumfields] ff ON f.[fid]=ff.[fid] WHERE [closed]<>1 AND [status]=1 AND [password]='' {3} ORDER BY [tid] DESC) AS tblTmp){3} ORDER BY [tid] DESC",
					pageSize, BaseConfigs.GetTablePrefix, pageTop, condition);
			}
			return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
		}

		private static string GetCondition(int forumid, string posterlist, string keylist, string startdate, string enddate)
		{
			string condition = "";
			if (forumid != 0)
			{
				condition += " AND t.[fid]=" + forumid;
			}
			if (posterlist != "")
			{
				string[] poster = posterlist.Split(',');
				condition += " AND [poster] in (";
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
			{
				//condition += " AND [postdatetime]>='" + startdate + " 00:00:00'";
				condition += " AND [postdatetime]>=@startdate";
			}
			if (enddate != "")
			{
				//condition += " AND [postdatetime]<='" + enddate + " 23:59:59'";
				condition += " AND [postdatetime]<=@enddate";
			}
			return condition;
		}

		public int GetTopicListCountByCondition(int forumid, string posterlist, string keylist, string startdate, string enddate)
		{
			string sql = string.Format("SELECT COUNT(1) FROM [{0}topics] t LEFT JOIN [{0}forums] f ON t.fid=f.fid LEFT JOIN [{0}forumfields] ff ON f.[fid]=ff.[fid] AND (ff.[viewperm] IS NULL OR CONVERT(NVARCHAR(1000),ff.[viewperm])='' OR CHARINDEX(',7,',','+CONVERT(NVARCHAR(1000),ff.[viewperm])+',')<>0) WHERE [closed]<>1 AND [status]=1 AND [password]=''", BaseConfigs.GetTablePrefix);
			string condition = GetCondition(forumid, posterlist, keylist, startdate, enddate);
			IDataParameter[] parms = GetParms(startdate, enddate);

			if (condition != "")
				sql += condition;
			return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql, parms).ToString());
		}

		public DataTable GetTopicListByTidlist(string posttableid, string tidlist)
		{
			string sql =
				string.Format(
				"SELECT p.*,t.[closed] FROM [{0}posts{1}] p LEFT JOIN [{0}topics] t ON p.[tid]=t.[tid] WHERE [layer]=0 AND [closed]<>1 and p.[tid] IN ({2}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),p.[tid]),'{2}')",
				BaseConfigs.GetTablePrefix, posttableid, tidlist);
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}



		#region 聚合相册相关函数

		public DataTable GetAlbumListByCondition(string username, string title, string description, string startdate, string enddate, int pageSize, int currentPage,bool isshowall)
		{
			string sql = "";
			string condition = GetAlbumListCondition(username, title, description, startdate, enddate);
			IDataParameter[] parms = GetParms(startdate, enddate);
			int pageTop = (currentPage - 1)*pageSize;
            
			string strisshowall = "";
			if (isshowall)
			{
				strisshowall = " 1=1";
			}
			else
			{
				strisshowall = " [type] = 0 AND  [imgcount] > 0 ";
			}
			if (currentPage == 1)
			{                
				sql =
					string.Format("SELECT TOP {0} *  FROM [{1}albums] WHERE {2} {3} ORDER BY [albumid] DESC", pageSize,
					BaseConfigs.GetTablePrefix,strisshowall, condition);
			}
			else
			{                
				sql =
					string.Format(
					"SELECT TOP {0} * FROM [{1}albums] WHERE [albumid]<(SELECT MIN([albumid]) FROM (SELECT TOP {2} [albumid] FROM [{1}albums] WHERE  {3} {4} ORDER BY [albumid] DESC) AS tblTmp) AND {3} {4} ORDER BY [albumid] DESC",
					pageSize, BaseConfigs.GetTablePrefix, pageTop,strisshowall, condition);
			}
			return DbHelper.ExecuteDataset(CommandType.Text, sql,parms).Tables[0];
		}

		private static string GetAlbumListCondition(string usernamelist, string titlelist, string descriptionlist, string startdate, string enddate)
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
			{
				//condition += " AND [createdatetime]>='" + startdate + " 00:00:00'";
				condition += " AND [createdatetime]>=@startdate";
			}
			if (enddate != "")
			{
				//condition += " AND [createdatetime]<='" + enddate + " 23:59:59'";
				condition += " AND [createdatetime]<=@enddate";
			}
			return condition;
		}

		public int GetAlbumListCountByCondition(string username, string title, string description, string startdate, string enddate,bool isshowall)
		{
			string sql = string.Format("SELECT COUNT(1) FROM [{0}albums] t", BaseConfigs.GetTablePrefix);
			if (isshowall)
			{
				sql += " WHERE 1=1";
			}
			else
			{
				sql += " WHERE [type] = 0 AND  [imgcount] > 0";
			}
          
			IDataParameter[] parms = GetParms(startdate, enddate);

			string condition = GetAlbumListCondition(username, title, description, startdate, enddate);
			if (condition != "")
				sql += condition;
			return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql, parms).ToString());
		}


		public DataTable GetAlbumLitByAlbumidList(string albumlist)
		{
			if (!Discuz.Common.Utils.IsNumericArray(albumlist.Split(',')))
			{
				return new DataTable();
			}
			string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "albums] WHERE [type] = 0 "
				+ "AND [albumid] IN (" + albumlist + ") ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[albumid]),'" + albumlist + "')";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		#endregion

		#region 聚合个人空间相关函数
		public int GetSpaceCountByCondition(string posterlist, string keylist, string startdate, string enddate)
		{
			string sql = "SELECT COUNT(1) FROM (SELECT s.*,u.[username] FROM [" + BaseConfigs.GetTablePrefix + "spaceconfigs] s LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] u ON s.[userid]=u.[uid]) AS tblTmp WHERE [status]=0";
			string condition = GetSpaceCondition(posterlist, keylist, startdate, enddate);
			IDataParameter[] parms = GetParms(startdate, enddate);
			if (condition != "")
				sql += condition;
			return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql,parms).ToString());
		}

		public DataTable GetSpaceByCondition(string posterlist, string keylist, string startdate, string enddate, int pageSize, int currentPage)
		{
			string sql = "";
			string condition = GetSpaceCondition(posterlist, keylist, startdate, enddate);
			IDataParameter[] parms = GetParms(startdate, enddate);
			int pageTop = (currentPage - 1) * pageSize;
			if (currentPage == 1)
			{

				sql = "SELECT TOP " + pageSize + " s.*,u.[username],f.[avatar],(SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "albums] WHERE [userid]=s.[userid]) albumcount "
					+ "FROM [" + BaseConfigs.GetTablePrefix + "spaceconfigs] s LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] f ON [userid]=f.[uid] "
					+ "LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] u ON u.[uid]=[userid] WHERE [status]=0" + condition + " ORDER BY s.[spaceid] DESC";
			}
			else
			{
				sql = "SELECT TOP " + pageSize + " s.*,u.[username],f.[avatar],(SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "albums] WHERE [userid]=s.[userid]) albumcount "
					+ "FROM [" + BaseConfigs.GetTablePrefix + "spaceconfigs] s LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] f ON [userid]=f.[uid] "
					+ "LEFT JOIN [" + BaseConfigs.GetTablePrefix + "users] u ON u.[uid]=[userid] WHERE [status]=0 AND s.[spaceid]<(SELECT MIN([spaceid]) FROM (SELECT TOP " + pageTop
					+ " [spaceid] FROM [" + BaseConfigs.GetTablePrefix + "spaceconfigs] WHERE [status]=0 " + condition + " ORDER BY [spaceid] DESC) AS tblTmp)" + condition + " ORDER BY s.[spaceid] DESC";
			}
			return DbHelper.ExecuteDataset(CommandType.Text, sql,parms).Tables[0];
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
			{
				//condition += " AND [createdatetime]>='" + startdate + " 00:00:00'";
				condition += " AND [createdatetime]>=@startdate";
			}
			if (enddate != "")
			{
				//condition += " AND [createdatetime]<='" + enddate + " 23:59:59'";
				condition += " AND [createdatetime]<=@enddate";
			}
			return condition;
		}

		public DataTable GetSpaceLitByTidlist(string spaceidlist)
		{
			if (!Discuz.Common.Utils.IsNumericArray(spaceidlist.Split(',')))
			{
				return new DataTable();
			}
			string sql = "SELECT s.*,f.[avatar],(SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "albums] WHERE userid=s.userid) albumcount "
				+ "FROM [" + BaseConfigs.GetTablePrefix + "spaceconfigs] s LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] f ON [userid]=f.[uid] "
				+ "WHERE ([spaceid] IN (" + spaceidlist + ")) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[spaceid]),'" + spaceidlist + "')";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}
		#endregion


		#region 前台聚合页相关函数

		public IDataReader GetAlbumListByCondition(int type, int focusphotocount, int vaildDays)
		{
			IDataParameter parm = DbHelper.MakeInParam("@vailddays", (DbType)SqlDbType.Int, 4, vaildDays);
			string sql = string.Format("SELECT TOP {0} * FROM [{1}albums] WHERE DATEDIFF(d,[createdatetime],getdate()) < @vailddays AND [imgcount]>0 AND [type]=0", focusphotocount, BaseConfigs.GetTablePrefix);

			switch (type)
			{
				case 0:
					sql += " ORDER BY [createdatetime] DESC";
					break;
				default:
					sql += " ORDER BY [createdatetime] DESC";
					break;
			}

			return DbHelper.ExecuteReader(CommandType.Text, sql, parm);
		}

		public DataTable GetWebSiteAggForumTopicList(string showtype, int topnumber)
		{
			DataTable topicList = new DataTable();
			switch (showtype)
			{
				default://按版块查
				{
					topicList = DbHelper.ExecuteDataset("SELECT f.[fid], f.[name], f.[lasttid] AS [tid], f.[lasttitle] AS [title] , f.[lastposterid] AS [posterid], f.[lastposter] AS [poster], f.[lastpost] AS [postdatetime], t.[views], t.[replies] FROM [" + BaseConfigs.GetTablePrefix + "forums] f LEFT JOIN [" + BaseConfigs.GetTablePrefix + "topics] t  ON f.[lasttid] = t.[tid] WHERE f.[status]=1 AND f.[layer]> 0 AND f.[fid] IN (SELECT ff.[fid] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] ff WHERE ff.[password] ='') AND t.[displayorder]>=0").Tables[0]; break;
				}
				case "1"://按最新主题查
				{
					topicList = DbHelper.ExecuteDataset("SELECT TOP " + topnumber + " t.[tid], t.[title], t.[postdatetime], t.[poster], t.[posterid], t.[fid], t.[views], t.[replies], f.[name] FROM [" + BaseConfigs.GetTablePrefix + "topics] t LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "forums] f ON t.[fid] = f.[fid] WHERE t.[displayorder]>=0 AND f.[status]=1 AND f.[layer]> 0 AND f.[fid] IN (SELECT ff.[fid] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] ff WHERE ff.[password] ='') ORDER BY t.[postdatetime] DESC").Tables[0]; break;
				}
				case "2"://按精华主题查
				{
					topicList = DbHelper.ExecuteDataset("SELECT TOP " + topnumber + " t.[tid], t.[title], t.[postdatetime], t.[poster], t.[posterid], t.[fid], t.[views], t.[replies], f.[name] FROM [" + BaseConfigs.GetTablePrefix + "topics] t LEFT OUTER JOIN [" + BaseConfigs.GetTablePrefix + "forums] f ON t.[fid] = f.[fid] WHERE t.[digest] >0 AND f.[status]=1 AND f.[layer]> 0 AND f.[fid] IN (SELECT ff.[fid] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] ff WHERE ff.[password] ='') ORDER BY t.[digest] DESC").Tables[0]; break;
				}
				case "3"://按版块查
				{
					topicList = DbHelper.ExecuteDataset("SELECT f.[fid], f.[name], f.[lasttid] AS [tid], f.[lasttitle] AS [title] , f.[lastposterid] AS [posterid], f.[lastposter] AS [poster], f.[lastpost] AS [postdatetime], t.[views], t.[replies] FROM [" + BaseConfigs.GetTablePrefix + "forums] f LEFT JOIN [" + BaseConfigs.GetTablePrefix + "topics] t  ON f.[lasttid] = t.[tid] WHERE f.[status]=1 AND f.[layer]> 0 AND f.[fid] IN (SELECT ff.[fid] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] ff WHERE ff.[password] ='') AND t.[displayorder]>=0").Tables[0]; break;
				}
			}
			return topicList;
		}


		public DataTable GetWebSiteAggHotForumList()
		{
			return DbHelper.ExecuteDataset("SELECT [fid], [name] FROM [" + BaseConfigs.GetTablePrefix + "forums] WHERE [status]=1 AND [layer]> 0 AND [fid] IN (SELECT [fid] FROM [" + BaseConfigs.GetTablePrefix + "forumfields] WHERE [password]='') ORDER BY [topics] DESC, [posts] DESC, [todayposts] DESC").Tables[0];
		}


		public DataTable GetWebSiteAggForumNewTopicList()
		{
			return DbHelper.ExecuteDataset("SELECT TOP 10 [tid], [title] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [displayorder]>=0 AND [closed]<>1 ORDER BY [tid] DESC ").Tables[0];
		}

		public DataTable GetWebSiteAggForumHotTopicList()
		{
			return DbHelper.ExecuteDataset("SELECT TOP 10 [tid], [title] FROM [" + BaseConfigs.GetTablePrefix + "topics]  WHERE [displayorder]>=0 AND [closed]<>1 ORDER BY [replies] DESC").Tables[0];
		}

		public DataTable GetWebSiteAggSpacePostList(int topnumber)
		{
			return DbHelper.ExecuteDataset(" SELECT TOP " + topnumber + " [postid], [author], [uid], [postdatetime], [title], [commentcount], [views] FROM [" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE [poststatus] = 1").Tables[0];
		}

		public DataTable GetWebSiteAggRecentUpdateSpaceList(int topnumber)
		{
			return DbHelper.ExecuteDataset(" SELECT TOP " + topnumber + " [spaceid], [userid], [spacetitle], [postcount], [commentcount], [visitedtimes] FROM [" + BaseConfigs.GetTablePrefix + "spaceconfigs] WHERE [status] = 0 AND [postcount]>0 ORDER BY [updatedatetime] DESC").Tables[0];
		}

		public DataTable GetWebSiteAggTopSpaceList(string orderby,int topnumber)
		{
			return DbHelper.ExecuteDataset(" SELECT TOP " + topnumber + " s.*,u.[avatar] FROM [" + BaseConfigs.GetTablePrefix + "spaceconfigs] s LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] u ON s.[userid] = u.[uid]  WHERE s.[status] = 0 ORDER BY s.[" + orderby + "] DESC").Tables[0];
		}

		public DataTable GetWebSiteAggTopSpacePostList(string orderby, int topnumber)
		{
			return DbHelper.ExecuteDataset(" SELECT TOP " + topnumber + " [postid],[title],[author],[uid],[postdatetime],[commentcount],[views] FROM [" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE [poststatus] = 1 ORDER BY [" + orderby + "] DESC").Tables[0];
		}


		public DataTable GetWebSiteAggSpacePostsList(int pageSize, int currentPage)
		{
			DataTable dt = new DataTable();

			int pageTop = (currentPage - 1) * pageSize;
			if (currentPage == 1)
			{
				string sql = "SELECT TOP " + pageSize + " * FROM "
					+ "[" + BaseConfigs.GetTablePrefix + "spaceposts] ORDER BY [postid] DESC";
				dt = DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
			}
			else
			{
				string sql = "SELECT TOP " + pageSize + " * FROM "
					+ "[" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE [postid] < (SELECT min([postid])  FROM "
					+ "(SELECT TOP " + pageTop + " [postid] FROM [" + BaseConfigs.GetTablePrefix + "spaceposts] "
					+ "ORDER BY [postid] DESC) AS tblTmp ) ORDER BY [postid] DESC";
				dt = DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
			}
			return dt;
		}

		public int GetWebSiteAggSpacePostsCount()
		{
			try
			{
				return (int)DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([postid]) FROM [" + BaseConfigs.GetTablePrefix + "spaceposts]");
			}
			catch
			{
				return 0;
			}
		}
		#endregion

		#region 照片操作相关函数

		public int GetPhotoCountByCondition(string photousernamelist, string keylist, string startdate, string enddate)
		{
			string sql = "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "photos] p LEFT JOIN [" + BaseConfigs.GetTablePrefix + "albums] a ON p.[albumid]=a.[albumid] WHERE a.[type]=0";
			IDataParameter[] parms = GetParms(startdate, enddate);
			string condition = GetPhotoCondition(photousernamelist, keylist, startdate, enddate);
			if (condition != "")
				sql += condition;
			return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql, parms).ToString());
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
			{
				condition += " AND p.[postdate]>=@startdate";
			}
			if (enddate != "")
			{
				condition += " AND p.[postdate]<=@enddate";
			}
			return condition;
		}

		public DataTable GetPhotoByCondition(string photousernamelist, string keylist, string startdate, string enddate, int pageSize, int currentPage)
		{
			string sql = "";
			IDataParameter[] parms = GetParms(startdate, enddate);
			string condition = GetPhotoCondition(photousernamelist, keylist, startdate, enddate);
			int pageTop = (currentPage - 1) * pageSize;
			if (currentPage == 1)
			{

				sql = "SELECT TOP " + pageSize + " p.* FROM [" + BaseConfigs.GetTablePrefix + "photos] p LEFT JOIN [" + BaseConfigs.GetTablePrefix + "albums] a ON p.[albumid]=a.[albumid] WHERE a.[type]=0" + condition + " ORDER BY p.[photoid] DESC";
			}
			else
			{
				sql = "SELECT TOP " + pageSize + " p.* FROM [" + BaseConfigs.GetTablePrefix + "photos] p LEFT JOIN [" + BaseConfigs.GetTablePrefix + "albums] a ON p.[albumid]=a.[albumid] WHERE a.[type]=0 AND p.[photoid]<(SELECT MIN([photoid]) FROM (SELECT TOP " + pageTop
					+ " p.[photoid] FROM [" + BaseConfigs.GetTablePrefix + "photos] p LEFT JOIN [" + BaseConfigs.GetTablePrefix + "albums] a ON p.[albumid]=a.[albumid] WHERE a.[type]=0 " + condition + " ORDER BY p.[photoid] DESC) AS tblTmp)" + condition + " ORDER BY p.[photoid] DESC";
			}
			return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
		}

		#endregion

		public DataTable GetPhotoByIdOrderList(string idlist)
		{
			if (!Common.Utils.IsNumericArray(idlist.Split(',')))
			{
				return new DataTable();
			}
			string sql = "SELECT [photoid],[title] FROM [" + BaseConfigs.GetTablePrefix + "photos] WHERE (photoid IN (" + idlist + ")) "
				+ "ORDER BY CHARINDEX(CONVERT(VARCHAR(8),photoid),'" + idlist + "')";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public DataTable GetAlbumByIdOrderList(string idlist)
		{
			if (!Common.Utils.IsNumericArray(idlist.Split(',')))
			{
				return new DataTable();
			}
			string sql = "SELECT [albumid],[title] FROM [" + BaseConfigs.GetTablePrefix + "albums] WHERE ([albumid] IN (" + idlist + ")) "
				+ "ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[albumid]),'" + idlist + "')";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}


		public DataTable GetWebSiteAggSpaceTopComments(int topnumber)
		{
			return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP " + topnumber + " [postid],[content],[posttitle],[author],[uid] FROM [" + BaseConfigs.GetTablePrefix + "spacecomments] ORDER BY [commentid] DESC").Tables[0];
		}

		public string[] GetSpaceLastPostInfo(int userid)
		{
			IDataParameter pram = DbHelper.MakeInParam("@uid", (DbType) SqlDbType.Int, 4, userid);
			string sql =
				string.Format(
				"SELECT TOP 1 [postid],[title] FROM [{0}spaceposts] WHERE [uid]=@uid ORDER BY [postdatetime] DESC",
				BaseConfigs.GetTablePrefix);

			DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sql, pram).Tables[0];
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
			string sql = "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE 1=1";
			string condition = GetSpacePostCondition(posterlist, keylist, startdate, enddate);

			IDataParameter[] parms = GetParms(startdate, enddate);

			if (condition != "")
				sql += condition;
			return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql, parms).ToString());
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
			{
				//condition += " AND [postdatetime]>='" + startdate + " 00:00:00'";
				condition += " AND [postdatetime]>=@startdate";
			}
			if (enddate != "")
			{
				//condition += " AND [postdatetime]<='" + enddate + " 23:59:59'";
				condition += " AND [postdatetime]<=@enddate";
			}
			return condition;
		}

		public DataTable GetSpacePostByCondition(string posterlist, string keylist, string startdate, string enddate, int pageSize, int currentPage)
		{
			string sql = "";
			IDataParameter[] parms = GetParms(startdate, enddate);

			//IDataParameter[] prams = {
			//                               DbHelper.MakeInParam("@startdate",(DbType)SqlDbType.DateTime, 8, ),
			//                               DbHelper.MakeInParam("@enddate",(DbType)SqlDbType.DateTime, 8,endtime)
			//                           };

			string condition = GetSpacePostCondition(posterlist, keylist, startdate, enddate);
			int pageTop = (currentPage - 1) * pageSize;
			if (currentPage == 1)
			{

				sql = "SELECT TOP " + pageSize + " * FROM [" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE 1=1" + condition + " ORDER BY [postid] DESC";
			}
			else
			{
				sql = "SELECT TOP " + pageSize + " * FROM [" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE [postid]<(SELECT MIN([postid]) FROM (SELECT TOP " + pageTop
					+ " [postid] FROM [" + BaseConfigs.GetTablePrefix + "spaceposts] WHERE 1=1 " + condition + " ORDER BY [postid] DESC) AS tblTmp)" + condition + " ORDER BY [postid] DESC";
			}
			return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
		}

		public DataTable GetSpacepostLitByTidlist(string postidlist)
		{
			if (!Common.Utils.IsNumericArray(postidlist.Split(',')))
			{
				return new DataTable();
			}
			string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "spaceposts]"
				+ "WHERE ([postid] IN (" + postidlist + ")) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[postid]),'" + postidlist + "')";
			return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
		}

		public int GetUidByAlbumid(int albumid)
		{
			IDataParameter pram = DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4, albumid);
			string sql = "SELECT [userid] FROM [" + BaseConfigs.GetTablePrefix + "albums] WHERE [albumid]=@albumid";
			return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, sql,pram).ToString());
		}

		#endregion

	}
}
#endif