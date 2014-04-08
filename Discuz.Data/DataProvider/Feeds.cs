using System;
using System.Data;
using System.Text;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Data
{
    /// <summary>
    /// Feed操作类
    /// </summary>
    public class Feeds
    {
        private static GeneralConfigInfo config
        { 
            get { return GeneralConfigs.GetConfig(); } 
        }

        private static string forumurl = "http://" + DNTRequest.GetCurrentFullHost() + BaseConfigs.GetForumPath.ToLower();
        /// <summary>
        /// 构建RSS输出内容
        /// </summary>
        /// <param name="ttl">TTL数值</param>
        /// <param name="fidList">版块id列表</param>
        /// <param name="forumname">版块名称</param>
        /// <returns></returns>
        public static string BuildRssOutput(int ttl, string fidList, string forumname)
        {
            StringBuilder sbRss = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
            IDataReader reader = DatabaseProvider.GetInstance().GetNewTopics(fidList, PostTables.GetPostTableId());

            sbRss.Append("<?xml-stylesheet type=\"text/xsl\" href=\"rss.xsl\" media=\"screen\"?>\r\n");
            sbRss.Append("<rss version=\"2.0\">\r\n");
            sbRss.Append("  <channel>\r\n");
            sbRss.AppendFormat("    <title>{0}{1}</title>\r\n", Utils.HtmlEncode(config.Forumtitle), !Utils.StrIsNullOrEmpty(forumname) ? " - " + Utils.HtmlEncode(forumname) : "");
            sbRss.AppendFormat("    <link>{0}", forumurl);

            if (Utils.IsNumeric(fidList))
            {
                if (config.Aspxrewrite == 1)
                    sbRss.AppendFormat("showforum-{0}{1}", fidList, config.Extname);
                else
                    sbRss.AppendFormat("showforum.aspx?forumid={0}", fidList);
            }

            sbRss.Append("</link>\r\n");
            sbRss.Append("    <description>Latest 20 threads</description>\r\n");
            sbRss.AppendFormat("    <copyright>Copyright (c) {0}</copyright>\r\n", Utils.HtmlEncode(config.Forumtitle));
            sbRss.Append("    <generator>Discuz!NT</generator>\r\n");
            sbRss.AppendFormat("    <pubDate>{0}</pubDate>\r\n", DateTime.Now.ToString("r"));
            sbRss.AppendFormat("    <ttl>{0}</ttl>\r\n", ttl.ToString());

            if (reader != null)
            {
                while (reader.Read())
                {
                    sbRss.Append("    <item>\r\n");
                    sbRss.AppendFormat("      <title>{0}</title>\r\n", Utils.HtmlEncode(reader["title"].ToString().Trim()));
                    sbRss.Append("    <description><![CDATA[");
                    if (reader["message"].ToString().IndexOf("[hide]") > -1)
                        sbRss.Append("***内容隐藏***");
                    else
                        sbRss.Append(Utils.HtmlEncode(Utils.GetSubString(Utils.ClearUBB(reader["message"].ToString()), 200, "......")));

                    sbRss.Append("]]></description>\r\n");
                    sbRss.AppendFormat("      <link>{0}", Utils.HtmlEncode(forumurl));
                    if (config.Aspxrewrite == 1)
                        sbRss.AppendFormat("showtopic-{0}{1}", reader["tid"].ToString(), config.Extname);
                    else
                        sbRss.AppendFormat("showtopic.aspx?topicid={0}", reader["tid"].ToString());

                    sbRss.Append("</link>\r\n");
                    if (!Utils.IsNumeric(fidList))
                    {
                        sbRss.AppendFormat("      <category>{0}</category>\r\n", Utils.HtmlEncode(reader["name"].ToString().Trim()));
                    }
                    sbRss.AppendFormat("      <author>{0}</author>\r\n", Utils.HtmlEncode(reader["poster"].ToString().Trim()));
                    sbRss.AppendFormat("      <pubDate>{0}</pubDate>\r\n", Utils.HtmlEncode(Convert.ToDateTime(reader["postdatetime"]).ToString("r").Trim()));
                    sbRss.Append("    </item>\r\n");
                }
                reader.Close();
            }
            return sbRss.Append("  </channel>\r\n</rss>").ToString();
        }

        /// <summary>
        /// 获得百度论坛收录协议xml
        /// </summary>
        /// <param name="ttl">TTL数值</param>
        /// <returns></returns>
        public static string GetBaiduSitemap(string sbforumlist, ShortUserInfo master)
        {
            StringBuilder sitemapBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
            IDataReader reader = DatabaseProvider.GetInstance().GetSitemapNewTopics(sbforumlist.ToString());

            sitemapBuilder.Append("<document xmlns:bbs=\"http://www.baidu.com/search/bbs_sitemap.xsd\">\r\n");
            sitemapBuilder.AppendFormat("  <webSite>{0}</webSite>\r\n", forumurl);
            sitemapBuilder.AppendFormat("  <webMaster>{0}</webMaster>\r\n", master != null ? master.Email : "");
            sitemapBuilder.AppendFormat("  <updatePeri>{0}</updatePeri>\r\n", config.Sitemapttl);
            sitemapBuilder.AppendFormat("  <updatetime>{0}</updatetime>\r\n", DateTime.Now.ToString("r"));
            sitemapBuilder.AppendFormat("  <version>Discuz!NT {0}</version>\r\n", Utils.GetAssemblyVersion());

            if (reader != null)
            {
                while (reader.Read())
                {
                    sitemapBuilder.Append("    <item>\r\n");
                    sitemapBuilder.AppendFormat("      <link>{0}", Utils.HtmlEncode(forumurl));
                    if (config.Aspxrewrite == 1)
                        sitemapBuilder.AppendFormat("showtopic-{0}{1}", reader["tid"], config.Extname);
                    else
                        sitemapBuilder.AppendFormat("showtopic-{0}", reader["tid"]);

                    sitemapBuilder.Append("      </link>\r\n");
                    sitemapBuilder.AppendFormat("      <title>{0}</title>\r\n", Utils.HtmlEncode(reader["title"].ToString().Trim()));
                    sitemapBuilder.AppendFormat("      <pubDate>{0}</pubDate>\r\n", Utils.HtmlEncode(reader["postdatetime"].ToString().Trim()));
                    sitemapBuilder.AppendFormat("      <bbs:lastDate>{0}</bbs:lastDate>\r\n", reader["lastpost"]);
                    sitemapBuilder.AppendFormat("      <bbs:reply>{0}</bbs:reply>\r\n", reader["replies"]);
                    sitemapBuilder.AppendFormat("      <bbs:hit>{0}</bbs:hit>\r\n", reader["views"]);
                    sitemapBuilder.AppendFormat("      <bbs:boardid>{0}</bbs:boardid>\r\n", reader["fid"]);
                    sitemapBuilder.AppendFormat("      <bbs:pick>{0}</bbs:pick>\r\n", reader["digest"]);
                    sitemapBuilder.Append("    </item>\r\n");
                }
                reader.Close();
                sitemapBuilder.Append("</document>");
            }
            else
            {
                sitemapBuilder.Length = 0;
                sitemapBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
                sitemapBuilder.Append("<document>Error</document>\r\n");
            }
            return sitemapBuilder.ToString();
        }
    }
}
