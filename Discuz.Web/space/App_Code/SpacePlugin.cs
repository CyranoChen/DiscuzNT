using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Plugin.Space;
using Discuz.Entity;
using Discuz.Space.Provider;
using Discuz.Space.Utilities;
using Discuz.Space.Data;
using Discuz.Forum;

namespace Discuz.Space
{
    public class SpacePlugin : SpacePluginBase
    {
        public override string SpaceHotTagJSONPCacheFileName
        {
            get { return SpaceTags.SpaceHotTagJSONPCacheFileName; }
        }

        public override SpacePostInfo GetSpacepostsInfo(int blogid)
        {
            return BlogProvider.GetSpacepostsInfo(blogid);
        }

        public override void WriteHotTagsListForSpaceJSONPCacheFile(int count)
        {
            SpaceTags.WriteHotTagsListForSpaceJSONPCacheFile(count);
        }

        public override void GetSpacePostTagsCacheFile(int postid)
        {
            SpaceTags.WriteSpacePostTagsCacheFile(postid);
        }

        public override int CheckSpaceRewriteNameAvailable(string rewriteName)
        {
            return Globals.CheckSpaceRewriteNameAvailable(rewriteName);
        }

        public override int GetSpacePostCountWithSameTag(int tagid)
        {
            return Spaces.GetSpacePostCountWithSameTag(tagid);
        }

        public override Common.Generic.List<Discuz.Entity.SpacePostInfo> GetSpacePostsWithSameTag(int tagid, int pageid, int tpp)
        {
            return Spaces.GetSpacePostsWithSameTag(tagid, pageid, tpp);
        }

        public override System.Data.DataTable GetWebSiteAggRecentUpdateSpaceList(int count)
        {
            return DbProvider.GetInstance().GetWebSiteAggRecentUpdateSpaceList(count);
        }

        public override System.Data.DataTable GetWebSiteAggTopSpaceList(string orderby, int topnumber)
        {
            return DbProvider.GetInstance().GetWebSiteAggTopSpaceList(orderby, topnumber);
        }

        public override string[] GetSpaceLastPostInfo(int userid)
        {
            return DbProvider.GetInstance().GetSpaceLastPostInfo(userid);
        }

        public override System.Data.DataTable GetWebSiteAggTopSpacePostList(string orderby, int topnumber)
        {
            return DbProvider.GetInstance().GetWebSiteAggTopSpacePostList(orderby, topnumber);
        }

        public override System.Data.DataTable GetWebSiteAggSpacePostsList(int pageSize, int currentPage)
        {
            return DbProvider.GetInstance().GetWebSiteAggSpacePostsList(pageSize, currentPage);
        }

        public override int GetWebSiteAggSpacePostsCount()
        {
            return DbProvider.GetInstance().GetWebSiteAggSpacePostsCount();
        }

        public override System.Data.DataTable GetWebSiteAggSpaceTopComments(int topnumber)
        {
            return DbProvider.GetInstance().GetWebSiteAggSpaceTopComments(topnumber);
        }

        protected override void OnTopicCreated(TopicInfo topic, PostInfo post, AttachmentInfo[] attachs)
        {
            SpacePostInfo spacepost = new SpacePostInfo();
            spacepost.Author = post.Poster;
            string content = Posts.GetPostMessageHTML(post, attachs);
            spacepost.Category = "";
            spacepost.Content = content;
            spacepost.Postdatetime = DateTime.Now;
            spacepost.PostStatus = 1;
            spacepost.PostUpDateTime = DateTime.Now;
            spacepost.Title = post.Title;
            spacepost.Uid = post.Posterid;

            DbProvider.GetInstance().AddSpacePost(spacepost);
        }

        public override System.Data.DataTable GetWebSiteAggSpacePostList(int count)
        {
            return DbProvider.GetInstance().GetWebSiteAggSpacePostList(count);
        }

        protected override System.Data.DataTable GetSearchResult(int pagesize, string idstr)
        {
            return DbProvider.GetInstance().GetSearchSpacePostsList(pagesize, idstr);
        }

        protected override string GetFeedXML(int ttl, int uid)
        {
            return SpaceFeeds.GetBlogRss(ttl, uid);
        }

        protected override string GetFeedXML(int ttl)
        {
            return SpaceFeeds.GetBlogAggRss(ttl);
        }

        protected override void OnUserDeleted(int userid)
        {
            Spaces.DeleteSpace(userid);
        }
    }
}
