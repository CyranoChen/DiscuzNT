using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using Discuz.Common.Generic;
using Discuz.Entity;
using Discuz.Common;
using Discuz.Config;
using System.IO;
using Discuz.Forum;
using System.Drawing;
using Discuz.Album.Data;
using Discuz.Cache;

namespace Discuz.Album
{
    public class Albums
    {

        #region AlbumJson
        /// <summary>
        /// 获得相册的JSON格式数据
        /// </summary>
        /// <param name="albumid">相册ID</param>
        /// <returns></returns>
        public static string GetAlbumJsonData(int albumid)
        {
            DataTable dtAlbum = Data.DbProvider.GetInstance().GetPhotosByAlbumid(albumid);
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"items\":[");
            foreach (DataRow dr in dtAlbum.Rows)
            {
                if (dr["filename"].ToString().Trim().ToLower().IndexOf("http") == 0)
                {
                    builder.AppendFormat(@"{{""photoid"":{0},""userid"":{1},""title"":""{2}"",""image"":""{3}"",""square"":""{4}"",""thumbnail"":""{5}""}},", dr["photoid"], dr["userid"], dr["title"].ToString().Trim().Replace("\"", "\\\""), dr["filename"].ToString().Trim(), Globals.GetSquareImage(dr["filename"].ToString().Trim()), Globals.GetThumbnailImage(dr["filename"].ToString().Trim()));
                }
                else
                {
                    builder.AppendFormat(@"{{""photoid"":{0},""userid"":{1},""title"":""{2}"",""image"":""{3}"",""square"":""{4}"",""thumbnail"":""{5}""}},", dr["photoid"], dr["userid"], dr["title"].ToString().Trim().Replace("\"", "\\\""), BaseConfigs.GetForumPath + dr["filename"].ToString().Trim(), Globals.GetSquareImage(BaseConfigs.GetForumPath + dr["filename"].ToString().Trim()), Globals.GetThumbnailImage(BaseConfigs.GetForumPath + dr["filename"].ToString().Trim()));
                }
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append("]}");
            return builder.ToString();
        }

        /// <summary>
        /// 生成json文件
        /// </summary>
        /// <param name="albumid">相册ID</param>
        /// <returns>json数据是否生成</returns>
        public static bool CreateAlbumJsonData(int albumid)
        {
            string json = GetAlbumJsonData(albumid);
            string cachedir = ForumUtils.GetCacheDir("album/") + (albumid / 1000 + 1).ToString() + "/";
            if (!Directory.Exists(Utils.GetMapPath(cachedir)))
                Utils.CreateDir(Utils.GetMapPath(cachedir));

            string filename = Utils.GetMapPath(cachedir + albumid.ToString() + "_json.txt");
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    Byte[] info = System.Text.Encoding.UTF8.GetBytes(json);
                    fs.Write(info, 0, info.Length);
                    fs.Close();
                    return true;
                }

            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region AlbumImage

        /// <summary>
        /// 生成图片标题PNG图片
        /// </summary>
        /// <param name="photoid">图片ID</param>
        /// <param name="title">标题</param>
        public static void CreatePhotoTitleImage(int photoid, string title)
        {
            string cachedir = ForumUtils.GetCacheDir("photo/") + (photoid / 1000 + 1).ToString() + "/";
            if (!Directory.Exists(Utils.GetMapPath(cachedir)))
                Utils.CreateDir(Utils.GetMapPath(cachedir));

            string filename = Utils.GetMapPath(cachedir + photoid.ToString() + "_title.png");
            if (File.Exists(filename))
                return;
            int quality = 100;
            string fontname = "Simhei";
            int fontsize = 30;

            try
            {
                ForumUtils.CreateTextImage(filename, title, quality, fontname, fontsize, Color.FromArgb(102, 153, 204));
            }
            catch
            { }

        }

