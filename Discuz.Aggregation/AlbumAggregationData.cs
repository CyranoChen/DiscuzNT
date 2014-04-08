using System;
using System.Text;
using System.Data;
using System.Xml;

using Discuz.Entity;
using Discuz.Plugin.Album;

namespace Discuz.Aggregation
{
    /// <summary>
    /// 相册聚合数据类
    /// </summary>
    public class AlbumAggregationData : AggregationData
    {
        /// <summary>
        /// 图片轮换字符串
        /// </summary>
        private static StringBuilder __albumRotatepic = null;
        /// <summary>
        /// 推荐到聚合首页的相册列表
        /// </summary>
        private static Discuz.Common.Generic.List<AlbumInfo> __recommandAlbumListForWebSite;
        /// <summary>
        /// 推荐到聚合空间首页的相册列表
        /// </summary>
        private static Discuz.Common.Generic.List<AlbumInfo> __recommandAlbumListForSpaceIndex;
        /// <summary>
        /// 推荐到聚合相册首页的相册列表
        /// </summary>
        private static Discuz.Common.Generic.List<AlbumInfo> __recommandAlbumListForAlbumIndex;
        /// <summary>
        /// 焦点相册列表
        /// </summary>
        private static Discuz.Common.Generic.List<AlbumInfo> __focusAlbumList;
        /// <summary>
        /// 焦点图片列表
        /// </summary>
        private static Discuz.Common.Generic.List<PhotoInfo> __focusPhotoList;
        /// <summary>
        /// 推荐图片列表
        /// </summary>
        private static Discuz.Common.Generic.List<PhotoInfo> __recommandPhotoList;
        /// <summary>
        /// 一周热图总排行
        /// </summary>
        private static Discuz.Common.Generic.List<PhotoInfo> __weekHotPhotoList;

        /// <summary>
        /// 从XML中检索出相册的轮换图片信息
        /// </summary>
        /// <returns></returns>
        public new string GetRotatePicData()
        {
            //当文件未被修改时将直接返回相关记录
            if (__albumRotatepic != null)
            {
                return __albumRotatepic.ToString();
            }
            __albumRotatepic = new StringBuilder();
            __albumRotatepic.Append(base.GetRotatePicStr("Albumindex"));

            return __albumRotatepic.ToString();
        }


        /// <summary>
        /// 清空数据绑定
        /// </summary>
        public override void ClearDataBind()
        {
            __recommandAlbumListForWebSite = null;
            __recommandAlbumListForSpaceIndex = null;
            __recommandAlbumListForAlbumIndex = null;
            __focusAlbumList = null;
            __focusPhotoList = null;
            __recommandPhotoList = null;
            __albumRotatepic = null;
            __weekHotPhotoList = null;
        }


