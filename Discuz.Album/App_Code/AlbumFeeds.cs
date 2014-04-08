using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Discuz.Common.Generic;
using Discuz.Entity;
using Discuz.Common;
using Discuz.Config;
using Discuz.Forum;
using System.Drawing;
using Discuz.Album.Data;
using Discuz.Cache;

namespace Discuz.Album
{
    public class AlbumFeeds
    {
        /// <summary>
        /// 获得图片的RSS
        /// </summary>
        /// <param name="ttl">Time To Live</param>
        /// <returns></returns>
        public static string GetPhotoRss(int ttl)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            //Discuz.Cache.ICacheStrategy ics = new RssCacheStrategy();
            //ics.TimeOut = ttl * 60;
            //cache.LoadCacheStrategy(ics);

            string str = cache.RetrieveObject("/PHOTO/RSS/Index") as string;
            if (str != null)
                return str;

            string forumurl = "http://" + DNTRequest.GetCurrentFullHost() + BaseConfigs.GetForumPath.ToLower();
            IDataReader reader = DbProvider.GetInstance().GetFocusPhotoList(2, 20, 100);
            if (reader == null)
                return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<Rss>Specified photos not found</Rss>\r\n";

            StringBuilder rssBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
            rssBuilder.Append("<?xml-stylesheet type=\"text/xsl\" href=\"rss.xsl\" media=\"screen\"?>\r\n");
            rssBuilder.Append("<rss version=\"2.0\">\r\n");
            rssBuilder.Append("  <channel>\r\n");
            rssBuilder.Append("    <title>");
            rssBuilder.Append(Utils.HtmlEncode(GeneralConfigs.GetConfig().Forumtitle));
            rssBuilder.Append(" - ");
            rssBuilder.Append("最新照片");
            rssBuilder.Append("</title>\r\n");
            rssBuilder.Append("    <link>");
            rssBuilder.Append(forumurl);
            rssBuilder.Append("spaceindex.aspx");
            rssBuilder.Append("</link>\r\n");
            rssBuilder.AppendFormat("    <description>Latest 20 photos</description>\r\n");
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
            while (reader.Read())
            {
                rssBuilder.Append("    <item>\r\n");
                rssBuilder.Append("      <title>");
                rssBuilder.Append(Utils.HtmlEncode(reader["title"].ToString().Trim()));
                rssBuilder.Append("</title>\r\n");
                rssBuilder.Append("      <description><![CDATA[<img src=\"");
                rssBuilder.Append(Utils.HtmlEncode(forumurl));
                rssBuilder.Append(reader["filename"].ToString().Trim());
                rssBuilder.Append("\"/>]]></description>\r\n");
                rssBuilder.Append("      <link>");
                rssBuilder.Append(Utils.HtmlEncode(forumurl));
                rssBuilder.Append("showphoto.aspx?photoid=");
                rssBuilder.Append(reader["photoid"].ToString());
                rssBuilder.Append("</link>\r\n");
                rssBuilder.Append("      <category>");
                rssBuilder.Append("</category>\r\n");
                rssBuilder.Append("      <author>");
                rssBuilder.Append(Utils.HtmlEncode(reader["username"].ToString().Trim()));
                rssBuilder.Append("</author>\r\n");
                rssBuilder.Append("      <pubDate>");
                rssBuilder.Append(Utils.HtmlEncode(Convert.ToDateTime(reader["postdate"]).ToString("r").Trim()));
                rssBuilder.Append("</pubDate>\r\n");
                rssBuilder.Append("    </item>\r\n");
            }
            reader.Close();
            rssBuilder.Append("  </channel>\r\n");
            rssBuilder.Append("</rss>");

            cache.AddObject("/PHOTO/RSS/Index", rssBuilder.ToString(), ttl * 60);
            //cache.LoadDefaultCacheStrategy();

            return rssBuilder.ToString();
        }

        /// <summary>
        /// 获得指定用户图片的RSS
        /// </summary>
        /// <param name="ttl">Time To Live</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static string GetPhotoRss(int ttl, int uid)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            //Discuz.Cache.ICacheStrategy ics = new RssCacheStrategy();
            //ics.TimeOut = ttl * 60;
            //cache.LoadCacheStrategy(ics);

            string str = cache.RetrieveObject("/PHOTO/RSS/Photo" + uid) as string;
            if (str != null)
                return str;

            string forumurl = "http://" + DNTRequest.GetCurrentFullHost() + BaseConfigs.GetForumPath.ToLower();
            IDataReader reader = DbProvider.GetInstance().GetFocusPhotoList(2, 20, 100, uid);

            if (reader == null)
                return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<Rss>Specified photos not found</Rss>\r\n";

            StringBuilder rssBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
            rssBuilder.Append("<?xml-stylesheet type=\"text/xsl\" href=\"rss.xsl\" media=\"screen\"?>\r\n");
            rssBuilder.Append("<rss version=\"2.0\">\r\n");
            rssBuilder.Append("  <channel>\r\n");
            rssBuilder.Append("    <title>");
            rssBuilder.Append(Utils.HtmlEncode(GeneralConfigs.GetConfig().Forumtitle));
            rssBuilder.Append(" - ");
            rssBuilder.Append("最新照片");
            rssBuilder.Append("</title>\r\n");
            rssBuilder.Append("    <link>");
            rssBuilder.Append(forumurl);
            rssBuilder.Append("spaceindex.aspx");
            rssBuilder.Append("</link>\r\n");
            rssBuilder.AppendFormat("    <description>Latest 20 photos</description>\r\n");
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
            while (reader.Read())
            {
                rssBuilder.Append("    <item>\r\n");
                rssBuilder.Append("      <title>");
                rssBuilder.Append(Utils.HtmlEncode(reader["title"].ToString().Trim()));
                rssBuilder.Append("</title>\r\n");
                rssBuilder.Append("      <description><![CDATA[<img src=\"");
                rssBuilder.Append(Utils.HtmlEncode(forumurl));
                rssBuilder.Append(reader["filename"].ToString().Trim());
                rssBuilder.Append("\"/>]]></description>\r\n");
                rssBuilder.Append("      <link>");
                rssBuilder.Append(Utils.HtmlEncode(forumurl));
                rssBuilder.Append("showphoto.aspx?photoid=");
                rssBuilder.Append(reader["photoid"].ToString());
                rssBuilder.Append("</link>\r\n");
                rssBuilder.Append("      <category>");
                rssBuilder.Append("</category>\r\n");
                rssBuilder.Append("      <author>");
                rssBuilder.Append(Utils.HtmlEncode(reader["username"].ToString().Trim()));
                rssBuilder.Append("</author>\r\n");
                rssBuilder.Append("      <pubDate>");
                rssBuilder.Append(Utils.HtmlEncode(Convert.ToDateTime(reader["postdate"]).ToString("r").Trim()));
                rssBuilder.Append("</pubDate>\r\n");
                rssBuilder.Append("    </item>\r\n");
            }
            reader.Close();
            rssBuilder.Append("  </channel>\r\n");
            rssBuilder.Append("</rss>");

            cache.AddObject("/PHOTO/RSS/Photo" + uid, rssBuilder.ToString(), ttl * 60);
            //cache.LoadDefaultCacheStrategy();

            return rssBuilder.ToString();
        }
    }
}