        /// <summary>
        /// 生成用户名图片
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="username">用户名</param>
        public static void CreateUserImage(int userid, string username)
        {
            string cachedir = ForumUtils.GetCacheDir("user/") + (userid / 1000 + 1).ToString() + "/";
            if (!Directory.Exists(Utils.GetMapPath(cachedir)))
                Utils.CreateDir(Utils.GetMapPath(cachedir));

            string filename = Utils.GetMapPath(cachedir + userid.ToString() + "_name.png");
            if (File.Exists(filename))
                return;
            int quality = 100;
            string fontname = "Simhei";
            int fontsize = 18;

            try
            {
                ForumUtils.CreateTextImage(filename, username, quality, fontname, fontsize, Color.FromArgb(153, 153, 153));
            }
            catch
            { }
        }

        /// <summary>
        /// 生成相册中图片标题和用户名图片
        /// </summary>
        /// <param name="albumid">相册ID</param>
        public static void CreatePhotoImageByAlbum(int albumid)
        {
            DataTable dtAlbum = Data.DbProvider.GetInstance().GetPhotosByAlbumid(albumid);
            foreach (DataRow dr in dtAlbum.Rows)
            {
                //生成图片标题
                CreatePhotoTitleImage(Utils.StrToInt(dr["photoid"], 0), dr["title"].ToString());
                //生成用户名图片
                CreateUserImage(Utils.StrToInt(dr["userid"], 0), dr["username"].ToString());
            }
        }
        #endregion

        public static int GetPhotoCountWithSameTag(int tagid)
        {
            return Data.DbProvider.GetInstance().GetPhotoCountWithSameTag(tagid);
        }

        public static Discuz.Common.Generic.List<PhotoInfo> GetPhotosWithSameTag(int tagid, int pageid, int pagesize)
        {
            IDataReader reader = Data.DbProvider.GetInstance().GetPhotosWithSameTag(tagid, pageid, pagesize);
            Discuz.Common.Generic.List<PhotoInfo> photolist = new Discuz.Common.Generic.List<PhotoInfo>();
            while (reader.Read())
            {
                photolist.Add(GetPhotoEntity(reader));
            }
            reader.Close();

            return photolist;
        }

        private static PhotoInfo GetPhotoEntity(IDataReader reader)
        {
            PhotoInfo p = new PhotoInfo();
            p.Photoid = Utils.StrToInt(reader["photoid"], 0);
            p.Filename = reader["filename"].ToString();
            p.Title = reader["title"].ToString();
            p.Filesize = Utils.StrToInt(reader["filesize"], 0);
            return p;
        }

        /// <summary>
        /// 相册相关页面的弹出导航菜单HTML内容
        /// </summary>
        /// <returns></returns>
        public static string GetPhotoListMenuDivCache()
        {
            string val = DNTCache.GetCacheService().RetrieveObject("/Photo/AlbumCategoryMenu") as string;
            if (val != null)
                return val;

            StringBuilder sb = new StringBuilder();
            Discuz.Common.Generic.List<AlbumCategoryInfo> acic = DTOProvider.GetAlbumCategory();
            if (acic.Count > 0)
            {
                sb.Append("<div id=\"NavHome_menu\" class=\"NavHomeMenu\" style=\"display:none\"><div id=\"NavHomeWindow\"><ul>");
                foreach (AlbumCategoryInfo aci in acic)
                {
                    sb.AppendLine("<li><a href=\"showalbumlist.aspx?cate=");
                    sb.AppendLine(aci.Albumcateid.ToString());
                    sb.Append("\">");
                    sb.AppendLine(aci.Title);
                    sb.AppendLine("</a></li>");
                }
                sb.Append("</ul></div></div>");
            }

            DNTCache.GetCacheService().AddObject("/Photo/AlbumCategoryMenu", sb.ToString());
            return sb.ToString();
        }

        /// <summary>
        /// 删除用户所有相册、相片、评论
        /// </summary>
        /// <param name="userid"></param>
        public static void DeleteAlbums(int userid)
        {
            DbProvider.GetInstance().DeleteAll(userid);
        }
    }
}
