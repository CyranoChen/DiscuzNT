using System;
using System.Collections;
using System.Data;
using System.Text;

using Discuz.Cache;
using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Entity;

namespace Discuz.Album.Data
{
    public class DTOProvider
    {
        #region Space 相册操作
        public static AlbumCategoryInfo GetAlbumCategory(int albumcateid)
        {
            foreach (AlbumCategoryInfo aci in GetAlbumCategory())
            {
                if (aci.Albumcateid == albumcateid)
                    return aci;
            }
            return new AlbumCategoryInfo();
        }
        
        public static AlbumInfo GetAlbumInfo(int aid)
        {
            IDataReader reader = DbProvider.GetInstance().GetSpaceAlbumById(aid);
            if (reader.Read())
            {
                AlbumInfo albumsinfo = GetAlbumEntity(reader);
                reader.Close();
                return albumsinfo;
            }
            else
            {
                reader.Close();
                return null;
            }
        }

        public static AlbumInfo[] GetSpaceAlbumsInfo(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return null;

            AlbumInfo[] albumsinfoarray = new AlbumInfo[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                albumsinfoarray[i] = new AlbumInfo();
                albumsinfoarray[i].Albumid = TypeConverter.ObjectToInt(dt.Rows[i]["albumid"]);
                albumsinfoarray[i].Userid = TypeConverter.ObjectToInt(dt.Rows[i]["userid"]);
                albumsinfoarray[i].Title = dt.Rows[i]["title"].ToString();
                albumsinfoarray[i].Description = dt.Rows[i]["description"].ToString();
                albumsinfoarray[i].Logo = dt.Rows[i]["logo"].ToString();
                albumsinfoarray[i].Password = dt.Rows[i]["password"].ToString();
                albumsinfoarray[i].Imgcount = TypeConverter.ObjectToInt(dt.Rows[i]["imgcount"]);
                albumsinfoarray[i].Views = TypeConverter.ObjectToInt(dt.Rows[i]["views"]);
                albumsinfoarray[i].Type = TypeConverter.ObjectToInt(dt.Rows[i]["type"]);
                albumsinfoarray[i].Createdatetime = dt.Rows[i]["createdatetime"].ToString();
            }

            dt.Dispose();
            return albumsinfoarray;
        }

        public static void UpdateAlbumViews(int albumid)
        {
            Data.DbProvider.GetInstance().UpdateAlbumViews(albumid);
        }
        #endregion

        #region Space 照片操作
        public static PhotoInfo GetPhotoInfo(IDataReader reader)
        {
            if (reader.Read())
            {
                PhotoInfo pi = GetPhotoEntity(reader);
                reader.Close();
                return pi;
            }
            else
            {
                reader.Close();
                return null;
            }
        }

        public static PhotoInfo GetPhotoEntity(IDataReader reader)
        {
            PhotoInfo photoinfo = new PhotoInfo();
            photoinfo.Photoid = TypeConverter.ObjectToInt(reader["photoid"]);
            photoinfo.Filename = reader["filename"].ToString();
            photoinfo.Attachment = reader["attachment"].ToString();
            photoinfo.Filesize = TypeConverter.ObjectToInt(reader["filesize"]);
            photoinfo.Description = reader["description"].ToString();
            photoinfo.Postdate = reader["postdate"].ToString();
            photoinfo.Albumid = TypeConverter.ObjectToInt(reader["albumid"]);
            photoinfo.Userid = TypeConverter.ObjectToInt(reader["userid"]);
            photoinfo.Title = reader["title"].ToString();
            photoinfo.Views = TypeConverter.ObjectToInt(reader["views"]);
            photoinfo.Commentstatus = (PhotoStatus)TypeConverter.ObjectToInt(reader["commentstatus"]);
            photoinfo.Tagstatus = (PhotoStatus)TypeConverter.ObjectToInt(reader["tagstatus"]);
            photoinfo.Comments = TypeConverter.ObjectToInt(reader["comments"]);
            photoinfo.Username = reader["username"].ToString();

            return photoinfo;
        }

