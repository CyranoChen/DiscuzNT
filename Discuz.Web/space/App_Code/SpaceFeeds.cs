using System;
using System.Text;
using Discuz.Common;
using Discuz.Config;
using Discuz.Space.Provider;
using Discuz.Entity;
using Discuz.Forum;
using System.Data;
using Discuz.Plugin.Album;

namespace Discuz.Space
{
    public class SpaceFeeds
    {
        /// <summary>
        /// 获得日志的RSS
        /// </summary>
        /// <param name="ttl">Time To Live</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static string GetBlogAggRss(int ttl)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            //Discuz.Cache.ICacheStrategy ics = new RssCacheStrategy();
            //ics.TimeOut = ttl * 60;
            //cache.LoadCacheStrategy(ics);

            string str = cache.RetrieveObject("/SPACE/RSS/Index") as string;
            if (str != null)
            {
                return str;
            }

            string forumurl = "http://" + DNTRequest.GetCurrentFullHost() + BaseConfigs.GetForumPath.ToLower();
            DataTable dt = Data.DbProvider.GetInstance().GetWebSiteAggSpacePostsList(20, 1);
            if (dt == null || dt.Rows.Count < 1)
            {
                return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<Rss>Specified blog not found</Rss>\r\n";
            }
            StringBuilder rssBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
            rssBuilder.Append("<?xml-stylesheet type=\"text/xsl\" href=\"rss.xsl\" media=\"screen\"?>\r\n");
            rssBuilder.Append("<rss version=\"2.0\">\r\n");
            rssBuilder.Append("  <channel>\r\n");
            rssBuilder.Append("    <title>");
            rssBuilder.Append(Utils.HtmlEncode(GeneralConfigs.GetConfig().Forumtitle));
            rssBuilder.Append(" - ");
            rssBuilder.Append("最新空间日志");
            rssBuilder.Append("</title>\r\n");
            rssBuilder.Append("    <link>");
            rssBuilder.Append(forumurl);
            rssBuilder.Append("spaceindex.aspx");
            rssBuilder.Append("</link>\r\n");
            rssBuilder.AppendFormat("    <description>Latest {0} blogs</description>\r\n", dt.Rows.Count);
            rssBuilder.Append("    <copyright>Copyright (c) ");
            rssBuilder.Append(Utils.HtmlEncode(GeneralConfigs.GetConfig().Forumtitle));
            rssBuilder.Append("</copyright>\r\n");
            rssBuilder.Append("    <generator>");
            rssBuilder.Append("Discuz!NT");
            rssBuilder.Append("</generator>\r\n");
            rssBuilder.Append("    <pubDate>");
            rssBuilder.Append(DateTime.Now.ToString("r"));
            rssBuilder.Append("</pubDate>\r\n");
            rssBuilder.AppendFormat("    <ttl>{0}</ttl>\r\n", ttl.ToString());
            foreach (DataRow dr in dt.Rows)
            {
                rssBuilder.Append("    <item>\r\n");
                rssBuilder.Append("      <title>");
                rssBuilder.Append(Utils.HtmlEncode(dr["title"].ToString().Trim()));
                rssBuilder.Append("</title>\r\n");
                rssBuilder.Append("      <description><![CDATA[");
                rssBuilder.Append(Utils.HtmlEncode(Utils.GetSubString(Utils.RemoveHtml(dr["content"].ToString()), 200, "......")));
                rssBuilder.Append("]]></description>\r\n");
                rssBuilder.Append("      <link>");
                rssBuilder.Append(Utils.HtmlEncode(forumurl));
                rssBuilder.Append("space/viewspacepost.aspx?postid=");
                rssBuilder.Append(dr["postid"].ToString());
                rssBuilder.Append("</link>\r\n");
                rssBuilder.Append("      <category>");
                rssBuilder.Append(Utils.HtmlEncode(dr["category"].ToString()));
                rssBuilder.Append("</category>\r\n");
                rssBuilder.Append("      <author>");
                rssBuilder.Append(Utils.HtmlEncode(dr["author"].ToString().Trim()));
                rssBuilder.Append("</author>\r\n");
                rssBuilder.Append("      <pubDate>");
                rssBuilder.Append(Utils.HtmlEncode(Convert.ToDateTime(dr["postdatetime"]).ToString("r").Trim()));
                rssBuilder.Append("</pubDate>\r\n");
                rssBuilder.Append("    </item>\r\n");
            }
            rssBuilder.Append("  </channel>\r\n");
            rssBuilder.Append("</rss>");

            cache.AddObject("/SPACE/RSS/Index", rssBuilder.ToString(), ttl * 60);
            //cache.LoadDefaultCacheStrategy();