        /// <summary>
        /// 获得推荐图片列表
        /// </summary>
        /// <param name="nodename">节点名称</param>
        /// <returns></returns>
        public Discuz.Common.Generic.List<PhotoInfo> GetRecommandPhotoList(string nodeName)
        {
            //当文件未被修改时将直接返回相关记录
            if (__recommandPhotoList != null)
            {
                return __recommandPhotoList;
            }

            __recommandPhotoList = new Discuz.Common.Generic.List<PhotoInfo>();
            XmlNodeList xmlnodelist = xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/" + nodeName + "/" + nodeName + "_photolist/Photo");

            foreach (XmlNode xmlnode in xmlnodelist)
            {
                PhotoInfo recommandPhoto = new PhotoInfo();

                recommandPhoto.Photoid = (xmlDoc.GetSingleNodeValue(xmlnode, "photoid") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "photoid"));
                recommandPhoto.Filename = (xmlDoc.GetSingleNodeValue(xmlnode, "filename") == null) ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "filename");
                recommandPhoto.Attachment = (xmlDoc.GetSingleNodeValue(xmlnode, "attachment") == null) ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "attachment");
                recommandPhoto.Filesize = (xmlDoc.GetSingleNodeValue(xmlnode, "filesize") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "filesize"));
                recommandPhoto.Description = (xmlDoc.GetSingleNodeValue(xmlnode, "description") == null) ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "description");
                recommandPhoto.Postdate = (xmlDoc.GetSingleNodeValue(xmlnode, "postdate") == null) ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "postdate");
                recommandPhoto.Albumid = (xmlDoc.GetSingleNodeValue(xmlnode, "albumid") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "albumid"));
                recommandPhoto.Userid = (xmlDoc.GetSingleNodeValue(xmlnode, "userid") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "userid"));
                recommandPhoto.Title = (xmlDoc.GetSingleNodeValue(xmlnode, "title") == null) ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "title");
                recommandPhoto.Views = (xmlDoc.GetSingleNodeValue(xmlnode, "views") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "views"));
                recommandPhoto.Commentstatus = (xmlDoc.GetSingleNodeValue(xmlnode, "commentstatus") == null) ? PhotoStatus.Owner : (PhotoStatus)Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "commentstatus"));
                recommandPhoto.Tagstatus = (xmlDoc.GetSingleNodeValue(xmlnode, "tagstatus") == null) ? PhotoStatus.Owner : (PhotoStatus)Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "tagstatus"));
                recommandPhoto.Comments = (xmlDoc.GetSingleNodeValue(xmlnode, "comments") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "comments"));
                recommandPhoto.Username = (xmlDoc.GetSingleNodeValue(xmlnode, "username") == null) ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "username");
                __recommandPhotoList.Add(recommandPhoto);

            }
            return __recommandPhotoList;
        }


        /// <summary>
        /// 获得焦点图片列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="focusphotocount">返回的记录数</param>
        /// <param name="vaildDays">有效天数</param>
        /// <returns></returns>
        public Discuz.Common.Generic.List<PhotoInfo> GetFocusPhotoList(int type, int focusPhotoCount, int vaildDays)
        {
            //当文件未被修改时将直接返回相关记录
            if (__focusPhotoList != null)
            {
                return __focusPhotoList;
            }

            __focusPhotoList = new Discuz.Common.Generic.List<PhotoInfo>();
            IDataReader reader = AlbumPluginProvider.GetInstance().GetFocusPhotoList(type, focusPhotoCount, vaildDays);
            if (reader != null)
            {
                while (reader.Read())
                {
                    PhotoInfo pi = AlbumPluginProvider.GetInstance().GetPhotoEntity(reader);
                    pi.Filename = AlbumPluginProvider.GetInstance().GetThumbnailImage(pi.Filename);
                    pi.Title = pi.Title.Trim();
                    __focusPhotoList.Add(pi);
                }
                reader.Close();
            }
            return __focusPhotoList;
        }


        /// <summary>
        /// 一周热图总排行
        /// </summary>
        /// <param name="focusphotocount">返回的记录数</param>
        /// <returns></returns>
        public Discuz.Common.Generic.List<PhotoInfo> GetWeekHotPhotoList(int focusPhotoCount)
        {
            //当文件未被修改时将直接返回相关记录
            if (__weekHotPhotoList != null)
            {
                return __weekHotPhotoList;
            }

            __weekHotPhotoList = new Discuz.Common.Generic.List<PhotoInfo>();
            IDataReader reader = AlbumPluginProvider.GetInstance().GetFocusPhotoList(0, focusPhotoCount, 7);
            if (reader != null)
            {
                while (reader.Read())
                {
                    PhotoInfo pi = AlbumPluginProvider.GetInstance().GetPhotoEntity(reader);
                    pi.Filename = AlbumPluginProvider.GetInstance().GetThumbnailImage(pi.Filename);
                    pi.Title = pi.Title.Trim();
                    __weekHotPhotoList.Add(pi);
                }
                reader.Close();
            }
            return __weekHotPhotoList;
        }

        
        /// <summary>
        /// 获得推荐相册列表
        /// </summary>
        /// <param name="nodename">节点名称</param>
        /// <returns></returns>
        public Discuz.Common.Generic.List<AlbumInfo> GetRecommandAlbumList(string nodeName)
        {
            Discuz.Common.Generic.List<AlbumInfo> __recommandAlbumList = null;

            switch (nodeName)
            {
                case "Website":      __recommandAlbumList = __recommandAlbumListForWebSite; break;
                case "Spaceindex":   __recommandAlbumList = __recommandAlbumListForSpaceIndex; break;
                case "Albumindex":   __recommandAlbumList = __recommandAlbumListForAlbumIndex; break;
                default:             __recommandAlbumList = __recommandAlbumListForWebSite; break;
            }

            //当文件未被修改时将直接返回相关记录
            if (__recommandAlbumList != null)
                return __recommandAlbumList;

            __recommandAlbumList = new Discuz.Common.Generic.List<AlbumInfo>();
            XmlNodeList xmlnodelist = xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/" + nodeName + "/" + nodeName + "_albumlist/Album");

            foreach (XmlNode xmlnode in xmlnodelist)
            {
                AlbumInfo album = new AlbumInfo();

                album.Albumid = (xmlDoc.GetSingleNodeValue(xmlnode, "albumid") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "albumid"));
                album.Userid = (xmlDoc.GetSingleNodeValue(xmlnode, "userid") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "userid"));
                album.Username = (xmlDoc.GetSingleNodeValue(xmlnode, "username") == null) ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "username");
                album.Title = (xmlDoc.GetSingleNodeValue(xmlnode, "title") == null) ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "title");
                album.Description = (xmlDoc.GetSingleNodeValue(xmlnode, "description") == null) ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "description");
                album.Logo = (xmlDoc.GetSingleNodeValue(xmlnode, "logo") == null) ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "logo");
                album.Password = (xmlDoc.GetSingleNodeValue(xmlnode, "password") == null) ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "password");
                album.Imgcount = (xmlDoc.GetSingleNodeValue(xmlnode, "imgcount") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "imgcount"));
                album.Views = (xmlDoc.GetSingleNodeValue(xmlnode, "views") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "views"));
                album.Type = (xmlDoc.GetSingleNodeValue(xmlnode, "type") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "type"));
                album.Createdatetime = (xmlDoc.GetSingleNodeValue(xmlnode, "createdatetime") == null) ? "" : xmlDoc.GetSingleNodeValue(xmlnode, "createdatetime");
                album.Albumcateid = (xmlDoc.GetSingleNodeValue(xmlnode, "albumcateid") == null) ? 0 : Convert.ToInt32(xmlDoc.GetSingleNodeValue(xmlnode, "albumcateid"));
                __recommandAlbumList.Add(album);
            }

            switch (nodeName)
            {
                case "Website":   __recommandAlbumListForWebSite = __recommandAlbumList; break;
                case "Spaceindex":__recommandAlbumListForSpaceIndex = __recommandAlbumList; break;
                case "Albumindex":__recommandAlbumListForAlbumIndex = __recommandAlbumList; break;
                default:          __recommandAlbumListForWebSite = __recommandAlbumList; break;
            }
            return __recommandAlbumList;
        }

        /// <summary>
        /// 获得焦点相册列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="focusphotocount">返回记录条数</param>
        /// <param name="vaildDays">有效天数</param>
        /// <returns></returns>
        public Discuz.Common.Generic.List<AlbumInfo> GetAlbumList(int type, int focusPhotoCount, int vaildDays)
        {
            //当文件未被修改时将直接返回相关记录
            if (__focusAlbumList != null)
                return __focusAlbumList;

            IDataReader reader = AlbumPluginProvider.GetInstance().GetAlbumListByCondition(type, focusPhotoCount, vaildDays);
            __focusAlbumList = new Discuz.Common.Generic.List<AlbumInfo>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    __focusAlbumList.Add(AlbumPluginProvider.GetInstance().GetAlbumEntity(reader));
                }
                reader.Close();
            }
            return __focusAlbumList;
        }
    }
}