        public static int GetSpacePhotosCount(int albumid)
        {
            return Data.DbProvider.GetInstance().GetSpacePhotoCountByAlbumId(albumid);
        }

        public static int GetSpaceAlbumListCount(int userid, int albumcategoryid)
        {
            return Data.DbProvider.GetInstance().SpaceAlbumsListCount(userid, albumcategoryid);
        }

        public static AlbumInfo GetAlbumEntity(IDataReader reader)
        {
            AlbumInfo album = new AlbumInfo();

            album.Albumid = TypeConverter.ObjectToInt(reader["albumid"]);
            album.Userid = TypeConverter.ObjectToInt(reader["userid"]);
            album.Username = reader["username"].ToString();
            album.Title = reader["title"].ToString();
            album.Description = reader["description"].ToString();
            album.Logo = reader["logo"].ToString();
            album.Password = reader["password"].ToString();
            album.Imgcount = TypeConverter.ObjectToInt(reader["imgcount"]);
            album.Views = TypeConverter.ObjectToInt(reader["views"]);
            album.Type = TypeConverter.ObjectToInt(reader["type"]);
            album.Createdatetime = reader["createdatetime"].ToString();
            album.Albumcateid = TypeConverter.ObjectToInt(reader["albumcateid"]);

            return album;
        }

        #endregion

        public static PhotoInfo GetPhotoInfo(int photoid, int albumid, byte mode)
        {
            return GetPhotoInfo(Data.DbProvider.GetInstance().GetPhotoByID(photoid, albumid, mode));
        }

        public static void UpdatePhotoViews(int photoid)
        {
            Data.DbProvider.GetInstance().UpdatePhotoViews(photoid);
        }

        public static void UpdatePhotoInfo(PhotoInfo photo)
        {
            Data.DbProvider.GetInstance().UpdatePhotoInfo(photo);
        }
        
        #region 集合改泛型

        public static Discuz.Common.Generic.List<AlbumCategoryInfo> GetAlbumCategory()
        {
            DNTCache cache = DNTCache.GetCacheService();
            Discuz.Common.Generic.List<AlbumCategoryInfo> acic = cache.RetrieveObject("/Space/AlbumCategory") as Discuz.Common.Generic.List<AlbumCategoryInfo>;

            if (acic == null)
            {
                acic = new Discuz.Common.Generic.List<AlbumCategoryInfo>();
                acic = Data.DbProvider.GetInstance().GetAlbumCategory();
                cache.AddObject("/Space/AlbumCategory", (ICollection)acic);
            }
            return acic;
        }


        public static Discuz.Common.Generic.List<AlbumInfo> GetSpaceAlbumList(int userid, int albumcategoryid, int pageSize, int currentPage)
        {
            Discuz.Common.Generic.List<AlbumInfo> aic = new Discuz.Common.Generic.List<AlbumInfo>();
            IDataReader reader = Data.DbProvider.GetInstance().SpaceAlbumsList(userid, albumcategoryid, pageSize, currentPage);
            while (reader.Read())
            {
                aic.Add(GetAlbumEntity(reader));
            }
            reader.Close();
            return aic;
        }


        public static Discuz.Common.Generic.List<AlbumInfo> GetAlbumRankList(int albumcount)
        {
            Discuz.Common.Generic.List<AlbumInfo> aic = DNTCache.GetCacheService().RetrieveObject(string.Format("/Photo/AlbumRank{0}", albumcount)) as Discuz.Common.Generic.List<AlbumInfo>;

            if (aic == null)
            {
                IDataReader reader = Data.DbProvider.GetInstance().GetPhotoRankList(3, albumcount);
                aic = new Discuz.Common.Generic.List<AlbumInfo>();
                while (reader.Read())
                {
                    aic.Add(GetAlbumEntity(reader));
                }
                reader.Close();
                DNTCache.GetCacheService().AddObject(string.Format("/Photo/AlbumRank{0}", albumcount), (ICollection)aic);
            }
            return aic;
        }

