using System;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Aggregation;
using System.Xml;
using Discuz.Album.Data;

namespace Discuz.Album.Pages
{
	/// <summary>
	/// 相册聚合首页
	/// </summary>
	public class albumindex : PageBase
    {
        #region 变量声明
        /// <summary>
        /// 公告列表
        /// </summary>
        public DataTable announcementlist;
        /// <summary>
        /// 公告数量
        /// </summary>
        public int announcementcount = 0;
        /// <summary>
        /// 推荐图片列表
        /// </summary>
        public string rotatepicdata = "";
        /// <summary>
        /// 推荐图片列表
        /// </summary>
        public Discuz.Common.Generic.List<PhotoInfo> recommendphotolist;
        /// <summary>
        /// 焦点图片列表
        /// </summary>
        public Discuz.Common.Generic.List<PhotoInfo> focusphotolist ;
        /// <summary>
        /// 推荐相册列表
        /// </summary>
        public Discuz.Common.Generic.List<AlbumInfo> recommendalbumlist;
        /// <summary>
        /// 焦点相册列表
        /// </summary>
        public Discuz.Common.Generic.List<AlbumInfo> focusalbumlist;
        /// <summary>
        /// 一周热门图片列表
        /// </summary>
        public Discuz.Common.Generic.List<PhotoInfo> weekhotphotolist ;
        /// <summary>
        /// 相册分类列表
        /// </summary>
        public Discuz.Common.Generic.List<AlbumCategoryInfo> albumcategorylist;
        /// <summary>
        /// 相册聚合设置信息
        /// </summary>
        public PhotoAggregationInfo photoconfig = AggregationFacade.PhotoAggregation.GetPhotoAggregationInfo();
        /// <summary>
        /// 最近更新的个人空间列表
        /// </summary>
        public DataTable recentupdatespaceList;
        #endregion 变量声明

		protected override void ShowPage()
		{
            pagetitle = config.Albumname + "首页";
            
            if (config.Enablealbum != 1)
            {
                AddErrLine("相册功能已被关闭");
                return;
            }
            
            if (config.Rssstatus == 1)
                AddLinkRss("tools/photorss.aspx", "最新图片");

            announcementlist = Announcements.GetAnnouncementList(Utils.GetDateTime(), "2999-01-01 00:00:00");

            if (announcementlist != null)
                announcementcount = announcementlist.Rows.Count;

            //轮显图片
            rotatepicdata = AggregationFacade.AlbumAggregation.GetRotatePicData();

            //推荐图片
            recommendphotolist = AggregationFacade.AlbumAggregation.GetRecommandPhotoList("Albumindex");
            //焦点图片
            focusphotolist = AggregationFacade.AlbumAggregation.GetFocusPhotoList(photoconfig.Focusphotoshowtype, photoconfig.Focusphotocount, photoconfig.Focusphotodays);
            //推荐相册
            recommendalbumlist = AggregationFacade.AlbumAggregation.GetRecommandAlbumList("Albumindex");
            //焦点相册
            focusalbumlist = AggregationFacade.AlbumAggregation.GetAlbumList(photoconfig.Focusalbumshowtype, photoconfig.Focusalbumcount, photoconfig.Focusalbumdays);
            //一周热图总排行
            weekhotphotolist = AggregationFacade.AlbumAggregation.GetWeekHotPhotoList(photoconfig.Weekhot);
            //相册分类
            albumcategorylist = DTOProvider.GetAlbumCategory();

            if (config.Enablespace == 1)
                recentupdatespaceList = AggregationFacade.SpaceAggregation.GetRecentUpdateSpaceList(AggregationConfig.GetConfig().RecentUpdateSpaceAggregationListCount);
		}
	}
}
