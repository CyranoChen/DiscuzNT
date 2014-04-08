using System;
using System.Data;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Data;
using Discuz.Config;
using System.IO;
using Discuz.Album.Data;

namespace Discuz.Album.Pages
{
    /// <summary>
    /// 相册内容页
    /// </summary>
    public class showalbum : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 相册Id
        /// </summary>
        public int albumid = DNTRequest.GetInt("albumid", 0);
        /// <summary>
        /// 相册信息
        /// </summary>
        public AlbumInfo album;
        /// <summary>
        /// 图片列表
        /// </summary>
        public List<PhotoInfo> photolist = new List<PhotoInfo>();
        /// <summary>
        /// 当前页码
        /// </summary>
        public int currentpage = DNTRequest.GetInt("page", 1);
        /// <summary>
        /// 图片总数
        /// </summary>
        public int photoscount = 0;
        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers = "";
        /// <summary>
        /// 分页总数
        /// </summary>
        public int pageCount = 0;
        /// <summary>
        /// 相册分类列表
        /// </summary>
        public AlbumCategoryInfo albumcategory;
        /// <summary>
        /// 是否需要密码访问
        /// </summary>
        public bool needpassword = true;
        /// <summary>
        /// 相册弹出导航菜单HTML代码
        /// </summary>
        public string navhomemenu = Albums.GetPhotoListMenuDivCache();
        /// <summary>
        /// 相册RSS UrlRewrite
        /// </summary>
        private string photorssurl = "";
        #endregion

        private int pageSize = 16;

        protected override void ShowPage()
        {
            if (config.Enablealbum != 1)
            {
                AddErrLine("相册功能已被关闭");
                return;
            }

            forumpath = BaseConfigs.GetForumPath;
            if (albumid < 1)
            {
                AddErrLine("指定的相册不存在");
                return;
            }

            album = DTOProvider.GetAlbumInfo(albumid);
            if (album == null)
            {
                AddErrLine("指定的相册不存在");
                return;
            }

            if (config.Rssstatus == 1)
            {
                if (GeneralConfigs.GetConfig().Aspxrewrite == 1)
                    photorssurl = string.Format("photorss-{0}{1}", album.Userid, GeneralConfigs.GetConfig().Extname);
                else
                    photorssurl = string.Format("rss.aspx?uid={0}&type=photo", album.Userid);

                AddLinkRss(string.Format("tools/{0}", photorssurl), "最新图片");
            }

            pagetitle = album.Title;

            //权限验证部分,私有相册,不是相册所有者
            if (album.Type == 1 && album.Userid != userid)
            {
                if (ForumUtils.GetCookie("album" + albumid + "password") != Utils.MD5(album.Password))
                {
                    //首先验证Cookie中如果相册密码不正确，则要求输入密码，并以输入值验证
                    string password = DNTRequest.GetFormString("albumpassword");
                    if (album.Password == password)
                    {
                        ForumUtils.WriteCookie("album" + albumid + "password", Utils.MD5(password));
                        needpassword = false;
                    }
                }
                else
                    needpassword = false;
            }
            else
                needpassword = false;

            if (Utils.InArray(usergroupid.ToString(), config.Photomangegroups))
                needpassword = false;

            albumcategory = DTOProvider.GetAlbumCategory(album.Albumcateid);
            photoscount = DTOProvider.GetSpacePhotosCount(albumid);

            pageCount = photoscount % pageSize == 0 ? photoscount / pageSize : photoscount / pageSize + 1;

            if (pageCount == 0)
                pageCount = 1;

            if (currentpage < 1)
                currentpage = 1;

            if (currentpage > pageCount)
                currentpage = pageCount;

            pagenumbers = Utils.GetPageNumbers(currentpage, pageCount, string.Format("showalbum.aspx?albumid={0}", albumid), 8);
            photolist = DTOProvider.GetSpacePhotosInfo(DbProvider.GetInstance().SpacePhotosList(pageSize, currentpage, album.Userid, album.Albumid));

            foreach (PhotoInfo photo in photolist)
            {
                //当是远程照片时
                if (photo.Filename.IndexOf("http") < 0)
                    photo.Filename = forumpath + Globals.GetThumbnailImage(photo.Filename);
                else
                    photo.Filename = Globals.GetThumbnailImage(photo.Filename);
            }

            if (photolist.Count == 0)
                AddMsgLine("暂无图片");

            ForumUtils.WriteCookie("referer", string.Format("showalbum.aspx?albumid={0}&page={1}", albumid, currentpage));
        }
    }
}