            return rssBuilder.ToString();
        }

        /// <summary>
        /// 获得指定用户日志的RSS
        /// </summary>
        /// <param name="ttl">Time To Live</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static string GetBlogRss(int ttl, int uid)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            //Discuz.Cache.ICacheStrategy ics = new RssCacheStrategy();
            //ics.TimeOut = ttl * 60;
            //cache.LoadCacheStrategy(ics);

            string str = cache.RetrieveObject("/SPACE/RSS/Blog" + uid) as string;
            if (str != null)
                return str;

            string forumurl = "http://" + DNTRequest.GetCurrentFullHost() + BaseConfigs.GetForumPath.ToLower();
            ShortUserInfo user = Users.GetShortUserInfo(uid);
            if (user == null || user.Uid < 1 || user.Uid != uid)
            {
                return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<Rss>Specified user not found</Rss>\r\n";
            }
            SpacePostInfo[] posts = BlogProvider.GetSpacepostsInfo(Space.Data.DbProvider.GetInstance().SpacePostsList(20, 1, uid, 1));
            if (posts == null || posts.Length < 1)
            {
                return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<Rss>Specified user's blog is not found</Rss>\r\n";
            }
            StringBuilder rssBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
            rssBuilder.Append("<?xml-stylesheet type=\"text/xsl\" href=\"rss.xsl\" media=\"screen\"?>\r\n");
            rssBuilder.Append("<rss version=\"2.0\">\r\n");
            rssBuilder.Append("  <channel>\r\n");
            rssBuilder.Append("    <title>");
            rssBuilder.Append(Utils.HtmlEncode(GeneralConfigs.GetConfig().Forumtitle));
            rssBuilder.Append(" - ");
            rssBuilder.Append(user.Username);
            rssBuilder.Append("的个人空间");
            rssBuilder.Append("</title>\r\n");
            rssBuilder.Append("    <link>");
            rssBuilder.Append(forumurl);
            rssBuilder.Append("space/?uid=");
            rssBuilder.Append(uid.ToString());
            rssBuilder.Append("</link>\r\n");
            rssBuilder.AppendFormat("    <description>Latest {0} blogs</description>\r\n", posts.Length);
            rssBuilder.Append("    <copyright>Copyright (c) ");
            rssBuilder.Append(Utils.HtmlEncode(GeneralConfigs.GetConfig().Forumtitle));
            rssBuilder.Append("</copyright>\r\n");
            rssBuilder.Append("    <generator>");
            rssBuilder.Append("Discuz!NT");
            rssBuilder.Append("</generator>\r\n");
            rssBuilder.Append("    <pubDate>");
            rssBuilder.Append(DateTime.Now.ToString("r"));
            rssBuilder.Append("</pubDate>\r\n");
            rssBuilder.AppendFormat("    <ttl>{0}</ttl>\r\n", ttl.ToString());
            foreach (SpacePostInfo post in posts)
            {
                rssBuilder.Append("    <item>\r\n");
                rssBuilder.Append("      <title>");
                rssBuilder.Append(Utils.HtmlEncode(post.Title.ToString().Trim()));
                rssBuilder.Append("</title>\r\n");
                rssBuilder.Append("      <description><![CDATA[");
                rssBuilder.Append(Utils.HtmlEncode(Utils.GetSubString(Utils.RemoveHtml(post.Content), 200, "......")));
                rssBuilder.Append("]]></description>\r\n");
                rssBuilder.Append("      <link>");
                rssBuilder.Append(Utils.HtmlEncode(forumurl));
                rssBuilder.Append("space/viewspacepost.aspx?postid=");
                rssBuilder.Append(post.Postid.ToString());
                rssBuilder.Append("</link>\r\n");
                rssBuilder.Append("      <category>");
                rssBuilder.Append(Utils.HtmlEncode(post.Category));
                rssBuilder.Append("</category>\r\n");
                rssBuilder.Append("      <author>");
                rssBuilder.Append(Utils.HtmlEncode(post.Author.ToString().Trim()));
                rssBuilder.Append("</author>\r\n");
                rssBuilder.Append("      <pubDate>");
                rssBuilder.Append(Utils.HtmlEncode(post.Postdatetime.ToString("r").Trim()));
                rssBuilder.Append("</pubDate>\r\n");
                rssBuilder.Append("    </item>\r\n");
            }
            rssBuilder.Append("  </channel>\r\n");
            rssBuilder.Append("</rss>");

            cache.AddObject("/SPACE/RSS/Blog" + uid, rssBuilder.ToString(), ttl * 60);
            //cache.LoadDefaultCacheStrategy();

            return rssBuilder.ToString();
        }

    }
}
