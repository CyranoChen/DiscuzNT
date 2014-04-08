using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using System.Web.UI;
using Discuz.Config;

namespace Discuz.Web.UI
{
	/// <summary>
	/// RSS“≥√Ê¿‡
	/// </summary>
	public class SitemapPage : Page
	{
        public SitemapPage()
		{
			if (GeneralConfigs.GetConfig().Sitemapstatus == 1)
			{
				System.Web.HttpContext.Current.Response.ContentType = "application/xml";
			    System.Web.HttpContext.Current.Response.AppendHeader("Last-Modified", DateTime.Now.ToString("r"));
                System.Web.HttpContext.Current.Response.Write( Feeds.GetBaiduSitemap(GeneralConfigs.GetConfig().Sitemapttl));
				System.Web.HttpContext.Current.Response.End();
			}
			System.Web.HttpContext.Current.Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
            System.Web.HttpContext.Current.Response.Write("<Document>Sitemap is forbidden</Document>\r\n");
			System.Web.HttpContext.Current.Response.End();
		}
	}
}
