using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.IO;

using Discuz.Config;
using Discuz.Data;
using Discuz.Entity;
using Discuz.Common;

namespace Discuz.Album.Data
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


        #region	相册 操作类
        public void AddAlbumCategory(AlbumCategoryInfo aci)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.NChar, 50, aci.Title),
                                        DbHelper.MakeInParam("@description", (DbType)SqlDbType.NChar, 300, aci.Description),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, aci.Displayorder)
                                };

            string commandText = string.Format(@"INSERT INTO [{0}albumcategories]([title], [description], [albumcount], [displayorder]) VALUES(@title, @description, 0, @displayorder)", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public void UpdateAlbumCategory(AlbumCategoryInfo aci)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@albumcateid", (DbType)SqlDbType.Int, 4, aci.Albumcateid),
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.NChar, 50, aci.Title),
                                        DbHelper.MakeInParam("@description", (DbType)SqlDbType.NChar, 300, aci.Description),
                                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, aci.Displayorder)
                                };

            string commandText = string.Format(@"UPDATE [{0}albumcategories] SET [title]=@title, [description]=@description, [displayorder]=@displayorder WHERE [albumcateid]=@albumcateid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }


        public void DeleteAlbumCategory(int albumcateid)
        {
            DbParameter parm = DbHelper.MakeInParam("@albumcateid", (DbType)SqlDbType.Int, 4, albumcateid);

            string commandText = string.Format(@"DELETE FROM [{0}albumcategories] WHERE [albumcateid]=@albumcateid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parm);
        }

        public int GetSpaceAlbumsCount(int userid)
        {
            try
            {
                return (int)DbHelper.ExecuteScalar(CommandType.Text, 
                                                   "SELECT COUNT([albumid]) FROM [" + BaseConfigs.GetTablePrefix + "albums] WHERE [userid]=@userid", 
                                                   DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid));
            }
            catch
            {
                return 0;
            }
        }

        public DataTable SpaceAlbumsList(int pageSize, int currentPage, int userid)
        {
            DbParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);
            int pageTop = (currentPage - 1) * pageSize;
            string commandText = "";
            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} * FROM [{1}albums] WHERE [userid]=@userid ORDER BY [albumid] DESC", pageSize, BaseConfigs.GetTablePrefix);
            else
                commandText = string.Format("SELECT TOP {0} * FROM [{1}albums] WHERE [albumid] < (SELECT min([albumid])  FROM "
                                            + "(SELECT TOP {2} [albumid] FROM [{1}albums] WHERE [userid]=@userid ORDER BY [albumid] DESC) AS tblTmp ) AND [userid]=@userid ORDER BY [albumid] DESC", 
                                            pageSize, BaseConfigs.GetTablePrefix, pageTop);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parm).Tables[0];
        }

        public IDataReader SpaceAlbumsList(int userid, int albumcategoryid, int pageSize, int currentPage)
        {
            DbParameter[] parms = {
										DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, userid),
										DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int, 4, pageSize),
										DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int, 4, currentPage),
                                        DbHelper.MakeInParam("@albumcateid", (DbType)SqlDbType.Int, 4, albumcategoryid)
								   };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getalbumlist", BaseConfigs.GetTablePrefix), parms);
        }

        public int SpaceAlbumsListCount(int userid, int albumcategoryid)
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}albums] WHERE [imgcount]>0 ", BaseConfigs.GetTablePrefix);
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int,  4, userid),
                                       DbHelper.MakeInParam("@albumcateid", (DbType)SqlDbType.Int, 4, albumcategoryid)
								   };
            if (userid > 0)
                commandText += " AND [userid]=@userid";
            if (albumcategoryid != 0)
                commandText += " AND [albumcateid]=@albumcateid";

            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms), 0);
        }

        public IDataReader GetSpaceAlbumById(int albumId)
        {
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "albums] WHERE [albumid]=" + albumId);
        }

        public DataTable GetSpaceAlbumByUserId(int userid)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "albums] WHERE [userid]=" + userid).Tables[0];
        }


        public bool AddSpaceAlbum(AlbumInfo spaceAlbum)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4,spaceAlbum.Userid),
					DbHelper.MakeInParam("@albumcateid", (DbType)SqlDbType.Int, 4,spaceAlbum.Albumcateid),
					DbHelper.MakeInParam("@title", (DbType)SqlDbType.NChar, 50,spaceAlbum.Title),
					DbHelper.MakeInParam("@description", (DbType)SqlDbType.NChar, 200,spaceAlbum.Description),
					DbHelper.MakeInParam("@password", (DbType)SqlDbType.NChar, 50,spaceAlbum.Password),
					DbHelper.MakeInParam("@type", (DbType)SqlDbType.Int, 8,spaceAlbum.Type),
                    DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar, 20, spaceAlbum.Username)
				};
            string commandText = String.Format("INSERT INTO [{0}albums] ([userid], [username], [albumcateid], [title], [description], [password], [type]) VALUES ( @userid, @username, @albumcateid, @title, @description, @password, @type)", BaseConfigs.GetTablePrefix);
            //向关联表中插入相关数据
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;
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

        public void UpdateAlbumViews(int albumid)
        {
            DbParameter parm = DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4, albumid);
            string commandText = string.Format("UPDATE [{0}albums] SET [views]=[views]+1 WHERE [albumid]=@albumid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parm);
        }

        public bool DeleteSpaceAlbum(int albumId, int userid)
        {
            //删除照片及文件
            string commandText = string.Format("DELETE FROM [{0}albums] WHERE [albumid]={1} AND [userid]={2}", BaseConfigs.GetTablePrefix, albumId, userid);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
            return true;
        }

        public Discuz.Common.Generic.List<AlbumCategoryInfo> GetAlbumCategory()
        {
            string commandText = string.Format("SELECT * FROM [{0}albumcategories] ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);

            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, commandText);
            Discuz.Common.Generic.List<AlbumCategoryInfo> acic = new Discuz.Common.Generic.List<AlbumCategoryInfo>();
            while (reader.Read())
            {
                AlbumCategoryInfo aci = new AlbumCategoryInfo();
                aci.Albumcateid = TypeConverter.ObjectToInt(reader["albumcateid"], 0);
                aci.Albumcount = TypeConverter.ObjectToInt(reader["albumcount"], 0);
                aci.Description = reader["description"].ToString();
                aci.Displayorder = TypeConverter.ObjectToInt(reader["displayorder"], 0);
                aci.Title = reader["title"].ToString();
                acic.Add(aci);
            }
            reader.Close();
            return acic;
        }

        public string GetAlbumCategorySql()
        {
            return string.Format("SELECT * FROM [{0}albumcategories] ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
        }
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
            string commandText = string.Format("SELECT TOP {0} [p].* FROM [{1}photos] [p],[{1}albums] [a] WHERE [a].[albumid] = [p].[albumid] AND [a].[type]=0 AND [p].[userid]=@userid", count, BaseConfigs.GetTablePrefix);

            if (albumid > 0)
                commandText += " AND [p].[albumid]=@albumid";

            commandText += " ORDER BY [p].[postdate] DESC";

            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
                                        DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4, albumid)
                                    };
            return DbHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得图片排行图集合
        /// </summary>
        /// <param name="type">排行方式，0浏览量，1评论数,2上传时间，3收藏数</param>
        /// <returns></returns>
        public IDataReader GetPhotoRankList(int type, int photocount)
        {
            string commandText = string.Format("SELECT TOP {0} [p].* FROM [{1}photos] [p],[{1}albums] [a] WHERE [a].[albumid] = [p].[albumid] AND [a].[type]=0",
                                        photocount, BaseConfigs.GetTablePrefix);

            switch (type)
            {
                case 0:
                    commandText += " ORDER BY [p].[views] DESC";
                    break;
                case 1:
                    commandText += " ORDER BY [p].[comments] DESC";
                    break;
                case 2:
                    commandText += " ORDER BY [p].[postdate] DESC";
                    break;
                case 3:
                    commandText = string.Format(@"SELECT * FROM [{0}albums] WHERE albumid IN (SELECT TOP {1} [tid] 
		                                                                FROM [{0}favorites]
		                                                                WHERE  [typeid]=1 AND [tid] in (SELECT [albumid] 
                                                                                                        FROM [{0}albums] 
                                                                                                        WHERE [type]=0) 
		                                                                GROUP BY [tid] 
		                                                                ORDER BY COUNT([tid]) DESC)", BaseConfigs.GetTablePrefix, photocount);
                    break;
                default:
                    commandText += " ORDER BY [p].[views] DESC";
                    break;
            }

            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public IDataReader GetFocusPhotoList(int type, int focusphotocount, int validDays)
        {
            return GetFocusPhotoList(type, focusphotocount, validDays, -1);
        }


        public IDataReader GetFocusPhotoList(int type, int focusphotocount, int validDays, int uid)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@validDays", (DbType)SqlDbType.Int, 4, validDays),
                                    DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
                               };
            string commandText = string.Format("SELECT TOP {0} [p].* FROM [{1}photos] [p],[{1}albums] [a] WHERE DATEDIFF(d, [postdate], getdate()) < @validDays AND [a].[albumid] = [p].[albumid] AND [a].[type]=0{2}",
                                        focusphotocount, BaseConfigs.GetTablePrefix, uid > 1 ? " AND [p].[userid] =@uid" : string.Empty);
            switch (type)
            {
                case 0:
                    commandText += " ORDER BY [p].[views] DESC";
                    break;
                case 1:
                    commandText += " ORDER BY [p].[comments] DESC";
                    break;
                case 2:
                    commandText += " ORDER BY [p].[postdate] DESC";
                    break;
                default:
                    commandText += " ORDER BY [p].[views] DESC";
                    break;
            }
            return DbHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }


        public IDataReader GetRecommendPhotoList(string idlist)
        {
            if (!Utils.IsNumericList(idlist))
                return null;

            string commandText = string.Format("SELECT [p].* FROM [{0}photos] [p],[{0}albums] [a] WHERE [p].[albumid] = [a].[albumid] AND [a].[type]=0 AND [p].[photoid] IN ({1}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[p].[photoid]),'{1}')", BaseConfigs.GetTablePrefix, idlist);

            return DbHelper.ExecuteReader(CommandType.Text, commandText);
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

        /// <summary>
        /// 更新图片信息(仅更新 标题、描述、评论设置和标签设置4项)
        /// </summary>
        /// <param name="photo"></param>
        public void UpdatePhotoInfo(PhotoInfo photo)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@photoid", (DbType)SqlDbType.Int, 4, photo.Photoid),
                                        DbHelper.MakeInParam("@title", (DbType)SqlDbType.NChar, 20, photo.Title),
                                        DbHelper.MakeInParam("@description", (DbType)SqlDbType.NChar, 200, photo.Description),
                                        DbHelper.MakeInParam("@commentstatus", (DbType)SqlDbType.TinyInt, 1, (byte)photo.Commentstatus),
                                        DbHelper.MakeInParam("@tagstatus", (DbType)SqlDbType.TinyInt, 1, (byte)photo.Tagstatus)
                                    };
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}photos] SET [title]=@title, [description]=@description, [commentstatus]=@commentstatus, [tagstatus]=@tagstatus WHERE [photoid]=@photoid", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 通过相册ID得到相册中所有图片的信息
        /// </summary>
        /// <param name="albumid">相册ID</param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public DataTable GetSpacePhotoByAlbumID(int albumid)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4,albumid)
				};
            //向关联表中插入相关数据
            return DbHelper.ExecuteDataset(CommandType.Text, String.Format("SELECT * FROM [{0}photos] WHERE [albumid] = @albumid", BaseConfigs.GetTablePrefix), parms).Tables[0];
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
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@photoid", (DbType)SqlDbType.Int, 4,photoid),
                    DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4, albumid)
				};
            string commandText;

            switch (mode)
            {
                case 1:
                    commandText = "SELECT TOP 1 * FROM [{0}photos] WHERE [albumid] = @albumid AND [photoid]<@photoid ORDER BY [photoid] DESC";
                    break;
                case 2:
                    commandText = "SELECT TOP 1 * FROM [{0}photos] WHERE [albumid] = @albumid AND [photoid]>@photoid ORDER BY [photoid] ASC";
                    break;
                default:
                    commandText = "SELECT * FROM [{0}photos] WHERE [photoid] = @photoid";
                    break;
            }
            //向关联表中插入相关数据
            return DbHelper.ExecuteReader(CommandType.Text, string.Format(commandText, BaseConfigs.GetTablePrefix), parms);
        }

        public void UpdatePhotoViews(int photoid)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, 
                                     string.Format("UPDATE [{0}photos] SET [views]=[views]+1 WHERE [photoid]=@photoid", BaseConfigs.GetTablePrefix), 
                                     DbHelper.MakeInParam("@photoid", (DbType)SqlDbType.Int, 4, photoid));
        }

        public void UpdatePhotoComments(int photoid, int count)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@photoid", (DbType)SqlDbType.Int, 4, photoid),
                DbHelper.MakeInParam("@count", (DbType)SqlDbType.Int, 4, count),
            };
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}photos] SET [comments]=[comments]+@count WHERE [photoid]=@photoid", BaseConfigs.GetTablePrefix), parms);
        }

        public int GetSpacePhotosCount(int albumid)
        {
            try
            {
                return (int)DbHelper.ExecuteScalar(CommandType.Text, 
                                                   string.Format("SELECT COUNT([photoid]) FROM [{0}photos] WHERE [albumid]=@albumid", BaseConfigs.GetTablePrefix),
                                                   DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4, albumid));
            }
            catch
            {
                return 0;
            }
        }

        public DataTable SpacePhotosList(int pageSize, int currentPage, int userid, int albumid)
        {
            DbParameter[] parms = 
				{
					DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4,userid),
					DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4,albumid)
				};
            int pageTop = (currentPage - 1) * pageSize;
            string commandText = "";
            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} * FROM [{1}photos] WHERE [userid]=@userid AND [albumid]=@albumid ORDER BY [photoid] ASC", pageSize, BaseConfigs.GetTablePrefix);
            else
                commandText = string.Format("SELECT TOP {0} * FROM [{1}photos] WHERE [photoid] > (SELECT MAX([photoid])  FROM (SELECT TOP {2} [photoid] FROM [{1}photos] WHERE "
                                            + "[userid]=@userid AND [albumid]=@albumid ORDER BY [photoid] ASC) AS tblTmp ) AND [userid]=@userid AND [albumid]=@albumid ORDER BY [photoid] ASC", 
                                            pageSize, BaseConfigs.GetTablePrefix, pageTop);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        public DataTable SpacePhotosList(int albumid)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, 
                                           string.Format("SELECT * FROM [{0}photos] WHERE [albumid]=@albumid ORDER BY [photoid] ASC", BaseConfigs.GetTablePrefix),
                                           DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4, albumid)).Tables[0];
        }

        public bool DeleteSpacePhotoByIDList(string photoidlist, int albumid, int userid)
        {
            if (photoidlist == "")
                return false;
            if (!Utils.IsNumericList(photoidlist))
                return false;

            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT [filename],[isattachment] FROM [" + BaseConfigs.GetTablePrefix + "photos] WHERE [photoid] IN( " + photoidlist + " ) AND [userid]=" + userid, null);
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

            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}photos] WHERE [photoid] IN ({1}) AND [userid]={2}",BaseConfigs.GetTablePrefix, photoidlist, userid));
            return true;
        }

        public int ChangeAlbum(int targetAlbumId, string photoIdList, int userid)
        {
            if (!Utils.IsNumericList(photoIdList))
                return 0;
                
            string commandText = string.Format("UPDATE [{0}photos] SET albumid={1} WHERE photoid IN ({2}) AND [userid]={3}", BaseConfigs.GetTablePrefix,targetAlbumId, photoIdList,userid);
            return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public int GetPhotoSizeByUserid(int userid)
        {
            string commandText = string.Format("SELECT ISNULL(SUM(filesize), 0) AS [filesize] FROM [{0}photos] WHERE userid={1}", BaseConfigs.GetTablePrefix, userid);
            return (int)DbHelper.ExecuteScalar(CommandType.Text, commandText);
        }

        public int GetSpacePhotoCountByAlbumId(int albumid)
        {
            DbParameter parm = DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4, albumid);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM [{0}photos] WHERE [albumid]=@albumid", BaseConfigs.GetTablePrefix), parm), 0);
        }

        public DataTable GetPhotosByAlbumid(int albumid)
        {
            DbParameter parm = DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4, albumid);
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT [photoid], [userid], [username], [title], [filename] FROM [{0}photos] WHERE [albumid]=@albumid", BaseConfigs.GetTablePrefix), parm).Tables[0];
        }
        #endregion


        #region PhotoComment

        public IDataReader GetPhotoCommentCollection(int photoid)
        {
            string commandText = string.Format("SELECT * FROM [{0}photocomments] WHERE [photoid]={1} ORDER BY [commentid] ASC", BaseConfigs.GetTablePrefix, photoid);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public int CreatePhotoComment(PhotoCommentInfo pcomment)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, pcomment.Userid),
                                        DbHelper.MakeInParam("@username", (DbType)SqlDbType.NVarChar, 20, pcomment.Username),
                                        DbHelper.MakeInParam("@photoid", (DbType)SqlDbType.Int, 4, pcomment.Photoid),
                                        DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.SmallDateTime, 4, pcomment.Postdatetime),
                                        DbHelper.MakeInParam("@ip", (DbType)SqlDbType.VarChar, 100, pcomment.Ip),
                                        DbHelper.MakeInParam("@content", (DbType)SqlDbType.NVarChar, 2000, pcomment.Content)
                                    };
            string commandText = string.Format("INSERT INTO [{0}photocomments]([userid], [username], [photoid], [postdatetime], [ip], [content]) VALUES(@userid, @username, @photoid, @postdatetime, @ip, @content);SELECT SCOPE_IDENTITY()", BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms), 0);
        }

        /// <summary>
        /// 删除图片评论
        /// </summary>
        /// <param name="commentid">评论Id</param>
        public void DeletePhotoComment(int commentid)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}photocomments] WHERE [commentid]={1}", BaseConfigs.GetTablePrefix, commentid));
        }
        #endregion


        public DataTable GetSearchAlbumList(int pagesize, string albumids)
        {
            if (!Utils.IsNumericList(albumids))
                return new DataTable();

            string commandText = string.Format("SELECT TOP {1} [{0}albums].[albumid], [{0}albums].[title], [{0}albums].[username], [{0}albums].[userid], [{0}albums].[createdatetime], [{0}albums].[imgcount], [{0}albums].[views], [{0}albums].[logo] ,[{0}albumcategories].[albumcateid],[{0}albumcategories].[title] AS [categorytitle] FROM [{0}albums] LEFT JOIN [{0}albumcategories] ON [{0}albumcategories].[albumcateid] = [{0}albums].[albumcateid] WHERE [{0}albums].[albumid] IN({2}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[{0}albums].[albumid]),'{2}')", BaseConfigs.GetTablePrefix, pagesize, albumids);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

      
        public void CreatePhotoTags(string tags, int photoid, int userid, string postdatetime)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@tags", (DbType)SqlDbType.NVarChar, 55, tags),
                DbHelper.MakeInParam("@photoid", (DbType)SqlDbType.Int, 4, photoid),
                DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid),
                DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, postdatetime)                
            };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}createphototags", BaseConfigs.GetTablePrefix), parms);
        }

        public IDataReader GetHotTagsListForPhoto(int count)
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT TOP {0} * FROM [{1}tags] WHERE [pcount] > 0 ORDER BY [pcount] DESC,[orderid]", count, BaseConfigs.GetTablePrefix));
        }

        public int GetPhotoCountWithSameTag(int tagid)
        {
            DbParameter parm = DbHelper.MakeInParam("@tagid", (DbType)SqlDbType.Int, 4, tagid);

            string commandText = string.Format("SELECT COUNT(1) FROM [{0}phototags] AS [pt],[{0}photos] AS [p],[{0}albums] AS [a] WHERE [pt].[tagid] = @tagid AND [p].[photoid] = [pt].[photoid] AND [p].[albumid] = [a].[albumid] AND [a].[type]=0", BaseConfigs.GetTablePrefix);

            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parm));
        }


        public IDataReader GetPhotosWithSameTag(int tagid, int pageid, int pagesize)
        {
            DbParameter[] parm = {
                                    DbHelper.MakeInParam("@tagid", (DbType)SqlDbType.Int, 4, tagid),
                                    DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int, 4, pageid),
                                    DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int, 4, pagesize)
                                 };
            string commandText = string.Format("{0}getphotolistbytag", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, commandText, parm);
        }

        public IDataReader GetTagsListByPhotoId(int photoid)
        {
            DbParameter parm = DbHelper.MakeInParam("@photoid", (DbType)SqlDbType.Int, 4, photoid);

            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT [{0}tags].* FROM [{0}tags], [{0}phototags] WHERE [{0}phototags].[tagid] = [{0}tags].[tagid] AND [{0}phototags].[photoid] = @photoid ORDER BY [orderid]", BaseConfigs.GetTablePrefix), parm);
        }


        #region 聚合相册相关函数

        public DataTable GetAlbumListByCondition(string username, string title, string description, string startdate, string enddate, int pageSize, int currentPage, bool isshowall)
        {
            string commandText = "";
            string condition = GetAlbumListCondition(username, title, description, startdate, enddate);
            DbParameter[] parms = GetDateSpanParms(startdate, enddate);
            int pageTop = (currentPage - 1) * pageSize;

            string strisshowall = (isshowall) ?　" 1=1" : " [type] = 0 AND  [imgcount] > 0 ";
            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} *  FROM [{1}albums] WHERE {2} {3} ORDER BY [albumid] DESC", pageSize,
                                           BaseConfigs.GetTablePrefix, strisshowall, condition);
            else
                commandText = string.Format("SELECT TOP {0} * FROM [{1}albums] WHERE [albumid]<(SELECT MIN([albumid]) FROM (SELECT TOP {2} [albumid] FROM [{1}albums] WHERE  {3} {4} ORDER BY [albumid] DESC) AS tblTmp) AND {3} {4} ORDER BY [albumid] DESC",
                                           pageSize, BaseConfigs.GetTablePrefix, pageTop, strisshowall, condition);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        private string GetAlbumListCondition(string usernamelist, string titlelist, string descriptionlist, string startdate, string enddate)
        {
            string condition = "";
            if (usernamelist != "")
            {
                condition += " AND [username] in (";
                string tempusernamelist = "";
                foreach (string p in RegEsc(usernamelist).Split(','))
                {
                    tempusernamelist += "'" + p + "',";
                }
                if (tempusernamelist != "")
                    tempusernamelist = tempusernamelist.Substring(0, tempusernamelist.Length - 1);
                condition += tempusernamelist + ")";
            }
            if (titlelist != "")
            {
                condition += " AND [title] in (";
                string temptitlelist = "";
                foreach (string p in RegEsc(titlelist).Split(','))
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
                foreach (string description in RegEsc(descriptionlist).Split(','))
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

            return condition;
        }

        public int GetAlbumListCountByCondition(string username, string title, string description, string startdate, string enddate, bool isshowall)
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}albums] t", BaseConfigs.GetTablePrefix);
            if (isshowall)
                commandText += " WHERE 1=1";
            else
                commandText += " WHERE [type] = 0 AND  [imgcount] > 0";

            DbParameter[] parms = GetDateSpanParms(startdate, enddate);

            string condition = GetAlbumListCondition(username, title, description, startdate, enddate);
            if (condition != "")
                commandText += condition;
            return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString());
        }


        public DataTable GetAlbumLitByAlbumidList(string albumlist)
        {
            if (!Utils.IsNumericList(albumlist))
                return new DataTable();

            string commandText = string.Format("SELECT * FROM [{0}albums] WHERE [type] = 0 AND [albumid] IN ({1}) ORDER BY CHARINDEX(CONVERT(VARCHAR(8),[albumid]),'{1}')",
                                                BaseConfigs.GetTablePrefix, albumlist);
            return DbHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        #endregion

     
        public int GetUidByAlbumid(int albumid)
        {
            return TypeConverter.ObjectToInt(
                                 DbHelper.ExecuteScalar(CommandType.Text, 
                                                        string.Format("SELECT [userid] FROM [{0}albums] WHERE [albumid]=@albumid", BaseConfigs.GetTablePrefix),
                                                        DbHelper.MakeInParam("@albumid", (DbType)SqlDbType.Int, 4, albumid)));
        }

        #region 照片操作相关函数
        //此方法在后台调用，没有防止commandText注入，暂没修改
        public int GetPhotoCountByCondition(string photousernamelist, string keylist, string startdate, string enddate)
        {
            string commandText = string.Format("SELECT COUNT(1) FROM [{0}photos] p LEFT JOIN [{0}albums] a ON p.[albumid]=a.[albumid] WHERE a.[type]=0", BaseConfigs.GetTablePrefix);
            DbParameter[] parms = GetDateSpanParms(startdate, enddate);
            string condition = GetPhotoCondition(photousernamelist, keylist, startdate, enddate);
            if (condition != "")
                commandText += condition;
            return int.Parse(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString());
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

        public DataTable GetPhotoByCondition(string photousernamelist, string keylist, string startdate, string enddate, int pageSize, int currentPage)
        {
            string commandText = "";
            DbParameter[] parms = GetDateSpanParms(startdate, enddate);
            string condition = GetPhotoCondition(photousernamelist, keylist, startdate, enddate);
            int pageTop = (currentPage - 1) * pageSize;
            if (currentPage == 1)
                commandText = string.Format("SELECT TOP {0} p.* FROM [{1}photos] p LEFT JOIN [{1}albums] a ON p.[albumid]=a.[albumid] WHERE a.[type]=0 {2} ORDER BY p.[photoid] DESC",
                                             pageSize, BaseConfigs.GetTablePrefix, condition);
            else
                commandText = string.Format("SELECT TOP {0} p.* FROM [{1}photos] p LEFT JOIN [{1}albums] a ON p.[albumid]=a.[albumid] WHERE a.[type]=0 AND p.[photoid]<(SELECT MIN([photoid]) FROM (SELECT TOP {2}"
                                            + " p.[photoid] FROM [{1}photos] p LEFT JOIN [{1}albums] a ON p.[albumid]=a.[albumid] WHERE a.[type]=0 {3} ORDER BY p.[photoid] DESC) AS tblTmp) {3} ORDER BY p.[photoid] DESC",
                                            pageSize, BaseConfigs.GetTablePrefix, pageTop, condition); 
            return DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }

        #endregion

        public IDataReader GetAlbumListByCondition(int type, int focusphotocount, int vaildDays)
        {
            DbParameter parm = DbHelper.MakeInParam("@vailddays", (DbType)SqlDbType.Int, 4, vaildDays);
            string commandText = string.Format("SELECT TOP {0} * FROM [{1}albums] WHERE DATEDIFF(d, [createdatetime], getdate()) < @vailddays AND [imgcount]>0 AND [type]=0", focusphotocount, BaseConfigs.GetTablePrefix);

            switch (type)
            {
                case 0:
                    commandText += " ORDER BY [createdatetime] DESC";
                    break;
                case 1:
                    commandText += " ORDER BY [views] DESC";
                    break;
                case 2:
                    commandText += " ORDER BY [imgcount] DESC";
                    break;
                default:
                    commandText += " ORDER BY [createdatetime] DESC";
                    break;
            }
            return DbHelper.ExecuteReader(CommandType.Text, commandText, parm);
        }

        public void DeletePhotoTags(int photoid)
        {
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, 
                                     string.Format("{0}deletephototags", BaseConfigs.GetTablePrefix), 
                                     DbHelper.MakeInParam("@photoid", (DbType)SqlDbType.Int, 4, photoid));
        }

        public void DeleteAll(int userid)
        {
            DbParameter parm = DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userid);

            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}photocomments] WHERE [userid]=@userid", BaseConfigs.GetTablePrefix), parm);
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}photos] WHERE [userid]=@userid", BaseConfigs.GetTablePrefix), parm);
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}albums] WHERE [userid]=@userid", BaseConfigs.GetTablePrefix), parm);
        }

    }
}
