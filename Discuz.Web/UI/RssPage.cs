using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Plugin.Space;
using Discuz.Plugin.Album;

namespace Discuz.Web.UI
{
	/// <summary>
	/// RSS页面类
	/// </summary>
	public class RssPage : PageBase
	{
		public RssPage()
		{
			System.Web.HttpContext.Current.Response.ContentType = "application/xml";
			if (config.Rssstatus == 1)
			{
                if (DNTRequest.GetString("type") == "space" && config.Enablespace == 1)
                {
                    System.Web.HttpContext.Current.Response.Write(DNTRequest.GetInt("uid", -1) == -1 ? 
                        SpacePluginProvider.GetInstance().GetFeed(config.Rssttl) : 
                        SpacePluginProvider.GetInstance().GetFeed(config.Rssttl, DNTRequest.GetInt("uid", -1)));
                    System.Web.HttpContext.Current.Response.End();
                    return;
                }
                if (DNTRequest.GetString("type") == "photo" && config.Enablealbum == 1)
                {
                    System.Web.HttpContext.Current.Response.Write(DNTRequest.GetInt("uid", -1) == -1 ? 
                        AlbumPluginProvider.GetInstance().GetFeed(config.Rssttl) : 
                        AlbumPluginProvider.GetInstance().GetFeed(config.Rssttl, DNTRequest.GetInt("uid", -1)));
                    System.Web.HttpContext.Current.Response.End();
                    return;
                }
                //获得论坛最新的20个主题的Rss描述
				if (DNTRequest.GetInt("forumid", -1) == -1)
				{
					System.Web.HttpContext.Current.Response.Write(Feeds.GetRssXml(config.Rssttl));
					System.Web.HttpContext.Current.Response.End();
					return;
				}
				else
				{
                    //获得指定版块最新的20个主题的Rss描述
                    ForumInfo forum = Forums.GetForumInfo(DNTRequest.GetInt("forumid", -1));
                    if (forum != null && forum.Allowrss == 1)
                    {
                        System.Web.HttpContext.Current.Response.Write(Feeds.GetForumRssXml(config.Rssttl, DNTRequest.GetInt("forumid", -1)));
                        System.Web.HttpContext.Current.Response.End();
                        return;
                    }
				}
			}
			System.Web.HttpContext.Current.Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
			System.Web.HttpContext.Current.Response.Write("<Rss>Error</Rss>\r\n");
			System.Web.HttpContext.Current.Response.End();
		}
	}
}
