using System;
using System.Data;
using Discuz.Common;
#if NET1
#else
using Discuz.Common.Generic; 
#endif
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Aggregation;

namespace Discuz.Space.Pages
{
    public class spaceindex : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 论坛新帖
        /// </summary>
        public DataTable newtopiclist;
        /// <summary>
        /// 论坛热帖
        /// </summary>
        public DataTable hottopiclist;
        /// <summary>
        /// 最近更新的空间
        /// </summary>
        public DataTable recentupdatespaceList;
        /// <summary>
        /// 推荐日志
        /// </summary>
        public SpaceShortPostInfo[] spacepostlist;
        /// <summary>
        /// 推荐空间列表
        /// </summary>
        public SpaceConfigInfoExt[] spaceconfigs;
        /// <summary>
        /// 公告数量
        /// </summary>
        public int announcementcount;
        /// <summary>
        /// 公告列表
        /// </summary>
        public DataTable announcementlist;
        /// <summary>
        /// 轮显图片数据
        /// </summary>
        public string rotatepicdata = "";
        /// <summary>
        /// 最多日志空间列表
        /// </summary>
        public DataTable topspacelistbypostcount;
        /// <summary>
        /// 最多评论空间列表
        /// </summary>
        public DataTable topspacelistbycommentcount;
        /// <summary>
        /// 最多访问空间列表
        /// </summary>
        public DataTable topspacelistbyvisitedtimes;
        /// <summary>
        /// 最新空间评论列表
        /// </summary>
        public DataTable topspacecomments;
        /// <summary>
        /// 最多评论日志列表
        /// </summary>
        public DataTable topspacepostcommentcount;
        /// <summary>
        /// 最多浏览日志列表
        /// </summary>
        public DataTable topspacepostviews;
        /// <summary>
        /// 推荐相册列表
        /// </summary>
        public List<AlbumInfo> recommendalbumlist = new List<AlbumInfo>();
        /// <summary>
        /// 相册聚合信息
        /// </summary>
        public PhotoAggregationInfo photoconfig = AggregationFacade.PhotoAggregation.GetPhotoAggregationInfo();
        #endregion

        protected override void ShowPage()
        {
            pagetitle = config.Spacename + "首页";

            if (config.Enablespace != 1)
            {
                AddErrLine("个人空间功能已被关闭");
                return;
            }

            if (config.Rssstatus == 1)
                AddLinkRss("tools/spacerss.aspx", "最新日志");

            newtopiclist = AggregationFacade.ForumAggregation.GetForumTopicList(10, 0, 0, TopicTimeType.All, TopicOrderType.PostDateTime, false, false);
            hottopiclist = AggregationFacade.ForumAggregation.GetForumTopicList(10, 0, 0, TopicTimeType.All, TopicOrderType.Views, false, false);
            recentupdatespaceList = AggregationFacade.SpaceAggregation.GetRecentUpdateSpaceList(AggregationConfig.GetConfig().RecentUpdateSpaceAggregationListCount);

            // 得到公告
            announcementlist = Announcements.GetSimplifiedAnnouncementList(nowdatetime, "2999-01-01 00:00:00");
            announcementcount = 0;
            if (announcementlist != null)
                announcementcount = announcementlist.Rows.Count;

            rotatepicdata = AggregationFacade.SpaceAggregation.GetRotatePicData();
            spacepostlist = AggregationFacade.SpaceAggregation.GetSpacePostList("Spaceindex");
            spaceconfigs = AggregationFacade.SpaceAggregation.GetSpaceListFromFile("Spaceindex");
            topspacelistbycommentcount = AggregationFacade.SpaceAggregation.GetTopSpaceListFromCache("commentcount");
            topspacelistbyvisitedtimes = AggregationFacade.SpaceAggregation.GetTopSpaceListFromCache("visitedtimes");
            topspacepostcommentcount = AggregationFacade.SpaceAggregation.GetTopSpacePostListFromCache("commentcount");
            topspacepostviews = AggregationFacade.SpaceAggregation.GetTopSpacePostListFromCache("views");
            topspacecomments = AggregationFacade.SpaceAggregation.GetSpaceTopComments();

            if (config.Enablealbum == 1)
                recommendalbumlist = AggregationFacade.AlbumAggregation.GetRecommandAlbumList("Spaceindex");            
        }
    }
}