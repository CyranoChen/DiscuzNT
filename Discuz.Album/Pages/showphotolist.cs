using System.Data;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Aggregation;
using Discuz.Album.Data;

namespace Discuz.Album.Pages
{
	/// <summary>
	/// 图片/相册排行榜
	/// </summary>
	public class showphotolist : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 相册列表
        /// </summary>
        public Discuz.Common.Generic.List<AlbumInfo> albumlist = new Discuz.Common.Generic.List<AlbumInfo>();
        /// <summary>
        /// 图片列表
        /// </summary>
        public Discuz.Common.Generic.List<PhotoInfo> photolist = new Discuz.Common.Generic.List<PhotoInfo>();
        /// <summary>
        /// 一周热门图片列表
        /// </summary>
        public Discuz.Common.Generic.List<PhotoInfo> weekhotphotolist = new Discuz.Common.Generic.List<PhotoInfo>();
        /// <summary>
        /// 相册分类列表
        /// </summary>
        public Discuz.Common.Generic.List<AlbumCategoryInfo> albumcategorylist = new Discuz.Common.Generic.List<AlbumCategoryInfo>();
        /// <summary>
        /// 查看方式,0=最多浏览,1=最多评论,2=最新图片,3=最多收藏
        /// </summary>
        public int type = DNTRequest.GetInt("type", 0);
        /// <summary>
        /// 图片数量
        /// </summary>
        public int photocount = 10;
        /// <summary>
        /// 公告列表
        /// </summary>
        public DataTable announcementlist;
        /// <summary>
        /// 公告数量
        /// </summary>
        public int announcementcount = 0;
        /// <summary>
        /// 图片聚合信息
        /// </summary>
        public PhotoAggregationInfo photoconfig = AggregationFacade.PhotoAggregation.GetPhotoAggregationInfo();
        /// <summary>
        /// 最近更新空间列表
        /// </summary>
        public DataTable recentupdatespaceList;
        /// <summary>
        /// 相册弹出导航菜单HTML代码
        /// </summary>
        public string navhomemenu = Albums.GetPhotoListMenuDivCache();
        #endregion

        protected override void ShowPage()
		{
            if (config.Enablealbum != 1)
            {
                AddErrLine("相册功能已被关闭");
                return;
            }
            pagetitle = "图片排行";
            announcementlist = Announcements.GetAnnouncementList(Utils.GetDateTime(), "2999-01-01 00:00:00");
            if (announcementlist != null)
                announcementcount = announcementlist.Rows.Count;

            type = type < 0 ? 0 : type;
            type = type > 3 ? 3 : type;

            if (type < 3)
                photolist = DTOProvider.GetPhotoRankList(type, photocount);
            else
                albumlist = DTOProvider.GetAlbumRankList(photocount);

            //一周热图总排行
            weekhotphotolist = AggregationFacade.AlbumAggregation.GetWeekHotPhotoList(photoconfig.Weekhot);
            //相册分类
            albumcategorylist = DTOProvider.GetAlbumCategory();
            recentupdatespaceList = AggregationFacade.SpaceAggregation.GetRecentUpdateSpaceList(AggregationConfig.GetConfig().RecentUpdateSpaceAggregationListCount);
		}
	}
}