        public static Discuz.Common.Generic.List<PhotoCommentInfo> GetPhotoCommentCollection(int photoid)
        {
            Discuz.Common.Generic.List<PhotoCommentInfo> comments = new Discuz.Common.Generic.List<PhotoCommentInfo>();
            IDataReader reader = Data.DbProvider.GetInstance().GetPhotoCommentCollection(photoid);
            while (reader.Read())
            {
                PhotoCommentInfo comment = new PhotoCommentInfo();
                comment.Commentid = TypeConverter.ObjectToInt(reader["commentid"]);
                comment.Content = Utils.RemoveHtml(reader["content"].ToString());
                comment.Ip = reader["ip"].ToString();
                comment.Photoid = TypeConverter.ObjectToInt(reader["photoid"]);
                comment.Postdatetime = Convert.ToDateTime(reader["postdatetime"]);
                comment.Userid = TypeConverter.ObjectToInt(reader["userid"]);
                comment.Username = reader["username"].ToString();

                comments.Add(comment);
            }
            reader.Close();
            return comments;
        }

        public static List<PhotoInfo> GetPhotoListByUserId(int userid, int albumid, int count)
        {
            List<PhotoInfo> photolist = new List<PhotoInfo>();

            IDataReader reader = Data.DbProvider.GetInstance().GetPhotoListByUserId(userid, albumid, count);
            while (reader.Read())
            {
                photolist.Add(GetPhotoEntity(reader));
            }
            reader.Close();

            return photolist;
        }

        public static Discuz.Common.Generic.List<PhotoInfo> GetSpacePhotosInfo(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return new Discuz.Common.Generic.List<PhotoInfo>();

            Discuz.Common.Generic.List<PhotoInfo> photosinfoarray = new Discuz.Common.Generic.List<PhotoInfo>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PhotoInfo photo = new PhotoInfo();
                photo.Photoid = TypeConverter.ObjectToInt(dt.Rows[i]["photoid"]);
                photo.Filename = dt.Rows[i]["filename"].ToString();
                photo.Attachment = dt.Rows[i]["attachment"].ToString();
                photo.Filesize = TypeConverter.ObjectToInt(dt.Rows[i]["filesize"]);
                photo.Description = dt.Rows[i]["description"].ToString();
                photo.Postdate = dt.Rows[i]["postdate"].ToString();
                photo.Albumid = TypeConverter.ObjectToInt(dt.Rows[i]["albumid"]);
                photo.Userid = TypeConverter.ObjectToInt(dt.Rows[i]["userid"]);
                photo.Title = dt.Rows[i]["title"].ToString();
                photo.Views = TypeConverter.ObjectToInt(dt.Rows[i]["views"]);
                photo.Commentstatus = (PhotoStatus)TypeConverter.ObjectToInt(dt.Rows[i]["commentstatus"]);
                photo.Tagstatus = (PhotoStatus)TypeConverter.ObjectToInt(dt.Rows[i]["tagstatus"]);
                photo.Comments = TypeConverter.ObjectToInt(dt.Rows[i]["comments"]);

                photosinfoarray.Add(photo);
            }
            dt.Dispose();
            return photosinfoarray;
        }


        /// <summary>
        /// 获得图片排行图集合
        /// </summary>
        /// <param name="type">排行方式，0浏览量，1评论数,2上传时间</param>
        /// <returns></returns>
        public static Discuz.Common.Generic.List<PhotoInfo> GetPhotoRankList(int type, int photocount)
        {
            Discuz.Common.Generic.List<PhotoInfo> pic = DNTCache.GetCacheService().RetrieveObject(string.Format("/Photo/PhotoRank{0}-{1}", type, photocount)) as Discuz.Common.Generic.List<PhotoInfo>;

            if (pic == null)
            {
                IDataReader reader = Data.DbProvider.GetInstance().GetPhotoRankList(type, photocount);
                pic = new Discuz.Common.Generic.List<PhotoInfo>();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        PhotoInfo pi = GetPhotoEntity(reader);
                        pi.Filename = Globals.GetSquareImage(pi.Filename);
                        pic.Add(pi);
                    }
                    reader.Close();
                }
                DNTCache.GetCacheService().AddObject(string.Format("/Photo/PhotoRank{0}-{1}", type, photocount), (ICollection)pic);
            }

            return pic;
        }
        #endregion
    }
}
