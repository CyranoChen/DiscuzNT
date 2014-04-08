using System;
using System.Data;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Aggregation;

namespace Discuz.Space.Pages
{
    public class bloglist : PageBase
    {
        public DataTable newtopiclist;

        public DataTable hottopiclist;

        public DataTable recentupdatespacelist;

        public int announcementcount;

        public DataTable announcementlist;

        public string rotatepicdata = "";

        public SpaceShortPostInfo[] spacepostlist;

        public int blogsCount;

        private int pageSize = 16;

        public DataTable postslist;

        public int currentpage;

        public int pagecount;

        public string pagenumbers;

        public PhotoAggregationInfo photoconfig = AggregationFacade.PhotoAggregation.GetPhotoAggregationInfo();

        public DataTable topspacecomments;

        public Discuz.Common.Generic.List<AlbumInfo> recommendalbumlist = new Discuz.Common.Generic.List<AlbumInfo>();

        protected override void ShowPage()
        {
            pagetitle = "日志列表";

            if (config.Enablespace != 1)
            {
                AddErrLine("个人空间功能已被关闭");
                return;
            }
  
            newtopiclist = AggregationFacade.ForumAggregation.GetForumTopicList(10, 0, 0, TopicTimeType.All, TopicOrderType.PostDateTime, false, false);
            hottopiclist = AggregationFacade.ForumAggregation.GetForumTopicList(10, 0, 0, TopicTimeType.All, TopicOrderType.Views, false, false);
            recentupdatespacelist = AggregationFacade.SpaceAggregation.GetRecentUpdateSpaceList(AggregationConfig.GetConfig().RecentUpdateSpaceAggregationListCount);

            // 得到公告
            announcementlist = Announcements.GetSimplifiedAnnouncementList(nowdatetime, "2999-01-01 00:00:00");
            announcementcount = 0;
            if (announcementlist != null)
                announcementcount = announcementlist.Rows.Count;

            rotatepicdata = AggregationFacade.BaseAggregation.GetRotatePicData();
            currentpage = DNTRequest.GetInt("page", 1);
            blogsCount = AggregationFacade.SpaceAggregation.GetSpacePostsCount();

            pagecount = blogsCount % pageSize == 0 ? blogsCount / pageSize : blogsCount / pageSize + 1;

            if (pagecount == 0)
                pagecount = 1;

            if (currentpage < 1)
                currentpage = 1;

            if (currentpage > pagecount)
                currentpage = pagecount;

            spacepostlist = AggregationFacade.SpaceAggregation.GetSpacePostList("Spaceindex");
            pagenumbers = Utils.GetPageNumbers(currentpage, pagecount, "bloglist.aspx", 8);
            postslist = AggregationFacade.SpaceAggregation.SpacePostsList(pageSize, currentpage);

            if (config.Enablealbum == 1)
                recommendalbumlist = AggregationFacade.AlbumAggregation.GetRecommandAlbumList("Spaceindex");

            topspacecomments = AggregationFacade.SpaceAggregation.GetSpaceTopComments();
        }
    }
}
