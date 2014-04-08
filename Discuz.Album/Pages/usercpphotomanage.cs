using System;
using System.Data;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Album.Data;

namespace Discuz.Album.Pages
{
    /// <summary>
    /// 个人中心管理照片
    /// </summary>
    public class usercpphotomanage : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid = DNTRequest.GetInt("page", 1);
        /// <summary>
        /// 图片总数
        /// </summary>
        public int photocount = 0;
        /// <summary>
        /// 分页页数
        /// </summary>
        public int pagecount = 1;
        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers = "";
        /// <summary>
        /// 所属相册
        /// </summary>
        public int albumid = DNTRequest.GetInt("albumid", 0);
        /// <summary>
        /// 相册列表
        /// </summary>
        public DataTable albumList = new DataTable();
        /// <summary>
        /// 相册封面列表
        /// </summary>
        public DataTable logoList = new DataTable();
        /// <summary>
        /// 图片列表
        /// </summary>
        public DataTable photoList = new DataTable();
        /// <summary>
        /// 当前相册名称
        /// </summary>
        public string currentalbumname = "";
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user = new UserInfo();
        private int pagesize = 15;
        #endregion        

        protected override void ShowPage()
        {
            pagetitle = "用户控制面板";

            if (userid == -1)
            {
                AddErrLine("你尚未登录");
                return;
            }
            user = Users.GetUserInfo(userid);
            if (config.Enablealbum != 1)
            {
                AddErrLine("相册功能已被关闭");
                return;
            }

            AlbumInfo albuminfo = DTOProvider.GetAlbumInfo(albumid);
            if (this.userid != albuminfo.Userid)
            {
                AddErrLine("您无权管理该相册");
                return;
            }
            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }

                string active = DNTRequest.GetFormString("active");
                if (active == "delete")
                    DeletePhoto();
                else if (active == "change")
                    ChangeAlbum();
                else if (active == "setlogo")
                    SetLogo();
                return;
            }
            albumList = DbProvider.GetInstance().GetSpaceAlbumByUserId(userid);
            foreach (DataRow dr in albumList.Rows)
            {
                if (dr["albumid"].ToString() == albumid.ToString())
                {
                    currentalbumname = dr["title"].ToString().Trim();
                    albumList.Rows.Remove(dr);
                    break;
                }
            }

            //获取主题总数
            photocount = DbProvider.GetInstance().GetSpacePhotosCount(albumid);
            //获取总页数
            pagecount = photocount%pagesize == 0 ? photocount/pagesize : photocount/pagesize + 1;
            if (pagecount == 0)
                pagecount = 1;

            //修正请求页数中可能的错误
            if (pageid < 1)
                pageid = 1;
            if (pageid > pagecount)
                pageid = pagecount;

            //获取收入记录并分页显示
            logoList.Columns.Add("photoid");
            logoList.Columns.Add("PhotoOrder");
            photoList = DbProvider.GetInstance().SpacePhotosList(pagesize, pageid, userid, albumid);
            if (photoList.Rows.Count == 0)
            {
                DataRow dr = logoList.NewRow();
                dr["PhotoOrder"] = "无照片";
                dr["photoid"] = "-1";
                logoList.Rows.Add(dr);
                return;
            }
            int i = 1;
            photoList.Columns.Add("TFileName");
            photoList.Columns.Add("PhotoOrder");
            foreach (DataRow singlePhotoInfo in photoList.Rows)
            {
                //当是远程照片时
                if (singlePhotoInfo["FileName"].ToString().Trim().ToLower().IndexOf("http") == 0)
                    singlePhotoInfo["TFileName"] = Globals.GetThumbnailImage(singlePhotoInfo["FileName"].ToString().Trim());
                else
                    singlePhotoInfo["TFileName"] = Globals.GetThumbnailImage(BaseConfigs.GetForumPath + singlePhotoInfo["FileName"].ToString().Trim());

                DataRow dr = logoList.NewRow();
                dr["photoid"] = singlePhotoInfo["photoid"];
                singlePhotoInfo["photoorder"] = "NO." + i;
                dr["PhotoOrder"] = "NO." + i;
                logoList.Rows.Add(dr);
                i++;
            }
            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "usercpspacemanagephoto.aspx?albumid=" + albumid, 8);
        }

        private void DeletePhoto()
        {
            string photoIdList = DNTRequest.GetFormString("photoid");
            if(!Utils.IsNumericList(photoIdList))
                return;

            DbProvider.GetInstance().DeleteSpacePhotoByIDList(photoIdList, albumid, userid);
            AlbumInfo _AlbumInfo = DTOProvider.GetAlbumInfo(albumid);
            _AlbumInfo.Imgcount = DbProvider.GetInstance().GetSpacePhotoCountByAlbumId(albumid);
            DbProvider.GetInstance().SaveSpaceAlbum(_AlbumInfo);

            //生成json数据
            Albums.CreateAlbumJsonData(albumid);

            SetUrl("usercpspacemanagephoto.aspx?albumid=" + albumid);
            SetMetaRefresh();
            SetShowBackLink(true);
            AddMsgLine("照片删除完毕");
        }

        private void ChangeAlbum()
        {
            int targetAlbumId = TypeConverter.StrToInt(DNTRequest.GetFormString("alibumList"));
            int sourceAlbumId = albumid;
            string photoIdList = DNTRequest.GetFormString("photoid");
            if (!Utils.IsNumericList(photoIdList))
                return;

            if (DTOProvider.GetAlbumInfo(targetAlbumId).Userid != userid)
            {
                AddErrLine("您所选的目标相册不存在");
                return;
            }
            DbProvider.GetInstance().ChangeAlbum(targetAlbumId, photoIdList, userid);
            AlbumInfo _sourceAlbum = DTOProvider.GetAlbumInfo(sourceAlbumId);
            _sourceAlbum.Imgcount = DbProvider.GetInstance().GetSpacePhotoCountByAlbumId(sourceAlbumId);
            DbProvider.GetInstance().SaveSpaceAlbum(_sourceAlbum);

            AlbumInfo _targetAlbum =  DTOProvider.GetAlbumInfo(targetAlbumId);
            _targetAlbum.Imgcount = DbProvider.GetInstance().GetSpacePhotoCountByAlbumId(targetAlbumId);
            DbProvider.GetInstance().SaveSpaceAlbum(_targetAlbum);
            //生成json数据
            Albums.CreateAlbumJsonData(albumid);

            SetUrl("usercpspacemanagephoto.aspx?albumid=" + albumid);
            SetMetaRefresh();
            SetShowBackLink(true);
            AddMsgLine("照片转移完毕");
        }

        private void SetLogo()
        {
            int photoId = Convert.ToInt32(DNTRequest.GetFormString("logoList"));
            if (photoId == -1)
                return;
            PhotoInfo _spacephotoinfo = DTOProvider.GetPhotoInfo(DbProvider.GetInstance().GetPhotoByID(photoId, 0, 0));
            AlbumInfo _AlbumInfo = DTOProvider.GetAlbumInfo(albumid);
            _AlbumInfo.Logo = Globals.GetThumbnailImage(_spacephotoinfo.Filename);
            DbProvider.GetInstance().SaveSpaceAlbum(_AlbumInfo);

            SetUrl("usercpspacemanagephoto.aspx?albumid=" + albumid);
            SetMetaRefresh();
            SetShowBackLink(true);
            AddMsgLine("封面已经设置成功");
        }
    }
}