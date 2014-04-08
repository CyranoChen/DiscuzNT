using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Space.Utilities;

namespace Discuz.Space
{
	/// <summary>
	/// feedøÁ”Ú∑√Œ ¥˙¿Ì°£
	/// </summary>
	public class feedproxy : Page
	{
		public feedproxy()
		{
			if (ForumUtils.IsCrossSitePost(DNTRequest.GetUrlReferrer(), DNTRequest.GetHost()))
				return;
//			if (ForumUtils.IsCrossSitePost())
//				return;

			XmlDocument doc = new XmlDocument();
			string url = DNTRequest.GetQueryString("url");
			if (url == null || url == string.Empty)
				return;
			url = HttpUtility.UrlDecode(url);

			string enc = "utf-8";
			if (DNTRequest.GetQueryString("enc") != "")
			{
				enc = DNTRequest.GetQueryString("enc");
			}
			string err = "";

			HttpWebResponse response = Globals.GetPageResponse(url, out err);
			if (response == null)
			{
				HttpContext.Current.Response.Write(err);
				HttpContext.Current.Response.End();
				return;
			}
			Encoding encoding = Encoding.GetEncoding(enc);
			HttpContext.Current.Response.ContentType = "text/html"; //response.ContentType;
			HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
			Stream instream = response.GetResponseStream();
			doc.Load(instream);

			XmlNode rootnode = doc.SelectSingleNode("/rss/channel");

			StringBuilder json = new StringBuilder("{");
			foreach (XmlNode node in rootnode.ChildNodes)
			{
				switch (node.Name.ToLower())
				{
					case "image":
						break;
					case "item":
						break;
					default:
						json.AppendFormat("\"{0}\":\"{1}\",", node.Name.ToLower(), node.InnerText.Replace("\"", "\\\""));
						break;
				}
			}
			json.Remove(json.Length - 1, 1);
			json.Append(",\"items\":[");

			XmlNodeList items = doc.SelectNodes("/rss/channel/item");

			foreach (XmlNode node in items)
			{
				json.Append("{");
				foreach (XmlNode newnode in node)
				{
					json.AppendFormat(@"""{0}"":""{1}"",", newnode.Name.ToLower(), newnode.InnerText.Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", ""));

				}
				json.Remove(json.Length - 1, 1);
				json.Append("},");
			}
			json.Remove(json.Length - 1, 1);
			json.Append("]}");

			HttpContext.Current.Response.Clear();
			HttpContext.Current.Response.Write(json.ToString());
			HttpContext.Current.Response.End();


		}

	}
}